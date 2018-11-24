using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim
{
    class BattleData
    {
        private Dictionary<string, double> recast;
        private Dictionary<string, DoT> dots;
        private Dictionary<string, double> damage;
        private LinkedList<Action> history;

        public BattleData()
        {
            recast = new Dictionary<string, double>();
            dots = new Dictionary<string, DoT>();
            damage = new Dictionary<string, double>();
            history = new LinkedList<Action>();
        }

        public void Clear()
        {
            recast.Clear();
            dots.Clear();
            damage.Clear();
            history.Clear();
        }

        public Dictionary<string, double> Recast { get { return recast; } }
        public Dictionary<string, DoT> DoTs { get { return dots; } }
        public Dictionary<string, double> Damage { get { return damage; } }
        public LinkedList<Action> History { get { return history; } }
        public DamageTable Table { get; set; }
    }
}
