using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class RelationNotReady : AI
    {
        public override bool IsAction()
        {
            foreach (Action act in Data.Action)
            {
                if (act.Name == relation)
                {
                    return !(act.CanAction() || act.IsActionByAI(true));
                }
            }
            return false;
        }
    }
}
