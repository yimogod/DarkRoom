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

        private Vector3 _pos = CDarkConst.INVALID_VEC3;

        public static UnitBornData CreateUnitBornData(int id, CUnitEntity.TeamSide group, Vector3 pos)
        {
            UnitBornData data = new UnitBornData(id, pos);
            data.direction = CDarkRandom.Next(4);
            data.group = group;

            return data;
        }

        public UnitBornData(int id, Vector3 pos)
        {
            metaId = id;
            _pos = pos;
            col = (int) pos.x;
            row = (int) pos.z;
        }

        public bool invalid
        {
            get { return CDarkUtil.IsInvalidVec3(_pos); }
        }
    }
}