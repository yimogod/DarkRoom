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
		}

		private void SetAttibute(){
			NameTxt.text = m_vo.Name;
			LevelTxt.text = m_vo.Level.ToString();

			var entity = m_vo.Entity.AttributeSet;
			StrengthItem.SetContent("力量", m_vo.Strength);
			DexterityItem.SetContent("敏捷", m_vo.Strength);
			IntellItem.SetContent("智力", m_vo.Strength);
			ConstitutionItem.SetContent("体质", m_vo.Strength);
			WillPowerItem.SetContent("意志", m_vo.Strength);
			CunningItem.SetContent("灵巧", m_vo.Strength);

			HealthItem.SetContent("生命", m_vo.Strength);
			ManaItem.SetContent("法力", m_vo.Strength);
			ViewRangeItem.SetContent("视野", m_vo.Strength);
			MoveRangeItem.SetContent("移动", m_vo.Strength);
			ExpItem.SetContent("经验", m_vo.Strength);
			NextLevelExpItem.SetContent("下一级", m_vo.Strength);
		}
	}
}