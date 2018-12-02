using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class After : AI
    {
        public override bool IsAction()
        {
            return Data.Time >= threshold_i;
        }
    }
}
