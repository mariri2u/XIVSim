using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class Require : AI
    {
        public override bool IsAction()
        {
            return Data.State[relation].IsValid();
        }
    }
}
