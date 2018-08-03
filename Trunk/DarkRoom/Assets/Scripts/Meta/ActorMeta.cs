using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword
{
    /// <summary>
    /// 角色的元数据, 存储了出生时的数据
    /// 我们的entity会读取这些数据, 然后存在相关的comp中
    /// 一些重要属性的说明
    /// ----------------
	/// 
	/// 二级属性
	/// Defense 减少近战的物理攻击伤害 Ranged Defense 减少远程攻击的伤害
	/// Defense bonuses from equipment
	/// + (Dexterity - 10) * 0.35 
	/// + Defense bonuses from talents 
	/// + Luck * 0.4
	/// 
	/// Accuracy 命中率-- TODO有点低啊
	/// 4 
	/// + (Dexterity - 10) * 1.00 
	/// + Luck * 0.40 
	/// + Accuracy bonuses from Combat Accuracy
	/// + Accuracy bonuses from equipment 
	/// + Accuracy bonuses or penalties from effects
	/// 
	/// Stamina 活力/精力
	/// 大部分物理天赋都会用到. 默认每回合恢复0.3, 有buff可以修改本值. 
	/// 休息的时候可以加快恢复速度
	/// 
	/// Psi (Psionic) 超自然力量
	/// 一些超自然种族需要的资源, 会缓慢恢复, 会被一些装备和天赋重置
	/// 思想伤害会减少Psi, 而不是HP
	/// 
	/// Max Health
	/// Life Rating * (1.1+(current_level/40)) , 也会被体质影响
	/// Life Rating 由种族和职业一起影响
	/// 
	/// </summary>
	public class ActorMeta : CBaseMeta{

	    public string Prefab;
        
	    /// <summary>
        /// 职业
        /// </summary>
	    public ActorClass MetaClass;

        /// <summary>
        /// 能走几格
        /// </summary>
        public int Speed = 3;

		/// <summary>
		/// 击杀获取的经验
		/// </summary>
		public int DeadExp;

		/// <summary>
		/// 原始视野的长度
		/// </summary>
		public int FOV = 6;

	    public int InitHealth;

	    public int InitMana;

	    public float InitDamage;

	    public float InitDef;

		//创建角色给的武器
		public int InitWeapon = 0;

		public ActorMeta(int id) : base(id){}

	}

    public class ActorMetaManager
    {
        private static Dictionary<int, ActorMeta> m_dict =
            new Dictionary<int, ActorMeta>();

        public static Dictionary<int, ActorMeta> Data => m_dict;

        public static void AddMeta(ActorMeta meta)
        {
            m_dict.Add(meta.Id, meta);
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
                meta.MetaClass = (ActorClass) m_reader.ReadInt();
                meta.FOV = m_reader.ReadInt();
                meta.Speed = m_reader.ReadInt();
                meta.DeadExp = m_reader.ReadInt();
                meta.InitHealth = m_reader.ReadInt();
                meta.InitMana = m_reader.ReadInt();
                meta.InitDamage = m_reader.ReadFloat();
                meta.InitDef = m_reader.ReadFloat();
                meta.InitWeapon = m_reader.ReadInt();

                ActorMetaManager.AddMeta(meta);
            }
        }
    }
}