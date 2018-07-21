using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.PCG {
    public class CCaveRoom : IComparable<CCaveRoom>
    {
        public List<Vector2Int> tiles;
        public List<Vector2Int> edgeTiles;
        public List<CCaveRoom> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public CCaveRoom() { }

        public CCaveRoom(List<Vector2Int> roomTiles, CCellularGrid map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<CCaveRoom>();

            edgeTiles = new List<Vector2Int>();
            foreach (Vector2Int tile in tiles)
            {
                for (int x = tile.x - 1; x <= tile.x + 1; x++)
                {
                    for (int z = tile.y - 1; z <= tile.y + 1; z++)
                    {
                        bool b = map.InMapRange(x, z);
                        if (!b) continue;

                        if (x == tile.x || z == tile.y)
                        {
                            if (map[x, z] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach (CCaveRoom connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(CCaveRoom roomA, CCaveRoom roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(CCaveRoom otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(CCaveRoom otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}
