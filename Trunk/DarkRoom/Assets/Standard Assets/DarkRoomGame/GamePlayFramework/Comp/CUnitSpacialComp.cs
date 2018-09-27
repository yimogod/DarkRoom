using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game{
	/// <summary>
	/// 描述单位空间位置和自身尺寸的组件. 对transform进行了简单的封装
	/// 我们值操作x,z坐标. y坐标用物理引擎来管理(或者不管理)
	/// </summary>
	public class CUnitSpacialComp : MonoBehaviour {
		public const int DIRECTION_LEFT = 2;
		public const int DIRECTION_RIGHT = 0;
		public const int DIRECTION_UP = 1;
		public const int DIRECTION_DOWN = 3;

		public const string LEFT = "left";
		public const string RIGHT = "right";
		public const string UP = "up";
		public const string DOWN = "down";


		//矢量的y值来表示绕y轴旋转的欧拉角
		private float m_rotationDegree = 0;

		private Vector3 m_direction = Vector3.right;

		//单位的投影面积
		private float m_widthInMeter = 1f; 
		private float m_lengthInMeter = 1f;
		private float m_radius = 0.5f;
		//我的体重
		private float m_weight = 1f;

		private Transform m_trans;
		private Vector3 m_lastPos;

		void Awake() {
			m_trans = transform;
		}

		/// <summary>
		/// transform的localPosition的代理
		/// </summary>
		public Vector3 localPosition {
			get { return m_trans.localPosition; }
		}

		/// <summary>
		/// transform的position的代理
		/// </summary>
		public Vector3 position{
			get{ return m_trans.position; }
		}

		/// <summary>
		/// 单位基于矢量的角度
		/// </summary>
		public Vector3 direction {
			get { return m_direction; }
		}

		/// <summary>
		/// 基于米的高度
		/// </summary>
		public virtual float Height {
			get { return 1f; }
		}

		/// <summary>
		/// 基于米身体宽度. 前胸到后背的距离
		/// </summary>
		public float Width {
			get { return m_widthInMeter; }
		}

		/// <summary>
		/// 基于米身体长度. 左胸到右胸的距离
		/// </summary>
		public float Length {
			get { return m_lengthInMeter; }
		}

		/// <summary>
		/// 如果单位是圆形, 那么就有半径
		/// </summary>
		public float Radius {
			get { return m_radius; }
		}

		/// <summary>
		/// 我的体重.可以当权重来使用
		/// </summary>
		public float Weight{
			get { return m_weight; }
		}

		public void SetPosY(float y){
			Vector3 local = localPosition;
			Vector3 pos = new Vector3(local.x, y, local.z);
			m_trans.localPosition = pos;
		}

		public void SetLocalPos(Vector3 value){
			m_lastPos = m_trans.localPosition;
			m_trans.localPosition = value;
		}

		public void SetLocalPosInXZ(Vector3 value){
			m_lastPos = m_trans.localPosition;
			value.y = m_lastPos.y;
			m_trans.localPosition = value;
		}

		/// <summary>
		/// 在xz平面方向上的矢量
		/// </summary>
		/// <param name="value">value的值是基于NDC空间的.0度旋转是vect3.right</param>
		public void SetDirection(Vector3 value){
			value.y = 0;
			if (CMathUtil.ZeroVector3(value))return;
			SetRotationByVelocity(value);
			m_trans.localEulerAngles = new Vector3(0, m_rotationDegree, 0);
		}

		/// <summary>
		/// 设定方向, 但不是一下转过去. 而是慢慢转过去
		/// </summary>
		public void SlerpDirection(Vector3 value)
		{
			SetRotationByVelocity(value);

			Vector3 old = m_trans.localEulerAngles;
			float dy = m_rotationDegree - old.y;
			if (Mathf.Abs(dy) < 5f)return;

			if (dy > 180f) dy -= 360f;
			if (dy < -180f) dy += 360f;

			old.y += dy * 0.1f;
			m_rotationDegree = old.y;
            m_trans.localEulerAngles = new Vector3(0, m_rotationDegree, 0);
		}

		//传入速度的方向. 设定m_direction的值
		private void SetRotationByVelocity(Vector3 value)
		{
			m_direction = value.normalized;
			//x接近0, 我们就不动
			if (Mathf.Abs(value.x) < 0.0001f) {
				//默认模型正北为0
				if (value.z > 0) m_rotationDegree = 0f;
				else if (value.z < 0) m_rotationDegree = 180f;
				else m_rotationDegree = 0;
				return;
			}
			//看清楚了. 这里是  <-value.z>
			//+90度的原因跟模型的0朝向有关系. 默认模型面向z轴正方向
			m_rotationDegree = Mathf.Atan2(-value.z, value.x) * Mathf.Rad2Deg + 90.0f;
		}

		/// <summary>
		/// 眼睛看向position这个位置
		/// </summary>
		/// <param name="position"></param>
		public void LookAt(Vector3 position)
		{
			Vector3 delta = position - localPosition;
			SetDirection(delta);
		}

		/// <summary>
		/// 设置单位的体形
		/// </summary>
		/// <param name="length">长</param>
		/// <param name="width">宽</param>
		public void SetBody(float length, float width, float radius, float weight){
			m_lengthInMeter = length;
			m_widthInMeter = width;
			m_radius = radius;
			m_weight = weight;
		}

		/// <summary>
		/// Transform的Translate代理接口
		/// </summary>
		/// <param name="translation"></param>
		public void Translate(Vector3 translation)
		{
			Vector3 p = localPosition;
			p.x += translation.x;
			p.y += translation.y;
			p.z += translation.z;
            SetLocalPos(p);
        }

		void OnDestroy(){
			m_trans = null;
		}
	}
}