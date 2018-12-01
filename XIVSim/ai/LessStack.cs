using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class LessStack : AI
    {
        public override bool IsAction()
        {
            return Data.State[relation].Stack <= stack;
        }
    }
}
