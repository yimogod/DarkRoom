using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	[RequireComponent(typeof(CPlainTerrainGenerator))]
	public class CPlainGenerator : CTileMapGeneratorBase {
		public float SeaLevel = 0.2f;
		public float BeachHeight = 0.22f;
		public float GrassHeight = 0.3f;
		public float GrassHeight2 = 0.35f;
		public float LandHeight = 0.52f;
		public float LandHeight2 = 0.8f;
		public float StoneHeight = 1f;

		public List<string> AssetList;

		private CPlainTerrainGenerator m_terrain;

		void Awake()
		{
			m_terrain = gameObject.GetComponent<CPlainTerrainGenerator>();
		}

		private string GetSpriteAtHeight(float height) {
			if (height < SeaLevel){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[0];
				return AssetList[1];
			}


			if (height <= BeachHeight){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[2];
				return AssetList[3];
			}

			if (height <= GrassHeight){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[4];
				return AssetList[5];
			}

			if (height <= GrassHeight2)
				return AssetList[6];

			if (height <= LandHeight)
				return AssetList[7];
			if (height <= LandHeight2)
				return AssetList[8];

			if (height <= StoneHeight){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[9];
				return AssetList[10];
			}

			return AssetList[4];
		}

		public override void Generate()
		{
			base.Generate();
			m_terrain.Generate(Width, Height);

			CPlain.PerlinMap perlin = m_terrain.Map;
			for (int x = 0; x < Width; x++) {
				for (int z = 0; z < Height; z++) {
					string asset = GetSpriteAtHeight(perlin[x, z]);
					m_grid.SetType(x, z, Tile.TileType.TERRIAN);
					m_grid.SetAsset(x, z, asset);
                }
			}
		}
	}
}
