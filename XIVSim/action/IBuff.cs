using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public interface IBuff
    {
        double Remain { get; set; }
        double Amplifier { get; }
        int Duration { get; }
    }
}
