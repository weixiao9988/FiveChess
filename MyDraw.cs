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
        private float pd = 30;
        //棋盘线的总数
        private int lineCount=15;
        //棋盘中心
        private PointF pp0 = new PointF(30 + 210, 30 + 210);
        //绘制点在棋盘中的位置点
        private Point ptLct = new Point();
        private List<Color> pcsColor;

        


        public MyDraw() { }
        public MyDraw(Rectangle rect, int num, int margin, List<Color> colors)
        {
            p0.X = margin;
            p0.Y = margin;
            pd = ((float)rect.Width) / num-1;
            lineCount = num;
            pp0.X = p0.X + pd * (num - 1)/2;
            pp0.Y = pp0.X;
            pcsColor = colors;

            ChessPad.InitPadInfo(num, pd);
            
        }

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
        /// <param name="pointF">输入点</param>
        /// <param name="nflag">输入点标志，1:为己方颜色标志，2：电脑方颜色标志</param>
        public void DrawPieces(Graphics grp,PointF pointF, int nflag)
        {
            ptLct= GetCursorPtF(ref ChessPad.cursorArry, new PointF(pointF.X-30,pointF.Y-30));
            PointF ptF = new PointF();
            ptF.X = ChessPad.cursorArry[ptLct.Y][ptLct.X].X;
            ptF.Y = ChessPad.cursorArry[ptLct.Y][ptLct.X].Y;
            
            if (ChessPad.pcsClsFlg[ptLct.Y, ptLct.X] == 0)
            {
                grp.FillEllipse(new SolidBrush(pcsColor[nflag]), 
                    new RectangleF(p0.X + ptF.X - ChessPad.pcsSize, p0.Y + ptF.Y - ChessPad.pcsSize,
                    ChessPad.pcsSize * 2, ChessPad.pcsSize * 2));
            }
            //else
            //    MessageBox.Show("此点已有棋子！");
           
            ChessPad.pcsClsFlg[ptLct.Y, ptLct.X] = nflag;

        }
            
        /// <summary>
        /// 根据输入点的坐标，贴近到棋盘交叉点
        /// </summary>
        /// <param name="pointFs">棋盘交叉点的坐标数组</param>
        /// <param name="pointF">输入坐标点</param>
        /// <returns>返回贴近点在棋盘上的行、列编号</returns>
        public Point GetCursorPtF(ref List<List<PointF>> pointFs, PointF pointF)
        {
            Point pt = new Point();
            
            //查找与点最近的列编号
            for (int i = 0; i < pointFs[0].Count; i++)
            {
                if (pointF.X-pointFs[0][i].X<=0)
                {
                    pt.X = Math.Abs(pointF.X - pointFs[0][i].X) < Math.Abs(pointF.X - pointFs[0][i - 1].X) ? i : i - 1;
                    break;
                }
            }

            //查找与点最近的行编号
            for (int i = 0; i < pointFs[0].Count; i++)
            {
                if (pointF.Y - pointFs[i][0].Y<= 0)
                {
                    pt.Y = Math.Abs(pointF.Y - pointFs[i][0].Y) < Math.Abs(pointF.Y - pointFs[i-1][0].Y) ? i : i - 1;
                    break;
                }
            }

            return pt;
        }

    }
}
