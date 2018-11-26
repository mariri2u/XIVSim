using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.actionai
{
    public class RefreshDoT : ActionAI
    {
        public override bool IsAction()
        {
            if (Action is DoT dot)
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
