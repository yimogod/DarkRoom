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
		/// 跟随路径行走完毕的结果
		/// </summary>
		public enum FinishResultType
		{
			/** nothing was happened */
			Default,

			/** Reached destination */
			Success,

			/** Movement was blocked */
			Blocked,

			/** Agent is not on path */
			OffPath,

			/** Aborted and stopped (failure) */
			Aborted,

			/** Request was invalid */
			Invalid,
		}

		/// <summary>
		/// 请求跟随路径的结果
		/// </summary>
		public enum RequestResultType
		{
			Failed,
			AlreadyAtGoal,
			RequestSuccessful
		}

		public enum PathFollowingStatus {
			/** No requests */
			Idle,

			/** Request with incomplete path, will start after UpdateMove() */
			Waiting,

			/** Request paused, will continue after ResumeMove() */
			Paused,

			/** Following path */
			Moving,
		}

		/// <summary>
		/// 行走完毕的回调代理原型
		/// </summary>
		public delegate void OnPathFinished(FinishResultType result);

		/// <summary>
		/// 跟随路径行走完毕的回调
		/// </summary>
		private OnPathFinished m_onPathFinishedCallBack;

		//本类的工作状态
		private PathFollowingStatus m_status = PathFollowingStatus.Idle;

		//正在行走的状态
		private FinishResultType m_result = FinishResultType.Default;
		

		//缓存空间组件
		private CUnitSpacialComp m_spacial;

		private CPawnMovementComp m_mover;

		//路径折点列表
		private CPathResult m_wayPoints;

		//现在走到了第几个折点
		private int m_pathStep = 0;

		//行走的下一个目的地, 也就是当前折点的坐标
		private Vector3 m_stepPos;

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
		public FinishResultType FinishResult {
			get { return m_result; }
		}

		/// <summary>
		/// 组件目前的状态, 休息,暂停,等待,移动中
		/// </summary>
		public PathFollowingStatus Status {
			get { return m_status; }
		}

		/// <summary>
		/// 重置本组件的初始状态
		/// </summary>
		public void ResetFinishResult() {
			m_result = FinishResultType.Default;
			m_status = PathFollowingStatus.Idle;
		}

		/// <summary>
		/// 当前走向的阶段目标
		/// </summary>
		public Vector3 TargetForNow {
			get { return m_stepPos; }
		}

		/// <summary>
		/// 当前运行的方向,归一化
		/// 这个是实时计算的
		/// </summary>
		public Vector3 ForceVector {
			get {
				if(m_status == PathFollowingStatus.Idle)return Vector3.zero;

				//TODO 3d漫游
				Vector3 force = m_stepPos - m_spacial.localPosition;
				force.y = 0;
				//Vector3 now = m_spacial.localPosition;
				//Vector3 force = new Vector3(m_stepPos.x - now.x, 0, m_stepPos.z - now.z);
				return force.normalized;
			}
		}

		/// <summary>
		/// 请求移动
		/// </summary>
		public void RequestMove(CPathResult wayPoints, CPawnMovementComp.MoveType moveType = CPawnMovementComp.MoveType.Direct)
		{
			m_mover.SetMoveType(moveType);
			m_wayPoints = wayPoints;
			m_result = FinishResultType.Default;
			m_status = PathFollowingStatus.Moving;

			m_pathStep = wayPoints.StartIndex;
			GotoNode(m_pathStep);
        }

		/// <summary>
		/// 请求移动, 然后回调. 回调是在结算完成时调用
		/// 比如失败, 打断都会回调
		/// </summary>
		/// <param name="wayPoints">基于col, row的路径列表</param>
		/// <param name="finishCallBack">完成的回调</param>
		public void RequestMove(CPathResult wayPoints, OnPathFinished finishCallBack)
		{
			m_onPathFinishedCallBack = finishCallBack;
			RequestMove(wayPoints);
		}

		/// <summary>
		/// 请求移动, 并且立刻结束
		/// </summary>
		public void RequestMoveWithImmediateFinish(FinishResultType result)
		{
			//先强行停止之前的移动
			if (m_status != PathFollowingStatus.Idle && m_onPathFinishedCallBack != null) {
				m_onPathFinishedCallBack(FinishResultType.Aborted);
            }

			//接着通知完成移动, 并告知完成的结果
			if (m_onPathFinishedCallBack != null) {
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
			m_result = FinishResultType.Aborted;
			m_status = PathFollowingStatus.Idle;

			m_wayPoints = null;
			if (m_onPathFinishedCallBack != null) {
				m_onPathFinishedCallBack(FinishResultType.Aborted);
				m_onPathFinishedCallBack = null;
			}
		}

		/// <summary>
		/// 恢复移动. 确保之前是手动暂停过
		/// </summary>
		public void ResumeMove()
		{
			//我们需要确认是之前暂停了, 才能恢复现场
			if (m_status == PathFollowingStatus.Paused) {
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
			if (m_status != PathFollowingStatus.Moving) {
				return;
			}

			//走在下一个node的过程中, 行走又PawnMovementComp来驱动
			if (!m_arriveNode) return;

			//走到了终点
			if (m_pathStep >= m_wayPoints.EndIndex) {
				Finish();
                return;
			}

			GotoNextNode();
		}

		private void Finish()
		{
			m_status = PathFollowingStatus.Idle;
			m_result = FinishResultType.Success;
			//跟随到了终点, 我们就停止了跟随
			m_mover.Stop();
			//Debug.Log("Path Follow Finished! At " + m_spacial.localPosition);

			m_wayPoints = null;
		}

		//每帧都会计算和判断是否到了下一个阶段性的目的地
		private bool m_arriveNode {
			get {
				//这里通过距离变小会有个隐藏问题
				//如果沿着圆的切线,也会先进后远
				Vector3 pos = m_spacial.localPosition;
				pos.y = 0;
                float dist = Vector3.SqrMagnitude(m_stepPos - pos);

				//TODO 3d漫游需要考虑y的数据
				//float dx = m_stepPos.x - pos.x;
				//float dz = m_stepPos.z - pos.z;
				//float dist = dx * dx + dz * dz;

				//我们新距离1要小于m_lastDist 或则2 大于1米
				if (dist <= m_lastDist || dist > 1f) {
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
			m_pathStep += 1;
			GotoNode(m_pathStep);
			OnSegmentFinished();
		}

		//基于node的行走方式
		public void GotoNode(int nodeIndex) {
			m_lastDist = float.MaxValue;
			if (nodeIndex > m_wayPoints.EndIndex) {
				Finish();
				return;
			}

			m_stepPos = m_wayPoints.WayPoints[nodeIndex];
			m_mover.Move(ForceVector);
		}

		void OnDestroy() {
			m_onPathFinishedCallBack = null;
		}
	}

}