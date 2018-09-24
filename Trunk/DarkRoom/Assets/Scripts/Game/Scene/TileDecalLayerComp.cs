using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.PCG;

namespace Sword
{
	/// <summary>
	/// 贴花和小装饰物层
	/// </summary>
	public class TileDecalLayerComp : TilePropsLayerComp
	{
		protected override string GetPrefabBySubType(int enumType)
		{
			string prefab = string.Empty;
			CForestBlockSubType subType = (CForestBlockSubType) enumType;

			if (subType == CForestBlockSubType.None) return prefab;

			switch (subType)
			{
				case CForestBlockSubType.Tree:
					prefab = "Tree_01";
					break;
	
			}

			return prefab;
		}
	}
}