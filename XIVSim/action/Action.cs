using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

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

        public Action(string name, int power, double cast, double recast, double motion)
        {
            this.name = name;
            this.power = power;
            this.cast = cast;
            this.recast = recast;
            this.motion = motion;
        }

        public abstract bool IsAction();
        public abstract void Calc();

        public BattleData Data
        {
            get; set;
        }

        public AI AI
        {
            get; set;
        }

        public string Name
        {
            get { return name; }
        }

        public int Power
        {
            get { return power; }
        }

        public double Cast
        {
            get { return cast; }
        }

        public double Recast
        {
            get { return recast; }
        }

        public double Motion
        {
            get { return motion; }
        }

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
