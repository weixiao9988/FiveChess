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

            Chess.InitPadInfo(num, pd);
            
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
                grp.DrawLine(mPen, p0.X + Chess.LineSpace * i, p0.Y, p0.X + Chess.LineSpace * i, p0.Y + Chess.PadWid);
                //绘制棋盘水平线                
                grp.DrawLine(mPen, p0.X , p0.Y + Chess.LineSpace * i, p0.X + Chess.PadWid , p0.Y + Chess.LineSpace * i);

                grp.FillRectangle(new SolidBrush(Color.BlueViolet), new RectangleF(pp0.X - 7, pp0.Y - 7, 14, 14));
            }
        }

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="grp">绘图设备</param>
        /// <param name="xySeir">输入点的行列编号</param>
        /// <param name="nflag">输入点标志，1:为己方颜色标志，2：他方颜色标志</param>
        public void DrawPieces(Graphics grp,Point xySeir, int nflag, int count)
        {
            int index = nflag == 1 ? 2 : 1;
            Point pt = new Point();
            pt.X = Chess.crossArry[xySeir.Y][xySeir.X].X;
            pt.Y = Chess.crossArry[xySeir.Y][xySeir.X].Y;
            
            //判断输入的行、列有没有棋子
            if (Chess.pcsFlg[xySeir.Y][ xySeir.X] == 0)
            {
                grp.FillEllipse(new SolidBrush(pcsColor[nflag]), 
                    new Rectangle(p0.X + pt.X - Chess.PcsSize, p0.Y + pt.Y - Chess.PcsSize,
                    Chess.PcsSize * 2, Chess.PcsSize * 2));

                StringFormat strFmt1 = new StringFormat(StringFormatFlags.NoWrap);
                strFmt1.Alignment = StringAlignment.Center;
                grp.DrawString(count.ToString(), new Font("微软雅黑", 10,FontStyle.Bold), new SolidBrush(pcsColor[index]), p0.X + pt.X - Chess.PcsSize+16, p0.Y + pt.Y - Chess.PcsSize+7, strFmt1);

                Chess.pcsFlg[xySeir.Y][ xySeir.X] = nflag;
                Chess.isMyPcs = !Chess.isMyPcs;
            }
            else
                MessageBox.Show("此点已有棋子！");
        }
    
        /// <summary>
        /// 绘制棋子和序号标志
        /// </summary>
        /// <param name="grp">绘图设备</param>
        /// <param name="Pcs">棋子坐标数组</param>
        /// <param name="colors">颜色数组</param>
        /// <param name="flag">棋子标志</param>
        public void DrawPcsAndMark(Graphics grp, List<Point> Pcs, List<Color> colors, int flag)
        {
            int index = flag == 1 ? 2 : 1;
            int m = flag - 1 ;
            int xx, yy;
            StringFormat strFmt1 = new StringFormat(StringFormatFlags.NoWrap);
            strFmt1.Alignment = StringAlignment.Center;
            for (int i = 0; i < Pcs.Count; i++)
            {
                xx = Chess.crossArry[Pcs[i].Y][Pcs[i].X].X;
                yy = Chess.crossArry[Pcs[i].Y][Pcs[i].X].Y;

                grp.FillEllipse(new SolidBrush(colors[flag]),
                            new Rectangle(p0.X + xx - Chess.PcsSize, p0.Y + yy - Chess.PcsSize, Chess.PcsSize * 2, Chess.PcsSize * 2));
                
                grp.DrawString((++m+i).ToString(), new Font("微软雅黑", 10, FontStyle.Bold), new SolidBrush(pcsColor[index]), p0.X + xx - Chess.PcsSize + 16, p0.Y + yy - Chess.PcsSize + 7, strFmt1);
            }
        }
    
    }
}
