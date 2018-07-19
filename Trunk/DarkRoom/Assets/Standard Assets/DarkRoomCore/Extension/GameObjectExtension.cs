using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{
    public static class GameObjectExtension
    {
        /** 传入的index是否合法 */
        public static T GetOrCreateComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null) comp = go.AddComponent<T>();
            return comp;
        }
    }
}
