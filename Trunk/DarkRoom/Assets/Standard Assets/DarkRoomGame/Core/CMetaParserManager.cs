using System.Collections.Generic;
using DarkRoom.PCG;
using UnityEngine;

namespace DarkRoom.Game
{
    public class CMetaParserManager
    {

        // ͬ�����ص�С��, �����ڴ�����ɫ����ѡ���ɫǰ���صļ���С�����ļ�
        protected List<KeyValuePair<string, CMetaParser>> m_liteList = new List<KeyValuePair<string, CMetaParser>>();
        // ��Ҫ���غͽ�������Ҫ�������ļ�
        //key��address, value�ǽ�����
        protected List<KeyValuePair<string, CMetaParser>> m_list = new List<KeyValuePair<string, CMetaParser>>();

        //�����֡����, �ǵ�ǰ���ڽ���������
        protected int m_currParseMainIndex = 0;

        public CMetaParserManager()
        {
        }

        public virtual void Initialize()
        {
            
        }

        /// <summary>
        /// ȫ������С��
        /// </summary>
        public virtual void ExecuteLite()
        {
            foreach (var item in m_liteList)
            {
                ParseSingle(item);
            }
        }

        /// <summary>
        /// ȫ��������Ҫ����
        /// </summary>
        public void ExecuteMain()
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
        public bool ExcuteNextMain()
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
            foreach (var item in m_liteList)
            {
                item.Value.Dispose();
            }

            foreach (var item in m_list)
            {
                item.Value.Dispose();
            }

            m_liteList.Clear();
            m_list.Clear();
            m_liteList = null;
            m_list = null;
        }

        protected void AddPaser(string path, CMetaParser parser, bool lite = false)
        {
            var item = new KeyValuePair<string, CMetaParser>(path, parser);
            if (lite) m_liteList.Add(item);
            else m_list.Add(item);
        }

        protected virtual void ParseSingle(KeyValuePair<string, CMetaParser> item)
        {
            CResourceManager.LoadText(item.Key, item.Value.Execute);
        }
    }
}