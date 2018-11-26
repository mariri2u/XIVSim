using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim.jobai
{
    public abstract class JobAI
    {
        protected const double eps = 1.0e-7;

        private const double dotTick = 3.0;

        private double time;
        private double delta;
        private int frame;
        private int fps;
        private double totalDmg;

        protected Dictionary<string, IAction> actions;

        private Logger logs;

        protected BattleData data;

        protected IAction used;

        public BattleData Data
        {
            get { return data; }
        }

        public IAction Used
        {
            get { return used; }
        }

        protected abstract DamageTable CreateTable();
        protected abstract Dictionary<string, IAction> CreateActions();

        public JobAI(double delta, string fname)
        {
            this.delta = delta;
            this.frame = 0;
            this.fps = (int)(1 / delta);
            this.data = new BattleData();
            logs = new Logger(fname);
        }

        protected abstract void PreInit();

        public void Init()
        {
            PreInit();

            data.Table = this.CreateTable();
            actions = this.CreateActions();

            this.time = 0.0;
            totalDmg = 0.0;

            data.Clear();

            used = null;

            data.Recast["global"] = 0.0;
            data.Recast["cast"] = 0.0;
            data.Recast["motion"] = 0.0;
            data.Recast["dot"] = dotTick;

            foreach (IAction act in actions.Values)
            {
                act.Data = data;
                act.AI.Data = data;
                if (act is IAbility)
                {
                    data.Recast[act.Name] = 0.0;
                }
                else if (act is IDoT dot)
                {
                    data.DoTs[act.Name] = dot;
                }
            }

            PostInit();
        }

        protected abstract void PostInit();

        public void Step()
        {
            InitStep();
            DoTTick();
            AIAction();
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
                foreach (IDoT dot in data.DoTs.Values)
                {
                    dot.Tick();
                }
                // DoTTickを更新
                data.Recast["dot"] = dotTick;
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

            frame++;
            time += delta;
        }
    }
}
