using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class RequireAmplifier : AI
    {
        public override bool IsAction()
        {
            return Data.GetAmplifier() >= amplifier;
        }
    }
}
