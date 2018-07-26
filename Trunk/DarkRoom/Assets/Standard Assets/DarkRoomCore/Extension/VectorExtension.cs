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
    }
}