using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    class DoT : Action
    {
        protected int slip;
        protected int duration;

        public DoT(string name, int power, double cast, double recast, int slip, int duration) : base(name, power, cast, recast, 0.1)
        {
            this.slip = slip;
            this.duration = duration;
        }

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

        public double Remain { get; set; }
            
        public int Slip
        {
            get { return slip; }
        }

        public int Duration
        {
            get { return duration; }
        }
    }
}
