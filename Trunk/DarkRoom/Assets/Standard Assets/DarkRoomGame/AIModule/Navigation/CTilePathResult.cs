using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Core;

namespace DarkRoom.AI
{
	/// <summary>
	/// 用于跟随路径行走的数据
	/// </summary>
	public class CTilePathResult
	{
		/// <summary>
		/// 开始坐标
		/// </summary>
		public Vector2Int StartPos;

		/// <summary>
		/// 结束坐标
		/// </summary>
		public Vector2Int EndPos;

		/// <summary>
		/// 跟随开始的节点
		/// </summary>
		public int StartIndex;

		/// <summary>
		/// 跟随结束的节点
		/// </summary>
		public int EndIndex;

		/// <summary>
		/// 路径节点的所有数据
		/// </summary>
		public List<Vector2Int> WayPoints;

		/// <summary>
		/// 随身携带的数据
		/// </summary>
		public System.Object Data;

		public CTilePathResult()
		{
		}

		/// <summary>
		/// 重置数据以待后用
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void Reset(Vector2Int start, Vector2Int end)
		{
			StartPos = start;
			EndPos = end;
			StartIndex = -1;
			EndIndex = -1;
			WayPoints = null;
		}

		public Vector2Int FirstWayPoint
		{
			get { return WayPoints[StartIndex]; }
		}

		public int Count
		{
			get { return EndIndex - StartIndex + 1; }
		}

		public void SetWayPoints(List<Vector2Int> value)
		{
			WayPoints = value;
			StartIndex = 0;
			EndIndex = value.Count - 1;
		}

		public void SetWayPoints(List<Vector2Int> value, int start, int end)
		{
			WayPoints = value;
			StartIndex = start;
			EndIndex = end;
		}

		public void Draw()
		{
			if (WayPoints == null)return;
			string str = string.Empty;
			for (int i = 1; i < WayPoints.Count; i++) {
				var start = WayPoints[i - 1];
				str += start.ToString() + " : ";
				var end = WayPoints[i];
				Debug.DrawLine(start.GetVector3(), end.GetVector3());
			}
			str += WayPoints[WayPoints.Count - 1].ToString() + " : ";
			Debug.Log(str);
		}

		/// <summary>
		/// 清理本数据. 干掉引用
		/// </summary>
		public void Clear()
		{
			WayPoints = null;
		}
	}
}