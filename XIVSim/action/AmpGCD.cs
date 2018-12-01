using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class AmpGCD : Ability
    {
        bool used = false;

        public override void Tick()
        {
            base.Tick();
            if(used)
            {
                stack--;
                if(stack <= 0)
                {
                    Remain = 0.0;
                }
            }
            used = false;
        }

        public override double Amplifier
        {
            get
            {
                if (Stack > 0 && Data.Plan is IGCD)
                {
                    used = true;
                    return amplifier;
                }
                else
                {
                    return 1.0;
                }
            }
        }

        public override void CalcAction()
        {
            base.CalcAction();
            Stack = Max;
        }
    }
}
