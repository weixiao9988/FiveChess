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
        private Point p0 = new Point(30, 30);
        //棋盘线的间隔
        private int pd = 30;
        //棋盘线的总数
        private int lineCount = 15;
        //棋盘中心
        private Point pp0 = new Point();

        private List<Color> pcsColors;

        public MyDraw() { }

        /// <summary>
        /// 自定义构造函数
        /// </summary>
        /// <param name="rect">需要绘图的矩形区域</param>
        /// <param name="count">棋盘线总数</param>
        /// <param name="margin">棋盘外框留白间隙</param>
        /// <param name="colors">绘图所需颜色集合</param>
        public MyDraw(Rectangle rect, int count, int margin, List<Color> colors)
        {
            p0.X = p0.Y = margin;
            lineCount = count;
            pd = rect.Width / count - 1;
            pp0.X = pp0.Y = p0.X + pd * (count - 1) / 2;
            pcsColors = colors;

            Chess.InitChessBoard(count, pd);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="rect">需要绘图的矩形区域</param>
        /// <param name="count">棋盘线总数</param>
        /// <param name="margin">棋盘外框留白间隙</param>
        /// <param name="colors">绘图所需颜色集合</param>
        public void InitData(Rectangle rect, int count, int margin, List<Color> colors)
        {
            p0.X = p0.Y = margin;
            lineCount = count;
            pd = rect.Width / count - 1;
            pp0.X = pp0.Y = p0.X + pd * (count - 1) / 2;
            pcsColors = colors;

            Chess.InitChessBoard(count, pd);
        }

        /// <summary>
        /// 绘制棋盘
        /// </summary>
        /// <param name="grp"></param>
        public void DrawChessBoard(Graphics grp)
        {
            Pen mPen = new Pen(Color.Black, 1);
            grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;    // 图形抗锯齿
            grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; // 文字抗锯齿
            StringFormat strFmt1 = new StringFormat(StringFormatFlags.NoWrap);
            strFmt1.Alignment = StringAlignment.Center;
            char chr;
            for (int i = 0; i < lineCount; i++)
            {
                //绘制棋盘垂直线                
                grp.DrawLine(mPen, p0.X + pd * i, p0.Y, p0.X + pd * i, p0.Y + pd * (lineCount - 1));
                //绘制棋盘水平线                
                grp.DrawLine(mPen, p0.X, p0.Y + pd * i, p0.X + pd * (lineCount - 1), p0.Y + pd * i);

                chr = (char)(i + 65);
                grp.DrawString(chr.ToString(), new Font("微软雅黑", 10), new SolidBrush(Color.Black), p0.X + i * pd + 5, p0.Y - 30, strFmt1);
                grp.DrawString(i.ToString(), new Font("微软雅黑", 10), new SolidBrush(Color.Black), p0.X - 23, p0.Y + pd * i - 10, strFmt1);

                grp.FillRectangle(new SolidBrush(Color.BlueViolet), new RectangleF(pp0.X - 7, pp0.Y - 7, 14, 14));
            }
            mPen.Dispose();
        }

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="grp">绘图设备</param>
        /// <param name="xySeir">输入点的行列编号</param>
        /// <param name="nflag">输入点标志，1:为己方颜色标志，2：他方颜色标志</param>
        public void DrawPieces(Graphics grp, Point xySeir, int nflag, int count)
        {
            int index = nflag == 1 ? 2 : 1;
            Point pt = new Point();
            pt.X = Chess.crossPoint[xySeir.X][xySeir.Y].X;
            pt.Y = Chess.crossPoint[xySeir.X][xySeir.Y].Y;
            grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;    // 图形抗锯齿
            grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; // 文字抗锯齿
            //判断输入的行、列有没有棋子
            if (Chess.pcsFlag[xySeir.X][xySeir.Y] == 0)
            {
                grp.FillEllipse(new SolidBrush(pcsColors[nflag]),
                    new Rectangle(p0.X + pt.X - Chess.PcsSize, p0.Y + pt.Y - Chess.PcsSize,
                    Chess.PcsSize * 2, Chess.PcsSize * 2));

                StringFormat strFmt1 = new StringFormat(StringFormatFlags.NoWrap);
                strFmt1.Alignment = StringAlignment.Center;
                grp.DrawString(count.ToString(), new Font("微软雅黑", 10, FontStyle.Bold), new SolidBrush(pcsColors[index]),
                    p0.X + pt.X, p0.Y + pt.Y - Chess.PcsSize / 2, strFmt1);

                Chess.pcsFlag[xySeir.X][xySeir.Y] = nflag;
                Chess.IsMyPcs = !Chess.IsMyPcs;
            }
            else
                Chess.IsMyPcs = !Chess.IsMyPcs;

        }

        /// <summary>
        /// 绘制棋子和序号标志
        /// </summary>
        /// <param name="grp">绘图设备</param>
        /// <param name="Pcs">棋子坐标数组</param>
        /// <param name="colors">颜色数组</param>
        /// <param name="flag">棋子标志</param>
        public void DrawPcsAndMark(Graphics grp, List<Point> Pcs, int flag)
        {
            int index = flag == 1 ? 2 : 1;
            int m = flag - 1;
            int xx, yy;
            StringFormat strFmt1 = new StringFormat(StringFormatFlags.NoWrap);
            strFmt1.Alignment = StringAlignment.Center;
            grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;    // 图形抗锯齿
            grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; // 文字抗锯齿
            for (int i = 0; i < Pcs.Count; i++)
            {
                xx = Chess.crossPoint[Pcs[i].X][Pcs[i].Y].X;
                yy = Chess.crossPoint[Pcs[i].X][Pcs[i].Y].Y;

                grp.FillEllipse(new SolidBrush(pcsColors[flag]),
                            new Rectangle(p0.X + xx - Chess.PcsSize, p0.Y + yy - Chess.PcsSize, Chess.PcsSize * 2, Chess.PcsSize * 2));

                grp.DrawString((++m + i).ToString(), new Font("微软雅黑", 10, FontStyle.Bold),
                    new SolidBrush(pcsColors[index]), p0.X + xx, p0.Y + yy - Chess.PcsSize / 2, strFmt1);
            }
        }

        /// <summary>
        /// 在开始和结束点周边绘制边框
        /// </summary>
        /// <param name="grp">绘图设备</param>
        /// <param name="startPt">开始点</param>
        /// <param name="endPt">结束点</param>
        /// <param name="direct">方向</param>
        public void DrawRectangle(Graphics grp, Point startPt, Point endPt, int direct)
        {
            Pen mPen = new Pen(Color.Blue, 2);
            grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;    // 图形抗锯齿
            int x1 = Chess.crossPoint[startPt.X][startPt.Y].X + p0.X;
            int y1 = Chess.crossPoint[startPt.X][startPt.Y].Y + p0.Y;
            int x2 = Chess.crossPoint[endPt.X][endPt.Y].X + p0.X;
            int y2 = Chess.crossPoint[endPt.X][endPt.Y].Y + p0.Y;
            int mSize = Chess.PcsSize;

            switch (direct)
            {
                case 0:
                    grp.DrawRectangle(mPen, x1 - mSize, y1 - mSize, x2 - x1 + mSize * 2, mSize * 2);
                    break;
                case 1:
                    grp.DrawRectangle(mPen, x1 - mSize, y1 - mSize, mSize * 2, y2 - y1 + mSize * 2);
                    break;
                case 2:
                    {
                        grp.DrawLine(mPen, x1, y1 - mSize, x1 + mSize, y1);
                        grp.DrawLine(mPen, x1 + mSize, y1, x2, y2 + mSize);
                        grp.DrawLine(mPen, x2, y2 + mSize, x2 - mSize, y2);
                        grp.DrawLine(mPen, x2 - mSize, y2, x1, y1 - mSize);
                        break;
                    }
                case 3:
                    {
                        grp.DrawLine(mPen, x1, y1 - mSize, x2 + mSize, y2);
                        grp.DrawLine(mPen, x2 + mSize, y2, x2, y2 + mSize);
                        grp.DrawLine(mPen, x2, y2 + mSize, x1 - mSize, y1);
                        grp.DrawLine(mPen, x1 - mSize, y1, x1, y1 - mSize);
                        break;
                    }
                default:
                    break;
            }
            mPen.Dispose();
        }

    }
}
