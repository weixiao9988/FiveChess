using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FiveChess
{
    static class Chess
    {
        /// <summary>
        /// 棋盘线总数
        /// </summary>
        public static int LineCount { get; set; }
        
        /// <summary>
        /// 棋盘宽度
        /// </summary>
        public static int PadWid { get; set; }
        
        /// <summary>
        /// 棋盘线间距
        /// </summary>
        public static int LineSpace { get; set; }

        /// <summary>
        /// 棋子大小
        /// </summary>
        public static int PcsSize { get; set; } = 16;

        public static int None { get; } = 0;

        public static int Black { get; } = 1;

        public static int White { get; } = 2;

        /// <summary>
        /// 标志是不是我方棋子
        /// </summary>
        public static bool isMyPcs { get; set; } = true;

        /// <summary>
        /// 棋盘交叉点坐标数组
        /// </summary>
        public static List<List<Point>> crossArry;
                
        /// <summary>
        /// 棋盘中棋子的颜色标志，0:空，1:黑色，2:白色，5:黑色最边缘棋子，6:白色最边缘棋子
        /// </summary>
        public static List<List<int>> pcsFlg;

        public static List<Color> pcsColors = new List<Color>();

        /// <summary>
        /// 棋子在棋盘中的行列编号，x：列编号，y：行编号
        /// </summary>
        public static Point XYSeir = new Point();


        /// <summary>
        /// 初始化棋盘信息
        /// </summary>
        /// <param name="count">棋盘线总数</param>
        /// <param name="space">棋盘线间距</param>
        public static void InitPadInfo(int count, int space)
        {
            LineCount = count;
            crossArry = new List<List<Point>>();
            pcsFlg = new List<List<int>>();
            PadWid = (int)((count - 1) * space);
            LineSpace = space;
            isMyPcs = true;

            for (int h = 0; h < LineCount; h++)
            {
                List<Point> listPt = new List<Point>();
                List<int> listInt = new List<int>();
                Point pt = new Point();
                for (int i = 0; i < LineCount; i++)
                {
                    pt.X = i * LineSpace;
                    pt.Y = h * LineSpace;
                    listPt.Add(pt);
                    listInt.Add(0);
                }
                crossArry.Add(listPt);
                pcsFlg.Add(listInt);
            }
        }

        /// <summary>
        /// 找到离输入点最近的棋盘交叉点并返回列、行编号点
        /// </summary>
        /// <param name="x">输入点x坐标</param>
        /// <param name="y">输入点y坐标</param>
        /// <returns>返回贴近点在棋盘上的行、列编号</returns>
        public static Point GetRCSeir(int x, int y)
        {
            Point pt = new Point();

            for (int i = 0; i < LineCount; i++)
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

            for (int i = 0; i < LineCount; i++)
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

        /// <summary>
        /// 获取落子后在四个方向一定范围内各自的棋子信息和坐标
        /// </summary>
        /// <param name="lstPad">棋盘信息</param>
        /// <param name="pt">输入点</param>
        /// <param name="incr">距离落子的范围</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="pcsInfo">输出棋子信息</param>
        /// <param name="posInfo">输出坐标信息</param>
        public static void GetPointRoundInfo(List<List<int>> lstPad, Point pt, int incr, int lineCount, out List<string> pcsInfo, out List<List<Point>> posInfo)
        {
            int[] xArr = GetMinMax(pt.X, lineCount, incr);
            int[] yArr = GetMinMax(pt.Y, lineCount, incr);
            int vMin, vMax;
            List<string> pcsLSt = new List<string>();
            List<List<Point>> posLst = new List<List<Point>>();
            for (int t = 1; t <= 4; t++)
            {
                string str = null;
                List<Point> pts = new List<Point>();
                switch (t)
                {
                    case 1:  /// 根据输入点和范围返回水平方向结果
                        {
                            vMin = xArr[0];
                            vMax = xArr[1];
                            for (int i = vMin; i <= vMax; i++)
                            {
                                str += lstPad[pt.Y][i].ToString();
                                pts.Add(new Point(i, pt.Y));
                            }
                            pcsLSt.Add(str);
                            posLst.Add(pts);
                            break;
                        }
                    case 2:     // 根据输入点和范围返回垂直方向结果
                        {
                            vMin = yArr[0];
                            vMax = yArr[1];
                            for (int i = vMin; i <= vMax; i++)
                            {
                                str += lstPad[i][pt.X].ToString();
                                pts.Add(new Point(pt.X, i));
                            }
                            pcsLSt.Add(str);
                            posLst.Add(pts);
                            break;
                        }
                    case 3:     // 根据输入点和范围返回撇[/]方向结果
                        {
                            vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X;
                            vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                            {
                                str += lstPad[pt.Y + i][pt.X - i].ToString();
                                pts.Add(new Point(pt.X - i, pt.Y + i));
                            }
                            pcsLSt.Add(str);
                            posLst.Add(pts);
                            break;
                        }
                    case 4:    // 根据输入点和范围返回捺[\]方向结果
                        {
                            vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
                            vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                            {
                                str += lstPad[pt.Y + i][pt.X + i].ToString();
                                pts.Add(new Point(pt.X + i, pt.Y + i));
                            }
                            pcsLSt.Add(str);
                            posLst.Add(pts);
                            break;
                        }
                    default:
                        break;
                }
            }
            pcsInfo = pcsLSt;
            posInfo = posLst;
        }

        /// <summary>
        /// 根据输入的值计算可能的最小、最大编号;
        /// </summary>
        /// <param name="inVal">输入值</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="incr">距离落子的范围</param>
        /// <returns>最小、最大编号数组，[0]：最小，[1]：最大</returns>
        public static int[] GetMinMax(int inVal, int lineCount, int incr)
        {
            int[] arr = new int[2];
            //根据输入点计算可能的最小、最大编号;
            if (inVal > incr && inVal < lineCount - incr)
            {
                arr[0] = inVal - incr;
                arr[1] = inVal + incr;
            }
            else
            {
                arr[0] = inVal <= incr ? 0 : inVal - incr;
                arr[1] = inVal <= incr ? inVal + incr : lineCount - 1;
            }
            return arr;
        }

        

        public static void SavePcsFlg(Point pt, int flg)
        {
            if (pt.X==0||pt.X==14||pt.Y==0||pt.Y==14 ||
                (pt.X==1&&pt.Y==1) || (pt.X == 1 && pt.Y == 13) || 
                (pt.X == 13 && pt.Y == 1) || (pt.X == 13 && pt.Y == 13))
            {
                pcsFlg[pt.Y][pt.X] = flg+4;
            }
            else
            {
                pcsFlg[pt.Y][pt.X] = flg;
            }
        }
        
    }
}
