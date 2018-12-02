using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class Hagakure : Ability
    {
        public override bool CanAction()
        {
            if (base.CanAction() && Data.State[RelationA].Stack > 0) { return true; }
            else { return false; }
        }

        public override void CalcAction()
        {
            base.CalcAction();

            Data.State[Relation].Stack += Increase * Data.State[RelationA].Stack;
            Data.State[RelationA].Stack = 0;
        }
    }
}
