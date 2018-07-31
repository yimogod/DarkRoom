using System;
using UnityEngine;
namespace Sword {
	public class GameConst
	{
		//tile的尺寸, 如果是3d, 则基于米, 如果是2d, 则基于像素
		public static float TILE_SIZE = 1f;
		//在点击地面时是否讲坐标转换为tile中心的坐标
		public static bool FormatPosIntoTileCenter = true;
		//玩具控制的单位是否需要最基本的ai
		//比如走到点击的地方和A到点击的地方和技能到某个地方
		public static bool HeroNeedBaseAI = true;

		public const int TYPE_COIN = 1;
		public const int TYPE_GOLD = 2;

		public const int DIRECTION_LEFT = 2;
		public const int DIRECTION_RIGHT = 0;
		public const int DIRECTION_UP = 1;
		public const int DIRECTION_DOWN = 3;

		public const string LEFT = "left";
		public const string RIGHT = "right";
		public const string UP = "up";
		public const string DOWN = "down";

		public const string ACTOR_VIEW_ROOT = "view_root";

		public const string CLIP_IDLE = "idle";
		public const string CLIP_RUN = "run";
		public const string CLIP_ATK = "attack";
		public const string CLIP_CAST = "cast";
		public const string CLIP_DIE = "die";

		// ai目标
		public const int AI_GOAL_DUMMY = -1;//没有意义的ai
		public const int AI_GOAL_HOLD = 0;//站在原地
		public const int AI_GOAL_PATROL = 1;//巡逻
		public const int AI_GOAL_EXPLORE = 2;//探索
		public const int AI_GOAL_ATK = 3;//攻击
		public const int AI_GOAL_ESCAPE = 4;//逃跑
		public const int AI_GOAL_WARNING = 5;//通知队友有目标
		public const int AI_GOAL_SEEK = 6;//追踪

		public const string LAYER_NAME_TERRAIN = "Terrain";
		public const string LAYER_NAME_TERRAIN_02 = "Terrain_02";
		public const string LAYER_NAME_LAKE = "Lake";
		public const string LAYER_NAME_DECAL = "Decal";
		public const string LAYER_NAME_TRIGGER = "Trigger";
		public const string LAYER_NAME_UNIT = "Unit";
		public const string LAYER_NAME_LOW_SKY = "LowSky";
		public const string LAYER_NAME_HIGH_SKY = "HighSky";
		public const string LAYER_NAME_WALKABLE = "Walkable";
		public const string LAYER_NAME_GLOW = "Glow";

		//单位类型, 普通单位, 人物, 触发器
		public const int UNIT_TYPE_UNIT = 0;
		public const int UNIT_TYPE_ACTOR = 1;
		public const int UNIT_TYPE_TRIGGER = 2;

		public const string GO_NAME_ATTACH_ROOT = "attach_root";

        public enum TileType {
			RESERVED = -1,
			TERRIAN,
			ROAD,
			LAKE,
			ROOM,
			DECAL,//贴花
			TREE,
			DECO_DESTROY,//可被破坏的装饰物
			DECO_BLOCK//装饰物, 有阻挡
		}

		public enum LayerIndex {
			TERRIAN = 0,
			TERRIAN_2, //这个代表terraincomp的第二种地形, 索引为1, 不能删除
			ROAD, //lake用这层
			DECAL,
			UNIT,//TRIGGER, Deco
			LOW_SKY,
			HIGH_SKY,
			WALKABLE
		}
	}

}