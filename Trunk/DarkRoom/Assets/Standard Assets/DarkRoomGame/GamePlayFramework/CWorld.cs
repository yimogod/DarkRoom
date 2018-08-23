using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 参照
	/// </summary>
	public class CWorld : CSingletonMono<CWorld> {
	    private CWorldLayer m_layer;

	    private CGameMode m_mode;

	    //游戏数据
	    protected CGameState m_gameState;

	    public CWorldLayer Layer => m_layer;

        private void Start()
	    {
	        m_layer = new CWorldLayer(gameObject);
	    }

        /// <summary>
        /// 初始化GameMode
        /// </summary>
        public void InitializeGameMode<T>() where T : CGameMode, new()
	    {
	        m_mode = new T();
	    }

        /// <summary>
        /// 初始化GameState
        /// </summary>
	    public void InitializeGameState<T>() where T : CGameState, new()
	    {
	        m_gameState = new T();
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
        public T SpawnUnit<T>(string unitName, Vector3 localPosition) where T : CUnitEntity
	    {
            GameObject go = new GameObject(unitName);
	        go.transform.parent = m_layer.UnitLayer;
	        go.transform.localPosition = localPosition;
	        var comp = go.AddComponent<T>();
	        return comp;
	    }

        /// <summary>
        /// 创建一个单位, n秒后自动销毁
        /// </summary>
	    public T SpawnAutoDestroyUnit<T>(string unitName, Vector3 localPosition, int timeLast) where T : CUnitEntity
        {
            T go = SpawnUnit<T>(unitName, localPosition);
            go.gameObject.AddComponent<CAutoDestoryComp>();
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

        protected override void OnDestroy(){
            base.OnDestroy();
		    m_layer.OnDestroy();
            m_gameState.OnDestroy();
            m_mode.OnDestroy();
        }
	}

    public class CWorldLayer
    {
        public const string LAYER_NAME_TERRAIN = "Terrain";

        /// <summary>
        /// 主要单位层. 
        /// 比如3d, 包含英雄, npc, 障碍物, 掉落物
        /// </summary>
        public const string LAYER_NAME_UNIT = "Unit";


        public GameObject Root;
        public Transform TerrainLayer;
        public Transform UnitLayer;//存放所有的可以交互的单位, 英雄,npc,掉落物品等等

        /// <summary>
        /// 做go缓存的layer
        /// </summary>
        public Transform PoolLayer;

        public CWorldLayer(GameObject root)
        {
            Root = root;

            TerrainLayer = Root.transform.GetOrCreateChild("TerrainLayer");
            TerrainLayer.gameObject.layer = LayerMask.NameToLayer(LAYER_NAME_TERRAIN);

            UnitLayer = Root.transform.GetOrCreateChild("UnitLayer");
            UnitLayer.gameObject.layer = LayerMask.NameToLayer(LAYER_NAME_UNIT);

            PoolLayer = Root.transform.GetOrCreateChild("PoolLayer");
        }


        public void OnDestroy()
        {
            Root = null;
            TerrainLayer = null;
            UnitLayer = null;
            PoolLayer = null;
        }
    }
}

