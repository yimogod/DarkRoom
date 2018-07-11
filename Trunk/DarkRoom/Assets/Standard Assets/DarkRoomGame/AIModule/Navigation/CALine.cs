using System;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.AI
{
	/// <summary>
	/// 直线探测二维格子地图的算法实现
	/// 用于优化减少AStar调用的频率
	/// 如果直线经过的格子全部通过, 那我们就直接通行了.
	/// 没必要去调用AStar寻路
	///2017/03/30/v1 标准算法实现
	/// Author. liuzhibin
	/// </summary>
	public class CALine{
		/// <summary>
		/// 标志直线的形态
		/// </summary>
		enum Style{
			Vertical, //竖线
			Horizon, //横线
			Diagonal //斜线
		}

		/// <summary>
		/// 直线是否需要扩展到两条, 用于更精确的判定
		/// </summary>
		public float ExpandValue = 0;

		/// <summary>
		/// 我们用两点式直线
		/// 公式见: (x-x1)/(x2-x1)=(y-y1)/(y2-y1)
		/// start为1, end为2
		/// </summary>
		public Vector3Int StartPoint;
		public Vector3Int EndPoint;

		//直线形态
		private Style m_style = Style.Diagonal;
		//起始点和终点的差值
		private Vector3Int m_delta;

		/// 如果是横线或者竖线指代的值
		private int m_value = -1;

		/// <summary>
		/// 默认直线, 没有卵用. 需要调用SetData方法使其有效化
		/// </summary>
		public CALine(){
			m_style = Style.Diagonal;
		}

		/// <summary>
		/// 通过两点构造直线
		/// </summary>
		public CALine(int x1, int z1, int x2, int z2){
			SetData(x1, z1, x2, z2);
		}

		/// <summary>
		/// 直线是否平行y轴
		/// </summary>
		/// <value><c>true</c> if is vertical; otherwise, <c>false</c>.</value>
		public bool isVertical{
			get{ return m_style == Style.Vertical; }
		}

		/// <summary>
		/// 直线是否平行x轴
		/// </summary>
		public bool IsHorizonal{
			get{ return m_style == Style.Horizon; }
		}

		/// <summary>
		/// 斜率绝对值较大, 这样我们会循环y, 来求x
		/// </summary>
		public bool kBigger{
			get{ return Math.Abs(m_delta.z) >= Math.Abs(m_delta.x); }
		}

		/// <summary>
		/// 斜率绝对值娇小, 我们循环x, 来求y
		/// </summary>
		public bool kSmaller{
			get{ return Math.Abs(m_delta.z) < Math.Abs(m_delta.x); }
		}

		/// <summary>
		/// 通过设置两点来确认直线数据
		/// </summary>
		public void SetData(int x1, int z1, int x2, int z2){
			m_style = Style.Diagonal;
			m_delta.x = x2 - x1;
			m_delta.z = z2 - z1;

			if(kBigger){
				//我们的直线永远是从下到上, y增加的方向
				if(z1 > z2){
					int temp = x1;
					x1 = x2;
					x2 = temp;

					int temp2 = z1;
					z1 = z2;
					z2 = temp2;
				}

			}else{
				//我们的直线永远是左到右, x增加的防线
				//这样我们就可以放心的从start循环到end了
				if(x1 > x2){
					int temp = x1;
					x1 = x2;
					x2 = temp;

					int temp2 = z1;
					z1 = z2;
					z2 = temp2;
				}
			}

			StartPoint.x = x1;
			StartPoint.z = z1;
			EndPoint.x = x2;
			EndPoint.z = z2;

			if(x1 == x2){
				m_style = Style.Vertical;
				m_value = x1;
			}

			if(z1 == z2){
				m_style = Style.Horizon;
				m_value = z1;
			}
		}

		/// <summary>
		/// 根据z,计算在此直线的x值
		/// 如果是竖线,x永远是固定的
		/// </summary>
		/// <returns>The x.</returns>
		/// <param name="z">The z coordinate.</param>
		public float GetX(float z){
			if (isVertical)return m_value;

			float right = (z - StartPoint.z) / m_delta.z;
			float x = right * m_delta.x + StartPoint.x;
			return x;
		}

		/// <summary>
		/// 根据x,计算在此直线的z值
		/// 如果是横线,z永远是固定的
		/// </summary>
		/// <returns>The z.</returns>
		/// <param name="x">The x coordinate.</param>
		public float GetZ(float x){
			if(IsHorizonal)return m_value;

			float left = (x - StartPoint.x) / m_delta.x;
			float z = left * m_delta.z + StartPoint.z;
			return z;
		}

		/// <summary>
		/// 数据来源超大地图的直线寻路结果
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="startRow"></param>
		/// <param name="startCol"></param>
		/// <param name="endRow"></param>
		/// <param name="endCol"></param>
		/// <returns></returns>
		public bool FindPath(IWalkableGrid grid, int startCol, int startRow,  int endCol, int endRow)
		{
			SetData(startCol, startRow, endCol, endRow);
			return Find(grid);
		}

		/// <summary>
		/// 数据来源AStar 表格地图的直线寻路结果
		/// </summary>
		/// <returns><c>true</c>, if path was found, <c>false</c> otherwise.</returns>
		/// <param name="grid">Grid.</param>
		public bool FindPath(CMapGrid<CStarNode> grid){
		    CStarNode startNode = grid.StartNode;
		    CStarNode endNode = grid.EndNode;
			SetData(startNode.Col, startNode.Row, endNode.Col, endNode.Row);
			return Find(grid);
		}

		/// <summary>
		/// 真正进行寻路的函数
		/// 注意我们会加粗直线进行两次探测, 防止一些极端情况的可通过情况的错误
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		private bool Find(IWalkableGrid grid)
		{
			//竖线探测
			if (isVertical) {
				int start = StartPoint.z;
				int end = EndPoint.z;
				int col = m_value;
				for (int row = start; row <= end; row++) {
					bool b = grid.IsWalkable(col, row);
					if (!b) return false;
				}
				return true;
			}

			//横线探测
			if (IsHorizonal) {
				int start = StartPoint.x;
				int end = EndPoint.x;
				int row = m_value;
				for (int col = start; col <= end; col++) {
					bool b = grid.IsWalkable(col, row);
					if (!b) return false;
				}
				return true;
			}

			//注意斜线算法会有可能遗漏一些chunk, 所以我们指定规则. chunk必须在正方向上相连
			//这也符合数据定义
			//遍历y轴
			if (kBigger) {
				float start = (float)StartPoint.z;
				float end = (float)EndPoint.z;
				for (float row = start; row <= end; row += 0.5f) {
					float col = GetX(row);
					bool b = grid.IsWalkable((int)row, (int)(col - ExpandValue));
					if (!b) return false;

					//对y轴进行偏移再检测. 防止一些极端情况的可通过
					b = grid.IsWalkable((int)row, (int)(col + ExpandValue));
					if (!b) return false;
				}


				return true;
			}

			//遍历x轴
			if (kSmaller) {
				float start = (float)StartPoint.x;
				float end = (float)EndPoint.x;
				for (float col = start; col <= end; col += 0.5f) {
					float row = GetZ(col);
					bool b = grid.IsWalkable((int)(row - ExpandValue), (int)col);
					if (!b) return false;

					//对y轴进行偏移再检测. 防止一些极端情况的可通过
					b = grid.IsWalkable((int)(row + ExpandValue), (int)col);
					if (!b) return false;
				}
				return true;
			}

			return false;
		}
	}
}

