using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.actionai;
using xivsim.config;

namespace xivsim
{
    public class BattleManager
    {
        protected const double eps = 1.0e-7;

        private const double dotTick = 3.0;

        private double time;
        private double delta;
        private int frame;
        private int fps;
        private double totalDmg;

        protected List<Action> actions;

        private Logger logs;

        protected BattleData data;

        protected Action used;

        public BattleData Data
        {
            get { return data; }
        }

        public Action Used
        {
            get { return used; }
        }

        protected void ConstructActionAndAI(List<Action> acts, Dictionary<string,List<ActionAI>> ais)
        {
            Dictionary<string, Action> actmap = Action.ListToMap(acts);

            // AIで定義された順にActionをソートしてAIをActionに登録する
            acts.Clear();
            foreach (string key in ais.Keys)
            {
                Action act = actmap[key];
                act.ResistAI(ais[key]);
                acts.Add(act);
            }
        }

        public BattleManager(double delta, string fname)
        {
            this.delta = delta;
            this.frame = 0;
            this.time = 0.0;
            this.fps = (int)(1 / delta);
            this.data = new BattleData();
            logs = new Logger(fname);
        }

        public void Init(string actfile, string aifile)
        {
            data.Table = new DamageTable();
            actions = this.CreateAction(actfile);
            Dictionary<string, List<ActionAI>> ais = this.CreateActionAI(aifile);
            ConstructActionAndAI(actions, ais);

            time = 0.0;
            frame = 0;
            totalDmg = 0.0;

            data.Clear();
            data.Recast["global"] = 0.0;
            data.Recast["cast"] = 0.0;
            data.Recast["motion"] = 0.0;
            data.Recast["dot"] = dotTick;

            used = null;

            foreach (Action act in actions)
            {
                act.Data = data;
                foreach (ActionAI ai in act.AI)
                {
                    ai.Data = this.Data;
                }

                if (act is IAbility)
                {
                    data.Recast[act.Name] = 0.0;
                }

                if (act.Slip > 0)
                {
                    data.DoTs[act.Name] = act;
                }
            }
        }

        private List<Action> CreateAction(string fname)
        {
            ActionConfig config = ActionConfig.Load(fname);
            List<Action> actions = null;
            if (config == null)
            {
                double gcd = 2.45;
                double cast = 1.47;
                actions = new List<Action>();
                actions.Add(new GCDAction("マレフィガ", 220, cast, gcd));
                actions.Add(new GCDAction("コンバラ", 0, 0.0, gcd, 50, 30));
                actions.Add(new Ability("クラウンロード", 300, 90));
                config = new ActionConfig();
                config.AddAll(actions);
                config.Save(fname);
            }
            else
            {
                actions = config.GetAll();
            }

            return actions;
        }

        private Dictionary<string, List<ActionAI>> CreateActionAI(string fname)
        {
            AIConfig config = AIConfig.Load(fname);
            if (config == null)
            {
                config = new AIConfig();
                config.Add("マレフィガ", new NoWait());
                config.Add("コンバラ", new Update());
                config.Add("クラウンロード", new NoInterrupt());
                config.Save(fname);
            }

            Dictionary<string, List<ActionAI>> ais = new Dictionary<string, List<ActionAI>>();

            foreach (AIPair pair in config.List)
            {
                ais[pair.Action] = config.Get(pair.Action);
            }

            return ais;
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
                foreach (Action dot in data.DoTs.Values)
                {
                    dot.Tick();
                }
                // DoTTickを更新
                data.Recast["dot"] = dotTick;
            }
        }

        private void ActionDamage()
        {
            if (Data.Casting == null)
            {
                foreach (Action act in actions)
                {
                    if (act.CanAction() && act.IsActionByAI())
                    {
                        used = act.UseAction();
                    }
                }
            }
            else
            {
                if (Data.Recast["cast"] < eps)
                {
                    used = Data.Casting.DoneCast();
                }
            }
        }

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
            if (frame % fps == 0 || dmg > eps || used != null)
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
