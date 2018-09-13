using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkRoom.Utility;

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
            CResourceManager.LoadSprite(address, OnLoaded);
		}

        private void OnLoaded(Sprite sprite){
            m_img.sprite = sprite;
        }
	}
}
