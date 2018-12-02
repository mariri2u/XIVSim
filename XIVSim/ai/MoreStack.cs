using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class MoreStack : AI
    {
        public override bool IsAction()
        {
            return Data.State[relation].Stack >= threshold_i;
        }
    }
}
