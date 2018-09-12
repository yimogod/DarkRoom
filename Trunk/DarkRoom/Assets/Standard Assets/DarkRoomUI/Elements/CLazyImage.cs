using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkRoom.UI
{
	[RequireComponent(typeof(Image))]
	public class CLazyImage : MonoBehaviour
	{
		private Image m_img;

		void Awake()
		{
			m_img = gameObject.GetComponent<Image>();
		}

		public void LoadSprite(string address)
		{

		}
	}
}
