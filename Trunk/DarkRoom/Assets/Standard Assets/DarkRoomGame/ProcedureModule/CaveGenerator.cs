using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	[RequireComponent(typeof(CaveTerrainGenerator))]
	public class CaveGenerator : CTileMapGeneratorBase {
		public List<string> AssetList;

		private CaveTerrainGenerator m_terrain;

		void Awake()
		{
			m_terrain = gameObject.GetComponent<CaveTerrainGenerator>();
		}

		public override void Generate()
		{
			base.Generate();
			m_terrain.Generate(Width, Height);


			CCave.CellularMap cellular = m_terrain.Map;
			for (int x = 0; x < Width; x++) {
				for (int z = 0; z < Height; z++) {
					//m_grid.SetType(x, z, CAssetNode.TileType.TERRIAN);

					bool walk = cellular[x, z] == 0;
					m_grid.SetWalkable(x, z, walk); //注意, 我们这里认为, 死亡的才是可同行的, 原因是这样图形好看
					m_grid.SetAsset(x, z, GetSpriteAtHeight(walk));
                }
			}
		}

		private string GetSpriteAtHeight(bool walk) {
			if (walk)return AssetList[0];

			if(CDarkRandom.SmallerThan(0.5f))
				return AssetList[1];
			return AssetList[2];
		}
	}
}
