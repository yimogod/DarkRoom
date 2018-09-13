using System;
using System.Collections.Generic;
using DarkRoom.Utility;

namespace DarkRoom.Game
{
    public class CMetaParserManager
    {
        // 需要加载和解析的主要的配置文件
        //key是address, value是解析器
        protected List<KeyValuePair<string, CMetaParser>> m_list = new List<KeyValuePair<string, CMetaParser>>();

        //如果分帧解析, 那当前正在解析的索引
        protected int m_currParseMainIndex = 0;

        public int MainMetaNum => m_list.Count;

        public CMetaParserManager()
        {
        }

        /// <summary>
        /// 全部解析主要内容
        /// </summary>
        public void Execute()
        {
            foreach (var item in m_list)
            {
                ParseSingle(item);
            }
        }

        /// <summary>
        /// 解析并加载下一个, 用于分帧加载
        /// 返回的结果为是否全部加载完毕
        /// </summary>
        public bool ExcuteNext()
        {
            if (m_list.Count == 0) return true;

            var item = m_list[m_currParseMainIndex];
            ParseSingle(item);

            //解析完毕当前的资源后是否全部解析完成
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