using System;
using System.Collections.Generic;

namespace DarkRoom.PCG {
    /// <summary>
    /// terrain用到的asset的索引
    /// </summary>
    public enum ForestTerrainAssetIndex
    {
        Grass1 = 0,
        Grass2,
        Land1,
        Land2,
        Wall,
        Pond,
        Road
    }

    /// <summary>
    /// terrain用到的tile的类型
    /// 需要注意的是目前这些类型和可通行性并没有关系. 看未来我们是否要用规则写死
    /// </summary>
    public enum ForestTerrainType
    {
        None,
        Floor,
        Wall,
        Pond,
        Road
    }

    public class CForestTerrain {
		
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
