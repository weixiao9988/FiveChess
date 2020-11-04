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
            int[] arry = GetMinMax(pt.X, lineMax);
            int[] result = GetHResult(arry, pt, flag);

            return result;
        }

        /// <summary>
        /// 根据输入点和查找范围返回水平方向结果
        /// </summary>
        /// <param name="arr">查找范围，[0]：最小，[1]：最大</param>
        /// <param name="pt">输入点</param>
        /// <param name="flag">棋子标志，1:己方，2:他方</param>
        /// <returns>结果数组，[0]：棋子数量，[1]：棋子标志</returns>
        public int[] GetHResult(int[] arr, Point pt, int flag)
        {
            int[] result = new int[2];
            for (int i = arr[0]; i <= arr[1]; i++)
            {
                result[0] = pcsInfo[pt.Y][i] == flag ? ++result[0] : 0;

                if (result[0] == 5)
                {
                    result[1] = flag;
                    break;
                }
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
