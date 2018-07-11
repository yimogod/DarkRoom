using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword {
	public class TMap {
		private static TMap _ins;

		//不可通过的地图数据, 包含山脉, 沼泽, 树, 小桌子等导致的不可通过
		public CMapGrid<CStarNode> walkGrid = new CMapGrid<CStarNode>();
		//从地图生成类中获取/或者在预制地图中获取, 目前用于高亮英雄
		public CAssetNode typeGrid = null;

		public TerrainComp terrain = null;

		private MapMeta _mapMeta;

		private TMap() { }

		public static TMap Instance {
			get {
				if (_ins == null) _ins = new TMap();
				return _ins;
			}
		}

		public MapMeta meta {
			get { return _mapMeta; }
		}

		public void Init(MapMeta meta) {
			_mapMeta = meta;
			walkGrid.Init(meta.cols, meta.rows);
		}

		/*格子是否可以通行*/
		public bool walkable(Vector3 tile) {
			return walkGrid.IsWalkable((int)tile.z, (int)tile.x);
		}

		public void SetWalkable(Vector3 tile, bool value) {
			walkGrid.SetWalkable((int)tile.z, (int)tile.x, value);
		}

		//寻找四周一圈的可通行的位置
		//如果自己可以通行, 那么就返回自己
		public Vector3 FindSurroundWalkablePos(Vector3 pos) {
			int row = (int)pos.z;
			int col = (int)pos.x;
			var node = walkGrid.GetNode(row, col);
			if (node != null && node.Walkable) return pos;

			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					node = walkGrid.GetNode(row + 1, col + j);
					//if (node != null && node.Walkable) return node.vector;
				}
			}

			return CDarkConst.INVALID_VEC3;
		}

		/*获取所在位置最近的可通行图*/
		public Vector3 FindNearestWalkablePos(Vector3 pos) {
			return walkGrid.FindNearestWalkablePos(pos);
		}

		/*地图中随机一个位置*/
		public Vector3 FindRandomNodeLocation() {
			return CMapUtil.FindRandomNodeLocation(_mapMeta.rows, _mapMeta.cols);
		}

		//切换场景调用这个函数
		public void Clean() {
		}
	}
}