using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    interface IStack
    {
        double Remain { get; set; }
        int StackedValue { get; set; }
        int Duration { get; }
    }
}
