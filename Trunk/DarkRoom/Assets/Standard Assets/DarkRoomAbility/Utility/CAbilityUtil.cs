using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.GamePlayAbility {
	public class CAbilityUtil {
		public static CAbilityEnum.Location GetLocation(string str){
			if (string.IsNullOrEmpty(str)) {
				return CAbilityEnum.Location.CasterUnit;
			}

			if (string.Equals("CasterUnit", str))
				return CAbilityEnum.Location.CasterUnit;

			if (string.Equals("CasterPoint", str))
				return CAbilityEnum.Location.CasterPoint;

			if (string.Equals("TargetUnit", str))
				return CAbilityEnum.Location.TargetUnit;

			if (string.Equals("TargetPoint", str))
				return CAbilityEnum.Location.TargetPoint;

			if (string.Equals("TargetDirection", str))
				return CAbilityEnum.Location.TargetDirection;

			return CAbilityEnum.Location.CasterUnit;
		}

		public static CAbilityEnum.Order GetOrder(string str) {
			if (string.IsNullOrEmpty(str)) {
				return CAbilityEnum.Order.Invalid;
			}

			if (string.Equals("Move", str))
				return CAbilityEnum.Order.Move;
			if (string.Equals("StopMove", str))
				return CAbilityEnum.Order.StopMove;

			return CAbilityEnum.Order.Invalid;
		}


	}
}
