﻿using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class HeroEntity : ActorEntity
	{
		protected override void PostRegisterAllComponents()
		{
			base.PostRegisterAllComponents();

			var go = new GameObject("MoveRange");
			go.transform.SetParent(transform);
			go.transform.localPosition = Vector3.zero;
			go.AddComponent<HeroMoveRange>();
		}
	}
}