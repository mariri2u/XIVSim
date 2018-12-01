using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class AbilityReady : AI
    {
        public override bool IsAction()
        {
            bool result = false;

            foreach (Action act in Data.Action)
            {
                if (act is IAbility)
                {
                    result |= act.CanAction() && act.IsActionByAI(true);
                }
            }

            return result;
        }
    }
}
