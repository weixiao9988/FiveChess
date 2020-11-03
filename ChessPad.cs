using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FiveChess
{
    static class ChessPad
    {
        /// <summary>
        /// 棋盘线总数
        /// </summary>
        public static int lineCount { get; set; }
        
        /// <summary>
        /// 棋盘宽度
        /// </summary>
        
        public static int padWid { get; set; }
        
        /// <summary>
        /// 棋盘线间距
        /// </summary>
        public static int lineSpace { get; set; }

        /// <summary>
        /// 棋子大小
        /// </summary>
        public static int pcsSize { get { return 16; } set { value = 16; } }
        
        /// <summary>
        /// 标志是不是我方棋子
        /// </summary>
        public static bool isMyPcs = true;

        /// <summary>
        /// 棋盘交叉点坐标数组
        /// </summary>
        public static List<List<Point>> crossArry;

        /// <summary>
        /// 棋子在棋盘中的行列编号，x：列编号，y：行编号
        /// </summary>
        public static Point XYSeir = new Point();
        
        /// <summary>
        /// 棋盘中棋子的颜色标志，0：无颜色，1：己方棋子颜色，2：电脑棋子颜色
        /// </summary>
        public static int[,] pcsClsFlg;
        
        /// <summary>
        /// 初始化棋盘信息
        /// <param name="count">棋盘线总数</param>
        /// <param name="space">棋盘线间隔</param>
        /// </summary>
        public static void InitPadInfo(int count, int space)
        {
            lineCount = count;
            crossArry = new List<List<Point>>();            
            pcsClsFlg = new int[count, count];
            padWid = (int)((count - 1) * space);
            lineSpace = space;

            for (int h = 0; h < lineCount; h++)
            {
                List<Point> listPt = new List<Point>();
                Point pt = new Point();
                for (int i = 0; i < lineCount; i++)
                {
                    pt.X = i * lineSpace;
                    pt.Y = h * lineSpace;
                    listPt.Add(pt);
                }
                crossArry.Add(listPt);
            }
        }

        /// <summary>
        /// 把输入点的坐标转换成棋盘中的列和行的编号的点
        /// </summary>
        /// <param name="point">输入坐标点</param>
        /// <returns>返回贴近点在棋盘上的行、列编号</returns>
        public static Point GetRCSeir(int x, int y)
        {
            Point pt = new Point();

            for (int i = 0; i < lineCount; i++)
            {
                //当输入点的x坐标值小于棋盘的某一列的值
                if (x - (int)crossArry[0][i].X <= 0)
                {
                    if (x - (int)crossArry[0][i].X == 0)
                    {
                        pt.X = i;
                        break;
                    }
                    else
                    {
                        //选择x值左右两列中离x值近的列
                        pt.X = Math.Abs(x - (int)crossArry[0][i].X) < Math.Abs(x - (int)crossArry[0][i - 1].X) ? i : i - 1;
                        break;
                    }
                }   
            }

            for (int i = 0; i < lineCount; i++)
            {
                if (y - (int)crossArry[i][0].Y <= 0)
                {
                    if (y - (int)crossArry[i][0].Y == 0)
                    {
                        pt.Y = i;
                        break;
                    }
                    else
                    {
                        pt.Y = Math.Abs(y - (int)crossArry[i][0].Y) < Math.Abs(y - (int)crossArry[i - 1][0].Y) ? i : i - 1;
                        break;
                    }
                }                
            }

            return pt;
        }

        public static int[] GetResult(Point pt, int flag)
        {
            int[] result = new int[2];

            int xMin = 0;
            int xMax = 0;

            if (pt.X<=4)
            {
                xMin = 0;
                xMax = pt.X+4 ;
            }
            else if (pt.X>4&&pt.X<lineCount-5)
            {
                xMin = pt.X - 4;
                xMax = pt.X + 4;
            }
            else
            {
                xMin = pt.X - 4;
                xMax = lineCount - 1;
            }

            for (int i = xMin; i <= xMax - 4; i++)
            {
                result[0] = 0;
                for (int h = 0; h < 5; h++)
                {
                    if (pcsClsFlg[pt.Y,  i + h] == flag)
                        ++result[0];
                    else
                        --result[0];
                }

                if (result[0] == 5)                
                    break;

            }

            result[1] = flag;

            return result;
        }

        
    }
}
