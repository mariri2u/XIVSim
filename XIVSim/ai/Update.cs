using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class Update : AI
    {
        public override bool IsAction()
        {
            return (Data.State[Action.Name].Remain <= threshold_f);
        }
    }
}
