using System;
using System.Collections.Generic;
using System.Text;
using xivsim.ai;

namespace xivsim.action
{
    public class Ability : Action, IAbility
    {
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
