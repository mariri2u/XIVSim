using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

namespace xivsim.action
{
    public class AutoAttack : Action
    {
        public AutoAttack() : base() { }

        public void ApplySpeed(double gcd)
        {
            Recast = gcd;
            Cast = 0.0;
            Motion = 0.0;

            AI.Clear();
            AI.Add(new NoWait());
        }

        public override bool CanAction()
        {
            if (Data.Recast["cast"] < eps && Data.Recast["aa"] < eps) { return true; }
            else { return false; }
        }

        public override void CalcAction()
        {
            Data.Recast["aa"] = Recast;
        }
    }
}
