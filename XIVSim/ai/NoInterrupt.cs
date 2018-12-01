using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class NoInterrupt : AI
    {
        public override bool IsAction()
        {
            return (Action.Motion < Data.Recast["global"]);
        }
    }
}
