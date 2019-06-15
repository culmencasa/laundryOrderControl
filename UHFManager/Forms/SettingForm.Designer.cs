namespace UHFManager
{
    partial class SettingForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblStatus1 = new System.Windows.Forms.Label();
            this.txtPort1 = new System.Windows.Forms.NumericUpDown();
            this.txtIP1 = new IPAddressControlLib.IPAddressControl();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatus2 = new System.Windows.Forms.Label();
            this.txtPort2 = new System.Windows.Forms.NumericUpDown();
            this.txtIP2 = new IPAddressControlLib.IPAddressControl();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblStatus4 = new System.Windows.Forms.Label();
            this.txtPort4 = new System.Windows.Forms.NumericUpDown();
            this.txtIP4 = new IPAddressControlLib.IPAddressControl();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlDevice = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblStatus3 = new System.Windows.Forms.Label();
            this.txtPort3 = new System.Windows.Forms.NumericUpDown();
            this.txtIP3 = new IPAddressControlLib.IPAddressControl();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort2)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort4)).BeginInit();
            this.pnlDevice.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort3)).BeginInit();
            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.lblStatus1);
            this.groupBox1.Controls.Add(this.txtPort1);
            this.groupBox1.Controls.Add(this.txtIP1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "  登记读写器IP地址  ";
            // 
            // lblStatus1
            // 
            this.lblStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatus1.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus1.ForeColor = System.Drawing.SystemColors.Control;
            this.lblStatus1.Location = new System.Drawing.Point(800, 29);
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(138, 54);
            this.lblStatus1.TabIndex = 16;
            // 
            // txtPort1
            // 
            this.txtPort1.Location = new System.Drawing.Point(201, 62);
            this.txtPort1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort1.Name = "txtPort1";
            this.txtPort1.Size = new System.Drawing.Size(149, 21);
            this.txtPort1.TabIndex = 15;
            this.txtPort1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // txtIP1
            // 
            this.txtIP1.AllowInternalTab = false;
            this.txtIP1.AutoHeight = true;
            this.txtIP1.BackColor = System.Drawing.SystemColors.Window;
            this.txtIP1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIP1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIP1.Location = new System.Drawing.Point(201, 29);
            this.txtIP1.MinimumSize = new System.Drawing.Size(93, 21);
            this.txtIP1.Name = "txtIP1";
            this.txtIP1.ReadOnly = false;
            this.txtIP1.Size = new System.Drawing.Size(149, 21);
            this.txtIP1.TabIndex = 10;
            this.txtIP1.Text = "...";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Location = new System.Drawing.Point(22, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 54);
            this.label8.TabIndex = 9;
            this.label8.Text = "入厂";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "端口:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(160, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.lblStatus2);
            this.groupBox2.Controls.Add(this.txtPort2);
            this.groupBox2.Controls.Add(this.txtIP2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(961, 113);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "  登记读写器IP地址  ";
            // 
            // lblStatus2
            // 
            this.lblStatus2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatus2.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblStatus2.Location = new System.Drawing.Point(800, 33);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new System.Drawing.Size(138, 54);
            this.lblStatus2.TabIndex = 17;
            // 
            // txtPort2
            // 
            this.txtPort2.Location = new System.Drawing.Point(201, 64);
            this.txtPort2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort2.Name = "txtPort2";
            this.txtPort2.Size = new System.Drawing.Size(149, 21);
            this.txtPort2.TabIndex = 14;
            this.txtPort2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // txtIP2
            // 
            this.txtIP2.AllowInternalTab = false;
            this.txtIP2.AutoHeight = true;
            this.txtIP2.BackColor = System.Drawing.SystemColors.Window;
            this.txtIP2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIP2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIP2.Location = new System.Drawing.Point(201, 29);
            this.txtIP2.MinimumSize = new System.Drawing.Size(93, 21);
            this.txtIP2.Name = "txtIP2";
            this.txtIP2.ReadOnly = false;
            this.txtIP2.Size = new System.Drawing.Size(149, 21);
            this.txtIP2.TabIndex = 12;
            this.txtIP2.Text = "...";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(22, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 54);
            this.label7.TabIndex = 8;
            this.label7.Text = "洗涤";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "端口:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "IP:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Controls.Add(this.lblStatus4);
            this.groupBox4.Controls.Add(this.txtPort4);
            this.groupBox4.Controls.Add(this.txtIP4);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(12, 399);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(961, 113);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "  登记读写器IP地址  ";
            // 
            // lblStatus4
            // 
            this.lblStatus4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatus4.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus4.ForeColor = System.Drawing.SystemColors.Control;
            this.lblStatus4.Location = new System.Drawing.Point(800, 33);
            this.lblStatus4.Name = "lblStatus4";
            this.lblStatus4.Size = new System.Drawing.Size(138, 54);
            this.lblStatus4.TabIndex = 17;
            // 
            // txtPort4
            // 
            this.txtPort4.Location = new System.Drawing.Point(201, 64);
            this.txtPort4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort4.Name = "txtPort4";
            this.txtPort4.Size = new System.Drawing.Size(149, 21);
            this.txtPort4.TabIndex = 16;
            this.txtPort4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // txtIP4
            // 
            this.txtIP4.AllowInternalTab = false;
            this.txtIP4.AutoHeight = true;
            this.txtIP4.BackColor = System.Drawing.SystemColors.Window;
            this.txtIP4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIP4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIP4.Location = new System.Drawing.Point(201, 30);
            this.txtIP4.MinimumSize = new System.Drawing.Size(93, 21);
            this.txtIP4.Name = "txtIP4";
            this.txtIP4.ReadOnly = false;
            this.txtIP4.Size = new System.Drawing.Size(149, 21);
            this.txtIP4.TabIndex = 14;
            this.txtIP4.Text = "...";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label9.Location = new System.Drawing.Point(22, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 54);
            this.label9.TabIndex = 12;
            this.label9.Text = "出库";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "IP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(160, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "端口:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(828, 29);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(135, 47);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlDevice
            // 
            this.pnlDevice.AutoScroll = true;
            this.pnlDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.pnlDevice.Controls.Add(this.groupBox3);
            this.pnlDevice.Controls.Add(this.groupBox1);
            this.pnlDevice.Controls.Add(this.groupBox2);
            this.pnlDevice.Controls.Add(this.groupBox4);
            this.pnlDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDevice.Location = new System.Drawing.Point(0, 0);
            this.pnlDevice.Name = "pnlDevice";
            this.pnlDevice.Size = new System.Drawing.Size(985, 567);
            this.pnlDevice.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.lblStatus3);
            this.groupBox3.Controls.Add(this.txtPort3);
            this.groupBox3.Controls.Add(this.txtIP3);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(12, 270);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(961, 113);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "  登记读写器IP地址  ";
            // 
            // lblStatus3
            // 
            this.lblStatus3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatus3.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus3.ForeColor = System.Drawing.SystemColors.Control;
            this.lblStatus3.Location = new System.Drawing.Point(800, 33);
            this.lblStatus3.Name = "lblStatus3";
            this.lblStatus3.Size = new System.Drawing.Size(138, 54);
            this.lblStatus3.TabIndex = 17;
            // 
            // txtPort3
            // 
            this.txtPort3.Location = new System.Drawing.Point(201, 64);
            this.txtPort3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort3.Name = "txtPort3";
            this.txtPort3.Size = new System.Drawing.Size(149, 21);
            this.txtPort3.TabIndex = 14;
            this.txtPort3.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // txtIP3
            // 
            this.txtIP3.AllowInternalTab = false;
            this.txtIP3.AutoHeight = true;
            this.txtIP3.BackColor = System.Drawing.SystemColors.Window;
            this.txtIP3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIP3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIP3.Location = new System.Drawing.Point(201, 29);
            this.txtIP3.MinimumSize = new System.Drawing.Size(93, 21);
            this.txtIP3.Name = "txtIP3";
            this.txtIP3.ReadOnly = false;
            this.txtIP3.Size = new System.Drawing.Size(149, 21);
            this.txtIP3.TabIndex = 12;
            this.txtIP3.Text = "...";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label11.Location = new System.Drawing.Point(22, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 54);
            this.label11.TabIndex = 8;
            this.label11.Text = "烘干";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(160, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 5;
            this.label12.Text = "端口:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(160, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 12);
            this.label13.TabIndex = 4;
            this.label13.Text = "IP:";
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.Controls.Add(this.btnTest);
            this.pnlToolbar.Controls.Add(this.btnSave);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 567);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(985, 100);
            this.pnlToolbar.TabIndex = 5;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(687, 29);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(135, 47);
            this.btnTest.TabIndex = 17;
            this.btnTest.Text = "测试连接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(985, 667);
            this.Controls.Add(this.pnlDevice);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "SettingForm";
            this.Text = "设置窗体";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort4)).EndInit();
            this.pnlDevice.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort3)).EndInit();
            this.pnlToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlDevice;
        private System.Windows.Forms.Panel pnlToolbar;
        private IPAddressControlLib.IPAddressControl txtIP1;
        private IPAddressControlLib.IPAddressControl txtIP2;
        private IPAddressControlLib.IPAddressControl txtIP4;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.NumericUpDown txtPort2;
        private System.Windows.Forms.NumericUpDown txtPort1;
        private System.Windows.Forms.NumericUpDown txtPort4;
        private System.Windows.Forms.Label lblStatus1;
        private System.Windows.Forms.Label lblStatus2;
        private System.Windows.Forms.Label lblStatus4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblStatus3;
        private System.Windows.Forms.NumericUpDown txtPort3;
        private IPAddressControlLib.IPAddressControl txtIP3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
    }
}