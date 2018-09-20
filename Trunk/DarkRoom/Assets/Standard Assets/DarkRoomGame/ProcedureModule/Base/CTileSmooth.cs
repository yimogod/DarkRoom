using System;
using DarkRoom.Game;

namespace DarkRoom.PCG {
	/// <summary>
	/// 平滑地图tile元素, 利用战争迷雾算法. 
	/// TODO 如果为了添加更多方法再重构
	/// </summary>
	public class CTileSmooth
	{
		public void SmoothTileMapSprite(CAssetGrid tileMap){

			//先将corner值都设置为0
			for (int x = 0; x < tileMap.NumCols; x++)
			{
				for (int z = 0; x < tileMap.NumRows; z++)
				{
					CAssetNode tile = tileMap.GetNode(x, z);
					tile.ResetCornerValue();
				}
			}

			//再重新计算
			for (int x = 0; x < tileMap.NumCols; x++)
			{
				for (int z = 0; x < tileMap.NumRows; z++)
				{
					SmoothSingleTileSprite(tileMap, x, z);
				}
			}
		}

		//根据四周情况, 平滑单个的tile
		private void SmoothSingleTileSprite(CAssetGrid map, int x, int z){
			SetTileCornerValue(map, x, z, CAssetNode.Corner.BottomRight, 4);
			SetTileCornerValue(map, x, z - 1, CAssetNode.Corner.TopRight, 1);
			SetTileCornerValue(map, x + 1, z, CAssetNode.Corner.BottomLeft, 8);
			SetTileCornerValue(map, x + 1, z - 1, CAssetNode.Corner.TopLeft, 2);
		}

		private void SetTileCornerValue(CAssetGrid map, int x, int z, CAssetNode.Corner corner, int value){
			CAssetNode tile = map.GetNode(x, z);
			if (tile == null)return;

			int fv = tile.GetFrogCornerValue(corner);
			if (fv == value)return;

			tile.SetFrogCornerValue(corner, fv + value);
		}
	}
}

