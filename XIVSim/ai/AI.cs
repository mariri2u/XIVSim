using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.config;

namespace xivsim.ai
{
    public abstract class AI
    {
        public const double eps = 1.0e-7;

        public Action Action { get; set; }
        public BattleData Data { get; set; }
        public string Name { get; set; }

        protected string group;
        protected string relation;
        protected double threshold_f;
        protected int threshold_i;

        public string Group { get { return group; } }

        public AI()
        {
            this.group = "common";
        }

        // 実行の可否をAIが判断する (アクションの実行可否は判断済みとする)
        public abstract bool IsAction();

        // AIファイルの引数を処理する
        public void LoadConfig(AIElement arg)
        {
            this.group = arg.Group;
            this.relation = arg.Relation;
            this.threshold_f = arg.Threshold;
            this.threshold_i = (int)(arg.Threshold + eps);
        }
    }
}
