using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 用于过程生成地图的数据结构
	/// </summary>
	public class CAssetGrid : CMapGrid<CAssetNode> {

		/// <summary>
		/// 默认构造函数. 啥都没干
		/// </summary>
		public CAssetGrid(){}

		public void SetValue(int col, int row, int value)
		{
			CAssetNode node = GetNode(col, row);
			if (node == null) {
				Debug.LogError("SetType Error");
				return;
			}

			node.Type = value;
		}

	    public int GetValue(int col, int row)
	    {
	        CAssetNode node = GetNode(col, row);
	        if (node == null)
	        {
	            Debug.LogError("SetType Error");
	            return -1;
	        }

	        return node.Type;
	    }

        public void SetAsset(int col, int row, string value) {
			CAssetNode node = GetNode(col, row);
			if (node == null) {
				Debug.LogError("SetAsset Error");
				return;
			}

			node.Asset = value;
		}

	    public string GetAsset(int col, int row)
	    {
	        CAssetNode node = GetNode(col, row);
	        if (node == null)
	        {
	            Debug.LogError("SetAsset Error");
	            return string.Empty;
	        }

	        return node.Asset;
	    }
    }

	/// <summary>
	/// 过程生成的单位格子
	/// </summary>
	public class CAssetNode : IWalkableNode
	{
	    /// <summary>
	    /// The row.
	    /// </summary>
	    public int Row { get; set; }

	    /// <summary>
		/// The col.
		/// </summary>
		public int Col { get; set; }

        /// <summary>
        /// 是否可通行
        /// </summary>
        public bool Walkable { get; set; }

        /// <summary>
        /// 本格子的类型
        /// </summary>
        public int Type;

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

		public CAssetNode(int col, int row, bool walkable) {
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
