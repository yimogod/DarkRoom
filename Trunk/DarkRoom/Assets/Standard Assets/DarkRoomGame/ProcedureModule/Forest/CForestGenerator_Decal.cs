using System;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.PCG
{
	/// <summary>
	/// 产生森林里面的树, 石头, 蘑菇, 坟地等障碍
	/// 
	/// 注意, 树会占用3x3的不可通行的格子
	/// 我们在产生树之后清理他周围的东西
	/// </summary>
	public class CForestGenerator_Decal : CTileMapGeneratorBase
	{
		/// <summary>
		/// 装饰物的比例
		/// </summary>
		public float DecalPercent = 0.06f;

		public float Height1 = 0.1f;
		public float Height2 = 0.2f;
		public float Height3 = 0.3f;
		public float Height4 = 0.4f;
		public float Height5 = 0.5f;
		public float Height6 = 0.6f;
		public float Height7 = 0.7f;
		public float Height8 = 0.8f;
		public float Height9 = 0.9f;
		public float Height10 = 1f;

		public CForestGenerator_Decal()
		{
		}

		/// <summary>
		/// 创建基础地形和河流
		/// </summary>
		public void Generate(int cols, int rows)
		{
			m_grid.Init(cols, rows);
			GenerateDecal();
		}

		/// <summary>
		/// 删除不合理位置的贴花
		/// </summary>
		public void DeleteDecalAtIllegalTerrainPostion(CAssetGrid terrainGrid)
		{
			var noneType = 0;
			var type = (int)CPCGLayer.Decal;

			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					var node = m_grid.GetNode(col, row);
					if(node.SubType == noneType)continue;

					var terrType = (CForestTerrainSubType)terrainGrid.GetNodeSubType(col, row);
					if(CForestUtil.CanPlacePropsOnTerrainType(terrType))continue;
					m_grid.FillData(col, row, type, noneType, true);
				}
			}
		}

		/// <summary>
		/// 删除不合理位置的贴花
		/// </summary>
		public void DeleteDecalAtIllegalBlockPostion(CAssetGrid blockGrid)
		{
			var noneType = 0;
			var type = (int)CPCGLayer.Decal;

			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					var node = m_grid.GetNode(col, row);
					if (node.SubType == noneType) continue;

					var walkable = blockGrid.IsWalkable(col, row);
					if (walkable) continue;
					m_grid.FillData(col, row, type, noneType, true);
				}
			}
		}

		private void GenerateDecal()
		{
			var perlin = new CUnityRandomMap(m_numCols, m_numRows);
			perlin.Generate();

			var type = (int) CPCGLayer.Decal;
			var scaleHeight = 1.0f / DecalPercent;
			var noneType = 0;

			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					var height = perlin[col, row];

					//没有在放置范围
					if (height > DecalPercent)
					{
						m_grid.FillData(col, row, type, noneType, true);
						continue;
					}

					height *= scaleHeight;
					var subType = GetSubTypeAtHeight(height);
					m_grid.FillData(col, row, type, subType, true);
				}
			}
		}

		/// <summary>
		/// 根据高度, 从配置中读取相关的asset
		/// </summary>
		private int GetSubTypeAtHeight(float height)
		{
			if (height <= Height1) return 1;
			if (height <= Height2) return 2;
			if (height <= Height3) return 3;
			if (height <= Height4) return 4;
			if (height <= Height5) return 5;
			if (height <= Height6) return 6;
			if (height <= Height7) return 7;
			if (height <= Height8) return 8;
			if (height <= Height9) return 9;
			if (height <= Height10) return 10;

			return 0;
		}

	}
}