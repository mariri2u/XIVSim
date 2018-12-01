using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class StackedBuff : Action
    {
        public override double Amplifier
        {
            get {
                return 1.0 + (amplifier - 1.0) * Stack / max;
            }
        }

        public override bool CanAction()
        {
            return false;
        }

        public override void CalcAction()
        {
        }
    }
}
