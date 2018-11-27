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
        public List<ActionAI> AI { get; }

        public Action(string name, int power, double cast, double recast, double motion)
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
            this.AI = new List<ActionAI>();
        }

        public IAction ResistAI(ActionAI ai)
        {
            ai.Action = this;
            AI.Add(ai);
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
            foreach(ActionAI ai in AI)
            {
                result &= ai.IsAction();
            }
            return result;
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
