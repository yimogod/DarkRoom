using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.PCG {
	public class CCave {
		/// <summary>
		/// 自动机地图
		/// </summary>
		public class CellularMap
		{
			//存储着自动机生成的数据 [x, z]
			private int[,] m_map;
			private int m_width;
			private int m_height;

			public int Width{ get { return m_width; }}
			public int Height { get { return m_height; } }

			public CellularMap(int[,] map)
			{
				m_map = map;
				m_width = m_map.GetLength(0);
				m_height = m_map.GetLength(1);
			}

			public CellularMap(int width, int height)
			{
				m_width = width;
				m_height = height;
				m_map = new int[width, height];
            }

			public void MakeAlive(int x, int z)
			{
				bool b = InMapRange(x, z);
				if (b) {
					m_map[x, z] = 1;
				}
			}

			/// <summary>
			/// 坐标是否在地图里面
			/// </summary>
			public bool InMapRange(int x, int z) {
				return x >= 0 && x < m_width && z >= 0 && z < m_height;
			}

			public int this[int x, int z]
			{
				get { return m_map[x, z]; }
				set { m_map[x, z] = value; }
			}

			public void Print(){
				CMapUtil.PrintGird(m_map);
			}
		}

		public struct Coord {
			public int tileX;
			public int tileZ;

			public Coord(int x, int z) {
				tileX = x;
				tileZ = z;
			}
		}

		public class Room : IComparable<Room> {
			public List<CCave.Coord> tiles;
			public List<CCave.Coord> edgeTiles;
			public List<Room> connectedRooms;
			public int roomSize;
			public bool isAccessibleFromMainRoom;
			public bool isMainRoom;

			public Room() {}

			public Room(List<CCave.Coord> roomTiles, CellularMap map) {
				tiles = roomTiles;
				roomSize = tiles.Count;
				connectedRooms = new List<Room>();

				edgeTiles = new List<CCave.Coord>();
				foreach (CCave.Coord tile in tiles) {
					for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
						for (int z = tile.tileZ - 1; z <= tile.tileZ + 1; z++) {
							bool b = map.InMapRange(x, z);
							if(!b)continue;

							if (x == tile.tileX || z == tile.tileZ) {
								if (map[x, z] == 1) {
									edgeTiles.Add(tile);
								}
							}
						}
					}
				}
			}

			public void SetAccessibleFromMainRoom() {
				if (!isAccessibleFromMainRoom) {
					isAccessibleFromMainRoom = true;
					foreach (Room connectedRoom in connectedRooms) {
						connectedRoom.SetAccessibleFromMainRoom();
					}
				}
			}

			public static void ConnectRooms(Room roomA, Room roomB) {
				if (roomA.isAccessibleFromMainRoom) {
					roomB.SetAccessibleFromMainRoom();
				} else if (roomB.isAccessibleFromMainRoom) {
					roomA.SetAccessibleFromMainRoom();
				}
				roomA.connectedRooms.Add(roomB);
				roomB.connectedRooms.Add(roomA);
			}

			public bool IsConnected(Room otherRoom) {
				return connectedRooms.Contains(otherRoom);
			}

			public int CompareTo(Room otherRoom) {
				return otherRoom.roomSize.CompareTo(roomSize);
			}
		}

		//end of class
	}
}
