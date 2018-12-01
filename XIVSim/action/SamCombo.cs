using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class SamCombo : GCDAction
    {
        protected override bool InBefore()
        {
            if (Data.State[RelationA].Remain > eps && Data.State[RelationA].Stack > 0) { return true; }
            else { return base.InBefore(); }
        }

        public override void CalcAction()
        {
            base.CalcAction();

            if(Data.State[RelationA].Remain > eps && Data.State[RelationA].Stack > 0)
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
