using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{
    public static class TransformExtension
    {
        /** 传入的index是否合法 */
        public static Transform GetOrCreateChild(this Transform parent, string Child)
        {
            var layer = parent.Find(Child);
            if (layer == null)
            {
                layer = new GameObject(Child).transform;
                layer.SetParent(parent);
                layer.localPosition = Vector3.zero;
            }

            return layer;
        }
    }
}
