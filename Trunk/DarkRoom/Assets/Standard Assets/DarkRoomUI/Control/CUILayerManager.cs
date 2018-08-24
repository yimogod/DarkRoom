using UnityEngine;
using System.Collections.Generic;

namespace DarkRoom.UI
{
    public class CUILayerManager : MonoBehaviour
    {
        private Transform m_hudLayer;
        private Transform m_normalLayer;
        private Transform m_dialogLayer;
        private Transform m_tipsLayer;
        private Transform m_maskLayer;

        void Awake()
        {
           
        }

        public void SetLayer(UIWindowBase window)
        {
            RectTransform rt = window.GetComponent<RectTransform>();
            switch (window.UIType)
            {
                case UIType.HUD:
                    window.transform.SetParent(m_hudLayer);
                    break;
                case UIType.Normal:
                    window.transform.SetParent(m_normalLayer);
                    break;
                case UIType.Dialog:
                    window.transform.SetParent(m_dialogLayer);
                    break;
                case UIType.Tips:
                    window.transform.SetParent(m_tipsLayer);
                    break;
                case UIType.Mask:
                    window.transform.SetParent(m_maskLayer);
                    break;
            }

            rt.localScale = Vector3.one;
            rt.sizeDelta = Vector2.zero;

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector3.one;

            rt.anchoredPosition = Vector3.zero;
            rt.SetAsLastSibling();
        }

    }
}