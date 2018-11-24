using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    class AstAI : AICore
    {
        public AstAI(double delta) : base(delta, @"ast_combat.csv") { }

        protected override DamageTable CreateTable()
        {
            return new DamageTable();
        }

        protected override Dictionary<string, Action> CreateActions()
        {
            double gcd = 2.45;
            double cast = 1.47;

            List<Action> actions = new List<Action>();
            actions.Add(new WeaponSkill("マレフィガ", 220, cast, gcd));
            actions.Add(new DoT("コンバラ", 0, 0.0, gcd, 50, 30));
            actions.Add(new Ability("クラウンロード", 300, 90));
            actions.Add(new Ability("アーサリースター", 200, 60));

            return Action.ListToMap(actions);
        }

        protected override void AIAction()
        {
            // 魔法・WS
            if (data.Recast["global"] < eps)
            {
                // DoT更新
                foreach (Action act in actions.Values)
                {
                    if (act is DoT dot && act.IsAction())
                    {
                        act.Calc();
                        used = act;
                        return;
                    }
                }

                // DoT更新が不要な場合
                foreach (Action act in actions.Values)
                {
                    if (act is WeaponSkill && act.IsAction())
                    {
                        act.Calc();
                        used = act;
                        return;
                    }
                }
            }
            // アビリティ (GCDアクションを行わない場合)
            else
            {
                foreach (Action act in actions.Values)
                {
                    if (act is Ability && act.IsAction())
                    {
                        act.Calc();
                        used = act;
                        return;
                    }
                }
            }
        }
    }
}
