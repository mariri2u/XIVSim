using System;
using System.Collections.Generic;
using System.Text;
using xivsim.jobai;
using xivsim.actionai;

namespace xivsim.action
{
    public abstract class Action : IAction
    {
        protected const double eps = 1.0e-7;

        protected int power;
        protected string name;
        protected double cast;
        protected double recast;
        protected double motion;

        public string Name { get { return name; } }
        public int Power { get { return power; } }
        public double Cast { get { return cast; } }
        public double Recast { get { return recast; } }
        public double Motion { get { return motion; } }

        public BattleData Data { get; set; }
        public ActionAI AI { get; set; }

        public Action(string name, int power, double cast, double recast, double motion, ActionAI ai)
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
            ai.Action = this;
            this.AI = ai;
        }

        public virtual bool CanAction()
        {
            if (Data.Recast["cast"] < eps && Data.Recast["motion"] < eps) { return true; }
            else { return false; } 
        }

        public abstract IAction CalcAction();

        public static Dictionary<string, IAction> ListToMap(List<IAction> list)
        {
            Dictionary<string, IAction> map = new Dictionary<string, IAction>();

            foreach (IAction act in list)
            {
                map[act.Name] = act;
            }

            return map;
        }
    }
}
