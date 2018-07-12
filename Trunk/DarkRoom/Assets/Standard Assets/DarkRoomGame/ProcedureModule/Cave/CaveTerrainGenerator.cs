using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG{
	/// <summary>
	/// 用细胞自动机来产生洞穴/森林地形. 产生的死亡地形为树/山这些不可通行的地方
	/// 另外我们的地图都64x64起
	/// 为什么要强调64x64, 因为如果地图小于64的话, 细胞自动机随机的不够, 不能够产生合理数据
	/// 
	/// 注意这里仅仅处理数据
	/// </summary>
	[RequireComponent(typeof(CCellularAutomaton))]
	public class CaveTerrainGenerator : MonoBehaviour
	{
		/// <summary>
		/// 封闭区域的格子数小于这个我们就删除这个封闭区域
		/// </summary>
		public int CloseReginThreshold = 50;

		//存储着自动机生成的数据 [x, y]
		private CCellularGrid m_map;

		private CFloodFill m_floodFill = new CFloodFill();

		//本地图中的房子
		private List<CCave.Room> m_survivingRooms = new List<CCave.Room>();

		/// <summary>
		/// 获取细胞自动机处理过的地图
		/// 死亡为不可通行
		/// </summary>
		public CCellularGrid Map { get { return m_map; } }

		/// <summary>
		/// 获取本地图中的屋子
		/// </summary>
		public List<CCave.Room> Rooms{ get { return m_survivingRooms; } }

		void Start()
		{
		}

		public void Generate(int numCols, int numRows)
		{
			m_map = new CCellularGrid(numCols, numRows);
		    m_map.Generate();

            ProcessRegion();
			ConnectClosestRooms(m_survivingRooms);
		}

		private void ProcessRegion()
		{
			//获取死亡的细胞组成的封闭区域
			ProcessLittleRegion(CloseReginThreshold, 0, 1, null);

			//保存下来的大的region
			List<List<CCave.Coord>> roomRegion = new List<List<CCave.Coord>>();
			//获取生存的细胞形成的封闭区域, 也叫room
			ProcessLittleRegion(CloseReginThreshold, 1, 0, roomRegion);

			m_survivingRooms.Clear();
			foreach (var item in roomRegion) {
				m_survivingRooms.Add(new CCave.Room(item, m_map));
            }
		}

		/// <summary>
		/// 抹去太小的封闭区域
		/// </summary>
		/// <param name="threshold">大小的阀值</param>
		/// <param name="little">小区域的格子的值</param>
		/// <param name="large">抹去的小区域的赋值</param>
		/// <param name="largeRegion">没有被抹去的大的区域</param>
		private void ProcessLittleRegion(int threshold, 
			int little, int large, List<List<CCave.Coord>> largeRegion)
		{
			List<List<CCave.Coord>> roomRegions = m_floodFill.GetRegions(m_map, little);
			foreach (List<CCave.Coord> roomRegion in roomRegions) {
				//如果区域太小, 就抹去
				if (roomRegion.Count < threshold) {
					foreach (CCave.Coord tile in roomRegion) {
						m_map[tile.tileX, tile.tileZ] = large;
					}
					continue;
				}

				if (largeRegion != null) {
					largeRegion.Add(roomRegion);
				}
			}
		}

		//连接相邻的距离最近的room, 并且在相邻的房子中间创建走路
		private void ConnectClosestRooms(List<CCave.Room> allRooms, bool forceAccessibilityFromMainRoom = false)
		{
			m_survivingRooms.Sort();
			m_survivingRooms[0].isMainRoom = true;
			m_survivingRooms[0].isAccessibleFromMainRoom = true;


			List<CCave.Room> roomListA = new List<CCave.Room>();
			List<CCave.Room> roomListB = new List<CCave.Room>();

			if (forceAccessibilityFromMainRoom) {
				foreach (CCave.Room room in allRooms) {
					if (room.isAccessibleFromMainRoom) {
						roomListB.Add(room);
					} else {
						roomListA.Add(room);
					}
				}
			} else {
				roomListA = allRooms;
				roomListB = allRooms;
			}

			int bestDistance = 0;
			CCave.Coord bestTileA = new CCave.Coord();
			CCave.Coord bestTileB = new CCave.Coord();
			CCave.Room bestRoomA = new CCave.Room();
			CCave.Room bestRoomB = new CCave.Room();
			bool possibleConnectionFound = false;

			foreach (CCave.Room roomA in roomListA) {
				if (!forceAccessibilityFromMainRoom) {
					possibleConnectionFound = false;
					if (roomA.connectedRooms.Count > 0) {
						continue;
					}
				}

				foreach (CCave.Room roomB in roomListB) {
					if (roomA == roomB || roomA.IsConnected(roomB)) {
						continue;
					}

					for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
						for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
							CCave.Coord tileA = roomA.edgeTiles[tileIndexA];
							CCave.Coord tileB = roomB.edgeTiles[tileIndexB];
							int distanceBetweenRooms =
								(int) (Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileZ - tileB.tileZ, 2));

							if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
								bestDistance = distanceBetweenRooms;
								possibleConnectionFound = true;
								bestTileA = tileA;
								bestTileB = tileB;
								bestRoomA = roomA;
								bestRoomB = roomB;
							}
						}
					}
				}
				if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
					CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
				}
			}

			if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
				ConnectClosestRooms(allRooms, true);
			}

			if (!forceAccessibilityFromMainRoom) {
				ConnectClosestRooms(allRooms, true);
			}
		}

		//在roomA和roomB之间创建走廊
		private void CreatePassage(CCave.Room roomA, CCave.Room roomB, 
			CCave.Coord tileA, CCave.Coord tileB){

			CCave.Room.ConnectRooms(roomA, roomB);
			List<CCave.Coord> line = GetLine(tileA, tileB);
			foreach (CCave.Coord c in line) {
				DrawCircle(c, 5);
			}
		}

		private void DrawCircle(CCave.Coord c, int r)
		{
			for (int x = -r; x <= r; x++) {
				for (int y = -r; y <= r; y++) {
					if (x * x + y * y <= r * r) {
						m_map.MakeAlive(c.tileX + x, c.tileZ + y);
					}
				}
			}
		}

		//获取直线通过的格子
		List<CCave.Coord> GetLine(CCave.Coord from, CCave.Coord to)
		{
			List<CCave.Coord> line = new List<CCave.Coord>();
			CIntLine il = new CIntLine(
				new Vector2(from.tileX, from.tileZ),
				new Vector2(to.tileX, to.tileZ));
			List<Vector2> list = il.GetLine();
			for (int i = 0; i < list.Count; i++) {
				int x = (int)list[i].x;
				int y = (int)list[i].y;
				CCave.Coord co = new CCave.Coord(x, y);
				line.Add(co);
			}

			return line;
		}

		void OnDestroy()
		{
			m_survivingRooms.Clear();
			m_survivingRooms = null;
		}

		//end of class
	}

}
