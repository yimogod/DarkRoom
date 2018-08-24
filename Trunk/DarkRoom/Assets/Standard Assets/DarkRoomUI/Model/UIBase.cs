using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace DarkRoom.UI
{
    public class UIBase : MonoBehaviour
    {
        public Canvas m_canvas;

        //当UI第一次打开时调用OnInit方法，调用时机在OnOpen之前
        public virtual void OnInit()
        {
        }

        public void DestroyUI()
        {
            ClearGuideModel();
            CleanItem();
            OnUIDestroy();
        }

        protected virtual void OnUIDestroy()
        {

        }

        private int m_UIID = -1;

        public int UIID => m_UIID;

        public string UIEventKey
        {
            get { return UIName + m_UIID; }
            //set { m_UIID = value; }
        }

        string m_UIName = null;

        public string UIName
        {
            get
            {
                if (m_UIName == null)
                {
                    m_UIName = name;
                }

                return m_UIName;
            }
            set { m_UIName = value; }
        }

        public void Init(int id)
        {
            m_UIID = id;
            m_canvas = GetComponent<Canvas>();
            m_UIName = null;
            CreateObjectTable();
            OnInit();
        }

        //public void Destroy()
        //{
        //    OnDestroy();
        //}

        public List<GameObject> m_objectList = new List<GameObject>();

        //生成对象表，便于快速获取对象，并忽略层级
        void CreateObjectTable()
        {
            m_objects.Clear();

            m_images.Clear();
            m_Sprites.Clear();
            m_texts.Clear();
            m_textmeshs.Clear();
            m_buttons.Clear();
            m_scrollRects.Clear();
            m_rawImages.Clear();
            m_rectTransforms.Clear();
            m_inputFields.Clear();
            m_Sliders.Clear();
            m_Canvas.Clear();

            for (int i = 0; i < m_objectList.Count; i++)
            {
                if (m_objectList[i] != null)
                {
                    //Debug.Log("===>"+m_objectList[i].name);
                    if (m_objects.ContainsKey(m_objectList[i].name))
                    {
                        Debug.LogError("CreateObjectTable ContainsKey ->" + m_objectList[i].name + "<-");
                    }
                    else
                    {
                        m_objects.Add(m_objectList[i].name, m_objectList[i]);
                    }
                }
                else
                {
                    Debug.LogWarning(name + " m_objectList[" + i + "] is Null !");
                }
            }
        }

        Dictionary<string, UIBase> m_uiBases = new Dictionary<string, UIBase>();

        Dictionary<string, GameObject> m_objects = new Dictionary<string, GameObject>();
        Dictionary<string, Image> m_images = new Dictionary<string, Image>();
        Dictionary<string, Sprite> m_Sprites = new Dictionary<string, Sprite>();
        Dictionary<string, Text> m_texts = new Dictionary<string, Text>();
        Dictionary<string, TextMesh> m_textmeshs = new Dictionary<string, TextMesh>();
        Dictionary<string, Button> m_buttons = new Dictionary<string, Button>();
        Dictionary<string, ScrollRect> m_scrollRects = new Dictionary<string, ScrollRect>();
        Dictionary<string, RawImage> m_rawImages = new Dictionary<string, RawImage>();
        Dictionary<string, RectTransform> m_rectTransforms = new Dictionary<string, RectTransform>();
        Dictionary<string, InputField> m_inputFields = new Dictionary<string, InputField>();
        Dictionary<string, Slider> m_Sliders = new Dictionary<string, Slider>();
        Dictionary<string, Canvas> m_Canvas = new Dictionary<string, Canvas>();
        Dictionary<string, Toggle> m_Toggle = new Dictionary<string, Toggle>();

        public bool HaveObject(string name)
        {
            bool has = false;
            has = m_objects.ContainsKey(name);
            return has;
        }

        public GameObject GetGameObject(string name)
        {
            if (m_objects == null)
            {
                CreateObjectTable();
            }

            if (m_objects.ContainsKey(name))
            {
                GameObject go = m_objects[name];

                if (go == null)
                {
                    throw new Exception("UIWindowBase GetGameObject error: " + UIName + " m_objects[" + name +
                                        "] is null !!");
                }

                return go;
            }
            else
            {
                throw new Exception("UIWindowBase GetGameObject error: " + UIName + " dont find ->" + name + "<-");
            }
        }

        public RectTransform GetRectTransform(string name)
        {
            if (m_rectTransforms.ContainsKey(name))
            {
                return m_rectTransforms[name];
            }

            RectTransform tmp = GetGameObject(name).GetComponent<RectTransform>();


            if (tmp == null)
            {
                throw new Exception(name + " GetRectTransform ->" + name + "<- is Null !");
            }

            m_rectTransforms.Add(name, tmp);
            return tmp;
        }

        public UIBase GetUIBase(string name)
        {
            if (m_uiBases.ContainsKey(name))
            {
                return m_uiBases[name];
            }

            UIBase tmp = GetGameObject(name).GetComponent<UIBase>();

            if (tmp == null)
            {
                throw new Exception(name + " GetUIBase ->" + name + "<- is Null !");
            }

            m_uiBases.Add(name, tmp);
            return tmp;
        }

        public Sprite GetSprite(string name)
        {
            if (m_Sprites.ContainsKey(name))
            {
                return m_Sprites[name];
            }

            Sprite tmp = GetGameObject(name).GetComponent<Sprite>();

            if (tmp == null)
            {
                throw new Exception(name + " GetImage ->" + name + "<- is Null !");
            }

            m_Sprites.Add(name, tmp);
            return tmp;
        }

        public Image GetImage(string name)
        {
            if (m_images.ContainsKey(name))
            {
                return m_images[name];
            }

            Image tmp = GetGameObject(name).GetComponent<Image>();

            if (tmp == null)
            {
                throw new Exception(name + " GetImage ->" + name + "<- is Null !");
            }

            m_images.Add(name, tmp);
            return tmp;
        }

        public TextMesh GetTextMesh(string name)
        {
            if (m_textmeshs.ContainsKey(name))
            {
                return m_textmeshs[name];
            }

            TextMesh tmp = GetGameObject(name).GetComponent<TextMesh>();



            if (tmp == null)
            {
                throw new Exception(name + " GetText ->" + name + "<- is Null !");
            }

            m_textmeshs.Add(name, tmp);
            return tmp;
        }

        public Text GetText(string name)
        {
            if (m_texts.ContainsKey(name))
            {
                return m_texts[name];
            }

            Text tmp = GetGameObject(name).GetComponent<Text>();

            if (tmp == null)
            {
                throw new Exception(name + " GetText ->" + name + "<- is Null !");
            }

            m_texts.Add(name, tmp);
            return tmp;
        }

        public Toggle GetToggle(string name)
        {
            if (m_Toggle.ContainsKey(name))
            {
                return m_Toggle[name];
            }

            Toggle tmp = GetGameObject(name).GetComponent<Toggle>();

            if (tmp == null)
            {
                throw new Exception(name + " GetText ->" + name + "<- is Null !");
            }

            m_Toggle.Add(name, tmp);
            return tmp;
        }

        public Button GetButton(string name)
        {
            if (m_buttons.ContainsKey(name))
            {
                return m_buttons[name];
            }

            Button tmp = GetGameObject(name).GetComponent<Button>();

            if (tmp == null)
            {
                throw new Exception(name + " GetButton ->" + name + "<- is Null !");
            }

            m_buttons.Add(name, tmp);
            return tmp;
        }

        public InputField GetInputField(string name)
        {
            if (m_inputFields.ContainsKey(name))
            {
                return m_inputFields[name];
            }

            InputField tmp = GetGameObject(name).GetComponent<InputField>();

            if (tmp == null)
            {
                throw new Exception(name + " GetInputField ->" + name + "<- is Null !");
            }

            m_inputFields.Add(name, tmp);
            return tmp;
        }

        public ScrollRect GetScrollRect(string name)
        {
            if (m_scrollRects.ContainsKey(name))
            {
                return m_scrollRects[name];
            }

            ScrollRect tmp = GetGameObject(name).GetComponent<ScrollRect>();

            if (tmp == null)
            {
                throw new Exception(name + " GetScrollRect ->" + name + "<- is Null !");
            }

            m_scrollRects.Add(name, tmp);
            return tmp;
        }

        public RawImage GetRawImage(string name)
        {
            if (m_rawImages.ContainsKey(name))
            {
                return m_rawImages[name];
            }

            RawImage tmp = GetGameObject(name).GetComponent<RawImage>();

            if (tmp == null)
            {
                throw new Exception(name + " GetRawImage ->" + name + "<- is Null !");
            }

            m_rawImages.Add(name, tmp);
            return tmp;
        }

        public Slider GetSlider(string name)
        {
            if (m_Sliders.ContainsKey(name))
            {
                return m_Sliders[name];
            }

            Slider tmp = GetGameObject(name).GetComponent<Slider>();

            if (tmp == null)
            {
                throw new Exception(name + " GetSlider ->" + name + "<- is Null !");
            }

            m_Sliders.Add(name, tmp);
            return tmp;
        }

        public Canvas GetCanvas(string name)
        {
            if (m_Canvas.ContainsKey(name))
            {
                return m_Canvas[name];
            }

            Canvas tmp = GetGameObject(name).GetComponent<Canvas>();

            if (tmp == null)
            {
                throw new Exception(name + " GetSlider ->" + name + "<- is Null !");
            }

            m_Canvas.Add(name, tmp);
            return tmp;
        }

        public Vector3 GetPosition(string name, bool islocal)
        {
            Vector3 tmp = Vector3.zero;
            GameObject go = GetGameObject(name);
            if (go != null)
            {
                if (islocal)
                    tmp = GetGameObject(name).transform.localPosition;
                else
                    tmp = GetGameObject(name).transform.position;
            }

            return tmp;
        }

        private RectTransform m_rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (m_rectTransform == null)
                {
                    m_rectTransform = GetComponent<RectTransform>();
                }

                return m_rectTransform;
            }
            set { m_rectTransform = value; }
        }

        public void SetSizeDelta(float w, float h)
        {
            RectTransform.sizeDelta = new Vector2(w, h);
        }

        List<UIBase> m_ChildList = new List<UIBase>();
        int m_childUIIndex = 0;

        public UIBase CreateItem(string itemName, string prantName, bool isActive)
        {
            GameObject item = new GameObject(); //GameObjectManager.CreateGameObjectByPool(itemName, GetGameObject(prantName), isActive);

            item.transform.localScale = Vector3.one;
            UIBase UIItem = item.GetComponent<UIBase>();

            if (UIItem == null)
            {
                throw new Exception("CreateItem Error : ->" + itemName + "<- don't have UIBase Component!");
            }

            UIItem.Init(m_childUIIndex++);
            UIItem.UIName = UIEventKey + "_" + UIItem.UIName;

            m_ChildList.Add(UIItem);

            return UIItem;
        }

        public UIBase CreateItem(GameObject itemObj, GameObject parent, bool isActive)
        {
            GameObject item = new GameObject(); //= GameObjectManager.CreateGameObjectByPool(itemObj, parent, isActive);

            item.transform.localScale = Vector3.one;
            UIBase UIItem = item.GetComponent<UIBase>();

            if (UIItem == null)
            {
                throw new Exception("CreateItem Error : ->" + itemObj.name + "<- don't have UIBase Component!");
            }

            UIItem.Init(m_childUIIndex++);
            UIItem.UIName = UIEventKey + "_" + UIItem.UIName;

            m_ChildList.Add(UIItem);

            return UIItem;
        }

        public UIBase CreateItem(string itemName, string prantName)
        {
            return CreateItem(itemName, prantName, true);
        }


        public void DestroyItem(UIBase item)
        {
            DestroyItem(item, true);
        }

        public void DestroyItem(UIBase item, bool isActive)
        {
            if (m_ChildList.Contains(item))
            {
                m_ChildList.Remove(item);
                //item.RemoveAllListener();
                item.OnUIDestroy();
               // GameObjectManager.DestroyGameObjectByPool(item.gameObject, isActive);
            }
        }

        public void DestroyItem(UIBase item, float t)
        {
            if (m_ChildList.Contains(item))
            {
                m_ChildList.Remove(item);
                item.OnUIDestroy();
               // GameObjectManager.DestroyGameObjectByPool(item.gameObject, t);
            }
        }

        public void CleanItem()
        {
            CleanItem(true);
        }

        public void CleanItem(bool isActive)
        {
            for (int i = 0; i < m_ChildList.Count; i++)
            {
                //m_ChildList[i].RemoveAllListener();
                //m_ChildList[i].OnUIDestroy();
               // GameObjectManager.DestroyGameObjectByPool(m_ChildList[i].gameObject, isActive);
            }

            m_ChildList.Clear();
            m_childUIIndex = 0;
        }

        public GameObject GetItem(string itemName)
        {
            //Debug.Log("GetItem  v0 " + m_ChildList.Count);
            if (!itemName.Contains("."))
            {
                //Debug.Log("GetItem  v1 " + m_ChildList.Count);
                int index = 0;
                if (itemName.Contains("["))
                {
                    index = int.Parse(itemName.Substring(itemName.IndexOf("[") + 1, 1));
                }

                int rd_index = 0;
                for (int i = 0; i < m_ChildList.Count; i++)
                {
                    if (m_ChildList[i].name == itemName)
                    {
                        if (index == rd_index)
                        {
                            return m_ChildList[i].gameObject;
                        }
                        else
                        {
                            rd_index = rd_index + 1;
                        }

                    }
                }
            }
            else
            {
                //Debug.Log("GetItem  v2 ");
                string[] itemNames = itemName.Split('.');
                //Debug.Log("itemNames.Length " + itemNames.Length);

                List<Transform> Comp_list = new List<Transform>();

                //for (int i = 0; i < m_ChildList.Count; i++)
                //{
                //    Comp_list.Add(m_ChildList[i].transform);
                //}

                for (int j = 0; j < itemNames.Length; j++)
                {

                    int index = 0;
                    string t_itemName = itemNames[j];

                    if (t_itemName.Contains("["))
                    {
                        index = int.Parse(t_itemName.Substring(t_itemName.IndexOf("[") + 1, 1));
                    }

                    t_itemName = t_itemName.Substring(0, t_itemName.Length - 3);
                    if (j == 0)
                    {
                        //Debug.Log("===tsss=>"+ t_itemName);
                        //Transform tss = GetGameObject(t_itemName).transform;

                        //Debug.Log("===t=>"+ tss.name,tss);
                        //Debug.Log("==child" + tss.GetChild(0), tss.GetChild(0));


                        //int len = tss.childCount;
                        //Debug.Log(len);
                        //for (int i = 0; i < len; i++)
                        //{
                        //    Transform itemts = tss.GetChild(i);
                        //    Debug.Log("===item>" + itemts.name);
                        //}

                        Transform[] aa = GetGameObject(t_itemName).GetComponentsInChildren<Transform>();
                        //for (int i = 0; i < aa.Length; i++)
                        //{
                        //    Debug.Log("bbbbb " + aa[i].name);
                        //}
                        //Debug.Log("aaaaaaaa " + t_itemName + " " + aa.Length);
                        Comp_list = new List<Transform>(aa);
                    }
                    else
                    {


                        //Debug.Log("t_itemNameA " + t_itemName + " " + index + " " + Comp_list.Count);
                        int rd_index = 0;
                        for (int i = 0; i < Comp_list.Count; i++)
                        {
                            //Debug.Log("Comp_list[i].name " + Comp_list[i].name);
                            if (Comp_list[i].name == t_itemName)
                            {
                                if (index == rd_index)
                                {
                                    if (j == itemNames.Length - 1)
                                    {
                                        //Debug.Log("GetItem  v4 ");
                                        return Comp_list[i].gameObject;
                                    }
                                    else
                                    {
                                        //Debug.Log("GetItem  v3 ");
                                        Transform[] aa = Comp_list[i].GetComponentsInChildren<Transform>();
                                        Comp_list = new List<Transform>(aa);
                                        break;
                                    }
                                }
                                else
                                {
                                    rd_index = rd_index + 1;
                                }

                            }
                        }
                    }
                }
            }

            throw new Exception(UIName + " GetItem Exception Dont find Item: " + itemName);
        }

        public UIBase GetItemByIndex(string itemName, int index)
        {
            for (int i = 0; i < m_ChildList.Count; i++)
            {
                if (m_ChildList[i].name == itemName)
                {
                    //Debug.Log("GetItemByIndex " + index, m_ChildList[i]);

                    index--;
                    if (index == 0)
                    {
                        return m_ChildList[i];
                    }
                }
            }

            throw new Exception(UIName + " GetItem Exception Dont find Item: " + itemName);
        }

        public UIBase GetItemByKey(string uiEvenyKey)
        {
            for (int i = 0; i < m_ChildList.Count; i++)
            {
                if (m_ChildList[i].UIEventKey == uiEvenyKey)
                {
                    return m_ChildList[i];
                }
            }

            throw new Exception(UIName + " GetItemByKey Exception Dont find Item: " + uiEvenyKey);
        }

        public bool GetItemIsExist(string itemName)
        {
            for (int i = 0; i < m_ChildList.Count; i++)
            {
                if (m_ChildList[i].name == itemName)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetText(string TextID, string content)
        {
            GetText(TextID).text = content.Replace("//n", "/n");
        }

        public void SetImageColor(string ImageID, Color color)
        {
            GetImage(ImageID).color = color;
        }

        public void SetTextColor(string TextID, Color color)
        {
            GetText(TextID).color = color;
        }

        public void SetImageAlpha(string ImageID, float alpha)
        {
            Color col = GetImage(ImageID).color;
            col.a = alpha;
            GetImage(ImageID).color = col;
        }

        public void SetInputText(string TextID, string content)
        {
            GetInputField(TextID).text = content;
        }

        public void SetSlider(string sliderID, float value)
        {
            GetSlider(sliderID).value = value;
        }

        public void SetActive(string gameObjectID, bool isShow)
        {
            GetGameObject(gameObjectID).SetActive(isShow);
        }

        /// <summary>
        /// Only Button
        /// </summary>
        public void SetEnabeled(string ID, bool enable)
        {
            GetButton(ID).enabled = enable;
        }

        /// <summary>
        /// Only Button
        /// </summary>
        public void SetButtonInteractable(string ID, bool enable)
        {
            GetButton(ID).interactable = enable;
        }

        public void SetRectWidth(string TextID, float value, float height)
        {
            GetRectTransform(TextID).sizeDelta = Vector2.right * -value * 2 + Vector2.up * height;
        }

        public void SetWidth(string TextID, float width, float height)
        {
            GetRectTransform(TextID).sizeDelta = Vector2.right * width + Vector2.up * height;
        }

        public void SetPosition(string TextID, float x, float y, float z, bool islocal)
        {
            if (islocal)
                GetRectTransform(TextID).localPosition = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
            else
                GetRectTransform(TextID).position = Vector3.right * x + Vector3.up * y + Vector3.forward * z;

        }

        public void SetAnchoredPosition(string ID, float x, float y)
        {
            GetRectTransform(ID).anchoredPosition = Vector2.right * x + Vector2.up * y;
        }

        public void SetScale(string TextID, float x, float y, float z)
        {
            GetGameObject(TextID).transform.localScale = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
        }

        public void SetMeshText(string TextID, string txt)
        {
            GetTextMesh(TextID).text = txt;
        }


        //------------------------------------新手引导使用

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
        }

        [ContextMenu("ObjectList 去重")]
        public void ClearObject()
        {
            List<GameObject> ls = new List<GameObject>();
            int len = m_objectList.Count;
            for (int i = 0; i < len; i++)
            {
                GameObject go = m_objectList[i];
                if (go != null)
                {
                    if (!ls.Contains(go)) ls.Add(go);
                }
            }

            ls.Sort((a, b) => { return a.name.CompareTo(b.name); });

            m_objectList = ls;
        }
    }
}