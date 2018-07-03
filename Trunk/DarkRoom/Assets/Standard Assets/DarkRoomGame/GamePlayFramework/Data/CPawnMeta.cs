using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Game{
	/// <summary>
	/// 角色的元数据, 存储了出生时的数据
	/// 注意, 里面的一些数据, 比如Radius, 长宽高等数据也会存储到各个组件中
	/// 这意味着有数据冗余. 因此我们制定下列读取规则
	/// vo需要这些数据就重meta中读取
	/// comp或者entity读取数据优先从组件里面读取
	/// TODO 未来会考虑讲这个冗余数据放置到各个comp中进行配置, 这需要配套的编辑器
	/// 或者直接用collider来读取数据
	/// </summary>
	public class CPawnMeta : CBaseMeta{

		/// <summary>
		/// 基于秒的单位为米(m)--一般人类的速度是10km/1h, 意味着1s是2.7m
		/// IMPORTANT! Speed在Movement中也有. meta的数据尽量只在vo中用
		/// </summary>
		public float Speed = 3f;

		/// <summary>
		/// 击杀获取的经验
		/// </summary>
		public int DeadExp;

		/// <summary>
		/// 原始视野的角度
		/// </summary>
		public float ViewAngle = 120f;

		/// <summary>
		/// 原始视野的长度
		/// </summary>
		public float ViewRange = 10f;

		/// <summary>
		/// resource key, which indeed  is prefab name
		/// </summary>
		public string Prefab;

		/// <summary>
		/// 身体圆心半径, 注意, 
		/// IMPORTANT! Radius在Spacial中也有. meta的数据尽量只在vo中用
		/// </summary>
		public float Radius;

		/// <summary>
		/// 我的体重,
		/// IMPORTANT! Weight在Spacial中也有. meta的数据尽量只在vo中用
		/// </summary>
		public float Weight;

		public int AtkBase;
		public int AtkStep;

		public int DefBase;
		public int DefStep;

		public int HpBase;
		public int HpStep;

		public int MpBase;
		public int MpStep;

		/* base on 1000 */
		public int CritBase;

		//创建角色给的武器
		public int InitWeapon = 0;

		//主动技能, x代表ability id， y代表ability lv
		public List<Vector2Int> ActiveAbilityList = new List<Vector2Int>();

		//talent, 出生自带的buff
		public List<Vector2Int> TalentList = new List<Vector2Int>();

		public CPawnMeta (int id) : base(id){}


		public int GetATK(int level){
			return AtkBase + level * AtkStep;
		}
		
		public int GetDEF(int level){
			return DefBase + level * DefStep;
		}

		public int GetHP(int level){
			return HpBase + level * HpStep;
		}

		public int GetMP(int level){
			return MpBase + level * MpBase;
		}

		public int GetCrit(int level){
			return CritBase;
		}
	}
}