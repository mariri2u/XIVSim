using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class LessRecast : AI
    {
        public override bool IsAction()
        {
            return Data.Recast[relation] <= recast;
        }
    }
}
