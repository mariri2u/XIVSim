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
        private double frameDmg;
        private double[] stepDmg;

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

        protected void ConstructActionAndAI(Dictionary<string,Action> acts, List<ActionAI> ais)
        {
            foreach (ActionAI aa in ais)
            {
                aa.Action = acts[aa.Name];
                foreach (AI ai in aa.AI)
                {
                    ai.Action = acts[aa.Name];
                }
            }

            foreach (Action act in acts.Values)
            {
                if (act is AutoAttack)
                {
                    ActionAI aa = new ActionAI();
                    aa.Name = act.Name;
                    aa.Action = act;
                    aa.AddAI(new NoWait());
                    ais.Add(aa);
                }
            }
        }

        public BattleManager(double delta, string fname)
        {
            this.delta = delta;
            frame = 0;
            time = 0.0;
            fps = (int)(1 / delta);
            data = new BattleData();
            stepDmg = new double[3000];
            for(int i = 0; i < stepDmg.Length; i++)
            {
                stepDmg[i] = 0.0;
            }
            logs = new Logger(fname);
        }

        public void Init(double gcd, DamageTable table, string actfile, string aifile)
        {
            data.Table = table;

            data.Action = CreateAction(gcd, actfile);
            data.ActionAI = CreateAI(aifile);
            ConstructActionAndAI(data.Action, data.ActionAI);

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

            foreach (Action act in data.Action.Values)
            {
                act.Data = data;

                if (act is IAbility)
                {
                    data.Recast[act.Name] = 0.0;
                }

                if (act.Duration > 0 || act.Max > 0 || act.Amp > eps || act.Haste > eps || act.Crit > eps || act.Direc > eps)
                {
                    data.State[act.Name] = act;
                }
            }

            foreach (ActionAI aa in data.ActionAI)
            {
                aa.Data = Data;

                foreach (AI ai in aa.AI)
                {
                    ai.Data = Data;
                }
            }
        }

        private Dictionary<string,Action> CreateAction(double gcd, string fname)
        {
            ActionConfig config = ActionConfig.Load(fname);
            Dictionary<string,Action> actions = null;
            if (config == null)
            {
                config = new ActionConfig();
                config.Add(new GCDAction(), "マレフィガ");
                config.Add(new GCDAction(), "コンバラ");
                config.Add(new Ability(), "クラウンロード");
                config.Save(fname);
            }
            actions = config.Get();

            // GCDアクションのリキャストと詠唱とモーションを書き換える
            foreach(Action act in actions.Values)
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

        private List<ActionAI> CreateAI(string fname)
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

            List<ActionAI> ais = config.Get();

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
            if (time < eps) { return 0; }
            else { return totalDmg / time; }
        }

        public double CalcSecDps(int sec)
        {
            double dmg = 0.0;
            int end = frame / fps - 1;
            int start = end - sec;

            if (start < 0) { start = 0; }
            if (end <= start) { return 0.0; }

            for (int i = start; i <= end; i++)
            {
                dmg += stepDmg[i];
            }
            return dmg / (end - start);
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
                foreach (ActionAI aa in data.ActionAI)
                {
                    if (aa.Action.CanAction() && aa.IsActionByAI())
                    {
                        used = aa.Action;
                        aa.Action.StartAction();
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
            frameDmg = 0.0;
            foreach( string key in data.Damage.Keys )
            {
                frameDmg += data.Damage[key];
            }
            stepDmg[frame / fps] += frameDmg;
            totalDmg += frameDmg;
        }

        // ステップの結果をログにダンプする
        private void Dump()
        {
            // 何らかの状態に変化があった場合のみダンプする
            if (frame % fps == 0 || frameDmg > eps || used != null)
            {
                logs.AddDouble("time", time);

                if (used != null)
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

                logs.AddDouble("dmg.sec", stepDmg[frame / fps]);
                logs.AddDouble("dps.sec", CalcSecDps(15));
                logs.AddDouble("dmg.all", totalDmg);
                logs.AddDouble("dps.all", CalcDps());

                logs.AddDouble("pureamp", Data.GetPureAmp());
                logs.AddDouble("critprob", Data.GetCritProb());
                logs.AddDouble("critamp", Data.GetCritAmp());
                logs.AddDouble("direcprob", Data.GetDirecProb());
                logs.AddDouble("direcamp", Data.GetDirecAmp());
                logs.AddDouble("amp", Data.GetAmp());
                logs.AddDouble("haste", Data.GetHaste());

                foreach (string key in data.Damage.Keys)
                {
                    logs.AddDouble(key + ".damage", data.Damage[key]);
                }
                logs.AddDouble("damage", frameDmg);

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
