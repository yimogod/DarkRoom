using System;
using UnityEngine;
namespace Sword {
	public class GameConst
	{
        /// <summary>
        /// terrain的中间位置
        /// </summary>
        public const float DEFAULT_TERRAIN_HEIGHT = 4f;


		//tile的尺寸, 如果是3d, 则基于米, 如果是2d, 则基于像素
		public static float TILE_SIZE = 1f;
		//在点击地面时是否讲坐标转换为tile中心的坐标
		public static bool FormatPosIntoTileCenter = true;
		//玩具控制的单位是否需要最基本的ai
		//比如走到点击的地方和A到点击的地方和技能到某个地方
		public static bool HeroNeedBaseAI = true;

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
	}

}