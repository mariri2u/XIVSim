using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim
{
    public class DamageTable
    {
        private readonly int x0;
        private readonly double y0;
        private readonly int x1;
        private readonly double y1;

        public DamageTable(int x0, int x1, double y0, double y1)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }

        public double Calc(int x)
        {
            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }

        public static DamageTable GetPhysicTable()
        {
            //return new DamageTable(160, 520, 3700 * 1.125, 11500 * 1.125);
            return new DamageTable(160, 520, 3700, 11500);
        }

        public static DamageTable GetMagicTable()
        {
            return new DamageTable(50, 220, 1900 * 1.125, 8000 * 1.125);
        }
    }
}
