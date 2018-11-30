using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.config;

namespace xivsim.actionai
{
    public abstract class ActionAI
    {
        public const double eps = 1.0e-7;

        public Action Action { get; set; }
        public BattleData Data { get; set; }

        protected double remain;
        protected string name;
        protected double recast;

        public ActionAI() { }

        // 実行の可否をAIが判断する (アクションの実行可否は判断済みとする)
        public abstract bool IsAction();

        // AIファイルの引数を処理する
        public void LoadConfig(AIElement arg)
        {
            this.remain = arg.Remain;
            this.name = arg.Name;
            this.recast = arg.Recast;
        }
    }
}
