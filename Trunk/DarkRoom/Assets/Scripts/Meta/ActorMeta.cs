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