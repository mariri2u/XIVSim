using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    class WhmAI : AICore
    {
        public WhmAI(double delta) : base(delta, @"whm_combat.csv") { }

        protected override DamageTable CreateTable()
        {
            return new DamageTable();
        }

        protected override Dictionary<string, Action> CreateActions()
        {
            double gcd = 2.41;
            double cast = 2.41;

            List<Action> actions = new List<Action>();
            actions.Add(new WeaponSkill("ストンジャ", 250, cast, gcd));
            actions.Add(new DoT("エアロラ", 50, 0.0, gcd, 50, 18));
            actions.Add(new DoT("エアロガ", 50, cast, gcd, 40, 24));
            actions.Add(new Ability("アサイズ", 300, 60));

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
                    if (act is DoT && act.IsAction())
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
