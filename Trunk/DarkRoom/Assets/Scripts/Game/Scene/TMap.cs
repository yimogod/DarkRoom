using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword {
	public class TMap : CSingleton<TMap> {

		//不可通过的地图数据, 包含山脉, 沼泽, 树, 小桌子等导致的不可通过
		public CStarGrid WalkableGrid => CTileNavigationSystem.Instance.WalkableGrid;

		public TileTerrainLayerComp Terrain = null;

		private MapMeta m_mapMeta;

		public MapMeta Meta => m_mapMeta;

		public void Init(MapMeta meta) {
			m_mapMeta = meta;
			CTileNavigationSystem.Instance.Initialize(meta.Cols, meta.Rows);
		}

		/*格子是否可以通行*/
		public bool Walkable(int col, int row) {
			return WalkableGrid.IsWalkable(col, row);
		}

		//寻找四周一圈的可通行的位置
		//如果自己可以通行, 那么就返回自己
		public Vector2Int FindSurroundWalkablePos(Vector2Int pos) {
			var node = WalkableGrid.GetNode(pos);
			if (node != null && node.Walkable) return pos;

			int row = pos.y;
			int col = pos.x;
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					node = WalkableGrid.GetNode(col + j, row + 1);
					if (node != null && node.Walkable) return new Vector2Int(node.Col, node.Row);
				}
			}

			return CDarkConst.INVALID_VEC2INT;
		}

		/*获取所在位置最近的可通行图*/
		public Vector2Int FindNearestWalkablePos(Vector2Int pos) {
			return WalkableGrid.FindNearestWalkablePos(pos);
		}

		/*地图中随机一个位置*/
		public Vector2Int FindRandomNodeLocation() {
			return CMapUtil.FindRandomNodeLocation(m_mapMeta.Rows, m_mapMeta.Cols);
		}

		//切换场景调用这个函数
		public void Clean() {
		}
	}
}