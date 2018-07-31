using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword{
	/// <summary>
	/// 角色的元数据, 存储了出生时的数据
	/// 注意, 里面的一些数据, 比如Radius, 长宽高等数据也会存储到各个组件中
	/// vo需要这些数据就重meta中读取
	/// 或者直接用collider来读取数据
	/// 
	/// 我们的entity会读取这些数据, 然后存在相关的comp中
	/// </summary>
	public class ActorMeta : CBaseMeta{

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

		public ActorMeta(int id) : base(id){}


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

    public class ActorMetaManager
    {
        private static Dictionary<int, ActorMeta> m_dict =
            new Dictionary<int, ActorMeta>();

        public static Dictionary<int, ActorMeta> Data => m_dict;

        public static void AddMeta(ActorMeta meta)
        {
            m_dict.Add(meta.nId, meta);
        }

        public static ActorMeta GetMeta(int id)
        {
            return m_dict[id];
        }
    }

    public class ActorMetaParser : CMetaParser
    {
        public override void Execute(string content)
        {
            base.Execute(content);

            for (int i = 0; i < m_reader.row; ++i)
            {
                m_reader.MarkRow(i);

                ActorMeta meta = new ActorMeta(m_reader.ReadInt());
                meta.NameKey = m_reader.ReadString();
                meta.Prefab = m_reader.ReadString();

                ActorMetaManager.AddMeta(meta);
            }
        }
    }

}