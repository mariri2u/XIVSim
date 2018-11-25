using System;
using System.Collections.Generic;
using System.Text;
using xivsim.actionai;

namespace xivsim.action
{
    public abstract class DoT : Action, IDoT
    {
        protected int slip;
        protected int duration;

        public int Slip { get { return slip; } }
        public int Duration { get { return duration; } }

        public double Remain { get; set; }

        public DoT(string name, int power, double cast, double recast, int slip, int duration, ActionAI ai)
            : base(name, power, cast, recast, 0.1, ai)
        {
            this.slip = slip;
            this.duration = duration;
        }

        public void Tick()
        {
            if (this.Remain > 0.0)
            {
                Data.Damage["dot"] += Data.Table.Calc(this.Slip);
            }
        }

    }
}
