using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword
{
	/// <summary>
	/// ע��, �������е����Զ������ӻ���ٵ�ֵ
	/// ���������ְҵ���������ݵ�����
	/// </summary>
	public class ActorBornAttributeMeta : CBaseMeta
	{
		public ActorRace Race => (ActorRace)Id;
		public ActorClass Class => (ActorClass)Id;

		public float Strength;

		public float Dexterity;

		public float Intelligence;

		public float Constitution;

		public float Willpower;

		public float Cunning;

		public float Luck;

		public float Health;

		public float HealthRating;

		public int MoveRange;

		/// <summary>
		/// 0~1�ľ���ͷ����߽����İٷֱ�
		/// </summary>
		public float ExpPenalty;

		public ActorBornAttributeMeta(int id) : base(id)
		{
		}

		public void ReadDataFromTable(CTabReader reader)
		{
			NameKey = reader.ReadString();
			Strength = reader.ReadFloat();
			Dexterity = reader.ReadFloat();
			Constitution = reader.ReadFloat();
			Intelligence = reader.ReadFloat();
			Willpower = reader.ReadFloat();
			Cunning = reader.ReadFloat();
			Luck = reader.ReadFloat();
			Health = reader.ReadFloat();
			HealthRating = reader.ReadFloat();
			ExpPenalty = reader.ReadFloat();
		}
	}

	/// <summary>
	/// ----------------------- �������� ---------------------------
	/// </summary>
	public class RaceMetaManager
	{
		private static Dictionary<int, ActorBornAttributeMeta> m_dict =
			new Dictionary<int, ActorBornAttributeMeta>();

		public static Dictionary<int, ActorBornAttributeMeta> Data => m_dict;

		public static void AddMeta(ActorBornAttributeMeta meta)
		{
			m_dict.Add(meta.Id, meta);
		}

		public static ActorBornAttributeMeta GetMeta(int id)
		{
			if (!m_dict.ContainsKey(id))
			{
				Debug.LogError($"{id} not in RaceMetaManager");
				return null;
			}
			return m_dict[id];
		}
	}

	public class RaceMetaParser : CMetaParser
	{
		public RaceMetaParser(Action complete) : base(complete)
		{
		}

		protected override void Parse()
		{
			for (int i = 0; i < m_reader.Row; ++i)
			{
				m_reader.MarkRow(i);

				var meta = new ActorBornAttributeMeta(m_reader.ReadInt());
				meta.ReadDataFromTable(m_reader);
				RaceMetaManager.AddMeta(meta);
			}
		}
	}


	/// <summary>
	/// ----------------------- ְҵ���� ---------------------------
	/// </summary>
	public class ClassMetaManager
	{
		private static Dictionary<int, ActorBornAttributeMeta> m_dict =
			new Dictionary<int, ActorBornAttributeMeta>();

		public static Dictionary<int, ActorBornAttributeMeta> Data => m_dict;

		public static void AddMeta(ActorBornAttributeMeta meta)
		{
			m_dict.Add(meta.Id, meta);
		}

		public static ActorBornAttributeMeta GetMeta(int id)
		{
			if (!m_dict.ContainsKey(id))
			{
				Debug.LogError($"{id} not in ClassMetaManager");
				return null;
			}
			return m_dict[id];
		}
	}

	public class ClassMetaParser : CMetaParser
	{
		public ClassMetaParser(Action complete) : base(complete)
		{
		}

		protected override void Parse()
		{
			for (int i = 0; i < m_reader.Row; ++i)
			{
				m_reader.MarkRow(i);

				var meta = new ActorBornAttributeMeta(m_reader.ReadInt());
				meta.ReadDataFromTable(m_reader);
				ClassMetaManager.AddMeta(meta);
			}
		}
	}
}