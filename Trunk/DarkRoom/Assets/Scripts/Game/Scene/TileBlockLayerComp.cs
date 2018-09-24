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
					prefab = RandomSelectAddress("Block/Tree_", 1, 9);
					break;
				case CForestBlockSubType.Rock:
					prefab = RandomSelectAddress("Block/Rock_", 1, 7);
					break;
				case CForestBlockSubType.Plant:
					prefab = RandomSelectAddress("Block/Tree_Small_", 1, 5);
					break;
				case CForestBlockSubType.Prop1:
					prefab = RandomSelectAddress("Block/Bones_", 1, 5);
					break;
				case CForestBlockSubType.Prop2:
					prefab = RandomSelectAddress("Block/Tree_Dead_", 1, 3);
					break;
				case CForestBlockSubType.Prop3:
					prefab = RandomSelectAddress("Block/Tree_Dead_", 1, 3);
					break;
				case CForestBlockSubType.Prop4:
					prefab = RandomSelectAddress("Block/Tree_Pine_", 1, 5);
					break;
				case CForestBlockSubType.Prop5:
					prefab = RandomSelectAddress("Block/Tree_Pine_", 1, 5);
					break;
			}

			Debug.Log(prefab);
			return prefab;
		}

		private string RandomSelectAddress(string prefix, int start, int end){
			int index = Random.Range(start, end + 1);
			if (index < 10) return $"{prefix}0{index}";
			return $"{prefix}{index}";
		}
	}
}