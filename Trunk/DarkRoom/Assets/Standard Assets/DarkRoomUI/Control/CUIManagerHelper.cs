using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DarkRoom.UI
{
    public class CUIManagerHelper
    {
        public static bool IsExitsInCache(UIWindowBase window, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (window == null) return false;

            if (!dict.ContainsKey(window.name)) return false;
            return dict[window.name].Contains(window);
        }

        /// <summary>
        /// 获取一个隐藏的UI,如果有多个同名UI，则返回最后创建的那一个
        /// </summary>
        /// <param name="winName">UI名</param>
        /// <returns></returns>
        public static UIWindowBase GetUIFromCache(string winName, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (!dict.ContainsKey(winName)) return null;
            int count = dict[winName].Count;
            if (count == 0) return null;

            UIWindowBase ui = dict[winName][count - 1];
            //默认返回最后创建的那一个
            return ui;
        }

        public static void AddUIToCache(UIWindowBase window, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (window == null) return;

            if (!dict.ContainsKey(window.name))
            {
                dict.Add(window.name, new List<UIWindowBase>());
            }

            dict[window.name].Add(window);
        }

        /// <summary>
        /// 从cache中删除ui, 如果不在列表里面, 就返回
        /// </summary>
        public static void RemoveUIFromCache(UIWindowBase window, Dictionary<string, List<UIWindowBase>> dict)
        {
            bool b = IsExitsInCache(window, dict);
            if (!b) return;

            if (window == null)
            {
                throw new Exception("UIManager: RemoveUI error l_UI is null: !");
            }

            if (!dict.ContainsKey(window.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + window.name + "  " + window);
            }

            if (!dict[window.name].Contains(window))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + window.name + "  " + window);
            }

            dict[window.name].Remove(window);
        }

        /// <summary>
        /// 清空列表里面所有的ui
        /// </summary>
        public static void DestroyUIInCache(Dictionary<string, List<UIWindowBase>> dict)
        {
            foreach (List<UIWindowBase> uis in dict.Values)
            {
                for (int i = 0; i < uis.Count; i++)
                {
                    try
                    {
                        uis[i].DestroyUI();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("OnDestroy :" + e.ToString());
                    }

                    GameObject.Destroy(uis[i].gameObject);
                }
            }

            dict.Clear();
        }

        /// <summary>
        /// 获取view所在列表的索引用来当做id
        /// </summary>
        public static int GetUIIDFromCache(string winName, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (!dict.ContainsKey(winName))return 0;
            var list = dict[winName];
            int id = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UIID != id)continue;
                id++;
                i = 0;
            }

            return id;
        }
    }
}
