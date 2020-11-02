using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveChess
{
    class MyDraw
    {
        //绘制棋盘的起点
        private PointF p0 = new PointF(30, 30);
        //棋盘线的间隔
        private float pd = 30;
        //棋盘线的总数
        private int lineCount=15;
        //棋盘中心
        private PointF pp0 = new PointF(30 + 210, 30 + 210);

        private ChessPad padInfo;


        public MyDraw() { }
        public MyDraw(Rectangle rect, int num)
        {
            pd = ((float)rect.Width) / num-1;
            lineCount = num;
            pp0.X = p0.X + pd * (num - 1)/2;
            pp0.Y = pp0.X;

            padInfo = new ChessPad(num,pd);
        }

        public void DrawChessPad(Graphics grp)
        {
            Pen mPen = new Pen(Color.Black, 1);

            for (int i = 0; i < lineCount; i++)
            {
                //绘制棋盘垂直线
                //grp.DrawLine(mPen, p0.X + pd * i, p0.Y, p0.X + pd * i, p0.Y + pd * (lineCount-1));
                grp.DrawLine(mPen, p0.X + padInfo.lineSpace * i, p0.Y, p0.X + padInfo.lineSpace * i, p0.Y + padInfo.padWid);
                //绘制棋盘水平线
                //grp.DrawLine(mPen, p0.X, p0.Y + pd * i, p0.X + pd * (lineCount-1), p0.Y + pd * i);
                grp.DrawLine(mPen, p0.X , p0.Y + padInfo.lineSpace * i, p0.X + padInfo.padWid , p0.Y + padInfo.lineSpace * i);

                grp.FillRectangle(new SolidBrush(Color.BlueViolet), new RectangleF(pp0.X - 7, pp0.Y - 7, 14, 14));
            }
        }

        public void DrawPieces(Graphics grp,PointF pointF)
        {
            PointF ptF= GetCursorPtF(padInfo.cursorArry, new PointF(pointF.X-30,pointF.Y-30));
            grp.FillEllipse(new SolidBrush(Color.White), new RectangleF(p0.X+ptF.X - padInfo.pcsSize, p0.Y+ptF.Y - padInfo.pcsSize, padInfo.pcsSize*2, padInfo.pcsSize * 2));
        }

        public PointF GetCursorPtF(List<List<PointF>> pointFs,PointF pointF)
        {
            PointF ptF = new PointF();
            int h=0, l=0;
            for (int i = 0; i < pointFs[0].Count; i++)
            {
                if (pointF.X-pointFs[0][i].X<=0)
                {
                    l=Math.Abs(pointF.X - pointFs[0][i].X) < Math.Abs(pointF.X - pointFs[0][i - 1].X) ? i : i - 1;
                    break;
                }
            }

            for (int i = 0; i < pointFs[0].Count; i++)
            {
                if (pointF.Y - pointFs[i][0].Y<= 0)
                {
                    h = Math.Abs(pointF.Y - pointFs[i][0].Y) < Math.Abs(pointF.Y - pointFs[i-1][0].Y) ? i : i - 1;
                    break;
                }
            }

            ptF = pointFs[h][l];

            return ptF;
        }

    }
}
