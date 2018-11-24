using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public interface IDoT
    {
        void Tick();

        double Remain { get; set; }
        int Slip { get; }
        int Duration { get; }
    }
}
