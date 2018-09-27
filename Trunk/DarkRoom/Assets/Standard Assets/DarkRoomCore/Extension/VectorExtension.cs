using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{
    public static class VectorExtension
    {
        /** 传入的index是否合法 */
        public static int ManhattanMagnitude(this Vector2Int me, Vector2Int inVector)
        {
            return Math.Abs(me.x - inVector.x) + Math.Abs(me.y - inVector.y);
        }

		public static Vector3 GetVector3(this Vector2Int me)
		{
			return new Vector3(me.x, 0, me.y);
		}

		public static Vector2Int GetVector2Int(this Vector3 me)
		{
			return new Vector2Int((int)me.x, (int)me.z);
		}
	}
}