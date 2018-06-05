using System;
using System.Collections.Generic;

namespace DarkRoom.Core
{
    public static class ListExtension
    {
        /** 传入的index是否合法 */
        public static bool IsValidIndex<T>(this IList<T> list, int index)
        {
            if (index < 0) return false;
            if (index >= list.Count) return false;
            return true;
        }

        /** 将list作为set使用 */
        public static int AddUnique<T>(this IList<T> list, T item)
        {
            int index = list.IndexOf(item);
            if (index == -1)
            {
                index = list.Count;
                list.Add(item);
            }

            return index;
        }
        
    }
}
