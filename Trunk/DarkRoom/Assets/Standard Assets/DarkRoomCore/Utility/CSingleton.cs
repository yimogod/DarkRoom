using UnityEngine;

namespace DarkRoom.Core
{
    /// <summary>
    /// 普通对象的单例
    /// </summary>
    public abstract class CSingleton<T> where T : new()
    {
        protected static T m_instance;
        private static object m_lock = new object();

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new T();
                        }
                    }
                }
                return m_instance;
            }
        }
    }


    /// <summary>
    /// MonoBehaviour 单例
    /// </summary>
    public class CSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = FindExistingInstance() ?? CreateNewInstance();
                return m_instance;
            }
        }

        private static T FindExistingInstance()
        {
            T[] existingInstances = FindObjectsOfType<T>();

            if (existingInstances == null || existingInstances.Length == 0) return null;
            return existingInstances[0];
        }


        private static T CreateNewInstance()
        {
            var go = new GameObject("__" + typeof(T).Name + " (Singleton)");
            go.transform.localPosition = Vector3.zero;
            return go.AddComponent<T>();
        }

        protected virtual void Awake()
        {
            T thisInstance = GetComponent<T>();

            //如果已经有instance, 就报错
            if (m_instance != null && thisInstance != m_instance)
            {
                Debug.LogError(string.Format("{0} alreay have one instance", typeof(T).Name));
                return;
            }

            m_instance = thisInstance;
            DontDestroyOnLoad(m_instance.gameObject);
        }

        protected virtual void OnDestroy()
        {
            m_instance = null;
        }
    }
}
