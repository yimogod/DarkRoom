using UnityEngine;

namespace DarkRoom.PCG {
	/// <summary>
	/// 用于过程生成地图的数据结构
	/// </summary>
	public class CProcedureMap {
		//地图所有的node数据.
		//[col][row]
		private Tile[,] m_nodes;
		private int m_numRows;
		private int m_numCols;

		/// <summary>
		/// 默认构造函数. 啥都没干
		/// </summary>
		public CProcedureMap(){}

		/// <summary>
		/// 返回网格列数
		/// </summary>
		/// <value>The number cols.</value>
		public int numCols{ get { return m_numCols; } }

		/// <summary>
		/// 返回网格行数
		/// </summary>
		/// <value>The number rows.</value>
		public int numRows{ get { return m_numRows; } }

		/// <summary>
		/// 初始化二维网格地图
		/// </summary>
		/// <param name="numCols">Number cols.</param>
		/// <param name="numRows">Number rows.</param>
		public void Init(int numCols, int numRows){
			Init(numCols, numRows, true);
		}

		/// <summary>
		/// 初始化二维网格地图. 并指定所有的格子的可通行性的初始化值
		/// </summary>
		public void Init(int numCols, int numRows, bool walkable){
			m_numRows = numRows;
			m_numCols = numCols;

			m_nodes = new Tile[m_numCols, m_numRows];
			for(int row = 0; row < m_numRows; row++){
				for(int col = 0; col < m_numCols; col++){
					m_nodes[col, row] = new Tile(col, row, walkable);
				}
			}
		}

		/// <summary>
		/// 设置地图所有的格子的walkable 为 value
		/// </summary>
		public void SetAllMapWalkable(bool value){
			for(int row = 0; row < numRows; row++){
				for(int col = 0; col < numCols; col++){
					m_nodes[col, row].Walkable = value;
				}
			}
		}

		/// <summary>
		/// 根据坐标获取格子
		/// 如果格子非法, 就返回 null
		/// </summary>
		public Tile GetTile(int col, int row) {
			if (row < 0 || col < 0)return null;
			if (row >= m_numRows)return null;
			if (col >= m_numCols)return null;
			return m_nodes[col, row];
		}

		/// <summary>
		/// 通过坐标本格子是否可通行
		/// 如果格子非法, 则不可通行
		/// </summary>
		public bool IsWalkable(int col, int row) {
			Tile node = GetTile(col, row);
			if(node == null)return false;
			return node.Walkable;
		}

		/// <summary>
		/// 设置合法的格子的可通行性
		/// </summary>
		public void SetWalkable(int col, int row, bool value){
			Tile node = GetTile(col, row);
			if (node == null){
				Debug.LogError("SetWalkable Error");
				return;
			}

			node.Walkable = value;
		}

		public void SetType(int col, int row, Tile.TileType value)
		{
			Tile node = GetTile(col, row);
			if (node == null) {
				Debug.LogError("SetWalkable Error");
				return;
			}

			node.Type = value;
		}

		public void SetAsset(int col, int row, string value) {
			Tile node = GetTile(col, row);
			if (node == null) {
				Debug.LogError("SetAsset Error");
				return;
			}

			node.Asset = value;
		}

		public void Print(){
			string str = "";
			for (int row = m_numRows - 1; row >= 0 ; row--) {
				for (int col = 0; col < m_numCols; col++) {
					bool node = IsWalkable(col, row);
					int v = node ? 1 : 0;
					str = string.Format("{0},{1}", str, v);
				}

				str += "\n";
			}

			Debug.Log(str);
		}

		/// <summary>
		/// 清理本实例持有的一些对象引用
		/// </summary>
		public void Dispose(){
			m_nodes = null;
		}
	}

	/// <summary>
	/// 过程生成的单位格子
	/// </summary>
	public class Tile
	{
		/// <summary>
		/// 格子的类型
		/// </summary>
		public enum TileType {
			RESERVED = -1,//预留的格子
			TERRIAN,
			DECAL,//贴花
			ROAD,
			LAKE,
			ROOM,
			TREE,
			DECO_DESTROY,//可被破坏的装饰物
			DECO_BLOCK//装饰物, 有阻挡
		}

		/// <summary>
		/// The row.
		/// </summary>
		public int Row;

		/// <summary>
		/// The col.
		/// </summary>
		public int Col;

		/// <summary>
		/// 是否可通行
		/// </summary>
		public bool Walkable;

		/// <summary>
		/// 本格子的类型
		/// </summary>
		public TileType Type;

		/// <summary>
		/// 格子上放的asset的名字
		/// </summary>
		public string Asset;

		/* 战争迷雾用到的用来标记4个角落的列表, 长度为4.
		 * 2(0) 1(2)
		 * 4(1) 8(3)
		*/
		public int[] WarFrogTag;

		/// <summary>
		/// tile的四个角落
		/// </summary>
		public enum Corner
		{
			TopLeft = 0,
			BottomLeft = 1,
			TopRight = 2,
			BottomRight = 3
		}

		public Tile(int col, int row, bool walkable) {
			Col = col;
			Row = row;
			Walkable = walkable;
			WarFrogTag = new int[4];
		}

		/// <summary>
		/// 战争迷雾的值
		/// </summary>
		public int WarFrogValue {
			get { return WarFrogTag[0] + WarFrogTag[1] + WarFrogTag[2] + WarFrogTag[3]; }
		}

		/// <summary>
		/// dir代表斜角的4个方向
		/// </summary>
		public void SetFrogCornerValue(Corner corner, int value) {
			WarFrogTag[(int)corner] = value;
		}

		public int GetFrogCornerValue(Corner corner) {
			return WarFrogTag[(int)corner];
		}

		public override string ToString() {
			string str = "col: " + Col + ", row: " + Row;
			return str;
		}
	}
}
