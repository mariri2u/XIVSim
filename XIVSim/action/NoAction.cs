using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class NoAction : Action
    {
        public NoAction() : base() { }

        public override bool CanAction()
        {
            return false;
        }

        public override void CalcAction()
        {
        }
    }
}
