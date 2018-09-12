using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sword
{
	public class UP_ActorBaseInfo : MonoBehaviour
	{
		public Text NameTxt;
		public Text LevelTxt;
		public Text RaceTxt;
		public Text ClassTxt;

		public void SetData(ActorVO vo)
		{
			if (vo == null) return;

			NameTxt.text = vo.Name;
			LevelTxt.text = vo.Level.ToString();
			RaceTxt.text = SwordUtil.GetRaceName(vo.RaceEnum);
			ClassTxt.text = SwordUtil.GetClassName(vo.ClassEnum);
		}
	}
}