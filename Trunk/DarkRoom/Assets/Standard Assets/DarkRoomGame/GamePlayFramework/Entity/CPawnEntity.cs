using System;
using UnityEngine;
using DarkRoom.Core;

namespace DarkRoom.Game {
	/// <summary>
	/// mvc中的view, 作为视图方面的总代理, 在具体继承中会持有各个comp的引用
	/// 自身带有移动功能, 默认我们提供基于手柄的输入移动--类似于unreal的default pawn提供的基于FPS的移动
	/// 
	/// Pawn本身表示的是一个“能动”的概念，重点在于“能”。而Controller代表的是动到“哪里”的概念，重点在于“方向”。所以如果是一些
	/// Pawn本身固有的能力逻辑，如前进后退、播放动画、碰撞检测之类的就完全可以在Pawn内实现
	/// 
	/// 如果一个逻辑只属于某一类Pawn，那么其实你放进Pawn内也挺好。而如果一个逻辑可以应用于多个Pawn，
	/// 那么放进Controller就可以组合应用了
	/// </summary>
	
	[RequireComponent(typeof(CPawnPathFollowingComp))]
	[RequireComponent(typeof(CPawnMovementComp))]
	public class CPawnEntity : CUnitEntity
	{
		/// <summary>
		/// 谁在这时伤害了我
		/// </summary>
		[HideInInspector, NonSerialized]
		public CController Instigator;

		//控制pawn移动的基础组件
		protected CPawnMovementComp m_movement;

		//跟随路径行走的组件
		protected CPawnPathFollowingComp m_follower;

		/// <summary>
		/// 我的视力扇形区域
		/// 当然, 我们也会用来做其他的事情
		/// 比如临时设置一个半径(这个半径可能是一个特殊值, 比如攻击范围)
		/// 来帮助确认某个人是否在扇形内.
		/// </summary>
		protected CCircularSector m_viewSight;

		/// <summary>
		/// 控制pawn行走的组件
		/// </summary>
		/// <value>The mover.</value>
		public CPawnMovementComp Mover
		{
			get { return m_movement; }
		}

		public CPawnPathFollowingComp Follower
		{
			get { return m_follower; }
		}

		/// <summary>
		/// 我的视野范围是一个扇形
		/// field of view
		/// </summary>
		public CCircularSector FOV
		{
			get { return m_viewSight; }
		}

		/// <summary>
		/// 是否正在跟随路径行走, 实现 nav agent的接口
		/// </summary>
		public bool IsFollowingPath {
			get { return m_follower.Status == CPawnPathFollowingComp.PathFollowingStatus.Moving; }
		}

		/// <summary>
		/// 是否完成路径行走
		/// </summary>
		public bool FinishedFollowingPath {
			get { return m_follower.FinishResult == CPawnPathFollowingComp.FinishResultType.Success; }
		}

		protected override void RegisterAllComponents(){
			base.RegisterAllComponents();
			//初始化CPawnMovementComp
			m_movement = GetComponent<CPawnMovementComp>();
			m_follower = GetComponent<CPawnPathFollowingComp>();
		}

		/// <summary>
		/// 我看向point, 会设置空间组件的方向
		/// </summary>
		/// <param name="point"></param>
		public virtual void LookAt(Vector3 point) {
			m_spacial.LookAt(point);
			m_viewSight.LookAt(point);
        }

		/// <summary>
		/// 冻住pawn--停止声音,动画,物理,武器开火
		/// </summary>
		public virtual void TurnOff()
		{
			//暂停移动. 其他的需求在子类覆盖编写
			m_movement.TurnOff();
			m_follower.PauseMove();
			m_view.Pause();
		}

		/// <summary>
		/// 解冻单位
		/// </summary>
		public virtual void TurnOn() {
			//暂停移动. 其他的需求在子类覆盖编写
			m_movement.TurnOn();
            m_follower.ResumeMove();
			m_view.Resume();
		}

		/// <summary>
		/// 停止移动, 停止移动器和路径跟随器
		/// </summary>
		public virtual void StopMovement() {
			m_movement.Stop();
			m_follower.AbortMove();
		}

		/// <summary>
		/// 让单位频死, 子类重写该方法保证多出来的组件的关闭
		/// </summary>
		public virtual void MakeDying()
		{
			//死亡关闭移动
			m_movement.TurnOff();
			m_follower.AbortMove();

			m_dying = true;
		}

		/// <summary>
		/// 使复活
		/// </summary>
		public virtual void MakeRevival()
		{
			m_dying = false;
			m_dead = false;
		}

		/// <summary>
		/// 让单位死亡, 会在下一帧销毁, 子类重写该方法保证多出来的组件的关闭
		/// </summary>
		public virtual void MakeDead()
		{
			MakeDying();
			m_dead = true;
		}

		/// <summary>
		/// 让单位失效
		/// </summary>
		public virtual void MakeInvalid()
		{
			m_invalid = true;
		}

		/// <summary>
		/// 让单位有效化
		/// </summary>
		public virtual void MakeValid()
		{
			m_invalid = false;
		}

		/// <summary>
		/// 重启. 一般会被controller调用
		/// </summary>
		public virtual void Restart()
		{
			
		}

		protected override void Update() {
			base.Update();
			if (m_viewSight != null) {
				m_viewSight.SetCenter(LocalPosition);
			}
		}

		protected override void OnDestroy() {
			base.OnDestroy();

			m_movement = null;
		}
	}
}