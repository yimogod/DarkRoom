using System;
using System.Collections.Generic;
using System.Linq;
using DarkRoom.Game;
using DarkRoom.UI;
using UnityEngine;
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
		public CLazyImage AvatarIcon;

		public override void OnReveal()
		{
			base.OnReveal();
			OnRaceClick(true);
		}

		protected override void OnBindEvent()
		{
			CreateCharacterBtn.onClick.AddListener(OnClickCreateBtn);
			BackEntryBtn.onClick.AddListener(OnClickEntryBtn);

			var races = RaceToggle.GetComponentsInChildren<Toggle>();
			foreach (var toggle in races)
			{
				toggle.onValueChanged.AddListener(OnRaceClick);
			}

			var classes = ClassToggle.GetComponentsInChildren<Toggle>();
			foreach (var toggle in classes)
			{
				toggle.onValueChanged.AddListener(OnClassClick);
			}
		}

		protected override void OnUnBindEvent()
		{
			CreateCharacterBtn.onClick.RemoveAllListeners();
			BackEntryBtn.onClick.RemoveAllListeners();

			var races = RaceToggle.GetComponentsInChildren<Toggle>();
			foreach (var toggle in races)
			{
				toggle.onValueChanged.RemoveAllListeners();
			}

			var classes = ClassToggle.GetComponentsInChildren<Toggle>();
			foreach (var toggle in classes)
			{
				toggle.onValueChanged.RemoveAllListeners();
			}
		}

		//点击单个种族
		private void OnRaceClick(bool v)
		{
			if(!v)return;
			var raceName = GetSelectedRaceName();
			var className = GetSelectedClassName();

            string address = AssetManager.GetHeroIconAddress(raceName, className);
            UnityEngine.Debug.LogWarning(address);
            AvatarIcon.LoadSprite(address);
        }

		//点击单个职业
		private void OnClassClick(bool v)
		{
			if (!v) return;
			var raceName = GetSelectedRaceName();
			var className = GetSelectedClassName();

            string address = AssetManager.GetHeroIconAddress(raceName, className);
            UnityEngine.Debug.LogWarning(address);
            AvatarIcon.LoadSprite(address);
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
			CApplicationManager.Instance.ChangeProcedure(Procedure_CharacterEntry.NAME);
		}

		private void OnClickCreateBtn()
		{
			bool b = ValidDectecte();
			if (!b)
			{
				Debug.LogError("Create Check Invalid");
				return;
			}

			var heroName = NameTxt.text;

			var raceName = GetSelectedRaceName();
			int raceValue = (int) SwordUtil.GetRaceEnum(raceName);

			var className = GetSelectedClassName();
			int classValue = (int) SwordUtil.GetClassEnum(className);

			//创建角色和英雄, 并存储数据
			ProxyPool.UserProxy.CreateCharacter(heroName, raceValue, classValue);
			ProxyPool.HeroProxy.CreateHero(heroName, raceValue, classValue);
			CApplicationManager.Instance.ChangeProcedure(Procedure_CharacterEntry.NAME);
		}

		private string GetSelectedRaceName()
		{
			var activeRaces = RaceToggle.ActiveToggles();
			var raceName = activeRaces.ElementAt(0).name;
			return raceName;
		}

		private string GetSelectedClassName()
		{
			var activesClasses = ClassToggle.ActiveToggles();
			var className = activesClasses.ElementAt(0).name;
			return className;
		}
	}
}