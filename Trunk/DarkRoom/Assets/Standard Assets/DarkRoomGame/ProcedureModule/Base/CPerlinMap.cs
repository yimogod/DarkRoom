using System;
using System.Collections.Generic;

namespace DarkRoom.PCG
{
    /// <summary>
    /// 柏林模糊生成的地图
    /// </summary>
    public class CPerlinMap
    {
        //存储着柏林模糊生成的数据 [x, z]
        private float[,] m_map;
        private int m_numCols;
        private int m_numRows;

        //柏林模糊
        private CPerlinNoise m_perlin;

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

        public CPerlinMap(int cols, int rows)
        {
            m_numCols = cols;
            m_numRows = rows;
        }

        /// <summary>
        /// 坐标是否在地图里面
        /// </summary>
        public bool InMapRange(int x, int z)
        {
            return x >= 0 && x < m_numCols && z >= 0 && z < m_numRows;
        }

        public float this[int x, int z]
        {
            get { return m_map[x, z]; }
            set { m_map[x, z] = value; }
        }

        public void Generate()
        {
            m_map = m_perlin.GetNoiseValues(m_numCols, m_numRows);
        }
    }
}
