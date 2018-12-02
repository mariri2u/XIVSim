using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.ai;
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

        protected void ConstructActionAndAI(List<Action> acts, Dictionary<string,List<AI>> ais)
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

            // AIで定義されていないActionを登録
            foreach (string key in actmap.Keys)
            {
                bool contain = false;
                foreach (Action act in acts)
                {
                    if (act.Name == key)
                    {
                        contain = true;
                        break;
                    }
                }
                if (!contain)
                {
                    acts.Add(actmap[key]);
                }
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

        public void Init(double gcd, DamageTable table, string actfile, string aifile)
        {
            data.Table = table;
            data.Action = this.CreateAction(gcd, actfile);
            Dictionary<string, List<AI>> ais = this.CreateAI(aifile);
            ConstructActionAndAI(data.Action, ais);

            time = 0.0;
            frame = 0;
            totalDmg = 0.0;

            data.Clear();
            data.Recast["aa"] = 0.0;
            data.Recast["global"] = 0.0;
            data.Recast["cast"] = 0.0;
            data.Recast["motion"] = 0.0;
            data.Recast["dot"] = dotTick;

            used = null;

            foreach (Action act in data.Action)
            {
                act.Data = data;
                foreach (AI ai in act.AI)
                {
                    ai.Data = this.Data;
                }

                if (act is IAbility)
                {
                    data.Recast[act.Name] = 0.0;
                }

                if (act.Duration > 0 || act.Max > 0 || act.Amp > eps || act.Haste > eps)
                {
                    data.State[act.Name] = act;
                }
            }
        }

        private List<Action> CreateAction(double gcd, string fname)
        {
            ActionConfig config = ActionConfig.Load(fname);
            List<Action> actions = null;
            if (config == null)
            {
                actions = new List<Action>();
                actions.Add(new GCDAction("マレフィガ", 220, 0.6, 1.0));
                actions.Add(new GCDAction("コンバラ", 0, 0.0, 1.0, 50, 30));
                actions.Add(new Ability("クラウンロード", 300, 90));
                config = new ActionConfig();
                config.AddAll(actions);
                config.Save(fname);
            }
            else
            {
                actions = config.GetAll();
            }

            // GCDアクションのリキャストと詠唱とモーションを書き換える
            foreach(Action act in actions)
            {
                if(act is IGCD g)
                {
                    g.ApplyGlobal(gcd);
                }
                else if(act is AutoAttack aa)
                {
                    aa.ApplySpeed(gcd);
                }
            }

            return actions;
        }

        private Dictionary<string,List<AI>> CreateAI(string fname)
        {
            AIConfig config = AIConfig.Load(fname);
            if (config == null)
            {
                config = new AIConfig();
                config.Add("マレフィガ", new NoWait());
                config.Add("コンバラ", new LessRemain());
                config.Add("クラウンロード", new NoInterrupt());
                config.Save(fname);
            }

            Dictionary<string, List<AI>> ais = new Dictionary<string, List<AI>>();

            foreach (AIPair pair in config.List)
            {
                ais[pair.Action] = config.Get(pair.Action);
            }

            return ais;
        }

        public void Step()
        {
            InitStep();
            Tick();
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

        private void Tick()
        {
            foreach (Action state in data.State.Values)
            {
                if (state.Remain > eps)
                {
                    state.Tick();
                }
            }
            if (data.Recast["dot"] < eps)
            {
                // DoTTickを更新
                data.Recast["dot"] = dotTick;
            }
        }

        private void ActionDamage()
        {
            if (Data.Casting == null)
            {
                foreach (Action act in data.Action)
                {
                    if (act.CanAction() && act.IsActionByAI())
                    {
                        used = act;
                        act.StartAction();
                        break;
                    }
                }
            }
            else
            {
                if (Data.Recast["cast"] < eps)
                {
                    used = Data.Casting;
                    Data.Casting.DoneAction();
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

            if (Data.Before != null)
            {
                logs.AddText("before", Data.Before.Name);
            }
            else
            {
                logs.AddText("before", "none");
            }

            logs.AddDouble("pureamp", Data.GetPureAmp());
            logs.AddDouble("critprob", Data.GetCritProb());
            logs.AddDouble("critamp", Data.GetCritAmp());
            logs.AddDouble("direcprob", Data.GetDirecProb());
            logs.AddDouble("direcamp", Data.GetDirecAmp());
            logs.AddDouble("amp", Data.GetAmp());
            logs.AddDouble("haste", Data.GetHaste());

            double dmg = 0.0;
            foreach( string key in data.Damage.Keys )
            {
                dmg += data.Damage[key];
                logs.AddDouble(key + ".damage", data.Damage[key]);
            }
            logs.AddDouble("damage", dmg);

            foreach (string key in data.State.Keys)
            {
                if (data.State[key].Duration > 0)
                {
                    logs.AddDouble(key + ".remain", data.State[key].Remain);
                }
            }

            foreach (string key in data.State.Keys)
            {
                if (data.State[key].Max > 0)
                {
                    logs.AddInt(key + ".stack", data.State[key].Stack);
                }
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

            keys = new List<string>(data.State.Keys);
            foreach (string key in keys)
            {
                Data.State[key].DoneStep();
                Data.State[key].Remain -= delta;
            }
            
            frame++;
            time += delta;
            Data.Time = time;
        }
    }
}
