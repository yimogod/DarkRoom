using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.PCG
{
    public class CProcedureGridBase<T>
    {
        //存储着自动机生成的数据 [x, z]
        protected T[,] m_map;
        protected int m_numCols;
        protected int m_numRows;

        /// <summary>
        /// 返回网格列数
        /// </summary>
        public int NumCols => m_numCols;

        /// <summary>
        /// 返回网格行数
        /// </summary>
        public int NumRows => m_numRows;

        public CProcedureGridBase(int cols, int rows)
        {
            m_numCols = cols;
            m_numRows = rows;
        }

        /// <summary>
        /// 坐标是否在地图里面
        /// </summary>
        public bool InMapRange(int col, int row)
        {
            return col >= 0 && col < m_numCols && row >= 0 && row < m_numRows;
        }

        public T this[int col, int row]
        {
            get { return m_map[col, row]; }
            set { m_map[col, row] = value; }
        }

        public virtual void Generate()
        {
        }

        public void Print()
        {
            CMapUtil.PrintGird(m_map);
        }
    }
}
