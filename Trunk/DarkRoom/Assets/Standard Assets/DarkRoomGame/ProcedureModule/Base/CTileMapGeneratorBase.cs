using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.PCG{
	public class CTileMapGeneratorBase {
	    protected int m_numCols => m_grid.NumCols;

	    /// <summary>
        /// 地图高度
        /// </summary>
        protected int m_numRows => m_grid.NumRows;

		//存储地形类型, 辅助地形自动生成
        //walk存储可通行数据, 同时有type和asset字段
		protected CAssetGrid m_grid = new CAssetGrid();

        /// <summary>
        /// 存储的asset列表
        /// 比如对于细胞自动机
        /// 0代表可通行区域代表的资源
        /// 1, 2代表不可通行区域的两个资源
        /// 
        /// 比如对于森林
        /// 默认情况下
        /// 0, 1代表两种草的颜色
        /// 2, 3代表两种地面
        /// 4 代表凸起
        /// 
        /// 当然最重要的, 你可以自定义. 无视我的命名
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
        /// 默认的资源只在index对应的资源没有的时候才会替换.
        /// 为了容错处理添加的
        /// </summary>
	    public void SetDefaultAsset(string asset, bool walkable)
	    {
	        m_defaultAsset = asset;
	        m_defaultAssetWalkable = walkable;
	    }

		public void SetAsset(int index, string asset, bool walkable)
		{
			Debug.Log(index + "  " + asset);
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
		}

        /// <summary>
        /// 根据索引获取asset. 如果index不合法或者assets对应位置没有资源, 返回默认资源
        /// 注意asset虽然没有, 但其索引和对应的type还是之前的数据
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
        /// 注意asset虽然没有, 但其索引和对应的type还是之前的数据
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