namespace UHFManager
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpSetting = new System.Windows.Forms.TabPage();
            this.tpEnteringWarehouse = new System.Windows.Forms.TabPage();
            this.tpWash = new System.Windows.Forms.TabPage();
            this.tpDry = new System.Windows.Forms.TabPage();
            this.tpExitWarehouse = new System.Windows.Forms.TabPage();
            this.pnlConnectInfo = new System.Windows.Forms.Panel();
            this.pnlConnectDetails = new System.Windows.Forms.Panel();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowLog = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbMain = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tcMain.SuspendLayout();
            this.pnlConnectInfo.SuspendLayout();
            this.pnlConnectDetails.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpSetting);
            this.tcMain.Controls.Add(this.tpEnteringWarehouse);
            this.tcMain.Controls.Add(this.tpWash);
            this.tcMain.Controls.Add(this.tpDry);
            this.tcMain.Controls.Add(this.tpExitWarehouse);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tcMain.HotTrack = true;
            this.tcMain.ItemSize = new System.Drawing.Size(150, 40);
            this.tcMain.Location = new System.Drawing.Point(200, 25);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(984, 716);
            this.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcMain.TabIndex = 4;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpSetting
            // 
            this.tpSetting.Location = new System.Drawing.Point(4, 44);
            this.tpSetting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpSetting.Name = "tpSetting";
            this.tpSetting.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpSetting.Size = new System.Drawing.Size(976, 668);
            this.tpSetting.TabIndex = 0;
            this.tpSetting.Text = "设备配置";
            // 
            // tpEnteringWarehouse
            // 
            this.tpEnteringWarehouse.Location = new System.Drawing.Point(4, 44);
            this.tpEnteringWarehouse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpEnteringWarehouse.Name = "tpEnteringWarehouse";
            this.tpEnteringWarehouse.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpEnteringWarehouse.Size = new System.Drawing.Size(976, 668);
            this.tpEnteringWarehouse.TabIndex = 1;
            this.tpEnteringWarehouse.Text = "入厂";
            // 
            // tpWash
            // 
            this.tpWash.Location = new System.Drawing.Point(4, 44);
            this.tpWash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpWash.Name = "tpWash";
            this.tpWash.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpWash.Size = new System.Drawing.Size(976, 668);
            this.tpWash.TabIndex = 2;
            this.tpWash.Text = "洗涤";
            // 
            // tpDry
            // 
            this.tpDry.Location = new System.Drawing.Point(4, 44);
            this.tpDry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpDry.Name = "tpDry";
            this.tpDry.Size = new System.Drawing.Size(976, 668);
            this.tpDry.TabIndex = 4;
            this.tpDry.Text = "烘干";
            // 
            // tpExitWarehouse
            // 
            this.tpExitWarehouse.Location = new System.Drawing.Point(4, 44);
            this.tpExitWarehouse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpExitWarehouse.Name = "tpExitWarehouse";
            this.tpExitWarehouse.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpExitWarehouse.Size = new System.Drawing.Size(976, 668);
            this.tpExitWarehouse.TabIndex = 3;
            this.tpExitWarehouse.Text = "出库";
            // 
            // pnlConnectInfo
            // 
            this.pnlConnectInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.pnlConnectInfo.Controls.Add(this.pnlConnectDetails);
            this.pnlConnectInfo.Controls.Add(this.pnlHead);
            this.pnlConnectInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlConnectInfo.Location = new System.Drawing.Point(0, 25);
            this.pnlConnectInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlConnectInfo.Name = "pnlConnectInfo";
            this.pnlConnectInfo.Size = new System.Drawing.Size(200, 716);
            this.pnlConnectInfo.TabIndex = 5;
            // 
            // pnlConnectDetails
            // 
            this.pnlConnectDetails.Controls.Add(this.rtbMain);
            this.pnlConnectDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConnectDetails.Location = new System.Drawing.Point(0, 43);
            this.pnlConnectDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlConnectDetails.Name = "pnlConnectDetails";
            this.pnlConnectDetails.Padding = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.pnlConnectDetails.Size = new System.Drawing.Size(200, 673);
            this.pnlConnectDetails.TabIndex = 1;
            // 
            // pnlHead
            // 
            this.pnlHead.Controls.Add(this.label1);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(200, 43);
            this.pnlHead.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1184, 25);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowLog,
            this.tsmiClearLog,
            this.toolStripMenuItem1,
            this.tsmiExit});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "菜单";
            // 
            // tsmiShowLog
            // 
            this.tsmiShowLog.Checked = true;
            this.tsmiShowLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiShowLog.Name = "tsmiShowLog";
            this.tsmiShowLog.Size = new System.Drawing.Size(180, 22);
            this.tsmiShowLog.Text = "显示日志栏";
            this.tsmiShowLog.Click += new System.EventHandler(this.tsmiShowLog_Click);
            // 
            // rtbMain
            // 
            this.rtbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbMain.Location = new System.Drawing.Point(5, 5);
            this.rtbMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.Size = new System.Drawing.Size(190, 660);
            this.rtbMain.TabIndex = 7;
            this.rtbMain.Text = "";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "日志";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiClearLog
            // 
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new System.Drawing.Size(180, 22);
            this.tsmiClearLog.Text = "清空日志";
            this.tsmiClearLog.Click += new System.EventHandler(this.tsmiClearLog_Click);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(180, 22);
            this.tsmiExit.Text = "退出";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 25);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 716);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(1184, 741);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.pnlConnectInfo);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "设备控制器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tcMain.ResumeLayout(false);
            this.pnlConnectInfo.ResumeLayout(false);
            this.pnlConnectDetails.ResumeLayout(false);
            this.pnlHead.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpSetting;
        private System.Windows.Forms.TabPage tpEnteringWarehouse;
        private System.Windows.Forms.TabPage tpWash;
        private System.Windows.Forms.TabPage tpExitWarehouse;
        private System.Windows.Forms.Panel pnlConnectInfo;
        private System.Windows.Forms.Panel pnlConnectDetails;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.TabPage tpDry;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowLog;
        private System.Windows.Forms.RichTextBox rtbMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearLog;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.Splitter splitter1;
    }
}

