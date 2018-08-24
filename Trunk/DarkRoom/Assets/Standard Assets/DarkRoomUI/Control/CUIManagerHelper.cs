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
        public static bool IsExitsInCache(UIWindowBase view, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (view == null) return false;

            if (!dict.ContainsKey(view.name)) return false;
            return dict[view.name].Contains(view);
        }

        /// <summary>
        /// 获取一个隐藏的UI,如果有多个同名UI，则返回最后创建的那一个
        /// </summary>
        /// <param name="viewName">UI名</param>
        /// <returns></returns>
        public static UIWindowBase GetUIFromCache(string viewName, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (!dict.ContainsKey(viewName)) return null;
            int count = dict[viewName].Count;
            if (count == 0) return null;

            UIWindowBase ui = dict[viewName][count - 1];
            //默认返回最后创建的那一个
            return ui;
        }

        public static void AddUIToCache(UIWindowBase UI, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (UI == null) return;

            if (!dict.ContainsKey(UI.name))
            {
                dict.Add(UI.name, new List<UIWindowBase>());
            }

            dict[UI.name].Add(UI);
        }

        /// <summary>
        /// 从cache中删除ui, 如果不在列表里面, 就返回
        /// </summary>
        public static void RemoveUIFromCache(UIWindowBase view, Dictionary<string, List<UIWindowBase>> dict)
        {
            bool b = IsExitsInCache(view, dict);
            if (!b) return;

            if (view == null)
            {
                throw new Exception("UIManager: RemoveUI error l_UI is null: !");
            }

            if (!dict.ContainsKey(view.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + view.name + "  " + view);
            }

            if (!dict[view.name].Contains(view))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + view.name + "  " + view);
            }

            dict[view.name].Remove(view);
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
        public static int GetUIIDFromCache(string viewName, Dictionary<string, List<UIWindowBase>> dict)
        {
            if (!dict.ContainsKey(viewName))return 0;
            var list = dict[viewName];
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
