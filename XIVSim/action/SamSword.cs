using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class SamSword : GCDAction
    {
        protected override bool InRelation()
        {
            return Data.State[Relation].Stack == Require;
        }
    }
}
