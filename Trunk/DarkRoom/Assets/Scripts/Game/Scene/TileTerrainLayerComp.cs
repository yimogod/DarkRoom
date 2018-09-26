using UnityEngine;
using System.Collections;
using DarkRoom.Game;
using DarkRoom.PCG;
using DarkRoom.AI;

namespace Sword
{
	/// <summary>
	/// 挂在地形/装饰物等layer的父亲上
	/// 代表整个terrain go
	/// 
	/// 存储着地形数据/装饰物信息等无关逻辑的信息
	/// 
	/// 注意, alpha的尺寸是terrain尺寸的2x2=4倍
	/// 高度图的尺寸是4x4 =16
	/// </summary>
	public class TileTerrainLayerComp : MonoBehaviour
	{
		public Terrain Terrain;

		private TerrainData m_data;
		private float[,] m_heightsList;
		private float[,,] m_alphasList;

		private CAssetGrid m_assetGrid;

		/// <summary>
		/// 高度图的尺寸
		/// </summary>
		public int NumRowsOfHeight
		{
			get { return m_data.heightmapHeight; }
		}

		/// <summary>
		/// 高度图的尺寸
		/// </summary>
		public int NumColsOfHeight
		{
			get { return m_data.heightmapWidth; }
		}

		/// <summary>
		/// terrain 绘制的图层数
		/// 相当于texture的个数
		/// </summary>
		public int LayerNum
		{
			get { return m_data.alphamapLayers; }
		}

		void Awake()
		{
			int desireLayer = LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_TERRAIN);
			if (desireLayer == -1)
			{
				Debug.LogError("We Do Not Set Terrain Layer. Please Set One Terrain layer");
			}

			Terrain = Terrain.activeTerrain;


			//获取terrain相关数据
			m_data = Terrain.terrainData;

			int width = m_data.heightmapWidth;
			int height = m_data.heightmapHeight;
			m_heightsList = m_data.GetHeights(0, 0, width, height);
			//Debug.Log(width + "  " + height);

			width = m_data.alphamapWidth;
			height = m_data.alphamapHeight;
			m_alphasList = m_data.GetAlphamaps(0, 0, width, height);
			//Debug.Log(LayerNum);
			//Debug.Log(width + "  " + height);
		}

		public void SetAssetGrid(CAssetGrid grid)
		{
			m_assetGrid = grid;
		}

		public void Build()
		{
			if (m_assetGrid == null)
			{
				Debug.LogError("AssetGrid Must Not Null");
				return;
			}

			CreateTerrain();
			CreatePond();

			m_data.SetAlphamaps(0, 0, m_alphasList);
			m_data.SetHeights(0, 0, m_heightsList);
			Terrain.Flush();
		}

		public void CreateTerrain()
		{
			//CMapUtil.PrintGird(m_assetGrid.RawData);
			for (int col = 0; col < m_assetGrid.NumCols; col++)
			{
				for (int row = 0; row < m_assetGrid.NumRows; row++)
				{
					int subType = m_assetGrid.GetNodeSubType(col, row);
					SetTileByName(col, row, subType, 1);
					switch ((CForestTerrainSubType)subType)
					{
						case CForestTerrainSubType.Grass1:
						case CForestTerrainSubType.Grass2:
						case CForestTerrainSubType.Land1:
						case CForestTerrainSubType.Land2:
							SetTileByHeight(col, row, 0.5f);
							break;
						case CForestTerrainSubType.Hill:
							SetTileByHeight(col, row, 0.7f);
							break;
						case CForestTerrainSubType.Pond:
							SetTileByHeight(col, row, 0.1f);
							break;
						case CForestTerrainSubType.Road:
							SetTileByHeight(col, row, 0.5f);
							break;
					}
				}
			}
		}

		//我们简单的将地形推低, 露出之前铺满的水平面即可
		public void CreatePond()
		{
			//降低地形, 这个时候walkable = true的是池塘
			//foreach (PondInfo pond in _pondList)
			//{
			//	terrainGenerator.UpdateHeightTerrainByPond(pond, 0.1f, 0.25f);
			//}

			//在这里特别要注意, 我们把池塘的不可通行区域扩充了一个格子
			//因为地形高度插值的原因-平地和池塘平滑过渡的地方, 我们设置为不可通行
			//CMapUtil.ExpandAliveAreaInGrid(_aliveGrid);
			//CMapUtil.ExpandAliveAreaInGrid(_aliveGrid);

			//标记地形为池塘
			//MapUtil.SetTileTypeWithAliveGird(typeGrid, _aliveGrid, (int)RayConst.TileType.LAKE);

			//拷贝池塘的不可通行到我们的通行表
			//_aliveGrid.RevertAliveDead();
			//walkGrid.CopyUnWalkableFrom(_aliveGrid);
		}



		//用value来填充整个tile, 覆盖其他layer
		//unity的terrain用的是索引来代表图层.
		private void SetTileByName(int col, int row, int layer, float value)
		{
            int unit = 2;
			int sc = col * unit;
			int ec = sc + unit;
			int sr = row * unit;
			int er = sr + unit;

			for (int c = sc; c < ec; c++){
				for (int r = sr; r < er; r++){
					for (int i = 0; i < LayerNum; ++i){
						m_alphasList[r, c, i] = 0;
					}
					m_alphasList[r, c, layer] = value;
				}
			}
		}

		private void SetTileAddiveByName(int col, int row, int layer, float value)
		{
			m_alphasList[row, col, layer] = value;
		}


		//这个col, row是alphamap的col, row
		//所以我们要做个转换
		private void SetTileByHeight(int col, int row, float value)
		{
            int unit = 4;
			int sc = col * unit;
			int ec = sc + unit;
			int sr = row * unit;
			int er = sr + unit;
			
			for (int c = sc; c < ec; c++)
			{
				for (int r = sr; r < er; r++)
				{
					m_heightsList[r, c] = value;
				}
			}
		}

		//end class
	}
}