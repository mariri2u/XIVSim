using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim
{
    public class BattleData
    {
        public BattleData()
        {
            Recast = new Dictionary<string, double>();
            Damage = new Dictionary<string, double>();
            DoTs = new Dictionary<string, Action>();
            Casting = null;
            History = new List<Action>();
            Reserve = new List<Action>();
        }
        
        public void Clear()
        {
            Recast.Clear();
            DoTs.Clear();
            Damage.Clear();
            Casting = null;
            History.Clear();
            Reserve.Clear();
        }

        public Dictionary<string, double> Recast { get; }
        public Dictionary<string, double> Damage { get; }
        public Dictionary<string, Action> DoTs { get; }
        public Action Casting { get; set; }
        public List<Action> History { get; }
        public List<Action> Reserve { get; }
        public DamageTable Table { get; set; }
    }
}
