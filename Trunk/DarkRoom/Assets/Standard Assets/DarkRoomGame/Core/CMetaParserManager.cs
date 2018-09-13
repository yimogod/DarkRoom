using System;
using System.Collections.Generic;
using DarkRoom.Utility;

namespace DarkRoom.Game
{
    public class CMetaParserManager
    {
        // ��Ҫ���غͽ�������Ҫ�������ļ�
        //key��address, value�ǽ�����
        protected List<KeyValuePair<string, CMetaParser>> m_list = new List<KeyValuePair<string, CMetaParser>>();

        //�����֡����, �ǵ�ǰ���ڽ���������
        protected int m_currParseMainIndex = 0;

        public int MainMetaNum => m_list.Count;

        public CMetaParserManager()
        {
        }

        /// <summary>
        /// ȫ��������Ҫ����
        /// </summary>
        public void Execute()
        {
            foreach (var item in m_list)
            {
                ParseSingle(item);
            }
        }

        /// <summary>
        /// ������������һ��, ���ڷ�֡����
        /// ���صĽ��Ϊ�Ƿ�ȫ���������
        /// </summary>
        public bool ExcuteNext()
        {
            if (m_list.Count == 0) return true;

            var item = m_list[m_currParseMainIndex];
            ParseSingle(item);

            //������ϵ�ǰ����Դ���Ƿ�ȫ���������
            m_currParseMainIndex++;
            if (m_currParseMainIndex >= m_list.Count) return true;
            return false;
        }

        public void Dispose()
        {
            foreach (var item in m_list)
            {
                item.Value.Dispose();
            }

            m_list.Clear();
            m_list = null;
        }

        protected void AddPaser(string path, CMetaParser parser)
        {
            var item = new KeyValuePair<string, CMetaParser>(path, parser);
            m_list.Add(item);
        }

        protected virtual void ParseSingle(KeyValuePair<string, CMetaParser> item)
        {
            CResourceManager.LoadText(item.Key, item.Value.Execute);
        }
    }
}