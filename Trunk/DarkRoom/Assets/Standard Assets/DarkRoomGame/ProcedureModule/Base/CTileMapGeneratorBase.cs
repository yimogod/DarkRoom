﻿using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.PCG{
	public class CTileMapGeneratorBase : MonoBehaviour {
		protected int m_numCols;

        /// <summary>
        /// 地图高度
        /// </summary>
        protected int m_numRows;

		//存储地形类型, 辅助地形自动生成
        //walk存储可通行数据, 同时有type和asset字段
		protected CAssetGrid m_grid = new CAssetGrid();

	    /// <summary>
	    /// 存储的asset列表
	    /// 比如对于细胞自动机
	    /// 0代表可通行区域代表的资源
	    /// 1, 2代表不可通行区域的两个资源
	    /// 
	    /// 比如对于柏林模糊
	    /// 默认情况下
	    /// 0, 1代表两种海的颜色
	    /// 2, 3代表海岸线
	    /// 4, 5, 6代表草地, 其中4代表默认的绿地
	    /// 7, 8 两种地面
	    /// 9, 10两种石头地面
	    /// </summary>
	    protected string[] m_assetList;

	    /// <summary>
	    /// 跟assets 一一对应的可通行性
	    /// </summary>
	    protected bool[] m_walkableList;

	    protected string m_defaultAsset;
	    protected bool m_defaultAssetWalkable;

        protected int m_maxAssetsNum = 11;

	    public CAssetGrid Grid => m_grid;

	    /// <summary>
        /// 设置默认的资源和可通行性
        /// </summary>
	    public void SetDefaultAsset(string asset, bool walkable)
	    {
	        m_defaultAsset = asset;
	        m_defaultAssetWalkable = walkable;
	    }

		public void SetAsset(int index, string asset, bool walkable)
		{
		    if (m_assetList.Length > m_maxAssetsNum)
		    {
                Debug.LogErrorFormat("{0} bigger than max assets num. max assets num is {1}", asset, m_maxAssetsNum);
		        return;
		    }

			m_assetList[index] = asset;
			m_walkableList[index] = walkable;
		}

		/// <summary>
		/// 初始化gird全部可以通行
		/// </summary>
		public virtual void Generate(){
			m_grid.Init(m_numCols, m_numRows, true);

			/*int minX = CDarkRandom.Next(4, m_numCols - 4);
			int minZ = CDarkRandom.Next(4, m_numRows - 4);
			m_grid.SetNodeType(minZ, minX, -1);

			minX = CDarkRandom.Next(4, m_numCols - 4);
			minZ = CDarkRandom.Next(4, m_numRows - 4);
			m_grid.SetNodeType(minZ, minX, -1);*/
		}

        /// <summary>
        /// 根据索引获取asset. 如果index不合法或者assets对应位置没有资源, 返货默认资源
        /// </summary>
	    protected string GetAsset(int index)
	    {
	        if (index < 0 || index >= m_assetList.Length) return m_defaultAsset;
	        string asset = m_assetList[index];
	        if (string.IsNullOrEmpty(asset)) return m_defaultAsset;
	        return asset;
	    }

        /// <summary>
        /// 返回对应的asset的可通行
        /// </summary>
	    protected bool GetAssetWalkable(int index)
	    {
	        if (index < 0 || index >= m_assetList.Length) return m_defaultAssetWalkable;
	        string asset = m_assetList[index];
	        if (string.IsNullOrEmpty(asset)) return m_defaultAssetWalkable;
	        return m_walkableList[index];
	    }
    }
}