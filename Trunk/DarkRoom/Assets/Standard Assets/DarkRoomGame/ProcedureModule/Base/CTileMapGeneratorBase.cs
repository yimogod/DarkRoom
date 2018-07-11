using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.PCG{
	public class CTileMapGeneratorBase : MonoBehaviour {
		/// <summary>
		/// 地图宽度
		/// </summary>
		public int Width;

		/// <summary>
		/// 地图高度
		/// </summary>
		public int Height;

		//存储地形类型, 辅助地形自动生成
		protected CAssetGrid m_grid = new CAssetGrid();

		public virtual void Generate(){
			m_grid.Init(Width, Height, true);

			int minX = CDarkRandom.Next(4, Width - 4);
			int minZ = CDarkRandom.Next(4, Height - 4);
			m_grid.SetValue(minZ, minX, -1);

			minX = CDarkRandom.Next(4, Width - 4);
			minZ = CDarkRandom.Next(4, Height - 4);
			m_grid.SetValue(minZ, minX, -1);
		}

		public CAssetGrid Grid {
			get { return m_grid; }
		}
	}
}