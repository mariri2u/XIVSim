using System;
using System.Collections.Generic;
using System.Text;
using xivsim.actionai;

namespace xivsim.action
{
    class Ability : Action, IAbility
    {
        public Ability(string name, int power, double recast)
            : base(name, power, 0.0, recast, 0.8, new AbilityAI())
        { }
        

        public override IAction Calc()
        {
            Data.Damage["action"] = Data.Table.Calc(this.Power);
            Data.Recast[this.Name] = this.Recast;
            Data.Recast["motion"] = this.Motion;
            Data.History.AddFirst(this);

            return this;
        }
    }
}
