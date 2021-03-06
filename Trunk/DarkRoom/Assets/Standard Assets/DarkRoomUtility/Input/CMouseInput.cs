using System;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkRoom.Utility
{
	public class CMouseEvent : CEvent
	{
		public static string TYPE = "CMouseClick";

		public Vector3 WorldPosition;

		public CMouseEvent() : base(TYPE)
		{
		}
	}

	public class CMouseInput : CSingleton<CMouseInput>
	{
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

		private CEventDispatcher m_dispatcher = new CEventDispatcher();
		private CMouseEvent m_clickEvent = new CMouseEvent();

		//鼠标按下点击的单位. 如果有的话
		private Transform m_hitUnit = null;

		private long m_lastDownTime = -1;

		//鼠标移动时, 经过的单位
		private Transform m_overUnit = null;

		public bool HasClicked => m_hasClicked;

		public bool HasDown => m_hasDown;

		public Vector3 MouseOverWorldPosition => m_overWorldPos;

		public Vector3 MouseDownWorldPosition => m_downWorldPos;

		public Transform HitUnit => m_hitUnit;

		public Transform OverUnit => m_overUnit;

		public void Active(bool value)
		{
			m_enabled = value;
		}

		public void AddClickListener(Action<CEvent> action)
		{
			m_dispatcher.AddEventListener(CMouseEvent.TYPE, action);
		}

		public void RemoveClickListener(Action<CEvent> action)
		{
			m_dispatcher.RemoveEventListener(CMouseEvent.TYPE, action);
		}

		public void Update()
		{
			m_overUnit = null;
			m_hasDown = false;

			//如果上一帧点击了, 那么本帧就清理对象
			if (m_hasClicked)
			{
				m_hasClicked = false;
				m_hitUnit = null;
			}

			if (!m_enabled) return;

			var cam = Camera.main;
			if (cam == null) return;

			//每帧我们都探测鼠标经过terrain的位置
			m_overWorldPos = Vector3.zero;
			Vector3 mousePos = Input.mousePosition;
			Ray ray = cam.ScreenPointToRay(mousePos);
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_TERRAIN);
			if (Physics.Raycast(ray, out hit, 1000f, layerMask))
			{
				m_overWorldPos = hit.point;
			}

			//探测我们是否经过了某个单位
			layerMask = 1 << LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_UNIT);
			if (Physics.Raycast(ray, out hit, 1000f, layerMask))
			{
				m_overUnit = hit.transform;
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (EventSystem.current != null)
				{
					bool clickUI = EventSystem.current.IsPointerOverGameObject();
					if (clickUI) return;
				}

				m_lastDownTime = CTimeUtil.GetCurrentMillSecondStamp();
				m_downScreenPos = mousePos;
				m_hasDown = true;
			}

			//鼠标松开
			if (Input.GetMouseButtonUp(0))
			{
				long delta = CTimeUtil.GetCurrentMillSecondStamp() - m_lastDownTime;
				if (delta > 200)return;

				m_hasClicked = true;

				//探测鼠标点击到的世界坐标和单位.
				if (m_overUnit != null)
				{
					m_hitUnit = m_overUnit;
					m_downWorldPos = m_hitUnit.position;
				}
				else
				{
					//如果没有点击到单位, 那么就直接赋值在terrain上的世界坐标
					m_downWorldPos = m_overWorldPos;
				}

				m_clickEvent.WorldPosition = m_downWorldPos;
				m_dispatcher.DispatchEvent(m_clickEvent);
			}
		}
	}
}