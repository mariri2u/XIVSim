using System;
using System.Collections.Generic;
using System.Text;
using xivsim.actionai;

namespace xivsim.action
{
    class Ability : Action, IAbility
    {
        public Ability(string name, int power, double recast)
            : base(name, power, 0.0, recast, 0.8)
        { }

        public override bool CanAction()
        {
            if (base.CanAction() && Data.Recast[Name] < eps) { return true; }
            else { return false; }
        }

        public override IAction CalcAction()
        {
            Data.Recast[Name] = Recast;

            return this;
        }
    }
}
