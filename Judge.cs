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
        private List<List<int>> chessPadInfo;
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

        public delegate void UpStatInfo(Point pt, string s);
        public event UpStatInfo UpInfoEvt;

        #region //各种棋型的评分值
        /// <summary>
        /// 各种棋型的评分值
        /// </summary>
        public Dictionary<string, int> TypeScore = new Dictionary<string, int>()
        {
            ["AAAAA"] = 100, /*连5型*/
            ["0AAAA0"] = 90, /*活4型*/
            ["0AAAA"] = 70, /*冲4型*/
            ["AAAA0"] = 70, /*冲4型*/
            ["A0AAA"] = 70, /*冲4型*/
            ["AA0AA"] = 70, /*冲4型*/
            ["AAA0A"] = 70, /*冲4型*/
            ["00AAA0"] = 65, /*活3型*/
            ["0AAA00"] = 65, /*活3型*/
            ["0A0AA0"] = 60, /*活3型*/
            ["0AA0A0"] = 60, /*活3型*/            
            ["00AAA"] = 45, /*眠3型*/
            ["0AAA0"] = 45, /*眠3型*/
            ["0A0AA"] = 40, /*眠3型*/
            ["0AA0A"] = 40, /*眠3型*/            
            ["A0AA0"] = 40, /*眠3型*/
            ["AA0A0"] = 40, /*眠3型*/
            ["AAA00"] = 45, /*眠3型*/
            ["AA00A"] = 40, /*眠3型*/
            ["A0A0A"] = 40, /*眠3型*/
            ["A00AA"] = 40, /*眠3型*/
            ["000AA0"] = 35, /*活2型*/
            ["0AA000"] = 35, /*活2型*/
            ["00AA00"] = 35, /*活2型*/
            ["00A0A0"] = 30, /*活2型*/            
            ["0A00A0"] = 30, /*活2型*/
            ["0A0A00"] = 30, /*活2型*/            
            ["000AA"] = 15, /*眠2型*/
            ["00AA0"] = 15, /*眠2型*/
            ["0AA00"] = 15, /*眠2型*/
            ["AA000"] = 15, /*眠2型*/
            ["00A0A"] = 10, /*眠2型*/            
            ["0A00A"] = 10, /*眠2型*/
            ["0A0A0"] = 10, /*眠2型*/            
            ["A00A0"] = 10, /*眠2型*/
            ["A0A00"] = 10, /*眠2型*/           
            ["A000A"] = 10, /*眠2型*/
            ["0000A0"] = 5, /*活2型*/
            ["000A00"] = 5, /*活2型*/
            ["00A000"] = 5, /*活2型*/
            ["0A0000"] = 5, /*活2型*/
            ["0000A"] = 0, /*眠2型*/
            ["000A0"] = 0, /*眠2型*/
            ["00A00"] = 0, /*眠2型*/
            ["0A000"] = 0, /*眠2型*/
            ["A0000"] = 0, /*眠2型*/
            
        };
        #endregion

      
        public Action<int[]> transParAct;

        public Judge(List<List<int>> arry)
        {
            chessPadInfo = arry;
            lineMax = arry[0].Count;
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
                default:
                    break;
            }
            return returnPt;
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
        /// 获取落子后在四个方向一定范围内各自的棋子信息和坐标
        /// </summary>
        /// <param name="lstPad">棋盘信息</param>
        /// <param name="pt">输入点</param>
        /// <param name="incr">距离落子的范围</param>
        /// <param name="lineCount">棋盘线总数</param>
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
                                str = i == pt.X && lstPad[pt.Y][i] == 0 ? str + flg.ToString() : str +lstPad[pt.Y][i].ToString();
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
                                str = i == pt.Y && lstPad[i][pt.X] == 0 ? str + flg.ToString() : str + lstPad[i][pt.X].ToString();
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
                                str = i ==0 && lstPad[pt.Y + i][pt.X - i] == 0 ? str + flg.ToString() : str + lstPad[pt.Y + i][pt.X - i].ToString();
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
                                str = i == 0 && lstPad[pt.Y + i][pt.X + i] == 0 ? str + flg.ToString() : str + lstPad[pt.Y + i][pt.X + i].ToString();
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
        /// 落子后判断胜负
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">棋子标志</param>    
        /// <param name="incr">查找范围</param>    
        /// <returns>返回int[]，[0]棋子标志，[1]连子数量，[2]四个方向之一</returns>
        public int[] JudgeWin(Point pt, int flg, int incr)
        {
            int[] result={ flg,0,0};
            string part = @flg.ToString() + "+";        //正则表达式
            int[] xArr = GetMinMax(pt.X, lineMax, incr);
            int[] yArr = GetMinMax(pt.Y, lineMax, incr);
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
                                str += chessPadInfo[pt.Y][i].ToString();
                            break;
                        }
                    case 1:     // 根据输入点和范围返回垂直方向结果
                        {
                            vMin = yArr[0];
                            vMax = yArr[1];
                            for (int i = vMin; i <= vMax; i++)
                                str += chessPadInfo[i][pt.X].ToString();
                            break;
                        }
                    case 2:     // 根据输入点和范围返回撇[/]方向结果
                        {
                            vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X;
                            vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                                str += chessPadInfo[pt.Y + i][pt.X - i].ToString();
                            break;
                        }
                    case 3:    // 根据输入点和范围返回捺[\]方向结果
                        {
                            vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
                            vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                                str += chessPadInfo[pt.Y + i][pt.X + i].ToString();
                            break;
                        }
                    default:
                        break;
                }
                MatchCollection matchs = Regex.Matches(str, part);                
                result[1] = matchs[0].Length > result[1] ? matchs[0].Length : result[1];
                foreach (Match item in matchs)
                {
                    if (item.Length>=5)
                    {
                        result[1] = 5;
                        result[2] = t;
                        return result;
                    }

                    if(item.Length > result[1])
                    {
                        result[1] = item.Length;
                        result[2] = t;
                    }                    
                }
            }
            return result;
        }

        public Point PrimaryAI(Point pt,int flg)
        {
            Point returnPt;// = new Point();
          
            //黑色棋子的判断、评分
            List<string> blackPcsType;// = new List<string>();     //四个方向的棋型
            List<int> blackPcsScore;// = new List<int>();     //四个方向的棋型的得分
            List<List<Point>> blackPcsScorePos;// = new List<List<Point>>();     //四个方向的棋型的坐标
                        
            GetPcsTypeScorePos(pt, flg, TypeScore, out blackPcsType, out blackPcsScore, out blackPcsScorePos);

            //获得评分最高的棋型及相对应的坐标数组            
            string tmpblackType = blackPcsType[blackPcsScore.IndexOf(blackPcsScore.Max())];
            List<Point> tmpblackPts = blackPcsScorePos[blackPcsScore.IndexOf(blackPcsScore.Max())];
            //获得最高评分和点
            Dictionary<int, Point> bDict = GetScoreAndPos(tmpblackType, tmpblackPts, TypeScore, 1);
            int blackScore = TypeScore[RestOldStr(tmpblackType,1)];

            //-----------------------------------------------------------------------------------------------//
            //白色棋子的判断、评分
            List<string> whitePcsType;// = new List<string>();     //四个方向的棋型
            List<int> whitePcsScore;// = new List<int>();     //四个方向的棋型的得分
            List<List<Point>> whitePcsScorePos;// = new List<List<Point>>();     //四个方向的棋型的坐标

            GetPcsTypeScorePos(Chess.whitePtsLst.Last(), 2, TypeScore, out whitePcsType, out whitePcsScore, out whitePcsScorePos);
            
            //获得评分最高的棋型及相对应的坐标数组            
            string tmpWhiteType = whitePcsType[whitePcsScore.IndexOf(whitePcsScore.Max())];
            List<Point> tmpWhitePts = whitePcsScorePos[whitePcsScore.IndexOf(whitePcsScore.Max())];
            //获得最高评分和点
            Dictionary<int, Point> wDict = GetScoreAndPos(tmpWhiteType, tmpWhitePts, TypeScore, 2);

            GetPcsTypeScorePos(wDict.Values.FirstOrDefault(), 2, TypeScore, out whitePcsType, out whitePcsScore, out whitePcsScorePos);

            //获得最大评分的棋型            
            tmpWhiteType = whitePcsType[whitePcsScore.IndexOf(whitePcsScore.Max())];
            //得到棋型的评分
            int whiteScore = TypeScore[RestOldStr(tmpWhiteType,2)];

            returnPt = blackScore >= whiteScore ? bDict.Values.FirstOrDefault() : wDict.Values.FirstOrDefault();

            //UpInfoEvt(dict.Values.FirstOrDefault(), flg.ToString());
            
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
        /// 根据输入的棋子信息获得棋型、棋型评分、棋型坐标
        /// </summary>
        /// <param name="pcsInfo">输入的棋子信息</param>
        /// <param name="pcsPos">输入的棋子信息的坐标</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="pcsTypeScore">棋子的评分标准，字典格式</param>
        /// <param name="pcsType">返回棋型</param>
        /// <param name="pcsScore">返回棋型得分</param>
        /// <param name="pcsScorePos">返回棋型坐标</param>
        public void GetPcsTypeScorePos(Point pt, int flg, Dictionary<string,int> pcsTypeScore, 
            out List<string> pcsType, out List<int> pcsScore, out List<List<Point>> pcsScorePos)
        {           
            char tmpChar = flg == 1 ? '1' : '2';
            /// 存储4个方向的棋子信息
            List<string> lstPcsInfo;// = new List<string>();
            List<string> tmpStr = new List<string>();
            /// 存储4个方向的坐标信息           
            List<List<Point>> lstPosInfo;// = new List<List<Point>>();
             //得到四个方向一定范围内的棋子信息                                        
            GetPointRoundInfo(chessPadInfo, pt, 4, flg, out lstPcsInfo, out lstPosInfo);
            ////如果输入的点还未在绘制，则在统计信息时添加
            //if (chessPadInfo[pt.Y][pt.X] == 0)
            //{
            //    for (int j = 0; j < lstPosInfo.Count; j++)
            //    {
            //        StringBuilder strBld = new StringBuilder(lstPcsInfo[j]);
            //        for (int i = 0; i < lstPosInfo[j].Count; i++)
            //        {
            //            if(lstPosInfo[j][i]==pt)
            //                strBld.Replace('0', tmpChar, i, 1);
            //        }
            //        tmpStr.Add(strBld.ToString());
            //    }
            //    lstPcsInfo = tmpStr;
            //}

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
            pcsType = pType;
            pcsScore = pScore;
            pcsScorePos = pScorePos;
        }

        /// <summary>
        /// 从棋型中找一个能够获得最大评分的坐标点
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

    }
}
