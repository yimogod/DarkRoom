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
			if (enumType <= 0) return prefab;

			switch (enumType)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					prefab = RandomSelectAddress("Decal/Bush_", 1, 9);
					break;
				case 6:
				case 7:
				case 8:
					prefab = RandomSelectAddress("Decal/Ground_Decal_", 1, 5);
					break;
				case 9:
				case 10:
					prefab = RandomSelectAddress("Decal/Rune_", 1, 6);
					break;
			}

			return prefab;
		}
	}
}