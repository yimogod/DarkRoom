using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.PCG
{
    /// <summary>
    /// 自动机地图
    /// </summary>
    public class CCellularMap
    {
        //存储着自动机生成的数据 [x, z]
        private int[,] m_map;
        private int m_numCols;
        private int m_numRows;

        public int NumCols { get { return m_numCols; } }
        public int NumRows { get { return m_numRows; } }

        public CCellularMap(int[,] map)
        {
            m_map = map;
            m_numCols = m_map.GetLength(0);
            m_numRows = m_map.GetLength(1);
        }

        public CCellularMap(int cols, int rows)
        {
            m_numCols = cols;
            m_numRows = rows;
            m_map = new int[rows, cols];
        }

        public void MakeAlive(int col, int row)
        {
            bool b = InMapRange(col, row);
            if (b)
            {
                m_map[col, row] = 1;
            }
        }

        /// <summary>
        /// 坐标是否在地图里面
        /// </summary>
        public bool InMapRange(int col, int row)
        {
            return col >= 0 && col < m_numCols && row >= 0 && row < m_numRows;
        }

        public int this[int col, int row]
        {
            get { return m_map[col, row]; }
            set { m_map[col, row] = value; }
        }

        public void Print()
        {
            CMapUtil.PrintGird(m_map);
        }
    }
}
