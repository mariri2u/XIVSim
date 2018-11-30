using System;
using System.Collections.Generic;
using System.Text;
using xivsim.actionai;
using xivsim.config;

namespace xivsim.action
{
    public abstract class Action
    {
        protected const double eps = 1.0e-7;

        public string name;
        public int power;
        public double cast;
        public double recast;
        public double motion;
        public int slip;
        public int duration;
        public double amplifier;
        public string relation;
        public int require;

        public string Name { get { return name; } }
        public int Power { get { return power; } }
        public double Cast { get { return cast; } }
        public double Recast { get { return recast; } }
        public double Motion { get { return motion; } }
        public int Slip { get { return slip; } }
        public int Duration { get { return duration; } }
        public double Amplifier { get { return amplifier; } }
        public string Relation { get { return relation; } }
        public int Require { get { return require; } }

        public double Remain { get; set; }

        public BattleData Data { get; set; }
        public List<ActionAI> AI { get; }

        public Action()
        {
            this.name = "";
            this.power = 0;
            this.cast = 0;
            this.recast = 0.0;
            this.motion = 0.0;
            this.slip = 0;
            this.duration = 0;
            this.amplifier = 0.0;
            this.relation = "";
            this.require = 0;
            this.AI = new List<ActionAI>();
        }

        public Action(string name, int power, double cast, double recast, double motion)
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
            this.slip = 0;
            this.duration = 0;
            this.amplifier = 0.0;
            this.relation = "";
            this.require = 0;
            this.AI = new List<ActionAI>();
        }

        public Action(string name, int power, double cast, double recast, double motion, int slip, int duration)
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
            this.slip = slip;
            this.duration = duration;
            this.amplifier = 0.0;
            this.AI = new List<ActionAI>();
        }

        public Action ResistAI(List<ActionAI> ais)
        {
            foreach (ActionAI ai in ais)
            {
                ai.Action = this;
                AI.Add(ai);
            }
            return this;
        }

        public virtual bool CanAction()
        {
            if (Data.Recast["cast"] < eps && Data.Recast["motion"] < eps) { return true; }
            else { return false; }
        }

        public bool IsActionByAI()
        {
            bool result = true;
            foreach (ActionAI ai in AI)
            {
                result &= ai.IsAction();
            }
            return result;
        }

        // Actionを実行したとき
        public Action UseAction()
        {
            // 詠唱がある場合
            if (Cast > eps)
            {
                Data.Recast["cast"] = Cast;
                Data.Casting = this;
                return this;
            }
            // 詠唱が無い場合は、Action使用時点で詠唱完了したものとする
            else
            {
                return DoneCast();
            }
        }

        // Actionの詠唱が完了したとき
        public Action DoneCast()
        {
            Action act = CalcAction();
            Data.Casting = null;

            // 着弾ダメージがある場合
            if (act.Power > 0)
            {
                Data.Damage["action"] = Data.Table.Calc(Power);
            }

            // DoTダメージがある場合
            if (act.Slip > 0)
            {
                Data.DoTs[act.Name] = this;
                act.Remain = act.Duration;
            }

            Data.Recast["motion"] = act.Motion;
            Data.History.Add(act);
            return act;
        }

        // リキャストの設定はこの中で実施
        public abstract Action CalcAction();

        // DoTダメージが発生する場合
        public Action Tick()
        {
            if (this.Remain > eps && this.Slip > 0)
            {
                Data.Damage["dot"] += Data.Table.Calc(this.Slip);
            }
            return this;
        }

        public static Dictionary<string, Action> ListToMap(List<Action> list)
        {
            Dictionary<string, Action> map = new Dictionary<string, Action>();

            foreach (Action act in list)
            {
                map[act.Name] = act;
            }

            return map;
        }

        public virtual void LoadConfig(ActionElement arg)
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
        }

        public virtual void SaveConfig(ActionElement arg)
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
        }
    }
}
