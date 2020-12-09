using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FiveChess
{
    class Judge
    {
        private List<List<int>> pcsFlag;
        private int lineCount;
        private int[][] result = new int[3][];
        private Method myMethod;

        public Judge()
        {
            pcsFlag = Chess.pcsFlag;
            lineCount = Chess.LineCount;
            myMethod = new Method();
            for (int i = 0; i < 3; i++)
                result[i] = new int[2];
        }
        public Judge(List<List<int>> arry)
        {
            pcsFlag = arry;
            lineCount= arry[0].Count;
            result[0] = new int[2];
            result[1] = new int[2];
            result[2] = new int[2];
        }

        public void InitData()
        {
            pcsFlag = Chess.pcsFlag;
            lineCount = Chess.LineCount;

            for (int i = 0; i < 3; i++)
                result[i] = new int[2];
        }

        /// <summary>
        /// 落子后判断胜负
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">棋子标志</param>    
        /// <param name="incr">查找范围</param>    
        /// <returns>返回int[]，[0]棋子标志，[1]连子数量，[2]四个方向之一</returns>
        public int[][] JudgeWin(Point pt, int flg, int incr)
        {
            int[] xArr = myMethod.GetMinMax(pt.X, lineCount, incr);
            int[] yArr = myMethod.GetMinMax(pt.Y, lineCount, incr);
            int vMin, vMax;
            List<int> lstCnn = new List<int>();
            for (int t = 0; t < 4; t++)
            {
                int count = 0, tmpCount = 0;
                switch (t)
                {
                    case 0:  /// 根据输入点和范围返回水平方向结果
                        {
                            vMin = xArr[0]; vMax = xArr[1];
                            for (int i = vMin; i <= vMax; i++)
                            {
                                tmpCount = pcsFlag[i][pt.Y] == flg ? ++tmpCount : 0;
                                count = tmpCount > count ? tmpCount : count;
                            }
                            lstCnn.Add(count);
                            break;
                        }
                    case 1:     // 根据输入点和范围返回垂直方向结果
                        {
                            vMin = yArr[0]; vMax = yArr[1];
                            for (int i = vMin; i <= vMax; i++)
                            {
                                tmpCount = pcsFlag[pt.X][i] == flg ? ++tmpCount : 0;
                                count = tmpCount > count ? tmpCount : count;
                            }
                            lstCnn.Add(count);
                            break;
                        }
                    case 2:     // 根据输入点和范围返回撇[/]方向结果
                        {
                            vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X;
                            vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                            {
                                tmpCount = pcsFlag[pt.X - i][pt.Y + i] == flg ? ++tmpCount : 0;
                                count = tmpCount > count ? tmpCount : count;
                            }
                            lstCnn.Add(count);
                            break;
                        }
                    case 3:    // 根据输入点和范围返回捺[\]方向结果
                        {
                            vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
                            vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                            {
                                tmpCount = pcsFlag[pt.X + i][pt.Y + i] == flg ? ++tmpCount : 0;
                                count = tmpCount > count ? tmpCount : count;
                            }
                            lstCnn.Add(count);
                            break;
                        }
                    default:
                        break;
                }

                if (lstCnn.Max() >= 5)
                {
                    result[flg][0] = count;
                    result[flg][1] = t;
                    return result;
                }
            }

            result[flg][0] = lstCnn.Max();
            result[flg][1] = lstCnn.IndexOf(lstCnn.Max());
            return result;
        }
        public int[] JudgeWin(Point pt, int flg, int incr, List<List<int>> padInfo)
        {
            int[] result = { flg, 0, 0 };
            string part = @flg.ToString() + "+";        //正则表达式
            int[] xArr = myMethod.GetMinMax(pt.X, lineCount, incr);
            int[] yArr = myMethod.GetMinMax(pt.Y, lineCount, incr);
            int vMin, vMax;

            for (int t = 0; t < 4; t++)
            {
                string str = null;
                switch (t)
                {
                    case 0:  /// 根据输入点和范围返回水平方向结果
                        {
                            vMin = xArr[0];
                            vMax = xArr[1];
                            for (int i = vMin; i <= vMax; i++)
                                str += pcsFlag[i][pt.Y].ToString();
                            break;
                        }
                    case 1:     // 根据输入点和范围返回垂直方向结果
                        {
                            vMin = yArr[0];
                            vMax = yArr[1];
                            for (int i = vMin; i <= vMax; i++)
                                str += pcsFlag[pt.X][i].ToString();
                            break;
                        }
                    case 2:     // 根据输入点和范围返回撇[/]方向结果
                        {
                            vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X;
                            vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                                str += pcsFlag[pt.X - i][pt.Y + i].ToString();
                            break;
                        }
                    case 3:    // 根据输入点和范围返回捺[\]方向结果
                        {
                            vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
                            vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                                str += pcsFlag[pt.X + i][pt.Y + i].ToString();
                            break;
                        }
                    default:
                        break;
                }
                MatchCollection matchs = Regex.Matches(str, part);
                result[1] = matchs[0].Length > result[1] ? matchs[0].Length : result[1];
                foreach (Match item in matchs)
                {
                    if (item.Length >= 5)
                    {
                        result[1] = 5;
                        result[2] = t;
                        return result;
                    }

                    if (item.Length > result[1])
                    {
                        result[1] = item.Length;
                        result[2] = t;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 分析棋盘信息
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="rank">智力等级</param>
        public Point AnalysePadInfo(Point pt, int flg, int rank)
        {
            Point returnPt = new Point();
            switch (rank)
            {
                case 0:
                    returnPt = LowLevelAI(pt, flg);
                    break;
                case 1:
                    returnPt = PrimaryAI();
                    break;
                case 2:
                    returnPt = MiddleAI();
                    break;
                default:
                    break;
            }
            return returnPt;
        }

        /// <summary>
        /// 低级机器智力
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">输入点标志</param>
        /// <returns>返回结果</returns>
        public Point LowLevelAI(Point pt, int flg)
        {
            Point returnPt;// = new Point();
            /// 存储4个方向的棋子信息
            List<string> lstPcsInfo;// = new List<string>();
            /// 存储4个方向的坐标信息           
            List<List<Point>> lstPosInfo;// = new List<List<Point>>();

            myMethod.GetPointRoundInfo(Chess.pcsFlag, pt, 4, flg, out lstPcsInfo, out lstPosInfo);
            List<Point> pts = myMethod.GetMaxCnnInfo(flg, lstPcsInfo, lstPosInfo);
            returnPt = myMethod.GetPcsPos(Chess.pcsFlag, Chess.LineCount, pts);

            return returnPt;
        }

        /// <summary>
        /// 初级机器智力
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">输入点标志</param>
        /// <returns></returns>
        public Point PrimaryAI()
        {
            Point returnPt = new Point();
            //黑色棋子的判断、评分
            string blackPcsType = null;    //最高得分的棋型
            int blackPcsScore = 0;     //最高得分
            List<Point> blackPcsScorePos = null;     //最高得分的棋型的坐标
            bool hasValue;
            //遍历所有黑子的落点，查找形成的评分最高的棋型
            for (int i = 0; i < Chess.blackPtsLst.Count; i++)
            {
                hasValue = myMethod.GetPcsTypeScorePos(Chess.blackPtsLst[i], 1, Chess.TypeScore,
                    out string rePcsType, out int rePcsScore, out List<Point> rePcsScorePos);
                if (hasValue && rePcsScore > blackPcsScore)
                {
                    blackPcsScore = rePcsScore;
                    blackPcsType = rePcsType;
                    blackPcsScorePos = rePcsScorePos;
                }
            }

            //在棋型中找个要落子的空位，使得新棋型评分最高
            Dictionary<int, Point> bDict = myMethod.GetScoreAndPos(blackPcsType, blackPcsScorePos, Chess.TypeScore, 1);

            //-----------------------------------------------------------------------------------------------//
            //白色棋子的判断、评分
            string whitePcsType = null;     //最高得分的棋型
            int whitePcsScore = 0;     //最高得分
            List<Point> whitePcsScorePos = null;     //最高得分的棋型的坐标

            Dictionary<string, int> minax = myMethod.GetPosMinMax(Chess.whitePtsLst);
            for (int col = minax["yMin"]; col <= minax["yMax"]; col++)
            {
                for (int row = minax["xMin"]; row <= minax["xMax"]; row++)
                {
                    //获得评分最高的棋型及相对应的坐标数组            
                    hasValue = myMethod.GetPcsTypeScorePos(new Point(row, col), 2, Chess.TypeScore,
                        out string rePcsType, out int rePcsScore, out List<Point> rePcsScorePos);
                    if (rePcsScore >= 100000)
                        return new Point(row, col);

                    if (hasValue && rePcsScore > whitePcsScore)
                    {
                        whitePcsScore = rePcsScore;
                        whitePcsType = rePcsType;
                        whitePcsScorePos = rePcsScorePos;
                    }
                }
            }
            //获得最高评分和点
            Dictionary<int, Point> wDict = myMethod.GetScoreAndPos(whitePcsType, whitePcsScorePos, Chess.TypeScore, 2);

            if (whitePcsScore >= 100000)
                return wDict.Values.FirstOrDefault();
            else if (whitePcsScore >= 10000)
                returnPt = blackPcsScore >= 10000 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();
            else
                returnPt = blackPcsScore > whitePcsScore || blackPcsScore >= 1200 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();


            return returnPt;
        }

        /// <summary>
        /// 中级机器智力
        /// </summary>
        /// <returns></returns>
        public Point MiddleAI()
        {
            Point returnPt = new Point();


            return returnPt;
        }

        /// <summary>
        /// 在输入点周围随意找一个点
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="max">棋盘界限</param>
        /// <param name="padInfo">棋盘信息</param>
        /// <returns></returns>
        public Point GetRandPt(Point pt)
        {
            Point tmp = new Point(-1, -1);
            for (int incr = 1; incr < lineCount; incr++)
            {
                for (int j = -incr; j <= incr; j++)
                {
                    for (int i = -incr; i <= incr; i++)
                    {
                        int x = pt.X + j;
                        int y = pt.Y + i;
                        //找到的位置不能越界，且不能有其他棋子
                        if (x >= 0 && x < lineCount && y >= 0 && y < lineCount && pcsFlag[x][y] == 0)
                        {
                            tmp.X = x;
                            tmp.Y = y;
                            return tmp;
                        }
                    }
                }
                if (tmp.X != -1)
                    return tmp;
            }
            return tmp;
        }

        /// <summary>
        /// 根据输入点，在棋盘中查找相连5子的首尾坐标
        /// </summary>
        /// <param name="pcsFlag">棋盘</param>
        /// <param name="pt">输入点</param>
        /// <param name="flag">棋子标志</param>
        /// <param name="direct">方向</param>
        /// <param name="incr">范围</param>
        /// <returns>首位棋子的坐标</returns>
        public List<Point> GetFiveCnnPos(List<List<int>> pcsFlag, Point pt, int flag, int direct, int incr)
        {
            List<Point> pts = new List<Point>();
            int[] xArr = myMethod.GetMinMax(pt.X, pcsFlag.Count, incr);
            int[] yArr = myMethod.GetMinMax(pt.Y, pcsFlag.Count, incr);
            int vMin, vMax;

            switch (direct)
            {
                case 0:
                    vMin = xArr[0]; vMax = xArr[1];
                    for (int i = vMin; i <= vMax; i++)
                    {
                        if (pcsFlag[i][pt.Y] == flag && pcsFlag[i + 1][pt.Y] == flag && pcsFlag[i + 2][pt.Y] == flag &&
                            pcsFlag[i + 3][pt.Y] == flag && pcsFlag[i + 4][pt.Y] == flag)
                        {
                            pts.Add(new Point(i, pt.Y));
                            pts.Add(new Point(i + 4, pt.Y));
                            return pts;
                        }
                    }
                    break;
                case 1:
                    vMin = xArr[0]; vMax = xArr[1];
                    for (int i = vMin; i <= vMax; i++)
                    {
                        if (pcsFlag[pt.X][i] == flag && pcsFlag[pt.X][i + 1] == flag && pcsFlag[pt.X][i + 2] == flag &&
                            pcsFlag[pt.X][i + 3] == flag && pcsFlag[pt.X][i + 4] == flag)
                        {
                            pts.Add(new Point(pt.X, i));
                            pts.Add(new Point(pt.X, i + 4));
                            return pts;
                        }
                    }
                    break;
                case 2:
                    vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X;
                    vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;
                    for (int i = -vMin; i <= vMax; i++)
                    {
                        if (pcsFlag[pt.X - i][pt.Y + i] == flag && pcsFlag[pt.X - i - 1][pt.Y + i + 1] == flag && pcsFlag[pt.X - i - 2][pt.Y + i + 2] == flag &&
                            pcsFlag[pt.X - i - 3][pt.Y + i + 3] == flag && pcsFlag[pt.X - i - 4][pt.Y + i + 4] == flag)
                        {
                            pts.Add(new Point(pt.X - i, pt.Y + i));
                            pts.Add(new Point(pt.X - i - 4, pt.Y + i + 4));
                            return pts;
                        }
                    }
                    break;
                case 3:
                    vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
                    vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;
                    for (int i = -vMin; i <= vMax; i++)
                    {
                        if (pcsFlag[pt.X + i][pt.Y + i] == flag && pcsFlag[pt.X + i + 1][pt.Y + i + 1] == flag && pcsFlag[pt.X + i + 2][pt.Y + i + 2] == flag &&
                           pcsFlag[pt.X + i + 3][pt.Y + i + 3] == flag && pcsFlag[pt.X + i + 4][pt.Y + i + 4] == flag)
                        {
                            pts.Add(new Point(pt.X + i, pt.Y + i));
                            pts.Add(new Point(pt.X + i + 4, pt.Y + i + 4));
                            return pts;
                        }
                    }
                    break;
                default:
                    break;
            }

            return pts;
        }



    }
}
