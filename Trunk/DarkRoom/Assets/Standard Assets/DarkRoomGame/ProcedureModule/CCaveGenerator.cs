using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	[RequireComponent(typeof(CaveTerrainGenerator))]
	public class CaveGenerator : CTileMapGeneratorBase {

	    /// <summary>
	    /// 存储的asset列表
	    /// 0代表可通行区域代表的资源
	    /// 1, 2代表不可通行区域的两个资源
	    /// </summary>
        public List<string> AssetList;

		private CaveTerrainGenerator m_terrain;

		void Awake()
		{
			m_terrain = gameObject.GetComponent<CaveTerrainGenerator>();
		}

		public override void Generate()
		{
			base.Generate();
			m_terrain.Generate(m_numCols, m_numRows);

		    CCellularGrid cellular = m_terrain.Map;
			for (int x = 0; x < m_numCols; x++) {
				for (int z = 0; z < m_numRows; z++) {

				    bool walk = cellular[x, z] == 0;
					m_grid.SetWalkable(x, z, walk);
				    int type = GetTypeByAlive(walk);
                    string asset = AssetList[type];
                    m_grid.SetTypeAndAsset(x, z, type, asset);
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
