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
			ValueTxt.text = value;
		}

		public void SetContent(string title, int value)
		{
			SetContent(title, value.ToString());
		}

		public void SetContent(string title, float value)
		{
			SetContent(title, (int)value);
		}

		public void SetContent(string title, int value1, int value2)
		{
			SetContent(title, $"{value1 / value2}");
		}

		public void SetContent(string title, float value1, float value2)
		{
			SetContent(title, $"{(int)value1} / {(int)value2}");
		}

		public void SetFloatContent(string title, float value)
		{
			SetContent(title, $"{value:F2}");
		}
	}
}