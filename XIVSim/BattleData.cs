using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim
{
    public class BattleData
    {
        private const double eps = 1.0e-7;

        public BattleData()
        {
            Time = 0.0;

            Recast = new Dictionary<string, double>();
            Damage = new Dictionary<string, double>();
            State = new Dictionary<string, Action>();

            Casting = null;
            Before = null;
            Plan = null;

            History = new List<Action>();
            Reserve = new List<Action>();
        }
        
        public void Clear()
        {
            Time = 0.0;

            Recast.Clear();
            Damage.Clear();
            State.Clear();
            Casting = null;
            Before = null;
            Plan = null;
            History.Clear();
            Reserve.Clear();
        }

        public double GetAmp()
        {
            return GetPureAmp() * GetCritAmp() * GetDirecAmp();
        }

        public double GetPureAmp()
        {
            double amp = 1.0;
            foreach (Action act in State.Values)
            {
                if (act.IsValid() && act.Amp > eps)
                {
                    amp *= act.Amp;
                }
            }
            return amp;
        }

        public double GetHaste()
        {
            double haste = 1.0;
            foreach(Action act in State.Values)
            {
                if(act.IsValid() && act.Haste > eps)
                {
                    haste *= act.Haste;
                }
            }
            return haste;
        }

        public double GetCritProb()
        {
            double prob = Table.BaseCritProb;
            foreach (Action act in State.Values)
            {
                if (act.IsValid() && act.Crit > eps)
                {
                    prob *= act.Crit;
                    if (act.Crit >= 10) { return 1.0; }
                    if (prob >= 1.0) { return 1.0; }
                }
            }
            return prob;
        }

        public double GetCritAmp()
        {
            return DamageTable.CalcExpect(GetCritProb(), Table.BaseCritAmp);
        }

        public double GetDirecProb()
        {
            double prob = Table.BaseDirecProb;
            foreach (Action act in State.Values)
            {
                if (act.IsValid() && act.Direc > eps)
                {
                    prob *= act.Direc;
                    if (act.Direc >= 10) { return 1.0; }
                    if (prob >= 1.0) { return 1.0; }
                }
            }
            return prob;
        }

        public double GetDirecAmp()
        {
            return DamageTable.CalcExpect(GetDirecProb(), Table.BaseDirecAmp);
        }

        public double Time { get; set; }
        public Dictionary<string, double> Recast { get; }
        public Dictionary<string, double> Damage { get; }
        public Dictionary<string, Action> State { get; }
        public Action Casting { get; set; }
        public Action Before { get; set; }
        public Action Plan { get; set; }
        public List<Action> History { get; }
        public List<Action> Reserve { get; }

        public List<Action> Action { get; set; }
        public DamageTable Table { get; set; }
    }
}
