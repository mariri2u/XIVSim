using System;
using System.Collections.Generic;
using System.Text;
using xivsim.actionai;

namespace xivsim.action
{
    class Ability : Action, IAbility
    {
        public Ability(string name, int power, double recast)
            : base(name, power, 0.0, recast, 0.8, new NoInterruptGCD())
        { }

        public override bool CanAction()
        {
            if (base.CanAction() && Data.Recast[Name] < eps) { return true; }
            else { return false; }
        }

        public override IAction CalcAction()
        {
            Data.Damage["action"] = Data.Table.Calc(this.Power);
            Data.Recast[Name] = Recast;
            Data.Recast["motion"] = Motion;
            Data.History.AddFirst(this);

            return this;
        }
    }
}
