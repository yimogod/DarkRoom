using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.AI {
	/// <summary>
	/// 平滑寻路路径的方法
	/// </summary>
	public class CPathFunnel {
		public static List<Vector3> FunnelPath(List<Vector3> path)
		{
			List<Vector3> temp = new List<Vector3>();
			if (path.Count <= 2) {
				temp.AddRange(path);
				return temp;
			}

			Dictionary<int, bool> removed = new Dictionary<int, bool>();
			for (int i = 0; i < path.Count; i++) {
				removed[i] = false;
			}

            for (int i = 0; i < path.Count - 1; i++) {
				if (removed[i])continue;

				Vector3 prev = path[i];
				for (int j = i + 1; j < path.Count - 1; j++) {
					if (removed[j]) continue;

					Vector3 now = path[j];
					Vector3 next = path[j + 1];

					//横着一条线
					if (CMathUtil.FloatEqual(prev.x, now.x) &&
					    CMathUtil.FloatEqual(next.x, now.x)) {
						removed[j] = true;
						continue;
					}

					//竖着一条线
					if (CMathUtil.FloatEqual(prev.z, now.z) &&
						CMathUtil.FloatEqual(next.z, now.z)) {
						removed[j] = true;
						continue;
					}

					//斜着一条线
					float dx1 = now.x - prev.x;
					float dx2 = next.x - now.x;
					float dz1 = now.z - prev.z;
					float dz2 = next.z - now.z;
					if (dx1 * dx2 > 0 &&
						dz1 * dz2 > 0 &&
						CMathUtil.FloatEqual(dz1 / dx1, dz2 / dx2)) {
						removed[j] = true;
						continue;
					}
				}
			}

			

			temp.Clear();
			for (int i = 0; i < path.Count; i++) {
				if (removed[i])continue;
				temp.Add(path[i]);
			}

			return temp;
		}
		
	}
}
