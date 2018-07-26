using DarkRoom.AI;
using DarkRoom.Core;
using UnityEngine;
namespace DarkRoom.Game{
	public class CMapUtil
	{
		/*地图中随机一个位置*/
		public static Vector2Int FindRandomNodeLocation(int maxCol, int maxRow)
		{
			int row = CDarkRandom.Next(0, maxRow);
			int col = CDarkRandom.Next(0, maxCol);

			return new Vector2Int(col, row);
		}

		/* for 2d, 左下角原点 */
		public static Vector2 GetTileCenterPosByColRow(int col, int row)
		{
			float x = (col + 0.5f);
			float y = (row + 0.5f);
			return new Vector2(x, y);
		}

		//方块的左下角坐标
		public static Vector2 GetTileLeftBottomPosByColRow(int col, int row)
		{
			Vector2 vec = new Vector2(col, row);
			return vec;
		}

		public static Vector2Int GetTileByPos(float x, float z)
		{
		    Vector2Int vec = new Vector2Int((int)x, (int)z);
			return vec;
		}

		public static void DrawGrid(CStarGrid gird)
		{
			var parent = new GameObject("map cube");
			var t = parent.transform;
			t.localPosition = Vector3.zero;

			for (int row = gird.NumRows - 1; row >= 0; row--) {
				for (int col = 0; col < gird.NumCols; col++) {
					bool node = gird.IsWalkable(col, row);
					if (!node)continue;

					var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
					go.transform.localPosition = new Vector3(col, 0, row);
					go.transform.SetParent(t, true);
                }
			}
		}

		public static void PrintGird(CStarGrid gird)
		{
			string str = "";
			for (int row = gird.NumRows - 1; row >= 0 ; row--) {
				for (int col = 0; col < gird.NumCols; col++) {
					bool node = gird.IsWalkable(col, row);
					int v = node ? 1 : 0;
					str = string.Format("{0},{1}", str, v);
				}

				str += "\n";
			}

			Debug.Log(str);
		}

		public static void PrintGird<T>(T[,] gird)
		{
			int numCols = gird.GetLength(0);
			int numRows = gird.GetLength(1);

			string str = "";
			for (int row = numRows - 1; row >= 0 ; row--) {
				for (int col = 0; col < numCols; col++) {
					T v = gird[col, row];
					str = string.Format("{0},{1}", str, v);
				}

				str += "\n";
			}

			Debug.Log(str);
		}
	}
}