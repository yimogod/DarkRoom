using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.Game
{
	public interface ICanSearchUnits {
		List<CController> SearchUnits(Vector3 center, float radius);
		List<CController> SearchUnits();
	}
}