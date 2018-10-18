using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sword
{

	public class UP_HeroAttrLabel : MonoBehaviour
	{
		public Text TitleTxt;
		public Text ValueTxt;

		public void SetContent(string title, string value){
			TitleTxt.text = title;
		}

		public void SetContent(string title, int value)
		{
			SetContent(title, value.ToString());
		}

		public void SetContent(string title, float value)
		{
			SetContent(title, value.ToString());
		}
	}
}