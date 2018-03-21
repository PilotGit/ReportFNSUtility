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
        Thread readReportThread, writeReportThread;
        ReadReport readReport;
        WriteReport writeReport;
        public static Form1 form = null;
        Fw16.EcrCtrl ecrCtrl;
        public Form1()
        {
            InitializeComponent();
            Form1.form = this;
            form.Text = "FNSUtility V.1.0.2.0(S)"; //А давай ка играть с названием формы что бы понятно так! H-Hamoru
            treeView1.TreeViewNodeSorter = new TreeSorter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                TB_Patch.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (readReportThread?.IsAlive ?? false)
            {
                readReportThread?.Abort();
                readReportThread.Join();
                B_Update.Text = "Обновить";
            }
            else
            {
                try
                {
                    readReport?.reader?.BaseStream?.Close();
                    treeView1.Nodes.Clear();
                    readReport = new ReadReport(TB_Patch.Text);

                    readReportThread = new Thread((ThreadStart)delegate { readReport.Read(); });
                    readReportThread.Start();
                    B_Update.Text = "Остановить";
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        

        private void B_startParse_Click(object sender, EventArgs e)
        {
            if (writeReportThread?.IsAlive ?? false)
            {
                writeReportThread?.Abort();
                writeReportThread.Join();
                (writeReport.ecrCtrl as IDisposable).Dispose();
                writeReport.fileStream?.Close();
                //(writeReport.ecrCtrl as IDisposable).Dispose();
                //writeReport = new WriteReport();
                progressBar1.Value = 0;
                ////(ecrCtrl as IDisposable).Dispose();

                B_startParse.Text = "Формировать отчет";
            }
            else
            {
                ecrCtrl = new Fw16.EcrCtrl();
                ConnectToFW(CB_Port.Text);
                try
                {
                    readReport?.reader?.BaseStream?.Close();
                    writeReport = new WriteReport(ecrCtrl, TB_fileWay.Text, TB_fileName.Text);

                    writeReportThread = new Thread((ThreadStart)delegate { writeReport.WriteReportStartParseFNS(); });
                    writeReportThread.Start();
                    B_startParse.Text = "Остановить";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                (ecrCtrl as IDisposable).Dispose();
            }
            catch { }
        }

        /// <summary>
        /// подключение к ККТ. обырв соеденения с ККТ происходит в "форме" при её закрытии.
        /// </summary>
        /// <param name="serialPort">serialPort</param>
        /// <param name="baudRate">частота</param>
        bool ConnectToFW(string name="default")
        {
            try
            {
                ecrCtrl.Init(name);             //Подключчение по порту и частоте
                return true;
            }
            catch
            {
                MessageBox.Show("Ошибка при подключении");
                return false;
            }
        }

        public void UpdateProgressBar(int val)
        {
            progressBar1.Value = val;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            readReportThread?.Abort();
        }

        private void B_fileWayDialog_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                TB_fileWay.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void TB_fileName_Enter(object sender, EventArgs e)
        {
            if(TB_fileName.Text=="По умолчанию")
            {
                TB_fileName.Text = string.Empty;
                TB_fileName.ForeColor = SystemColors.WindowText;
            }
        }

        private void TB_fileName_Leave(object sender, EventArgs e)
        {
            if (TB_fileName.Text == string.Empty)
            {
                TB_fileName.Text = "По умолчанию";
                TB_fileName.ForeColor = SystemColors.ActiveCaption;
            }
        }
    }
}
