using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.AI
{
	public interface IPathPlanner
	{
		/// <summary>
		/// 同步寻路
		/// </summary>
		List<Vector2Int> FindPath(Vector2Int start, Vector2Int end);

		/// <summary>
		/// 异步寻路
		/// </summary>
		void FindPathAsyn(CPathResult result);

		void SetWalkable(int col, int row, bool walkable);
	}
}