using System;
using System.Collections.Generic;
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
        private List<List<int>> pcsInfo;
        private int lineMax;
        private List<string> listPcsInfo = new List<string>();
        private List<List<Point>> listPosInfo = new List<List<Point>>();

        public delegate void UpStatInfo(Point pt, string s);
        public event UpStatInfo UpInfoEvt;

        #region //各种棋型的评分值
        /// <summary>
        /// 各种棋型的评分值
        /// </summary>
        public Dictionary<string, int> TypeScore = new Dictionary<string, int>()
        {
            ["AAAAA"] = 100,  /*连5型*/
            ["0AAAA0"] = 90,  /*活4型*/
            ["0AAAAB"] = 70,  /*冲4型*/
            ["BAAAA0"] = 70,  /*冲4型*/
            ["0AAAC"] = 70,  /*冲4型*/
            ["CAAA0"] = 70,  /*冲4型*/
            ["A0AAA"] = 70, /*冲4型*/
            ["AA0AA"] = 70, /*冲4型*/
            ["AAA0A"] = 70, /*冲4型*/
            ["C0AAA"] = 70, /*冲4型*/
            ["CA0AA"] = 70, /*冲4型*/
            ["CAA0A"] = 70, /*冲4型*/
            ["A0AAC"] = 70, /*冲4型*/
            ["AA0AC"] = 70, /*冲4型*/
            ["AAA0C"] = 70, /*冲4型*/
            ["00AAA0"] = 65, /*活3型*/
            ["0A0AA0"] = 60, /*活3型*/
            ["0AA0A0"] = 60, /*活3型*/
            ["0AAA00"] = 65, /*活3型*/
            ["00AAAB"] = 40,  /*眠3型*/
            ["0A0AAB"] = 40,  /*眠3型*/
            ["0AA0AB"] = 40,  /*眠3型*/
            ["0AAA0B"] = 45,  /*眠3型*/
            ["BAAA00"] = 45,  /*眠3型*/
            ["BAA0A0"] = 40,  /*眠3型*/
            ["BA0AA0"] = 40,  /*眠3型*/
            ["B0AAA0"] = 45,  /*眠3型*/
            ["00AAC"] = 40,   /*眠3型*/
            ["0A0AC"] = 40,   /*眠3型*/
            ["0AA0C"] = 40,   /*眠3型*/
            ["CAA00"] = 40,   /*眠3型*/
            ["CA0A0"] = 40,   /*眠3型*/
            ["C0AA0"] = 40,   /*眠3型*/
            ["00AAA"] = 42,    /*眠3型*/
            ["0A0AA"] = 40,    /*眠3型*/
            ["0AA0A"] = 40,    /*眠3型*/
            ["0AAA0"] = 42,    /*眠3型*/
            ["A00AA"] = 40,    /*眠3型*/
            ["A0A0A"] = 40,    /*眠3型*/
            ["A0AA0"] = 40,    /*眠3型*/
            ["AA00A"] = 40,    /*眠3型*/
            ["AA0A0"] = 40,    /*眠3型*/
            ["AAA00"] = 42,    /*眠3型*/
            ["C00AA"] = 40,    /*眠3型*/
            ["C0A0A"] = 40,    /*眠3型*/            
            ["CA00A"] = 40,    /*眠3型*/
            ["AA00A"] = 40,    /*眠3型*/
            ["A0A0C"] = 40,    /*眠3型*/
            ["A00AC"] = 40,    /*眠3型*/
            ["000AA0"] = 35, /*活2型*/
            ["00A0A0"] = 30, /*活2型*/
            ["00AA00"] = 35, /*活2型*/
            ["0A00A0"] = 30, /*活2型*/
            ["0A0A00"] = 30, /*活2型*/
            ["0AA000"] = 35, /*活2型*/
            ["000AAB"] = 15,  /*眠2型*/
            ["00A0AB"] = 10,  /*眠2型*/
            ["00AA0B"] = 15,  /*眠2型*/
            ["0A00AB"] = 10,  /*眠2型*/
            ["0A0A0B"] = 10,  /*眠2型*/
            ["0AA00B"] = 15,  /*眠2型*/
            ["B0AA00"] = 15,  /*眠2型*/
            ["BA0A00"] = 10,  /*眠2型*/
            ["BAA000"] = 15,  /*眠2型*/
            ["B0A0A0"] = 10,  /*眠2型*/
            ["BA00A0"] = 10,  /*眠2型*/
            ["B00AA0"] = 15,  /*眠2型*/
            ["000AC"] = 10,   /*眠2型*/
            ["00A0C"] = 10,   /*眠2型*/
            ["0A00C"] = 10,   /*眠2型*/
            ["C0C00"] = 10,   /*眠2型*/
            ["CA000"] = 10,   /*眠2型*/
            ["C00A0"] = 10,   /*眠2型*/
            ["000AA"] = 15,    /*眠2型*/
            ["00A0A"] = 10,    /*眠2型*/
            ["00AA0"] = 15,    /*眠2型*/
            ["0A00A"] = 10,    /*眠2型*/
            ["0A0A0"] = 10,    /*眠2型*/
            ["0AA00"] = 15,    /*眠2型*/
            ["A000A"] = 10,    /*眠2型*/
            ["A00A0"] = 10,    /*眠2型*/
            ["A0A00"] = 10,    /*眠2型*/
            ["AA000"] = 15,    /*眠2型*/
            ["C000A"] = 10,    /*眠2型*/
            ["A000C"] = 10    /*眠2型*/
        };
        #endregion

        public Dictionary<string, int> TypeScore1 = new Dictionary<string, int>()
        {
            ["11111"] = 10000,  /*连5型*/
            ["011110"] = 1000,  /*活4型*/
            ["011112"] = 1000,  /*冲4型*/
            ["211110"] = 1000,  /*冲4型*/
            ["01115"] = 1000,  /*冲4型*/
            ["51110"] = 1000,  /*冲4型*/
            ["10111"] = 1000, /*冲4型*/
            ["11011"] = 1000, /*冲4型*/
            ["11101"] = 1000, /*冲4型*/
            ["50111"] = 1000, /*冲4型*/
            ["51011"] = 1000, /*冲4型*/
            ["51101"] = 1000, /*冲4型*/
            ["10115"] = 1000, /*冲4型*/
            ["11015"] = 1000, /*冲4型*/
            ["11105"] = 1000, /*冲4型*/
            ["001110"] = 1000, /*活3型*/
            ["010110"] = 1000, /*活3型*/
            ["011010"] = 1000, /*活3型*/
            ["011100"] = 1000, /*活3型*/
            ["001112"] = 1000,  /*眠3型*/
            ["010112"] = 1000,  /*眠3型*/
            ["011012"] = 1000,  /*眠3型*/
            ["011102"] = 1000,  /*眠3型*/
            ["211100"] = 1000,  /*眠3型*/
            ["211010"] = 1000,  /*眠3型*/
            ["210110"] = 1000,  /*眠3型*/
            ["201110"] = 1000,  /*眠3型*/
            ["00115"] = 1000,   /*眠3型*/
            ["01015"] = 1000,   /*眠3型*/
            ["01105"] = 1000,   /*眠3型*/
            ["51100"] = 1000,   /*眠3型*/
            ["51010"] = 1000,   /*眠3型*/
            ["50110"] = 1000,   /*眠3型*/
            ["00111"] = 1000,    /*眠3型*/
            ["01011"] = 1000,    /*眠3型*/
            ["01101"] = 1000,    /*眠3型*/
            ["01110"] = 1000,    /*眠3型*/
            ["10011"] = 1000,    /*眠3型*/
            ["10101"] = 1000,    /*眠3型*/
            ["10110"] = 1000,    /*眠3型*/
            ["11001"] = 1000,    /*眠3型*/
            ["11010"] = 1000,    /*眠3型*/
            ["11100"] = 1000,    /*眠3型*/
            ["50011"] = 1000,    /*眠3型*/
            ["50101"] = 1000,    /*眠3型*/
            ["51001"] = 1000,    /*眠3型*/
            ["11005"] = 1000,    /*眠3型*/
            ["10105"] = 1000,    /*眠3型*/
            ["10015"] = 1000,    /*眠3型*/
            ["000110"] = 1000, /*活2型*/
            ["001010"] = 1000, /*活2型*/
            ["001100"] = 1000, /*活2型*/
            ["010010"] = 1000, /*活2型*/
            ["010100"] = 1000, /*活2型*/
            ["011000"] = 1000, /*活2型*/
            ["000112"] = 1000,  /*眠2型*/
            ["001012"] = 1000,  /*眠2型*/
            ["001102"] = 1000,  /*眠2型*/
            ["010012"] = 1000,  /*眠2型*/
            ["010102"] = 1000,  /*眠2型*/
            ["011002"] = 1000,  /*眠2型*/
            ["201100"] = 1000,  /*眠2型*/
            ["210100"] = 1000,  /*眠2型*/
            ["211000"] = 1000,  /*眠2型*/
            ["201010"] = 1000,  /*眠2型*/
            ["210010"] = 1000,  /*眠2型*/
            ["200110"] = 1000,  /*眠2型*/
            ["00015"] = 1000,   /*眠2型*/
            ["00105"] = 1000,   /*眠2型*/
            ["01005"] = 1000,   /*眠2型*/
            ["50100"] = 1000,   /*眠2型*/
            ["51000"] = 1000,   /*眠2型*/
            ["50010"] = 1000,   /*眠2型*/
            ["00011"] = 1000,    /*眠2型*/
            ["00101"] = 1000,    /*眠2型*/
            ["00110"] = 1000,    /*眠2型*/
            ["01001"] = 1000,    /*眠2型*/
            ["01010"] = 1000,    /*眠2型*/
            ["01100"] = 1000,    /*眠2型*/
            ["10001"] = 1000,    /*眠2型*/
            ["10010"] = 1000,    /*眠2型*/
            ["10100"] = 1000,    /*眠2型*/
            ["11000"] = 1000,    /*眠2型*/
            ["50001"] = 1000,    /*眠2型*/
            ["10005"] = 1000    /*眠2型*/
        };

        public Judge(List<List<int>> arry)
        {
            pcsInfo = arry;
            lineMax = arry[0].Count;
        }

        public int[] GetResult(Point pt, int flag)
        {
            int[] result = new int[2];

            GetPcsStat(pcsInfo, pt, 4, lineMax, listPcsInfo, listPosInfo);
            List<int> lstScore = new List<int>();
            List<Point> lstPts = new List<Point>();
            SortedDictionary<int, Point> sortDict = new SortedDictionary<int, Point>();

            for (int i = 0; i < 4 ; i++)
            {
                string str = null, str1 = null, tmpS = null;
                List<int> score = new List<int>();
                
                foreach (var item in TypeScore.Keys)
                {
                    str = GetNewStr(item, flag);                    
                    int n = listPcsInfo[i].IndexOf(str);
                    if (n != -1)
                    {
                        if (str == "11111" || str == "22222")
                        {
                            result[0] = 5;
                            result[1] = flag;
                            return result;
                        }
                        else
                        {
                            score.Add(TypeScore[item]);
                            GetPcsScorePos(str, i, flag, lstScore, lstPts);
                        }                        
                    }
                }
            }

            Point pp=new Point();
            if (lstPts.Count > 0 && lstScore.Count > 0)
            {
                pp = lstPts[lstScore.IndexOf(lstScore.Max())];
                UpInfoEvt(pp, lstScore.Max().ToString());
            }
            return result;
        }

        public void GetPcsScorePos(string scStr, int m, int flag, List<int> lstScore,List<Point> lstPts)
        {
            char[] ch;
            string str1;
            int n = listPcsInfo[m].IndexOf(scStr);

            for (int k = 0; k < scStr.Length; k++)
            {
                ch = scStr.ToCharArray();
                int iLeft = k - 1 >= 0 ? k - 1 : 0;
                int iRight = k + 1 < scStr.Length ? k + 1 : scStr.Length - 1;
                char mch= flag == 1 ? '1' : '2';

                if (ch[k] == '0' && (ch[iLeft] == mch|| ch[iRight] == mch))
                {
                    ch[k] = flag == 1 ? '1' : '2';
                    str1 = new string(ch);

                    str1 = RestOldStr(str1, flag);
                    if (TypeScore.ContainsKey(str1))
                    {
                        if (lstScore.Count == 0)
                        {
                            lstScore.Add(TypeScore[str1]);
                            lstPts.Add(listPosInfo[m][n + k]);
                        }
                        else
                        {
                            if (TypeScore[str1] > lstScore.Max())
                            {
                                lstScore.Add(TypeScore[str1]);
                                lstPts.Add(listPosInfo[m][n + k]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回对应标志的棋型字符串
        /// </summary>
        /// <param name="str">通用的棋型字符串</param>
        /// <param name="m">棋子标志</param>
        /// <returns>对应棋子的棋型字符串</returns>
        public string GetNewStr(string str, int m)
        {
            string s = null;
            switch (m)
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
        /// <param name="m">棋子标志</param>
        /// <returns></returns>
        public string RestOldStr(string str, int m)
        {
            string s = null;
            switch (m)
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
        /// 获取落子后在四个方向一定范围内各自形成的棋子形势
        /// </summary>
        /// <param name="padInfo">落子后的棋盘信息</param>
        /// <param name="pt">落子的位置</param>
        /// <param name="incr">距离落子的范围</param>
        /// <param name="lineCount">棋盘线总数</param>
        /// <param name="lstPcs">返回每个点的棋子标志:(0空，1黑子，2白子)</param>
        /// <param name="lstPts">返回每个点的坐标</param>
        public void GetPcsStat(List<List<int>> padInfo, Point pt, int incr, int lineCount,List<string> lstPcs,List<List<Point>> lstPts)
        {           
            for (int t = 0; t < 4; t++)
            {
                string str = null;
                List<Point> pts = new List<Point>();

                for (int i = -incr; i <= incr; i++)
                {
                    switch (t)
                    {
                        case 0: /// 根据输入点和范围返回水平方向结果
                            if (bInPad(pt.X + i, pt.Y + i, lineCount))
                            {
                                str += padInfo[pt.Y][pt.X + i].ToString();
                                pts.Add(new Point(pt.X + i, pt.Y));
                            }
                            break;
                        case 1:     // 根据输入点和范围返回垂直方向结果
                            if (bInPad(pt.X, pt.Y + i, lineCount))
                            {
                                str += padInfo[pt.Y + i][pt.X].ToString();
                                pts.Add(new Point(pt.X, pt.Y + i));
                            }                                
                            break;
                        case 2:     // 根据输入点和范围返回撇[/]方向结果
                            if (bInPad(pt.X - i, pt.Y + i, lineCount))
                            {
                                str += padInfo[pt.Y + i][pt.X - i].ToString();
                                pts.Add(new Point(pt.X - i, pt.Y + i));
                            }                                
                            break;
                        case 3:     // 根据输入点和范围返回捺[\]方向结果
                            if (bInPad(pt.X + i, pt.Y + i, lineCount))
                            {
                                str += padInfo[pt.Y + i][pt.X + i].ToString();
                                pts.Add(new Point(pt.X + i, pt.Y + i));
                            }
                            break;
                        default:
                            break;
                    }
                }
                lstPcs.Add(str);
                lstPts.Add(pts);                
            }
            
        }

        /// <summary>
        /// 判断输入点是否在棋盘内
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="max">棋盘线最大值</param>
        /// <returns>在棋盘内返回true，否则返回false</returns>
        public bool bInPad(int x,int y,int max)
        {
            if (x >= 0 && x < max && y >= 0 && y < max)
                return true;
            return false;
        }

        /// <summary>
        /// 落子后判断输赢
        /// </summary>
        /// <param name="pt">输入点</param>
        /// <param name="flg">棋子标志</param>
        /// <param name="lstPad">棋盘状态</param>
        /// <param name="lineCount">棋盘线最大值</param>
        /// <returns>返回int[]，[0]棋子标志，[1]</returns>
        public int[] IsWin(Point pt, int flg,List<List<int>> lstPad, int lineCount)
        {            
            int[] result = { flg, 0 };
            int[] xArr = GetMinMax(pt.X, lineCount);
            int[] yArr = GetMinMax(pt.Y, lineCount);
            int vMin, vMax, k;
            string part = @flg.ToString() + "+";
            
            List<int> count = new List<int>();

            for (int t = 1; t <= 4; t++)
            {
                string str = null;
                switch (t)
                {
                    case 1:  /// 根据输入点和范围返回水平方向结果
                        {
                            for (int i = xArr[0]; i <= xArr[1]; i++)
                                str += lstPad[pt.Y][i].ToString();

                            MatchCollection match = Regex.Matches(str, part);
                            foreach (Match item in match)
                                count.Add(item.Length);

                            if ((result[1] = count.Max()) == 5)
                                return result;
                            break;
                        }
                    case 2:     // 根据输入点和范围返回垂直方向结果
                        {
                            for (int i = yArr[0]; i <= yArr[1]; i++)
                                str += lstPad[i][pt.X].ToString();

                            MatchCollection match = Regex.Matches(str, part);
                            foreach (Match item in match)
                                count.Add(item.Length);

                            if ((result[1] = count.Max()) == 5)
                                return result;
                            break;
                        }                        
                    case 3:     // 根据输入点和范围返回撇[/]方向结果
                        {
                            vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X;
                            vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                                str += lstPad[pt.Y + i][pt.X - i].ToString();
                            MatchCollection match = Regex.Matches(str, part);
                            foreach (Match item in match)
                                count.Add(item.Length);

                            if ((result[1] = count.Max()) == 5)
                                return result;
                            break;
                        }
                    case 4:    // 根据输入点和范围返回捺[\]方向结果
                        {
                            vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
                            vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;
                            for (int i = -vMin; i <= vMax; i++)
                                str += lstPad[pt.Y + i][pt.X + i].ToString();
                            MatchCollection match = Regex.Matches(str, part);
                            foreach (Match item in match)
                                count.Add(item.Length);

                            if ((result[1] = count.Max()) == 5)
                                return result;
                            break;
                        }
                    default:
                        break;
                }               
            }
            return result;
        }

        /// <summary>
        /// 根据输入点和查找范围返回水平方向结果
        /// </summary>
        /// <param name="arr">查找范围，[0]：最小，[1]：最大</param>
        /// <param name="pt">输入点</param>
        /// <param name="flag">棋子标志，1:己方，2:他方</param>
        /// <returns>结果数组，[0]：棋子数量，[1]：棋子标志</returns>
        public int GetHResult(Point pt, int flag)
        {
            int[] arry = GetMinMax(pt.X, lineMax);

            int result = 0;
            for (int i = arry[0]; i <= arry[1]; i++)
            {
                result = pcsInfo[pt.Y][i] == flag ? ++result : 0;

                if (result == 5)
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据输入点和查找范围返回垂直方向结果
        /// </summary>
        /// <param name="arr">查找范围，[0]：最小，[1]：最大</param>
        /// <param name="pt">输入点</param>
        /// <param name="flag">棋子标志，1:己方，2:他方</param>
        /// <returns>返回结果</returns>
        public int GetVResult(Point pt, int flag)
        {
            int[] arry = GetMinMax(pt.Y, lineMax);
            int result = 0;
            for (int i = arry[0]; i <= arry[1]; i++)
            {
                result = pcsInfo[i][pt.X] == flag ? ++result : 0;

                if (result == 5)
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据输入点和查找范围返回撇(/)方向结果
        /// </summary>
        /// <param name="arr">查找范围，[0]：最小，[1]：最大</param>
        /// <param name="pt">输入点</param>
        /// <param name="flag">棋子标志，1:己方，2:他方</param>
        /// <returns>返回结果</returns>
        public int GetPResult(Point pt, int flag)
        {
            int[] xArr = GetMinMax(pt.X, lineMax);
            int[] yArr = GetMinMax(pt.Y, lineMax);

            int vMin = pt.Y - yArr[0] < xArr[1] - pt.X ? pt.Y - yArr[0] : xArr[1] - pt.X ;
            int vMax = pt.X - xArr[0] < yArr[1] - pt.Y ? pt.X - xArr[0] : yArr[1] - pt.Y;


            int result = 0;
            for (int i = -vMin; i <= vMax; i++)
            {
                result = pcsInfo[pt.Y+i][pt.X-i] == flag ? ++result : 0;

                if (result == 5)
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据输入点和查找范围返回拉(\)方向结果
        /// </summary>
        /// <param name="arr">查找范围，[0]：最小，[1]：最大</param>
        /// <param name="pt">输入点</param>
        /// <param name="flag">棋子标志，1:己方，2:他方</param>
        /// <returns>返回结果</returns>
        public int GetLResult(Point pt, int flag)
        {
            int[] xArr = GetMinMax(pt.X, lineMax);
            int[] yArr = GetMinMax(pt.Y, lineMax);

            int vMin = pt.Y - yArr[0] < pt.X - xArr[0] ? pt.Y - yArr[0] : pt.X - xArr[0];
            int vMax = xArr[1] - pt.X < yArr[1] - pt.Y ? xArr[1] - pt.X : yArr[1] - pt.Y;


            int result = 0;
            for (int i = -vMin; i <= vMax; i++)
            {
                result = pcsInfo[pt.Y + i][pt.X + i] == flag ? ++result : 0;

                if (result == 5)
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据输入的值计算可能的最小、最大编号;
        /// </summary>
        /// <param name="val">输入值</param>
        /// <param name="count">棋盘最大格子数量</param>
        /// <returns>最小、最大编号数组，[0]：最小，[1]：最大</returns>
        public int[] GetMinMax(int val, int count)
        {
            int[] arr = new int[2];
            //根据输入点计算可能的最小、最大编号;
            if (val > 4 && val < count - 5)
            {
                arr[0]= val - 4;
                arr[1] = val + 4;
            }
            else
            {
                arr[0] = val <= 4 ? 0 : val - 4;
                arr[1] = val <= 4 ? val + 4 : count - 1;
            }
            return arr;
        }

    }
}
