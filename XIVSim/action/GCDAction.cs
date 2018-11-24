﻿using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

namespace xivsim.action
{
    class GCDAction : Action
    {
        public GCDAction(string name, int power, double cast, double recast) : base(name, power, cast, recast, 0.1)
        {
        }

        public override bool IsAction()
        {
            return true;
        }

        public override void Calc()
        {
            Data.Damage["action"] = Data.Table.Calc(this.Power);
            Data.Recast["cast"] = this.Cast;
            Data.Recast["motion"] = this.Motion;
            Data.Recast["global"] = this.Recast;
            Data.History.AddFirst(this);
        }
    }
}