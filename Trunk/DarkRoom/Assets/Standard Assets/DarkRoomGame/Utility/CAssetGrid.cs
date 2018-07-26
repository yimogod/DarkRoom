using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 用于过程生成地图的数据结构
	/// </summary>
	public class CAssetGrid : CMapGrid<CAssetNode> {

	    public void SetNodeType(int col, int row, int value)
		{
			CAssetNode node = GetNode(col, row);
			if (node.Invalid) {
				Debug.LogError("SetType Error");
				return;
			}

			node.Type = value;
		}

	    public int GetNodeType(int col, int row)
	    {
	        CAssetNode node = GetNode(col, row);
	        if (node.Invalid)
	        {
	            Debug.LogError("SetType Error");
	            return -1;
	        }

	        return node.Type;
	    }

        public void SetNodeSubType(int col, int row, int value) {
			CAssetNode node = GetNode(col, row);
			if (node == null) {
				Debug.LogError("SetAsset Error");
				return;
			}

			node.SubType = value;
		}

	    public int GetNodeSubType(int col, int row)
	    {
	        CAssetNode node = GetNode(col, row);
	        if (node.Invalid)
	        {
	            Debug.LogError("SetAsset Error");
	            return -1;
	        }

	        return node.SubType;
	    }

	    public void FillData(int col, int row, int type, int subType, bool walkable)
	    {
	        CAssetNode node = GetNode(col, row);
	        if (node.Invalid)
	        {
	            Debug.LogError("Set Value and Asset Error");
	            return;
	        }

	        node.Type = type;
            node.SubType = subType;
			node.Walkable = walkable;
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

	    public bool Invalid
	    {
	        get { return Col < 0 || Row < 0; }
	    }

	    /// <summary>
        /// 本格子的类型
        /// </summary>
        public int Type;

		/// <summary>
		/// 格子的次级类型
		/// </summary>
		public int SubType;

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

		public CAssetNode()
		{
			Col = -1;
			Row = -1;
			Walkable = false;
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
