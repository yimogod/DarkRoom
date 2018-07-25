using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DarkRoom.PCG
{
    public class CForestGenerator_Room
    {
        /// <summary>
        /// 入口要在地图边缘的方位
        /// -1代表不需要在边缘
        /// 正x轴为0, 逆时针增加
        /// </summary>
        public int EdgeEntrances = -1;

        /// <summary>
        /// 路尽头的房子来一个
        /// </summary>
        public int EndRoadRoomId = -1;

        /// <summary>
        /// 地图中需要的房子
        /// </summary>
        public List<int> RequiredRoomsId = new List<int>();

        /// <summary>
        /// 随机房子的种子
        /// </summary>
        public List<int> RandomRoomSeeds = new List<int>();

        /// <summary>
        /// 随机房子的个数
        /// </summary>
        public int RandomRoomNum = 0;

        private Vector2Int m_size;
        private CForestRoomTileData[,] m_roomMap;

        public void Generate(int cols, int rows)
        {
            m_roomMap = new CForestRoomTileData[cols, rows];

            //开辟路尽头房子
            RoomAllocInEdge(EndRoadRoomId);

            //开辟指定的房子
            foreach (int id in RequiredRoomsId)
            {
                RoomAlloc(id);
            }

            //开辟随机房子
            int tryNum = Mathf.FloorToInt(RandomRoomNum * 1.5f);
            tryNum = Random.Range(0, tryNum);
            for (int i = 0; i < tryNum; i++)
            {
                int id = Random.Range(0, RandomRoomSeeds.Count);
                RoomAlloc(id);
            }
        }

        /// <summary>
        /// 全地图开辟一个房间
        /// 开辟过程中会做4次尝试
        /// 如果开辟失败就不开辟
        /// </summary>
        private void RoomAlloc(int roomId)
        {
            if(roomId < 0)return;
            var meta = CForestRoomMetaManager.GetMeta(roomId.ToString());

            if (meta.HasPreferLocation)
            {
                PreferLocation(roomId);
                return;
            }

            int tryNum = 10;
            while (tryNum > 0)
            {
                int sx = Random.Range(0, m_size.x - meta.Size.x);
                int sy = Random.Range(0, m_size.y - meta.Size.y);
                int ex = sx + meta.Size.x;
                int ey = sy + meta.Size.y;
                bool b = CanPlaceRoom(sx, sy, ex, ey);
                if (b) PlaceRoom(sx, sy, meta);

                tryNum--;
            }
        }

        /// <summary>
        /// 在地图周边开辟房间
        /// </summary>
        private void RoomAllocInEdge(int roomId)
        {

        }

        //这个矩形内是否有其他的房子
        //我们假定ex,ey肯定大于sxsy
        private bool CanPlaceRoom(int sx, int sy, int ex, int ey)
        {
            for (int x = sx; x < ex; x++)
            {
                for (int y = sy; y < ey; y++)
                {
                    var tile = m_roomMap[x, y];
                    if (tile != null) return false;
                }
            }

            return true;
        }

        private void PlaceRoom(int sx, int sy, CForestRoomMeta meta)
        {
            for (int x = 0; x < meta.Size.x; x++)
            {
                for (int y = 0; y < meta.Size.y; y++)
                {
                    var tile = new CForestRoomTileData();
                    tile.Id = meta.Id;
                    tile.TileType = meta.GetSpot(x, y);

                    m_roomMap[sx + x, sy + y] = tile;
                }
            }
        }

        /// <summary>
        /// 我们期望房子开辟在这里, 所以会覆盖所有的其他数据
        /// </summary>
        private void PreferLocation(int roomId)
        {
            if (roomId < 0) return;
            var meta = CForestRoomMetaManager.GetMeta(roomId.ToString());
            if (!meta.HasPreferLocation)return;

            int sx = meta.PreferLocation.x;
            int sy = meta.PreferLocation.y;
            int ex = sx + meta.Size.x;
            int ey = sy + meta.Size.y;

            if (sx < 0 || sy < 0)
            {
                Debug.LogError("PreferLocation Must Bigger Than 0!!");
                return;
            }

            if (ex >= m_size.x || ey >= m_size.y)
            {
                Debug.LogError("PreferLocation + RoomSize Must Smaller Than MapSize!!");
                return;
            }

            PlaceRoom(sx, sy, meta);
        }

        /// <summary>
        /// 在地图边缘做个梯子
        /// </summary>
        private void MakeStairsSides()
        {

        }

        private void MakeStairsInside()
        {

        }
    }
}
