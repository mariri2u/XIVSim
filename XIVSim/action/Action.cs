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

        // 固定値のパラメータ
        public string Name { get; protected set; }
        public int Power { get; protected set; }
        public double Cast { get; protected set; }
        public double Recast { get; protected set; }
        public double Motion { get; protected set; }
        public int Slip { get; protected set; }
        public int Duration { get; protected set; }
        public int Require { get; protected set; }
        public int Increase { get; protected set; }
        public int Max { get; protected set; }
        public string Relation { get; protected set; }
        public string RelationA { get; protected set; }
        public string RelationB { get; protected set; }
        public string RelationC { get; protected set; }
        public string Before { get; protected set; }
        public bool Combo { get; protected set; }

        public virtual double Amp { get; protected set; }
        public virtual double Haste { get; protected set; }
        public virtual double Crit { get; protected set; }
        public virtual double Direc { get; protected set; }

        // 状態量
        public double Remain { get; set; }
        public double SlipAmp { get; set; }

        protected int stack;
        public virtual int Stack
        {
            get { return stack; }
            set
            {
                if (value > Max) { stack = Max; }
                else { stack = value; }
            }
        }

        public BattleData Data { get; set; }

        public Action()
        {
            Name = null;
            Power = 0;
            Cast = 0;
            Recast = 0.0;
            Motion = 0.0;
            Slip = 0;
            Duration = 0;
            Amp = 0.0;
            Relation = null;
            Require = 0;
            Increase = 0;
            Before = null;
            Max = 0;
            Haste = 0.0;
            Combo = true;

            Remain = 0.0;
            Stack = 0;
            SlipAmp = 1.0;
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
            else if( Require == 0) { return true; }
            else if (Data.State.ContainsKey(Relation) && Data.State[Relation].Stack >= Require) { return true; }
            else { return false; }
        }

        // 直前に実行したアクションの判定
        protected virtual bool InBefore()
        {
            if(Before == null) { return true; }
            else if(Data.Before != null && Data.Before.Name == Before) { return true; }
            else { return false; }
        }

        // 有効かどうか
        public virtual bool IsValid()
        {
            bool result = true; ;

            // 有効時間のあるアクションの場合
            if (Duration > eps)
            {
                // 効果時間中の場合
                if (Remain > eps)
                {
                    result &= true;
                }
                else
                {
                    result &= false;
                }
            }

            // スタック数のあるアクションの場合
            if (Max > 0)
            {
                // 効果時間中の場合
                if (Stack > 0)
                {
                    result &= true;
                }
                else
                {
                    result &= false;
                }
            }

            return result;
        }

        // Actionを実行開始したとき
        public void StartAction()
        {
            // 詠唱がある場合
            if (Cast > eps)
            {
                Data.Recast["cast"] = Data.GetHaste() * Cast;
                Data.Casting = this;
            }
            // 詠唱が無い場合は、Action使用時点で完了したものとする
            else
            {
                DoneAction();
            }
        }

        // Actionが完了したとき
        public void DoneAction()
        {
            Data.Plan = this;

            CalcAction();
            Data.Casting = null;

            // 着弾ダメージがある場合
            if (Power > 0)
            {
                Data.Damage["action"] = Data.GetAmp() * Data.Table.Calc(Power);
            }

            // DoTダメージがある場合
            if (Slip > 0)
            {
                SlipAmp = Data.GetAmp();
            }

            // 設定すべき効果がある場合
            if (Duration > 0)
            {
                Remain = Duration;
            }

            // スタックする場合
            if (Max > 0)
            {
                Stack = Max;
            }

            // 関連アクションがある場合
            if(Relation != null)
            {
                CalcRelation();
            }

            // モーションリキャストが現在のActionのモーション値を上回っている場合のみ設定(AA対策)
            if (Data.Recast["motion"] < Motion)
            {
                Data.Recast["motion"] = Motion;
            }

            Data.History.Add(this);
            Data.Plan = null;
        }

        protected virtual void CalcRelation()
        {
            // スタック数を増やす場合
            if (Increase > 0)
            {
                Data.State[Relation].Stack += Increase;
            }

            // スタック数を消費するアクションの場合
            if (Require > 0)
            {
                Data.State[Relation].Stack -= Require;
            }
        }

        // リキャストの設定はこの中で実施
        public abstract void CalcAction();

        // フレーム毎に実行する処理
        public virtual void Tick()
        {
            if (this.Slip > 0 && Data.Recast["dot"] < eps)
            {
                Data.Damage["dot"] += this.SlipAmp * Data.Table.Calc(this.Slip);
            }
        }

        // Buffの効果が切れた場合
        public virtual void DoneStep() { }

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
            Power = arg.Power;
            Name = arg.Name;
            Cast = arg.Cast;
            Recast = arg.Recast;
            Motion = arg.Motion;
            Slip = arg.Slip;
            Duration = arg.Duration;
            Amp = arg.Amp;
            Crit = arg.Crit;
            Direc = arg.Direc;
            Relation = arg.Relation;
            Require = arg.Require;
            Increase = arg.Increase;
            Before = arg.Before;
            Max = arg.Max;
            Haste = arg.Haste;
            RelationA = arg.RelationA;
            RelationB = arg.RelationB;
            RelationC = arg.RelationC;
            Combo = arg.Combo;
        }

        public void SaveConfig(ActionElement arg)
        {
            arg.Power = Power;
            arg.Name = Name;
            arg.Cast = Cast;
            arg.Recast = Recast;
            arg.Motion = Motion;
            arg.Slip = Slip;
            arg.Duration = Duration;
            arg.Amp = Amp;
            arg.Crit = Crit;
            arg.Direc = Direc;
            arg.Relation = Relation;
            arg.Require = Require;
            arg.Increase = Increase;
            arg.Before = Before;
            arg.Max = Max;
            arg.Haste = Haste;
            arg.RelationA = RelationA;
            arg.RelationB = RelationB;
            arg.RelationC = RelationC;
        }
    }
}
