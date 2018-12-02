using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

namespace xivsim.action
{
    public class GCDAction : Action, IGCD
    {
        public void ApplyGlobal(double gcd)
        {
            Recast = gcd;
            Cast *= gcd;
            Motion = 0.1;
        }

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
