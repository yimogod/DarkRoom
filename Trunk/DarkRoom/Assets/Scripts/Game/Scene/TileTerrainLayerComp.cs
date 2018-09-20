using UnityEngine;
using System.Collections;
using DarkRoom.Game;
using DarkRoom.PCG;

namespace Sword
{
	/// <summary>
	/// 挂在地形/装饰物等layer的父亲上
	/// 代表整个terrain go
	/// 
	/// 存储着地形数据/装饰物信息等无关逻辑的信息
	/// </summary>
	public class TileTerrainLayerComp : MonoBehaviour
	{
		public Terrain terrain;

		private TerrainData _data;
		//我们的高度图是alpha图的4呗. 这样做是让陡崖更陡
		private float[,] _heightsList;
		private float[,,] _alphasList;

		private CAssetGrid m_assetGrid;

		private Transform m_tran;

		/// <summary>
		/// 高度图的尺寸
		/// </summary>
		public int numRowsOfHeight
		{
			get { return _data.heightmapHeight; }
		}

		/// <summary>
		/// 高度图的尺寸
		/// </summary>
		public int numColsOfHeight
		{
			get { return _data.heightmapWidth; }
		}

		/// <summary>
		/// terrain 绘制的图层数
		/// </summary>
		public int layerNum
		{
			get { return _data.alphamapLayers; }
		}

		protected void Start()
		{
			m_tran = transform;

			int desireLayer = LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_TERRAIN);
			if (desireLayer == -1)
			{
				Debug.LogError("We Do Not Set Terrain Layer. Please Set One Terrain layer");
			}

			//获取terrain相关数据
			_data = terrain.terrainData;

			int width = _data.heightmapWidth;
			int height = _data.heightmapHeight;
			_heightsList = _data.GetHeights(0, 0, width, height);

			width = _data.alphamapWidth;
			height = _data.alphamapHeight;
			_alphasList = _data.GetAlphamaps(0, 0, width, height);
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

			_data.SetAlphamaps(0, 0, _alphasList);
			_data.SetHeights(0, 0, _heightsList);
			terrain.Flush();
		}

		public void CreateTerrain()
		{

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
		private void SetTileByName(int row, int col, int layer, float value)
		{
			for (int i = 0; i < layerNum; ++i)
			{
				_alphasList[row, col, i] = 0;
			}

			_alphasList[row, col, layer] = value;
		}

		private void SetTileAddiveByName(int row, int col, int layer, float value)
		{
			_alphasList[row, col, layer] = value;
		}

		//这个col, row是heightmap的col, row. 跟alphamap不一样
		private void SetTileByHeight(int row, int col, float value)
		{
			_heightsList[row, col] = value;
		}

		IEnumerator CoroutineBuild()
		{
			for (int i = 0; i < m_assetGrid.NumCols; i++)
			{
				yield return new WaitForEndOfFrame();
				for (int j = 0; j < m_assetGrid.NumRows; j++)
				{
					var tile = m_assetGrid.GetNodeSubType(i, j);
					var prefab = GetPrefabBySubType(tile);

					var pos = CMapUtil.GetTileCenterPosByColRow(i, j);
					LoadAndCreateTile(prefab, pos);
				}
			}

			yield return new WaitForEndOfFrame();
		}

		private string GetPrefabBySubType(int subType)
		{
			CForestTerrainSubType index = (CForestTerrainSubType) subType;
			string str = string.Empty;
			switch (index)
			{
				case CForestTerrainSubType.Grass1:
					str = "Tile_1_A";
					break;
				case CForestTerrainSubType.Grass2:
					str = "Tile_1_B";
					break;
				case CForestTerrainSubType.Land1:
					str = "Tile_1_C";
					break;
				case CForestTerrainSubType.Land2:
					str = "Tile_1_D";
					break;
			}

			return str;
		}

		private void LoadAndCreateTile(string prefab, Vector3 pos)
		{
			AssetManager.LoadTilePrefab("map_forest", prefab, m_tran, pos);
		}

		//end class
	}
}