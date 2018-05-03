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
        ReadReport readReport;
        WriteReport writeReport;
        public static Form1 form = null;
        Fw16.EcrCtrl ecrCtrl;
        public Form1()
        {
            InitializeComponent();
            Form1.form = this;
            form.Text = Program.nameProgram;
            treeView1.TreeViewNodeSorter = new TreeSorter();
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
            TB_IncomeCount.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.IncomeCount)));
            TB_IncomeSum.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.IncomeSum)));
            TB_IncomeBackSum.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.IncomeBackSum)));
            TB_IncomeBackCount.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.IncomeBackCount)));
            TB_OutcomeBackCount.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.OutcomeBackCount)));
            TB_OutcomeBackSum.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.OutcomeBackSum)));
            TB_OutcomeCount.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.OutcomeCount)));
            TB_OutcomeSum.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.OutcomeSum)));
            TB_CorrectionIncomeCount.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.CorrectionIncomeCount)));
            TB_CorrectionIncomeSum.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.CorrectionIncomeSum)));
            TB_CorrectionOutcomeCount.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.CorrectionOutcomeCount)));
            TB_CorrectionOutcomeSum.DataBindings.Add(new Binding("Text", Program.reportFNS.treeOfTags, nameof(Program.reportFNS.treeOfTags.CorrectionOutcomeSum)));

            TB_Name.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.Name)));
            TB_Program.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.Program)));
            TB_NumberECR.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.NumberECR)));
            TB_NumberFS.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.NumberFS)));
            TB_VersionFFD.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.VersionFFD)));
            TB_CountShift.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.CountShift)));
            TB_CountFiscalDoc.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.CountFiscalDoc)));
            TB_Hesh.DataBindings.Add(new Binding("Text", Program.reportFNS.reportHeader, nameof(Program.reportFNS.reportHeader.Hash)));
        }

        ////////////////////////////////////////////////////////////////

        private void B_Browse_Click(object sender, EventArgs e)
        {
            if (OpenFD_binFile.ShowDialog() == DialogResult.OK)
                TB_Patch.Text = OpenFD_binFile.FileName;
        }

        public void B_UpdateStop_Click(object sender, EventArgs e)
        {
            //if (readReportThread?.IsAlive ?? false)
            //{
            //    readReportThread?.Abort();
            //    readReportThread.Join();
            //    B_UpdateStop.Text = "Обновить";
            //}
            //else
            //{
            //    try
            //    {
            //        readReport?.reader?.BaseStream?.Close();
            //        treeView1.Nodes.Clear();
            //        readReport = new ReadReport(TB_Patch.Text);

            //        readReportThread = new Thread((ThreadStart)delegate { readReport.Read(); });
            //        readReportThread.Start();
            //        B_UpdateStop.Text = "Остановить";
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
            //////
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
                    treeView1.Nodes.Clear();
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
                            UpdateProgressBar(0);
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
                    treeView1.Nodes.Clear();
                    UInt32 _start = (UInt32)NUD_StartNumberDoc.Value;
                    UInt32 _end = (UInt32)NUD_EndNumberDoc.Value;
                    if (_start <= _end && _start != 0 && _end != 0)
                    {
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
                        readReport?.reader?.BaseStream?.Close();
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
