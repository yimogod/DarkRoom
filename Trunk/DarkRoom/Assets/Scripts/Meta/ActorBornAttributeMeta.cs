using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword
{
	public class ActorBornAttributeMeta : CBaseMeta
	{
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
	}

    public class ActorBornAttributeMetaManager
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

    public class ActorBornAttributeMetaParser : CMetaParser
    {
        public override void Execute(string content)
        {
            base.Execute(content);

            for (int i = 0; i < m_reader.Row; ++i)
            {
                m_reader.MarkRow(i);

                var meta = new ActorBornAttributeMeta(m_reader.ReadInt());
                meta.NameKey = m_reader.ReadString();
                meta.Strength = m_reader.ReadFloat();
                meta.Dexterity = m_reader.ReadFloat();
                meta.Constitution = m_reader.ReadFloat();
                meta.Magic = m_reader.ReadFloat();
                meta.Willpower = m_reader.ReadFloat();
                meta.Cunning = m_reader.ReadFloat();
                meta.Luck = m_reader.ReadFloat();
                meta.Health = m_reader.ReadFloat();
                meta.HealthRating = m_reader.ReadFloat();
                meta.ExpPenalty = m_reader.ReadFloat();

                ActorBornAttributeMetaManager.AddMeta(meta);
            }
        }
    }
}