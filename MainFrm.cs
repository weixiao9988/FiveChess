using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        private const int LINE_MAX = 15;

        private Rectangle drawRect = new Rectangle();
        private Graphics bufGrp, picGrp;
        private MyDraw myDraw;
        
        private static Bitmap backBmp ;
        //自己棋子的坐标点
        private List<PointF> ownChessArry;
        //电脑棋子的坐标点
        private List<PointF> cpuChessArry;
        private ColorDialog colorDlg = new ColorDialog();
        private List<Color> pcsColors = new List<Color>();
       


        public MainFrm()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Interval = 1000;
            //timer.Enabled = true;
            timer.Tick += new EventHandler(Timer_Tick);
            StatusLabel2.Text = string.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);

            drawRect = picBox.ClientRectangle;
        }

        

        private void MainFrm_Load(object sender, EventArgs e)
        {
            pcsColors.Add(Color.Khaki);
            pcsColors.Add(ownColor_Btn.BackColor);
            pcsColors.Add(cpuColor_Btn.BackColor);

            picGrp = picBox.CreateGraphics();
            myDraw = new MyDraw(drawRect, LINE_MAX,pcsColors);

            DrawBackImg();

            ownChessArry = new List<PointF>();
            cpuChessArry = new List<PointF>();

            
            //myDraw.DrawChessPad(picGrp);
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

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {            
            if ((e.X >= 30 && e.X <= (drawRect.Width / LINE_MAX - 1) * (LINE_MAX - 1) + 30) &&
                e.Y >= 30 && e.Y <= (drawRect.Width / LINE_MAX - 1) * (LINE_MAX - 1) + 30)
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
            if (e.Button==MouseButtons.Left)
            {
                if ((e.X >= 30 && e.X <= (drawRect.Width / LINE_MAX - 1) * (LINE_MAX - 1) + 30) &&
                e.Y >= 30 && e.Y <= (drawRect.Width / LINE_MAX - 1) * (LINE_MAX - 1) + 30)
                {
                    label1.Text = e.X.ToString() + " - " + e.Y.ToString();
                    //ownChessArry.Add(new PointF(e.X, e.Y));

                    myDraw.DrawPieces(picGrp, new PointF(e.X, e.Y),1);
                }
                    
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            //myDraw.DrawChessPad(e.Graphics);
        }

        private void StartPlaye_Btn_Click(object sender, EventArgs e)
        {

        }

        private void Back_Btn_Click(object sender, EventArgs e)
        {

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

       

        private void Timer_Tick(object sender, EventArgs e)
        {
            StatusLabel2.Text = String.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);
        }



    }
}
