using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;

namespace xivsim
{
    public class BattleData
    {
        private Dictionary<string, double> recast;
        private Dictionary<string, IDoT> dots;
        private Dictionary<string, double> damage;
        private LinkedList<IAction> history;

        public BattleData()
        {
            recast = new Dictionary<string, double>();
            dots = new Dictionary<string, IDoT>();
            damage = new Dictionary<string, double>();
            history = new LinkedList<IAction>();
        }

        public void Clear()
        {
            recast.Clear();
            dots.Clear();
            damage.Clear();
            history.Clear();
        }

        public Dictionary<string, double> Recast { get { return recast; } }
        public Dictionary<string, IDoT> DoTs { get { return dots; } }
        public Dictionary<string, double> Damage { get { return damage; } }
        public LinkedList<IAction> History { get { return history; } }
        public DamageTable Table { get; set; }
    }
}
