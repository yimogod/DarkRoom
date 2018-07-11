using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword {
	public class TMap : CSingleton<TMap> {

		//不可通过的地图数据, 包含山脉, 沼泽, 树, 小桌子等导致的不可通过
		public CMapGrid<CStarNode> WalkableGrid = new CMapGrid<CStarNode>();

		public TileTerrainComp Terrain = null;

		private MapMeta m_mapMeta;

		public MapMeta Meta {
			get { return m_mapMeta; }
		}

		public void Init(MapMeta meta) {
			m_mapMeta = meta;
			WalkableGrid.Init(meta.Cols, meta.Rows);
		}

		/*格子是否可以通行*/
		public bool walkable(Vector3 tile) {
			return WalkableGrid.IsWalkable((int)tile.z, (int)tile.x);
		}

		public void SetWalkable(Vector3 tile, bool value) {
			WalkableGrid.SetWalkable((int)tile.z, (int)tile.x, value);
		}

		//寻找四周一圈的可通行的位置
		//如果自己可以通行, 那么就返回自己
		public Vector3 FindSurroundWalkablePos(Vector3 pos) {
			int row = (int)pos.z;
			int col = (int)pos.x;
			var node = WalkableGrid.GetNode(row, col);
			if (node != null && node.Walkable) return pos;

			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					node = WalkableGrid.GetNode(row + 1, col + j);
					//if (node != null && node.Walkable) return node.vector;
				}
			}

			return CDarkConst.INVALID_VEC3;
		}

		/*获取所在位置最近的可通行图*/
		public Vector3 FindNearestWalkablePos(Vector3 pos) {
			return WalkableGrid.FindNearestWalkablePos(pos);
		}

		/*地图中随机一个位置*/
		public Vector3 FindRandomNodeLocation() {
			return CMapUtil.FindRandomNodeLocation(m_mapMeta.Rows, m_mapMeta.Cols);
		}

		//切换场景调用这个函数
		public void Clean() {
		}
	}
}