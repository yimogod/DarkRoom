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
		public InputField NameTxt;

		/// <summary>
		/// 创建角色游戏
		/// </summary>
		public Button CreateCharacterBtn;

		/// <summary>
		/// 返回角色界面
		/// </summary>
		public Button BackEntryBtn;

		/// <summary>
		/// 种族选择
		/// </summary>
		public ToggleGroup RaceToggle;

		/// <summary>
		/// 职业选择
		/// </summary>
		public ToggleGroup ClassToggle;

		/// <summary>
		/// 形象显示
		/// </summary>
		public Image AvatarIcon;

		public override void OnReveal()
		{
			base.OnReveal();
		}

		protected override void OnBindEvent()
		{
			CreateCharacterBtn.onClick.AddListener(OnClickCreateBtn);
			BackEntryBtn.onClick.AddListener(OnClickEntryBtn);
		}

		protected override void OnUnBindEvent()
		{
			CreateCharacterBtn.onClick.RemoveAllListeners();
			BackEntryBtn.onClick.RemoveAllListeners();
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

		//返回角色入口
		private void OnClickEntryBtn()
		{
			CApplicationManager.Instance.ChangeProcedure(CharacterEntry_Procedure.NAME);
		}


		private void OnClickCreateBtn()
		{
			bool b = ValidDectecte();
			if (!b) return;

			var heroName = NameTxt.text;
			var activeRaces = RaceToggle.ActiveToggles();
			var raceName = activeRaces.ElementAt(0).name;
			int raceValue = (int) CStringEnum.Parse(typeof(ActorRace), raceName, true);

			var activesClasses = ClassToggle.ActiveToggles();
			var className = activesClasses.ElementAt(0).name;
			int classValue = (int) CStringEnum.Parse(typeof(ActorClass), className, true);

			ProxyPool.UserProxy.CreateCharacter(name, raceValue, classValue);
			ProxyPool.HeroProxy.CreateHero(heroName, raceValue, classValue);
			CApplicationManager.Instance.ChangeProcedure(CharacterEntry_Procedure.NAME);
		}
	}
}