using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim
{
    class DamageTable
    {
        private readonly int x0;
        private readonly double y0;
        private readonly int x1;
        private readonly double y1;

        public DamageTable()
        {
            x0 = 50;
            y0 = 1900 * 1.125;
            x1 = 220;
            y1 = 8000 * 1.125;
        }

        public double Calc(int x)
        {
            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }
    }
}
