using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 控制飞行物移动的组件
	/// 和 CUnitMovementComp不太一样
	/// </summary>
	[RequireComponent(typeof(CUnitSpacialComp))]
	public class CMissleMovementComp : MonoBehaviour
	{
		/// <summary>
		/// 移动方式, 是mover的直接移动， 还是由rvo驱动的移动
		/// 对于Unknow(默认值), 的意思是外部操控
		/// </summary>
		public enum MoveType {
			None,
			FlyTo, //在xz平面直线飞行
			ParaCurve, //抛物线
			DropTo, //石头一样砸下来
		}

		/// <summary>
		/// 基于秒/米的速率
		/// </summary>
		public float Speed = 10f;

		/// <summary>
		/// 物体在抛物线运动中的最高度
		/// </summary>
		public float Height = 10f;

		/// <summary>
		/// 是否根据行进方向进行旋转
		/// </summary>
		public bool Slerp = false;
		
		//抛物线中的加速度, 会根据高度和时间反算
		private float m_g = 9.8f * 4f;

		private MoveType m_type = MoveType.None;
		private CUnitSpacialComp m_spacial;

		//速度
		private Vector3 m_velocity;
		private Transform m_tran;

		void Awake()
		{
			m_spacial = GetComponent<CUnitSpacialComp>();
			m_velocity = Vector3.zero;
			m_tran = transform;
		}

		public void Launch(MoveType type, Vector3 end)
		{
			m_type = type;
			switch (m_type) {
				case MoveType.FlyTo:
					m_velocity = end - m_spacial.localPosition;
					m_velocity.y = 0;
					m_velocity = m_velocity.normalized * Speed;
					break;
				case MoveType.DropTo:
					m_velocity = Vector3.down * Speed;
                    break;
				case MoveType.ParaCurve:
					m_velocity = end - m_spacial.localPosition;
					m_velocity.y = 0;
					//优化抛物线
					float dis = Vector3.SqrMagnitude(m_velocity);
					if (dis < 225f) Height *= 0.4f;

					//Debug.Log("Cal dis is " + m_velocity);
					//物体抛物线运动的时间
					float t = m_velocity.magnitude / Speed;
					m_velocity = m_velocity.normalized * Speed;

					//Debug.Log("Cal Time is " + t);
					//跑到抛物线顶点需要一半时间
					t *= 0.5f;
					m_g = 2f * Height / (t * t);
					m_velocity.y = m_g * t;
                    //Debug.Log("init speed is " + m_velocity);
					break;
			}
        }

		void Update(){
			switch (m_type) {
				case MoveType.FlyTo:
					m_spacial.Translate(m_velocity * Time.deltaTime);
					RotateWithVelocity();
					break;
				case MoveType.DropTo:
					m_spacial.Translate(m_velocity * Time.deltaTime);
					break;
				case MoveType.ParaCurve:
					m_spacial.Translate(m_velocity * Time.deltaTime);
					m_velocity.y = m_velocity.y - m_g * Time.deltaTime;
					RotateWithVelocity();
					break;
			}
		}

		private void RotateWithVelocity(){
			if (!Slerp)return;
			m_spacial.SetDirection(m_velocity);

			Vector3 v = m_velocity;
			v.y = 0;
			float xz = Vector3.Magnitude(v);

			Vector3 euler = m_tran.localEulerAngles;
			euler.x = Mathf.Atan2(-m_velocity.y, xz) * Mathf.Rad2Deg;
			m_tran.localEulerAngles = euler;
		}
	}
}
