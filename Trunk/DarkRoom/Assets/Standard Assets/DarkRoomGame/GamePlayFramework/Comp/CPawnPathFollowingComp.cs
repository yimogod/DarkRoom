using DarkRoom.AI;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 跟随路径行走, 依赖于空间和移动组件
	/// 会在折点调用移动组件更改方向-->不要轻易改动这个设计
	/// </summary>
	[RequireComponent(typeof(CUnitSpacialComp))]
	[RequireComponent(typeof(CPawnMovementComp))]
	public class CPawnPathFollowingComp : MonoBehaviour
	{
		/// <summary>
		/// 行走完毕的回调代理原型
		/// </summary>
		public delegate void OnPathFinished(FinishPathResultType result);

		/// <summary>
		/// 跟随路径行走完毕的回调
		/// </summary>
		private OnPathFinished m_onPathFinishedCallBack;

		//本类的工作状态
		private PathFollowingStatus m_status = PathFollowingStatus.Idle;

		//正在行走的状态
		private FinishPathResultType m_result = FinishPathResultType.Default;


		//缓存空间组件
		private CUnitSpacialComp m_spacial;

		private CPawnMovementComp m_mover;

		//路径折点列表
		private CTilePathResult m_wayPoints;

		//之前折点的tile的索引坐标
		private Vector2Int m_lastStep;

		//当前折点的tile的索引坐标
		private Vector2Int m_currStep;

		//现在走到了第几个折点
		private int m_pathStepIndex = 0;

		//当前的位置和下一步目标点的距离,
		//如果距离在变大, 说明在远离, 那么就是到了
		private float m_lastDist;

		void Start()
		{
			m_spacial = GetComponent<CUnitSpacialComp>();
			m_mover = GetComponent<CPawnMovementComp>();
		}

		/// <summary>
		/// 请求跟随路径行走后, 我们每帧会更新这个状态
		/// 如果success就代表正常完成
		/// 当然也可以通过回调在完成的时候
		/// </summary>
		public FinishPathResultType FinishResult
		{
			get { return m_result; }
		}

		/// <summary>
		/// 组件目前的状态, 休息,暂停,等待,移动中
		/// </summary>
		public PathFollowingStatus Status
		{
			get { return m_status; }
		}

		/// <summary>
		/// 重置本组件的初始状态
		/// </summary>
		public void ResetFinishResult()
		{
			m_result = FinishPathResultType.Default;
			m_status = PathFollowingStatus.Idle;
		}

		/// <summary>
		/// 当前走向的阶段目标
		/// </summary>
		public Vector3 StepDestinationForNow
		{
			get { return CMapUtil.GetTileCenterPosByColRow(m_currStep); }
		}

		/// <summary>
		/// 当前运行的方向,归一化
		/// 这个是实时计算的
		/// </summary>
		public Vector3 ForceVector
		{
			get
			{
				if (m_status == PathFollowingStatus.Idle) return Vector3.zero;

				var delta = m_currStep - m_lastStep;
				Vector3 force = new Vector3(delta.x, 0, delta.y);
				//Vector3 now = m_spacial.localPosition;
				//Vector3 force = new Vector3(m_stepPos.x - now.x, 0, m_stepPos.z - now.z);
				return force.normalized;
			}
		}

		/// <summary>
		/// 请求移动
		/// </summary>
		public void RequestMove(CTilePathResult wayPoints, MoveType moveType)
		{
			m_mover.SetMoveType(moveType);
			m_wayPoints = wayPoints;
			m_result = FinishPathResultType.Default;
			m_status = PathFollowingStatus.Moving;

			m_pathStepIndex = wayPoints.StartIndex;
			m_currStep = wayPoints.StartPos;
			GotoNode(m_pathStepIndex);
		}

		/// <summary>
		/// 请求移动, 然后回调. 回调是在结算完成时调用
		/// 比如失败, 打断都会回调
		/// </summary>
		/// <param name="wayPoints">基于col, row的路径列表</param>
		/// <param name="finishCallBack">完成的回调</param>
		public void RequestMove(CTilePathResult wayPoints, OnPathFinished finishCallBack)
		{
			m_onPathFinishedCallBack = finishCallBack;
			RequestMove(wayPoints, MoveType.Direct);
		}

		/// <summary>
		/// 请求移动, 并且立刻结束
		/// </summary>
		public void RequestMoveWithImmediateFinish(FinishPathResultType result)
		{
			//先强行停止之前的移动
			if (m_status != PathFollowingStatus.Idle && m_onPathFinishedCallBack != null)
			{
				m_onPathFinishedCallBack(FinishPathResultType.Aborted);
			}

			//接着通知完成移动, 并告知完成的结果
			if (m_onPathFinishedCallBack != null)
			{
				m_onPathFinishedCallBack(result);
			}

			m_result = result;
		}

		/// <summary>
		/// 暂停移动, 比如说英雄技能挂起整个战场
		/// </summary>
		public void PauseMove()
		{
			m_status = PathFollowingStatus.Paused;
		}

		/// <summary>
		/// 外来原因中止移动
		/// </summary>
		public void AbortMove()
		{
			m_result = FinishPathResultType.Aborted;
			m_status = PathFollowingStatus.Idle;

			m_wayPoints = null;
			if (m_onPathFinishedCallBack != null)
			{
				m_onPathFinishedCallBack(FinishPathResultType.Aborted);
				m_onPathFinishedCallBack = null;
			}
		}

		/// <summary>
		/// 恢复移动. 确保之前是手动暂停过
		/// </summary>
		public void ResumeMove()
		{
			//我们需要确认是之前暂停了, 才能恢复现场
			if (m_status == PathFollowingStatus.Paused)
			{
				m_status = PathFollowingStatus.Moving;
			}
		}

		/// <summary>
		/// pos是否属于到达了目标位置
		/// acceptanceRadius 数据距离阀值, 单位米
		/// </summary>
		public bool HasReached(Vector3 goalPos, float acceptanceRadius)
		{
			Vector3 pos = m_spacial.localPosition;
			Vector3 delta = pos - goalPos;
			//TODO delta.y = 0;3d漫游不能设置为0

			acceptanceRadius *= acceptanceRadius;
			return Vector3.SqrMagnitude(delta) < acceptanceRadius;
		}

		/// 路径是由一系列的折点组成, 当更换折点时调用此函数
		private void OnSegmentFinished()
		{
		}

		void Update()
		{
			if (m_status != PathFollowingStatus.Moving)
			{
				return;
			}

			//走在下一个node的过程中, 行走又PawnMovementComp来驱动
			if (!m_arriveNode) return;

			//走到了终点
			if (m_pathStepIndex >= m_wayPoints.EndIndex)
			{
				Finish();
				return;
			}

			GotoNextNode();
		}

		private void Finish()
		{
			m_status = PathFollowingStatus.Idle;
			m_result = FinishPathResultType.Success;
			//跟随到了终点, 我们就停止了跟随
			m_mover.Stop();
			m_onPathFinishedCallBack?.Invoke(m_result);

			var dest = StepDestinationForNow;
			m_spacial.SetLocalPosInXZ(StepDestinationForNow);
			//Debug.Log("Path Follow Finished! At " + m_spacial.localPosition);

			m_wayPoints = null;
		}

		//每帧都会计算和判断是否到了下一个阶段性的目的地
		private bool m_arriveNode
		{
			get
			{
				//这里通过距离变小会有个隐藏问题
				//如果沿着圆的切线,也会先进后远
				Vector3 pos = m_spacial.localPosition;
				pos.y = 0;
				float dist = Vector3.SqrMagnitude(StepDestinationForNow - pos);

				//我们新距离1要小于m_lastDist 或则2 大于1米
				if (dist <= m_lastDist || dist > 1f)
				{
					m_lastDist = dist;
					return false;
				}

				m_lastDist = float.MaxValue;
				return true;
			}
		}

		//走向下一个折点
		private void GotoNextNode()
		{
			m_pathStepIndex += 1;
			GotoNode(m_pathStepIndex);
			OnSegmentFinished();
		}

		//基于node的行走方式
		public void GotoNode(int nodeIndex)
		{
			m_lastStep = m_currStep;
			m_lastDist = float.MaxValue;
			if (nodeIndex > m_wayPoints.EndIndex)
			{
				Finish();
				return;
			}

			//注意, 这里获取到路经点是格子的索引, 换算成需要换算成坐标
			m_currStep = m_wayPoints.WayPoints[nodeIndex];
			m_mover.Move(ForceVector);
		}

		void OnDestroy()
		{
			m_onPathFinishedCallBack = null;
		}
	}
}