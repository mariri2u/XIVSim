using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.ai
{
    abstract class AICore
    {
        protected const double eps = 1.0e-7;

        private const double dotTick = 3.0;

        private double time;
        private double delta;
        private double totalDmg;

        protected Dictionary<string, Action> actions;

        private Logger logs;

        protected BattleData data;

        protected Action used;

        public BattleData Data
        {
            get { return data; }
        }

        protected abstract DamageTable CreateTable();
        protected abstract Dictionary<string, Action> CreateActions();

        public AICore(double delta, string fname)
        {
            this.delta = delta;
            this.data = new BattleData();
            logs = new Logger(fname);
        }

        public void PreInit()
        {
            data.Table = this.CreateTable();
            actions = this.CreateActions();
        }

        public void Init()
        {
            this.time = 0.0;
            totalDmg = 0.0;

            data.Clear();

            used = null;

            data.Recast["global"] = 0.0;
            data.Recast["cast"] = 0.0;
            data.Recast["motion"] = 0.0;
            data.Recast["dot"] = dotTick;

            foreach (Action act in actions.Values)
            {
                act.Data = data;
                act.AI = this;
                if (act is Ability)
                {
                    data.Recast[act.Name] = 0.0;
                }
                else if (act is DoT)
                {
                    data.DoTs[act.Name] = (DoT)act;
                }
            }
        }

        public void Step()
        {
            InitStep();
            DoTTick();
            ActionDamage();
            MergeDamage();
            Dump();
            StepToNext();
        }

        public void Close()
        {
            logs.Close();
        }

        public double CalcDps()
        {
            return totalDmg / time;
        }

        private void InitStep()
        {
            used = null;
            data.Damage.Clear();
            data.Damage["dot"] = 0.0;
            data.Damage["action"] = 0.0;
        }

        private void DoTTick()
        {
            if (data.Recast["dot"] < eps)
            {
                foreach (DoT dot in data.DoTs.Values)
                {
                    if ( dot.Remain > 0.0)
                    {
                        data.Damage["dot"] += data.Table.Calc(dot.Slip);
                    }
                }
                // DoTダメージを計算すればDoTTickを更新
                data.Recast["dot"] = dotTick;
            }
        }

        private void ActionDamage()
        {
            // キャスト中・モーション中は行動不可
            if (data.Recast["cast"] < eps && data.Recast["motion"] < eps)
            {
                // 行動の可否のみを判断して、実際の行動は個々のAIに判断を任せる
                AIAction();
            }
        }

        // 個々のAIが判断して行動する
        protected abstract void AIAction();

        // DoTのダメージとAIの行動結果をマージする
        private void MergeDamage()
        {
            foreach( string key in data.Damage.Keys )
            {
                totalDmg += data.Damage[key];
            }
        }

        // ステップの結果をログにダンプする
        private void Dump()
        {
            logs.AddDouble("time", time);

            if( used != null )
            {
                logs.AddText("action", used.Name);
            }
            else
            {
                logs.AddText("action", "none");
            }

            double dmg = 0.0;
            foreach( string key in data.Damage.Keys )
            {
                dmg += data.Damage[key];
                logs.AddDouble(key + ".damage", data.Damage[key]);
            }
            logs.AddDouble("damage", dmg);

            foreach (string key in data.DoTs.Keys)
            {
                logs.AddDouble(key + ".remain", data.DoTs[key].Remain);
            }

            foreach (string key in data.Recast.Keys)
            {
                logs.AddDouble(key + ".recast", data.Recast[key]);
            }

            logs.AddDouble("totalDamage", totalDmg);
            double dps = CalcDps();
            if(time < eps)
            {
                dps = 0.0;
            }
            logs.AddDouble("dps", dps);

            // 何らかの状態に変化があった場合のみダンプする
            if (dmg > eps || used != null)
            {
                logs.Dump();
            }
            else
            {
                logs.Clear();
            }
        }

        private void StepToNext()
        {
            List<string> keys = new List<string>(data.Recast.Keys);
            foreach( string key in keys )
            {
                data.Recast[key] -= delta;
            }

            keys = new List<string>(data.DoTs.Keys);
            foreach( string key in keys )
            {
                data.DoTs[key].Remain -= delta;
            }

            time += delta;
        }
    }
}
