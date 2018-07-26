using System;
using System.Collections.Generic;

namespace DarkRoom.PCG
{

    public class CForestRoomMap : CProcedureGridBase<CForestRoomTileType>
    {
        public CForestRoomMap(int cols, int rows) : base(cols, rows)
        {
            m_map = new CForestRoomTileType[cols, rows];
        }
    }
}
