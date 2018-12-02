using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class StackedBuff : NoAction
    {
        public override double Amp
        {
            get
            {
                if (base.Amp > eps) { return 1.0 + (base.Amp - 1.0) * Stack / Max; }
                else { return 1.0; }
            }
        }

        public override double Crit
        {
            get
            {
                if (base.Crit > eps) { return 1.0 + (base.Crit - 1.0) * Stack / Max; }
                else { return 1.0; }
            }
        }

        public override double Direc
        {
            get
            {
                if (base.Direc > eps) { return 1.0 + (base.Direc - 1.0) * Stack / Max; }
                else { return 1.0; }
            }
        }
    }
}
