using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

namespace xivsim.action
{
    public class GCDAction : Action, IGCD
    {
        public GCDAction() : base() { }

        public void ApplyGlobal(double gcd)
        {
            Recast = gcd;
            Cast *= gcd;
            Motion = 0.1;
        }

        public GCDAction(string name, int power, double cast, double recast)
            : base(name, power, cast, recast, 0.1)
        { }

        public GCDAction(string name, int power, double cast, double recast, int slip, int duration)
            : base(name, power, cast, recast, 0.1, slip, duration)
        { }

        public override bool CanAction()
        {
            if (base.CanAction() && Data.Recast["global"] < eps) { return true; }
            else { return false; }
        }

        public override void CalcAction()
        {
            Data.Recast["global"] = Data.GetHaste() * (Recast - Cast);
            if (Combo) { Data.Before = this; }
        }
    }
}
