using UnityEngine;
using System.Collections.Generic;
using System;
using DarkRoom.Core;
using DarkRoom.Game;

/// <summary>
/// ui板子总体的管理器, 每个板子存在6中行为
/// create, open, close, show, hide, destroy
/// 注意, 我们的create和第一次open都是异步的
/// </summary>
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
        public Transform UIRoot => transform;

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
            ActiveDict = new Dictionary<string, List<UIWindowBase>>();

        /// <summary>
        /// 当前隐藏的UI列表
        /// </summary>
        public Dictionary<string, List<UIWindowBase>>
            HiddenDict = new Dictionary<string, List<UIWindowBase>>();

        public void Initialize()
        {
            LayerManager = GetComponent<CUILayerManager>();
            AnimManager = GetComponent<CUIAnimManager>();
            StackManager = GetComponent<CUIStackManager>();
        }

        /// <summary>
        /// 创建UI, 放在Hide列表中, 用于预创建ui
        /// </summary>
        public void CreateUI<T>() where T : UIWindowBase
        {
            string address = typeof(T).Name;
            CResourceManager.InstantiatePrefab(address, UIRoot, InternalCreateUI<T>);
        }

        private void InternalCreateUI<T>(GameObject go) where T : UIWindowBase
        {
            string winName = typeof(T).Name;
            UIWindowBase window = go.GetComponent<UIWindowBase>();
            try
            {
                int id = CUIManagerHelper.GetUIIDFromCache(winName, ActiveDict);
                window.Init(id);
            }
            catch (Exception e)
            {
                Debug.LogError("OnInit Exception: " + e.ToString());
            }

            window.Hide();
            CUIManagerHelper.AddUIToCache(window, HiddenDict);
            LayerManager.SetLayer(window); //设置层级
        }

        public void OpenUI<T>(UICallBack callback = null, params object[] objs) where T : UIWindowBase
        {
            string winName = typeof(T).Name;
            UIWindowBase window = CUIManagerHelper.GetUIFromCache(winName, HiddenDict);
            if (window == null)CreateUI<T>();

            CUIManagerHelper.RemoveUIFromCache(window, HiddenDict);
            CUIManagerHelper.AddUIToCache(window, ActiveDict);
            window.Show();

            StackManager.OnUIOpen(window);
            LayerManager.SetLayer(window); //设置层级

            try
            {
                window.OnOpen();
            }
            catch (Exception e)
            {
                Debug.LogError(winName + " OnOpen Exception: " + e);
            }


            AnimManager.StartEnterAnim(window, callback, objs); //播放动画
        }

        private void InternalOpenUI<T>(UICallBack callback = null, params object[] objs) where T : UIWindowBase
        {

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
            CUIManagerHelper.RemoveUIFromCache(UI, ActiveDict);
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

        public void CloseUIWindow(string UIname, bool isPlayAnim = true, UICallBack callback = null,
            params object[] objs)
        {
            UIWindowBase ui = CUIManagerHelper.GetUIFromCache(UIname, ActiveDict);

            if (ui == null)
            {
                Debug.LogError("CloseUIWindow Error UI ->" + UIname + "<-  not Exist!");
                return;
            }

            CloseUIWindow(ui, isPlayAnim, callback, objs);
        }

        public void CloseUIWindow<T>(bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
            where T : UIWindowBase
        {
            CloseUIWindow(typeof(T).Name, isPlayAnim, callback, objs);
        }

        /// <summary>
        /// 移除全部UI
        /// </summary>
        public void CloseAllUI(bool isPlayerAnim = false)
        {
            List<string> keys = new List<string>(ActiveDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = ActiveDict[keys[i]];
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

        public UIWindowBase ShowUI(string viewName)
        {
            UIWindowBase ui = CUIManagerHelper.GetUIFromCache(viewName, ActiveDict);
            return ShowUI(ui);
        }

        public UIWindowBase ShowUI(UIWindowBase view)
        {
            try
            {
                view.Show();
                view.OnShow();
            }
            catch (Exception e)
            {
                Debug.LogError(view.UIName + " OnShow Exception: " + e.ToString());
            }

            return view;
        }

        public void ShowOtherUI(string viewName)
        {
            List<string> keys = new List<string>(ActiveDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = ActiveDict[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].UIName != viewName)
                    {
                        ShowUI(list[j]);
                    }
                }
            }
        }

        public UIWindowBase HideUI(string viewName)
        {
            UIWindowBase ui = CUIManagerHelper.GetUIFromCache(viewName, ActiveDict);
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

        public void HideOtherUI(string viewName)
        {
            List<string> keys = new List<string>(ActiveDict.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = ActiveDict[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].UIName != viewName)
                    {
                        HideUI(list[j]);
                    }
                }
            }
        }

        public void DestroyUI(UIWindowBase UI)
        {
            Debug.Log("UIManager DestroyUI " + UI.name);

            CUIManagerHelper.RemoveUIFromCache(UI, HiddenDict);
            CUIManagerHelper.RemoveUIFromCache(UI, ActiveDict);

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
            CUIManagerHelper.DestroyUIInCache(ActiveDict);
            CUIManagerHelper.DestroyUIInCache(HiddenDict);
        }

        public int GetNormalUICount()
        {
            return LayerManager.normalUIList.Count;
        }

        private void CloseUIWindowCallBack(UIWindowBase UI, params object[] objs)
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
            CUIManagerHelper.AddUIToCache(UI, HiddenDict);
            UI.Hide();
        }
    }
}