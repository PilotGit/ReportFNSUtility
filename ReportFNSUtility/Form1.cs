using System;
using System.Collections;
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
using static ReportFNSUtility.ReportFNS.TreeOfTags.Statistic;

namespace ReportFNSUtility
{
    public partial class Form1 : Form
    {
        Thread readReportThread, writeReportThread, showNodesThread;
        public Thread computeStats;
        WriteReport writeReport;
        public static Form1 form = null;
        Fw16.EcrCtrl ecrCtrl;
        public Form1()
        {
            InitializeComponent();
            form = this;
            this.Text = Program.nameProgram;
            TV_TreeTags.TreeViewNodeSorter = new TreeSorter();
            OpenFD_binFile.InitialDirectory = Application.StartupPath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.reportFNS.treeOfTags.StopComputeStats();

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

        public void ReadHeader()
        {
            var _header = Program.reportFNS.reportHeader;
            TB_Name.Text = _header.Name;
            TB_Program.Text = _header.NameProgram;
            TB_NumberECR.Text = _header.NumberECR;
            TB_NumberFS.Text = _header.NumberFS;
            TB_VersionFFD.Text = _header.VersionFFD.ToString();
            TB_CountShift.Text = _header.CountShift.ToString();
            TB_CountFiscalDoc.Text = _header.CountFiscalDoc.ToString();
            TB_Hash.Text = _header.Hash.ToString("X");
        }
        private void ClearHeder()
        {
            TB_Name.Text = "";
            TB_Program.Text = "";
            TB_NumberECR.Text = "";
            TB_NumberFS.Text = "";
            TB_VersionFFD.Text = "";
            TB_CountShift.Text = "";
            TB_CountFiscalDoc.Text = "";
            TB_Hash.Text = "";
        }
        public void ReadStats()
        {
            var _stat = Program.reportFNS.treeOfTags.stat;
            TB_CorrectionIncomeCount.Text = _stat[StatsName.correctionIncomeCount].ToString();
            TB_CorrectionIncomeSum.Text = _stat[StatsName.correctionIncomeSum].ToString("0.00");
            TB_CorrectionOutcomeCount.Text = _stat[StatsName.correctionOutcomeCount].ToString();
            TB_CorrectionOutcomeSum.Text = _stat[StatsName.correctionOutcomeSum].ToString("0.00");
            TB_IncomeBackCount.Text = _stat[StatsName.incomeBackCount].ToString();
            TB_IncomeBackSum.Text = _stat[StatsName.incomeBackSum].ToString("0.00");
            TB_IncomeCount.Text = _stat[StatsName.incomeCount].ToString();
            TB_IncomeSum.Text = _stat[StatsName.incomeSum].ToString("0.00");
            TB_OutcomeBackCount.Text = _stat[StatsName.outcomeBackCount].ToString();
            TB_OutcomeBackSum.Text = _stat[StatsName.outcomeBackSum].ToString("0.00");
            TB_OutcomeCount.Text = _stat[StatsName.outcomeCount].ToString();
            TB_OutcomeSum.Text = _stat[StatsName.outcomeSum].ToString("0.00");
        }

        ////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработка нажатия на кнопку "обзор". Вывод окна выбора файла.
        /// </summary>
        private void B_Browse_Click(object sender, EventArgs e)
        {
            if (OpenFD_binFile.ShowDialog() == DialogResult.OK)
                TB_Patch.Text = OpenFD_binFile.FileName;
        }
        /// <summary>
        /// Запускает или останавливает считывание файла в зависисмости от состояния потока.
        /// </summary>
        public void B_UpdateStop_Click(object sender, EventArgs e)
        {
            if ((readReportThread?.IsAlive ?? false) || (Program.reportFNS.treeOfTags.computeStats?.IsAlive ?? false))
            {
                Program.reportFNS.treeOfTags.StopComputeStats();
                if (readReportThread?.IsAlive ?? false)
                {
                    readReportThread?.Abort();
                    readReportThread.Join();
                }
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
                    ClearHeder();
                    TV_TreeTags.Nodes.Clear();
                    readReportThread = new Thread((ThreadStart)delegate
                    {
                        try
                        {
                            Program.reportFNS.treeOfTags.StopComputeStats();
                            Program.reportReader.UpdateData(TB_Patch.Text);
                            if (Program.reportFNS.treeOfTags.CountDocs > 0)
                            {
                                Program.form?.Invoke((MethodInvoker)delegate
                                {
                                    B_ShowNodes.Enabled = true;
                                    NUD_EndNumberDoc.Maximum = NUD_StartNumberDoc.Maximum = Program.reportFNS.treeOfTags.CountDocs;
                                });
                            }
                            Program.form?.Invoke((MethodInvoker)delegate { ReadHeader(); });
                        }
                        catch (Exception ex)
                        {
                            Program.form?.Invoke((MethodInvoker)delegate
                            {
                                B_ShowNodes.Enabled = false;
                                if (!(ex is ThreadAbortException))
                                    MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                B_UpdateStop.Text = "Обновить";
                            });
                        }
                    });
                    readReportThread.Start();
                    B_UpdateStop.Text = "Остановить";
                }
                catch (Exception ex)
                {
                    if (!(ex is ThreadAbortException))
                        MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Запускает или останавливает заполнение дерева в зависимости от состояния потока.
        /// </summary>
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
                                Program.form?.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show("Не удалось обновить дерево тегов согласно переданным параметрам.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                });
                            }
                            Program.form?.Invoke((MethodInvoker)delegate
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
                        MessageBox.Show("Пожалуйста скорректируйте значения номеров \nдокументов, которые вы хотите отобразить.\n - Начальное значение должно быть меньше конечного.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

        private void NUD_Validating(object sender, CancelEventArgs e)
        {
            if((sender as Control).Text == "") {
                (sender as NumericUpDown).Value= (sender as NumericUpDown).Minimum;
                (sender as Control).Text = (sender as NumericUpDown).Minimum.ToString();
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

    class TreeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if (ty.Parent == null)
            {
                return -1;
            }
            return string.Compare(tx.Text, ty.Text);
        }
    }
}
