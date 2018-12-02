using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class SamCombo : GCDAction
    {
        protected override bool InBefore()
        {
            if (Data.State[RelationA].IsValid()) { return true; }
            else { return base.InBefore(); }
        }

        public override void CalcAction()
        {
            base.CalcAction();

            if(Data.State[RelationA].IsValid())
            {
                Data.State[RelationA].Stack--;
            }

            if (RelationB != null)
            {
                Data.State[RelationB].Stack = 1;
            }
        }
    }
}
