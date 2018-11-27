using System;
using System.Collections.Generic;
using System.Text;
using xivsim.jobai;
using xivsim.actionai;

namespace xivsim.action
{
    class GCDAction : Action, IGCD
    {
        public GCDAction(string name, int power, double cast, double recast)
            : base(name, power, cast, recast, 0.1)
        { }

        public override bool CanAction()
        {
            if (base.CanAction() && Data.Recast["global"] < eps) { return true; }
            else { return false; }
        }

        public override IAction CalcAction()
        {
            Data.Damage["action"] = Data.Table.Calc(this.Power);
            Data.Recast["cast"] = Cast;
            Data.Recast["motion"] = Motion;
            Data.Recast["global"] = Recast;
            Data.History.AddFirst(this);

            return this;
        }
    }
}
