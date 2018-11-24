﻿using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public abstract class DoT : Action, IDoT
    {
        protected int slip;
        protected int duration;

        public DoT(string name, int power, double cast, double recast, int slip, int duration) : base(name, power, cast, recast, 0.1)
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

        public double Remain { get; set; }

        public int Slip
        {
            get { return slip; }
        }

        public int Duration
        {
            get { return duration; }
        }
    }
}