using UnityEngine;

namespace DarkRoom.Core
{
	/// <summary>
	/// 基于2d(xz)坐标的扇形, 参数使用的都是vector3, 但我们不考虑vector3的y值
	/// </summary>
	public class CCircularSector
	{
		//在xz平面的中心线, 以自身为圆心为0
		private Vector3 m_direction;

		//圆心坐标
		private Vector3 m_center;

		//扇形角度范围
		private float m_angle;

		//半径
		private float m_radius;

		//扇形的左右臂. 角度小的是开始
		private Vector3 m_startArm, m_endArm;
		//扇形中心线的角度
		private float m_centerAngle;

		/// <summary>
		/// 默认我们给了一个第一象限的扇形
		/// </summary>
		public CCircularSector()
		{
			m_angle = 90f;
			m_radius = 1f;
			m_center = Vector3.zero;
			m_direction = Vector3.one;
			m_startArm = new Vector3(1f, 0, 0);
			m_endArm = new Vector3(0, 0, 1f);
		}

		public CCircularSector(float angle, float radius) {
			m_center = Vector3.zero;
			m_direction = Vector3.one;

			m_angle = angle;
			m_radius = radius;
		}

		/// <summary>
		/// 设置扇形半径
		/// </summary>
		/// <param name="v"></param>
		public void SetRadius(float v)
		{
			m_radius = v;
		}

		/// <summary>
		/// 设置扇形的扇面角度
		/// </summary>
		/// <param name="v">角度</param>
		public void SetAngle(float v)
		{
			if (v < 0) {
				v += 360f;
				Debug.LogError("Notice circular sector angle range < 0");
			}
				
			m_angle = v;
		}

		/// <summary>
		/// 设置扇形的中心点坐标
		/// </summary>
		/// <param name="center"></param>
		public void SetCenter(Vector3 center)
		{
			m_center = center;
			m_center.y = 0;
		}

		/// <summary>
		/// 设置扇形中心线
		/// </summary>
		/// <param name="direction"></param>
		public void SetDirection(Vector3 direction)
		{
			m_direction = direction;
			m_direction.y = 0;
			m_centerAngle = CMathUtil.GetVec3AngleInXZ(m_direction);
		}

		/// <summary>
		/// 扇形中心线连向target
		/// </summary>
		/// <param name="target"></param>
		public void LookAt(Vector3 target)
		{
			SetDirection(target - m_center);
		}

		/// <summary>
		/// point是否在扇形范围内
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool ContainPoint(Vector3 point)
		{
			point.y = 0;
			Vector3 line = point - m_center;
			float dis = Vector3.SqrMagnitude(line) - m_radius * m_radius;

			//如果角度为0, 我们就只判断距离
			if (m_angle <= 0) {
				return dis <= m_radius * m_radius;
			}
			
			//距离太远在圆形外
			if (dis > 0) return false;

			float startAngle = m_centerAngle - m_angle * 0.5f;
			float endAngle = m_centerAngle + m_angle * 0.5f;

			float targetAngle = CMathUtil.GetVec3AngleInXZ(line);
			return (targetAngle > startAngle && targetAngle < endAngle) ;
		}
	}
}