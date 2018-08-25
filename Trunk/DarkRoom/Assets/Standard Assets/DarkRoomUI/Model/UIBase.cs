using UnityEngine;

namespace DarkRoom.UI
{
    /// <summary>
    /// go刚启动的时候, 调用顺序是 awake enable start update
    /// 
    /// 当go active的时候, 组件的onenable会重新调用
    /// 前提是enable之前会true, 如果为false, onenable 不会调用
    /// 当组件enable = true时会调用onenable, 但如果gameobject不是active, onenable不会调用
    /// 
    /// 组件的enable和active必须都喂true的时候, 在设置其中一个值的时候,onenable才会调用
    /// disable遵循同样道理
    /// </summary>
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
        /// 注意, 这里我们只绑定ui内部控件的事件
        /// 如果有广播事件, 在外部的mediator中做
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