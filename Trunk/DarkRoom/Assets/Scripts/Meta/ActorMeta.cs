using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword
{
	/// <summary>
	/// 一般是怪物的配置, 根据职业和种族去读取配置
	/// </summary>
	public class ActorMeta : CBaseMeta
	{
		public string Address;

		/// <summary>
		/// 具体的职业
		/// </summary>
		public ActorClass SubClass;

		/// <summary>
		/// 具体的种族
		/// </summary>
		public ActorRace SubRace;

		/// <summary>
		/// 击杀获取的经验
		/// </summary>
		public int DeadExp;

		public ActorMeta(int id) : base(id)
		{
		}
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
		public ActorMetaParser(Action complete) : base(complete)
		{
		}

		protected override void Parse()
		{
			for (int i = 0; i < m_reader.Row; ++i)
			{
				m_reader.MarkRow(i);

				ActorMeta meta = new ActorMeta(m_reader.ReadInt());
				meta.NameKey = m_reader.ReadString();
				meta.Address = m_reader.ReadString();
				meta.SubClass = (ActorClass) m_reader.ReadInt();
				meta.SubRace = (ActorRace) m_reader.ReadInt();
				meta.Speed = m_reader.ReadInt();
				meta.DeadExp = m_reader.ReadInt();

				ActorMetaManager.AddMeta(meta);
			}
		}
	}
}