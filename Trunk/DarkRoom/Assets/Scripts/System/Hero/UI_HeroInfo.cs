using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.UI;
using UnityEngine.UI;

namespace Sword {
	public class UI_HeroInfo : CUIWindowBase
	{
		public Text NameTxt;
		public Text LevelTxt;

		public UP_HeroAttrLabel RaceItem;
		public UP_HeroAttrLabel ClassItem;

		public UP_HeroAttrLabel StrengthItem;
		public UP_HeroAttrLabel DexterityItem;
		public UP_HeroAttrLabel IntellItem;
		public UP_HeroAttrLabel ConstitutionItem;
		public UP_HeroAttrLabel WillPowerItem;
		public UP_HeroAttrLabel CunningItem;


		public UP_HeroAttrLabel HealthItem;
		public UP_HeroAttrLabel ManaItem;
		public UP_HeroAttrLabel ViewRangeItem;
		public UP_HeroAttrLabel MoveRangeItem;
		public UP_HeroAttrLabel ExpItem;
		public UP_HeroAttrLabel NextLevelExpItem;

		private HeroVO m_vo;

		public override void OnReveal()
		{
			base.OnReveal();

			if(m_args == null){
				Debug.LogError("m_args should not be null when open UI_HeroInfo panel");
				return;
			}

			m_vo = m_args[0] as HeroVO;
			if (m_vo == null)
			{
				Debug.LogError("m_args should not be null when open UI_HeroInfo panel");
				return;
			}

			SetAttibute();
		}

		private void SetAttibute(){
			NameTxt.text = m_vo.Name;
			LevelTxt.text = m_vo.Level.ToString();

			RaceItem.SetContent("种族", m_vo.RaceEnum.ToString());
			ClassItem.SetContent("职业", m_vo.ClassEnum.ToString());

			var attr = m_vo.Entity.AttributeSet;
			StrengthItem.SetContent("力量", attr.Strength);
			DexterityItem.SetContent("敏捷", attr.Dexterity);
			IntellItem.SetContent("智力", attr.Intelligence);
			ConstitutionItem.SetContent("体质", attr.Constitution);
			WillPowerItem.SetContent("意志", attr.Willpower);
			CunningItem.SetContent("灵巧", attr.Cunning);

			HealthItem.SetContent("生命", attr.Health, attr.MaxHealth);
			ManaItem.SetContent("法力", attr.Mana, attr.MaxMana);
			ViewRangeItem.SetContent("视野", attr.ViewRange);
			MoveRangeItem.SetContent("移动", attr.MoveRange);
			ExpItem.SetContent("经验", attr.Exp);
			NextLevelExpItem.SetContent("下一级", attr.NextLevelExp);
		}
	}
}