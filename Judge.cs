using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveChess
{
    class Judge
    {
        private List<List<int>> pcsInfo;
        private int lineMax;
        public Judge(List<List<int>> arry)
        {
            pcsInfo = arry;
            lineMax = arry[0].Count;
        }
        public int[] GetResult(Point pt, int flag)
        {
            int[] result = new int[2];

            //检查横 竖 撇 捺 四种形式的返回值
            if (GetHResult(pt, flag)==5)
            {
                result[0] = 5;
                result[1] = 1;
                return result;
            }
            else if (GetVResult(pt, flag) == 5)
            {
                result[0] = 5;
                result[1] = 2;
                return result;
            }
            else if(GetPResult(pt, flag) == 5)
            {
                result[0] = 5;
                result[1] = 3;
                return result;
            }
            else if (GetLResult(pt, flag) == 5)
            {
                result[0] = 5;
                result[1] = 4;
                return result;
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
