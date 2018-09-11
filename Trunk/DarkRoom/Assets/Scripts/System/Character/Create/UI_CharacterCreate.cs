using System;
using System.Collections.Generic;
using System.Linq;
using DarkRoom.Core;
using DarkRoom.Game;
using DarkRoom.UI;
using UnityEngine.UI;

namespace Sword
{
	public class UI_CharacterCreate : CUIWindowBase
	{
		/// <summary>
		/// 角色的名称
		/// </summary>
		public Text NameTxt;

		/// <summary>
		/// 创建角色游戏
		/// </summary>
		public Button CreateCharacterBtn;

		/// <summary>
		/// 种族选择
		/// </summary>
		public ToggleGroup RaceToggle;

		/// <summary>
		/// 职业选择
		/// </summary>
		public ToggleGroup ClassToggle;

		/// <summary>
		/// 形象选择
		/// </summary>
		public ToggleGroup AvatarToggle;

		public override void OnDisplay()
		{
			base.OnDisplay();
		}

		protected override void OnBindEvent()
		{
			CreateCharacterBtn.onClick.AddListener(OnClickCreateBtn);
		}

		protected override void OnUnBindEvent()
		{
			CreateCharacterBtn.onClick.RemoveAllListeners();
		}

		// 判断选项是否合法
		private bool ValidDectecte()
		{
			if (string.IsNullOrEmpty(NameTxt.text))
			{
				//TODO show tips 名字不能为空
				return false;
			}

			return true;
		}

		private void OnClickCreateBtn()
		{
			bool b = ValidDectecte();
			if(!b)return;

			var name = NameTxt.text;
			var activeRaces = RaceToggle.ActiveToggles();
			var raceName = activeRaces.ElementAt(0).name;
			int raceValue = (int)CStringEnum.Parse(typeof(ActorRace), raceName, true);

			var activesClasses = ClassToggle.ActiveToggles();
			var className = activesClasses.ElementAt(0).name;
			int classValue = (int)CStringEnum.Parse(typeof(ActorClass), className, true);

			ProxyPool.UserProxy.CreateCharacter(name, raceValue, classValue);
			ProxyPool.HeroProxy.CreateHero(name, raceValue, classValue);
			CApplicationManager.Instance.ChangeProcedure(CharacterEntry_Procedure.NAME);
		}
	}
}