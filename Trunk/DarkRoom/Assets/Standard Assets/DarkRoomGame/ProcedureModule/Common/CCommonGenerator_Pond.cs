using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG
{
	public class CPondGenerator
	{
        //[x, y]
	    private float[,] m_perlinMap;
        //[x, y], 用于平滑池塘的. 简单的生死算法. 
        //初始化全是死亡, 及值为-1
        private float[,] m_pondMap;

        //池塘的尺寸
	    private Vector2Int m_size = Vector2Int.zero;

	    //最低点数据
	    private Vector2Int m_lowestSpot;
	    private float m_lowestValue = float.MaxValue;

	    //1. 先遍历所有的格子, 针对每一个格子产生一个柏林噪声值
	    //2. 值最小的地方就是池塘的最低点
	    //3. 循环池塘矩形的四条边, 寻找合理的池塘点
	    //4. 生命游戏圆滑一个池塘边缘
        public float[,] Generate(Vector2Int size){
            if (m_size != size)
            {
                m_perlinMap = new float[size.x, size.y];
                m_pondMap = new float[size.x, size.y];
                m_size = size;
            }

            m_lowestSpot = Vector2Int.zero;
	        m_lowestValue = float.MaxValue;

            //对整个地图产生柏林模糊, 然后找到最低点
	        for (int y = 0; y < m_size.y; y++)
	        {
	            for (int x = 0; x < m_size.x; x++)
	            {
	                m_pondMap[x, y] = -1f;
	                float perlin = CDarkRandom.NextPerlinValueNoise(x, y, 0.4f);
	                m_perlinMap[x, y] = perlin;

	                if (perlin > m_lowestValue) continue;
	                m_lowestValue = perlin;
	                m_lowestSpot = new Vector2Int(x, y);
	            }
	        }

            //两道横边
            for (int x = 0; x < m_size.x; x++)
            {
                Quadrant(x, 0);
                Quadrant(x, m_size.y - 1);
            }

            //两道竖边
            for (int y = 0; y < m_size.y; y++)
	        {
	            Quadrant(0, y);
	            Quadrant(m_size.x - 1, y);
	        }

            //平滑池塘
	        for (int y = 0; y < m_size.y; y++)
	        {
	            for (int x = 0; x < m_size.x; x++)
	            {
	                int cell = 0;
	                cell += HasPondTileConnected(x + 1, y);
	                cell += HasPondTileConnected(x + 1, y + 1);
	                cell += HasPondTileConnected(x, y + 1);
	                cell += HasPondTileConnected(x - 1, y + 1);
	                cell += HasPondTileConnected(x - 1, y);
	                cell += HasPondTileConnected(x - 1, y - 1);
	                cell += HasPondTileConnected(x, y - 1);
	                cell += HasPondTileConnected(x + 1, y - 1);

	                if (cell < 4)m_pondMap[x, y] = -1;
	            }
	        }

            return m_pondMap;
        }

        //四周是不是有邻居
	    private int HasPondTileConnected(int x, int y)
	    {
	        if (x < 0 || x >= m_size.x) return 0;
	        if (y < 0 || y >= m_size.y) return 0;

	        float v = m_pondMap[x, y];
	        return v > 0 ? 1 : 0;
	    }

	    //连接最低点到矩形边上的点, 形成一条直线
	    //然后寻找这个直线上最高的点, 然后这个最高点和最低点之间的格子就是合理的池塘
	    private void Quadrant(int x, int y)
	    {
	        var highestSpot = Vector2Int.zero;
	        float highestValue = float.MinValue;

	        CPondLine line = new CPondLine(m_lowestSpot, new Vector2Int(x,y));

            //获取直线上的最高点
	        var v = line.NextStep();
	        while (CDarkUtil.IsValidVec2Int(v))
	        {
	            if (v.y >= m_size.y || v.x >= m_size.x || v.y < 0 || v.x < 0)
	            {
	                //Debug.Log("Error line at " + v.ToString());
	                v = line.NextStep();
	                continue;
	            }
	            float p = m_perlinMap[v.x, v.y];
	            if (p > highestValue)
	            {
	                highestValue = p;
	                highestSpot = v;
	            }

	            v = line.NextStep();
	        }

	        //float split = highestValue + m_lowestValue;
	        line = new CPondLine(m_lowestSpot, highestSpot);
	        v = line.NextStep();
	        while (CDarkUtil.IsValidVec2Int(v))
	        {
	            if (v.y >= m_size.y || v.x >= m_size.x || v.y < 0 || v.x < 0)
	            {
	                //Debug.Log("Error line at " + v.ToString());
	                v = line.NextStep();
	                continue;
	            }

	            m_pondMap[v.x, v.y] = m_perlinMap[v.x, v.y];
	            v = line.NextStep();
	        }

	    }
	}


    /// <summary>
    /// 用于池塘的线段结构
    /// </summary>
	public struct CPondLine
	{
	    private Vector2Int m_start;
	    private Vector2Int m_end;

		private bool m_isVertical;
        //直线的走向是朝右
		private bool m_right;
        //直线的走向是朝上
		private bool m_up;
		private float m_k;

		private int m_stepTile;

		public CPondLine(Vector2Int start, Vector2Int end)
		{
		    m_isVertical = false;

            m_start = start;
		    m_end = end;

			int dx = end.x - start.x;
			int dy = end.y - start.y;
			if (dx == 0){
				m_isVertical = true;
				m_stepTile = start.y;
			    m_k = 1f;
            } else{
				m_k = Mathf.Tan((float)dy / (float)dx);
				m_stepTile = start.x;
			}

			m_right = dx > 0;
			m_up = dy > 0;
		}

		public Vector2Int NextStep(){
			int step = m_stepTile;
			if (m_isVertical && m_up)
				m_stepTile++;
			else if (!m_isVertical && m_right)
				m_stepTile++;
			else
				m_stepTile--;

			if (m_isVertical){
				if(m_up && step >= m_end.x)return CDarkConst.INVALID_VEC2INT;
				if(!m_up && step <= m_end.x) return CDarkConst.INVALID_VEC2INT;
				return new Vector2Int(m_start.x, step);
			}

			if(m_right && step >= m_end.y) return CDarkConst.INVALID_VEC2INT;
			if(!m_right && step <= m_end.y) return CDarkConst.INVALID_VEC2INT;

			int dr = (int)(m_k * (step - m_start.x) + m_start.y);
			return new Vector2Int(step, dr);
		}
	}
}