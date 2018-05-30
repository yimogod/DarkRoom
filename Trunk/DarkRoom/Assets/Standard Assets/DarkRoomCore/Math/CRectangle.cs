using UnityEngine;

namespace DarkRoom.Core
{
	/// <summary>
	/// 自定义的带有旋转的xz平面的矩形
	/// </summary>
	public class CRectangle
	{
		private Vector3 m_a;
		private Vector3 m_b;
		private Vector3 m_c;
		private Vector3 m_d;

		//矩形面积
		private float m_area = -1;

		public CRectangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
		{
			m_a = p1;
			m_b = p2;
			m_c = p3;
			m_d = p4;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool pointInRect(Vector3 pt) {
			//Arectangle = 0.5⋅(yA − yC)⋅(xD−xB)+(yB−yD)⋅(xA−xC)
			//四边形面积公式
			if (m_area < 0) {
				m_area = 0.5f * Mathf.Abs(
					(m_a.y - m_c.y) * (m_d.x - m_b.x) +
					(m_b.y - m_d.y) * (m_a.x - m_c.x));
			}

			
			

			//Atriangle=0.5⋅(x1⋅(y2−y3)+x2⋅(y3−y1)+x3⋅(y1−y2))
			//三角形面积


			float abp = GetTriangleArea(m_a, m_b, pt);
            float bcp = GetTriangleArea(m_b, m_c, pt);
			float cdp = GetTriangleArea(m_c, m_d, pt);
			float dap = GetTriangleArea(m_d, m_a, pt);
			float area = abp + bcp + cdp + dap;

			return CMathUtil.FloatEqual(area, m_area, 0.1f);
		}

		private float GetTriangleArea(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			float area = //r[0][0] * (r[1][1] - r[2][1])
							p0.x * (p1.y - p2.y) +
							//+ r[1][0] * (r[2][1] - r[0][1])
							p1.x * (p2.y - p0.y) +
							//+ r[2][0] * (r[0][1] - r[1][1])
							p2.x * (p0.y - p1.y);
			return area * 0.5f;
		}
	}
}
