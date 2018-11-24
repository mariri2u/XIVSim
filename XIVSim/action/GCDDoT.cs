using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    class GCDDoT : DoT
    {
        public GCDDoT(string name, int power, double cast, double recast, int slip, int duration) :
            base(name, power, cast, recast, slip, duration)
        { }

        public override bool IsAction()
        {
            return (this.Remain < this.Cast + this.Recast);
        }

        public override void Calc()
        {
            Data.DoTs[this.Name] = this;
            this.Remain = this.Duration;
            Data.Recast["cast"] = this.Cast;
            Data.Recast["motion"] = this.Motion;
            Data.Recast["global"] = this.Recast;

            // 着弾ダメージがある場合
            if (this.Power > eps)
            {
                Data.Damage["action"] = Data.Table.Calc(this.Power);
            }
            Data.History.AddFirst(this);
        }
    }
}
