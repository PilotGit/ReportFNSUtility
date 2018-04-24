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
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.GB_PreviewReport.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.T_page_report_Generation.SuspendLayout();
            this.T_page_headInfo.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
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
            this.GB_PreviewReport.Controls.Add(this.tabControl2);
            this.GB_PreviewReport.Controls.Add(this.progressBar1);
            this.GB_PreviewReport.Controls.Add(this.B_UpdateStop);
            this.GB_PreviewReport.Controls.Add(this.B_Browse);
            this.GB_PreviewReport.Controls.Add(this.TB_Patch);
            this.GB_PreviewReport.Location = new System.Drawing.Point(286, 12);
            this.GB_PreviewReport.Name = "GB_PreviewReport";
            this.GB_PreviewReport.Size = new System.Drawing.Size(865, 480);
            this.GB_PreviewReport.TabIndex = 1;
            this.GB_PreviewReport.TabStop = false;
            this.GB_PreviewReport.Text = "Просмотр отчёта";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 453);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(846, 21);
            this.progressBar1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(15, 37);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(803, 320);
            this.treeView1.Sorted = true;
            this.treeView1.TabIndex = 4;
            // 
            // B_UpdateStop
            // 
            this.B_UpdateStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_UpdateStop.Location = new System.Drawing.Point(777, 17);
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
            this.B_Browse.Location = new System.Drawing.Point(696, 17);
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
            this.TB_Patch.Size = new System.Drawing.Size(684, 20);
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
            this.tabControl1.Size = new System.Drawing.Size(255, 480);
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
            this.T_page_report_Generation.Size = new System.Drawing.Size(247, 454);
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
            this.T_page_headInfo.Size = new System.Drawing.Size(247, 415);
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
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Location = new System.Drawing.Point(6, 46);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(846, 401);
            this.tabControl2.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.numericUpDown2);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(838, 375);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Дерево";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(838, 375);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Общая информация";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 357);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Чеки";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBox9);
            this.groupBox2.Controls.Add(this.textBox10);
            this.groupBox2.Controls.Add(this.textBox12);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox11);
            this.groupBox2.Location = new System.Drawing.Point(416, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 357);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Чеки коррекции";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(118, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Приход";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Возврат прихода";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(118, 77);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Расход";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(118, 103);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Возврат расхода";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(118, 129);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 6;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(241, 129);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 11;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(241, 103);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 10;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(241, 77);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 9;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(241, 51);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(100, 20);
            this.textBox8.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Количество";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(271, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Сумма";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(237, 80);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(100, 20);
            this.textBox9.TabIndex = 19;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(237, 51);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(100, 20);
            this.textBox10.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Расход";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(118, 80);
            this.textBox11.Name = "textBox11";
            this.textBox11.ReadOnly = true;
            this.textBox11.Size = new System.Drawing.Size(100, 20);
            this.textBox11.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Приход";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(118, 51);
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(100, 20);
            this.textBox12.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(267, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Сумма";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(135, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Количество";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(826, 363);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "С";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(138, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "По";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(32, 11);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(100, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(165, 11);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(100, 20);
            this.numericUpDown2.TabIndex = 9;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(289, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Отобразить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 504);
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
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
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
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox11;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
    }
}

