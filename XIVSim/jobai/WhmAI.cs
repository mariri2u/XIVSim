﻿using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.jobai
{
    public class WhmAI : JobAI
    {
        public WhmAI(double delta) : base(delta, @"whm_combat.csv") { }

        protected override void PreInit() { }

        protected override void PostInit() { }

        protected override DamageTable CreateTable()
        {
            return new DamageTable();
        }

        protected override Dictionary<string, IAction> CreateActions()
        {
            double gcd = 2.41;
            double cast = 2.41;

            List<IAction> actions = new List<IAction>();
            actions.Add(new GCDAction("ストンジャ", 250, cast, gcd));
            actions.Add(new GCDDoT("エアロラ", 50, 0.0, gcd, 50, 18));
            actions.Add(new GCDDoT("エアロガ", 50, cast, gcd, 40, 24));
            actions.Add(new Ability("アサイズ", 300, 60));

            return Action.ListToMap(actions);
        }

        protected override void AIAction()
        {
            // GCDアクション
            if (data.Recast["global"] < eps)
            {
                // DoT更新
                foreach (IAction act in actions.Values)
                {
                    if (act is GCDDoT && act.AI.IsAction())
                    {
                        used = act.Calc();
                        return;
                    }
                }

                // DoT更新が不要な場合
                foreach (IAction act in actions.Values)
                {
                    if (act is GCDAction && act.AI.IsAction())
                    {
                        used = act.Calc();
                        return;
                    }
                }
            }
            // アビリティ (GCDアクションを行わない場合)
            else
            {
                foreach (IAction act in actions.Values)
                {
                    if (act is IAbility && data.Recast[act.Name] < eps && act.AI.IsAction())
                    {
                        used = act.Calc();
                        return;
                    }
                }
            }
        }
    }
}