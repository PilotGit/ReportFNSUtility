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
            this.GB_Connect = new System.Windows.Forms.GroupBox();
            this.L_fileWay = new System.Windows.Forms.Label();
            this.L_fileName = new System.Windows.Forms.Label();
            this.TB_fileName = new System.Windows.Forms.TextBox();
            this.B_fileWayDialog = new System.Windows.Forms.Button();
            this.TB_fileWay = new System.Windows.Forms.TextBox();
            this.B_startParse = new System.Windows.Forms.Button();
            this.L_Rate = new System.Windows.Forms.Label();
            this.L_Port = new System.Windows.Forms.Label();
            this.CB_Rate = new System.Windows.Forms.ComboBox();
            this.CB_Port = new System.Windows.Forms.ComboBox();
            this.GB_PreviewReport = new System.Windows.Forms.GroupBox();
            this.ChB_VisibleValue = new System.Windows.Forms.CheckBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.B_Update = new System.Windows.Forms.Button();
            this.B_Browse = new System.Windows.Forms.Button();
            this.TB_Patch = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.GB_Connect.SuspendLayout();
            this.GB_PreviewReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_Connect
            // 
            this.GB_Connect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GB_Connect.Controls.Add(this.L_fileWay);
            this.GB_Connect.Controls.Add(this.L_fileName);
            this.GB_Connect.Controls.Add(this.TB_fileName);
            this.GB_Connect.Controls.Add(this.B_fileWayDialog);
            this.GB_Connect.Controls.Add(this.TB_fileWay);
            this.GB_Connect.Controls.Add(this.B_startParse);
            this.GB_Connect.Controls.Add(this.L_Rate);
            this.GB_Connect.Controls.Add(this.L_Port);
            this.GB_Connect.Controls.Add(this.CB_Rate);
            this.GB_Connect.Controls.Add(this.CB_Port);
            this.GB_Connect.Location = new System.Drawing.Point(21, 29);
            this.GB_Connect.Name = "GB_Connect";
            this.GB_Connect.Size = new System.Drawing.Size(236, 455);
            this.GB_Connect.TabIndex = 0;
            this.GB_Connect.TabStop = false;
            this.GB_Connect.Text = "Подключение к ККТ";
            // 
            // L_fileWay
            // 
            this.L_fileWay.AutoSize = true;
            this.L_fileWay.Location = new System.Drawing.Point(7, 217);
            this.L_fileWay.Name = "L_fileWay";
            this.L_fileWay.Size = new System.Drawing.Size(74, 13);
            this.L_fileWay.TabIndex = 17;
            this.L_fileWay.Text = "Путь к файлу";
            // 
            // L_fileName
            // 
            this.L_fileName.AutoSize = true;
            this.L_fileName.Location = new System.Drawing.Point(7, 173);
            this.L_fileName.Name = "L_fileName";
            this.L_fileName.Size = new System.Drawing.Size(64, 13);
            this.L_fileName.TabIndex = 16;
            this.L_fileName.Text = "Имя файла";
            // 
            // TB_fileName
            // 
            this.TB_fileName.Location = new System.Drawing.Point(10, 189);
            this.TB_fileName.Name = "TB_fileName";
            this.TB_fileName.Size = new System.Drawing.Size(184, 20);
            this.TB_fileName.TabIndex = 14;
            // 
            // B_fileWayDialog
            // 
            this.B_fileWayDialog.Location = new System.Drawing.Point(200, 233);
            this.B_fileWayDialog.Name = "B_fileWayDialog";
            this.B_fileWayDialog.Size = new System.Drawing.Size(30, 20);
            this.B_fileWayDialog.TabIndex = 13;
            this.B_fileWayDialog.Text = "🔍";
            this.B_fileWayDialog.UseVisualStyleBackColor = true;
            // 
            // TB_fileWay
            // 
            this.TB_fileWay.Location = new System.Drawing.Point(10, 233);
            this.TB_fileWay.Name = "TB_fileWay";
            this.TB_fileWay.Size = new System.Drawing.Size(184, 20);
            this.TB_fileWay.TabIndex = 12;
            // 
            // B_startParse
            // 
            this.B_startParse.Location = new System.Drawing.Point(10, 259);
            this.B_startParse.Name = "B_startParse";
            this.B_startParse.Size = new System.Drawing.Size(220, 23);
            this.B_startParse.TabIndex = 11;
            this.B_startParse.Text = "Формировать отчет";
            this.B_startParse.UseVisualStyleBackColor = true;
            this.B_startParse.Click += new System.EventHandler(this.B_startParse_Click);
            // 
            // L_Rate
            // 
            this.L_Rate.AutoSize = true;
            this.L_Rate.Location = new System.Drawing.Point(7, 71);
            this.L_Rate.Name = "L_Rate";
            this.L_Rate.Size = new System.Drawing.Size(49, 13);
            this.L_Rate.TabIndex = 3;
            this.L_Rate.Text = "Частота";
            // 
            // L_Port
            // 
            this.L_Port.AutoSize = true;
            this.L_Port.Location = new System.Drawing.Point(7, 19);
            this.L_Port.Name = "L_Port";
            this.L_Port.Size = new System.Drawing.Size(32, 13);
            this.L_Port.TabIndex = 2;
            this.L_Port.Text = "Порт";
            // 
            // CB_Rate
            // 
            this.CB_Rate.FormattingEnabled = true;
            this.CB_Rate.Location = new System.Drawing.Point(7, 90);
            this.CB_Rate.Name = "CB_Rate";
            this.CB_Rate.Size = new System.Drawing.Size(223, 21);
            this.CB_Rate.TabIndex = 1;
            // 
            // CB_Port
            // 
            this.CB_Port.FormattingEnabled = true;
            this.CB_Port.Location = new System.Drawing.Point(7, 42);
            this.CB_Port.Name = "CB_Port";
            this.CB_Port.Size = new System.Drawing.Size(223, 21);
            this.CB_Port.TabIndex = 0;
            // 
            // GB_PreviewReport
            // 
            this.GB_PreviewReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_PreviewReport.Controls.Add(this.ChB_VisibleValue);
            this.GB_PreviewReport.Controls.Add(this.treeView1);
            this.GB_PreviewReport.Controls.Add(this.B_Update);
            this.GB_PreviewReport.Controls.Add(this.B_Browse);
            this.GB_PreviewReport.Controls.Add(this.TB_Patch);
            this.GB_PreviewReport.Location = new System.Drawing.Point(281, 29);
            this.GB_PreviewReport.Name = "GB_PreviewReport";
            this.GB_PreviewReport.Size = new System.Drawing.Size(717, 455);
            this.GB_PreviewReport.TabIndex = 1;
            this.GB_PreviewReport.TabStop = false;
            this.GB_PreviewReport.Text = "Просмотр отчёта";
            // 
            // ChB_VisibleValue
            // 
            this.ChB_VisibleValue.AutoSize = true;
            this.ChB_VisibleValue.Location = new System.Drawing.Point(7, 42);
            this.ChB_VisibleValue.Name = "ChB_VisibleValue";
            this.ChB_VisibleValue.Size = new System.Drawing.Size(177, 17);
            this.ChB_VisibleValue.TabIndex = 5;
            this.ChB_VisibleValue.Text = "Отображать данные в дереве";
            this.ChB_VisibleValue.UseVisualStyleBackColor = true;
            this.ChB_VisibleValue.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(7, 65);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(697, 384);
            this.treeView1.TabIndex = 4;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // B_Update
            // 
            this.B_Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Update.Location = new System.Drawing.Point(629, 17);
            this.B_Update.Name = "B_Update";
            this.B_Update.Size = new System.Drawing.Size(75, 23);
            this.B_Update.TabIndex = 3;
            this.B_Update.Text = "Обновить";
            this.B_Update.UseVisualStyleBackColor = true;
            this.B_Update.Click += new System.EventHandler(this.button2_Click);
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
            this.B_Browse.Click += new System.EventHandler(this.button1_Click);
            // 
            // TB_Patch
            // 
            this.TB_Patch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Patch.Location = new System.Drawing.Point(6, 19);
            this.TB_Patch.Name = "TB_Patch";
            this.TB_Patch.Size = new System.Drawing.Size(536, 20);
            this.TB_Patch.TabIndex = 0;
            this.TB_Patch.Text = "C:\\Users\\Ulovkov\\Documents\\9999078900011412.fnc";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "bin";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Бинарный файл|*.bin; *.fnc";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(21, 490);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(977, 21);
            this.progressBar1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 523);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.GB_PreviewReport);
            this.Controls.Add(this.GB_Connect);
            this.Name = "Form1";
            this.Text = "ReportFNSUtility";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.GB_Connect.ResumeLayout(false);
            this.GB_Connect.PerformLayout();
            this.GB_PreviewReport.ResumeLayout(false);
            this.GB_PreviewReport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Connect;
        private System.Windows.Forms.Label L_Rate;
        private System.Windows.Forms.Label L_Port;
        private System.Windows.Forms.ComboBox CB_Rate;
        private System.Windows.Forms.ComboBox CB_Port;
        private System.Windows.Forms.GroupBox GB_PreviewReport;
        private System.Windows.Forms.Button B_Browse;
        private System.Windows.Forms.TextBox TB_Patch;
        private System.Windows.Forms.Button B_Update;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.TreeView treeView1;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.CheckBox ChB_VisibleValue;
        private System.Windows.Forms.Label L_fileWay;
        private System.Windows.Forms.Label L_fileName;
        private System.Windows.Forms.TextBox TB_fileName;
        private System.Windows.Forms.Button B_fileWayDialog;
        private System.Windows.Forms.TextBox TB_fileWay;
        private System.Windows.Forms.Button B_startParse;
    }
}

