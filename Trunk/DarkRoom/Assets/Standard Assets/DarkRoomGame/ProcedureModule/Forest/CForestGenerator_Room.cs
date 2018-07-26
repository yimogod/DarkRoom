using System.Collections.Generic;
using DarkRoom.Core;
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
        private List<CForestRoomData> m_roomList = new List<CForestRoomData>();

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

            //给房子们连路
            TunnelRooms();
        }

        /// <summary>
        /// 全地图开辟一个房间
        /// 开辟过程中会做20次尝试
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

            TryRoomAllocInRange(meta, new Vector2Int(0, 0), m_size);
        }

        /// <summary>
        /// 在地图外围1/3的区域开辟房间
        /// </summary>
        private void RoomAllocInEdge(int roomId)
        {
            if (roomId < 0) return;
            var meta = CForestRoomMetaManager.GetMeta(roomId.ToString());

            Vector2Int bottomLeft = new Vector2Int(0, 0);
            Vector2Int topRight = m_size;

            int edgeCols = Mathf.CeilToInt(m_size.x * 0.33f);
            int edgeRows = Mathf.CeilToInt(m_size.y * 0.33f);
            //45 90 135 在下半区域放房子
            if (EdgeEntrances == 1 || EdgeEntrances == 2 || EdgeEntrances == 3)
            {
                bottomLeft.x = 0;
                bottomLeft.y = 0;
                topRight.x = m_size.x;
                topRight.y = edgeRows;
            }
            //225, 270, 315 在上半区域放房子
            else if (EdgeEntrances == 4 || EdgeEntrances == 6 || EdgeEntrances == 7)
            {
                bottomLeft.x = 0;
                bottomLeft.y = m_size.y - edgeRows;
                topRight.x = m_size.x;
                topRight.y = m_size.y;
            }
            else if (EdgeEntrances == 0) //0 在左侧放房子
            {
                bottomLeft.x = 0;
                bottomLeft.y = 0;
                topRight.x = edgeCols;
                topRight.y = m_size.y;
            }
            else//剩余的在右侧放房子
            {
                bottomLeft.x = m_size.x - edgeCols;
                bottomLeft.y = 0;
                topRight.x = m_size.x;
                topRight.y = m_size.y;
            }

            TryRoomAllocInRange(meta, bottomLeft, topRight);
        }

        private void TryRoomAllocInRange(CForestRoomMeta meta, Vector2Int bottomLeft, Vector2Int topRight)
        {
            int tryNum = 20;
            while (tryNum > 0)
            {
                int sx = Random.Range(bottomLeft.x, topRight.x - meta.Size.x);
                int sy = Random.Range(bottomLeft.y, topRight.y - meta.Size.y);
                int ex = sx + meta.Size.x;
                int ey = sy + meta.Size.y;
                bool b = CanPlaceRoom(sx, sy, ex, ey);
                if (b) PlaceRoom(sx, sy, meta);

                tryNum--;
            }
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
            var room = new CForestRoomData(meta.Id, sx, sy);
            m_roomList.Add(room);
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

        /// <summary>
        /// 连接房子
        /// </summary>
        private void TunnelRooms()
        {
            if (m_roomList.Count == 0)return;

            for (int i = 0; i < m_roomList.Count - 1; i++)
            {
                if(Random.value > 0.5)continue;
                TunnelTwoRoom(m_roomList[i], m_roomList[i + 1]);
            }
        }

        private void TunnelTwoRoom(CForestRoomData a, CForestRoomData b)
        {
            bool v = CheckTunnelDoor(a, b);
            if (!v)return;


        }

        /// <summary>
        /// 根据两个房子的方向, 确认两个房子的连接路时的门
        /// </summary>
        private bool CheckTunnelDoor(CForestRoomData a, CForestRoomData b)
        {
            if (a.Meta.DoorPosList.Count == 0) return false;
            if (b.Meta.DoorPosList.Count == 0) return false;

            int minDis = int.MaxValue;
            int aIndex = -1;
            int bIndex = -1;
            for (int i = 0; i < a.Meta.DoorPosList.Count; i++)
            {
                Vector2Int ad = a.GetDoorPosition(i);
                for (int j = 0; j < b.Meta.DoorPosList.Count; j++)
                {
                    Vector2Int bd = b.GetDoorPosition(j);
                    int dis = ad.ManhattanMagnitude(bd);
                    if (dis < minDis)
                    {
                        minDis = dis;
                        aIndex = i;
                        bIndex = j;
                    }
                }
            }
            a.SetTempDoorForTunnel(aIndex);
            b.SetTempDoorForTunnel(bIndex);

            return true;
        }
    }
}
