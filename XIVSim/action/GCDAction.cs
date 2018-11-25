using System;
using System.Collections.Generic;
using System.Text;
using xivsim.jobai;
using xivsim.actionai;

namespace xivsim.action
{
    class GCDAction : Action
    {
        public GCDAction(string name, int power, double cast, double recast)
            : base(name, power, cast, recast, 0.1, new GCDActionAI())
        { }
        
        public override IAction Calc()
        {
            Data.Damage["action"] = Data.Table.Calc(this.Power);
            Data.Recast["cast"] = this.Cast;
            Data.Recast["motion"] = this.Motion;
            Data.Recast["global"] = this.Recast;
            Data.History.AddFirst(this);

            return this;
        }
    }
}
