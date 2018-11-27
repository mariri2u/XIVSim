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

        public IAction Action { get; set; }
        public BattleData Data { get; set; }

        // 実行の可否をAIが判断する (アクションの実行可否は判断済みとする)
        public abstract bool IsAction();
    }
}
