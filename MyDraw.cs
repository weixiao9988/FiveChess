using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiveChess
{
    class MyDraw
    {
        //绘制棋盘的起点
        private Point p0 = new Point(30, 30);
        //棋盘线的间隔
        private int pd = 30;
        //棋盘线的总数
        private int lineCount=15;
        //棋盘中心
        private PointF pp0 = new PointF(30 + 210, 30 + 210);
        
        private List<Color> pcsColor;

        


        public MyDraw() { }
        public MyDraw(Rectangle rect, int num, int margin, List<Color> colors)
        {
            p0.X = margin;
            p0.Y = margin;
            pd = rect.Width / num-1;
            lineCount = num;
            pp0.X = p0.X + pd * (num - 1)/2;
            pp0.Y = pp0.X;
            pcsColor = colors;

            ChessPad.InitPadInfo(num, pd);
            
        }

        /// <summary>
        /// 绘制棋盘
        /// </summary>
        /// <param name="grp"></param>
        public void DrawChessPad(Graphics grp)
        {
            Pen mPen = new Pen(Color.Black, 1);

            for (int i = 0; i < lineCount; i++)
            {
                //绘制棋盘垂直线                
                grp.DrawLine(mPen, p0.X + ChessPad.lineSpace * i, p0.Y, p0.X + ChessPad.lineSpace * i, p0.Y + ChessPad.padWid);
                //绘制棋盘水平线                
                grp.DrawLine(mPen, p0.X , p0.Y + ChessPad.lineSpace * i, p0.X + ChessPad.padWid , p0.Y + ChessPad.lineSpace * i);

                grp.FillRectangle(new SolidBrush(Color.BlueViolet), new RectangleF(pp0.X - 7, pp0.Y - 7, 14, 14));
            }
        }

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="grp">绘图设备</param>
        /// <param name="xySeir">输入点的行列编号</param>
        /// <param name="nflag">输入点标志，1:为己方颜色标志，2：他方颜色标志</param>
        public void DrawPieces(Graphics grp,Point xySeir, int nflag)
        {
            
            PointF ptF = new PointF();
            ptF.X = ChessPad.crossArry[xySeir.Y][xySeir.X].X;
            ptF.Y = ChessPad.crossArry[xySeir.Y][xySeir.X].Y;
            
            if (ChessPad.pcsClsFlg[xySeir.Y, xySeir.X] == 0)
            {
                grp.FillEllipse(new SolidBrush(pcsColor[nflag]), 
                    new RectangleF(p0.X + ptF.X - ChessPad.pcsSize, p0.Y + ptF.Y - ChessPad.pcsSize,
                    ChessPad.pcsSize * 2, ChessPad.pcsSize * 2));

                ChessPad.pcsClsFlg[xySeir.Y, xySeir.X] = nflag;
                ChessPad.isMyPcs = !ChessPad.isMyPcs;
            }
            //else
            //    MessageBox.Show("此点已有棋子！");
           
            

        }
            
       

    }
}
