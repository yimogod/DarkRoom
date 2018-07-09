using DarkRoom.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkRoom.Utility
{
	public class CMouseInput{
		private static CMouseInput s_ins;

		//鼠标点击的col, row
		private int m_col;
		private int m_row;

		private bool m_enabled = true;
		private bool m_hasDown = false;
		private bool m_hasClicked = false;

		//鼠标按下的屏幕和世界坐标
		private Vector3 m_downScreenPos;
		private Vector3 m_downWorldPos;
		private Vector3 m_overWorldPos;

		//鼠标按下点击的单位. 如果有的话
		private Transform m_hitUnit = null;
		//鼠标移动时, 经过的单位
		private Transform m_overUnit = null;

		private CMouseInput(){}

		public static CMouseInput Instance {
			get{
				if (s_ins == null)s_ins = new CMouseInput();
				return s_ins;
			}
		}

		public void Activity(bool value){
			m_enabled = value;
		}

		public bool HasClicked{
			get { return m_hasClicked; }
		}

		public bool HasDown{
			get { return m_hasDown; }
		}

		public Vector3 MouseOverWorldPosition{
			get { return m_overWorldPos; }
		}

		public Vector3 MouseDownWorldPosition{
			get { return m_downWorldPos; }
		}

		public Transform HitUnit {
			get { return m_hitUnit; }
		}

		public Transform OverUnit{
			get { return m_overUnit; }
		}

		public void Update(){
			m_overUnit = null;
			m_hasDown = false;

			//如果上一帧点击了, 那么本帧就清理对象
			if (m_hasClicked){
				m_hasClicked = false;
				m_hitUnit = null;
			}
			if (!m_enabled)return;

			//每帧我们都探测鼠标经过terrain的位置
			m_overWorldPos = Vector3.zero;
			Vector3 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_TERRAIN);
			if (Physics.Raycast(ray, out hit, 1000f, layerMask)){
				m_overWorldPos = hit.point;
			}

			//探测我们是否经过了某个单位
			layerMask = 1 << LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_UNIT);
			if (Physics.Raycast(ray, out hit, 1000f, layerMask)){
				m_overUnit = hit.transform;
			}

			if (Input.GetMouseButtonDown(0)){
				if (EventSystem.current != null) {
					bool clickUI = EventSystem.current.IsPointerOverGameObject();
					if (clickUI) return;
				}

				m_downScreenPos = mousePos;
				m_hasDown = true;

				//探测鼠标点击到的世界坐标和单位.

				if (m_overUnit != null){
					m_hitUnit = m_overUnit;
					m_downWorldPos = m_hitUnit.position;
				} else{
					//如果没有点击到单位, 那么就直接赋值在terrain上的世界坐标
					m_downWorldPos = m_overWorldPos;
				}
			}

			//鼠标松开
			if (Input.GetMouseButtonUp(0)){
				m_hasClicked = true;
			}
		}
	}
}