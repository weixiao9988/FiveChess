using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveChess
{
    static class ChessPad
    {
        /// <summary>
        /// 棋盘线总数
        /// </summary>
        public static int lineCount { get; set; }
        
        /// <summary>
        /// 棋盘宽度
        /// </summary>
        
        public static int padWid { get; set; }
        /// <summary>
        /// 棋盘线间距
        /// </summary>
        public static float lineSpace { get; set; }

        /// <summary>
        /// 棋子大小
        /// </summary>
        public static int pcsSize { get { return 16; } set { value = 16; } }
        /// <summary>
        /// 棋盘交叉点坐标数组
        /// </summary>
        public static List<List<PointF>> cursorArry;

        /// <summary>
        /// 棋子在棋盘中的行列编号，x：列编号，y：行编号
        /// </summary>
        public static Point XYSeir = new Point();
        
        /// <summary>
        /// 棋盘中棋子的颜色标志，0：无颜色，1：己方棋子颜色，2：电脑棋子颜色
        /// </summary>
        public static int[,] pcsClsFlg;
        /// <summary>
        /// 棋盘构造函数
        /// </summary>
        /// <param name="wid">棋盘宽度</param>
        /// <param name="count">棋盘线总数</param>
        /// <param name="space">棋盘线间隔</param>


        /// <summary>
        /// 初始化棋盘信息
        /// <param name="count">棋盘线总数</param>
        /// <param name="space">棋盘线间隔</param>
        /// </summary>
        public static void InitPadInfo(int count, float space)
        {
            lineCount = count;
            cursorArry = new List<List<PointF>>();            
            pcsClsFlg = new int[count, count];
            padWid = (int)((count - 1) * space);
            lineSpace = space;

            for (int h = 0; h < lineCount; h++)
            {
                List<PointF> listPtf = new List<PointF>();
                PointF ptF = new PointF();
                for (int i = 0; i < lineCount; i++)
                {
                    ptF.X = i * lineSpace;
                    ptF.Y = h * lineSpace;
                    listPtf.Add(ptF);
                }
                cursorArry.Add(listPtf);
            }
        }

        
    }
}
