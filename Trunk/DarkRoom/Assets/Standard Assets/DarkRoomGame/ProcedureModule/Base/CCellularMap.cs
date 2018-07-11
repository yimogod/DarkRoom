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

        //细胞自动机
        private CCellularAutomaton m_cellular;

        /// <summary>
        /// 返回网格列数
        /// </summary>
        /// <value>The number cols.</value>
        public int NumCols { get { return m_numCols; } }

        /// <summary>
        /// 返回网格行数
        /// </summary>
        /// <value>The number rows.</value>
        public int NumRows { get { return m_numRows; } }

        public CCellularMap(int cols, int rows)
        {
            m_numCols = cols;
            m_numRows = rows;
            m_cellular = new CCellularAutomaton();
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

        public void Generate()
        {
            m_map = m_cellular.GenerateTerrianWithCellular(m_numCols, m_numRows);
        }

        public void Print()
        {
            CMapUtil.PrintGird(m_map);
        }
    }
}
