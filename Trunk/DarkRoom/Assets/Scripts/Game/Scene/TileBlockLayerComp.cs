using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.PCG;

namespace Sword
{
	/// <summary>
	/// 障碍物层
	/// </summary>
	public class TileBlockLayerComp : TilePropsLayerComp
	{
		protected override string GetPrefabBySubType(int enumType)
		{
			CForestBlockSubType subType = (CForestBlockSubType) enumType;
			string prefab = string.Empty;
			if (subType == CForestBlockSubType.None) return prefab;

			switch (subType)
			{
				case CForestBlockSubType.Tree:
					prefab = "Tree_01";
					break;
				case CForestBlockSubType.Rock1:
					prefab = "Rock_01";
					break;
				case CForestBlockSubType.Rock2:
					prefab = "Rock_02";
					break;
				case CForestBlockSubType.Plant1:
					prefab = "Rock_01";
					break;
				case CForestBlockSubType.Plant2:
					prefab = "Rock_02";
					break;
			}

			return prefab;
		}
	}
}