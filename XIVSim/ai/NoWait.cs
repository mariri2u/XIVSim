﻿using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    public class NoWait : AI
    {
        public override bool IsAction()
        {
            return true;
        }
    }
}
