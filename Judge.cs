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
        

        /// <summary>
        /// 分析棋盘信息
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="rank">智力等级</param>
        public Point SelectAIRanke(Point pt, int flg, int rank)
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
            ///黑棋每落一个子后会形成相连棋型，白棋策略就是查找黑棋最大相连，然后封堵。
            
            Point returnPt;// = new Point();
            /// 存储4个方向的棋子信息
            List<string> lstPcsInfo;// = new List<string>();
            /// 存储4个方向的坐标信息           
            List<List<Point>> lstPosInfo;// = new List<List<Point>>();

            myMethod.GetPointRoundInfo(Chess.pcsFlag, pt, 4, flg, flg==2, out lstPcsInfo, out lstPosInfo);
            List<Point> pts = myMethod.GetMaxCnnInfo(flg, lstPcsInfo, lstPosInfo);
            returnPt = myMethod.GetPcsPos(Chess.pcsFlag, pts);

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
            
            //遍历所有黑子的落点，查找形成的评分最高的棋型,在棋型中找个要落子的空位，使得新棋型评分最高
            Dictionary<int, Point> bDict = myMethod.GetMaxScoreAndPos(Chess.blackPtsLst, Chess.TypeScore, 1);

            //遍历所有白子的落点，查找形成的评分最高的棋型,在棋型中找个要落子的空位，使得新棋型评分最高
            Dictionary<int, Point> wDictold = myMethod.GetMaxScoreAndPos(Chess.whitePtsLst, Chess.TypeScore, 2);
                        
            Point[] pts = new Point[Chess.whitePtsLst.Count];
            Chess.whitePtsLst.CopyTo(pts);
            List<Point> newWhitePtsLst = pts.ToList();
            newWhitePtsLst.Add(wDictold.Values.FirstOrDefault());

            Dictionary<int, Point> wDict = myMethod.GetMaxScoreAndPos(newWhitePtsLst, Chess.TypeScore, 2);

            if (wDict.Keys.FirstOrDefault() >= Chess.LIVE5)
                return wDict.Values.FirstOrDefault();
            else if (wDict.Keys.FirstOrDefault() >= Chess.SPRT4)
                returnPt = bDict.Keys.FirstOrDefault() >= Chess.SPRT4 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();
            else
                returnPt = bDict.Keys.FirstOrDefault() > wDict.Keys.FirstOrDefault() || bDict.Keys.FirstOrDefault() >= Chess.LIVE3 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();


            return returnPt;
        }

        /// <summary>
        /// 中级机器智力
        /// </summary>
        /// <returns></returns>
        public Point MiddleAI()
        {
            ///策略：白棋落子前先分析一定范围内的空位，在这些空位分别计算黑棋和白棋在4个方向的总分，
            ///然后挑选分值最大的点返回。

            Dictionary<string, int> dict = myMethod.GetRange(Chess.blackPtsLst, Chess.whitePtsLst, lineCount);
            return myMethod.GetMaxScorePos(pcsFlag, dict, Chess.TypeScore);
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
                    vMin = yArr[0]; vMax = yArr[1];
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
