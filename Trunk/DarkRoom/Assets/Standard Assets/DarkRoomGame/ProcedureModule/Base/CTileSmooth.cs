using System;

namespace DarkRoom.PCG {
	/// <summary>
	/// 平滑地图tile元素, 利用战争迷雾算法. 
	/// TODO 如果为了添加更多方法再重构
	/// </summary>
	public class CTileSmooth
	{
		public void SmoothTileMapSprite(CProcedureMap tileMap){
			for (int x = 0; x < tileMap.numCols; x++)
			{
				for (int z = 0; x < tileMap.numRows; z++)
				{
					SmoothSingleTileSprite(tileMap, x, z);
				}
			}
		}

		//根据四周情况, 平滑单个的tile
		private void SmoothSingleTileSprite(CProcedureMap map, int x, int z){
			SetTileCornerValue(map, x, z, Tile.Corner.BottomRight, 4);
			SetTileCornerValue(map, x, z - 1, Tile.Corner.TopRight, 1);
			SetTileCornerValue(map, x + 1, z, Tile.Corner.BottomLeft, 8);
			SetTileCornerValue(map, x + 1, z - 1, Tile.Corner.TopLeft, 2);
		}

		private void SetTileCornerValue(CProcedureMap map, int x, int z, Tile.Corner corner, int value){
			Tile tile = map.GetTile(x, z);
			if (tile == null)return;

			int fv = tile.GetFrogCornerValue(corner);
			if (fv == value)return;

			tile.SetFrogCornerValue(corner, fv + value);
		}
	}
}

