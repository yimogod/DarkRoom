using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.PCG {
	public class CCave {
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

			public Room(List<CCave.Coord> roomTiles, CCellularGrid map) {
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
