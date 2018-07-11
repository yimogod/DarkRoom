﻿using UnityEngine;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;

namespace Sword
{

//创建地图的辅助类,
//1. 一些实用方法
//2. 创建单位的实用方法
    public class DungeonMapHelper
    {
        /*如果添加一个怪, 则周围的2层都会被占用, 用于随机创建一个怪时避免重叠, key = row * 10000 + col;*/
        private Dictionary<int, bool> _unitRangeDict = new Dictionary<int, bool>();

        /*存储怪和不可通行的位置, 用于创建trigger时避开, 但可以在怪的影响力范围, key = row * 10000 + col;*/
        private Dictionary<int, bool> _unitPosDict = new Dictionary<int, bool>();

        private int _numRows;
        private int _numCols;

        public DungeonMapHelper(CMapGrid<CStarNode> walkGrid)
        {
            _numRows = walkGrid.NumRows;
            _numCols = walkGrid.NumCols;

            for (int row = 0; row < _numRows; ++row)
            {
                for (int col = 0; col < _numCols; ++col)
                {
                    bool w = walkGrid.IsWalkable(row, col);
                    if (w) continue;

                    int key = row * 10000 + col;
                    _unitRangeDict[key] = true;
                    _unitPosDict[key] = true;
                }
            }
        }

        //创建所有的单位后, 需要调用此函数, 来更新地图 空位 的情况
        public void AddUnitToDict(Vector3 pos, int range = 2)
        {
            int key = 10000 * (int) pos.z + (int) pos.x;
            _unitPosDict[key] = true;
            if (range <= 0) return;

            int minCol = (int) pos.x - range;
            int maxCol = minCol + range * 2;
            int minRow = (int) pos.z - range;
            int maxRow = minRow + range * 2;

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    key = row * 10000 + col;
                    _unitRangeDict[key] = true;
                }
            }
        }

        //创建单位生成的信息,比如位置和方向
        public UnitBornData CreateRandomUnitBorn(int id, CUnitEntity.TeamSide group)
        {
            Vector3 pos = FindFreeTileNotInDict(_unitRangeDict);
            return UnitBornData.CreateUnitBornData(id, group, pos);
        }

        //寻找地图中空白的位置
        public Vector3 FindFreeTile()
        {
            return FindFreeTileNotInDict(_unitPosDict);
        }

        //寻找tile点附近的可以放置怪物的坐标
        public Vector3 FindFreeTileNear(Vector3 pos)
        {
            int centerCol = (int) pos.x;
            int centerRow = (int) pos.z;

            int col, row, key;
            bool block;
            for (int i = 1; i < 4; ++i)
            {
                row = centerRow;

                col = centerCol - i;
                key = 10000 * row + col;
                block = _unitPosDict.ContainsKey(key);
                if (!block) return new Vector3(col, 0, row);

                col = centerCol + i;
                key = 10000 * row + col;
                block = _unitPosDict.ContainsKey(key);
                if (!block) return new Vector3(col, 0, row);



                col = centerCol;

                row = centerRow - i;
                key = 10000 * row + col;
                block = _unitPosDict.ContainsKey(key);
                if (!block) return new Vector3(col, 0, row);


                row = centerRow + i;
                key = 10000 * row + col;
                block = _unitPosDict.ContainsKey(key);
                if (!block) return new Vector3(col, 0, row);
            }


            return FindFreeTile();
        }

        //创建, 地图中可以空置的位置
        private Vector3 FindFreeTileNotInDict(Dictionary<int, bool> dict)
        {
            Vector3 pos = CMapUtil.FindRandomNodeLocation(_numRows, _numCols);
            int col = (int) pos.x;
            int row = (int) pos.z;
            int key = 10000 * row + col;

            bool block = dict.ContainsKey(key);
            int maxTryTimes = 100;
            int tryTimes = 0;
            while (block && tryTimes < maxTryTimes)
            {
                pos = CMapUtil.FindRandomNodeLocation(_numRows, _numCols);
                col = (int) pos.x;
                row = (int) pos.z;
                key = 10000 * row + col;
                block = dict.ContainsKey(key);
                tryTimes++;
            }

            if (tryTimes == maxTryTimes)
            {
                Debug.LogError("check forest num, we can not find free tile in walkable grid!");
                return CDarkConst.INVALID_VEC3;
            }

            return pos;
        }

        /*清理辅助数据, 只在创建时做辅助用*/
        public void Clear()
        {
            _unitRangeDict.Clear();
            _unitPosDict.Clear();

            _unitRangeDict = null;
            _unitPosDict = null;
        }
    }
}