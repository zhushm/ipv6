namespace Multipath6
{
    partial class FileTransfer
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRecvIP = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.rState = new System.Windows.Forms.Label();
            this.rtbConnection = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRecvFileName = new System.Windows.Forms.TextBox();
            this.groupBoxS = new System.Windows.Forms.GroupBox();
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
            this.btnStartinProportion = new System.Windows.Forms.Button();
            this.txtRecvPort2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRecvIP2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRecvPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxC = new System.Windows.Forms.GroupBox();
            this.ckbRandom = new System.Windows.Forms.CheckBox();
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
            this.btnSendinProportion = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.t1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sState = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnRouting = new System.Windows.Forms.Button();
            this.groupBoxS.SuspendLayout();
            this.groupBoxC.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP address:";
            // 
            // txtRecvIP
            // 
            this.txtRecvIP.Location = new System.Drawing.Point(132, 18);
            this.txtRecvIP.Name = "txtRecvIP";
            this.txtRecvIP.Size = new System.Drawing.Size(273, 21);
            this.txtRecvIP.TabIndex = 1;
            this.txtRecvIP.Text = "2001:da8:8020:3::d";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(592, 18);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(142, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // rState
            // 
            this.rState.AutoSize = true;
            this.rState.Location = new System.Drawing.Point(29, 132);
            this.rState.Name = "rState";
            this.rState.Size = new System.Drawing.Size(71, 12);
            this.rState.TabIndex = 0;
            this.rState.Text = "等待启动...";
            // 
            // rtbConnection
            // 
            this.rtbConnection.Location = new System.Drawing.Point(15, 174);
            this.rtbConnection.Name = "rtbConnection";
            this.rtbConnection.Size = new System.Drawing.Size(722, 240);
            this.rtbConnection.TabIndex = 3;
            this.rtbConnection.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(461, 642);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "File Name:";
            // 
            // txtRecvFileName
            // 
            this.txtRecvFileName.Location = new System.Drawing.Point(132, 101);
            this.txtRecvFileName.Name = "txtRecvFileName";
            this.txtRecvFileName.Size = new System.Drawing.Size(605, 21);
            this.txtRecvFileName.TabIndex = 1;
            this.txtRecvFileName.Text = "C:\\temp\\test.jpg";
            // 
            // groupBoxS
            // 
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
            this.groupBoxS.Controls.Add(this.btnStartinProportion);
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
            this.groupBoxS.Controls.Add(this.rState);
            this.groupBoxS.Controls.Add(this.btnStart);
            this.groupBoxS.Location = new System.Drawing.Point(9, 200);
            this.groupBoxS.Name = "groupBoxS";
            this.groupBoxS.Size = new System.Drawing.Size(760, 435);
            this.groupBoxS.TabIndex = 4;
            this.groupBoxS.TabStop = false;
            this.groupBoxS.Text = "Server";
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
            this.txtRecvIP1.Text = "2001:da8:8020:3::d";
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
            // btnStartinProportion
            // 
            this.btnStartinProportion.Location = new System.Drawing.Point(592, 47);
            this.btnStartinProportion.Name = "btnStartinProportion";
            this.btnStartinProportion.Size = new System.Drawing.Size(145, 40);
            this.btnStartinProportion.TabIndex = 45;
            this.btnStartinProportion.Text = "Start in Proportion";
            this.btnStartinProportion.UseVisualStyleBackColor = true;
            this.btnStartinProportion.Click += new System.EventHandler(this.btnStartinProportion_Click);
            // 
            // txtRecvPort2
            // 
            this.txtRecvPort2.Location = new System.Drawing.Point(458, 76);
            this.txtRecvPort2.Name = "txtRecvPort2";
            this.txtRecvPort2.Size = new System.Drawing.Size(61, 21);
            this.txtRecvPort2.TabIndex = 44;
            this.txtRecvPort2.Text = "11112";
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
            this.txtRecvIP2.Text = "2001:da8:8020:3::9";
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
            // groupBoxC
            // 
            this.groupBoxC.Controls.Add(this.ckbRandom);
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
            this.groupBoxC.Controls.Add(this.btnSendinProportion);
            this.groupBoxC.Controls.Add(this.btnSendFile);
            this.groupBoxC.Controls.Add(this.t1);
            this.groupBoxC.Controls.Add(this.label9);
            this.groupBoxC.Controls.Add(this.sState);
            this.groupBoxC.Controls.Add(this.label25);
            this.groupBoxC.Controls.Add(this.label24);
            this.groupBoxC.Controls.Add(this.btnSelectFile);
            this.groupBoxC.Location = new System.Drawing.Point(9, 12);
            this.groupBoxC.Name = "groupBoxC";
            this.groupBoxC.Size = new System.Drawing.Size(760, 182);
            this.groupBoxC.TabIndex = 5;
            this.groupBoxC.TabStop = false;
            this.groupBoxC.Text = "Client";
            // 
            // ckbRandom
            // 
            this.ckbRandom.AutoSize = true;
            this.ckbRandom.Location = new System.Drawing.Point(654, 99);
            this.ckbRandom.Name = "ckbRandom";
            this.ckbRandom.Size = new System.Drawing.Size(78, 16);
            this.ckbRandom.TabIndex = 61;
            this.ckbRandom.Text = "at random";
            this.ckbRandom.UseVisualStyleBackColor = true;
            // 
            // txtBandwidth1
            // 
            this.txtBandwidth1.Location = new System.Drawing.Point(612, 83);
            this.txtBandwidth1.Name = "txtBandwidth1";
            this.txtBandwidth1.Size = new System.Drawing.Size(31, 21);
            this.txtBandwidth1.TabIndex = 60;
            this.txtBandwidth1.Text = "10";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(529, 85);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 12);
            this.label19.TabIndex = 59;
            this.label19.Text = "Bandwidth1：";
            // 
            // txtBandwidth2
            // 
            this.txtBandwidth2.Location = new System.Drawing.Point(612, 110);
            this.txtBandwidth2.Name = "txtBandwidth2";
            this.txtBandwidth2.Size = new System.Drawing.Size(31, 21);
            this.txtBandwidth2.TabIndex = 58;
            this.txtBandwidth2.Text = "10";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(529, 115);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 12);
            this.label20.TabIndex = 57;
            this.label20.Text = "Bandwidth2：";
            // 
            // txtSendPort1
            // 
            this.txtSendPort1.Location = new System.Drawing.Point(458, 82);
            this.txtSendPort1.Name = "txtSendPort1";
            this.txtSendPort1.Size = new System.Drawing.Size(61, 21);
            this.txtSendPort1.TabIndex = 56;
            this.txtSendPort1.Text = "11111";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(411, 85);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 12);
            this.label17.TabIndex = 55;
            this.label17.Text = "Port1：";
            // 
            // txtSendIP1
            // 
            this.txtSendIP1.Location = new System.Drawing.Point(132, 82);
            this.txtSendIP1.Name = "txtSendIP1";
            this.txtSendIP1.Size = new System.Drawing.Size(273, 21);
            this.txtSendIP1.TabIndex = 54;
            this.txtSendIP1.Text = "2001:da8:8020:3::d";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(35, 85);
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
            this.txtSendPort2.Location = new System.Drawing.Point(458, 110);
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
            this.label5.Location = new System.Drawing.Point(411, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "Port2：";
            // 
            // txtSendIP2
            // 
            this.txtSendIP2.Location = new System.Drawing.Point(132, 110);
            this.txtSendIP2.Name = "txtSendIP2";
            this.txtSendIP2.Size = new System.Drawing.Size(273, 21);
            this.txtSendIP2.TabIndex = 42;
            this.txtSendIP2.Text = "2001:da8:8020:3::9";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 41;
            this.label6.Text = "IP address2:";
            // 
            // txtSendPort
            // 
            this.txtSendPort.Location = new System.Drawing.Point(458, 49);
            this.txtSendPort.Name = "txtSendPort";
            this.txtSendPort.Size = new System.Drawing.Size(61, 21);
            this.txtSendPort.TabIndex = 40;
            this.txtSendPort.Text = "11110";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(411, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 39;
            this.label3.Text = "Port：";
            // 
            // txtSendIP
            // 
            this.txtSendIP.Location = new System.Drawing.Point(132, 49);
            this.txtSendIP.Name = "txtSendIP";
            this.txtSendIP.Size = new System.Drawing.Size(273, 21);
            this.txtSendIP.TabIndex = 38;
            this.txtSendIP.Text = "2001:da8:8020:3::d";
            // 
            // txtSendFileName
            // 
            this.txtSendFileName.Location = new System.Drawing.Point(132, 20);
            this.txtSendFileName.Name = "txtSendFileName";
            this.txtSendFileName.Size = new System.Drawing.Size(605, 21);
            this.txtSendFileName.TabIndex = 35;
            this.txtSendFileName.Text = "D:\\temp\\1.jpg";
            // 
            // btnSendinProportion
            // 
            this.btnSendinProportion.Location = new System.Drawing.Point(592, 136);
            this.btnSendinProportion.Name = "btnSendinProportion";
            this.btnSendinProportion.Size = new System.Drawing.Size(145, 40);
            this.btnSendinProportion.TabIndex = 33;
            this.btnSendinProportion.Text = "Send in Proportion";
            this.btnSendinProportion.UseVisualStyleBackColor = true;
            this.btnSendinProportion.Click += new System.EventHandler(this.btnSendinProportion_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(592, 47);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(145, 23);
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
            this.label25.Location = new System.Drawing.Point(35, 52);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(71, 12);
            this.label25.TabIndex = 37;
            this.label25.Text = "IP address:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(35, 25);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(65, 12);
            this.label24.TabIndex = 34;
            this.label24.Text = "File Name:";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(103, 19);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(23, 23);
            this.btnSelectFile.TabIndex = 36;
            this.btnSelectFile.Text = ">";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnRouting
            // 
            this.btnRouting.Location = new System.Drawing.Point(601, 641);
            this.btnRouting.Name = "btnRouting";
            this.btnRouting.Size = new System.Drawing.Size(100, 23);
            this.btnRouting.TabIndex = 6;
            this.btnRouting.Text = "Close";
            this.btnRouting.UseVisualStyleBackColor = true;
            this.btnRouting.Click += new System.EventHandler(this.btnRouting_Click);
            // 
            // FileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 676);
            this.Controls.Add(this.btnRouting);
            this.Controls.Add(this.groupBoxC);
            this.Controls.Add(this.groupBoxS);
            this.Controls.Add(this.btnClear);
            this.Name = "FileTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileTransfer";
            this.Load += new System.EventHandler(this.FileTransfer_Load);
            this.groupBoxS.ResumeLayout(false);
            this.groupBoxS.PerformLayout();
            this.groupBoxC.ResumeLayout(false);
            this.groupBoxC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRecvIP;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label rState;
        private System.Windows.Forms.RichTextBox rtbConnection;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRecvFileName;
        private System.Windows.Forms.GroupBox groupBoxS;
        private System.Windows.Forms.GroupBox groupBoxC;
        private System.Windows.Forms.TextBox txtSendPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSendIP;
        private System.Windows.Forms.TextBox txtSendFileName;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Label sState;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtRecvPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRouting;
        private System.Windows.Forms.Button btnSendinProportion;
        private System.Windows.Forms.TextBox txtSendPort2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSendIP2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRecvPort2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRecvIP2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnStartinProportion;
        private System.Windows.Forms.Label t4_t3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label t4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label t3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label t2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label t2_t1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label t1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRecvPort1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRecvIP1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSendPort1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtSendIP1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox ckbRandom;
        private System.Windows.Forms.TextBox txtBandwidth1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtBandwidth2;
        private System.Windows.Forms.Label label20;
    }
}