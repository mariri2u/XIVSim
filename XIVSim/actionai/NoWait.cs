using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.actionai
{
    public class NoWait : ActionAI
    {
        public override bool IsAction()
        {
            return true;
        }
    }
}
