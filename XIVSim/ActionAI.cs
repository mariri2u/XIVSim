using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.ai;

namespace xivsim
{
    public class ActionAI
    {
        public string Name { get; set; }

        public Action Action { get; set; }
        public BattleData Data { get; set; }

        public ActionAI()
        {
            AI = new List<AI>();
        }
        
        public List<AI> AI { get; set; }

        public void AddAI(AI ai)
        {
            AI.Add(ai);
        }

        public void ResistAI(List<AI> ais)
        {
            foreach (AI ai in ais)
            {
                AI.Add(ai);
            }
        }

        // どのAIも無視せず実行判定を行う
        public bool IsActionByAI() { return IsActionByAI(false); }

        public bool IsActionByAI(bool ignore)
        {
            // グループ毎に実行判定を行う
            Dictionary<string, bool> resultTable = new Dictionary<string, bool>();
            foreach (AI ai in AI)
            {
                // 各グループの初期値はtrue
                if (!resultTable.ContainsKey(ai.Group))
                {
                    resultTable[ai.Group] = true;
                }

                // 引数がtrueの場合はNoInterruptを無視する
                if (!ignore || !(ai is NoInterrupt))
                {
                    resultTable[ai.Group] &= ai.IsAction();
                }
            }

            // commonグループは全てのグループの結果に影響する
            bool all = true;
            if (resultTable.ContainsKey("common"))
            {
                all = resultTable["common"];
                resultTable.Remove("common");
            }

            // いずれかのグループがtrueなら実行する
            bool result = false;
            if (resultTable.Count > 0)
            {
                foreach (bool res in resultTable.Values)
                {
                    result |= res & all;
                }
            }
            else
            {
                result = all;
            }

            return result;
        }

        public bool IsActionByAnyAI()
        {
            bool result = false;
            foreach (ActionAI aa in Data.ActionAI)
            {
                if (aa.Name == Name)
                {
                    result |= aa.IsActionByAI(true);
                }
            }
            return result;
        }
    }
}
