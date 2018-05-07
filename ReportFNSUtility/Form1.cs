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
        Thread readReportThread, writeReportThread, showNodesThread;
        WriteReport writeReport;
        public static Form1 form = null;
        Fw16.EcrCtrl ecrCtrl;
        public Form1()
        {
            InitializeComponent();
            Form1.form = this;
            form.Text = Program.nameProgram;
            TV_TreeTags.TreeViewNodeSorter = new TreeSorter();
            OpenFD_binFile.InitialDirectory = Application.StartupPath;
            Bind();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (writeReportThread?.IsAlive ?? false)
            {
                writeReportThread?.Abort();
                writeReportThread.Join();
                (writeReport.ecrCtrl as IDisposable).Dispose();
                writeReport.fileStream?.Close();
                writeReport.writer?.Close();
            }
            if (readReportThread?.IsAlive ?? false)
            {
                readReportThread?.Abort();
                readReportThread.Join();
            }
        }

        public void UpdateProgressBar(int val, int max = 100)
        {
            progressBar1.Maximum = max;
            if (val <= max)
            {
                progressBar1.Value = val;
            }
            else
            {
                progressBar1.Value = max;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            (ecrCtrl as IDisposable)?.Dispose();
        }

        private void Bind()
        {
            TB_Name.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.Name)));
            TB_Program.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.Program)));
            TB_NumberECR.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.NumberECR)));
            TB_NumberFS.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.NumberFS)));
            TB_VersionFFD.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.VersionFFD)));
            TB_CountShift.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.CountShift)));
            TB_CountFiscalDoc.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.CountFiscalDoc)));
            TB_Hesh.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.Hash)));
        }

        public void ReadStats()
        {
            TB_CorrectionIncomeCount.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.correctionIncomeCount].ToString();
            TB_CorrectionIncomeSum.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.correctionIncomeSum].ToString();
            TB_CorrectionOutcomeCount.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.correctionOutcomeCount].ToString();
            TB_CorrectionOutcomeSum.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.correctionOutcomeSum].ToString();
            TB_IncomeBackCount.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.incomeBackCount].ToString();
            TB_IncomeBackSum.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.incomeBackSum].ToString();
            TB_IncomeCount.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.incomeCount].ToString();
            TB_IncomeSum.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.incomeSum].ToString();
            TB_OutcomeBackCount.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.outcomeBackCount].ToString();
            TB_OutcomeBackSum.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.outcomeBackSum].ToString();
            TB_OutcomeCount.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.outcomeCount].ToString();
            TB_OutcomeSum.Text = Program.reportFNS.treeOfTags.Stat[ReportFNS.TreeOfTags.Statistic.StatsName.outcomeSum].ToString();
        }

        ////////////////////////////////////////////////////////////////

        private void B_Browse_Click(object sender, EventArgs e)
        {
            if (OpenFD_binFile.ShowDialog() == DialogResult.OK)
                TB_Patch.Text = OpenFD_binFile.FileName;
        }

        public void B_UpdateStop_Click(object sender, EventArgs e)
        {
            if (readReportThread?.IsAlive ?? false)
            {
                readReportThread?.Abort();
                readReportThread.Join();
                B_UpdateStop.Text = "Обновить";
            }
            else
            {
                try
                {
                    if (showNodesThread?.IsAlive ?? false)
                    {
                        showNodesThread.Abort();
                        showNodesThread.Join();
                        B_ShowNodes.Text = "Отобразить";
                    }
                    B_ShowNodes.Enabled = false;
                    readReportThread = new Thread((ThreadStart)delegate
                    {
                        try
                        {
                            Program.reportReader.UpdateData(TB_Patch.Text);
                            if (Program.reportFNS.treeOfTags.CountDocs > 0)
                            {
                                Form1.form.Invoke((MethodInvoker)delegate
                                {
                                    B_ShowNodes.Enabled = true;
                                    NUD_EndNumberDoc.Maximum = NUD_StartNumberDoc.Maximum = Program.reportFNS.treeOfTags.CountDocs;
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            B_ShowNodes.Enabled = false;
                            MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        Form1.form.Invoke((MethodInvoker)delegate
                        {
                            B_UpdateStop.Text = "Обновить";
                            TV_TreeTags.Nodes.Clear();
                        });
                    });
                    readReportThread.Start();
                    B_UpdateStop.Text = "Остановить";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void B_ShowNodes_Click(object sender, EventArgs e)
        {
            if (showNodesThread?.IsAlive ?? false)
            {
                showNodesThread?.Abort();
                showNodesThread.Join();
                B_ShowNodes.Text = "Отобразить";
            }
            else
            {
                try
                {
                    TV_TreeTags.Nodes.Clear();
                    UInt32 _start = (UInt32)NUD_StartNumberDoc.Value;
                    UInt32 _end = (UInt32)NUD_EndNumberDoc.Value;
                    if (_start <= _end && _start != 0 && _end != 0)
                    {
                        _start--;
                        _end--;
                        showNodesThread = new Thread((ThreadStart)delegate
                        {
                            if (!Program.reportReader.GetNodes(_start, _end))
                            {
                                Form1.form.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show("Не удалось обновить дерево тегов согласно переданным параметрам.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                });
                            }
                            Form1.form.Invoke((MethodInvoker)delegate
                            {
                                B_ShowNodes.Text = "Отобразить";
                                UpdateProgressBar(0);
                            });
                        });
                        showNodesThread.Start();
                        B_ShowNodes.Text = "Остановить";
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста скорректируйте значения номеров \nдокументов которые вы хотите отобразить.\n - Начальное значение должно быть меньше конечного.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        /////////////////////////////////////////////////////////////////////////////////

        private void B_startParse_Click(object sender, EventArgs e)
        {
            if (writeReportThread?.IsAlive ?? false)
            {
                writeReportThread?.Abort();
                writeReportThread.Join();
                (writeReport.ecrCtrl as IDisposable).Dispose();
                writeReport.fileStream?.Close();
                writeReport.writer?.Close();
                progressBar1.Value = 0;

                B_startParse.Text = "Формировать отчет";
            }
            else
            {
                ecrCtrl = new Fw16.EcrCtrl();
                if (ConnectToFW(CB_Port.Text))
                {
                    try
                    {
                        writeReport = new WriteReport(ecrCtrl, TB_fileWay.Text, TB_fileName.ForeColor == SystemColors.ActiveCaption ? "" : TB_fileName.Text);

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
        }

        /// <summary>
        /// подключение к ККТ. обырв соеденения с ККТ происходит в "форме" при её закрытии.
        /// </summary>
        /// <param name="serialPort">serialPort</param>
        /// <param name="baudRate">частота</param>
        bool ConnectToFW(string name = "default")
        {
            try
            {
                ecrCtrl.Init(name);             //Подключчение по порту и частоте
                return true;
            }
            catch
            {
                MessageBox.Show("Ошибка при подключении", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
            if (TB_fileName.Text == "По умолчанию" && TB_fileName.ForeColor == SystemColors.ActiveCaption)
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
