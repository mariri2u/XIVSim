using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class LessRemain : AI
    {
        public override bool IsAction()
        {
            return Data.State[relation].Remain <= remain;
        }
    }
}
