using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class Hagakure : Ability
    {
        public override bool CanAction()
        {
            int sen = 0;
            if (Data.State[RelationA].Stack > 0) { sen++; }
            if (Data.State[RelationB].Stack > 0) { sen++; }
            if (Data.State[RelationC].Stack > 0) { sen++; }

            if (base.CanAction() && sen >= require) { return true; }
            else { return false; }
        }

        public override void CalcAction()
        {
            base.CalcAction();

            int sen = 0;
            if (Data.State[RelationA].Stack > 0) { sen++; }
            if (Data.State[RelationB].Stack > 0) { sen++; }
            if (Data.State[RelationC].Stack > 0) { sen++; }

            Data.State[RelationA].Stack = 0;
            Data.State[RelationB].Stack = 0;
            Data.State[RelationC].Stack = 0;
            Data.State[Relation].Stack += increase * sen;
        }
    }
}
