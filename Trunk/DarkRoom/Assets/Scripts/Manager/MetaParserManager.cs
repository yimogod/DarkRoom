using System.Collections.Generic;
using  DarkRoom.Game;
using DarkRoom.PCG;
using UnityEngine;

namespace Sword
{
    public class MetaParserManager
    {
        public List<string> m_urlList = new List<string>();
        public List<CMetaParser> m_parserList = new List<CMetaParser>();

        private int m_num = 0;

        public MetaParserManager()
        {
        }

        public void Init()
        {
            //AddPaser("Meta/ability_meta", new CAbilityMetaParser());
            //AddPaser("Meta/effect_meta", new CEffectMetaParser());
            //AddPaser("Meta/buff_meta", new CBuffMetaParser());

            AddPaser("Meta/actor_meta", new ActorMetaParser());

            AddPaser("Meta/forest_room_meta", new CForestRoomMetaParser());
            AddPaser("Meta/map_meta", new MapMetaParser());

            m_num = m_urlList.Count;
        }

        public void Execute()
        {
            for (int i = 0; i < m_num; i++)
            {
                UnityEngine.Object obj = Resources.Load(m_urlList[i]);
                m_parserList[i].Execute(obj.ToString());
            }
        }

        public void Dispose()
        {
            m_urlList.Clear();

            for (int i = 0; i < m_parserList.Count; ++i)
            {
                m_parserList[i].Dispose();
            }

            m_parserList.Clear();

            m_urlList = null;
            m_parserList = null;
        }

        private void AddPaser(string path, CMetaParser parser)
        {
            m_urlList.Add(path);
            m_parserList.Add(parser);
        }
    }
}