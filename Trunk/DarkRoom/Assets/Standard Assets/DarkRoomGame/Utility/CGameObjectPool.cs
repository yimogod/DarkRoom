using UnityEngine;
using System.Collections.Generic;
using System;
using DarkRoom.Core;

namespace DarkRoom.Game
{
    public class CGameObjectPool : CSingletonMono<CGameObjectPool>
    {
        private Vector3 m_outOfRange = new Vector3(-10000, -10000, -10000);

        private Transform s_poolParent;
        public Transform PoolParent
        {
            get
            {
                if (s_poolParent == null)
                {
                    GameObject instancePool = new GameObject("ObjectPool");
                    s_poolParent = instancePool.transform;
                    if(Application.isPlaying)
                    DontDestroyOnLoad(s_poolParent);
                }

                return s_poolParent;
            }
        }

        Dictionary<string, List<CPoolEntity>> s_objectPool_new = new Dictionary<string, List<CPoolEntity>>();

        /// <summary>
        /// 加载一个对象并把它实例化
        /// </summary>
        /// <param name="gameObjectName">对象名</param>
        /// <param name="parent">对象的父节点,可空</param>
        /// <returns></returns>
        CPoolEntity CreatePoolObject(string gameObjectName, GameObject parent = null)
        {
            //CResourceManager.InstantiatePrefab();
            GameObject go = null;//ResourceManager.Load<GameObject>(gameObjectName);

            if (go == null)
            {
                throw new Exception("CreatPoolObject error dont find : ->" + gameObjectName + "<-");
            }

            GameObject instanceTmp = Instantiate(go);
            instanceTmp.name = go.name;

            CPoolEntity po = instanceTmp.GetComponent<CPoolEntity>();

            if (po == null)
            {
                throw new Exception("CreatPoolObject error : ->" + gameObjectName + "<- not is PoolObject !");
            }

            po.OnCreate();

            if (parent != null)
            {
                instanceTmp.transform.SetParent(parent.transform);
            }

            instanceTmp.SetActive(true);

            return po;
        }

        /// <summary>
        /// 把一个对象放入对象池
        /// </summary>
        /// <param name="gameObjectName"></param>
        public void PutPoolObject(string gameObjectName)
        {
            DestroyPoolObject(CreatePoolObject(gameObjectName));
        }

        /// <summary>
        /// 预存入对象池
        /// </summary>
        /// <param name="name"></param>
        public void PutPoolGameOject(string name)
        {
            //DestroyGameObjectByPool(CreateGameObjectByPool(name));
        }

        public bool IsExist_New(string objectName)
        {
            if (objectName == null)
            {
                Debug.LogError("IsExist_New error : objectName is null!");
                return false;
            }

            if (s_objectPool_new.ContainsKey(objectName) && s_objectPool_new[objectName].Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 从对象池取出一个对象，如果没有，则直接创建它
        /// </summary>
        /// <param name="name">对象名</param>
        /// <param name="parent">要创建到的父节点</param>
        /// <returns>返回这个对象</returns>
        public CPoolEntity GetPoolObject(string name, GameObject parent = null)
        {
            CPoolEntity po;
            if (IsExist_New(name))
            {
                po = s_objectPool_new[name][0];
                s_objectPool_new[name].RemoveAt(0);
                if (po && po.SetActive)
                    po.gameObject.SetActive(true);

                if (parent == null)
                {
                    po.transform.SetParent(null);
                }
                else
                {
                    po.transform.SetParent(parent.transform);
                }
            }
            else
            {
                po = CreatePoolObject(name, parent);
            }

            po.OnFetch();

            return po;
        }

        /// <summary>
        /// 将一个对象放入对象池
        /// </summary>
        /// <param name="obj">目标对象</param>
        public void DestroyPoolObject(CPoolEntity obj)
        {
            string key = obj.name.Replace("(Clone)", "");

            if (s_objectPool_new.ContainsKey(key) == false)
            {
                s_objectPool_new.Add(key, new List<CPoolEntity>());
            }

            if (s_objectPool_new[key].Contains(obj))
            {
                throw new Exception("DestroyPoolObject:-> Repeat Destroy GameObject !" + obj);
            }

            s_objectPool_new[key].Add(obj);

            if (obj.SetActive)
                obj.gameObject.SetActive(false);
            else
                obj.transform.position = m_outOfRange;

            obj.OnRecycle();

            obj.name = key;
            obj.transform.SetParent(PoolParent);
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void CleanPool_New()
        {
            foreach (string name in s_objectPool_new.Keys)
            {
                if (s_objectPool_new.ContainsKey(name))
                {
                    List<CPoolEntity> objList = s_objectPool_new[name];

                    for (int i = 0; i < objList.Count; i++)
                    {
                        try
                        {
                            objList[i].OnObjectDestroy();
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
                        }

                        Destroy(objList[i].gameObject);
                    }

                    objList.Clear();
                }
            }

            s_objectPool_new.Clear();
        }

        /// <summary>
        /// 清除掉某一个对象的所有对象池缓存
        /// </summary>
        public void CleanPoolByName_New(string name)
        {
            if (s_objectPool_new.ContainsKey(name))
            {
                List<CPoolEntity> objList = s_objectPool_new[name];

                for (int i = 0; i < objList.Count; i++)
                {
                    try
                    {
                        objList[i].OnObjectDestroy();
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e.ToString());
                    }

                    Destroy(objList[i].gameObject);
                }

                objList.Clear();
                s_objectPool_new.Remove(name);
            }
        }

        /*public void CreatePoolObjectAsync(string name, CallBack<CPoolEntity> callback, GameObject parent = null)
        {
            ResourceManager.LoadAsync(name, (status,res) =>
            {
                try
                {
                    callback(CreatePoolObject(name, parent));
                }
                catch(Exception e)
                {
                    Debug.LogError("CreatePoolObjectAsync Exception: " + e.ToString());
                }
            });
        }*/
    }
}
