using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.actionai
{
    public class AlwaysAction : ActionAI
    {
        public override bool IsAction()
        {
            return true;
        }
    }
}
