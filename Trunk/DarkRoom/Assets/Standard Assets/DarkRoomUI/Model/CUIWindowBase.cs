using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DarkRoom.UI
{
	/// <summary>
	/// 弹出面板的基类
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	public class CUIWindowBase : CUIBase
	{
		public Button CloseBtn;

		protected UIType m_uiType = UIType.Normal;
		private int m_UIID = -1;
		private string m_UIName;
		protected object[] m_args;

		public int UIID => m_UIID;
		public UIType UIType => m_uiType;
		public string UIName => m_UIName;

		

		/// <summary>
		/// 在awake后面, start前面调用
		/// </summary>
		/// <param name="id"></param>
		public void Init(int id)
		{
			m_UIID = id;
			m_UIName = null;
		}

		/// <summary>
		/// 刷新是主动调用
		/// </summary>
		public void Refresh(params object[] args)
		{
			m_args = args;
		}

		public virtual void OnOpen(params object[] args)
		{
			m_args = args;
			OnBindEvent();
			OnReveal();
		}

		public virtual void OnClose()
		{
			OnUnBindEvent();
			OnHide();
		}

		public virtual void OnHide()
		{
			gameObject.SetActive(false);
		}

		/// <summary>
		/// 隐藏的反义词, 显示出来
		/// </summary>
		public virtual void OnReveal()
		{
			gameObject.SetActive(true);
		}

		public virtual IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack)
		{
			//默认无动画
			l_animComplete(this, l_callBack);

			yield break;
		}

		public virtual void OnCompleteEnterAnim()
		{
		}

		public virtual IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack)
		{
			//默认无动画
			l_animComplete(this, l_callBack);

			yield break;
		}

		public virtual void OnCompleteExitAnim()
		{
		}

		protected override void OnBindEvent()
		{
			if(CloseBtn != null) CloseBtn.onClick.AddListener(OnClose);
		}

		protected override void OnUnBindEvent()
		{
			if (CloseBtn != null) CloseBtn.onClick.RemoveAllListeners();
		}

		//------------------------------------新手引导使用
		/*
		protected List<GameObject> m_GuideList = new List<GameObject>();

		protected Dictionary<GameObject, GuideChangeData> m_CreateCanvasDict =
		    new Dictionary<GameObject, GuideChangeData>(); //保存Canvas的创建状态

		public void SetGuideMode(string objName, int order = 1)
		{
		    SetGuideMode(GetGameObject(objName), order);
		}

		public void SetItemGuideMode(string itemName, int order = 1)
		{
		    SetGuideMode(GetItem(itemName), order);
		}

		public void SetItemGuideModeByIndex(string itemName, int index, int order = 1)
		{
		    SetGuideMode(GetItemByIndex(itemName, index).gameObject, order);
		}

		public void SetSelfGuideMode(int order = 1)
		{
		    SetGuideMode(gameObject, order);
		}

		public void SetGuideMode(GameObject go, int order = 1)
		{
		    Canvas canvas = go.GetComponent<Canvas>();
		    GraphicRaycaster graphic = go.GetComponent<GraphicRaycaster>();

		    GuideChangeData status = new GuideChangeData();

		    if (canvas == null)
		    {
		        canvas = go.AddComponent<Canvas>();

		        status.isCreateCanvas = true;
		    }

		    if (graphic == null)
		    {
		        graphic = go.AddComponent<GraphicRaycaster>();

		        status.isCreateGraphic = true;
		    }

		    status.OldOverrideSorting = canvas.overrideSorting;
		    status.OldSortingOrder = canvas.sortingOrder;
		    status.oldSortingLayerName = canvas.sortingLayerName;

		    //如果检测到目标对象
		    bool oldActive = go.activeSelf;
		    if (!oldActive)
		    {
		        go.SetActive(true);
		    }

		    canvas.overrideSorting = true;
		    canvas.sortingOrder = order;
		    canvas.sortingLayerName = "Guide";


		    if (!oldActive)
		    {
		        go.SetActive(false);
		    }

		    if (!m_CreateCanvasDict.ContainsKey(go))
		    {
		        m_CreateCanvasDict.Add(go, status);
		        m_GuideList.Add(go);
		    }
		    else
		    {
		        Debug.LogError("m_CreateCanvasDict " + go);
		    }
		}

		public void CancelGuideModel(GameObject go)
		{
		    if (go == null)
		    {
		        Debug.LogError("go is null");
		        return;
		    }

		    Canvas canvas = go.GetComponent<Canvas>();
		    GraphicRaycaster graphic = go.GetComponent<GraphicRaycaster>();

		    if (m_CreateCanvasDict.ContainsKey(go))
		    {
		        GuideChangeData status = m_CreateCanvasDict[go];

		        if (graphic != null && status.isCreateGraphic)
		        {
		            DestroyImmediate(graphic);
		        }

		        if (canvas != null && status.isCreateCanvas)
		        {
		            DestroyImmediate(canvas);
		        }
		        else
		        {
		            if (canvas != null)
		            {
		                canvas.overrideSorting = status.OldOverrideSorting;
		                canvas.sortingOrder = status.OldSortingOrder;
		                canvas.sortingLayerName = status.oldSortingLayerName;
		            }
		        }
		    }
		    else
		    {
		        Debug.LogError("m_CreateCanvasDict.ContainsKey(go) is error");
		    }
		}

		protected struct GuideChangeData
		{
		    public bool isCreateCanvas;
		    public bool isCreateGraphic;

		    public string oldSortingLayerName;
		    public int OldSortingOrder;
		    public bool OldOverrideSorting;
		}

		public void ClearGuideModel()
		{
		    for (int i = 0; i < m_GuideList.Count; i++)
		    {
		        CancelGuideModel(m_GuideList[i]);
		    }

		    for (int i = 0; i < m_ChildList.Count; i++)
		    {
		        m_ChildList[i].ClearGuideModel();
		    }

		    m_GuideList.Clear();
		    m_CreateCanvasDict.Clear();
		}*/
	}
}