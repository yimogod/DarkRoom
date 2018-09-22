using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.PCG
{
	public enum CForestRoomTileType
	{
		None = -1,
		Floor, //可通行的地面 .
		Wall, //墙壁或者柱子 #
		Exit, //出口 !
		Door, //$
		InnerRoad, //房屋内部道路 ^
		OuterRoad, //连接房间的道路, 没有符号. 我们在算法里面使用
	}

	/// <summary>
	/// terrain用到的asset的索引, 通行性和规则是对应的
	/// </summary>
	public enum CForestTerrainSubType
	{
		Grass1 = 0,
		Grass2,
		Land1,
		Land2,
		Hill, //凸起的地形, 不可通行
		Pond, //池塘, 不可通行
		Road,
	}

	/// <summary>
	/// 森林里面障碍物的类型
	/// 在使用的时候, 可以接着随机使用不同的asset
	/// 本质上是6类, 每类可以是任何block
	/// 
	/// none说明没东西.但是否可通行不确定
	/// 默认是可通行, 但如果周边的格子有树, 就不可通行
	/// </summary>
	public enum CForestBlockSubType
	{
		None = 0,
		Tree1,
		Tree2,
		Rock1,
		Rock2,
		Plant1,
		Plant2,
	}

	public class CForestUtil
	{
		public static bool GetTerrainTypeWalkable(CForestTerrainSubType subType)
		{
			bool v = false;
			switch (subType)
			{
				case CForestTerrainSubType.Grass1:
				case CForestTerrainSubType.Grass2:
				case CForestTerrainSubType.Land1:
				case CForestTerrainSubType.Land2:
				case CForestTerrainSubType.Road:
					v = true;
					break;
				case CForestTerrainSubType.Hill:
				case CForestTerrainSubType.Pond:
					v = false;
					break;
			}

			return v;
		}

		public static bool CanPlaceTreeOnTerrainType(CForestTerrainSubType subType)
		{
			bool v = false;
			switch (subType)
			{
				case CForestTerrainSubType.Grass1:
				case CForestTerrainSubType.Grass2:
				case CForestTerrainSubType.Land1:
				case CForestTerrainSubType.Land2:
					v = true;
					break;
				case CForestTerrainSubType.Road:
				case CForestTerrainSubType.Hill:
				case CForestTerrainSubType.Pond:
					v = false;
					break;
			}

			return v;
		}
	}

	/// <summary>
	/// 森林开放房屋信息
	/// </summary>
	public class CForestRoomData
	{
		public string Id;

		/// <summary>
		/// 房屋左下角的位置
		/// </summary>
		public Vector2Int Pos;

		public CForestRoomMeta Meta => CForestRoomMetaManager.GetMeta(Id);

		public int NumCols => Meta.Size.x;
		public int NumRows => Meta.Size.y;

		/// <summary>
		/// 临时用的用于当前房屋tunnel的门位置
		/// </summary>
		public Vector2Int DoorForTunnel => Pos + Meta.DoorPosList[m_tempDoorIndexForTunnel];

		private int m_tempDoorIndexForTunnel = -1;

		public CForestRoomData(string id, int x, int y)
		{
			Id = id;
			Pos = new Vector2Int(x, y);
		}

		/// <summary>
		/// 
		/// </summary>
		public Vector2Int GetDoorPosition(int i)
		{
			return Pos + Meta.DoorPosList[i];
		}

		public void SetTempDoorForTunnel(int i)
		{
			m_tempDoorIndexForTunnel = i;
		}

		/// <summary>
		/// 内部坐标转换为地图坐标
		/// </summary>
		public Vector2Int GetTilePosition(int innerCol, int innerRow)
		{
			return new Vector2Int(innerCol, innerRow) + Pos;
		}
	}
}