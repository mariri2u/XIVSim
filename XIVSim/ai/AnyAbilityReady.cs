using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class AnyAbilityReady : AI
    {
        public override bool IsAction()
        {
            bool result = false;

            foreach (ActionAI aa in Data.ActionAI)
            {
                if (Data.Action[aa.Name] is IAbility)
                {
                    result |= Data.Action[relation].CanAction() && aa.IsActionByAnyAI();
                }
            }

            return result;
        }
    }
}
