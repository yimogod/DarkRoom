using System.Collections;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;
using DarkRoom.Utility;
using PureMVC.Patterns;

namespace Sword
{
	public class SelectLevelEntry : MonoBehaviour
	{
		/// <summary>
		/// 大关卡
		/// </summary>
		public int LevelZone;

		private Transform m_tran;

		// Use this for initialization
		void Start()
		{
			m_tran = transform;
		}

		// Update is called once per frame
		void Update()
		{
			if (!CMouseInput.Instance.HasClicked) return;

			var hit = CMouseInput.Instance.HitUnit;
			if (m_tran != hit) return;

			Debug.Log("click level " + name);
			Facade.instance.SendNotification(NotiConst.Open_LevelChoose);

			CApplicationManager.Instance.ChangeProcedure(Procedure_DungeonBattle.NAME);
		}
	}
}