using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG
{
	public class PondGenerator
	{
        //[x, y]
	    private float[,] m_perlinMap;
        //[x, y], 用于平滑池塘的. 简单的生死算法. 
        //初始化全是死亡, 及值为-1
        private float[,] m_pondMap;

        //池塘的尺寸
	    private Vector2Int m_size;

	    //最低点数据
	    private Vector2Int m_lowestSpot;
	    private float m_lowestValue = float.MaxValue;

	    //1. 先遍历所有的格子, 针对每一个格子产生一个柏林噪声值
	    //2. 值最小的地方就是池塘的最低点
	    //3. 循环池塘矩形的四条边, 寻找合理的池塘点
	    //4. 生命游戏圆滑一个池塘边缘
        public void Generate(Vector2Int size){
	        m_perlinMap = new float[size.x, size.y];
	        m_pondMap = new float[size.x, size.y];

            m_size = size;

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
        }

        //四周是不是有邻居
	    private int HasPondTileConnected(int col, int row)
	    {
	        if (col < 0 || col >= m_size.x) return 0;
	        if (row < 0 || row >= m_size.y) return 0;

	        float v = m_pondMap[row, col];
	        return v > 0 ? 1 : 0;
	    }

	    //连接最低点到矩形边上的点, 形成一条直线
	    //然后寻找这个直线上最高的点, 然后这个最高点和最低点之间的格子就是合理的池塘
	    private void Quadrant(int x, int y)
	    {
	        var highestSpot = Vector2Int.zero;
	        float highestValue = float.MinValue;

	        CPondLine line = new CPondLine(m_lowestSpot, new Vector2Int(x,y));
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

	        float split = highestValue + m_lowestValue;
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

	    private void PrintUseful()
	    {
	        string str = "@@@@@ \n";
	        for (int row = 0; row < m_size.y; row++)
	        {
	            string line = "";
	            for (int col = 0; col < m_size.x; col++)
	            {
	                line += m_pondMap[row, col].ToString() + ", ";
	            }
	            line += "\n";
	            str += line;
	        }

	        Debug.Log(str);
	    }
	}


	public class CPondLine
	{
	    public Vector2Int Start;
	    public Vector2Int End;

		private bool _isVertical = false;
        //直线的走向是朝右
		private bool _right = true;
        //直线的走向是朝上
		private bool _up = true;
		private float _k;

		private int _stepTile;

		public CPondLine(Vector2Int start, Vector2Int end)
		{
		    Start = start;
		    End = end;

			int dx = end.x - start.x;
			int dy = end.y - start.y;
			if (dx == 0){
				_isVertical = true;
				_stepTile = start.y;
			} else{
				_k = Mathf.Tan((float)dy / (float)dx);
				_stepTile = start.x;
			}

			_right = dx > 0;
			_up = dy > 0;
		}

		public Vector2Int NextStep(){
			int step = _stepTile;
			if (_isVertical && _up)
				_stepTile++;
			else if (!_isVertical && _right)
				_stepTile++;
			else
				_stepTile--;

			if (_isVertical){
				if(_up && step >= End.x)return CDarkConst.INVALID_VEC2INT;
				if(!_up && step <= End.x) return CDarkConst.INVALID_VEC2INT;
				return new Vector2Int(Start.x, step);
			}

			if(_right && step >= End.y) return CDarkConst.INVALID_VEC2INT;
			if(!_right && step <= End.y) return CDarkConst.INVALID_VEC2INT;

			int dr = (int)(_k * (step - Start.x) + Start.y);
			return new Vector2Int(step, dr);
		}
	}
}