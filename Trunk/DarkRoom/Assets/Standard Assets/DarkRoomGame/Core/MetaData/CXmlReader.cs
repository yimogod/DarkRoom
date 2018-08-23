using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// xml读取器. 我们有些数据结构比较复杂的meta, 用xml来表示更合理
	/// 这个时候用xml reader来解析
	/// </summary>
	public class CXmlReader
	{
		private XmlDocument m_xml;
		private XmlNode m_rootNode;

		public CXmlReader(){}

		public XmlNodeList rootChildNodes
		{
			get { return m_rootNode.ChildNodes; }
		}

		public bool Parse(string content)
		{
			m_xml = new XmlDocument();
			m_xml.LoadXml(content);

			return true;
		}

		public void ReadRootNode(string rootStr = "Catalog")
		{
			m_rootNode = m_xml.SelectSingleNode(rootStr);
		}

		//从parent中寻找一个child node, 然后从node中读取一个属性
		//这里用ref的原因是因为, 如果在node中找不到这个属性, 就不改变t的值
		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref int t)
		{
			XmlElement node =  parent.SelectSingleNode(childNodeName) as XmlElement;
			if (node == null)return false;

			string v = node.GetAttribute(attrName);
			return Int32.TryParse(v, out t);
		}

	    /// <summary>
	    /// 读取属性. 类似于  <Clip value='false'/>
	    /// </summary>
		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref bool t) {
			int i = 0;
			bool b = TryReadChildNodeAttr(parent, childNodeName, attrName, ref i);
			t = (i != 0);
			return b;
		}

	    /// <summary>
	    /// 读取属性. 类似于  <Clip value='1.0'/>
	    /// </summary>
		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref float t)
		{
			XmlElement node =  parent.SelectSingleNode(childNodeName) as XmlElement;
			if (node == null)return false;

			string v = node.GetAttribute(attrName);
			return float.TryParse(v, out t);
		}

        /// <summary>
        /// 读取属性. 类似于  <Clip value='atk'/>
        /// </summary>
		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref string t)
		{
			XmlElement node =  parent.SelectSingleNode(childNodeName) as XmlElement;
			if (node == null)return false;

			t = node.GetAttribute(attrName);
			return true;
		}

	    //从parent中寻找一个child node, 然后从node中读取一个属性
	    //这里用ref的原因是因为, 如果在node中找不到这个属性, 就不改变t的值
	    public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, ref Vector2Int t)
	    {
	        XmlElement node = parent.SelectSingleNode(childNodeName) as XmlElement;
	        if (node == null) return false;

	        string sx = node.GetAttribute("x");
	        int x = 0;
	        
	        bool b = Int32.TryParse(sx, out x);
	        if (!b) return false;

	        string sy = node.GetAttribute("y");
            int y = 0;
            b = Int32.TryParse(sy, out y);
	        if (!b) return false;

	        t.x = x;
	        t.y = y;
	        return true;
	    }

        /// <summary>
        /// 获取名字一样的数组
        /// parent = a
        /// TryReadChildNodesAttr(parent, a, list)
        /// <a>
        ///     <b v = '1'>
        ///     <b v = '2'>
        ///     <b v = '3'>
        ///     <b v = '4'>
        /// </a>
        /// 这样list获取的数组值有1,2,3,4
        /// </summary>
        public bool TryReadChildNodesAttr(XmlNode parent, string childNodeName, List<string> list) {
			XmlNodeList nodes = parent.SelectNodes(childNodeName);
			if (nodes == null) return false;

			foreach (var node in nodes) {
				string t = (node as XmlElement).GetAttribute("v");
				list.Add(t);
			}
			
            return true;
		}

        /// <summary>
        /// <a>
        ///     <b x = '1' y='1' z='1' />
        ///     <b x = '2' y='1' z='1' />
        /// </a>
        /// </summary>
        public bool TryReadChildNodesAttr(XmlNode parent, string childNodeName, List<Vector3Int> list)
	    {
	        XmlNodeList nodes = parent.SelectNodes(childNodeName);
	        if (nodes == null) return false;

	        foreach (var node in nodes)
	        {
                Vector3Int item = Vector3Int.zero;
	            string x = (node as XmlElement).GetAttribute("x");
	            string y = (node as XmlElement).GetAttribute("y");
	            string z = (node as XmlElement).GetAttribute("z");
	            item.x = int.Parse(x);
	            item.y = int.Parse(y);
	            item.z = int.Parse(z);
                list.Add(item);
	        }

	        return true;
	    }

        public void Close()
		{
			m_xml = null;
		}
	}
}