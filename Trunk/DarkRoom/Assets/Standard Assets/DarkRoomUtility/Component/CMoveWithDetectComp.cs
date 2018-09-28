using DarkRoom.AI;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.Utility
{
	/// <summary>
	/// 在用键盘手柄控制移动的时候, 我们需要像坦克大战那样,前进的时候可以摩擦着过去障碍物
	/// </summary>
	[RequireComponent(typeof(CUnitSpacialComp))]
	[RequireComponent(typeof(CPawnMovementComp))]
	public class CMoveWithDetectComp : MonoBehaviour
	{
		private CPawnMovementComp m_mover;
		private CUnitSpacialComp m_spacial;

		void Awake()
		{
			m_mover = GetComponent<CPawnMovementComp>();
			m_spacial = GetComponent<CUnitSpacialComp>();
		}

		void Update()
		{
			if (!m_mover.IsMoving) return;

			Vector3 v = m_mover.Velocity * Time.deltaTime;
			Vector3 pos = m_spacial.localPosition + v;
			TryMove(m_spacial.localPosition, pos);
		}

		//我们移动到下一帧的时候, 探测能否过去, 有时候甚至需要顺着边缘挤过去
		private bool TryMove(Vector3 currPos, Vector3 pos)
		{
			Vector3 left, right;
			float halfLength = m_spacial.Length * 0.5f;
			float halfWidth = m_spacial.Width * 0.5f;

			//确认tile级的面向方向
			int directionConst = 0;
			float dx = Mathf.Abs(pos.x - currPos.x);
			float dz = Mathf.Abs(pos.z - currPos.z);
			if (dx > dz) {
				if (pos.x > currPos.x)
					directionConst = CUnitSpacialComp.DIRECTION_RIGHT;
				else
					directionConst = CUnitSpacialComp.DIRECTION_LEFT;
			} else {
				if (pos.z > currPos.z)
					directionConst = CUnitSpacialComp.DIRECTION_UP;
				else
					directionConst = CUnitSpacialComp.DIRECTION_DOWN;
			}


			if (directionConst == CUnitSpacialComp.DIRECTION_UP) {
				left = new Vector3(pos.x - halfLength, 0, pos.z + halfWidth);
				right = new Vector3(pos.x + halfLength, 0, pos.z + halfWidth);
			} else if (directionConst == CUnitSpacialComp.DIRECTION_DOWN) {
				left = new Vector3(pos.x + halfLength, 0, pos.z - halfWidth);
				right = new Vector3(pos.x - halfLength, 0, pos.z - halfWidth);
			} else if (directionConst == CUnitSpacialComp.DIRECTION_LEFT) {
				left = new Vector3(pos.x - halfWidth, 0, pos.z - halfLength);
				right = new Vector3(pos.x - halfWidth, 0, pos.z + halfLength);
			} else {
				left = new Vector3(pos.x + halfWidth, 0, pos.z + halfLength);
				right = new Vector3(pos.x + halfWidth, 0, pos.z - halfLength);
			}

		    Vector2Int leftTile = CMapUtil.GetTileByPos(left.x, left.z);
			bool canLeft = CTileNavigationSystem.Instance.IsWalkable(leftTile.x, leftTile.y);

		    Vector2Int rightTile = CMapUtil.GetTileByPos(right.x, right.z);
			bool canRight = CTileNavigationSystem.Instance.IsWalkable(rightTile.x, rightTile.y);

			//Debug.Log("canleft -----" + canLeft.ToString() + "----- left tile " + leftTile.ToString());
			//Debug.Log("canRight -----" + canRight.ToString() + "----- right tile " + rightTile.ToString());

			if (canLeft && canRight) return true;
			/* oh, no. sth is in front of me, 所以得停止前进 */
			if (!canLeft && !canRight)return false;

			/* 有一侧可以通过,绕着边缘, 挤过去 */
			float lowspeed = m_mover.MaxSpeed * 0.8f;
			Vector3 edge = Vector3.zero;
			float edgeRight, edgeLeft;
			if (directionConst == CUnitSpacialComp.DIRECTION_UP) {
				edge = new Vector3(-lowspeed, 0, 0);

				edgeLeft = (leftTile.x + 1.0f);
				edgeLeft = edgeLeft - left.x;
			} else if (directionConst == CUnitSpacialComp.DIRECTION_DOWN) {
				edge = new Vector3(lowspeed, 0, 0);

				edgeLeft = leftTile.x;
				edgeLeft = left.x - edgeLeft;

			} else if (directionConst == CUnitSpacialComp.DIRECTION_LEFT) {
				edge = new Vector3(0, 0, -lowspeed);

				edgeLeft = (leftTile.y + 1.0f);
				edgeLeft = edgeLeft - left.z;
			} else {
				edge = new Vector3(0, 0, lowspeed);

				edgeLeft = leftTile.y;
				edgeLeft = left.z - edgeLeft;
			}

			/* 左前方不能通行, */
			if (!canLeft && edgeLeft < 0.5f) {
				m_mover.Move(-edge, false);
				return true;
			}

			/* 右前方不能通行, 但 右边缘擦肩而过*/
			edgeRight = m_spacial.Length - edgeLeft;
			if (!canRight && edgeRight < 0.5f) {
				m_mover.Move(edge, false);
				return true;
			}

			//有一边不能通过, 但又挤不过去
			return false;
		}
	}
}