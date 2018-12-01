using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

namespace xivsim.action
{
    public class Ability : Action, IAbility
    {
        public Ability() : base() { }

        public Ability(string name, int power, double recast)
            : base(name, power, 0.0, recast, 0.8)
        { }

        public override bool CanAction()
        {
            if (base.CanAction() && Data.Recast[Name] < eps) { return true; }
            else { return false; }
        }

        public override void CalcAction()
        {
            Data.Recast[Name] = Recast;
        }
    }
}
