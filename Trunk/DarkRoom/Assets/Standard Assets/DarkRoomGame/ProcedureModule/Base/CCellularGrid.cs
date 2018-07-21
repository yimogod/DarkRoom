using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.PCG
{
    /// <summary>
    /// 自动机地图
    /// </summary>
    public class CCellularGrid : CProcedureGridBase<int>
    {
        //细胞自动机
        private CCellularAutomaton m_cellular;

        public CCellularGrid(int cols, int rows) : base(cols, rows)
        {
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

        public override void Generate()
        {
            m_map = m_cellular.Generate(m_numCols, m_numRows);
        }
    }
}
