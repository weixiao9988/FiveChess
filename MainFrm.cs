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

        private int padLineMax = 15;
        private int padMargin = 30;
        private Rectangle drawRect = new Rectangle();
        private Graphics bufGrp, picGrp;
        private MyDraw myDraw;
        private Judge mJudge;
        
        private static Bitmap backBmp ;
        //自己棋子的坐标点
        private List<PointF> ownChessArry;
        //电脑棋子的坐标点
        private List<PointF> cpuChessArry;
        private ColorDialog colorDlg = new ColorDialog();
        private List<Color> pcsColors = new List<Color>();
        private int gameMode=0;
        private int AIRank;
        
        private int pcsMax;
        

        private bool isWin = false;
        public List<Point> ptLstBlack = new List<Point>();
        public List<Point> ptLstWight = new List<Point>();

        /// <summary>
        /// 评分后返回的点
        /// </summary>
        private Point ReBackPos = new Point();

        public MainFrm()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Interval = 1000;
            //timer.Enabled = true;
            timer.Tick += new EventHandler(Timer_Tick);
            StatusLabel3.Text = string.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);

            drawRect = picBox.ClientRectangle;
        }

        

        private void MainFrm_Load(object sender, EventArgs e)
        {
            //pcsColors.Add(Color.Khaki);
            pcsColors.Add(ChsPadColor_Btn.BackColor);
            pcsColors.Add(ownColor_Btn.BackColor);
            pcsColors.Add(cpuColor_Btn.BackColor);

            picGrp = picBox.CreateGraphics();
            myDraw = new MyDraw(drawRect, padLineMax, padMargin, pcsColors);

            DrawBackImg();

            ownChessArry = new List<PointF>();
            cpuChessArry = new List<PointF>();

            gameMode_cbBox.SelectedIndex = 1;
            gameMode = 1;
            pcsMax = padLineMax * padLineMax;
            aiRank_cbBox.SelectedIndex = 1;
            AIRank = 1;

            //myDraw.DrawChessPad(picGrp);

            mJudge = new Judge(Chess.pcsFlg);
            mJudge.UpInfoEvt += this.UpdatStateBar;
        }

        /// <summary>
        /// 绘制picBox的背景为棋盘
        /// </summary>
        public void DrawBackImg()
        {
            backBmp = new Bitmap(drawRect.Width, drawRect.Height);
            bufGrp = Graphics.FromImage(backBmp);
            bufGrp.Clear(pcsColors[0]);

            myDraw.DrawChessPad(bufGrp);

            picBox.BackgroundImage = GetCopyBmp(drawRect, backBmp);
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

        private void StartPlaye_Btn_Click(object sender, EventArgs e)
        {
            Chess.InitPadInfo(padLineMax, drawRect.Width / padLineMax - 1);
            isWin = false;
            ptLstBlack.Clear();
            ptLstWight.Clear();

            //pcsColors.Clear();
            pcsColors.Add(Color.Khaki);
            //pcsColors.Add(Color.White);
            pcsColors.Add(ownColor_Btn.BackColor);
            pcsColors.Add(cpuColor_Btn.BackColor);

            
            gameMode = gameMode_cbBox.SelectedIndex;  
            AIRank = aiRank_cbBox.SelectedIndex;

            DrawBackImg();
            picBox.Refresh();
        }


        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            float x1 = drawRect.Width / padLineMax - 1;
            float y1 = drawRect.Width / padLineMax - 1;
            if ((e.X >= padMargin && e.X <= (int)(x1 * (padLineMax - 1) + padMargin)) &&
                e.Y >= padMargin && e.Y <= (int)((y1) * (padLineMax - 1) + padMargin))
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

        private void picBox_MouseClick(object sender, MouseEventArgs e)
        {
            float x1 = drawRect.Width / padLineMax - 1;
            float y1 = drawRect.Width / padLineMax - 1;
            //int flg = Chess.isMyPcs ? 1 : 2;
            

            if (e.Button==MouseButtons.Left)
            {
                if ((e.X >= padMargin && e.X <= (int)(x1 * (padLineMax - 1) + padMargin)) &&
                e.Y >= padMargin && e.Y <= (int)((y1) * (padLineMax - 1) + padMargin))
                {
                    

                    if (!isWin)
                    {
                        Point pt = Chess.GetRCSeir(e.X - padMargin, e.Y - padMargin);
                        if (Chess.pcsFlg[pt.Y][pt.X] == 0)      //棋盘有空位
                        {

                            int[] n = gameMode == 0 ? PerVsPer(pt) : PerVsAI(pt);

                            if (n.Length > 0 && n[1] >= 5)
                            {
                                isWin = true;
                                ShowInfoDlg(n[0]);
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
        public int[] PerVsPer(Point pt)
        {
            int flg = Chess.isMyPcs ? 1 : 2;
            int[] tArry= {0,0,0 } ;
            myDraw.DrawPieces(picGrp, pt, flg);
            //保存己方和他方的棋子
            if (flg == 1)
                ptLstBlack.Add(pt);
            else
                ptLstWight.Add(pt);

            if (ptLstBlack.Count >= 4 || ptLstWight.Count >= 4)
            {
                tArry = mJudge.JudgeWin(pt, flg);
            }
            StatusLabel2.Text = tArry[0].ToString() + " " + tArry[1].ToString() + " " + tArry[2].ToString();
            return tArry;
        }

        /// <summary>
        /// 返回人机对战的结果
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">输入点标志</param>
        /// <returns></returns>
        public int[] PerVsAI(Point pt)
        {
            int flg;
            int[] tArry = {0,0,0 };
            if (ptLstBlack.Count == 0)     //第一个黑子和白子随意落子，不用判断评分
            {
                flg = Chess.isMyPcs ? 1 : 2;
                myDraw.DrawPieces(picGrp, pt, flg);
                ptLstBlack.Add(pt);

                flg = Chess.isMyPcs ? 1 : 2;
                Point tPt = GetRandPt(pt, padLineMax,Chess.pcsFlg);
                myDraw.DrawPieces(picGrp, tPt, flg);                
                ptLstWight.Add(tPt);
            }
            else
            {
                flg = Chess.isMyPcs ? 1 : 2;
                myDraw.DrawPieces(picGrp, pt, flg);
                ptLstBlack.Add(pt);
                int[] arr1 = mJudge.JudgeWin(pt, flg);
                
                if (arr1[1] >= 5)
                {
                    tArry = arr1;
                    return tArry;
                }

                ReBackPos = ReBackPos.X == -1 && ReBackPos.Y == -1 ? GetRandPt(pt, padLineMax, Chess.pcsFlg) : ReBackPos;

                flg = Chess.isMyPcs ? 1 : 2;
                myDraw.DrawPieces(picGrp, ReBackPos, flg);
                ptLstWight.Add(ReBackPos);                
                int[] arr2= mJudge.JudgeWin(ReBackPos, flg);
                
                if (arr2[1] >= 5)
                {
                    tArry = arr1;
                    return tArry;
                }
            }
                return tArry;
        }

        /// <summary>
        /// 在输入点周围随意找一个点
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="max">棋盘界限</param>
        /// <returns></returns>
        public Point GetRandPt(Point pt, int max,List<List<int>> padInfo)
        {
            Point tmp = new Point();
            for (int x = -1; x <=1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    tmp.X = pt.X + x;
                    tmp.Y = pt.Y + y;
                    //找到的位置不能越界，且不能有其他棋子
                    if (tmp.X >= 0 && tmp.X < max && tmp.Y >= 0 && tmp.Y < max && padInfo[tmp.Y][tmp.X] == 0)
                        return tmp;
                }
            }
            return tmp;
        }

        public void ShowInfoDlg(int k)
        {
            switch (k)
            {
                case 1:
                    MessageBox.Show("黑棋获胜！","提示", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                case 2:
                    MessageBox.Show("白棋获胜！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                case 3:
                    MessageBox.Show("游戏已结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                case 4:
                    MessageBox.Show("棋盘已满！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                default:
                    break;
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            //myDraw.DrawChessPad(e.Graphics);
        }

        private void UpdatStateBar(Point pt, string str)
        {
            ReBackPos = pt;
            StatusLabel1.Text =pt.X.ToString()+"_"+pt.Y.ToString()+"_"+ str;
        }
        

        private void Back_Btn_Click(object sender, EventArgs e)
        {
            int flg = 1;
            string s = "001011110011";
            List<string> lstS = new List<string>() { "00101110011", "01101011110011" };
            string part = @"1+";
            part=flg.ToString() + "+";
            for (int i = 0; i < lstS.Count; i++)
            {
                MatchCollection match = Regex.Matches(lstS[i], part);
                Match mt = match[0];
                foreach (Match item in match)
                {
                    if (item.Length > mt.Length)
                        mt = item;
                }
                listBox1.Items.Add(mt.Value);
            }
            
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void ChsPadColor_Btn_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog()==DialogResult.OK)
            {
                ChsPadColor_Btn.BackColor = colorDlg.Color;
                ChsPadColor_Btn.Text = colorDlg.Color.Name;
            }
            pcsColors[0] = ChsPadColor_Btn.BackColor;
        }

        private void ownColor_Btn_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog()==DialogResult.OK)
            {
                ownColor_Btn.BackColor = colorDlg.Color;
                ownColor_Btn.Text = colorDlg.Color.Name;
            }
            pcsColors[1] = ownColor_Btn.BackColor;
        }

        private void cpuColor_Btn_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog()==DialogResult.OK)
            {
                cpuColor_Btn.BackColor = colorDlg.Color;
                cpuColor_Btn.Text = colorDlg.Color.Name;
            }
            pcsColors[2] = cpuColor_Btn.BackColor;
        }

        private void gameMode_cbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (gameMode_cbBox.SelectedIndex)
            {
                case 0:
                    {
                        aiRank_Lab.Visible = false;
                        aiRank_cbBox.Visible = false;
                        pcsMax = padLineMax * padLineMax;
                        break;
                    }
                case 1:
                    {
                        aiRank_Lab.Visible = true;
                        aiRank_cbBox.Visible = true;
                        pcsMax = padLineMax * padLineMax / 2;
                        break;
                    }
                default:
                    break;
            }
            gameMode = gameMode_cbBox.SelectedIndex;
        }

        private void aiRank_cbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AIRank = aiRank_cbBox.SelectedIndex;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            StatusLabel3.Text = String.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);
        }



    }
}
