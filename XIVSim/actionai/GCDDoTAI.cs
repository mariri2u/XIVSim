using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.actionai
{
    public class GCDDoTAI : ActionAI
    {
        public override bool IsAction()
        {
            if (Action is GCDDoT dot)
            {
                return (dot.Remain < dot.Cast + dot.Recast);
            }
            else
            {
                return false;
            }
        }
    }
}
