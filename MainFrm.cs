using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiveChess
{
    public partial class MainFrm : Form
    {
        #region
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern void DeleteObject(IntPtr obj);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        //[DllImport("gdi32", EntryPoint = "StretchBlt")]
        public static extern int StretchBlt(
                IntPtr hdc,
                int x,
                int y,
                int nWidth,
                int nHeight,
                IntPtr hSrcDC,
                int xSrc,
                int ySrc,
                int nSrcWidth,
                int nSrcHeight,
                TernaryRasterOperations dwRop
        );

        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062
        };
        #endregion

        private Rectangle picRect = new Rectangle();
        private Graphics picGrp, bufGrp;
        private Bitmap backBmp;

        private List<Color> pcsColors = new List<Color>() 
        { Color.Khaki,Color.Black,Color.White};
        private int[][] result = new int[3][];

        private MyDraw myDraw;
        private Judge myJudge;


        /// <summary>
        /// 棋盘线的最大值
        /// </summary>
        public int PadLineMax { get; set; } = 15;

        /// <summary>
        /// 棋盘外框周围的空白间隙
        /// </summary>
        public int PadMargin { get; set; } = 30;

        /// <summary>
        /// 游戏对战模式，0：人人对战；1：人机对战
        /// </summary>
        public int GameMode { get; set; } = 0;
        
        /// <summary>
        /// AI难度等级，0，1，2，3
        /// </summary>
        public int AIRank { get; set; } = 0;
        
        /// <summary>
        /// 
        /// </summary>
        public int PcsCount { get; set; }

        /// <summary>
        /// 是否胜利
        /// </summary>
        public bool IsWin { get; set; } = false;

        /// <summary>
        /// 评分后返回的点
        /// </summary>
        private Point ReBackPos = new Point();
        

        public MainFrm()
        {
            InitializeComponent();

            pcsColors.Add(BoardColor_Btn.BackColor);
            pcsColors.Add(PlayerColor_Btn.BackColor);
            pcsColors.Add(CpuColor_Btn.BackColor);

            Timer timer = new Timer();
            timer.Interval = 1000;
            //timer.Enabled = true;
            timer.Tick += new EventHandler(Timer_Tick);
            StatusLabel3.Text = string.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);

            for (int i = 0; i < 3; i++)
                result[i] = new int[2];
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            InitCtrls();
            GameMode_cBox.SelectedIndex = 1;
            AIRank_cBox.SelectedIndex = 2;

            picGrp = picBox.CreateGraphics();
            picRect = picBox.ClientRectangle;
            myDraw = new MyDraw(picRect, PadLineMax, PadMargin, pcsColors);

            DrawBackBmp();

            myJudge = new Judge();

        }

        private void InitCtrls()
        {
            this.Size = new Size(950, 665);
            picBox.Location = new Point(170, 0);
            picBox.Size = new Size(600, 600);
            Set_gBox.Location = new Point(775, 5);
            Set_gBox.Size = new Size(159, 192);
            Btn_panel.Location = new Point(775, 203);
            Btn_panel.Size = new Size(159, 401);

            listView.Columns.Add("序号", 40, HorizontalAlignment.Center);
            listView.Columns.Add("位置", 60, HorizontalAlignment.Center);
            listView.Columns.Add("颜色", 65, HorizontalAlignment.Center);

            StatusLabel3.Width = 200;
            StatusLabel1.Width = (this.Width - 200) / 2;
            StatusLabel2.Width = (this.Width - 200) / 2;
        }

        /// <summary>
        /// 绘制picBox的背景为棋盘
        /// </summary>
        public void DrawBackBmp()
        {
            backBmp = new Bitmap(picRect.Width, picRect.Height);
            bufGrp = Graphics.FromImage(backBmp);
            bufGrp.Clear(pcsColors[0]);

            myDraw.DrawChessBoard(bufGrp);
            picBox.BackgroundImage = backBmp;
        }

        /// <summary>
        /// 从虚拟画布中复制图像
        /// </summary>
        /// <param name="rect">源图像的尺寸</param>
        /// <param name="srcBmp">源图像</param>
        /// <returns>得到的目标图像</returns>
        private Bitmap GetCopyBmp(Rectangle rect, Bitmap srcBmp)
        {
            Bitmap b1 = new Bitmap(rect.Width + rect.X, rect.Height + rect.Y);
            Graphics g1 = Graphics.FromImage(b1);
            IntPtr hg1 = g1.GetHdc();

            IntPtr hSrcBmp = srcBmp.GetHbitmap();
            IntPtr srcHdc = CreateCompatibleDC(hg1);
            IntPtr pOrig = SelectObject(srcHdc, hSrcBmp);
            
            StretchBlt(hg1, rect.X, rect.Y, rect.Width, rect.Height, srcHdc, 0, 0, srcBmp.Width, srcBmp.Height, TernaryRasterOperations.SRCCOPY);
            IntPtr pNew = SelectObject(srcHdc, pOrig);
            DeleteObject(pNew);
            DeleteDC(srcHdc);
            
            g1.ReleaseHdc(hg1);
            return b1;
        }

        /// <summary>
        /// 游戏开始，初始化各种状态数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Btn_Click(object sender, EventArgs e)
        {
            Chess.RestData();
            Chess.InitPecies();
            IsWin = false;

            listView.Items.Clear();
            PcsCount = 0;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    result[i][j] = 0;

            myJudge.InitData();
            //mJudge.UpInfoEvt += this.UpdatStatuBar;

            DrawBackBmp();
            picBox.Refresh();
        }

        private void ReBack_Btn_Click(object sender, EventArgs e)
        {
            int bIndex = Chess.blackPtsLst.Count - 1;
            int wIndex = Chess.whitePtsLst.Count - 1;
            if (bIndex > wIndex)
            {
                Chess.pcsFlag[Chess.blackPtsLst[bIndex].Y][Chess.blackPtsLst[bIndex].X] = 0;
                Chess.blackPtsLst.RemoveAt(bIndex);
                listView.Items.RemoveAt(listView.Items.Count - 1);
                PcsCount -= 1;
                Chess.IsMyPcs = true;
            }
            else
            {
                Chess.pcsFlag[Chess.blackPtsLst[bIndex].Y][Chess.blackPtsLst[bIndex].X] = 0;
                Chess.pcsFlag[Chess.whitePtsLst[wIndex].Y][Chess.whitePtsLst[wIndex].X] = 0;
                Chess.blackPtsLst.RemoveAt(bIndex);
                Chess.whitePtsLst.RemoveAt(wIndex);
                listView.Items.RemoveAt(listView.Items.Count - 1);
                listView.Items.RemoveAt(listView.Items.Count - 1);
                PcsCount -= 2;
            }

            IsWin = false;
            picBox.Refresh();
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            float x1 = picRect.Width / PadLineMax - 1;
            float y1 = picRect.Width / PadLineMax - 1;
            if ((e.X >= PadMargin && e.X <= (int)(x1 * (PadLineMax - 1) + PadMargin)) &&
                e.Y >= PadMargin && e.Y <= (int)((y1) * (PadLineMax - 1) + PadMargin))
            {
                this.Cursor = Cursors.Cross;                
            }
            else
                this.Cursor = Cursors.Default;
            
        }

        private void picBox_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            float x1 = picRect.Width / PadLineMax - 1;
            float y1 = picRect.Width / PadLineMax - 1;

            if (e.Button == MouseButtons.Left)
            {
                if ((e.X >= PadMargin && e.X <= (int)(x1 * (PadLineMax - 1) + PadMargin)) &&
                e.Y >= PadMargin && e.Y <= (int)((y1) * (PadLineMax - 1) + PadMargin))
                {
                    if (!IsWin)
                    {
                        Point pt = Chess.GetXYSerial(e.X - PadMargin, e.Y - PadMargin);
                        if (Chess.pcsFlag[pt.X][pt.Y] == 0)      //棋盘有空位
                        {
                            switch (GameMode)
                            {
                                case 0:
                                    PerVsPer(pt);
                                    break;
                                case 1:
                                    PerVsAI(pt, AIRank);
                                    break;
                                default:
                                    break;
                            }
                            if (result[1][0] >= 5 || result[2][0] >= 5)
                            {
                                IsWin = true;
                                DrawRect();
                                int k = 0;
                                k = result[1][0] >= 5 ? 1 : k;
                                k = result[2][0] >= 5 ? 2 : k;
                                UpdatStatuBar(result);
                                ShowInfoDlg(k);
                                return;
                            }
                        }
                    }
                    else
                        ShowInfoDlg(3);
                }
            }
        }

        /// <summary>
        /// 返回人人对战的结果
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">输入点的标志</param>
        /// <returns></returns>
        public void PerVsPer(Point pt)
        {
            int flag = Chess.IsMyPcs ? 1 : 2;

            myDraw.DrawPieces(picGrp, pt, flag, ++PcsCount);
            UpdatListView(pt, flag, PcsCount);
            //保存己方和他方的棋子
            if (flag == 1)
                Chess.blackPtsLst.Add(pt);
            else
                Chess.whitePtsLst.Add(pt);

            if (Chess.blackPtsLst.Count >= 4 || Chess.whitePtsLst.Count >= 4)
            {
                result = myJudge.JudgeWin(pt, flag, 4);
            }
            StatusLabel2.Text = result[0].ToString() + " " + result[1].ToString() + " " + result[2].ToString();

            if (Chess.blackPtsLst.Count + Chess.whitePtsLst.Count == PadLineMax * PadLineMax)
                ShowInfoDlg(4);
        }

        /// <summary>
        /// 返回人机对战的结果
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">输入点标志</param>
        /// <param name="rank">智力等级</param>
        /// <returns></returns>
        public void PerVsAI(Point pt, int rank)
        {
            int flg;
            if (Chess.blackPtsLst.Count == 0)     //第一个黑子和白子随意落子，不用判断评分
            {
                flg = Chess.IsMyPcs ? 1 : 2;
                myDraw.DrawPieces(picGrp, pt, flg, ++PcsCount);
                Chess.blackPtsLst.Add(pt);
                UpdatListView(pt, flg, PcsCount);

                flg = Chess.IsMyPcs ? 1 : 2;
                Point tPt = myJudge.GetRandPt(pt);
                myDraw.DrawPieces(picGrp, tPt, flg, ++PcsCount);
                Chess.whitePtsLst.Add(tPt);
                UpdatListView(tPt, flg, PcsCount);
            }
            else
            {
                flg = Chess.IsMyPcs ? 1 : 2;
                myDraw.DrawPieces(picGrp, pt, flg, ++PcsCount);
                Chess.blackPtsLst.Add(pt);
                UpdatListView(pt, flg, PcsCount);
                result = myJudge.JudgeWin(pt, flg, 4);

                if (result[flg][0] >= 5)
                    return;
                if (Chess.blackPtsLst.Count + Chess.whitePtsLst.Count == PadLineMax * PadLineMax)
                    ShowInfoDlg(4);

                ////////////////////////////////////////////////////////////////////////////
                ReBackPos = myJudge.AnalysePadInfo(pt, flg, rank);

                if (ReBackPos.X == -1 && ReBackPos.Y == -1)
                    ReBackPos = myJudge.GetRandPt(pt);

                flg = Chess.IsMyPcs ? 1 : 2;
                myDraw.DrawPieces(picGrp, ReBackPos, flg, ++PcsCount);
                Chess.whitePtsLst.Add(ReBackPos);
                UpdatListView(ReBackPos, flg, PcsCount);
                result = myJudge.JudgeWin(ReBackPos, flg, 4);

            }
        }

        /// <summary>
        /// 在输入点周围随意找一个点
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="max">棋盘界限</param>
        /// <param name="padInfo">棋盘信息</param>
        /// <returns></returns>
        public Point GetRandPt(Point pt,int max,List<List<int>> padInfo)
        {
            Point tmp = new Point(-1,-1);
            for (int incr = 1; incr < max; incr++)
            {
                for (int j = -incr; j <= incr; j++)
                {
                    for (int i = -incr; i <= incr; i++)
                    {
                        int x = pt.X + j;
                        int y = pt.Y + i;
                        //找到的位置不能越界，且不能有其他棋子
                        if (x >= 0 && x < max && y >= 0 && y < max && padInfo[y][x] == 0)
                        {
                            tmp.X = x;
                            tmp.Y = y;
                            return tmp;
                        }
                    }
                }
                if (tmp.X!=-1)
                    return tmp;
            }
            return tmp;
        }

        public void ShowInfoDlg(int k)
        {            
            switch (k)
            {
                case 1:
                    StatusLabel1.Text += "黑棋获胜！";
                    MessageBox.Show("黑棋获胜！","提示", MessageBoxButtons.OK,MessageBoxIcon.Information);                    
                    return;
                case 2:
                    StatusLabel2.Text += "白棋获胜！";
                    MessageBox.Show("白棋获胜！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                    return;
                case 3:
                    StatusLabel1.Text += "游戏已结束！";
                    MessageBox.Show("游戏已结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);                    
                    return;
                case 4:
                    StatusLabel1.Text += "游戏已结束！";
                    MessageBox.Show("棋盘已满，本局平局！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);                    
                    return;
                default:
                    break;
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            if (Chess.blackPtsLst.Count > 0 && Chess.whitePtsLst.Count > 0)
            {
                myDraw.DrawPcsAndMark(e.Graphics, Chess.blackPtsLst, 1);
                myDraw.DrawPcsAndMark(e.Graphics, Chess.whitePtsLst, 2);
            }
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        private void DrawRect()
        {
            int flag = Chess.blackPtsLst.Count > Chess.whitePtsLst.Count ? 1 : 2;
            Point pt = flag == 1 ? Chess.blackPtsLst[Chess.blackPtsLst.Count - 1] : Chess.whitePtsLst[Chess.whitePtsLst.Count - 1];

            List<Point> pts = myJudge.GetFiveCnnPos(Chess.pcsFlag, pt, flag, result[flag][1], 4);
            myDraw.DrawRectangle(picGrp, pts[0], pts[1], result[flag][1]);
        }

        private void UpdatStatuBar(int[][] arr)
        {
            StatusLabel1.Text = 1.ToString() + "  " + result[1][0].ToString() + "  " + result[1][1].ToString();
            StatusLabel2.Text = 2.ToString() + "  " + result[2][0].ToString() + "  " + result[2][1].ToString();
        }
        private void UpdatStatuBar(Point pt, string str)
        {
            ReBackPos = pt;
            StatusLabel1.Text =pt.X.ToString()+"_"+pt.Y.ToString()+"_"+ str;
        }

        private void UpdatListView(Point pt, int flag, int count)
        {
            ListViewItem ivi = new ListViewItem();
            string sColor = flag == 1 ? "黑子" : "白子";
            char chr = (char)(pt.X + 65);
            ivi.Text = count.ToString();
            ivi.SubItems.Add(chr.ToString() + "," + pt.Y.ToString());
            ivi.SubItems.Add(sColor);

            listView.Items.Add(ivi);
        }

       

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void BoardColor_Btn_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
                BoardColor_Btn.BackColor = colorDialog.Color;
            pcsColors[0] = BoardColor_Btn.BackColor;
            listView.BackColor = BoardColor_Btn.BackColor;
            picBox.BackColor = BoardColor_Btn.BackColor;
        }

        private void PlayerColor_Btn_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
                PlayerColor_Btn.BackColor = colorDialog.Color;
            pcsColors[1] = PlayerColor_Btn.BackColor;
        }

        private void CpuColor_Btn_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
                CpuColor_Btn.BackColor = colorDialog.Color;
            pcsColors[2] = CpuColor_Btn.BackColor;
        }

        private void GameMode_cBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GameMode_cBox.SelectedIndex)
            {
                case 0:
                    {
                        AIRank_Lab.Visible = false;
                        AIRank_cBox.Visible = false;                        
                        break;
                    }
                case 1:
                    {
                        AIRank_Lab.Visible = true;
                        AIRank_cBox.Visible = true;                        
                        break;
                    }
                default:
                    break;
            }
            GameMode = GameMode_cBox.SelectedIndex;
        }

        private void AIRank_cBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AIRank = AIRank_cBox.SelectedIndex;
        }

        private void MainFrm_SizeChanged(object sender, EventArgs e)
        {
            int scrWidth = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height - 20;
            int addSize = this.Size.Width - 950;
            int frmtop = (scrWidth + 20 - 665 - addSize) / 2;
            //窗体恢复正常时   
            if (this.WindowState == FormWindowState.Normal)
            {
                this.Location = new Point(300, frmtop);
                this.MaximumSize = new Size(scrWidth + 285, scrWidth);
                this.Size = new Size(this.Size.Width, addSize + 665);
                picBox.Size = new Size(this.Size.Height - 65, this.Size.Height - 65);

                picRect = picBox.ClientRectangle;
                myDraw.InitData(picRect, PadLineMax, PadMargin, pcsColors);
                DrawBackBmp();
                picBox.Refresh();
            }

            StatusLabel3.Width = 200;
            StatusLabel1.Width = (this.Width - 200) / 2;
            StatusLabel2.Width = (this.Width - 200) / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<List<int>> tmp = new List<List<int>>();
            List<int> arry1 = new List<int>() { 1, 2, 3 };
            List<int> arry2 = new List<int>() { 4, 5, 6 };
            List<int> arry3 = new List<int>() { 7, 8, 9 };
            tmp.Add(arry1);
            tmp.Add(arry2);
            tmp.Add(arry3);

            List<int> tmp2 = new List<int>();
            foreach (List<int> item in tmp)
            {
                tmp2.Add(item.Max());
            }

            //int m = tmp.IndexOf(tmp.Max());
            //textBox1.Text = m.ToString();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            StatusLabel3.Text = String.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);
        }



    }
}
