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
	public class CForestGenerator_Block : CTileMapGeneratorBase
	{
		/// <summary>
		/// 障碍物的比例
		/// </summary>
		public float BlockPercent = 0.06f;

		public float TreeHeight = 0.5f;
		public float RockHeight = 0.6f;
		public float PlantHeight = 0.7f;

		public float PropHeight_1 = 0.75f;
		public float PropHeight_2 = 0.80f;
		public float PropHeight_3 = 0.85f;
		public float PropHeight_4 = 0.90f;
		public float PropHeight_5 = 0.95f;

		public CForestGenerator_Block()
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

		private void GenerateTree()
		{
			var perlin = new CUnityRandomMap(m_numCols, m_numRows);
			perlin.Generate();

			var type = (int) CPCGLayer.Block;
			var scaleHeight = 1.0f / BlockPercent;
			var noneType = (int)CForestBlockSubType.None;

			for (int col = 0; col < m_numCols; col++)
			{
				for (int row = 0; row < m_numRows; row++)
				{
					var height = perlin[col, row];

					//不是树
					if (height > BlockPercent)
					{
						m_grid.FillData(col, row, type, noneType, true);
						continue;
					}

					height *= scaleHeight;
					var subType = GetSubTypeAtHeight(height);
					m_grid.FillData(col, row, type, (int)subType, false);
				}
			}
		}

		/// <summary>
		/// 根据高度, 从配置中读取相关的asset
		/// </summary>
		private CForestBlockSubType GetSubTypeAtHeight(float height)
		{
			if (height <= TreeHeight)return CForestBlockSubType.Tree;
			if (height <= RockHeight) return CForestBlockSubType.Rock;
			if (height <= PlantHeight) return CForestBlockSubType.Plant;

			if (height <= PropHeight_1) return CForestBlockSubType.Prop1;
			if (height <= PropHeight_2) return CForestBlockSubType.Prop2;
			if (height <= PropHeight_3) return CForestBlockSubType.Prop3;
			if (height <= PropHeight_4) return CForestBlockSubType.Prop4;
			if (height <= PropHeight_5) return CForestBlockSubType.Prop5;

			return CForestBlockSubType.Tree;
		}

		/// <summary>
		/// 树周边没有东西, 但不可通行
		/// </summary>
		private void BreatheTree()
		{
			var tree = (int)CForestBlockSubType.Tree;
			var newSubType = (int)CForestBlockSubType.None;
			var type = (int)CPCGLayer.Block;

			//我们是从左下角开始遍历的, 所以只看右上方向即可
			for (int row = 0; row < m_numRows; ++row)
			{
				for (int col = 0; col < m_numCols; ++col)
				{
					var subType = m_grid.GetNode(col, row).SubType;
					if(subType != tree)continue;

                    m_grid.FillData(col - 1, row + 1, type, newSubType, false);
                    m_grid.FillData(col,     row + 1, type, newSubType, false);
                    m_grid.FillData(col + 1, row + 1, type, newSubType, false);
                    m_grid.FillData(col + 1, row,     type, newSubType, false);
                    m_grid.FillData(col + 1, row - 1, type, newSubType, false);

                }
			}
		}//现在可以呼吸新鲜空气了
	}
}