using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using xivsim.ai;

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

            AICore ai = new WhmAI(delta);
            ai.PreInit();
            ai.Init();

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

            AICore ai = new AstAI(delta);
            ai.PreInit();
            ai.Init();

            for (double time = 0.0; time <= 1200; time += delta)
            {
                ai.Step();
            }
            textBox1.Text += "Ast DPS is " + (int)ai.CalcDps() + "\r\n";
            ai.Close();
        }
    }
}
