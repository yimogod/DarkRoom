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
	public class CForestGenerator_Tree : CTileMapGeneratorBase
	{
		/// <summary>
		/// 障碍物的比例
		/// </summary>
		public float TreePercent = 0.4f;

		public float TreeHeight1 = 0.23f;
		public float TreeHeight2 = 0.5f;

		public float RockHeight1 = 0.6f;
		public float RockHeight2 = 0.8f;

		public float PlantHeight1 = 0.85f;
		public float PlantHeight2 = 0.94f;

		public CForestGenerator_Tree()
		{
		}

		/// <summary>
		/// 创建基础地形和河流
		/// </summary>
		public void Generate(int cols, int rows)
		{
			m_grid.Init(cols, rows);
			GenerateTree();
		}

		/// <summary>
		/// 删除不合理的树的位置
		/// 比如此地块不可通行, 那么就不能种树
		/// </summary>
		public void DeleteTreeAtIllegalPostion(CAssetGrid terrainGrid)
		{
			var noneType = (int)CForestBlockSubType.None;
			var type = (int)CPCGLayer.Block;

			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					var node = m_grid.GetNode(col, row);
					if(node.SubType == noneType)continue;

					var terrType = (CForestTerrainSubType)terrainGrid.GetNodeSubType(col, row);
					if(CForestUtil.CanPlaceTreeOnTerrainType(terrType))continue;
					m_grid.FillData(col, row, type, noneType, true);
				}
			}

			BreatheTree();
		}

		/// <summary>
		/// 柏林噪声产生地形数据, 
		/// </summary>
		private void GenerateTree()
		{
			var perlin = new CPerlinMap(m_numCols, m_numRows);
			perlin.Generate();

			var type = (int) CPCGLayer.Block;
			var scaleHeight = 1.0f / (1.0f - TreePercent);
			var noneType = (int)CForestBlockSubType.None;

			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					var height = perlin[col, row];

					//不是树
					if (height > TreePercent)
					{
						m_grid.FillData(col, row, type, noneType, true);
						continue;
					}
					else
					{
						height = (height - TreePercent) * scaleHeight;
						var subType = GetSubTypeAtHeight(height);
						m_grid.FillData(col, row, type, (int)subType, false);
					}
				}
			}
		}

		/// <summary>
		/// 根据高度, 从配置中读取相关的asset
		/// </summary>
		private CForestBlockSubType GetSubTypeAtHeight(float height)
		{
			if (height <= TreeHeight1)return CForestBlockSubType.Tree1;
			if (height <= TreeHeight2) return CForestBlockSubType.Tree2;

			if (height <= RockHeight1) return CForestBlockSubType.Rock1;
			if (height <= RockHeight2) return CForestBlockSubType.Rock2;

			if (height <= PlantHeight1) return CForestBlockSubType.Plant1;
			if (height <= PlantHeight2) return CForestBlockSubType.Plant2;

			return CForestBlockSubType.Tree1;
		}

		/// <summary>
		/// 树周边没有东西, 但不可通行
		/// </summary>
		private void BreatheTree()
		{
			var tree1 = (int) CForestBlockSubType.Tree1;
			var tree2 = (int)CForestBlockSubType.Tree2;
			var newSubType = (int)CForestBlockSubType.None;
			var type = (int)CPCGLayer.Block;

			//我们是从左下角开始遍历的, 所以只看右上方向即可
			for (int row = 0; row < m_numRows; ++row)
			{
				for (int col = 0; col < m_numCols; ++col)
				{
					var subType = m_grid.GetNode(col, row).SubType;
					if(subType != tree1 && subType != tree2)continue;

					m_grid.FillData(col + 1, row,       type, newSubType, false);
					m_grid.FillData(col,       row + 1, type, newSubType, false);
					m_grid.FillData(col + 1, row + 1, type, newSubType, false);
				}
			}
		}//现在可以呼吸新鲜空气了
	}
}