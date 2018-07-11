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

		public CAssetGrid Grid {
			get { return m_grid; }
		}
	}
}