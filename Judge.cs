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
        public List<List<int>> chessPadInfo;
        private int lineMax;

        ///// <summary>
        ///// 存储4个方向的棋子信息
        ///// </summary>
        //private List<string> lstPcsInfo = new List<string>();

        ///// <summary>
        ///// 存储4个方向的坐标信息
        ///// </summary>
        //private List<List<Point>> lstPosInfo = new List<List<Point>>();
        
        ///// <summary>
        ///// 存储最长相连棋子坐标
        ///// </summary>
        //private List<Point> lstCnnPcsPos = new List<Point>();

        private int[][] result = new int[3][];
        public delegate void UpStatInfo(Point pt, string s);
        public event UpStatInfo UpInfoEvt;

        #region //各种棋型的评分值
        /// <summary>
        /// 各种棋型的评分值
        /// </summary>
        public Dictionary<string, int> TypeScore = new Dictionary<string, int>()
        {
            ["AAAAA"] = Chess.LIVE5, /*连5型*/
            ["0AAAA0"] = Chess.LIVE4, /*活4型*/
            ["0AAAA"] = Chess.CNN4, /*冲4型*/
            ["AAAA0"] = Chess.CNN4, /*冲4型*/
            ["A0AAA"] = Chess.SPRT4, /*冲4型*/
            ["AA0AA"] = Chess.SPRT4, /*冲4型*/
            ["AAA0A"] = Chess.SPRT4, /*冲4型*/
            ["00AAA0"] = Chess.LIVE3, /*活3型*/
            ["0AAA00"] = Chess.LIVE3, /*活3型*/
            ["0A0AA0"] = Chess.LIVE3, /*活3型*/
            ["0AA0A0"] = Chess.LIVE3, /*活3型*/
            ["00AAA"] = Chess.CNN3, /*眠3型*/
            ["0AAA0"] = Chess.CNN3, /*眠3型*/
            ["AAA00"] = Chess.CNN3, /*眠3型*/
            ["0A0AA"] = Chess.SPRT3, /*眠3型*/
            ["0AA0A"] = Chess.SPRT3, /*眠3型*/
            ["A0AA0"] = Chess.SPRT3, /*眠3型*/
            ["AA0A0"] = Chess.SPRT3, /*眠3型*/
            ["AA00A"] = Chess.SPRT3, /*眠3型*/
            ["A0A0A"] = Chess.SPRT3, /*眠3型*/
            ["A00AA"] = Chess.SPRT3, /*眠3型*/
            ["000AA"] = Chess.LIVE2, /*眠2型*/
            ["00AA0"] = Chess.LIVE2, /*眠2型*/
            ["0AA00"] = Chess.LIVE2, /*眠2型*/
            ["AA000"] = Chess.LIVE2, /*眠2型*/
            ["00A0A"] = Chess.SPRT2, /*眠2型*/
            ["0A00A"] = Chess.SPRT2, /*眠2型*/
            ["0A0A0"] = Chess.SPRT2, /*眠2型*/
            ["A00A0"] = Chess.SPRT2, /*眠2型*/
            ["A0A00"] = Chess.SPRT2, /*眠2型*/
            ["A000A"] = Chess.SPRT2, /*眠2型*/
            ["0000A"] = Chess.LIVE1, /*眠1型*/
            ["000A0"] = Chess.LIVE1, /*眠1型*/
            ["00A00"] = Chess.LIVE1, /*眠1型*/
            ["0A000"] = Chess.LIVE1, /*眠1型*/
            ["A0000"] = Chess.LIVE1, /*眠1型*/
            ["00000"] = Chess.OTHER
        };
        #endregion

      
        public Action<int[]> transParAct;

        public Judge(List<List<int>> arry)
        {
            chessPadInfo = arry;
            lineMax = arry[0].Count;
            result[0] = new int[2];
            result[1] = new int[2];
            result[2] = new int[2];
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
                    returnPt = PrimaryAI(pt, flg);
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
        /// 落子后判断胜负
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">棋子标志</param>    
        /// <param name="incr">查找范围</param>    
        /// <returns>返回int[]，[0]棋子标志，[1]连子数量，[2]四个方向之一</returns>
        public int[][] JudgeWin(Point pt, int flg, int incr,List<List<int>> padInfo)
        {
            //string part = @flg.ToString() + "+";        //正则表达式
            int[] xArr = GetMinMax(pt.X, lineMax, incr);
            int[] yArr = GetMinMax(pt.Y, lineMax, incr);
            int vMin, vMax;
            List<int> lstCnn = new List<int>();
            for (int t = 0; t < 4; t++)
            {
                //string str = null;
                int count = 0, tmpCount = 0;
                switch (t)
                {
                    case 0:  /// 根据输入点和范围返回水平方向结果
                        {
                            vMin = xArr[0]; vMax = xArr[1];
                            for (int i = vMin; i <= vMax; i++)
                            {
                                tmpCount = padInfo[pt.Y][i] == flg ? ++tmpCount : 0;                                
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
                                tmpCount = padInfo[i][pt.X] == flg ? ++tmpCount : 0;
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
                                tmpCount = padInfo[pt.Y + i][pt.X - i] == flg ? ++tmpCount : 0;
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
                                tmpCount = padInfo[pt.Y + i][pt.X - i] == flg ? ++tmpCount : 0;
                                count = tmpCount > count ? tmpCount : count;
                                //if (padInfo[pt.Y + i][pt.X + i] == flg)
                                //    tmpCount++;
                                //else
                                //{
                                //    if (tmpCount > count)
                                //        count = tmpCount;
                                //    tmpCount = 0;
                                //}
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
        /// 中级机器智力
        /// </summary>
        /// <returns></returns>
        public Point MiddleAI()
        {
            Point returnPt = new Point() ;
            //黑色棋子的判断、评分;
            string blackPcsType=null;    //最高得分的棋型;
            int blackPcsScore=0;     //最高得分;
            List<Point> blackPcsScorePos=null;     //最高得分的棋型的坐标;
            bool hasValue;
            string rePcsType;
            int rePcsScore;
            List<Point> rePcsScorePos;
            //遍历所有黑子的落点，查找形成的评分最高的棋型;
            for (int i = 0; i < Chess.blackPtsLst.Count; i++)
            {
                hasValue = GetPcsTypeScorePos(Chess.blackPtsLst[i], 1, TypeScore, 
                    out rePcsType, out rePcsScore, out  rePcsScorePos);
                if (hasValue&& rePcsScore> blackPcsScore)
                {
                    blackPcsScore = rePcsScore;
                    blackPcsType = rePcsType;
                    blackPcsScorePos = rePcsScorePos;
                }
            }
            
            //在棋型中找个要落子的空位，使得新棋型评分最高
            Dictionary<int, Point> bDict = GetScoreAndPos(blackPcsType, blackPcsScorePos, TypeScore, 1);

            //-----------------------------------------------------------------------------------------------//
            //白色棋子的判断、评分
            string whitePcsType=null;     //最高得分的棋型
            int whitePcsScore=0;     //最高得分
            List<Point> whitePcsScorePos=null;     //最高得分的棋型的坐标

            Dictionary<string, int> minax = GetPosMinMax(Chess.whitePtsLst);
            for (int col = minax["yMin"]; col <= minax["yMax"]; col++)
            {
                for (int row = minax["xMin"]; row <= minax["xMax"]; row++)
                {
                    //获得评分最高的棋型及相对应的坐标数组            
                    hasValue=GetPcsTypeScorePos(new Point(row,col), 2, TypeScore,
                        out rePcsType, out rePcsScore, out rePcsScorePos);
                    if (rePcsScore >= Chess.LIVE5)
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
            Dictionary<int, Point> wDict = GetScoreAndPos(whitePcsType, whitePcsScorePos, TypeScore, 2);

            if(whitePcsScore >= Chess.LIVE5)
                return wDict.Values.FirstOrDefault();
            else if (whitePcsScore >= Chess.SPRT4)
                returnPt = blackPcsScore >= Chess.SPRT4 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();
             else
                returnPt = blackPcsScore > whitePcsScore || blackPcsScore >= Chess.LIVE3 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();


            return returnPt;
        }

        /// <summary>
        /// 初级机器智力
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">输入点标志</param>
        /// <returns></returns>
        public Point PrimaryAI(Point pt,int flg)
        {
            Point returnPt;// = new Point();
          
            //黑色棋子的判断、评分
            string blackPcsType;// = new List<string>();     //最高得分的棋型
            int blackPcsScore;// = new List<int>();     //最高得分
            List<Point> blackPcsScorePos;// = new List<List<Point>>();     //最高得分的棋型的坐标
            //获得评分最高的棋型及相对应的坐标数组            
            GetPcsTypeScorePos(pt, flg, TypeScore, out blackPcsType, out blackPcsScore, out blackPcsScorePos);
                      
            //在棋型中找个要落子的空位，使得新棋型评分最高
            Dictionary<int, Point> bDict = GetScoreAndPos(blackPcsType, blackPcsScorePos, TypeScore, 1);
            
            //-----------------------------------------------------------------------------------------------//
            //白色棋子的判断、评分
            string whitePcsType;// = new List<string>();     //最高得分的棋型
            int whitePcsScore;// = new List<int>();     //最高得分
            List<Point> whitePcsScorePos;// = new List<List<Point>>();     //最高得分的棋型的坐标
            // 获得评分最高的棋型及相对应的坐标数组
            bool hasRslt = GetPcsTypeScorePos(Chess.whitePtsLst.Last(), 2, TypeScore, out whitePcsType, out whitePcsScore, out whitePcsScorePos);
                        
            Dictionary<int, Point> wDict;
            if (hasRslt)
            {
                //获得最高评分和点
                wDict = GetScoreAndPos(whitePcsType, whitePcsScorePos, TypeScore, 2);
                //获得最大评分的棋型            
                GetPcsTypeScorePos(wDict.Values.FirstOrDefault(), 2, TypeScore, out whitePcsType, out whitePcsScore, out whitePcsScorePos);
                              
                if(whitePcsScore >= Chess.SPRT4)
                    return wDict.Values.FirstOrDefault();
                else                    
                    returnPt = blackPcsScore > whitePcsScore || blackPcsScore >= Chess.SPRT3 ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();
            }
            else
                return bDict.Values.FirstOrDefault();//上一回合白棋落子后如果没有形成有效的棋型时直接返回黑子坐标

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

            GetPointRoundInfo(chessPadInfo, pt, 4, flg, out lstPcsInfo, out lstPosInfo);            
            List<Point> pts = GetMaxCnnInfo(flg, lstPcsInfo, lstPosInfo);
            returnPt = GetPcsPos(chessPadInfo, lineMax, pts);

            return returnPt;
        }

        /// <summary>
        /// 落子后在四个方向一定范围内获取棋盘中的棋子信息和坐标
        /// </summary>
        /// <param name="lstPad">棋盘信息</param>
        /// <param name="pt">输入点</param>
        /// <param name="incr">距离落子的范围</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="pcsInfo">输出棋子信息</param>
        /// <param name="posInfo">输出坐标信息</param>
        public void GetPointRoundInfo(List<List<int>> lstPad, Point pt, int incr, int flg, out List<string> pcsInfo, out List<List<Point>> posInfo)
        {
            int[] xArr = GetMinMax(pt.X, lstPad.Count, incr);
            int[] yArr = GetMinMax(pt.Y, lstPad.Count, incr);
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
                                str = flg == 2 && i == pt.X && lstPad[pt.Y][i] == 0 ? str + flg.ToString() : str + lstPad[pt.Y][i].ToString();
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
                                str = flg == 2 && i == pt.Y && lstPad[i][pt.X] == 0 ? str + flg.ToString() : str + lstPad[i][pt.X].ToString();
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
                                str = flg == 2 && i == 0 && lstPad[pt.Y + i][pt.X - i] == 0 ? str + flg.ToString() : str + lstPad[pt.Y + i][pt.X - i].ToString();
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
                                str = flg == 2 && i == 0 && lstPad[pt.Y + i][pt.X + i] == 0 ? str + flg.ToString() : str + lstPad[pt.Y + i][pt.X + i].ToString();
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
        /// 在输入点周围四个方向获取符合要求的评分最高的棋型、棋型评分、棋型坐标
        /// </summary>
        /// <param name="pcsInfo">输入的棋子信息</param>
        /// <param name="pcsPos">输入的棋子信息的坐标</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="pcsTypeScore">棋子的评分标准，字典格式</param>
        /// <param name="pcsType">返回棋型</param>
        /// <param name="pcsScore">返回棋型得分</param>
        /// <param name="pcsScorePos">返回棋型坐标</param>
        public bool GetPcsTypeScorePos(Point pt, int flg, Dictionary<string,int> pcsTypeScore, 
            out string pcsType, out int pcsScore, out List<Point> pcsScorePos)
        {           
            char tmpChar = flg == 1 ? '1' : '2';
            /// 存储4个方向的棋子信息
            List<string> lstPcsInfo;// = new List<string>();
            List<string> tmpStr = new List<string>();
            /// 存储4个方向的坐标信息           
            List<List<Point>> lstPosInfo;// = new List<List<Point>>();
             //得到四个方向一定范围内的棋子信息                                        
            GetPointRoundInfo(chessPadInfo, pt, 4, flg, out lstPcsInfo, out lstPosInfo);
            
            List<string> pType = new List<string>();
            List<int> pScore = new List<int>();
            List<List<Point>> pScorePos = new List<List<Point>>();
            
            //从四个方向的棋子信息中找出符合的棋型，评分和对应的坐标
            for (int t = 0; t < lstPcsInfo.Count; t++)
            {
                MatchCollection match;
                string tmpS = null;
                List<Point> pts = new List<Point>();
                foreach (string type in pcsTypeScore.Keys)
                {
                    tmpS = GetNewStr(type, flg);
                    match = Regex.Matches(lstPcsInfo[t], tmpS);
                    if (match.Count>0)
                    {
                        for (int i = match[0].Index; i < match[0].Index + match[0].Length; i++)
                            pts.Add(lstPosInfo[t][i]);
                        break;
                    }
                }
                if (pts.Count > 0)
                {
                    pType.Add(tmpS);
                    pScore.Add(pcsTypeScore[RestOldStr(tmpS, flg)]);
                    pScorePos.Add(pts); 
                }
            }
           
            pcsScore = pScore.Count > 0 ? pScore[pScore.IndexOf(pScore.Max())] : 0;
            pcsType = pScore.Count > 0 ? pType[pScore.IndexOf(pScore.Max())] : null;
            pcsScorePos = pScore.Count > 0 ? pScorePos[pScore.IndexOf(pScore.Max())] : null;
            
            return pScore.Count > 0;
        }

        /// <summary>
        /// 在棋型中寻找一个空位，使得在此位置落子后得分最高
        /// </summary>
        /// <param name="type">棋型</param>
        /// <param name="pts">棋型对应的坐标点</param>
        /// <param name="typeScore">棋型评分字典</param>
        /// <param name="flag">棋子标志</param>
        /// <returns>返回一个点和此点的评分</returns>
        public Dictionary<int,Point> GetScoreAndPos(string type, List<Point> pts, Dictionary<string,int> typeScore, int flag)
        {
            Dictionary<int, Point> dict = new Dictionary<int, Point>();
            char tch = flag == 1 ? '1' : '2';
            //在确定的棋型中增加一个棋子，使新棋型得分最高
            int idx = 0, smax = typeScore[RestOldStr(type, flag)];
            for (int i = 0; i < type.Length; i++)
            {
                if (type[i] == '0')
                {
                    StringBuilder tStr = new StringBuilder(type);
                    tStr.Replace('0', tch, i, 1);
                    string s = RestOldStr(tStr.ToString(), flag);
                    if (typeScore.ContainsKey(s) && typeScore[s] > smax)
                    {
                        smax = typeScore[s];
                        idx = i;
                    }
                }
            }
            
            dict.Add(smax, pts[idx]);
            return dict;
        }

        /// <summary>
        /// 得到相连棋子旁边的一个点
        /// </summary>
        /// <param name="chessPad">棋盘中棋子的信息</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="lstPts">相连棋子的坐标</param>
        /// <param name="direct">相连棋子的方向[- | / \]</param>
        /// <returns></returns>
        private Point GetPcsPos(List<List<int>> chessPad, int lineCount, List<Point> lstPts)
        {
            int direct = JudgePtsDirect(lstPts);
            Point backPt = new Point(-1,-1);
            switch (direct)
            {
                case 0: //在水平方向相连的两端落子，如果两端都不能落子返回(-1,-1)点
                    {
                        Point fistPt = new Point(lstPts[0].X - 1, lstPts[0].Y);
                        Point endPt = new Point(lstPts[lstPts.Count - 1].X + 1, lstPts[0].Y);
                        if (bInPad(fistPt, lineCount) && bInPad(endPt, lineCount))
                        {
                            if (chessPad[fistPt.Y][fistPt.X] == 0)
                                backPt = fistPt;
                            else if (chessPad[endPt.Y][endPt.X] == 0)
                                backPt = endPt;
                            else
                                backPt = new Point(-1, -1);
                        }                           
                        break;
                    }
                case 1:     //在垂直方向相连的两端落子，如果两端都不能落子返回(-1,-1)点
                    {
                        Point fistPt = new Point(lstPts[0].X , lstPts[0].Y-1);
                        Point endPt = new Point(lstPts[0].X , lstPts[lstPts.Count - 1].Y+1);
                        if (bInPad(fistPt, lineCount) && bInPad(endPt, lineCount))
                        {
                            if (chessPad[fistPt.Y][fistPt.X] == 0)
                                backPt = fistPt;
                            else if (chessPad[endPt.Y][endPt.X] == 0)
                                backPt = endPt;
                            else
                                backPt = new Point(-1, -1);
                        }
                        break;
                    }
                case 2:     //在撇[/]方向相连的两端落子，如果两端都不能落子返回(-1,-1)点
                    {
                        Point fistPt = new Point(lstPts[0].X+1, lstPts[0].Y - 1);
                        Point endPt = new Point(lstPts[lstPts.Count - 1].X-1, lstPts[lstPts.Count - 1].Y + 1);
                        if (bInPad(fistPt, lineCount) && bInPad(endPt, lineCount))
                        {
                            if (chessPad[fistPt.Y][fistPt.X] == 0)
                                backPt = fistPt;
                            else if (chessPad[endPt.Y][endPt.X] == 0)
                                backPt = endPt;
                            else
                                backPt = new Point(-1, -1);
                        }
                        break;
                    }
                case 3:     //在捺[\]方向相连的两端落子，如果两端都不能落子返回(-1,-1)点
                    {
                        Point fistPt = new Point(lstPts[0].X - 1, lstPts[0].Y - 1);
                        Point endPt = new Point(lstPts[lstPts.Count - 1].X + 1, lstPts[lstPts.Count - 1].Y + 1);
                        if (bInPad(fistPt, lineCount) && bInPad(endPt, lineCount))
                        {
                            if (chessPad[fistPt.Y][fistPt.X] == 0)
                                backPt = fistPt;
                            else if (chessPad[endPt.Y][endPt.X] == 0)
                                backPt = endPt;
                            else
                                backPt = new Point(-1, -1);
                        }                       
                        break;
                    }
                default:
                    break;
            }
            return backPt;
        }

        /// <summary>
        /// 得到最大连接时棋子的连接信息和坐标信息
        /// </summary>
        /// <param name="flg">棋子标志</param>
        /// <param name="pcsInfo">输入的棋子信息</param>
        /// <param name="posInfo">输入的坐标信息</param>
        /// <returns>返回字典类型</returns>
        public List<Point> GetMaxCnnInfo(int flg, List<string> pcsInfo, List<List<Point>> posInfo)
        {
            string part = @flg.ToString() + "+";        //正则表达式
            List<int> count = new List<int>() { };
            List<List<Point>> tmpLstPts = new List<List<Point>>() { };

            //查找4个方向
            for (int i = 0; i < 4; i++)
            {
                List<Point> tmpPts = new List<Point>();
                //搜索字符串中标志相同的连在一起的
                MatchCollection match = Regex.Matches(pcsInfo[i], part);
                Match mt;
                if (match.Count > 0)
                {
                    mt = match[0];
                    foreach (Match item in match)
                    {
                        if (item.Length > mt.Length)
                            mt = item;
                    }
                    for (int k = mt.Index; k < mt.Length + mt.Index; k++)
                    {
                        tmpPts.Add(posInfo[i][k]);
                    }
                    tmpLstPts.Add(tmpPts);
                    count.Add(mt.Length);
                }               
            }
            
            return tmpLstPts[count.IndexOf(count.Max())];
        }

        /// <summary>
        /// 返回对应标志的棋型字符串
        /// </summary>
        /// <param name="str">通用的棋型字符串</param>
        /// <param name="flag">棋子标志</param>
        /// <returns>对应棋子的棋型字符串</returns>
        public string GetNewStr(string str, int flag)
        {
            string s = null;
            switch (flag)
            {
                case 1:
                    s = str.Replace('A', '1');
                    s = s.Replace('B', '2');
                    s = s.Replace('C', '5');
                    break;
                case 2:
                    s = str.Replace('A', '2');
                    s = s.Replace('B', '1');
                    s = s.Replace('C', '6');
                    break;
                default:
                    break;
            }
            return s;
        }

        /// <summary>
        /// /还原字符串为棋型通用格式
        /// </summary>
        /// <param name="str">标志型的棋型字符串</param>
        /// <param name="flag">棋子标志</param>
        /// <returns></returns>
        public string RestOldStr(string str, int flag)
        {
            string s = null;
            switch (flag)
            {
                case 1:
                    s = str.Replace('1', 'A');
                    s = s.Replace('2', 'B');
                    s = s.Replace('5', 'C');
                    break;
                case 2:
                    s = str.Replace('2', 'A');
                    s = s.Replace('1', 'B');
                    s = s.Replace('6', 'C');
                    break;
                default:
                    break;
            }
            return s;
        }

        /// <summary>
        /// 根据输入的值计算可能的最小、最大编号;
        /// </summary>
        /// <param name="inVal">输入值</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="incr">距离落子的范围</param>
        /// <returns>最小、最大编号数组，[0]：最小，[1]：最大</returns>
        public int[] GetMinMax(int inVal, int lineCount, int incr)
        {
            int[] arr = new int[2];
            //根据输入点计算可能的最小、最大编号;
            if (inVal > incr && inVal < lineCount - incr)
            {
                arr[0]= inVal - incr;
                arr[1] = inVal + incr;
            }
            else
            {
                arr[0] = inVal <= incr ? 0 : inVal - incr;
                arr[1] = inVal <= incr ? inVal + incr : lineCount - 1;
            }
            return arr;
        }

        /// <summary>
        /// 判断输入点是否在棋盘内
        /// </summary>
        /// <param name="pt">输入点</param>    
        /// <param name="lineCount">棋盘线总数</param>
        /// <returns>在棋盘内返回true，否则返回false</returns>
        public bool bInPad(Point pt, int lineCount)
        {
            if (pt.X >= 0 && pt.X < lineCount && pt.Y >= 0 && pt.Y < lineCount)
                return true;
            return false;
        }

        /// <summary>
        /// 根据输入的坐标点判断方向
        /// </summary>
        /// <param name="pts">坐标点数组</param>
        /// <returns>返回横竖撇捺【-|/\】中的一个</returns>
        public int JudgePtsDirect(List<Point> pts)
        {
            int direct;
            if (pts.Count > 1)
            {
                int x = pts[1].X - pts[0].X;
                int y = pts[1].Y - pts[0].Y;

                if (x > 0 && y == 0)
                    direct = 0;
                else if (x == 0 & y > 0)
                    direct = 1;
                else if (x < 0 && y > 0)
                    direct = 2;
                else
                    direct = 3;
            }
            else
                direct = 0;            

            return direct;
        }

        /// <summary>
        /// 输入一组坐标点，判断点的分布范围
        /// </summary>
        /// <param name="pts">坐标点数组</param>
        /// <returns>返回xy方向的最小和最大值</returns>
        public Dictionary<string,int> GetPosMinMax(List<Point> pts)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            int xMin =pts[0].X, xMax = pts[0].X, yMin = pts[0].Y, yMax = pts[0].Y;
            foreach (Point pt in pts)
            {
                xMin = pt.X < xMin ? pt.X : xMin;
                xMax = pt.X > xMax ? pt.X : xMax;
                yMin = pt.Y < yMin ? pt.Y : yMin;
                yMax = pt.Y> yMax ? pt.Y : yMax;
            }

            dict.Add("xMin", xMin);
            dict.Add("xMax", xMax);
            dict.Add("yMin", yMin);
            dict.Add("yMax", yMax);

            return dict;
        }

    }
}
