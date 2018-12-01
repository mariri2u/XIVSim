using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    class OverrideStack : Ability
    {
        private int cache;

        public override void CalcAction()
        {
            Data.Recast[Name] = Recast;
            cache = Data.State[relation].Stack;
            Data.State[Relation].Stack = Data.State[Relation].Max;
        }

        public override void Tick()
        {
            base.Tick();
            Data.State[Relation].Stack = Data.State[Relation].Max;
        }

        public override void ExpireBuff()
        {
            Data.State[Relation].Stack = cache;
        }
    }
}
