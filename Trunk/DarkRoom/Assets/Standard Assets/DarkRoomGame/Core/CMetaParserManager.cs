using System.Collections.Generic;
using DarkRoom.PCG;
using UnityEngine;

namespace DarkRoom.Game
{
    public class CMetaParserManager
    {

        // 同步加载的小包, 比如在创建角色或者选择角色前加载的几个小配置文件
        protected List<KeyValuePair<string, CMetaParser>> m_liteList = new List<KeyValuePair<string, CMetaParser>>();
        // 需要加载和解析的主要的配置文件
        //key是address, value是解析器
        protected List<KeyValuePair<string, CMetaParser>> m_list = new List<KeyValuePair<string, CMetaParser>>();

        //如果分帧解析, 那当前正在解析的索引
        protected int m_currParseMainIndex = 0;

        public CMetaParserManager()
        {
        }

        public virtual void Initialize()
        {
            
        }

        /// <summary>
        /// 全部解析小包
        /// </summary>
        public virtual void ExecuteLite()
        {
            foreach (var item in m_liteList)
            {
                ParseSingle(item);
            }
        }

        /// <summary>
        /// 全部解析主要内容
        /// </summary>
        public void ExecuteMain()
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
        public bool ExcuteNextMain()
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