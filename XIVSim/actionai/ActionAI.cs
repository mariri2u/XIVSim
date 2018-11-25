using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.jobai;

namespace xivsim.actionai
{
    public abstract class ActionAI
    {
        public const double eps = 1.0e-7;

        public Action Action { get; set; }
        public BattleData Data { get; set; }

        public abstract bool IsAction();
    }
}
