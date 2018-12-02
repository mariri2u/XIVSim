using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class MoreRecast : AI
    {
        public override bool IsAction()
        {
            return Data.Recast[relation] >= threshold_f;
        }
    }
}
