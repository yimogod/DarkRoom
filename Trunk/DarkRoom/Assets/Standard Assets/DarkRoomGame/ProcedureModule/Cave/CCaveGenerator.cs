using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	[RequireComponent(typeof(CaveTerrainGenerator))]
	public class CaveGenerator : CTileMapGeneratorBase {
		private CaveTerrainGenerator m_terrain;

		void Awake()
		{
		    m_maxAssetsNum = 2;
			//m_terrain = gameObject.GetComponent<CaveTerrainGenerator>();
		}

		public override void Generate()
		{
			base.Generate();
			m_terrain.Generate(m_numCols, m_numRows);

		    CCellularGrid cellular = m_terrain.Map;
			for (int x = 0; x < m_numCols; x++) {
				for (int z = 0; z < m_numRows; z++) {

				    bool walk = cellular[x, z] == 0;
				    int type = GetTypeByAlive(walk);
                    var asset = m_assetList[type];
                    m_grid.FillData(x, z, type, asset, walk);
                }
			}
		}

        /// <summary>
        /// walk = true mean alive
        /// </summary>
		private int GetTypeByAlive(bool alive) {
			if (alive)return 0;

			if(CDarkRandom.SmallerThan(0.5f))return 1;
			return 2;
		}
	}
}
