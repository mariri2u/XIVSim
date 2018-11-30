using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.actionai
{
    public class Update : ActionAI
    {
        public override bool IsAction()
        {
            return (Action.Remain < this.remain);
        }
    }
}
