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
            Recast.Clear();
            Damage.Clear();
            State.Clear();
            Casting = null;
            Before = null;
            Plan = null;
            History.Clear();
            Reserve.Clear();
        }

        public double GetAmplifier()
        {
            double amp = 1.0;
            foreach (Action act in State.Values)
            {
                if ((act.Duration < eps || act.Remain > eps) && act.Amplifier > eps)
                {
                    amp *= act.Amplifier;
                }
            }
            return amp;
        }

        public double GetHaste()
        {
            double haste = 1.0;
            foreach(Action act in State.Values)
            {
                if((act.Duration<eps || act.Remain>eps) && act.Haste > eps)
                {
                    haste *= act.Haste;
                }
            }
            return haste;
        }

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
