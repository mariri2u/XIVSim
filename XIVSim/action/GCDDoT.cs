using System;
using System.Collections.Generic;
using System.Text;
using xivsim.actionai;

namespace xivsim.action
{
    class GCDDoT : DoT, IGCD
    {
        public GCDDoT(string name, int power, double cast, double recast, int slip, int duration) :
            base(name, power, cast, recast, slip, duration)
        { }

        public override bool CanAction()
        {
            if (base.CanAction() && Data.Recast["global"] < eps) { return true; }
            else { return false; }
        }

        public override IAction CalcAction()
        {
            Data.DoTs[Name] = this;
            Remain = Duration;
            Data.Recast["global"] = Recast;

            return this;
        }
    }
}
