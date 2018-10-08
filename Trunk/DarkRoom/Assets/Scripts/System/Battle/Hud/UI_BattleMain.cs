using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.UI;
using UnityEngine.UI;

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
	}
}
