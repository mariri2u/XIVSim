using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class PrevAction : AI
    {
        public override bool IsAction()
        {
            return Data.Before.Name == relation;
        }
    }
}
