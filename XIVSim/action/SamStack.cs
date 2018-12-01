﻿using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.action
{
    public class SamStack : Action
    {
        public override int Stack
        {
            get
            {
                if(RelationA == null || RelationB == null || RelationC == null)
                {
                    return 0;
                }

                int sen = 0;
                if (Data.State[RelationA].Stack > 0) { sen++; }
                if (Data.State[RelationB].Stack > 0) { sen++; }
                if (Data.State[RelationC].Stack > 0) { sen++; }
                return sen;
            }

            set
            {
                if (RelationA == null || RelationB == null || RelationC == null)
                {
                    return;
                }

                if (value == 0)
                {
                    Data.State[RelationA].Stack = 0;
                    Data.State[RelationB].Stack = 0;
                    Data.State[RelationC].Stack = 0;
                }
            }
        }

        public override bool CanAction()
        {
            return false;
        }

        public override void CalcAction()
        {
        }
    }
}
