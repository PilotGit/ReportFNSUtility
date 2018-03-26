namespace ReportFNSUtility
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.L_fileWay = new System.Windows.Forms.Label();
            this.L_fileName = new System.Windows.Forms.Label();
            this.TB_fileName = new System.Windows.Forms.TextBox();
            this.B_fileWayDialog = new System.Windows.Forms.Button();
            this.TB_fileWay = new System.Windows.Forms.TextBox();
            this.B_startParse = new System.Windows.Forms.Button();
            this.L_Port = new System.Windows.Forms.Label();
            this.CB_Port = new System.Windows.Forms.ComboBox();
            this.GB_PreviewReport = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.B_UpdateStop = new System.Windows.Forms.Button();
            this.B_Browse = new System.Windows.Forms.Button();
            this.TB_Patch = new System.Windows.Forms.TextBox();
            this.OpenFD_binFile = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.T_page_report_Generation = new System.Windows.Forms.TabPage();
            this.T_page_headInfo = new System.Windows.Forms.TabPage();
            this.TB_8_CheckSum = new System.Windows.Forms.TextBox();
            this.L_8_CheckSum = new System.Windows.Forms.Label();
            this.TB_7_NumberOfFiscalDOC = new System.Windows.Forms.TextBox();
            this.L_7_NumberOfFiscalDOC = new System.Windows.Forms.Label();
            this.TB_6_NumberOfShifts = new System.Windows.Forms.TextBox();
            this.L_6_NumberOfShifts = new System.Windows.Forms.Label();
            this.TB_5_NumberFFD = new System.Windows.Forms.TextBox();
            this.L_5_NumberFFD = new System.Windows.Forms.Label();
            this.TB_4_NumberFN = new System.Windows.Forms.TextBox();
            this.L_4_NumberFN = new System.Windows.Forms.Label();
            this.TB_3_RegNumber = new System.Windows.Forms.TextBox();
            this.L_3_RegNumber = new System.Windows.Forms.Label();
            this.TB_2_UnloadingProgram = new System.Windows.Forms.TextBox();
            this.L_2_UnloadingProgram = new System.Windows.Forms.Label();
            this.TB_1_saveFile = new System.Windows.Forms.TextBox();
            this.L_1_saveFile = new System.Windows.Forms.Label();
            this.GB_PreviewReport.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.T_page_report_Generation.SuspendLayout();
            this.T_page_headInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // L_fileWay
            // 
            this.L_fileWay.AutoSize = true;
            this.L_fileWay.Location = new System.Drawing.Point(12, 111);
            this.L_fileWay.Name = "L_fileWay";
            this.L_fileWay.Size = new System.Drawing.Size(74, 13);
            this.L_fileWay.TabIndex = 17;
            this.L_fileWay.Text = "Путь к файлу";
            // 
            // L_fileName
            // 
            this.L_fileName.AutoSize = true;
            this.L_fileName.Location = new System.Drawing.Point(12, 67);
            this.L_fileName.Name = "L_fileName";
            this.L_fileName.Size = new System.Drawing.Size(64, 13);
            this.L_fileName.TabIndex = 16;
            this.L_fileName.Text = "Имя файла";
            // 
            // TB_fileName
            // 
            this.TB_fileName.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.TB_fileName.Location = new System.Drawing.Point(15, 83);
            this.TB_fileName.Name = "TB_fileName";
            this.TB_fileName.Size = new System.Drawing.Size(184, 20);
            this.TB_fileName.TabIndex = 14;
            this.TB_fileName.Text = "По умолчанию";
            this.TB_fileName.Enter += new System.EventHandler(this.TB_fileName_Enter);
            this.TB_fileName.Leave += new System.EventHandler(this.TB_fileName_Leave);
            // 
            // B_fileWayDialog
            // 
            this.B_fileWayDialog.Location = new System.Drawing.Point(205, 127);
            this.B_fileWayDialog.Name = "B_fileWayDialog";
            this.B_fileWayDialog.Size = new System.Drawing.Size(30, 20);
            this.B_fileWayDialog.TabIndex = 13;
            this.B_fileWayDialog.Text = "🔍";
            this.B_fileWayDialog.UseVisualStyleBackColor = true;
            this.B_fileWayDialog.Click += new System.EventHandler(this.B_fileWayDialog_Click);
            // 
            // TB_fileWay
            // 
            this.TB_fileWay.Location = new System.Drawing.Point(15, 127);
            this.TB_fileWay.Name = "TB_fileWay";
            this.TB_fileWay.Size = new System.Drawing.Size(184, 20);
            this.TB_fileWay.TabIndex = 12;
            // 
            // B_startParse
            // 
            this.B_startParse.Location = new System.Drawing.Point(15, 148);
            this.B_startParse.Name = "B_startParse";
            this.B_startParse.Size = new System.Drawing.Size(220, 23);
            this.B_startParse.TabIndex = 11;
            this.B_startParse.Text = "Формировать отчет";
            this.B_startParse.UseVisualStyleBackColor = true;
            this.B_startParse.Click += new System.EventHandler(this.B_startParse_Click);
            // 
            // L_Port
            // 
            this.L_Port.AutoSize = true;
            this.L_Port.Location = new System.Drawing.Point(12, 20);
            this.L_Port.Name = "L_Port";
            this.L_Port.Size = new System.Drawing.Size(153, 13);
            this.L_Port.TabIndex = 2;
            this.L_Port.Text = "Наименование подключения";
            // 
            // CB_Port
            // 
            this.CB_Port.AutoCompleteCustomSource.AddRange(new string[] {
            "default"});
            this.CB_Port.FormattingEnabled = true;
            this.CB_Port.Items.AddRange(new object[] {
            "default"});
            this.CB_Port.Location = new System.Drawing.Point(15, 43);
            this.CB_Port.Name = "CB_Port";
            this.CB_Port.Size = new System.Drawing.Size(223, 21);
            this.CB_Port.TabIndex = 0;
            this.CB_Port.Text = "default";
            // 
            // GB_PreviewReport
            // 
            this.GB_PreviewReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_PreviewReport.Controls.Add(this.progressBar1);
            this.GB_PreviewReport.Controls.Add(this.treeView1);
            this.GB_PreviewReport.Controls.Add(this.B_UpdateStop);
            this.GB_PreviewReport.Controls.Add(this.B_Browse);
            this.GB_PreviewReport.Controls.Add(this.TB_Patch);
            this.GB_PreviewReport.Location = new System.Drawing.Point(286, 12);
            this.GB_PreviewReport.Name = "GB_PreviewReport";
            this.GB_PreviewReport.Size = new System.Drawing.Size(717, 441);
            this.GB_PreviewReport.TabIndex = 1;
            this.GB_PreviewReport.TabStop = false;
            this.GB_PreviewReport.Text = "Просмотр отчёта";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 414);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(698, 21);
            this.progressBar1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(7, 46);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(697, 362);
            this.treeView1.Sorted = true;
            this.treeView1.TabIndex = 4;
            // 
            // B_UpdateStop
            // 
            this.B_UpdateStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_UpdateStop.Location = new System.Drawing.Point(629, 17);
            this.B_UpdateStop.Name = "B_UpdateStop";
            this.B_UpdateStop.Size = new System.Drawing.Size(75, 23);
            this.B_UpdateStop.TabIndex = 3;
            this.B_UpdateStop.Text = "Обновить";
            this.B_UpdateStop.UseVisualStyleBackColor = true;
            this.B_UpdateStop.Click += new System.EventHandler(this.B_UpdateStop_Click);
            // 
            // B_Browse
            // 
            this.B_Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Browse.Location = new System.Drawing.Point(548, 17);
            this.B_Browse.Name = "B_Browse";
            this.B_Browse.Size = new System.Drawing.Size(75, 23);
            this.B_Browse.TabIndex = 1;
            this.B_Browse.Text = "Обзор";
            this.B_Browse.UseVisualStyleBackColor = true;
            this.B_Browse.Click += new System.EventHandler(this.B_Browse_Click);
            // 
            // TB_Patch
            // 
            this.TB_Patch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Patch.Location = new System.Drawing.Point(6, 19);
            this.TB_Patch.Name = "TB_Patch";
            this.TB_Patch.Size = new System.Drawing.Size(536, 20);
            this.TB_Patch.TabIndex = 0;
            // 
            // OpenFD_binFile
            // 
            this.OpenFD_binFile.DefaultExt = "bin";
            this.OpenFD_binFile.Filter = "Бинарный файл|*.bin; *.fnc";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.T_page_report_Generation);
            this.tabControl1.Controls.Add(this.T_page_headInfo);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(255, 441);
            this.tabControl1.TabIndex = 2;
            // 
            // T_page_report_Generation
            // 
            this.T_page_report_Generation.Controls.Add(this.L_fileWay);
            this.T_page_report_Generation.Controls.Add(this.CB_Port);
            this.T_page_report_Generation.Controls.Add(this.L_fileName);
            this.T_page_report_Generation.Controls.Add(this.TB_fileName);
            this.T_page_report_Generation.Controls.Add(this.L_Port);
            this.T_page_report_Generation.Controls.Add(this.B_fileWayDialog);
            this.T_page_report_Generation.Controls.Add(this.TB_fileWay);
            this.T_page_report_Generation.Controls.Add(this.B_startParse);
            this.T_page_report_Generation.Location = new System.Drawing.Point(4, 22);
            this.T_page_report_Generation.Name = "T_page_report_Generation";
            this.T_page_report_Generation.Padding = new System.Windows.Forms.Padding(3);
            this.T_page_report_Generation.Size = new System.Drawing.Size(247, 415);
            this.T_page_report_Generation.TabIndex = 0;
            this.T_page_report_Generation.Text = "Формирование отчета";
            // 
            // T_page_headInfo
            // 
            this.T_page_headInfo.Controls.Add(this.TB_8_CheckSum);
            this.T_page_headInfo.Controls.Add(this.L_8_CheckSum);
            this.T_page_headInfo.Controls.Add(this.TB_7_NumberOfFiscalDOC);
            this.T_page_headInfo.Controls.Add(this.L_7_NumberOfFiscalDOC);
            this.T_page_headInfo.Controls.Add(this.TB_6_NumberOfShifts);
            this.T_page_headInfo.Controls.Add(this.L_6_NumberOfShifts);
            this.T_page_headInfo.Controls.Add(this.TB_5_NumberFFD);
            this.T_page_headInfo.Controls.Add(this.L_5_NumberFFD);
            this.T_page_headInfo.Controls.Add(this.TB_4_NumberFN);
            this.T_page_headInfo.Controls.Add(this.L_4_NumberFN);
            this.T_page_headInfo.Controls.Add(this.TB_3_RegNumber);
            this.T_page_headInfo.Controls.Add(this.L_3_RegNumber);
            this.T_page_headInfo.Controls.Add(this.TB_2_UnloadingProgram);
            this.T_page_headInfo.Controls.Add(this.L_2_UnloadingProgram);
            this.T_page_headInfo.Controls.Add(this.TB_1_saveFile);
            this.T_page_headInfo.Controls.Add(this.L_1_saveFile);
            this.T_page_headInfo.Location = new System.Drawing.Point(4, 22);
            this.T_page_headInfo.Name = "T_page_headInfo";
            this.T_page_headInfo.Padding = new System.Windows.Forms.Padding(3);
            this.T_page_headInfo.Size = new System.Drawing.Size(247, 407);
            this.T_page_headInfo.TabIndex = 1;
            this.T_page_headInfo.Text = "Отчет о считывании";
            // 
            // TB_8_CheckSum
            // 
            this.TB_8_CheckSum.BackColor = System.Drawing.Color.LightGray;
            this.TB_8_CheckSum.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_8_CheckSum.Location = new System.Drawing.Point(4, 302);
            this.TB_8_CheckSum.Name = "TB_8_CheckSum";
            this.TB_8_CheckSum.ReadOnly = true;
            this.TB_8_CheckSum.Size = new System.Drawing.Size(240, 20);
            this.TB_8_CheckSum.TabIndex = 15;
            // 
            // L_8_CheckSum
            // 
            this.L_8_CheckSum.AutoSize = true;
            this.L_8_CheckSum.Location = new System.Drawing.Point(7, 285);
            this.L_8_CheckSum.Name = "L_8_CheckSum";
            this.L_8_CheckSum.Size = new System.Drawing.Size(109, 13);
            this.L_8_CheckSum.TabIndex = 14;
            this.L_8_CheckSum.Text = "Контрольная сумма";
            // 
            // TB_7_NumberOfFiscalDOC
            // 
            this.TB_7_NumberOfFiscalDOC.BackColor = System.Drawing.Color.LightGray;
            this.TB_7_NumberOfFiscalDOC.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_7_NumberOfFiscalDOC.Location = new System.Drawing.Point(4, 262);
            this.TB_7_NumberOfFiscalDOC.Name = "TB_7_NumberOfFiscalDOC";
            this.TB_7_NumberOfFiscalDOC.ReadOnly = true;
            this.TB_7_NumberOfFiscalDOC.Size = new System.Drawing.Size(240, 20);
            this.TB_7_NumberOfFiscalDOC.TabIndex = 13;
            // 
            // L_7_NumberOfFiscalDOC
            // 
            this.L_7_NumberOfFiscalDOC.AutoSize = true;
            this.L_7_NumberOfFiscalDOC.Location = new System.Drawing.Point(7, 245);
            this.L_7_NumberOfFiscalDOC.Name = "L_7_NumberOfFiscalDOC";
            this.L_7_NumberOfFiscalDOC.Size = new System.Drawing.Size(195, 13);
            this.L_7_NumberOfFiscalDOC.TabIndex = 12;
            this.L_7_NumberOfFiscalDOC.Text = "Количество фискальных документов";
            // 
            // TB_6_NumberOfShifts
            // 
            this.TB_6_NumberOfShifts.BackColor = System.Drawing.Color.LightGray;
            this.TB_6_NumberOfShifts.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_6_NumberOfShifts.Location = new System.Drawing.Point(4, 222);
            this.TB_6_NumberOfShifts.Name = "TB_6_NumberOfShifts";
            this.TB_6_NumberOfShifts.ReadOnly = true;
            this.TB_6_NumberOfShifts.Size = new System.Drawing.Size(240, 20);
            this.TB_6_NumberOfShifts.TabIndex = 11;
            // 
            // L_6_NumberOfShifts
            // 
            this.L_6_NumberOfShifts.AutoSize = true;
            this.L_6_NumberOfShifts.Location = new System.Drawing.Point(7, 205);
            this.L_6_NumberOfShifts.Name = "L_6_NumberOfShifts";
            this.L_6_NumberOfShifts.Size = new System.Drawing.Size(95, 13);
            this.L_6_NumberOfShifts.TabIndex = 10;
            this.L_6_NumberOfShifts.Text = "Количество смен";
            // 
            // TB_5_NumberFFD
            // 
            this.TB_5_NumberFFD.BackColor = System.Drawing.Color.LightGray;
            this.TB_5_NumberFFD.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_5_NumberFFD.Location = new System.Drawing.Point(4, 181);
            this.TB_5_NumberFFD.Name = "TB_5_NumberFFD";
            this.TB_5_NumberFFD.ReadOnly = true;
            this.TB_5_NumberFFD.Size = new System.Drawing.Size(240, 20);
            this.TB_5_NumberFFD.TabIndex = 9;
            // 
            // L_5_NumberFFD
            // 
            this.L_5_NumberFFD.AutoSize = true;
            this.L_5_NumberFFD.Location = new System.Drawing.Point(7, 165);
            this.L_5_NumberFFD.Name = "L_5_NumberFFD";
            this.L_5_NumberFFD.Size = new System.Drawing.Size(117, 13);
            this.L_5_NumberFFD.TabIndex = 8;
            this.L_5_NumberFFD.Text = "Номер версии ФФД ";
            // 
            // TB_4_NumberFN
            // 
            this.TB_4_NumberFN.BackColor = System.Drawing.Color.LightGray;
            this.TB_4_NumberFN.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_4_NumberFN.Location = new System.Drawing.Point(4, 141);
            this.TB_4_NumberFN.Name = "TB_4_NumberFN";
            this.TB_4_NumberFN.ReadOnly = true;
            this.TB_4_NumberFN.Size = new System.Drawing.Size(240, 20);
            this.TB_4_NumberFN.TabIndex = 7;
            // 
            // L_4_NumberFN
            // 
            this.L_4_NumberFN.AutoSize = true;
            this.L_4_NumberFN.Location = new System.Drawing.Point(7, 125);
            this.L_4_NumberFN.Name = "L_4_NumberFN";
            this.L_4_NumberFN.Size = new System.Drawing.Size(63, 13);
            this.L_4_NumberFN.TabIndex = 6;
            this.L_4_NumberFN.Text = "Номер ФН";
            // 
            // TB_3_RegNumber
            // 
            this.TB_3_RegNumber.BackColor = System.Drawing.Color.LightGray;
            this.TB_3_RegNumber.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_3_RegNumber.Location = new System.Drawing.Point(4, 102);
            this.TB_3_RegNumber.Name = "TB_3_RegNumber";
            this.TB_3_RegNumber.ReadOnly = true;
            this.TB_3_RegNumber.Size = new System.Drawing.Size(240, 20);
            this.TB_3_RegNumber.TabIndex = 5;
            // 
            // L_3_RegNumber
            // 
            this.L_3_RegNumber.AutoSize = true;
            this.L_3_RegNumber.Location = new System.Drawing.Point(7, 86);
            this.L_3_RegNumber.Name = "L_3_RegNumber";
            this.L_3_RegNumber.Size = new System.Drawing.Size(157, 13);
            this.L_3_RegNumber.TabIndex = 4;
            this.L_3_RegNumber.Text = "Регистрационный номер ККТ";
            // 
            // TB_2_UnloadingProgram
            // 
            this.TB_2_UnloadingProgram.BackColor = System.Drawing.Color.LightGray;
            this.TB_2_UnloadingProgram.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_2_UnloadingProgram.Location = new System.Drawing.Point(4, 63);
            this.TB_2_UnloadingProgram.Name = "TB_2_UnloadingProgram";
            this.TB_2_UnloadingProgram.ReadOnly = true;
            this.TB_2_UnloadingProgram.Size = new System.Drawing.Size(240, 20);
            this.TB_2_UnloadingProgram.TabIndex = 3;
            // 
            // L_2_UnloadingProgram
            // 
            this.L_2_UnloadingProgram.AutoSize = true;
            this.L_2_UnloadingProgram.Location = new System.Drawing.Point(7, 47);
            this.L_2_UnloadingProgram.Name = "L_2_UnloadingProgram";
            this.L_2_UnloadingProgram.Size = new System.Drawing.Size(120, 13);
            this.L_2_UnloadingProgram.TabIndex = 2;
            this.L_2_UnloadingProgram.Text = "Программа выгрузки ";
            // 
            // TB_1_saveFile
            // 
            this.TB_1_saveFile.BackColor = System.Drawing.Color.LightGray;
            this.TB_1_saveFile.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TB_1_saveFile.Location = new System.Drawing.Point(4, 24);
            this.TB_1_saveFile.Name = "TB_1_saveFile";
            this.TB_1_saveFile.ReadOnly = true;
            this.TB_1_saveFile.Size = new System.Drawing.Size(240, 20);
            this.TB_1_saveFile.TabIndex = 1;
            // 
            // L_1_saveFile
            // 
            this.L_1_saveFile.AutoSize = true;
            this.L_1_saveFile.Location = new System.Drawing.Point(7, 7);
            this.L_1_saveFile.Name = "L_1_saveFile";
            this.L_1_saveFile.Size = new System.Drawing.Size(128, 13);
            this.L_1_saveFile.TabIndex = 0;
            this.L_1_saveFile.Text = "Путь сохранения файла";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 465);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.GB_PreviewReport);
            this.Name = "Form1";
            this.Text = "ReportFNSUtility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.GB_PreviewReport.ResumeLayout(false);
            this.GB_PreviewReport.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.T_page_report_Generation.ResumeLayout(false);
            this.T_page_report_Generation.PerformLayout();
            this.T_page_headInfo.ResumeLayout(false);
            this.T_page_headInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label L_Port;
        private System.Windows.Forms.ComboBox CB_Port;
        private System.Windows.Forms.Button B_Browse;
        private System.Windows.Forms.OpenFileDialog OpenFD_binFile;
        public System.Windows.Forms.TreeView treeView1;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label L_fileWay;
        private System.Windows.Forms.Label L_fileName;
        private System.Windows.Forms.TextBox TB_fileName;
        private System.Windows.Forms.Button B_fileWayDialog;
        private System.Windows.Forms.TextBox TB_fileWay;
        public System.Windows.Forms.GroupBox GB_PreviewReport;
        public System.Windows.Forms.Button B_startParse;
        public System.Windows.Forms.Button B_UpdateStop;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage T_page_report_Generation;
        private System.Windows.Forms.Label L_1_saveFile;
        private System.Windows.Forms.Label L_8_CheckSum;
        private System.Windows.Forms.Label L_7_NumberOfFiscalDOC;
        private System.Windows.Forms.Label L_6_NumberOfShifts;
        private System.Windows.Forms.Label L_5_NumberFFD;
        private System.Windows.Forms.Label L_4_NumberFN;
        private System.Windows.Forms.Label L_3_RegNumber;
        private System.Windows.Forms.Label L_2_UnloadingProgram;
        public System.Windows.Forms.TabPage T_page_headInfo;
        public System.Windows.Forms.TextBox TB_1_saveFile;
        public System.Windows.Forms.TextBox TB_8_CheckSum;
        public System.Windows.Forms.TextBox TB_7_NumberOfFiscalDOC;
        public System.Windows.Forms.TextBox TB_6_NumberOfShifts;
        public System.Windows.Forms.TextBox TB_5_NumberFFD;
        public System.Windows.Forms.TextBox TB_4_NumberFN;
        public System.Windows.Forms.TextBox TB_3_RegNumber;
        public System.Windows.Forms.TextBox TB_2_UnloadingProgram;
        public System.Windows.Forms.TextBox TB_Patch;
    }
}

