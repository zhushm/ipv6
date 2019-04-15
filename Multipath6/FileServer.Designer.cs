namespace Multipath6
{
    partial class FileServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxS = new System.Windows.Forms.GroupBox();
            this.dataGridViewFile = new System.Windows.Forms.DataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtRecvPort1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRecvIP1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.t4_t3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.t4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.t3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtRecvPort2 = new System.Windows.Forms.TextBox();
            this.txtRecvFileName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRecvIP2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRecvPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rtbConnection = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRecvIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sState = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBoxS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxS
            // 
            this.groupBoxS.Controls.Add(this.dataGridViewFile);
            this.groupBoxS.Controls.Add(this.btnSelectFile);
            this.groupBoxS.Controls.Add(this.btnClient);
            this.groupBoxS.Controls.Add(this.btnClear);
            this.groupBoxS.Controls.Add(this.txtRecvPort1);
            this.groupBoxS.Controls.Add(this.label10);
            this.groupBoxS.Controls.Add(this.txtRecvIP1);
            this.groupBoxS.Controls.Add(this.label15);
            this.groupBoxS.Controls.Add(this.t4_t3);
            this.groupBoxS.Controls.Add(this.label13);
            this.groupBoxS.Controls.Add(this.t4);
            this.groupBoxS.Controls.Add(this.label12);
            this.groupBoxS.Controls.Add(this.t3);
            this.groupBoxS.Controls.Add(this.label14);
            this.groupBoxS.Controls.Add(this.txtRecvPort2);
            this.groupBoxS.Controls.Add(this.txtRecvFileName);
            this.groupBoxS.Controls.Add(this.label7);
            this.groupBoxS.Controls.Add(this.txtRecvIP2);
            this.groupBoxS.Controls.Add(this.label8);
            this.groupBoxS.Controls.Add(this.txtRecvPort);
            this.groupBoxS.Controls.Add(this.label4);
            this.groupBoxS.Controls.Add(this.rtbConnection);
            this.groupBoxS.Controls.Add(this.label2);
            this.groupBoxS.Controls.Add(this.txtRecvIP);
            this.groupBoxS.Controls.Add(this.label1);
            this.groupBoxS.Controls.Add(this.sState);
            this.groupBoxS.Controls.Add(this.btnStart);
            this.groupBoxS.Location = new System.Drawing.Point(12, 9);
            this.groupBoxS.Name = "groupBoxS";
            this.groupBoxS.Size = new System.Drawing.Size(760, 655);
            this.groupBoxS.TabIndex = 5;
            this.groupBoxS.TabStop = false;
            this.groupBoxS.Text = "Server";
            // 
            // dataGridViewFile
            // 
            this.dataGridViewFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.FileDate,
            this.FileType,
            this.FileSize});
            this.dataGridViewFile.Location = new System.Drawing.Point(0, 415);
            this.dataGridViewFile.MultiSelect = false;
            this.dataGridViewFile.Name = "dataGridViewFile";
            this.dataGridViewFile.ReadOnly = true;
            this.dataGridViewFile.RowTemplate.Height = 23;
            this.dataGridViewFile.Size = new System.Drawing.Size(760, 238);
            this.dataGridViewFile.TabIndex = 61;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "文件名";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 300;
            // 
            // FileDate
            // 
            this.FileDate.HeaderText = "修改日期";
            this.FileDate.Name = "FileDate";
            this.FileDate.ReadOnly = true;
            this.FileDate.Width = 200;
            // 
            // FileType
            // 
            this.FileType.HeaderText = "类型";
            this.FileType.Name = "FileType";
            this.FileType.ReadOnly = true;
            // 
            // FileSize
            // 
            this.FileSize.HeaderText = "大小(KB)";
            this.FileSize.Name = "FileSize";
            this.FileSize.ReadOnly = true;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(103, 100);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(23, 23);
            this.btnSelectFile.TabIndex = 37;
            this.btnSelectFile.Text = ">";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnClient
            // 
            this.btnClient.Location = new System.Drawing.Point(655, 101);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(83, 23);
            this.btnClient.TabIndex = 60;
            this.btnClient.Text = "Close";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(655, 64);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(83, 23);
            this.btnClear.TabIndex = 59;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtRecvPort1
            // 
            this.txtRecvPort1.Location = new System.Drawing.Point(458, 49);
            this.txtRecvPort1.Name = "txtRecvPort1";
            this.txtRecvPort1.Size = new System.Drawing.Size(61, 21);
            this.txtRecvPort1.TabIndex = 58;
            this.txtRecvPort1.Text = "11111";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(411, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 57;
            this.label10.Text = "Port2：";
            // 
            // txtRecvIP1
            // 
            this.txtRecvIP1.Location = new System.Drawing.Point(132, 46);
            this.txtRecvIP1.Name = "txtRecvIP1";
            this.txtRecvIP1.Size = new System.Drawing.Size(273, 21);
            this.txtRecvIP1.TabIndex = 56;
            this.txtRecvIP1.Text = "::1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(29, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 55;
            this.label15.Text = "IP address2:";
            // 
            // t4_t3
            // 
            this.t4_t3.AutoSize = true;
            this.t4_t3.Location = new System.Drawing.Point(525, 153);
            this.t4_t3.Name = "t4_t3";
            this.t4_t3.Size = new System.Drawing.Size(11, 12);
            this.t4_t3.TabIndex = 53;
            this.t4_t3.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(468, 153);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 54;
            this.label13.Text = "t4-t3：";
            // 
            // t4
            // 
            this.t4.AutoSize = true;
            this.t4.Location = new System.Drawing.Point(317, 153);
            this.t4.Name = "t4";
            this.t4.Size = new System.Drawing.Size(11, 12);
            this.t4.TabIndex = 49;
            this.t4.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(234, 153);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 50;
            this.label12.Text = "接收时刻t4：";
            // 
            // t3
            // 
            this.t3.AutoSize = true;
            this.t3.Location = new System.Drawing.Point(112, 153);
            this.t3.Name = "t3";
            this.t3.Size = new System.Drawing.Size(11, 12);
            this.t3.TabIndex = 47;
            this.t3.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(29, 153);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 48;
            this.label14.Text = "接收时刻t3：";
            // 
            // txtRecvPort2
            // 
            this.txtRecvPort2.Location = new System.Drawing.Point(458, 76);
            this.txtRecvPort2.Name = "txtRecvPort2";
            this.txtRecvPort2.Size = new System.Drawing.Size(61, 21);
            this.txtRecvPort2.TabIndex = 44;
            this.txtRecvPort2.Text = "11112";
            // 
            // txtRecvFileName
            // 
            this.txtRecvFileName.Location = new System.Drawing.Point(132, 101);
            this.txtRecvFileName.Name = "txtRecvFileName";
            this.txtRecvFileName.Size = new System.Drawing.Size(387, 21);
            this.txtRecvFileName.TabIndex = 1;
            this.txtRecvFileName.Text = "C:\\";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(411, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 43;
            this.label7.Text = "Port1：";
            // 
            // txtRecvIP2
            // 
            this.txtRecvIP2.Location = new System.Drawing.Point(132, 73);
            this.txtRecvIP2.Name = "txtRecvIP2";
            this.txtRecvIP2.Size = new System.Drawing.Size(273, 21);
            this.txtRecvIP2.TabIndex = 42;
            this.txtRecvIP2.Text = "::1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 41;
            this.label8.Text = "IP address1:";
            // 
            // txtRecvPort
            // 
            this.txtRecvPort.Location = new System.Drawing.Point(458, 21);
            this.txtRecvPort.Name = "txtRecvPort";
            this.txtRecvPort.Size = new System.Drawing.Size(61, 21);
            this.txtRecvPort.TabIndex = 40;
            this.txtRecvPort.Text = "11110";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(411, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "Port：";
            // 
            // rtbConnection
            // 
            this.rtbConnection.Location = new System.Drawing.Point(15, 174);
            this.rtbConnection.Name = "rtbConnection";
            this.rtbConnection.Size = new System.Drawing.Size(722, 222);
            this.rtbConnection.TabIndex = 3;
            this.rtbConnection.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Folder Name:";
            // 
            // txtRecvIP
            // 
            this.txtRecvIP.Location = new System.Drawing.Point(132, 18);
            this.txtRecvIP.Name = "txtRecvIP";
            this.txtRecvIP.Size = new System.Drawing.Size(273, 21);
            this.txtRecvIP.TabIndex = 1;
            this.txtRecvIP.Text = "::1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP address:";
            // 
            // sState
            // 
            this.sState.AutoSize = true;
            this.sState.Location = new System.Drawing.Point(29, 132);
            this.sState.Name = "sState";
            this.sState.Size = new System.Drawing.Size(71, 12);
            this.sState.TabIndex = 0;
            this.sState.Text = "等待启动...";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(655, 24);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(83, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // FileServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 676);
            this.Controls.Add(this.groupBoxS);
            this.Name = "FileServer";
            this.Text = "FileServer";
            this.Shown += new System.EventHandler(this.FileServer_Shown);
            this.groupBoxS.ResumeLayout(false);
            this.groupBoxS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxS;
        private System.Windows.Forms.TextBox txtRecvPort1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRecvIP1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label t4_t3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label t4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label t3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtRecvPort2;
        private System.Windows.Forms.TextBox txtRecvFileName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRecvIP2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRecvPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rtbConnection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRecvIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sState;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.DataGridView dataGridViewFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
    }
}