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
            this.listView = new System.Windows.Forms.ListView();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.Set_gBox = new System.Windows.Forms.GroupBox();
            this.AIRank_cBox = new System.Windows.Forms.ComboBox();
            this.AIRank_Lab = new System.Windows.Forms.Label();
            this.GameMode_cBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CpuColor_Btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PlayerColor_Btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.BoardColor_Btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_panel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.ReBack_Btn = new System.Windows.Forms.Button();
            this.Start_Btn = new System.Windows.Forms.Button();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.Set_gBox.SuspendLayout();
            this.Btn_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView.BackColor = System.Drawing.Color.Khaki;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(170, 600);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.StatusLabel2,
            this.StatusLabel3});
            this.statusBar.Location = new System.Drawing.Point(0, 601);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(934, 25);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.AutoSize = false;
            this.StatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.StatusLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(200, 20);
            this.StatusLabel1.Text = "toolStripStatusLabel1";
            this.StatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.AutoSize = false;
            this.StatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.StatusLabel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(200, 20);
            this.StatusLabel2.Text = "toolStripStatusLabel2";
            this.StatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabel3
            // 
            this.StatusLabel3.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
            this.StatusLabel3.Name = "StatusLabel3";
            this.StatusLabel3.Size = new System.Drawing.Size(519, 20);
            this.StatusLabel3.Spring = true;
            this.StatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // picBox
            // 
            this.picBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBox.BackColor = System.Drawing.Color.Khaki;
            this.picBox.Location = new System.Drawing.Point(170, 0);
            this.picBox.Margin = new System.Windows.Forms.Padding(0);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(600, 600);
            this.picBox.TabIndex = 2;
            this.picBox.TabStop = false;
            this.picBox.Paint += new System.Windows.Forms.PaintEventHandler(this.picBox_Paint);
            this.picBox.MouseLeave += new System.EventHandler(this.picBox_MouseLeave);
            this.picBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseMove);
            this.picBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseUp);
            // 
            // Set_gBox
            // 
            this.Set_gBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Set_gBox.Controls.Add(this.AIRank_cBox);
            this.Set_gBox.Controls.Add(this.AIRank_Lab);
            this.Set_gBox.Controls.Add(this.GameMode_cBox);
            this.Set_gBox.Controls.Add(this.label4);
            this.Set_gBox.Controls.Add(this.CpuColor_Btn);
            this.Set_gBox.Controls.Add(this.label3);
            this.Set_gBox.Controls.Add(this.PlayerColor_Btn);
            this.Set_gBox.Controls.Add(this.label2);
            this.Set_gBox.Controls.Add(this.BoardColor_Btn);
            this.Set_gBox.Controls.Add(this.label1);
            this.Set_gBox.Font = new System.Drawing.Font("宋体", 10.5F);
            this.Set_gBox.Location = new System.Drawing.Point(775, 5);
            this.Set_gBox.Name = "Set_gBox";
            this.Set_gBox.Size = new System.Drawing.Size(159, 192);
            this.Set_gBox.TabIndex = 3;
            this.Set_gBox.TabStop = false;
            this.Set_gBox.Text = "设置";
            // 
            // AIRank_cBox
            // 
            this.AIRank_cBox.FormattingEnabled = true;
            this.AIRank_cBox.Items.AddRange(new object[] {
            "低级",
            "初级",
            "中级",
            "高级"});
            this.AIRank_cBox.Location = new System.Drawing.Point(75, 150);
            this.AIRank_cBox.Name = "AIRank_cBox";
            this.AIRank_cBox.Size = new System.Drawing.Size(75, 22);
            this.AIRank_cBox.TabIndex = 9;
            this.AIRank_cBox.SelectedIndexChanged += new System.EventHandler(this.AIRank_cBox_SelectedIndexChanged);
            // 
            // AIRank_Lab
            // 
            this.AIRank_Lab.AutoSize = true;
            this.AIRank_Lab.Location = new System.Drawing.Point(8, 153);
            this.AIRank_Lab.Name = "AIRank_Lab";
            this.AIRank_Lab.Size = new System.Drawing.Size(63, 14);
            this.AIRank_Lab.TabIndex = 8;
            this.AIRank_Lab.Text = "难度等级";
            // 
            // GameMode_cBox
            // 
            this.GameMode_cBox.FormattingEnabled = true;
            this.GameMode_cBox.Items.AddRange(new object[] {
            "人<->人",
            "人<->机"});
            this.GameMode_cBox.Location = new System.Drawing.Point(75, 120);
            this.GameMode_cBox.Name = "GameMode_cBox";
            this.GameMode_cBox.Size = new System.Drawing.Size(75, 22);
            this.GameMode_cBox.TabIndex = 7;
            this.GameMode_cBox.SelectedIndexChanged += new System.EventHandler(this.GameMode_cBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "对战模式";
            // 
            // CpuColor_Btn
            // 
            this.CpuColor_Btn.BackColor = System.Drawing.Color.White;
            this.CpuColor_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CpuColor_Btn.Location = new System.Drawing.Point(75, 84);
            this.CpuColor_Btn.Name = "CpuColor_Btn";
            this.CpuColor_Btn.Size = new System.Drawing.Size(75, 25);
            this.CpuColor_Btn.TabIndex = 5;
            this.CpuColor_Btn.UseVisualStyleBackColor = false;
            this.CpuColor_Btn.Click += new System.EventHandler(this.CpuColor_Btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "电脑颜色";
            // 
            // PlayerColor_Btn
            // 
            this.PlayerColor_Btn.BackColor = System.Drawing.Color.Black;
            this.PlayerColor_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayerColor_Btn.Location = new System.Drawing.Point(75, 52);
            this.PlayerColor_Btn.Name = "PlayerColor_Btn";
            this.PlayerColor_Btn.Size = new System.Drawing.Size(75, 25);
            this.PlayerColor_Btn.TabIndex = 3;
            this.PlayerColor_Btn.UseVisualStyleBackColor = false;
            this.PlayerColor_Btn.Click += new System.EventHandler(this.PlayerColor_Btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "玩家颜色";
            // 
            // BoardColor_Btn
            // 
            this.BoardColor_Btn.BackColor = System.Drawing.Color.Khaki;
            this.BoardColor_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BoardColor_Btn.Location = new System.Drawing.Point(75, 20);
            this.BoardColor_Btn.Name = "BoardColor_Btn";
            this.BoardColor_Btn.Size = new System.Drawing.Size(75, 25);
            this.BoardColor_Btn.TabIndex = 1;
            this.BoardColor_Btn.UseVisualStyleBackColor = false;
            this.BoardColor_Btn.Click += new System.EventHandler(this.BoardColor_Btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "棋盘颜色";
            // 
            // Btn_panel
            // 
            this.Btn_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_panel.Controls.Add(this.textBox1);
            this.Btn_panel.Controls.Add(this.button1);
            this.Btn_panel.Controls.Add(this.Cancel_Btn);
            this.Btn_panel.Controls.Add(this.ReBack_Btn);
            this.Btn_panel.Controls.Add(this.Start_Btn);
            this.Btn_panel.Location = new System.Drawing.Point(775, 203);
            this.Btn_panel.Name = "Btn_panel";
            this.Btn_panel.Size = new System.Drawing.Size(159, 401);
            this.Btn_panel.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(35, 243);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(35, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Bold);
            this.Cancel_Btn.Location = new System.Drawing.Point(35, 317);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(90, 35);
            this.Cancel_Btn.TabIndex = 2;
            this.Cancel_Btn.Text = "退  出";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            this.Cancel_Btn.Click += new System.EventHandler(this.Cancel_Btn_Click);
            // 
            // ReBack_Btn
            // 
            this.ReBack_Btn.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Bold);
            this.ReBack_Btn.Location = new System.Drawing.Point(35, 129);
            this.ReBack_Btn.Name = "ReBack_Btn";
            this.ReBack_Btn.Size = new System.Drawing.Size(90, 35);
            this.ReBack_Btn.TabIndex = 1;
            this.ReBack_Btn.Text = "悔  棋";
            this.ReBack_Btn.UseVisualStyleBackColor = true;
            this.ReBack_Btn.Click += new System.EventHandler(this.ReBack_Btn_Click);
            // 
            // Start_Btn
            // 
            this.Start_Btn.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Bold);
            this.Start_Btn.Location = new System.Drawing.Point(35, 28);
            this.Start_Btn.Name = "Start_Btn";
            this.Start_Btn.Size = new System.Drawing.Size(90, 35);
            this.Start_Btn.TabIndex = 0;
            this.Start_Btn.Text = "开始游戏";
            this.Start_Btn.UseVisualStyleBackColor = true;
            this.Start_Btn.Click += new System.EventHandler(this.Start_Btn_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 626);
            this.Controls.Add(this.Btn_panel);
            this.Controls.Add(this.Set_gBox);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.statusBar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(950, 665);
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.Resize += new System.EventHandler(this.MainFrm_Resize);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.Set_gBox.ResumeLayout(false);
            this.Set_gBox.PerformLayout();
            this.Btn_panel.ResumeLayout(false);
            this.Btn_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel3;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.GroupBox Set_gBox;
        private System.Windows.Forms.ComboBox GameMode_cBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CpuColor_Btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button PlayerColor_Btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BoardColor_Btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox AIRank_cBox;
        private System.Windows.Forms.Label AIRank_Lab;
        private System.Windows.Forms.Panel Btn_panel;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.Button ReBack_Btn;
        private System.Windows.Forms.Button Start_Btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

