using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{
    public static class ComponentExtension
    {
        /** 传入的index是否合法 */
        public static T GetOrCreateComponentOnGameObject<T>(this Component comp) where T : Component
        {
            var go = comp.gameObject;
            return go.GetOrCreateComponent<T>();
        }
    }
}
