using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;
using xivsim.config;

namespace xivsim.action
{
    public abstract class Action
    {
        protected const double eps = 1.0e-7;

        protected string name;
        protected int power;
        protected double cast;
        protected double recast;
        protected double motion;
        protected int slip;
        protected int duration;
        protected double amplifier;
        protected string relation;
        protected string relationA;
        protected string relationB;
        protected string relationC;
        protected int require;
        protected int increase;
        protected string before;
        protected int stack;
        protected int max;
        protected double haste;
        protected bool combo;

        public double slipAmp;

        public string Name { get { return name; } }
        public int Power { get { return power; } }
        public double Cast { get { return cast; } }
        public double Recast { get { return recast; } }
        public double Motion { get { return motion; } }
        public int Slip { get { return slip; } }
        public int Duration { get { return duration; } }
        public virtual double Amplifier { get { return amplifier; } }
        public string Relation { get { return relation; } }
        public int Require { get { return require; } }
        public int Increase { get { return increase; } }
        public int Max { get { return max; } }
        public virtual double Haste { get { return haste; } }
        public string RelationA { get { return relationA; } }
        public string RelationB { get { return relationB; } }
        public string RelationC { get { return relationC; } }

        public double Remain { get; set; }
        public virtual int Stack {
            get { return stack; }
            set {
                if (value > max) { stack = max; }
                else { stack = value; }
            }
        }

        public BattleData Data { get; set; }
        public List<AI> AI { get; }

        public Action()
        {
            this.name = null;
            this.power = 0;
            this.cast = 0;
            this.recast = 0.0;
            this.motion = 0.0;
            this.slip = 0;
            this.duration = 0;
            this.amplifier = 0.0;
            this.relation = null;
            this.require = 0;
            this.increase = 0;
            this.before = null;
            this.max = 0;
            this.haste = 0.0;
            this.slipAmp = 1.0;

            Remain = 0.0;
            Stack = 0;

            this.AI = new List<AI>();
        }

        public Action(string name, int power, double cast, double recast, double motion) : this()
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
        }

        public Action(string name, int power, double cast, double recast, double motion, int slip, int duration) : this()
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
            this.slip = slip;
            this.duration = duration;
        }

        public Action ResistAI(List<AI> ais)
        {
            foreach (AI ai in ais)
            {
                ai.Action = this;
                AI.Add(ai);
            }
            return this;
        }

        public virtual bool CanAction()
        {
            if (InMotion() && InRelation() && InBefore()) { return true; }
            else { return false; }
        }

        // モーションと詠唱の判定
        protected bool InMotion()
        {
            if (Data.Recast["cast"] < eps && Data.Recast["motion"] < eps) { return true; }
            else { return false; }
        }

        // 依存アクションのスタック数が必要数あるかどうか
        protected virtual bool InRelation()
        {
            if (Relation == null) { return true; }
            else if( require == 0) { return true; }
            else if (Data.State.ContainsKey(Relation) && Data.State[Relation].Stack >= Require) { return true; }
            else { return false; }
        }

        // 直前に実行したアクションの判定
        protected virtual bool InBefore()
        {
            if(before == null) { return true; }
            else if(Data.Before != null && Data.Before.Name == before) { return true; }
            else { return false; }
        }

        // どのAIも無視せず実行判定を行う
        public bool IsActionByAI() { return IsActionByAI(false); }

        public bool IsActionByAI(bool ignore)
        {
            // グループ毎に実行判定を行う
            Dictionary<string, bool> resultTable = new Dictionary<string, bool>();
            foreach (AI ai in AI)
            {
                // 各グループの初期値はtrue
                if(!resultTable.ContainsKey(ai.Group))
                {
                    resultTable[ai.Group] = true;
                }

                // 引数がtrueの場合はNoInterruptを無視する
                if (!ignore || !(ai is NoInterrupt))
                {
                    resultTable[ai.Group] &= ai.IsAction();
                }
            }

            // デフォルトグループは全てのグループの結果に影響する
            bool all = true;
            if (resultTable.ContainsKey("default"))
            {
                all = resultTable["default"];
                resultTable.Remove("default");
            }

            // いずれかのグループがtrueなら実行する
            bool result = false;
            if (resultTable.Count > 0)
            {
                foreach (bool res in resultTable.Values)
                {
                    result |= res & all;
                }
            }
            else
            {
                result = all;
            }

            return result;
        }

        // Actionを実行したとき
        public void UseAction()
        {
            // 詠唱がある場合
            if (Cast > eps)
            {
                Data.Recast["cast"] = Data.GetHaste() * Cast;
                Data.Casting = this;
            }
            // 詠唱が無い場合は、Action使用時点で詠唱完了したものとする
            else
            {
                DoneCast();
            }
        }

        // Actionの詠唱が完了したとき
        public void DoneCast()
        {
            Data.Plan = this;

            CalcAction();
            Data.Casting = null;

            // 着弾ダメージがある場合
            if (Power > 0)
            {
                Data.Damage["action"] = Data.GetAmplifier() * Data.Table.Calc(Power);
            }

            // DoTダメージがある場合
            if (Slip > 0)
            {
                slipAmp = Data.GetAmplifier();
            }

            // 設定すべき効果がある場合
            if (Duration > 0)
            {
                Remain = Duration;
            }

            // 関連アクションがある場合
            if(relation != null)
            {
                CalcRelation();
            }

            Data.Recast["motion"] = Motion;
            Data.History.Add(this);
            Data.Plan = null;
        }

        protected virtual void CalcRelation()
        {
            // スタック数を増やす場合
            if (increase > 0)
            {
                Data.State[relation].Stack += increase;
            }

            // スタック数を消費するアクションの場合
            if (require > 0)
            {
                Data.State[Relation].Stack -= require;
            }
        }

        // リキャストの設定はこの中で実施
        public abstract void CalcAction();

        // フレーム毎に実行する処理
        public virtual void Tick()
        {
            if (this.Slip > 0 && Data.Recast["dot"] < eps)
            {
                Data.Damage["dot"] += this.slipAmp * Data.Table.Calc(this.Slip);
            }
        }

        // Buffの効果が切れた場合
        public virtual void ExpireBuff() { }

        public static Dictionary<string, Action> ListToMap(List<Action> list)
        {
            Dictionary<string, Action> map = new Dictionary<string, Action>();

            foreach (Action act in list)
            {
                map[act.Name] = act;
            }

            return map;
        }

        public void LoadConfig(ActionElement arg)
        {
            this.power = arg.Power;
            this.name = arg.Name;
            this.cast = arg.Cast;
            this.recast = arg.Recast;
            this.motion = arg.Motion;
            this.slip = arg.Slip;
            this.duration = arg.Duration;
            this.amplifier = arg.Amplifier;
            this.relation = arg.Relation;
            this.require = arg.Require;
            this.increase = arg.Increase;
            this.before = arg.Before;
            this.max = arg.Max;
            this.haste = arg.Haste;
            this.relationA = arg.RelationA;
            this.relationB = arg.RelationB;
            this.relationC = arg.RelationC;
            this.combo = arg.Combo;
        }

        public void SaveConfig(ActionElement arg)
        {
            arg.Power = this.power;
            arg.Name = this.name;
            arg.Cast = this.cast;
            arg.Recast = this.recast;
            arg.Motion = this.motion;
            arg.Slip = this.slip;
            arg.Duration = this.duration;
            arg.Amplifier = this.amplifier;
            arg.Relation = this.relation;
            arg.Require = this.require;
            arg.Increase = this.increase;
            arg.Before = this.before;
            arg.Max = this.max;
            arg.Haste = this.haste;
            arg.RelationA = this.relationA;
            arg.RelationB = this.relationB;
            arg.RelationC = this.relationC;
        }
    }
}
