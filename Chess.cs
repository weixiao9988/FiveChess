using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FiveChess
{
    static class Chess
    {
        #region///属性和成员
        /// <summary>
        /// 棋盘线总数
        /// </summary>
        public static int LineCount { get; set; }

        /// <summary>
        /// 棋盘宽度
        /// </summary>
        public static int PadWidth { get; set; }

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
        public static bool IsMyPcs { get; set; } = true;

        /// <summary>
        /// 棋盘交叉点坐标数组
        /// </summary>
        public static List<List<Point>> crossPoint;

        /// <summary>
        /// 棋盘中棋子的颜色标志，0:空，1:黑色，2:白色
        /// </summary>
        public static List<List<int>> pcsFlag = new List<List<int>>();

        public static List<Color> pcsColors = new List<Color>();

        /// <summary>
        /// 棋子在棋盘中的行列编号，x：列编号，y：行编号
        /// </summary>
        public static Point XYSerial = new Point();

        public static List<Point> blackPtsLst = new List<Point>();
        public static List<Point> whitePtsLst = new List<Point>();
        #endregion

        #region //各种棋型的评分值
        /// <summary>
        /// 各种棋型的评分值
        /// </summary>
        public static Dictionary<string, int> TypeScore = new Dictionary<string, int>()
        {
            ["AAAAA"] = LIVE5, /*连5型*/
            ["0AAAA0"] = LIVE4, /*活4型*/
            ["0AAAA"] = CNN4, /*冲4型*/
            ["AAAA0"] = CNN4, /*冲4型*/
            ["A0AAA"] = SPRT4, /*冲4型*/
            ["AA0AA"] = SPRT4, /*冲4型*/
            ["AAA0A"] = SPRT4, /*冲4型*/
            ["00AAA0"] = LIVE3, /*活3型*/
            ["0AAA00"] = LIVE3, /*活3型*/
            ["0A0AA0"] = LIVE3, /*活3型*/
            ["0AA0A0"] = LIVE3, /*活3型*/
            ["0AAA0"] = LIVE3, /*眠3型*/
            ["00AAA"] = CNN3, /*眠3型*/            
            ["AAA00"] = CNN3, /*眠3型*/
            ["0A0AA"] = SPRT3, /*眠3型*/
            ["0AA0A"] = SPRT3, /*眠3型*/
            ["A0AA0"] = SPRT3, /*眠3型*/
            ["AA0A0"] = SPRT3, /*眠3型*/
            ["AA00A"] = SPRT3, /*眠3型*/
            ["A0A0A"] = SPRT3, /*眠3型*/
            ["A00AA"] = SPRT3, /*眠3型*/            
            ["00AA0"] = LIVE2, /*眠2型*/
            ["0AA00"] = LIVE2, /*眠2型*/
            ["000AA"] = LIVE2, /*眠2型*/
            ["AA000"] = LIVE2, /*眠2型*/
            ["00A0A"] = SPRT2, /*眠2型*/
            ["0A00A"] = SPRT2, /*眠2型*/
            ["0A0A0"] = SPRT2, /*眠2型*/
            ["A00A0"] = SPRT2, /*眠2型*/
            ["A0A00"] = SPRT2, /*眠2型*/
            ["A000A"] = SPRT2, /*眠2型*/            
            ["000A0"] = LIVE1, /*眠1型*/
            ["00A00"] = LIVE1, /*眠1型*/
            ["0A000"] = LIVE1, /*眠1型*/
            ["0000A"] = LIVE1, /*眠1型*/
            ["A0000"] = LIVE1, /*眠1型*/
            ["00000"] = OTHER
        };
        #endregion

        #region
        /// <summary>
        /// 连5型
        /// </summary>
        public const int LIVE5 = 100000;
        /// <summary>
        /// 活4型，4个相连，两边都有空位
        /// </summary>
        public const int LIVE4 = 40000;
        /// <summary>
        /// 连4型，4个相连，一边有空位
        /// </summary>
        public const int CNN4 = 20000;
        /// <summary>
        /// 跳4型，4个不相连，加一子可成连5
        /// </summary>
        public const int SPRT4 = 10000;
        /// <summary>
        /// 活3型，加一子可形成活4
        /// </summary>
        public const int LIVE3 = 4000;
        /// <summary>
        /// 连3型，加一子可形成连4
        /// </summary>
        public const int CNN3 = 2000;
        /// <summary>
        /// 跳3型，加一子可形成跳4
        /// </summary>
        public const int SPRT3 = 1000;
        /// <summary>
        /// 连2型
        /// </summary>
        public const int LIVE2 = 200;
        /// <summary>
        /// 跳2型
        /// </summary>
        public const int SPRT2 = 100;
        /// <summary>
        /// 连1型
        /// </summary>
        public const int LIVE1 = 10;
        /// <summary>
        /// 其他
        /// </summary>
        public const int OTHER = 0;
        #endregion


        /// <summary>
        /// 初始化棋盘信息
        /// </summary>
        /// <param name="count">棋盘线总数</param>
        /// <param name="space">棋盘线间距</param>
        public static void InitChessBoard(int count, int space)
        {
            crossPoint = new List<List<Point>>();
            LineCount = count;
            PadWidth = (count - 1) * space;
            LineSpace = space;
            PcsSize = space / 2 - 3;
            IsMyPcs = true;

            for (int i = 0; i < LineCount; i++)
            {
                List<Point> pos = new List<Point>();
                Point point = new Point();
                for (int j = 0; j < LineCount; j++)
                {
                    point.X = i * space;
                    point.Y = j * space;
                    pos.Add(point);                   
                }
                crossPoint.Add(pos);                
            }
        }

        /// <summary>
        /// 初始化棋盘的棋子
        /// </summary>
        public static void InitPecies()
        {
            for (int j = 0; j < LineCount; j++)
            {
                List<int> arry = new List<int>();                
                for (int i = 0; i < LineCount; i++)
                    arry.Add(0);
                pcsFlag.Add(arry);
            }           
        }

        /// <summary>
        /// 重置棋盘棋子和保存的棋子
        /// </summary>
        public static void RestData()
        {
            blackPtsLst.Clear();
            whitePtsLst.Clear();

            pcsFlag.Clear();
            IsMyPcs = true;
        }

        /// <summary>
        /// 找到离输入点最近的棋盘交叉点并返回列、行编号点
        /// </summary>
        /// <param name="x">输入点x坐标</param>
        /// <param name="y">输入点y坐标</param>
        /// <returns>返回贴近点在棋盘上的行、列编号</returns>
        public static Point GetXYSerial(int x, int y)
        {
            Point pt = new Point();
            for (int i = 0; i < LineCount; i++)
            {
                //当输入点的x坐标值小于棋盘的某一列的值;
                if (x - crossPoint[i][0].X <= 0)
                {
                    if (x - crossPoint[i][0].X == 0)
                    {
                        pt.X = i;
                        break;
                    }
                    else
                    {//选择x值左右两列中离x值近的列
                        pt.X = Math.Abs(x - crossPoint[i][0].X) < Math.Abs(x - crossPoint[i - 1][0].X) ? i : i - 1;
                        break;
                    }
                }
            }

            for (int i = 0; i < LineCount; i++)
            {
                if (y - crossPoint[0][i].Y <= 0)
                {
                    if (y - crossPoint[0][i].Y == 0)
                    {
                        pt.Y = i;
                        break;
                    }
                    else
                    {//选择x值左右两列中离x值近的列
                        pt.Y = Math.Abs(y - crossPoint[0][i].Y) < Math.Abs(y - crossPoint[0][i - 1].Y) ? i : i - 1;
                        break;
                    }
                }
            }

            return pt;
        }


    }
}
