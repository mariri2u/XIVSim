using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.jobai
{
    public class AstAI : JobAI
    {
        public AstAI(double delta) : base(delta, @"ast_combat.csv") { }

        protected override void PreInit() { }

        protected override void PostInit() { }

        protected override DamageTable CreateTable()
        {
            return new DamageTable();
        }

        protected override Dictionary<string, IAction> CreateActions()
        {
            double gcd = 2.45;
            double cast = 1.47;

            List<IAction> actions = new List<IAction>();
            actions.Add(new GCDAction("マレフィガ", 220, cast, gcd));
            actions.Add(new GCDDoT("コンバラ", 0, 0.0, gcd, 50, 30));
            actions.Add(new Ability("クラウンロード", 300, 90));
            actions.Add(new Ability("アーサリースター", 200, 60));

            return Action.ListToMap(actions);
        }

        protected override void AIAction()
        {
            // GCD: DoT更新する場合
            foreach (IAction act in actions.Values)
            {
                if (act is GCDDoT dot && act.CanAction() && act.AI.IsAction())
                {
                    used = act.CalcAction();
                    return;
                }
            }

            // GCD: DoT更新が不要な場合
            foreach (IAction act in actions.Values)
            {
                if (act is GCDAction && act.CanAction() && act.AI.IsAction())
                {
                    used = act.CalcAction();
                    return;
                }
            }

            // アビリティ (GCDアクションを行わない場合)
            foreach (IAction act in actions.Values)
            {
                if (act is IAbility && act.CanAction() && act.AI.IsAction())
                {
                    used = act.CalcAction();
                    return;
                }
            }            
        }
    }
}
