using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportFNSUtility
{
    public partial class Form1 : Form
    {

        public static Form1 form = null;
        public Form1()
        {
            InitializeComponent();
            Form1.form = this;
            form.Text = "FNSUtility V.0.0.2.0(S)"; //А давай ка играть с названием формы что бы понятно так! H-Hamoru
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                TB_Patch.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            ReadReport readReport = new ReadReport(TB_Patch.Text);
            Thread t = new Thread((ThreadStart)delegate { readReport.Read(); });
            t.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ChB_VisibleValue.Checked)
            {
                if (MessageBox.Show("Процедура займёт значительное количество\n" +
                    "времени при большом объёме данных.\n" +
                    "Вы уверены что хотите применить это свойство?",
                    "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    ChB_VisibleValue.Checked = false;
                }
            }
        }

        private void B_startParse_Click(object sender, EventArgs e)
        {

        }

        public void UpdateProgressBar(int val)
        {
            progressBar1.Value = val;
        }
    }
}
