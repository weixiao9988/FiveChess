using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FiveChess
{
    class Method
    {
        /// <summary>
        /// 落子后在四个方向一定范围内获取棋盘中的棋子信息和坐标
        /// </summary>
        /// <param name="lstPad">棋盘信息</param>
        /// <param name="pt">输入点</param>
        /// <param name="incr">距离落子的范围</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="isAdd">是否增加棋子</param>
        /// <param name="pcsInfo">输出棋子信息</param>
        /// <param name="posInfo">输出坐标信息</param>
        public void GetPointRoundInfo(List<List<int>> lstPad, Point pt, int incr, int flg, bool isAdd, out List<string> pcsInfo, out List<List<Point>> posInfo)
        {
            int[] xArr = GetMinMax(pt.X, lstPad.Count, incr);
            int[] yArr = GetMinMax(pt.Y, lstPad.Count, incr);
            int vMin, vMax;
            List<string> pcsLSt = new List<string>();
            List<List<Point>> posLst = new List<List<Point>>();

            for (int t = 0; t < 4; t++)
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
                                str = isAdd && i == pt.X && lstPad[i][pt.Y] == 0 ? str + flg.ToString() : str + lstPad[i][pt.Y].ToString();
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
                                str = isAdd && i == pt.Y && lstPad[pt.X][i] == 0 ? str + flg.ToString() : str + lstPad[pt.X][i].ToString();
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
                                str = isAdd && i == 0 && lstPad[pt.X - i][pt.Y + i] == 0 ? str + flg.ToString() : str + lstPad[pt.X - i][pt.Y + i].ToString();
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
                                str = isAdd && i == 0 && lstPad[pt.X + i][pt.Y + i] == 0 ? str + flg.ToString() : str + lstPad[pt.X + i][pt.Y + i].ToString();
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
            for (int i = 0; i < pcsInfo.Count; i++)
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
        /// 得到相连棋子旁边的一个点
        /// </summary>
        /// <param name="chessPad">棋盘中棋子的信息</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="lstPts">相连棋子的坐标</param>
        /// <param name="direct">相连棋子的方向[- | / \]</param>
        /// <returns></returns>
        public Point GetPcsPos(List<List<int>> chessPad, List<Point> lstPts)
        {
            int direct = JudgePtsDirect(lstPts);
            Point backPt = new Point(-1, -1);
            ///x,y坐标值在横【-】竖【|】撇【/】捺【\】四个方向的变化规律;
            
            int[] x0= new int[] { 1, 0, -1, 1 };
            int[] y0 = new int[] { 0, 1, 1, 1 };

            Point fistPt = new Point(lstPts[0].X - x0[direct], lstPts[0].Y - y0[direct]);
            Point endPt = new Point(lstPts[lstPts.Count - 1].X + x0[direct], lstPts[lstPts.Count - 1].Y + y0[direct]);

            if (bInPad(fistPt, chessPad.Count) && bInPad(endPt, chessPad.Count))
            {
                if (chessPad[fistPt.X][fistPt.Y] == 0)
                    backPt = fistPt;
                else if (chessPad[endPt.X][endPt.Y] == 0)
                    backPt = endPt;
               
            }
                        
            return backPt;
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
        public bool GetPcsTypeScorePos(Point pt, int flg, Dictionary<string, int> pcsTypeScore,
            out string pcsType, out int pcsScore, out List<Point> pcsScorePos)
        {
            char tmpChar = flg == 1 ? '1' : '2';
            /// 存储4个方向的棋子信息
            List<string> lstPcsInfo;// = new List<string>();
            List<string> tmpStr = new List<string>();
            /// 存储4个方向的坐标信息           
            List<List<Point>> lstPosInfo;// = new List<List<Point>>();
                                         //得到四个方向一定范围内的棋子信息                                        
            GetPointRoundInfo(Chess.pcsFlag, pt, 4, flg, flg==2, out lstPcsInfo, out lstPosInfo);

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
                    if (match.Count > 0)
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
        public Dictionary<int, Point> GetScoreAndPos(string type, List<Point> pts, Dictionary<string, int> typeScore, int flag)
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
        /// 输入一组坐标点，判断点的分布范围
        /// </summary>
        /// <param name="pts">坐标点数组</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="isCreat">是否向外扩展</param>
        /// <param name="add">向外扩展数</param>
        /// <returns>返回xy方向的最小和最大值</returns>
        public Dictionary<string, int> GetPosMinMax(List<Point> pts,int lineCount,bool isCreat,int add)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            int xMin = pts[0].X, xMax = pts[0].X, yMin = pts[0].Y, yMax = pts[0].Y;
            foreach (Point pt in pts)
            {
                xMin = pt.X < xMin ? pt.X : xMin;
                xMax = pt.X > xMax ? pt.X : xMax;
                yMin = pt.Y < yMin ? pt.Y : yMin;
                yMax = pt.Y > yMax ? pt.Y : yMax;
            }

            if(isCreat)
            {
                xMin = xMin - add > 0 ? xMin - add : xMin;
                xMax = xMax + add < lineCount ? xMax + add : xMax;
                yMin = yMin - add > 0 ? yMin - add : yMin;
                yMax = yMax + add < lineCount ? yMax + add : yMax;
            }

            dict.Add("xMin", xMin);
            dict.Add("xMax", xMax);
            dict.Add("yMin", yMin);
            dict.Add("yMax", yMax);

            return dict;
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
        /// 根据输入的范围，在其每个空位分别计算黑棋和白棋的评分，并获得最大评分的点
        /// </summary>
        /// <param name="pcsFlag">棋盘中的棋子信息</param>
        /// <param name="range">范围</param>
        /// <param name="typeSocre">棋型评分标准</param>
        /// <returns></returns>
        public Point GetMaxScorePos(List<List<int>> pcsFlag, Dictionary<string, int> range, Dictionary<string,int> typeSocre)
        {
            Point pt = new Point();
            List<string> pcsStr;
            int bValue, wValue, result = 0, tmpV;            

            for (int j = range["yMin"]; j <= range["yMax"]; j++)
            {
                for (int i = range["xMin"]; i <= range["xMax"]; i++)
                {
                    if (pcsFlag[i][j] == 0)
                    {
                        pcsStr=GetPointRoundInfo(pcsFlag, i, j, 4);
                        bValue = GetMaxScoreValue(pcsStr, typeSocre, 1);
                        wValue = GetMaxScoreValue(pcsStr, typeSocre, 2);
                        tmpV = bValue > wValue ? bValue : wValue;
                        if (tmpV > result)
                        {
                            result = tmpV;
                            pt.X = i;
                            pt.Y = j;

                        }
                    }
                }
            }

            return pt;
        }

        /// <summary>
        /// 计算两组坐标点的最大范围，并向外扩展一格
        /// </summary>
        /// <param name="pts1">坐标集合1</param>
        /// <param name="pts2">坐标集合2</param>
        /// <param name="lineCount">棋盘线总数</param>       
        /// <returns></returns>
        public Dictionary<string,int> GetRange(List<Point> pts1,List<Point> pts2,int lineCount)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            Dictionary<string, int> bRange = GetPosMinMax(pts1, lineCount, false, 0);
            Dictionary<string, int> wRange = GetPosMinMax(pts2, lineCount, false, 0);

            List<Point> pts = new List<Point>();
            pts.Add(new Point(bRange["xMin"], bRange["yMin"]));
            pts.Add(new Point(bRange["xMax"], bRange["yMax"]));
            pts.Add(new Point(wRange["xMin"], wRange["yMin"]));
            pts.Add(new Point(wRange["xMax"], wRange["yMax"]));

            dict= GetPosMinMax(pts, lineCount, true, 1);

            return dict;
        } 


        /// <summary>
        /// 在输入点四个方向一定范围内获取棋盘中的棋子信息
        /// </summary>
        /// <param name="lstPad">棋盘信息</param>
        /// <param name="pt">输入点</param>
        /// <param name="incr">距离落子的范围</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="isAdd">是否增加棋子</param>
        /// <param name="pcsInfo">输出棋子信息</param>        
        public List<string> GetPointRoundInfo(List<List<int>> lstPad, int x, int y, int incr)
        {
            int[] xArr = GetMinMax(x, lstPad.Count, incr);
            int[] yArr = GetMinMax(y, lstPad.Count, incr);
            int vMin, vMax;
            List<string> pcsLSt = new List<string>();
            for (int t = 1; t <= 4; t++)
            {
                string str = null;
                switch (t)
                {
                    case 1:  /// 根据输入点和范围返回水平方向结果
                        {
                            vMin = xArr[0];
                            vMax = xArr[1];
                            for (int i = vMin; i <= vMax; i++)
                                str = str + lstPad[i][y].ToString();
                            pcsLSt.Add(str);
                            break;
                        }
                    case 2:     // 根据输入点和范围返回垂直方向结果
                        {
                            vMin = yArr[0];
                            vMax = yArr[1];
                            for (int i = vMin; i <= vMax; i++)
                                str = str + lstPad[x][i].ToString();
                            pcsLSt.Add(str);
                            break;
                        }
                    case 3:     // 根据输入点和范围返回撇[/]方向结果
                        {
                            vMin = y - yArr[0] < xArr[1] - x ? y - yArr[0] : xArr[1] - x;
                            vMax = x - xArr[0] < yArr[1] - y ? x - xArr[0] : yArr[1] - y;
                            for (int i = -vMin; i <= vMax; i++)
                                str = str + lstPad[x - i][y + i].ToString();
                            pcsLSt.Add(str);
                            break;
                        }
                    case 4:    // 根据输入点和范围返回捺[\]方向结果
                        {
                            vMin = y - yArr[0] < x - xArr[0] ? y - yArr[0] : x- xArr[0];
                            vMax = xArr[1] - x < yArr[1] - y ? xArr[1] - x : yArr[1] - y;
                            for (int i = -vMin; i <= vMax; i++)
                                str = str + lstPad[x + i][y + i].ToString();
                            pcsLSt.Add(str);
                            break;
                        }
                    default:
                        break;
                }
            }
            return pcsLSt;
        }

        /// <summary>
        /// 计算一组棋型得分的总和
        /// </summary>
        /// <param name="pcsInfo">棋型集合</param>
        /// <param name="pcsScore">棋型的评分标准</param>
        /// <param name="flag">棋子标志</param>
        /// <returns></returns>
        public int GetMaxScoreValue(List<string> pcsInfo,Dictionary<string,int> pcsScore, int flag)
        {            
            int score=0;
            //从棋子信息中找出符合的棋型和评分总和
            foreach (string item in pcsInfo)
            {
                MatchCollection match;
                string tmpS = null;
                int tmpV = 0;
                foreach (string type in pcsScore.Keys)
                {                    
                    tmpS = GetNewStr(type, flag);
                    match = Regex.Matches(item, tmpS);
                    if (match.Count>0)
                    {
                        foreach (Match mt in match)
                        {
                            int m = pcsScore[RestOldStr(mt.ToString(), flag)];
                            tmpV = m > tmpV ? m : tmpV;
                        }
                    }
                }

                score += tmpV;
            }
            return score;
        }

       

    }
}
