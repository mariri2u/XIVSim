using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class AmpGCD : Ability
    {
        bool used = false;

        public override double Amp
        {
            get
            {
                if (Stack > 0 && Data.Plan is IGCD)
                {
                    used = true;
                    return base.Amp;
                }
                else
                {
                    return 1.0;
                }
            }
        }

        public override void DoneStep()
        {
            base.DoneStep();
            if (used)
            {
                Stack--;
                if (Stack <= 0)
                {
                    Remain = 0.0;
                }
            }
            used = false;
        }
    }
}
