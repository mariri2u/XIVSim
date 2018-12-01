using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class Before : AI
    {
        public override bool IsAction()
        {
            return Data.Before.Name == relation;
        }
    }
}
