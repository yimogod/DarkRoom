using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.PCG {
    /// <summary>
    /// terrain用到的asset的索引
    /// </summary>
    public enum ForestTerrainAssetIndex
    {
        Grass1 = 0,
        Grass2,
        Land1,
        Land2,
        Wall,
        Pond,
        Road
    }

    /// <summary>
    /// terrain用到的tile的类型
    /// 需要注意的是目前这些类型和可通行性并没有关系. 看未来我们是否要用规则写死
    /// </summary>
    public enum ForestTerrainType
    {
        None,
        Floor,
        Wall,
        Pond,
        Road
    }

    public class CForestTerrain {
		
	}

    public struct CForestRoom
    {
        public Vector2Int Size;
        /// <summary>
        /// 是否有推荐位置
        /// </summary>
        public Vector2Int PreferLocation;

        public string Name;

        /// <summary>
        /// 是否唯一的
        /// </summary>
        public bool Unique;

        /// <summary>
        /// 是否有边界?
        /// </summary>
        public bool Border;

        /// <summary>
        /// 是否有隧道
        /// </summary>
        public bool NoTunnels;

        /// <summary>
        /// 是否特殊?
        /// </summary>
        public bool Special;

        public int[] Spots;

        public int[] Exits;
    }


    /// <summary>
    /// 用于森林地形的房子
    /// 我们默认的房子都是坐北朝南, 即门口都朝下
    /// </summary>
    public class CForestRoom1
    {
        public const int MIN_COL = 10;
        public const int MIN_ROW = 10;

        public int row;
        public int col;
        public int width; // on z
        public int length; // on x
        public string index = "01";

        public int posId
        {
            get { return row * 10000 + col; }
        }

        public int doorCol
        {
            get { return col + (int)(width * 0.5f); }
        }

        public int doorRow
        {
            get { return row - 1; }
        }

        public string prefab
        {
            get { return string.Format("Preb_Room_{0}x{1}_{2}", length, width, index); }
        }

        //只有正常的terrain上可以放置房子, 路, 池塘都不可以
        public Vector2 FindLocationInMap(CAssetGrid typeGrid, int startCol, int startRow)
        {
            //从起始点在四个方向上各个寻找10个单位

            //右方向
            int endCol = startCol + 10;
            int endRow = startRow;
            int col = startCol;
            int row = startRow;
            for (col = startCol; col < endCol; ++col)
            {
                if (col < MIN_COL) continue;
                bool valid = IsLocationValid(col, col + length, row, row + width, typeGrid);
                if (valid) return new Vector2(col, row);
            }

            //左方向
            endCol = startCol - 10;
            endRow = startRow;
            col = startCol;
            row = startRow;
            for (col = startCol; col > endCol; --col)
            {
                if (col < MIN_COL) continue;
                bool valid = IsLocationValid(col, col + length, row, row + width, typeGrid);
                if (valid) return new Vector2(col, row);
            }

            //上方向
            endCol = startCol;
            endRow = startRow + 10;
            col = startCol;
            row = startRow;
            for (row = startRow; row < endRow; ++row)
            {
                if (row < MIN_ROW) continue;
                bool valid = IsLocationValid(col, col + length, row, row + width, typeGrid);
                if (valid) return new Vector2(col, row);
            }


            //下方向
            endCol = startCol;
            endRow = startRow - 10;
            col = startCol;
            row = startRow;
            for (row = startRow; row > endRow; --row)
            {
                if (row < MIN_ROW) continue;
                bool valid = IsLocationValid(col, col + length, row, row + width, typeGrid);
                if (valid) return new Vector2(col, row);
            }

            //意味着找不到
            return Vector2.one;
        }

        private bool IsLocationValid(int startCol, int endCol, int startRow, int endRow, CAssetGrid typeGrid)
        {
            for (int row = startRow; row < endRow; ++row)
            {
                for (int col = startCol; col < endCol; ++col)
                {
                    //CStarNode node = typeGrid.GetNode(row, col);
                    //if (node == null) return false;

                    //bool special = typeGrid.IsSpecial(row, col);
                    //if (special) return false;
                }
            }

            return true;
        }

        public static CForestRoom1 Create(int minLength, int maxLength, int minWidth, int maxWidth)
        {
            //3, 4, 5
            int width = CDarkRandom.Next(minWidth, maxWidth);
            //6,7,8
            int length = CDarkRandom.Next(minLength, maxLength);

            //int rotation = Rand.Next(-1, 3);

            CForestRoom1 room = new CForestRoom1();
            room.width = width;
            room.length = length;

            //房子来两种就够了
            float value = CDarkRandom.Next();
            if (value > 0.5f) room.index = "02";

            return room;
        }
    }
}
