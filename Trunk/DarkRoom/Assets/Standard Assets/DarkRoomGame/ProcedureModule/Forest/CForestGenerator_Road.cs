﻿using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG
{
	/// <summary>
	/// 平原产生河流和道路的类
	/// </summary>
	public class CForestGenerator_Road {
        protected int _numCols;
        protected int _numRows;

        protected AStar astar = new AStar();

        //路开始的点
        protected Vector3 _startPoint;
        //路结束的点, 放置有传送门
        protected Vector3 _endPoint;

        //存储主路点的node坐标, node来自于walkgrid
        protected List<MapNode> _wayPointList = new List<MapNode>();

        public RoadGenerator(int cols, int rows)
        {
            _numCols = cols;
            _numRows = rows;
        }

        public Vector3 startTile
        {
            get { return _startPoint; }
        }

        public Vector3 endTile
        {
            get { return _endPoint; }
        }

        //支线的路, 要连接到院子和两个神秘点
        public void CreateRoadAttached(Dictionary<int, RoomInfo> roomDict, MapGrid walkGrid, MapGrid typeGrid)
        {
            foreach (KeyValuePair<int, RoomInfo> item in roomDict)
            {
                RoomInfo room = item.Value;
                int doorCol = room.doorCol;
                int doorRow = room.doorRow;
                int targetCol = -1;
                int targetRow = -1;

                //遍历主路, 找到最近的路点
                int dis = 1000000;
                foreach (MapNode node in _wayPointList)
                {
                    int d = node.DisBetween(doorRow, doorCol);
                    if (d < dis)
                    {
                        dis = d;
                        targetCol = node.col;
                        targetRow = node.row;
                    }
                }

                //路离房子特别近, 就不修路了
                if (dis < 36) continue;

                walkGrid.SetStartNode(doorRow, doorCol);
                walkGrid.SetEndNode(targetRow, targetCol);
                bool r = astar.FindPath(walkGrid);
                //找到路就根据a*结果画路
                if (!r) continue;

                int roadType = (int)RayConst.TileType.ROAD;
                foreach (MapNode node in astar.path)
                {
                    //新的路需要设置类型
                    typeGrid.SetType(node.row, node.col, roadType);
                    _wayPointList.Add(node);
                }
                ExpandAttachRoad(astar.path, walkGrid, typeGrid);
            }

        }

        protected virtual void ExpandAttachRoad(Stack<MapNode> path, MapGrid walkGrid, MapGrid typeGrid) { }

        //创建道路
        public void CreateMainRoad(MapGrid walkGrid, MapGrid typeGrid)
        {
            //1. 我们先从左下角随机起点.
            //我们的路的起点在整张图里面缩3个格子
            int startRange = 3;
            int range = 9;
            int minX = RayRandom.Next(startRange, range);
            int minZ = RayRandom.Next(startRange, range);
            _startPoint = new Vector3(minX, 0, minZ);

            //2. 随机三个数来确定重点在那个角落--左上, 右上, 右下
            int rand = RayRandom.Next(3);
            int maxX = 0;
            int maxZ = 0;


            const int TOP_LEFT = 1;
            const int BOTTOM_RIGHT = 2;
            //top left
            if (rand == TOP_LEFT)
            {
                Debug.Log("Road direciton is Top Left");
                maxX = RayRandom.Next(startRange, range);
                maxZ = _numRows - RayRandom.Next(startRange, range);
            }
            else if (rand == BOTTOM_RIGHT)
            {//bottom right
                Debug.Log("Road direciton is Bottom Right");
                maxX = _numCols - RayRandom.Next(startRange, range);
                maxZ = RayRandom.Next(startRange, range);
            }
            else
            {//top right
                Debug.Log("Road direciton is Top Right");
                maxX = _numCols - RayRandom.Next(startRange, range);
                maxZ = _numRows - RayRandom.Next(startRange, range);
            }
            _endPoint = new Vector3(maxX, 0, maxZ);
            _endPoint = walkGrid.FindNearestWalkablePos(_endPoint);

            //我们需要把起点和终点置为可通行
            walkGrid.SetWalkable(minZ, minX, true);
            walkGrid.SetWalkable(maxZ, maxX, true);

            //3. 随机30个点, 然后根据起点和终点的方向进行排序
            List<Vector3> possibleWayPoint = new List<Vector3>();
            for (int i = 0; i < 30; ++i)
            {
                int col = RayRandom.Next(_numCols);
                int row = RayRandom.Next(_numRows);
                if (!walkGrid.IsWalkable(row, col)) continue;

                Vector3 vec = new Vector3(col, 0, row);
                possibleWayPoint.Add(vec);
            }
            possibleWayPoint.Add(_startPoint);
            possibleWayPoint.Add(_endPoint);


            //根据起点和终点的方向排序路点列表
            string axis = "x";
            int startValue = 0;
            int finishValue = 0;
            if (rand == TOP_LEFT)
            {//根据z进行排序
                possibleWayPoint.Sort(CompareZ);
                finishValue = (int)_endPoint.z;
                axis = "z";
            }
            else
            {//根据x进行排序
                possibleWayPoint.Sort(CompareX);
                finishValue = (int)_endPoint.x;
            }

            //确定合法路径点
            List<Vector3> validWayPoint = new List<Vector3>();
            validWayPoint.Add(_startPoint);
            for (int i = 0; i < possibleWayPoint.Count; ++i)
            {
                Vector3 curr = possibleWayPoint[i];
                Vector3 last = validWayPoint[validWayPoint.Count - 1];
                bool valid = CheckWayPointValid(curr, last, axis, startValue, finishValue);
                if (!valid) continue;

                validWayPoint.Add(curr);
            }
            validWayPoint.Add(_endPoint);

            //5.路径way point已经生成.现在进行寻路, 形成路网
            _wayPointList.Clear();

            for (int i = 1; i < validWayPoint.Count; ++i)
            {
                Vector3 start = validWayPoint[i - 1];
                Vector3 end = validWayPoint[i];

                walkGrid.SetStartNode((int)start.z, (int)start.x);
                walkGrid.SetEndNode((int)end.z, (int)end.x);
                bool r = astar.FindPath(walkGrid);
                //找到路就根据a*结果画路
                if (r)
                {
                    foreach (MapNode node in astar.path)
                    {
                        _wayPointList.Add(node);
                    }
                }
                else
                {//找不到路就直线连接
                 //TODO
                }
            }


            //寻路不会将起点放进去. 所以我们放入起点
            _wayPointList.Add(walkGrid.startNode);
            _wayPointList.Add(walkGrid.GetNode((int)_startPoint.z, (int)_startPoint.x));

            ExpandMainRoad(walkGrid, typeGrid);

            //6.这时, wayPointList已经是所有的路的连接点, 我们遍历设置地图数据, 我们会最终一起绘制路
            foreach (MapNode node in _wayPointList)
            {
                MapNode tn = typeGrid.GetNode(node.row, node.col);
                tn.type = (int)RayConst.TileType.ROAD;
            }

            //finish
        }

        protected virtual void ExpandMainRoad(MapGrid walkGrid, MapGrid typeGrid) { }

        //curr是目前检测是否合法的点
        //last是上一个合法的点
        protected bool CheckWayPointValid(Vector3 curr, Vector3 last, string axis, int start, int finish)
        {
            int minDis = 6;
            int progress = 0;
            int measure = 0;
            int invertProgress = 0;
            int invertMeasure = 0;

            if (axis == "x")
            {
                progress = (int)last.x;
                invertProgress = (int)last.z;
                measure = (int)curr.x;
                invertMeasure = (int)curr.z;
            }
            else
            {
                progress = (int)last.z;
                invertProgress = (int)last.x;
                measure = (int)curr.z;
                invertMeasure = (int)curr.x;
            }

            //目前不可能, 我们的起点永远在左下角, finish 永远大于 start
            if (finish < start)
            {
                progress *= -1;
                invertProgress *= -1;
                measure *= -1;
                invertMeasure *= -1;
            }

            //两个点在axis轴上距离太短了
            bool check = (measure - progress) <= minDis;
            if (check) return false;

            //离终点太近了
            check = (finish - measure) <= minDis * 2;
            if (check) return false;

            //拐的太厉害了
            //两个点在非axis方向上的距离超过了起点和终点间距的0.2
            check = Math.Abs(invertProgress - invertMeasure) >= Math.Abs((finish - start)) * 0.3f;
            if (check) return false;

            return true;
        }

        protected int CompareZ(Vector3 node1, Vector3 node2)
        {
            if (RMathUtil.FloatEqual(node1.z, node2.z)) return 0;
            if (node1.z < node2.z) return -1;
            return 1;
        }

        protected int CompareX(Vector3 node1, Vector3 node2)
        {
            if (RMathUtil.FloatEqual(node1.z, node2.z)) return 0;
            if (node1.x < node2.x) return -1;
            return 1;
        }





        /// <summary>
        /// 最简单的产生路径的方法. 不考虑地图的实际情况
        /// 请保证xy1 和 xy2 的距离足够远. 至少得有6 * 5个格子以上吧
        /// 返回的列表是经过排序的
        /// </summary>
        public List<Vector2> GetWayPoints(int width, int height, int x1, int y1, int x2, int y2)
		{
			CIntLine line = new CIntLine(new Vector2(x1, y1), new Vector2(x2, y2));

			int step = 6;
			int maxLimit = 10;
			List<Vector2> list = new List<Vector2>();
			
            int dx = Mathf.Abs(x1 - x2);
			int dy = Mathf.Abs(y1 - y2);

			int x, y, delta, intX, intY, min, max;
			Vector2 start, end;

			if (dx > dy) {
				if (x1 < x2) {
					start = new Vector2(x1, y1);
					end = new Vector2(x2, y2);
					min = x1;
					max = x2;
				} else {
					end = new Vector2(x1, y1);
					start = new Vector2(x2, y2);
					max = x1;
					min = x2;
				}

				list.Add(start);
				for (x = min; x < max; x += step) {
					y = Mathf.RoundToInt(height * CDarkRandom.NextPerlinValueNoise(width, height, 10.0f));
					intY = line.GetY(x);
					delta = intY - y;
					if (delta > maxLimit) {
						y = intY + maxLimit;
					} else if (delta < -maxLimit) {
						y = intY - maxLimit;
					}

					Vector2 v = new Vector2(x, y);
					MakeTileInMap(width, height, ref v);
					list.Add(v);
				}
				list.Add(end);

			} else {
				if (y1 < y2) {
					start = new Vector2(x1, y1);
					end = new Vector2(x2, y2);
					min = y1;
					max = y2;
				} else {
					end = new Vector2(x1, y1);
					start = new Vector2(x2, y2);
					max = y1;
					min = y2;
				}
				list.Add(start);

				for (y = min; y < max; y += step) {
					x = Mathf.RoundToInt(width * CDarkRandom.NextPerlinValueNoise(width, height, 10.0f));
					intX = line.GetX(y);
					delta = intX - x;
					if (delta > maxLimit) {
						x = intX + maxLimit;
					} else if (delta < -maxLimit) {
						x = intX - maxLimit;
					}

					Vector2 v = new Vector2(x, y);
					MakeTileInMap(width, height, ref v);
					list.Add(v);
				}

				list.Add(end);
			}

			return list;
		}

		private void MakeTileInMap(int width, int height, ref Vector2 v)
		{
			if (v.x < 0) v.x = 0;
			if (v.y < 0) v.y = 0;
			if (v.x > (width - 1)) v.x = width - 1;
			if (v.y > (height - 1)) v.y = height - 1;
		}
	}
}