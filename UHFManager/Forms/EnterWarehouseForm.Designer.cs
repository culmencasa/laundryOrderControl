namespace UHFManager
{
    partial class EnterWarehouseForm
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
            this.dgvMainOrder = new System.Windows.Forms.DataGridView();
            this.bgwEnterWarehouse = new System.ComponentModel.BackgroundWorker();
            this.pnlTopHint = new System.Windows.Forms.Panel();
            this.btnGotoSetting = new System.Windows.Forms.Button();
            this.btnTryConnect = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.testClassBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReadTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSplit = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainOrder)).BeginInit();
            this.pnlTopHint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testClassBindingSource)).BeginInit();
            this.SuspendLayout();
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
            this.dgvMainOrder.Margin = new System.Windows.Forms.Padding(4);
            this.dgvMainOrder.MultiSelect = false;
            this.dgvMainOrder.Name = "dgvMainOrder";
            this.dgvMainOrder.ReadOnly = true;
            this.dgvMainOrder.RowTemplate.Height = 23;
            this.dgvMainOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMainOrder.Size = new System.Drawing.Size(934, 408);
            this.dgvMainOrder.TabIndex = 1;
            this.dgvMainOrder.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMainOrder_CellContentClick);
            this.dgvMainOrder.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMainOrder_CellContentDoubleClick);
            // 
            // bgwEnterWarehouse
            // 
            this.bgwEnterWarehouse.WorkerReportsProgress = true;
            this.bgwEnterWarehouse.WorkerSupportsCancellation = true;
            this.bgwEnterWarehouse.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwEnterWarehouse_DoWork);
            this.bgwEnterWarehouse.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwEnterWarehouse_ProgressChanged);
            this.bgwEnterWarehouse.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwEnterWarehouse_RunWorkerCompleted);
            // 
            // pnlTopHint
            // 
            this.pnlTopHint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(225)))));
            this.pnlTopHint.Controls.Add(this.btnGotoSetting);
            this.pnlTopHint.Controls.Add(this.btnTryConnect);
            this.pnlTopHint.Controls.Add(this.lblHint);
            this.pnlTopHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHint.Location = new System.Drawing.Point(0, 0);
            this.pnlTopHint.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTopHint.Name = "pnlTopHint";
            this.pnlTopHint.Size = new System.Drawing.Size(934, 45);
            this.pnlTopHint.TabIndex = 2;
            this.pnlTopHint.Visible = false;
            // 
            // btnGotoSetting
            // 
            this.btnGotoSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGotoSetting.Location = new System.Drawing.Point(837, 7);
            this.btnGotoSetting.Margin = new System.Windows.Forms.Padding(4);
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
            this.btnTryConnect.Location = new System.Drawing.Point(742, 7);
            this.btnTryConnect.Margin = new System.Windows.Forms.Padding(4);
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
            // testClassBindingSource
            // 
            this.testClassBindingSource.AllowNew = true;
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
            this.colSplit.Text = "拆单";
            this.colSplit.UseColumnTextForButtonValue = true;
            // 
            // EnterWarehouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(934, 638);
            this.Controls.Add(this.dgvMainOrder);
            this.Controls.Add(this.pnlTopHint);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnterWarehouseForm";
            this.Text = "送达工厂";
            this.Activated += new System.EventHandler(this.EnterWarehouseForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnterWarehouseForm_FormClosing);
            this.Load += new System.EventHandler(this.EnterWarehouseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainOrder)).EndInit();
            this.pnlTopHint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testClassBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMainOrder;
        private System.Windows.Forms.BindingSource testClassBindingSource;
        private System.ComponentModel.BackgroundWorker bgwEnterWarehouse;
        private System.Windows.Forms.Panel pnlTopHint;
        private System.Windows.Forms.Button btnTryConnect;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Button btnGotoSetting;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReadTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewButtonColumn colSplit;
    }
}