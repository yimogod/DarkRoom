using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using DarkRoom.Core;

namespace DarkRoom.UI
{
    [RequireComponent(typeof(CUIStackManager))]
    [RequireComponent(typeof(CUILayerManager))]
    [RequireComponent(typeof(CUIAnimManager))]
    public class CUIManager : CSingletonMono<CUIManager>
    {
        /// <summary>
        /// 所有ui挂在UIRoot下面
        /// </summary>
        public GameObject UIRoot => gameObject;

        /// <summary>
        /// UI层级管理器
        /// </summary>
        public CUILayerManager LayerManager;

        /// <summary>
        /// UI动画管理器
        /// </summary>
        public CUIAnimManager AnimManager;

        /// <summary>
        /// UI栈管理器
        /// </summary>
        public CUIStackManager StackManager;

        /// <summary>
        /// 当前打开的UI列表
        /// </summary>
        public Dictionary<string, List<UIWindowBase>>
            DisplayUIDict = new Dictionary<string, List<UIWindowBase>>();

        /// <summary>
        /// 当前隐藏的UI列表
        /// </summary>
        public Dictionary<string, List<UIWindowBase>>
            HiddenUIDict = new Dictionary<string, List<UIWindowBase>>();

        public void Initialize()
        {
            LayerManager = GetComponent<CUILayerManager>();
            AnimManager = GetComponent<CUIAnimManager>();
            StackManager = GetComponent<CUIStackManager>();
        }

        /// <summary>
        /// 创建UI,如果不打开则存放在Hide列表中
        /// </summary>
        public T CreateUIWindow<T>() where T : UIWindowBase
        {
            return (T) CreateUIWindow(typeof(T).Name);
        }

        public UIWindowBase CreateUIWindow(string UIName)
        {
            GameObject UItmp = new GameObject(); //GameObjectManager.CreateGameObject(UIName, s_UIManagerGo);
            UIWindowBase UIbase = UItmp.GetComponent<UIWindowBase>();
            try
            {
                UIbase.Init(GetUIID(UIName));
            }
            catch (Exception e)
            {
                Debug.LogError("OnInit Exception: " + e.ToString());
            }

            AddHideUI(UIbase);

            LayerManager.SetLayer(UIbase); //设置层级

            return UIbase;
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="UIName">UI名</param>
        /// <param name="callback">动画播放完毕回调</param>
        /// <param name="objs">回调传参</param>`
        /// <returns>返回打开的UI</returns>
        public UIWindowBase OpenUIWindow(string UIName, UICallBack callback = null, params object[] objs)
        {
            UIWindowBase UIbase = GetHideUI(UIName);

            if (UIbase == null)
            {
                UIbase = CreateUIWindow(UIName);
            }

            RemoveHideUI(UIbase);
            AddUI(UIbase);

            StackManager.OnUIOpen(UIbase);
            LayerManager.SetLayer(UIbase); //设置层级

            try
            {
                UIbase.OnOpen();
            }
            catch (Exception e)
            {
                Debug.LogError(UIName + " OnOpen Exception: " + e.ToString());
            }


            AnimManager.StartEnterAnim(UIbase, callback, objs); //播放动画
            return UIbase;
        }

        public T OpenUIWindow<T>() where T : UIWindowBase
        {
            return (T) OpenUIWindow(typeof(T).Name);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="UI">目标UI</param>
        /// <param name="isPlayAnim">是否播放关闭动画</param>
        /// <param name="callback">动画播放完毕回调</param>
        /// <param name="objs">回调传参</param>
        public void CloseUIWindow(UIWindowBase UI, bool isPlayAnim = true, UICallBack callback = null,
            params object[] objs)
        {
            RemoveUI(UI); //移除UI引用
            LayerManager.RemoveUI(UI);

            if (isPlayAnim)
            {
                //动画播放完毕删除UI
                if (callback != null)
                {
                    callback += CloseUIWindowCallBack;
                }
                else
                {
                    callback = CloseUIWindowCallBack;
                }

                AnimManager.StartExitAnim(UI, callback, objs);
            }
            else
            {
                CloseUIWindowCallBack(UI, objs);
            }
        }

        void CloseUIWindowCallBack(UIWindowBase UI, params object[] objs)
        {
            try
            {
                UI.OnClose();
            }
            catch (Exception e)
            {
                Debug.LogError(UI.UIName + " OnClose Exception: " + e.ToString());
            }

            StackManager.OnUIClose(UI);
            AddHideUI(UI);
        }

        public void CloseUIWindow(string UIname, bool isPlayAnim = true, UICallBack callback = null,
            params object[] objs)
        {
            UIWindowBase ui = GetUI(UIname);

            if (ui == null)
            {
                Debug.LogError("CloseUIWindow Error UI ->" + UIname + "<-  not Exist!");
            }
            else
            {
                CloseUIWindow(GetUI(UIname), isPlayAnim, callback, objs);
            }
        }

        public void CloseUIWindow<T>(bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
            where T : UIWindowBase
        {
            CloseUIWindow(typeof(T).Name, isPlayAnim, callback, objs);
        }

        public UIWindowBase ShowUI(string UIname)
        {
            UIWindowBase ui = GetUI(UIname);
            return ShowUI(ui);
        }

        public UIWindowBase ShowUI(UIWindowBase ui)
        {
            try
            {
                ui.Show();
                ui.OnShow();
            }
            catch (Exception e)
            {
                Debug.LogError(ui.UIName + " OnShow Exception: " + e.ToString());
            }

            return ui;
        }

        public UIWindowBase HideUI(string UIname)
        {
            UIWindowBase ui = GetUI(UIname);
            return HideUI(ui);
        }

        public UIWindowBase HideUI(UIWindowBase ui)
        {
            try
            {
                ui.Hide();
                ui.OnHide();
            }
            catch (Exception e)
            {
                Debug.LogError(ui.UIName + " OnShow Exception: " + e.ToString());
            }

            return ui;
        }

        public void HideOtherUI(string UIName)
        {
            List<string> keys = new List<string>(DisplayUIDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = DisplayUIDict[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].UIName != UIName)
                    {
                        HideUI(list[j]);
                    }
                }
            }
        }

        public void ShowOtherUI(string UIName)
        {
            List<string> keys = new List<string>(DisplayUIDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = DisplayUIDict[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].UIName != UIName)
                    {
                        ShowUI(list[j]);
                    }
                }
            }
        }

        /// <summary>
        /// 移除全部UI
        /// </summary>
        public void CloseAllUI(bool isPlayerAnim = false)
        {
            List<string> keys = new List<string>(DisplayUIDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = DisplayUIDict[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    CloseUIWindow(list[i], isPlayerAnim);
                }
            }
        }

        public void CloseLastUI(UIType uiType = UIType.Normal)
        {
            StackManager.CloseLastUIWindow(uiType);
        }

        public void OpenUIAsync<T>(UICallBack callback, params object[] objs) where T : UIWindowBase
        {
            string UIName = typeof(T).Name;
            OpenUIAsync(UIName, callback, objs);
        }

        public void OpenUIAsync(string UIName, UICallBack callback, params object[] objs)
        {
            /*ResourceManager.LoadAsync(UIName, (loadState, resObject) =>
            {
                if (loadState.isDone)
                {
                    OpenUIWindow(UIName, callback, objs);
                }
            });*/
        }

        public void DestroyUI(UIWindowBase UI)
        {
            Debug.Log("UIManager DestroyUI " + UI.name);

            if (GetIsExitsHide(UI))
            {
                RemoveHideUI(UI);
            }
            else if (GetIsExits(UI))
            {
                RemoveUI(UI);
            }

            try
            {
                UI.DestroyUI();
            }
            catch (Exception e)
            {
                Debug.LogError("OnDestroy :" + e.ToString());
            }

            Destroy(UI.gameObject);
        }

        public void DestroyAllUI()
        {
            DestroyAllActiveUI();
            DestroyAllHideUI();
        }

        /// <summary>
        /// 删除所有打开的UI
        /// </summary>
        public void DestroyAllActiveUI()
        {
            foreach (List<UIWindowBase> uis in DisplayUIDict.Values)
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

                    Destroy(uis[i].gameObject);
                }
            }

            DisplayUIDict.Clear();
        }

        public T GetUI<T>() where T : UIWindowBase
        {
            return (T) GetUI(typeof(T).Name);
        }

        public UIWindowBase GetUI(string UIname)
        {
            if (!DisplayUIDict.ContainsKey(UIname))
            {
                //Debug.Log("!ContainsKey " + l_UIname);
                return null;
            }
            else
            {
                if (DisplayUIDict[UIname].Count == 0)
                {
                    //Debug.Log("s_UIs[UIname].Count == 0");
                    return null;
                }
                else
                {
                    //默认返回最后创建的那一个
                    return DisplayUIDict[UIname][DisplayUIDict[UIname].Count - 1];
                }
            }
        }

        public UIBase GetUIBaseByEventKey(string eventKey)
        {
            string UIkey = eventKey.Split('.')[0];
            string[] keyArray = UIkey.Split('_');

            string uiEventKey = "";

            UIBase uiTmp = null;
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (i == 0)
                {
                    uiEventKey = keyArray[0];
                    uiTmp = GetUIWindowByEventKey(uiEventKey);
                }
                else
                {
                    uiEventKey += "_" + keyArray[i];
                    uiTmp = uiTmp.GetItemByKey(uiEventKey);
                }

                Debug.Log("uiEventKey " + uiEventKey);
            }

            return uiTmp;
        }

        static Regex uiKey = new Regex(@"(\S+)\d+");

        UIWindowBase GetUIWindowByEventKey(string eventKey)
        {
            string UIname = uiKey.Match(eventKey).Groups[1].Value;

            if (!DisplayUIDict.ContainsKey(UIname))
            {
                throw new Exception("UIManager: GetUIWindowByEventKey error dont find UI name: ->" + eventKey + "<-  " +
                                    UIname);
            }

            List<UIWindowBase> list = DisplayUIDict[UIname];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UIEventKey == eventKey)
                {
                    return list[i];
                }
            }

            throw new Exception("UIManager: GetUIWindowByEventKey error dont find UI name: ->" + eventKey + "<-  " +
                                UIname);
        }

        bool GetIsExits(UIWindowBase UI)
        {
            if (!DisplayUIDict.ContainsKey(UI.name))
            {
                return false;
            }
            else
            {
                return DisplayUIDict[UI.name].Contains(UI);
            }
        }

        void AddUI(UIWindowBase UI)
        {
            if (!DisplayUIDict.ContainsKey(UI.name))
            {
                DisplayUIDict.Add(UI.name, new List<UIWindowBase>());
            }

            DisplayUIDict[UI.name].Add(UI);

            UI.Show();
        }

        void RemoveUI(UIWindowBase UI)
        {
            if (UI == null)
            {
                throw new Exception("UIManager: RemoveUI error l_UI is null: !");
            }

            if (!DisplayUIDict.ContainsKey(UI.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find UI name: ->" + UI.name + "<-  " + UI);
            }

            if (!DisplayUIDict[UI.name].Contains(UI))
            {
                throw new Exception("UIManager: RemoveUI error dont find UI: ->" + UI.name + "<-  " + UI);
            }
            else
            {
                DisplayUIDict[UI.name].Remove(UI);
            }
        }

        int GetUIID(string UIname)
        {
            if (!DisplayUIDict.ContainsKey(UIname))
            {
                return 0;
            }
            else
            {
                int id = DisplayUIDict[UIname].Count;

                for (int i = 0; i < DisplayUIDict[UIname].Count; i++)
                {
                    if (DisplayUIDict[UIname][i].UIID == id)
                    {
                        id++;
                        i = 0;
                    }
                }

                return id;
            }
        }

        public int GetNormalUICount()
        {
            return LayerManager.normalUIList.Count;
        }

        /// <summary>
        /// 删除所有隐藏的UI
        /// </summary>
        public void DestroyAllHideUI()
        {
            foreach (List<UIWindowBase> uis in HiddenUIDict.Values)
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

                    Destroy(uis[i].gameObject);
                }
            }

            HiddenUIDict.Clear();
        }

        /// <summary>
        /// 获取一个隐藏的UI,如果有多个同名UI，则返回最后创建的那一个
        /// </summary>
        /// <param name="UIname">UI名</param>
        /// <returns></returns>
        public UIWindowBase GetHideUI(string UIname)
        {
            if (!HiddenUIDict.ContainsKey(UIname))
            {
                return null;
            }
            else
            {
                if (HiddenUIDict[UIname].Count == 0)
                {
                    return null;
                }
                else
                {
                    UIWindowBase ui = HiddenUIDict[UIname][HiddenUIDict[UIname].Count - 1];
                    //默认返回最后创建的那一个
                    return ui;
                }
            }
        }

        bool GetIsExitsHide(UIWindowBase UI)
        {
            if (!HiddenUIDict.ContainsKey(UI.name))
            {
                return false;
            }
            else
            {
                return HiddenUIDict[UI.name].Contains(UI);
            }
        }

        void AddHideUI(UIWindowBase UI)
        {
            if (!HiddenUIDict.ContainsKey(UI.name))
            {
                HiddenUIDict.Add(UI.name, new List<UIWindowBase>());
            }

            HiddenUIDict[UI.name].Add(UI);

            UI.Hide();
        }


        void RemoveHideUI(UIWindowBase UI)
        {
            if (UI == null)
            {
                throw new Exception("UIManager: RemoveUI error l_UI is null: !");
            }

            if (!HiddenUIDict.ContainsKey(UI.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
            }

            if (!HiddenUIDict[UI.name].Contains(UI))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
            }
            else
            {
                HiddenUIDict[UI.name].Remove(UI);
            }
        }
    }
}