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

        private Rectangle drawRect = new Rectangle();
        private Graphics bufGrp, memGrp, picGrp;
        private MyDraw myDraw;
        private const int LINE_MAX= 15;
        private static Bitmap backBmp, memBmp;
        //自己棋子的坐标点
        private List<PointF> ownChessArry;
        //电脑棋子的坐标点
        private List<PointF> cpuChessArry;


       


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
            picGrp = picBox.CreateGraphics();
            myDraw = new MyDraw(drawRect, LINE_MAX);

            DrawBackImg();

            ownChessArry = new List<PointF>();
            cpuChessArry = new List<PointF>();


            //myDraw.DrawChessPad(picGrp);
        }

        public void DrawBackImg()
        {
            backBmp = new Bitmap(drawRect.Width, drawRect.Height);
            bufGrp = Graphics.FromImage(backBmp);
            bufGrp.Clear(Color.Khaki);

            myDraw.DrawChessPad(bufGrp);

            picBox.BackgroundImage = GetCopyBmp(drawRect, backBmp);
        }

        private Bitmap GetCopyBmp(Rectangle rect, Bitmap srcBmp)
        {
            Bitmap b1 = new Bitmap(rect.Width + rect.X, rect.Height + rect.Y);
            Graphics g1 = Graphics.FromImage(b1);
            IntPtr hg1 = g1.GetHdc();

            IntPtr hSrcBmp = srcBmp.GetHbitmap();
            IntPtr srcHdc = CreateCompatibleDC(hg1);
            IntPtr pOrig = SelectObject(srcHdc, hSrcBmp);

            //int res = StretchBlt(hg1, roudw.X, roudw.Y, rect.Width-roudw.Width, rect.Height-roudw.Height, 
            //                             srcHdc, 0, 0, srcBmp.Width , srcBmp.Height, TernaryRasterOperations.SRCCOPY);
            StretchBlt(hg1, rect.X, rect.Y, rect.Width, rect.Height, srcHdc, 0, 0, srcBmp.Width, srcBmp.Height, TernaryRasterOperations.SRCCOPY);
            IntPtr pNew = SelectObject(srcHdc, pOrig);
            DeleteObject(pNew);
            DeleteDC(srcHdc);
            //release handles
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
                    ownChessArry.Add(new PointF(e.X, e.Y));

                    myDraw.DrawPieces(picGrp, new PointF(e.X, e.Y));
                }
                    
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            //myDraw.DrawChessPad(e.Graphics);
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            StatusLabel2.Text = String.Format("{0:yyyy-MM-dd  HH:mm:ss}", DateTime.Now);
        }



    }
}
