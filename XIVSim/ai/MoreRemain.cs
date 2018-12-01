using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class MoreRemain : AI
    {
        public override bool IsAction()
        {
            return Data.State[relation].Remain >= remain;
        }
    }
}
