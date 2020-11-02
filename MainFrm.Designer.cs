namespace FiveChess
{
    partial class MainFrm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.StartPlaye_Btn = new System.Windows.Forms.Button();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ownColor_Btn = new System.Windows.Forms.Button();
            this.cpuColor_Btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.CPUAI_cbBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PalyModel_cbBox = new System.Windows.Forms.ComboBox();
            this.Back_Btn = new System.Windows.Forms.Button();
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ChsPadColor_Btn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.Cancel_Btn);
            this.panel1.Controls.Add(this.Back_Btn);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.StartPlaye_Btn);
            this.panel1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(636, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(123, 408);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 267);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // StartPlaye_Btn
            // 
            this.StartPlaye_Btn.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StartPlaye_Btn.Location = new System.Drawing.Point(18, 21);
            this.StartPlaye_Btn.Name = "StartPlaye_Btn";
            this.StartPlaye_Btn.Size = new System.Drawing.Size(87, 36);
            this.StartPlaye_Btn.TabIndex = 0;
            this.StartPlaye_Btn.Text = "开始游戏";
            this.StartPlaye_Btn.UseVisualStyleBackColor = true;
            this.StartPlaye_Btn.Click += new System.EventHandler(this.StartPlaye_Btn_Click);
            // 
            // picBox
            // 
            this.picBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBox.BackColor = System.Drawing.SystemColors.Control;
            this.picBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.picBox.Location = new System.Drawing.Point(0, 0);
            this.picBox.Margin = new System.Windows.Forms.Padding(0);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(610, 620);
            this.picBox.TabIndex = 1;
            this.picBox.TabStop = false;
            this.picBox.Paint += new System.Windows.Forms.PaintEventHandler(this.picBox_Paint);
            this.picBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseClick);
            this.picBox.MouseLeave += new System.EventHandler(this.picBox_MouseLeave);
            this.picBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.StatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 606);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 25);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(569, 20);
            this.StatusLabel1.Spring = true;
            this.StatusLabel1.Text = "toolStripStatusLabel1";
            this.StatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.AutoSize = false;
            this.StatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.StatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(200, 20);
            this.StatusLabel2.Text = "toolStripStatusLabel1";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ChsPadColor_Btn);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.PalyModel_cbBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CPUAI_cbBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cpuColor_Btn);
            this.groupBox1.Controls.Add(this.ownColor_Btn);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(614, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 177);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "己方颜色";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "他方颜色";
            // 
            // ownColor_Btn
            // 
            this.ownColor_Btn.BackColor = System.Drawing.Color.White;
            this.ownColor_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ownColor_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ownColor_Btn.Location = new System.Drawing.Point(75, 52);
            this.ownColor_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.ownColor_Btn.Name = "ownColor_Btn";
            this.ownColor_Btn.Size = new System.Drawing.Size(83, 23);
            this.ownColor_Btn.TabIndex = 2;
            this.ownColor_Btn.Text = "白色";
            this.ownColor_Btn.UseVisualStyleBackColor = false;
            this.ownColor_Btn.Click += new System.EventHandler(this.ownColor_Btn_Click);
            // 
            // cpuColor_Btn
            // 
            this.cpuColor_Btn.BackColor = System.Drawing.Color.Black;
            this.cpuColor_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cpuColor_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cpuColor_Btn.ForeColor = System.Drawing.Color.White;
            this.cpuColor_Btn.Location = new System.Drawing.Point(75, 75);
            this.cpuColor_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.cpuColor_Btn.Name = "cpuColor_Btn";
            this.cpuColor_Btn.Size = new System.Drawing.Size(83, 23);
            this.cpuColor_Btn.TabIndex = 3;
            this.cpuColor_Btn.Text = "黑色";
            this.cpuColor_Btn.UseVisualStyleBackColor = false;
            this.cpuColor_Btn.Click += new System.EventHandler(this.cpuColor_Btn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 4;
            this.label4.Text = "机器智力";
            // 
            // CPUAI_cbBox
            // 
            this.CPUAI_cbBox.FormattingEnabled = true;
            this.CPUAI_cbBox.Items.AddRange(new object[] {
            "低级",
            "初级",
            "中级",
            "高级"});
            this.CPUAI_cbBox.Location = new System.Drawing.Point(75, 106);
            this.CPUAI_cbBox.Name = "CPUAI_cbBox";
            this.CPUAI_cbBox.Size = new System.Drawing.Size(83, 22);
            this.CPUAI_cbBox.TabIndex = 5;
            this.CPUAI_cbBox.Tag = "1";
            this.CPUAI_cbBox.Text = "初级";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "对战模式";
            // 
            // PalyModel_cbBox
            // 
            this.PalyModel_cbBox.DisplayMember = "0";
            this.PalyModel_cbBox.FormattingEnabled = true;
            this.PalyModel_cbBox.Items.AddRange(new object[] {
            "人机对战",
            "人人对战"});
            this.PalyModel_cbBox.Location = new System.Drawing.Point(75, 138);
            this.PalyModel_cbBox.Name = "PalyModel_cbBox";
            this.PalyModel_cbBox.Size = new System.Drawing.Size(83, 22);
            this.PalyModel_cbBox.TabIndex = 7;
            this.PalyModel_cbBox.Text = "人机对战";
            this.PalyModel_cbBox.ValueMember = "0";
            // 
            // Back_Btn
            // 
            this.Back_Btn.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Back_Btn.Location = new System.Drawing.Point(18, 98);
            this.Back_Btn.Name = "Back_Btn";
            this.Back_Btn.Size = new System.Drawing.Size(87, 36);
            this.Back_Btn.TabIndex = 2;
            this.Back_Btn.Text = "悔  棋";
            this.Back_Btn.UseVisualStyleBackColor = true;
            this.Back_Btn.Click += new System.EventHandler(this.Back_Btn_Click);
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Cancel_Btn.Location = new System.Drawing.Point(18, 360);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(87, 36);
            this.Cancel_Btn.TabIndex = 3;
            this.Cancel_Btn.Text = "退 出";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            this.Cancel_Btn.Click += new System.EventHandler(this.Cancel_Btn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 8;
            this.label6.Text = "棋盘颜色";
            // 
            // ChsPadColor_Btn
            // 
            this.ChsPadColor_Btn.BackColor = System.Drawing.Color.Khaki;
            this.ChsPadColor_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChsPadColor_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ChsPadColor_Btn.Location = new System.Drawing.Point(75, 26);
            this.ChsPadColor_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.ChsPadColor_Btn.Name = "ChsPadColor_Btn";
            this.ChsPadColor_Btn.Size = new System.Drawing.Size(83, 23);
            this.ChsPadColor_Btn.TabIndex = 9;
            this.ChsPadColor_Btn.Text = "卡其色";
            this.ChsPadColor_Btn.UseVisualStyleBackColor = false;
            this.ChsPadColor_Btn.Click += new System.EventHandler(this.ChsPadColor_Btn_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 631);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.panel1);
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "五子棋";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button StartPlaye_Btn;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox PalyModel_cbBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CPUAI_cbBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cpuColor_Btn;
        private System.Windows.Forms.Button ownColor_Btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.Button Back_Btn;
        private System.Windows.Forms.Button ChsPadColor_Btn;
        private System.Windows.Forms.Label label6;
    }
}

