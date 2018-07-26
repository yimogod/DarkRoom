using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.PCG {
    /// <summary>
    /// terrain用到的asset的索引, 通行性和规则是对应的
    /// </summary>
    public enum ForestTerrainSubType
    {
        Grass1,
        Grass2,
        Land1,
        Land2,
        Hill, //凸起的地形, 不可通行
        Floor, //屋子的地板
        Wall, //屋子的墙, 不可通行
        Pond, //池塘, 不可通行
        Road,
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
    }

    /// <summary>
    /// 开放式房屋的地块信息, 辅助terrain的数据生成
    /// </summary>
    public class CForestRoomTileData
    {
        public string Id;
        //只有在出口的地方才可以开门
        public CForestRoomMeta.TileType TileType;

        public bool IsValid => string.IsNullOrEmpty(Id);
        public bool CanOpen => TileType == CForestRoomMeta.TileType.Exit;

        public CForestRoomTileData()
        {
            Id = "";
            TileType = CForestRoomMeta.TileType.None;
        }
    }
}
