using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 用来表示飞行物的抽象类
	/// 比如抛物线飞行的剑, 直线飞行的导弹, 从天而降的陨石等
	/// </summary>
	[RequireComponent(typeof(CMissleMovementComp))]
	public class CMissleEntity : CUnitEntity
	{
		/// <summary>
		/// 命中回调
		/// </summary>
		public Action<Vector3> OnImpact;

		/// <summary>
		/// 目标飞行的目标, 如果有的话做碰撞测试
		/// </summary>
		public CController FlyTarget;

		/// <summary>
		/// missile要飞往的位置
		/// 在追踪移动模式中, 此变量实时读取controller的位置
		/// 在直接飞行过程中, 此变量在初始化赋值了
		/// </summary>
		private Vector3 m_targetPos;

		//控制missle移动的基础组件
		protected CMissleMovementComp m_movement;

		private float m_hitTestDist = 0.5f;
		private float m_lastDist = float.MaxValue;

		/// <summary>
		/// 控制missle的移动组件
		/// </summary>
		/// <value>The mover.</value>
		public CMissleMovementComp Mover {
			get { return m_movement; }
		}

		protected override void RegisterAllComponents() {
			base.RegisterAllComponents();
			//初始化CPawnMovementComp
			m_movement = GetComponent<CMissleMovementComp>();
		}

		protected override void Update()
		{
			base.Update();
			if (FlyTarget != null) {
				m_targetPos = FlyTarget.LocalPosition;
				if (m_hitTest) {
					if (OnImpact != null) {
						OnImpact(m_spacial.localPosition);
					}
					GameObject.Destroy(gameObject);
				}
			}
		}

		private bool m_hitTest {
			get {
				//这里通过距离变小会有个隐藏问题
				//如果沿着圆的切线,也会先进后远
				Vector3 pos = m_spacial.localPosition;
				float dist = Vector3.SqrMagnitude(m_targetPos - pos);

				if (dist <= m_lastDist) {
					m_lastDist = dist;
					return false;
				}

				m_lastDist = float.MaxValue;
				return true;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			OnImpact = null;
			FlyTarget = null;
		}
	}
}
