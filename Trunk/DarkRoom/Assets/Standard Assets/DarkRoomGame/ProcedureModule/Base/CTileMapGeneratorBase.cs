using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.PCG
{
	public class CTileMapGeneratorBase
	{
		protected int m_numCols => m_grid.NumCols;

		/// <summary>
		/// 地图高度
		/// </summary>
		protected int m_numRows => m_grid.NumRows;

		//存储地形类型, 辅助地形自动生成
		//walk存储可通行数据, 同时有type和asset字段
		protected CAssetGrid m_grid = new CAssetGrid();

		public CAssetGrid Grid => m_grid;


		/// <summary>
		/// 初始化gird全部可以通行
		/// </summary>
		public virtual void Generate()
		{
			m_grid.Init(m_numCols, m_numRows, true);
		}
	}
}