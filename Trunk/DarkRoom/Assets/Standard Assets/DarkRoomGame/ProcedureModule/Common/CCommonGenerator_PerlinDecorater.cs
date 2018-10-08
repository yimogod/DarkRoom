using System;
using DarkRoom.Core;
using DarkRoom.Game;

namespace Rayman{
	//我们假定所有的石头啊, 装饰物啊最多有5种
	public class DecoGenerator{
		private int _numCols;
		private int _numRows;

		public DecoGenerator(int cols, int rows){
			_numCols = cols;
			_numRows = rows;
		}

		//路标, 大块石头, 路灯等
		public void CreateDecoration(int decoBlockNum, CAssetGrid walkGrid, CAssetGrid typeGrid, CAssetGrid assetGrid){
			string name;

			//添加阻碍装饰物
			for(int i = 0; i < decoBlockNum; ++i) {
				int col = CDarkRandom.Next(_numCols);
				int row = CDarkRandom.Next(_numRows);
				if(!walkGrid.IsWalkable(col, row))continue;

				walkGrid.SetWalkable(col, row, false);

				int nameIndex = CDarkRandom.Next(1, 6);
				name = string.Format("Preb_Deco_Block_00{0}", nameIndex);
				//assetGrid.SetName(row, col, name);
			}
		}

		public void CreateDecal(string decalNameRoot, int decalStartIndex, CAssetGrid walkGrid, CAssetGrid typeGrid, CAssetGrid assetGrid){
			string name;

			//添加贴花
			for(int i = 0; i < 30; ++i) {
				int col = CDarkRandom.Next(_numCols);
				int row = CDarkRandom.Next(_numRows);
				//if(typeGrid.IsSpecial(row, col))continue;
				if(!walkGrid.IsWalkable(row, col))continue;

				//typeGrid.SetType(row, col, (int)RayConst.TileType.DECAL);

				int nameIndex = CDarkRandom.Next(decalStartIndex, decalStartIndex + 5);
				name = string.Format("{0}{1}", decalNameRoot, nameIndex);
				//assetGrid.SetName(row, col, name);
			}
		}

		//创建可以销毁的障碍物. 比如说坦克大战和炸弹人的地块
		public void CreateDestroyBlock(int decoDestroyNum, CAssetGrid walkGrid, CAssetGrid typeGrid, CAssetGrid assetGrid){
			string name;

			//添加阻碍装饰物
			for(int i = 0; i < decoDestroyNum; ++i) {
				int col = CDarkRandom.Next(_numCols);
				int row = CDarkRandom.Next(_numRows);
				//if(typeGrid.IsSpecial(row, col))continue;
				if(!walkGrid.IsWalkable(row, col))continue;

				//typeGrid.SetType(row, col, (int)RayConst.TileType.DECO_DESTROY);
				walkGrid.SetWalkable(row, col, false);

				//5个里面随机一个
				int nameIndex = CDarkRandom.Next(1, 6);
				name = string.Format("Preb_Deco_Destroy_00{0}", nameIndex);
				//assetGrid.SetName(row, col, name);
			}
		}
	}
}