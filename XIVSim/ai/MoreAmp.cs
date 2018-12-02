using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class MoreAmp : AI
    {
        public override bool IsAction()
        {
            return Data.GetAmp() >= threshold_f;
        }
    }
}
