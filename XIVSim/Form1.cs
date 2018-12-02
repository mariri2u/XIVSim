using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace xivsim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void whm_Click(object sender, EventArgs e)
        {
            // 初期化
            double delta = 0.01;

            BattleManager ai = new BattleManager(delta, @"whm_combat.csv");
            ai.Init(2.41, DamageTable.GetHealerTable(), @"action/whm.xml", @"ai/whm.xml");

            for (double time = 0.0; time <= 1200; time += delta)
            {
                ai.Step();
            }
            textBox1.Text += "Whm DPS is " + (int)ai.CalcDps() + "\r\n";
            ai.Close();
        }

        private void ast_Click(object sender, EventArgs e)
        {
            // 初期化
            double delta = 0.01;

            BattleManager ai = new BattleManager(delta, @"ast_combat.csv");
            ai.Init(2.45, DamageTable.GetHealerTable(), @"action/ast.xml", @"ai/ast.xml");

            for (double time = 0.0; time <= 1200; time += delta)
            {
                ai.Step();
            }
            textBox1.Text += "Ast DPS is " + (int)ai.CalcDps() + "\r\n";
            ai.Close();
        }

        private void war_Click(object sender, EventArgs e)
        {
            // 初期化
            double delta = 0.01;

            BattleManager ai = new BattleManager(delta, @"war_combat.csv");
            ai.Init(2.37, DamageTable.GetTankTable(), @"action/war.xml", @"ai/war.xml");

            for (double time = 0.0; time <= 1200; time += delta)
            {
                ai.Step();
            }
            textBox1.Text += "War DPS is " + (int)ai.CalcDps() + "\r\n";
            ai.Close();
        }

        private void sch_Click(object sender, EventArgs e)
        {
            // 初期化
            double delta = 0.01;

            BattleManager ai = new BattleManager(delta, @"sch_combat.csv");
            ai.Init(2.45, DamageTable.GetHealerTable(),  @"action/sch.xml", @"ai/sch.xml");

            for (double time = 0.0; time <= 1200; time += delta)
            {
                ai.Step();
            }
            textBox1.Text += "Sch DPS is " + (int)ai.CalcDps() + "\r\n";
            ai.Close();
        }

        private void sam_Click(object sender, EventArgs e)
        {
            // 初期化
            double delta = 0.01;

            BattleManager ai = new BattleManager(delta, @"sam_combat.csv");
            ai.Init(2.37, DamageTable.GetPhysicTable(), @"action/sam.xml", @"ai/sam.xml");

            for (double time = 0.0; time <= 1200; time += delta)
            {
                ai.Step();
            }
            textBox1.Text += "Sam DPS is " + (int)ai.CalcDps() + "\r\n";
            ai.Close();
        }
    }
}
