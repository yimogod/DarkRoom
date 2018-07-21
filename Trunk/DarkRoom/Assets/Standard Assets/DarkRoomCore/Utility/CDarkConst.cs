using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{
    public class CDarkConst
    {
        //不合法的vect
        public static Vector3 INVALID_VEC3 = new Vector3(-100000f, -100000f, -100000f);
        public static Vector2 INVALID_VEC2 = new Vector2(-100000f, -100000f);

        public static Vector3Int INVALID_VEC3INT = new Vector3Int(-100000, -100000, -100000);
        public static Vector2Int INVALID_VEC2INT = new Vector2Int(-100000, -100000);
    }
}
