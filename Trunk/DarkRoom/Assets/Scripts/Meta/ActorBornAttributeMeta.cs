using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword
{
	public class ActorBornAttributeMeta : CBaseMeta
	{
	    public ActorRace Race => (ActorRace)Id;
	    public ActorClass Class => (ActorClass)Id;

        public float Strength;

	    public float Dexterity;

	    public float Constitution;

	    public float Magic;

	    public float Willpower;

	    public float Cunning;

	    public float Luck;

	    public float Health;

	    public float HealthRating;

        /// <summary>
        /// 0~1的经验惩罚或者奖励的百分比
        /// </summary>
        public float ExpPenalty;

        public ActorBornAttributeMeta(int id) : base(id){}

	    public void ReadDataFromTable(CTabReader reader)
	    {
	        NameKey = reader.ReadString();
	        Strength = reader.ReadFloat();
	        Dexterity = reader.ReadFloat();
	        Constitution = reader.ReadFloat();
	        Magic = reader.ReadFloat();
	        Willpower = reader.ReadFloat();
	        Cunning = reader.ReadFloat();
	        Luck = reader.ReadFloat();
	        Health = reader.ReadFloat();
	        HealthRating = reader.ReadFloat();
	        ExpPenalty = reader.ReadFloat();
        }
	}

    /// <summary>
    /// ----------------------- 种族数据 ---------------------------
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
            return m_dict[id];
        }
    }

    public class RaceMetaParser : CMetaParser
    {
        public override void Execute(string content)
        {
            base.Execute(content);

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
    /// ----------------------- 职业数据 ---------------------------
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
            return m_dict[id];
        }
    }

    public class ClassMetaParser : CMetaParser
    {
        public override void Execute(string content)
        {
            base.Execute(content);

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