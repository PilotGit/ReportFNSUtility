using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportFNSUtility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            ReadReport readReport = new ReadReport(textBox1.Text);
            ReadReport.form = this;
            readReport.Read();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if(MessageBox.Show("Процедура займёт значительное количество\n" +
                    "времени при большом объёме данных.\n" +
                    "Вы уверены что хотите применить это свойство?",
                    "Предупреждение",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    checkBox1.Checked = false;
                }
            }
        }
    }
}
