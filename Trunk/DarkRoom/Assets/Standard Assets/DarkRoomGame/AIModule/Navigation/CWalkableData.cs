namespace DarkRoom.AI {
	/// <summary>
	/// 地图的通行数据类.
	/// 提供了分块数据以处理超大地图的数据.
	/// 所有数据以一维数组保存.
	/// </summary>
	public class CWalkableData : IWalkable
	{
		//地图尺寸
		public int NumRows;
		public int NumCols;

		//col, row, value
		//基于gird的可通行数据
		private int[,] m_walkableGrid;

		/// <summary>
		/// 传入地图的大小来初始化地图数据
		/// </summary>
		public CWalkableData(int cols, int rows)
		{
			NumRows = rows;
			NumCols = cols;
			m_walkableGrid = new int[cols, rows];
		}

		/// <summary>
		/// 设置地图所有的格子的walkable 为 value
		/// </summary>
		public void MakeAllWalkable(bool value) {
			for (int row = 0; row < NumRows; row++) {
				for (int col = 0; col < NumCols; col++) {
					SetWalkable(col, row, value);
				}
			}
		}

		/// <summary>
		/// Row和Col代表的格子是否是有效格子
		/// </summary>
		/// <returns>当坐标小于 0 或者大于地图尺寸的时候, 格子就是不合法的. 返回false</returns>
		public bool ValidNode(int col, int row)
		{
			if (row < 0 || col < 0) return false;
			if (row >= NumRows) return false;
			if (col >= NumCols) return false;

			return true;
		}

		//设置格子的可通行性
		public void SetWalkable(int col, int row, bool walkable)
		{
			bool b = ValidNode(col, row);
			if (!b) return;
			m_walkableGrid[col, row] = walkable ? 1 : 0;
		}

		//获取格子的可通行性
		public bool IsWalkable(int col, int row)
		{
			bool b = ValidNode(col, row);
			if (!b) return false;

			//if (m_walkableGrid[index] == 0) {
			//	Debug.Log("not walkable");
			//}

			return m_walkableGrid[col, row] == 1;
		}

		public void FillStarGrid(CAStarGrid grid)
		{
			for (int row = 0; row < NumRows; row++) {
				for (int col = 0; col < NumCols; col++) {
					bool w = IsWalkable(col, row);
					grid.SetWalkable(col, row, w);
				}
			}
		}

	}
	
}