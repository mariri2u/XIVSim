using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.actionai
{
    public class NoInterrupt : ActionAI
    {
        public override bool IsAction()
        {
            return (Action.Motion < Data.Recast["global"]);
        }
    }
}
