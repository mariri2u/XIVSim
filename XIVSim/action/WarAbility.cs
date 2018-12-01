using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class WarAbility : Ability
    {
        protected override bool InRelation()
        {
            if (Data.State[RelationA].Remain > eps) { return true; }
            else { return base.InRelation(); }
        }

        protected override void CalcRelation()
        {
            if (Data.State[RelationA].Remain < 0.0)
            {
                base.CalcRelation();
            }
        }

    }
}
