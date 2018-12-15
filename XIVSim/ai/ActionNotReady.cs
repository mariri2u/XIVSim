using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class ActionNotReady : AI
    {
        public override bool IsAction()
        {
            return !(Data.Action[relation].CanAction() || ActionAI.IsActionByAnyAI());
        }
    }
}
