using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.PCG
{
	/// <summary>
	/// 使用unity的随机获取的地图数据
	/// </summary>
	public class CUnityRandomMap : CProcedureGridBase<float>
	{
		public CUnityRandomMap(int cols, int rows) : base(cols, rows)
		{
		}

		public override void Generate()
		{
			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					m_map[col, row] = UnityEngine.Random.value;
				}
			}
		}
	}
}