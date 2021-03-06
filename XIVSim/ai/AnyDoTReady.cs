﻿using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class AnyDoTReady : AI
    {
        public override bool IsAction()
        {
            bool result = false;

            foreach (Action act in Data.Action.Values)
            {
                if (act.Slip > 0)
                {
                    result |= act.CanAction() && ActionAI.IsActionByAnyAI();
                }
            }

            return result;
        }
    }
}
