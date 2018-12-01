using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class DoTReady : AI
    {
        public override bool IsAction()
        {
            bool result = false;

            foreach (Action act in Data.Action)
            {
                if (act.Slip > 0)
                {
                    result |= act.CanAction() && act.IsActionByAI(true);
                }
            }

            return result;
        }
    }
}
