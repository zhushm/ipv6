namespace Multipath6
{
    partial class FileClient
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
            this.groupBoxC = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.ckbMultiPath = new System.Windows.Forms.CheckBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.txtBandwidth1 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtBandwidth2 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtSendPort1 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtSendIP1 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.t2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.t2_t1 = new System.Windows.Forms.Label();
            this.txtSendPort2 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSendIP2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSendPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSendIP = new System.Windows.Forms.TextBox();
            this.txtSendFileName = new System.Windows.Forms.TextBox();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.t1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sState = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.dataGridViewFile = new System.Windows.Forms.DataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxC
            // 
            this.groupBoxC.Controls.Add(this.btnClear);
            this.groupBoxC.Controls.Add(this.ckbMultiPath);
            this.groupBoxC.Controls.Add(this.btnServer);
            this.groupBoxC.Controls.Add(this.txtBandwidth1);
            this.groupBoxC.Controls.Add(this.label19);
            this.groupBoxC.Controls.Add(this.txtBandwidth2);
            this.groupBoxC.Controls.Add(this.label20);
            this.groupBoxC.Controls.Add(this.txtSendPort1);
            this.groupBoxC.Controls.Add(this.label17);
            this.groupBoxC.Controls.Add(this.txtSendIP1);
            this.groupBoxC.Controls.Add(this.label18);
            this.groupBoxC.Controls.Add(this.t2);
            this.groupBoxC.Controls.Add(this.label11);
            this.groupBoxC.Controls.Add(this.t2_t1);
            this.groupBoxC.Controls.Add(this.txtSendPort2);
            this.groupBoxC.Controls.Add(this.label16);
            this.groupBoxC.Controls.Add(this.label5);
            this.groupBoxC.Controls.Add(this.txtSendIP2);
            this.groupBoxC.Controls.Add(this.label6);
            this.groupBoxC.Controls.Add(this.txtSendPort);
            this.groupBoxC.Controls.Add(this.label3);
            this.groupBoxC.Controls.Add(this.txtSendIP);
            this.groupBoxC.Controls.Add(this.txtSendFileName);
            this.groupBoxC.Controls.Add(this.btnSendFile);
            this.groupBoxC.Controls.Add(this.t1);
            this.groupBoxC.Controls.Add(this.label9);
            this.groupBoxC.Controls.Add(this.sState);
            this.groupBoxC.Controls.Add(this.label25);
            this.groupBoxC.Controls.Add(this.label24);
            this.groupBoxC.Controls.Add(this.btnSelectFile);
            this.groupBoxC.Location = new System.Drawing.Point(12, 12);
            this.groupBoxC.Name = "groupBoxC";
            this.groupBoxC.Size = new System.Drawing.Size(760, 195);
            this.groupBoxC.TabIndex = 6;
            this.groupBoxC.TabStop = false;
            this.groupBoxC.Text = "Client";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(649, 67);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(83, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ckbMultiPath
            // 
            this.ckbMultiPath.AutoSize = true;
            this.ckbMultiPath.Location = new System.Drawing.Point(531, 115);
            this.ckbMultiPath.Name = "ckbMultiPath";
            this.ckbMultiPath.Size = new System.Drawing.Size(78, 16);
            this.ckbMultiPath.TabIndex = 61;
            this.ckbMultiPath.Text = "MultiPath";
            this.ckbMultiPath.UseVisualStyleBackColor = true;
            // 
            // btnServer
            // 
            this.btnServer.Location = new System.Drawing.Point(649, 108);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(83, 23);
            this.btnServer.TabIndex = 7;
            this.btnServer.Text = "Close";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txtBandwidth1
            // 
            this.txtBandwidth1.Location = new System.Drawing.Point(612, 55);
            this.txtBandwidth1.Name = "txtBandwidth1";
            this.txtBandwidth1.Size = new System.Drawing.Size(31, 21);
            this.txtBandwidth1.TabIndex = 60;
            this.txtBandwidth1.Text = "10";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(529, 57);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 12);
            this.label19.TabIndex = 59;
            this.label19.Text = "Bandwidth1：";
            // 
            // txtBandwidth2
            // 
            this.txtBandwidth2.Location = new System.Drawing.Point(612, 82);
            this.txtBandwidth2.Name = "txtBandwidth2";
            this.txtBandwidth2.Size = new System.Drawing.Size(31, 21);
            this.txtBandwidth2.TabIndex = 58;
            this.txtBandwidth2.Text = "10";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(529, 87);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 12);
            this.label20.TabIndex = 57;
            this.label20.Text = "Bandwidth2：";
            // 
            // txtSendPort1
            // 
            this.txtSendPort1.Location = new System.Drawing.Point(458, 54);
            this.txtSendPort1.Name = "txtSendPort1";
            this.txtSendPort1.Size = new System.Drawing.Size(61, 21);
            this.txtSendPort1.TabIndex = 56;
            this.txtSendPort1.Text = "11111";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(411, 57);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 12);
            this.label17.TabIndex = 55;
            this.label17.Text = "Port1：";
            // 
            // txtSendIP1
            // 
            this.txtSendIP1.Location = new System.Drawing.Point(132, 54);
            this.txtSendIP1.Name = "txtSendIP1";
            this.txtSendIP1.Size = new System.Drawing.Size(273, 21);
            this.txtSendIP1.TabIndex = 54;
            this.txtSendIP1.Text = "::1";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(29, 57);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 12);
            this.label18.TabIndex = 53;
            this.label18.Text = "IP address1:";
            // 
            // t2
            // 
            this.t2.AutoSize = true;
            this.t2.Location = new System.Drawing.Point(317, 167);
            this.t2.Name = "t2";
            this.t2.Size = new System.Drawing.Size(11, 12);
            this.t2.TabIndex = 45;
            this.t2.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(234, 167);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 46;
            this.label11.Text = "发送时刻t2：";
            // 
            // t2_t1
            // 
            this.t2_t1.AutoSize = true;
            this.t2_t1.Location = new System.Drawing.Point(525, 167);
            this.t2_t1.Name = "t2_t1";
            this.t2_t1.Size = new System.Drawing.Size(11, 12);
            this.t2_t1.TabIndex = 51;
            this.t2_t1.Text = "0";
            // 
            // txtSendPort2
            // 
            this.txtSendPort2.Location = new System.Drawing.Point(458, 82);
            this.txtSendPort2.Name = "txtSendPort2";
            this.txtSendPort2.Size = new System.Drawing.Size(61, 21);
            this.txtSendPort2.TabIndex = 44;
            this.txtSendPort2.Text = "11112";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(468, 167);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 12);
            this.label16.TabIndex = 52;
            this.label16.Text = "t2-t1：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(411, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "Port2：";
            // 
            // txtSendIP2
            // 
            this.txtSendIP2.Location = new System.Drawing.Point(132, 82);
            this.txtSendIP2.Name = "txtSendIP2";
            this.txtSendIP2.Size = new System.Drawing.Size(273, 21);
            this.txtSendIP2.TabIndex = 42;
            this.txtSendIP2.Text = "::1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 41;
            this.label6.Text = "IP address2:";
            // 
            // txtSendPort
            // 
            this.txtSendPort.Location = new System.Drawing.Point(458, 21);
            this.txtSendPort.Name = "txtSendPort";
            this.txtSendPort.Size = new System.Drawing.Size(61, 21);
            this.txtSendPort.TabIndex = 40;
            this.txtSendPort.Text = "11110";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(411, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 39;
            this.label3.Text = "Port：";
            // 
            // txtSendIP
            // 
            this.txtSendIP.Location = new System.Drawing.Point(132, 21);
            this.txtSendIP.Name = "txtSendIP";
            this.txtSendIP.Size = new System.Drawing.Size(273, 21);
            this.txtSendIP.TabIndex = 38;
            this.txtSendIP.Text = "::1";
            // 
            // txtSendFileName
            // 
            this.txtSendFileName.Location = new System.Drawing.Point(132, 111);
            this.txtSendFileName.Name = "txtSendFileName";
            this.txtSendFileName.Size = new System.Drawing.Size(387, 21);
            this.txtSendFileName.TabIndex = 35;
            this.txtSendFileName.Text = "D:\\";
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(649, 24);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(83, 23);
            this.btnSendFile.TabIndex = 33;
            this.btnSendFile.Text = "Send";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // t1
            // 
            this.t1.AutoSize = true;
            this.t1.Location = new System.Drawing.Point(118, 167);
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(11, 12);
            this.t1.TabIndex = 37;
            this.t1.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 37;
            this.label9.Text = "发送时刻t1：";
            // 
            // sState
            // 
            this.sState.AutoSize = true;
            this.sState.Location = new System.Drawing.Point(35, 141);
            this.sState.Name = "sState";
            this.sState.Size = new System.Drawing.Size(71, 12);
            this.sState.TabIndex = 37;
            this.sState.Text = "等待发送...";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(34, 24);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(71, 12);
            this.label25.TabIndex = 37;
            this.label25.Text = "IP address:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(29, 115);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(77, 12);
            this.label24.TabIndex = 34;
            this.label24.Text = "Folder Name:";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(105, 109);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(23, 23);
            this.btnSelectFile.TabIndex = 36;
            this.btnSelectFile.Text = ">";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // dataGridViewFile
            // 
            this.dataGridViewFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.FileDate,
            this.FileType,
            this.FileSize});
            this.dataGridViewFile.Location = new System.Drawing.Point(12, 223);
            this.dataGridViewFile.MultiSelect = false;
            this.dataGridViewFile.Name = "dataGridViewFile";
            this.dataGridViewFile.ReadOnly = true;
            this.dataGridViewFile.RowTemplate.Height = 23;
            this.dataGridViewFile.Size = new System.Drawing.Size(760, 440);
            this.dataGridViewFile.TabIndex = 9;
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
            // FileClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 676);
            this.Controls.Add(this.dataGridViewFile);
            this.Controls.Add(this.groupBoxC);
            this.Name = "FileClient";
            this.Text = "FileClient";
            this.Shown += new System.EventHandler(this.FileClient_Shown);
            this.groupBoxC.ResumeLayout(false);
            this.groupBoxC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxC;
        private System.Windows.Forms.TextBox txtBandwidth1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtBandwidth2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtSendPort1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtSendIP1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label t2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label t2_t1;
        private System.Windows.Forms.TextBox txtSendPort2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSendIP2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSendPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSendIP;
        private System.Windows.Forms.TextBox txtSendFileName;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Label t1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label sState;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.CheckBox ckbMultiPath;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dataGridViewFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
    }
}