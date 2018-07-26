using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{

//出生点信息, 在自动地形和怪物生成中会用到
    public class UnitBornData
    {
        public enum BornType
        {
            Actor,
            Trigger
        };

        public int col;
        public int row;

        public int metaId;
        public BornType bornType;

        public int direction = GameConst.DIRECTION_RIGHT;
        public CUnitEntity.TeamSide group = CUnitEntity.TeamSide.Red;

        private Vector2Int _pos = CDarkConst.INVALID_VEC2INT;

        public static UnitBornData CreateUnitBornData(int id, CUnitEntity.TeamSide group, Vector2Int pos)
        {
            UnitBornData data = new UnitBornData(id, pos);
            data.direction = CDarkRandom.Next(4);
            data.group = group;

            return data;
        }

        public UnitBornData(int id, Vector2Int pos)
        {
            metaId = id;
            _pos = pos;
            col = pos.x;
            row = pos.y;
        }

        public bool invalid
        {
            get { return CDarkUtil.IsValidVec2Int(_pos); }
        }
    }
}