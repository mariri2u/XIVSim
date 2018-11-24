using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.ai;

namespace xivsim.action
{
    public interface IAction
    {
        bool IsAction();
        void Calc();

        BattleData Data { get; set; }
        AI AI { get; set; }
        string Name { get; }
        int Power { get; }
        double Cast { get; }
        double Recast { get; }
        double Motion { get; }
    }
}
