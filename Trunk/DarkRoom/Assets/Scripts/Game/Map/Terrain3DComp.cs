using System;
using System.Collections;
using DarkRoom.Core;
using UnityEngine;
namespace Sword {
	public class Terrain3DComp : TerrainComp {
		public Terrain terrain;

		private TerrainData _data;
		//我们的高度图是alpha图的4呗. 这样做是为了平滑高度图
		private float[,] _heightsList;
		private float[,,] _alphasList;

		public override int numRows {
			get { return (int)_data.size.z; }
		}

		public override int numCols {
			get { return (int)_data.size.x; }
		}

		public override int numRowsOfHeight {
			get { return _data.heightmapHeight; }
		}

		public override int numColsOfHeight {
			get { return _data.heightmapWidth; }
		}

		public int layerNum {
			get { return _data.alphamapLayers; }
		}

		protected override void Start() {
			base.Start();

			_data = terrain.terrainData;

			int width = _data.heightmapWidth;
			int height = _data.heightmapHeight;
			_heightsList = _data.GetHeights(0, 0, width, height);

			width = _data.alphamapWidth;
			height = _data.alphamapHeight;
			//int num = terrain.terrainData.alphamapLayers;
			_alphasList = _data.GetAlphamaps(0, 0, width, height);
		}

		public override void ForceBuild() {
			_data.SetAlphamaps(0, 0, _alphasList);
			_data.SetHeights(0, 0, _heightsList);
			terrain.Flush();
		}

		//用value来填充整个tile, 覆盖其他layer
		//unity的terrain用的是索引来代表图层.
		//对于3d terrain comp, 我们仅仅用来处理terrain. 但对于2d, 我们会处理很多. 包含地形, 贴花等
		public override void SetTileByName(int row, int col, int layer, float value) {
			for (int i = 0; i < layerNum; ++i) {
				_alphasList[row, col, i] = 0;
			}

			_alphasList[row, col, layer] = value;
		}

		public void SetTileAddiveByName(int row, int col, int layer, float value) {
			_alphasList[row, col, layer] = value;
		}

		//这个col, row是heightmap的col, row.有可能跟alphamap不一样
		public void SetTileByHeight(int row, int col, float value) {
			_heightsList[row, col] = value;
		}

		//我们约定所有的地形高度最高都为8, 然后我们讲地形推到中间. 做上下起伏
		//此函数功能为生成一个较为平缓的地形
		//目前不做腐蚀地形. 地形一高低起伏. 需要处理的事情太多了!
		[Obsolete]
		public void AverageHeight() {
			int width = _data.heightmapWidth;
			int height = _data.heightmapHeight;

			for (int row = 0; row < height; row += 1) {
				for (int col = 0; col < width; col += 1) {
					_heightsList[row, col] = CDarkRandom.NextPerlinValueNoise(col, row, 0.4f);
				}
			}

			//腐蚀参数
			float k = 0.5f;
			for (int i = 0; i < 4; i++) {
				for (int row = 1; row < height; ++row) {
					for (int col = 0; col < width; ++col) {
						float v1 = _heightsList[row - 1, col];
						float v2 = _heightsList[row, col];
						_heightsList[row, col] = k * v1 + (1 - k) * v2;
					}
				}

				for (int col = 1; col < width; ++col) {
					for (int row = 0; row < height; ++row) {
						float v1 = _heightsList[row, col - 1];
						float v2 = _heightsList[row, col];
						_heightsList[row, col] = k * v1 + (1 - k) * v2;
					}
				}
			}

		}

		public void AverageHeight(float aveHeight) {
			int width = numColsOfHeight;
			int height = numRowsOfHeight;
			float v = aveHeight / _data.size.y;
			for (int row = 0; row < height; row += 1) {
				for (int col = 0; col < width; col += 1) {
					_heightsList[row, col] = v;
				}
			}
		}

		//row = z, col = x
		public override float GetWorldY(int x, int z) {
			float v = _heightsList[z, x];
			return v * _data.size.y;
		}

		//end class
	}
}