using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.ai;

namespace xivsim.action
{
    abstract class Action
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

        abstract public bool IsAction();
        abstract public void Calc();

        public BattleData Data
        {
            get; set;
        }

        public AICore AI
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

        public static Dictionary<string, Action> ListToMap(List<Action> list)
        {
            Dictionary<string, Action> map = new Dictionary<string, Action>();

            foreach( Action act in list )
            {
                map[act.Name] = act;
            }

            return map;
        }
    }
}
