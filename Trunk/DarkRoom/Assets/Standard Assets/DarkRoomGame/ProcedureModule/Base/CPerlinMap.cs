using System;
using System.Collections.Generic;

namespace DarkRoom.PCG
{
    /// <summary>
    /// 柏林模糊生成的地图
    /// </summary>
    public class CPerlinMap : CProcedureGridBase<float>
    {
        //柏林模糊
        private CPerlinNoise m_perlin;
        
        public CPerlinMap(int cols, int rows) : base(cols, rows)
        {
            m_perlin = new CPerlinNoise();
        }

        public override void Generate()
        {
            m_map = m_perlin.GetNoiseValues(m_numCols, m_numRows);
        }
    }
}
