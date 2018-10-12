using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.UI;
using PureMVC.Patterns;
using UnityEngine.UI;

namespace Sword
{
	public class UI_BattleMain : CUIWindowBase
	{
		/// <summary>
		/// 进入游戏
		/// </summary>
		public Button HeroBtn;

		protected override void OnBindEvent()
		{
			HeroBtn.onClick.AddListener(OnClickHeroBtn);
		}

		protected override void OnUnBindEvent()
		{
			HeroBtn.onClick.RemoveAllListeners();
		}

		private void OnClickHeroBtn()
		{
			Facade.instance.SendNotification(NotiConst.Open_HeroInfo);
		}
	}
}