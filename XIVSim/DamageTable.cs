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

        public double BaseCritProb { get; set; }
        public double BaseCritAmp { get; set; }
        public double BaseDirecProb { get; set; }
        public double BaseDirecAmp { get; set; }

        public DamageTable(int x0, int x1, double y0, double y1)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;

            BaseCritProb = 0.15;
            BaseCritAmp = 1.5;
            BaseDirecProb = 0.0;
            BaseDirecAmp = 1.15;
        }

        public double Calc(int x)
        {
            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }

        public static double CalcExpect(double prob, double maxamp)
        {
            double amp = 1.0;

            if(prob <= 0.0) { amp = 1.0; }
            else if(prob >= 1.0) { amp = maxamp; }
            else { amp = 1.0 + (maxamp - 1.0) * prob; }

            return amp;
        }

        public static DamageTable GetTankTable()
        {
            DamageTable table = GetPhysicTable();
            table.BaseDirecProb = 0.0;
            return table;
        }

        public static DamageTable GetPhysicTable()
        {
            DamageTable table = new DamageTable(150, 540, 3000, 11200);
            //return new DamageTable(160, 520, 3700 * 1.125, 11500 * 1.125);
            table.BaseDirecProb = 0.3;
            return table;
        }

        public static DamageTable GetMagicTable()
        {
            DamageTable table = new DamageTable(50, 220, 1900, 8000);
            //DamageTable table = new DamageTable(50, 220, 1900 * 1.125, 8000 * 1.125);
            table.BaseDirecProb = 0.3;
            return table;
        }

        public static DamageTable GetHealerTable()
        {
            DamageTable table = GetMagicTable();
            //DamageTable table = new DamageTable(50, 220, 1900 * 1.125, 8000 * 1.125);
            table.BaseDirecProb = 0.0;
            return table;
        }
    }
}
