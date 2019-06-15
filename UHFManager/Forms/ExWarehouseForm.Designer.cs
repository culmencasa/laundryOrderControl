namespace UHFManager
{
    partial class ExWarehouseForm
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
            this.components = new System.ComponentModel.Container();
            this.testClassBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pnlTopHint = new System.Windows.Forms.Panel();
            this.btnGotoSetting = new System.Windows.Forms.Button();
            this.btnTryConnect = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.dgvMainOrder = new System.Windows.Forms.DataGridView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReadTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSplit = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.testClassBindingSource)).BeginInit();
            this.pnlTopHint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopHint
            // 
            this.pnlTopHint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(225)))));
            this.pnlTopHint.Controls.Add(this.btnGotoSetting);
            this.pnlTopHint.Controls.Add(this.btnTryConnect);
            this.pnlTopHint.Controls.Add(this.lblHint);
            this.pnlTopHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHint.Location = new System.Drawing.Point(0, 0);
            this.pnlTopHint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTopHint.Name = "pnlTopHint";
            this.pnlTopHint.Size = new System.Drawing.Size(800, 45);
            this.pnlTopHint.TabIndex = 3;
            this.pnlTopHint.Visible = false;
            // 
            // btnGotoSetting
            // 
            this.btnGotoSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGotoSetting.Location = new System.Drawing.Point(703, 7);
            this.btnGotoSetting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGotoSetting.Name = "btnGotoSetting";
            this.btnGotoSetting.Size = new System.Drawing.Size(88, 32);
            this.btnGotoSetting.TabIndex = 2;
            this.btnGotoSetting.Text = "去设置";
            this.btnGotoSetting.UseVisualStyleBackColor = true;
            this.btnGotoSetting.Click += new System.EventHandler(this.btnGotoSetting_Click);
            // 
            // btnTryConnect
            // 
            this.btnTryConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTryConnect.Location = new System.Drawing.Point(608, 7);
            this.btnTryConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTryConnect.Name = "btnTryConnect";
            this.btnTryConnect.Size = new System.Drawing.Size(88, 32);
            this.btnTryConnect.TabIndex = 1;
            this.btnTryConnect.Text = "尝试连接";
            this.btnTryConnect.UseVisualStyleBackColor = true;
            this.btnTryConnect.Click += new System.EventHandler(this.btnTryConnect_Click);
            // 
            // lblHint
            // 
            this.lblHint.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHint.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblHint.Location = new System.Drawing.Point(14, 7);
            this.lblHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(704, 32);
            this.lblHint.TabIndex = 0;
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            // 
            // dgvMainOrder
            // 
            this.dgvMainOrder.AllowUserToAddRows = false;
            this.dgvMainOrder.AllowUserToDeleteRows = false;
            this.dgvMainOrder.AllowUserToResizeRows = false;
            this.dgvMainOrder.AutoGenerateColumns = false;
            this.dgvMainOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMainOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colDeviceLabel,
            this.colReadTime,
            this.colUser,
            this.colArea,
            this.colSplit});
            this.dgvMainOrder.DataSource = this.testClassBindingSource;
            this.dgvMainOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvMainOrder.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMainOrder.Location = new System.Drawing.Point(0, 45);
            this.dgvMainOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvMainOrder.MultiSelect = false;
            this.dgvMainOrder.Name = "dgvMainOrder";
            this.dgvMainOrder.ReadOnly = true;
            this.dgvMainOrder.RowTemplate.Height = 23;
            this.dgvMainOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMainOrder.Size = new System.Drawing.Size(800, 408);
            this.dgvMainOrder.TabIndex = 4;
            // 
            // colNumber
            // 
            this.colNumber.HeaderText = "序号";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Visible = false;
            // 
            // colDeviceLabel
            // 
            this.colDeviceLabel.DataPropertyName = "TagSN";
            this.colDeviceLabel.HeaderText = "标签";
            this.colDeviceLabel.Name = "colDeviceLabel";
            this.colDeviceLabel.ReadOnly = true;
            // 
            // colReadTime
            // 
            this.colReadTime.DataPropertyName = "ReadTimes";
            this.colReadTime.HeaderText = "读取次数";
            this.colReadTime.Name = "colReadTime";
            this.colReadTime.ReadOnly = true;
            // 
            // colUser
            // 
            this.colUser.DataPropertyName = "UserName";
            this.colUser.HeaderText = "用户名";
            this.colUser.Name = "colUser";
            this.colUser.ReadOnly = true;
            // 
            // colArea
            // 
            this.colArea.DataPropertyName = "OrderArea";
            this.colArea.HeaderText = "下单区域";
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            // 
            // colSplit
            // 
            this.colSplit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.colSplit.HeaderText = "操作";
            this.colSplit.Name = "colSplit";
            this.colSplit.ReadOnly = true;
            this.colSplit.Text = "查看";
            this.colSplit.UseColumnTextForButtonValue = true;
            // 
            // ExWarehouseForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvMainOrder);
            this.Controls.Add(this.pnlTopHint);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Name = "ExWarehouseForm";
            this.Text = "出库";
            ((System.ComponentModel.ISupportInitialize)(this.testClassBindingSource)).EndInit();
            this.pnlTopHint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlTopHint;
        private System.Windows.Forms.Button btnGotoSetting;
        private System.Windows.Forms.Button btnTryConnect;
        private System.Windows.Forms.Label lblHint;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.BindingSource testClassBindingSource;
        private System.Windows.Forms.DataGridView dgvMainOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReadTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewButtonColumn colSplit;
    }
}