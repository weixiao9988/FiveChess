using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveChess
{
    class ChessPad
    {
        /// <summary>
        /// 棋盘线总数
        /// </summary>
        public int lineCount { get; set; }
        
        /// <summary>
        /// 棋盘宽度
        /// </summary>
        
        public int padWid { get; set; }
        /// <summary>
        /// 棋盘线间距
        /// </summary>
        public float lineSpace { get; set; }

        /// <summary>
        /// 棋子大小
        /// </summary>
        public int pcsSize { get { return 16; } set { value = 16; } }
        /// <summary>
        /// 棋盘交叉点坐标数组
        /// </summary>
        public List<List<PointF>> cursorArry;
        /// <summary>
        /// 棋盘交叉点是否为棋子，1 为棋子，0为空
        /// </summary>
        public int[,] isPiecesArry;
        
        /// <summary>
        /// 棋盘构造函数
        /// </summary>
        /// <param name="wid">棋盘宽度</param>
        /// <param name="count">棋盘线总数</param>
        /// <param name="space">棋盘线间隔</param>
        public ChessPad(int count,float space)
        { 
            lineCount = count;
            cursorArry = new List<List<PointF>>();
            isPiecesArry = new int[count, count];
            padWid = (int)((count-1)*space);
            lineSpace = space;

            InitPadInfo();
        }

       public void InitPadInfo()
        {            
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
