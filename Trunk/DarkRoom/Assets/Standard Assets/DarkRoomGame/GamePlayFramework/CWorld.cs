using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 参照
	/// </summary>
	public class CWorld : MonoBehaviour{
	    private CWorldLayer m_layer;

	    private CGameMode m_mode;

	    //游戏数据
	    protected CGameState m_gameState;

        private static CWorld m_ins = null;

		public static CWorld Instance{
			get{
				if (m_ins == null)
				{
					GameObject go = new GameObject("World");
					go.transform.localPosition = Vector3.zero;
					m_ins = go.AddComponent<CWorld>();
				}
				return m_ins; 
			}
		}

	    public CWorldLayer Layer
	    {
	        get { return m_layer; }
	    }

        private void Awake()
	    {
	        m_ins = this;
	        m_layer = new CWorldLayer(gameObject);
	    }

        /// <summary>
        /// 初始化GameMode
        /// </summary>
        public void InitializeGameMode<T>() where T : CGameMode
	    {
	        m_mode = default(T);
	    }

        /// <summary>
        /// 初始化GameState
        /// </summary>
	    public void InitializeGameState<T>() where T : CGameState
	    {
	        m_gameState = default(T);
	    }

        /// <summary>
        /// 获取游戏逻辑
        /// </summary>
	    public T GetGameMode<T>() where T : CGameMode
	    {
            return m_mode as T;
	    }

        /// <summary>
        /// 获取游戏数据
        /// </summary>
	    public T GetGameState<T>() where T : CGameState
        {
	        return m_gameState as T;
	    }

        /// <summary>
        /// 创建一个单位
        /// </summary>
        public GameObject SpawnUnit<T>(string unitName, Vector3 localPosition) where T : CUnitEntity
	    {
            GameObject go = new GameObject(unitName);
	        go.transform.parent = m_layer.UnitLayer;
	        go.transform.localPosition = localPosition;
	        go.AddComponent<T>();
	        return go;
	    }

        /// <summary>
        /// 创建一个单位, n秒后自动销毁
        /// </summary>
	    public GameObject SpawnAutoDestroyUnit<T>(string unitName, Vector3 localPosition, int timeLast) where T : CUnitEntity
        {
            GameObject go = SpawnUnit<T>(unitName, localPosition);
            go.AddComponent<CAutoDestoryComp>();
	        return go;
	    }

	    /// <summary>
	    /// 在指定坐标创建效果
	    /// </summary>
	    public ParticleSystem SpawnEmitterAtLocation(string prefab, Vector3 localPosition)
	    {
	        GameObject go = new GameObject(prefab);
	        go.transform.parent = m_layer.UnitLayer;
	        go.transform.localPosition = localPosition;
	        return null;
	    }

        /// <summary>
        /// 在parent身上的slot child身上创建prefab效果
        /// 有位置偏移offset
        /// </summary>
	    public ParticleSystem SpawnEmitterAttachedTransform(string prefab, Transform root, string slot, Vector3 offset)
        {
            Transform parent = null;
	        if (!string.IsNullOrEmpty(slot))parent = root.Find("slot");
            if (parent == null) parent = root;

            GameObject go = new GameObject(prefab);
	        go.transform.parent = parent;
	        go.transform.localPosition = offset;
	        return null;
	    }

        private void OnDestroy(){
			m_ins = null;
		    m_layer.OnDestroy();
            m_mode.OnDestroy();
            m_gameState.OnDestroy();
        }
	}

    public class CWorldLayer
    {
        public const string LAYER_NAME_TERRAIN = "Terrain";
        public const string LAYER_NAME_TERRAIN_02 = "Terrain_02";
        public const string LAYER_NAME_LAKE = "Lake";

        /// <summary>
        /// 贴花. 这个层需要再观察是否需要
        /// </summary>
        public const string LAYER_NAME_DECAL = "Decal";

        /// <summary>
        /// 类似于魔兽争霸地编的 trigger 区域. 没有视图.
        /// </summary>
        public const string LAYER_NAME_TRIGGER = "Trigger";

        /// <summary>
        /// 主要单位层. 
        /// 比如3d, 包含英雄, npc, 障碍物, 掉落物
        /// </summary>
        public const string LAYER_NAME_UNIT = "Unit";
        public const string LAYER_NAME_WALKABLE = "Walkable";
        public const string LAYER_NAME_GLOW = "Glow";

        public enum LayerIndex
        {
            TERRIAN = 0,
            TERRIAN_02, //这个代表terraincomp的第二种地形, 索引为1, 不能删除
            ROAD, //lake用这层
            DECAL,
            UNIT,//TRIGGER, Deco
            LOW_SKY,
            HIGH_SKY,
            WALKABLE
        }

        public GameObject Root;
        public Transform UnitLayer;//存放所有的可以交互的单位, 英雄,npc,掉落物品等等
        public Transform TriggerLayer;
        public Transform DecoLayer;//装饰物品
        public Transform PoolLayer;
        public Transform ItemPoolLayer; //一些英雄的装备

        public CWorldLayer(GameObject root)
        {
            Root = root;

            GameObject go = null;
            UnitLayer = Root.transform.Find("unit_layer");
            if (UnitLayer == null)
            {
                UnitLayer = new GameObject("unit_layer").transform;
                UnitLayer.parent = Root.transform;
                UnitLayer.localPosition = Vector3.zero;
                return;
            }

            go = UnitLayer.gameObject;
            go.layer = LayerMask.NameToLayer("Unit");

            go = CreateLayer("trigger_layer");
            TriggerLayer = go.transform;
            TriggerLayer.localPosition = UnitLayer.localPosition;

            go = CreateLayer("deco_layer");
            DecoLayer = go.transform;
            DecoLayer.localPosition = UnitLayer.localPosition;

            go = CreateLayer("pool_layer");
            PoolLayer = go.transform;
        }

        private GameObject CreateLayer(string name)
        {
            GameObject go = new GameObject(name);
            CDarkUtil.AddChild(Root.transform, go.transform);
            return go;
        }

        public void OnDestroy()
        {
            Root = null;
            DecoLayer = null;
            UnitLayer = null;
            PoolLayer = null;
        }
    }
}

