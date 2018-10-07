using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Game;
using DarkRoom.PCG;
using UnityEngine;

namespace Sword
{
	/// <summary>
	/// 单位的分布, 其实就是产生位置. 组件单位在ActorLayerComp中做
	/// </summary>
	public class ActorGenerator
	{
		private ActorGeneraterPlaceholder m_tilesData;

		//存储生成怪物的信息
		private CAssetGrid m_grid = new CAssetGrid();

		public CAssetGrid Grid => m_grid;
		public Vector2Int HeroBornPos;

		public void Generate(MapMeta meta, CStarGrid walkableGrid)
		{
			m_tilesData = new ActorGeneraterPlaceholder(walkableGrid);
			GenerateActor(meta);
		}

		//创建单位, 英雄和怪物
		private void GenerateActor(MapMeta meta)
		{
			//我们固定英雄的位置
			HeroBornPos = m_tilesData.AddUnitToDict(Vector2Int.one * 5, 5);

			//2. 创建怪物
			foreach (var m in meta.Monsters)
			{
				for (int i = 0; i < m.z; i++)
				{
					var pos = m_tilesData.FindFreeTileForMonster();
					pos = m_tilesData.AddUnitToDict(pos, 2);
					//type = id, subtype = level
					m_grid.FillData(pos.x, pos.y, m.x, m.y, false);
				}
			}


			//TODO 创建boss, boss创建位置应该是精心调整的算法
		}
	}
}
