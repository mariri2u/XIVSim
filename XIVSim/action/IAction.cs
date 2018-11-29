using System;
using System.Collections.Generic;
using System.Text;
using xivsim.action;
using xivsim.jobai;
using xivsim.actionai;

namespace xivsim.action
{
    public interface IAction
    {
        BattleData Data { get; set; }
        List<ActionAI> AI { get; }
        string Name { get; }
        int Power { get; }
        double Cast { get; }
        double Recast { get; }
        double Motion { get; }

        IAction ResistAI(ActionAI ai);

        // 現在の状態から判断してアクションを実行できるか判断する
        bool CanAction();

        // 登録されたAIでアクションを実行するか判断する
        bool IsActionByAI();

        // アクションを実行する (アクションの実行可否は判断済みとする)
        IAction UseAction();
    }
}
