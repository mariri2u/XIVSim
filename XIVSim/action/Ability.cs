using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    class Ability : Action, IAbility
    {
        public Ability(string name, int power, double recast) : base(name, power, 0.0, recast, 0.8)
        {
        }

        public override bool IsAction()
        {
            // リキャストが完了していて、モーションによってGCDの食い込みが発生しない場合
            return (Data.Recast[this.Name] < eps && this.Motion < Data.Recast["global"]);
        }

        public override void Calc()
        {
            Data.Damage["action"] = Data.Table.Calc(this.Power);
            Data.Recast[this.Name] = this.Recast;
            Data.Recast["motion"] = this.Motion;
            Data.History.AddFirst(this);
        }
    }
}
