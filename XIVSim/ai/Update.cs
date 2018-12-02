using System;
using System.Collections.Generic;
using System.Text;

namespace xivsim.ai
{
    public class Update : AI
    {
        public override bool IsAction()
        {
            bool result = true; ;

            // 有効時間のあるアクションの場合
            if (Action.Duration > eps)
            {
                // 効果時間中の場合
                if (Action.Remain <= threshold_f)
                {
                    result &= true;
                }
                else
                {
                    result &= false;
                }
            }

            // スタック数のあるアクションの場合
            if (Action.Max > 0)
            {
                // 効果時間中の場合
                if (Action.Stack <= threshold_i)
                {
                    result &= true;
                }
                else
                {
                    result &= false;
                }
            }

            return result;
        }
    }
}
