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
        /// 具体的职业
        /// </summary>
        public ActorClass SubClass;

        /// <summary>
        /// 具体的种族
        /// </summary>
        public ActorRace SubRace;

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

            for (int i = 0; i < m_reader.Row; ++i)
            {
                m_reader.MarkRow(i);

                ActorMeta meta = new ActorMeta(m_reader.ReadInt());
                meta.NameKey = m_reader.ReadString();
                meta.Prefab = m_reader.ReadString();
                meta.SubClass = (ActorClass)m_reader.ReadInt();
                meta.SubRace = (ActorRace)m_reader.ReadInt();
                meta.FOV = m_reader.ReadInt();
                meta.Speed = m_reader.ReadInt();
                meta.DeadExp = m_reader.ReadInt();
                meta.InitWeapon = m_reader.ReadInt();

                ActorMetaManager.AddMeta(meta);
            }
        }
    }
}