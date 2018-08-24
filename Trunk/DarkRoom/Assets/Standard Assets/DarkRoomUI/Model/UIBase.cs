using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace DarkRoom.UI
{
    public class UIBase : MonoBehaviour
    {


        private void Awake()
        {
            //m_rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 在awake后面, start前面调用
        /// 创建的时候调用一次
        /// </summary>
        protected virtual void OnCreated()
        {

        }

        /// <summary>
        /// 销毁的时候调用
        /// </summary>
        protected virtual void OnDestroy()
        {

        }

        /// <summary>
        /// open的时候绑定事件
        /// </summary>
        protected virtual void OnBindEvent()
        {

        }

        /// <summary>
        /// close 的时候调用
        /// </summary>
        protected virtual void OnUnBindEvent()
        {

        }
    }
}