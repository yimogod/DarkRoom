using System;
using System.Collections.Generic;
using UnityEngine;
namespace Rayman{
	#region generator
	public class PondGenerator{
		protected MapGrid _aliveGrid;
		protected int _numCols;
		protected int _numRows;
		protected int _pondMinSize;
		protected int _pondMaxSize;

		protected List<PondInfo> _pondList = new List<PondInfo>();
		protected MapCellularHelper _helper = new MapCellularHelper();

		public PondGenerator(int cols, int rows){
			_numCols = cols;
			_numRows = rows;

			_aliveGrid = new MapGrid();
			_aliveGrid.Init(_numCols, _numRows);
			_helper.Init(_numCols, _numRows);

			_pondMinSize = (int)(_numRows / 4);
			_pondMaxSize = (int)(_numRows / 2);
		}

		//用细胞自动机产生池塘的数据, 返回的grid数据中, alive = true代表这个地方是池塘
		//池塘的左下脚要从8-8开始, 5-5之内我们需要随机出生点和道路, 但我们会扩展两圈的不可通行,
		//所以我们池塘从8-8开始
		protected void GeneratePondData(int pondNum, int pondMinSize, int pondMaxSize){
			_pondList.Clear();
			//我们的地图默认都是活着的
			_aliveGrid.MakeAllWalkable(true);

			for(int i = 0; i < pondNum; ++i){
				int cols = RayRandom.Next(pondMinSize, pondMaxSize);
				int rows = RayRandom.Next(pondMinSize, pondMaxSize);
				//左下角坐标随机一个
				//池塘的外围矩形就确定了. 我们要在这个矩形中找一个不规则闭合曲线来做池塘
				int minCol = RayRandom.Next(8, _numCols - cols);
				int minRow = RayRandom.Next(8, _numRows - rows);

				PondInfo pond = new PondInfo(minCol, minRow, cols, rows);
				pond.Generate(_aliveGrid);
				_pondList.Add(pond);



				//细胞机产生池塘--活的是池塘
				//马丹. 这里产生池塘算法有问题
				//_helper.MakeGridDead();
				//MapGrid grid = _helper.GenerateTerrianWithCellular(42, 5, rect, false);
				//PondInfo pond = new PondInfo(grid.GetCloneOne());
				//_aliveGrid.CopyUnWalkableFrom(grid, minX, minZ, (int)(minX + size.x), (int)(minZ + size.z));
			}

			_aliveGrid.RevertAliveDead();
		}

		public virtual void Dispose(){
			_pondList.Clear();
			_pondList = null;

			_helper.Dispose();
			_helper = null;
		}
	}
	#endregion

	#region PondInfo
	public class PondInfo{
		private float[,] _perlinNodes;
		private float[,] _usefullNodes;

		private int _minCol;
		private int _minRow;
		private int _cols;
		private int _rows;

		//最低点数据
		private int _lowestCol = 0;
		private int _lowestRow = 0;
		private float _lowestValue = float.MaxValue;
		//最高点
		private int _highestCol = 0;
		private int _highestRow = 0;
		private float _highestValue = float.MinValue;


		//walk/alive = true 的是池塘
		public PondInfo(int minCol, int minRow, int cols, int rows){
			_minCol = minCol;
			_minRow = minRow;
			_cols = cols;
			_rows = rows;
		}

		public int startRow{
			get{ return _minRow; }
		}

		public int startCol{
			get{ return _minCol; }
		}

		public int numRows{
			get{ return _rows; }
		}

		public int numCols{
			get{ return _cols; }
		}

		public float GetUsefulValue(int row, int col){
			return _usefullNodes[row, col];
		}

		//1. 先遍历所有的格子, 针对每一个格子产生一个柏林噪声值
		//2. 值最小的地方就是池塘的最低点
		//3. 循环池塘矩形的四条边, 寻找合理的池塘点
		//4. 生命游戏圆滑一个池塘边缘
		public void Generate(MapGrid aliveGrid){
			_perlinNodes = new float[_rows, _cols];
			_usefullNodes = new float[_rows, _cols];

			_lowestCol = 0;
			_lowestRow = 0;
			_lowestValue = float.MaxValue;

			Debug.Log("generate------------------ " + _rows + " -- " + _cols);
			for (int row = 0; row < _rows; row++){
				for (int col = 0; col < _cols; col++){
					_usefullNodes[row, col] = -1f;
					float perlin = RayRandom.NextPerlinValueNoise(0.4f, col, row);
					_perlinNodes[row, col] = perlin;

					if (perlin > _lowestValue)continue;
					_lowestValue = perlin;
					_lowestCol = col;
					_lowestRow = row;
				}
			}

			//两道竖边
			for (int row = 0; row < _rows; row++){
				Quadrant(0, row);
				Quadrant(_cols - 1, row);
			}

			//两道横边
			for (int col = 0; col < _cols; col++){
				Quadrant(col, 0);
				Quadrant(col, _rows - 1);
			}

			//PrintUseful();
			for (int row = 0; row < _rows; row++){
				for (int col = 0; col < _cols; col++){
					int cell = 0;
					cell += hasUseful(col + 1, row);
					cell += hasUseful(col + 1, row + 1);
					cell += hasUseful(col, row + 1);
					cell += hasUseful(col - 1, row + 1);
					cell += hasUseful(col - 1, row);
					cell += hasUseful(col - 1, row - 1);
					cell += hasUseful(col, row - 1);
					cell += hasUseful(col + 1, row - 1);

					if (cell < 4)
						_usefullNodes[row, col] = -1;
				}
			}
			//PrintUseful();

			for (int row = 0; row < _rows; row++){
				for (int col = 0; col < _cols; col++){
					float v = _usefullNodes[row, col];
					if (v > 0)
						aliveGrid.SetWalkable(_minRow + row, _minCol + col, false);
				}
			}

		}

		private void PrintUseful(){
			string str = "@@@@@ \n";
			for (int row = 0; row < _rows; row++){
				string line = "";
				for (int col = 0; col < _cols; col++){
					line += _usefullNodes[row, col].ToString() +  ", ";
				}
				line += "\n";
				str += line;
			}

			Debug.Log(str);
		}

		private int hasUseful(int col, int row){
			if (col < 0 || col >= _cols)return 0;
			if (row < 0 || row >= _rows)return 0;

			float v = _usefullNodes[row, col];
			return v > 0 ? 1 : 0;
		}

		//连接最低点到矩形边上的点, 形成一条直线
		//然后寻找这个直线上最高的点, 然后这个最高点和最低点之间的格子就是合理的池塘
		private void Quadrant(int col, int row){
			_highestCol = 0;
			_highestRow = 0;
			_highestValue = float.MinValue;

			PondLine line = new PondLine(_lowestCol, _lowestRow, col, row);
			Vector3 v = line.NextStep();
			while (RayUtil.IsValidVec3(v)){
				int c = (int)v.x;
				int r = (int)v.z;
				if (r >= _rows || c >= _cols || r < 0 || c < 0){
					//Debug.Log("Error line at " + v.ToString());
					v = line.NextStep();
					continue;
				}
				float p = _perlinNodes[r, c];
				if (p > _highestValue){
					_highestValue = p;
					_highestCol = c;
					_highestRow = r;
				}


				v = line.NextStep();
			}

			float split = _highestValue + _lowestValue;
			line = new PondLine(_lowestCol, _lowestRow, _highestCol, _highestRow);
			v = line.NextStep();
			while (RayUtil.IsValidVec3(v)){
				int c = (int)v.x;
				int r = (int)v.z;
				if (r >= _rows || c >= _cols || r < 0 || c < 0){
					//Debug.Log("Error line at " + v.ToString());
					v = line.NextStep();
					continue;
				}

				_usefullNodes[r, c] = _perlinNodes[r, c];

				v = line.NextStep();
			}

		}
	}
	#endregion

	#region line
	public class PondLine{
		public int _startCol;
		public int _startRow;
		public int _endCol;
		public int _endRow;

		private bool _isVertical = false;	
		private bool _right = true;
		private bool _up = true;
		private float _k;

		private int _stepTile;

		public PondLine(int startCol, int startRow, int endCol, int endRow){
			_startCol = startCol;
			_startRow = startRow;
			_endCol = endCol;
			_endRow = endRow;

			int dc = _endCol - _startCol;
			int dr = _endRow - _startRow;
			if (dc == 0){
				_isVertical = true;
				_stepTile = startRow;
			} else{
				_k = Mathf.Tan((float)dr / (float)dc);
				_stepTile = startCol;
			}

			_right = dc > 0;
			_up = dr > 0;
		}

		public Vector3 NextStep(){
			int step = _stepTile;
			if (_isVertical && _up)
				_stepTile++;
			else if (!_isVertical && _right)
				_stepTile++;
			else
				_stepTile--;

			if (_isVertical){
				if(_up && step >= _endCol)return RayConst.INVALID_VEC3;
				if(!_up && step <= _endCol)return RayConst.INVALID_VEC3;
				return new Vector3(_startCol, 0, step);
			}

			if(_right && step >= _endRow)return RayConst.INVALID_VEC3;
			if(!_right && step <= _endRow)return RayConst.INVALID_VEC3;

			int dr = (int)(_k * (step - _startCol) + _startRow);
			return new Vector3(step, 0, dr);
		}
	}

	#endregion
}