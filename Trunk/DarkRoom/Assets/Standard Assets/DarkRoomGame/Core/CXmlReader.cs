using System;
using System.Collections.Generic;
using System.Xml;

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

		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref bool t) {
			int i = 0;
			bool b = TryReadChildNodeAttr(parent, childNodeName, attrName, ref i);
			t = (i != 0);
			return b;
		}

		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref float t)
		{
			XmlElement node =  parent.SelectSingleNode(childNodeName) as XmlElement;
			if (node == null)return false;

			string v = node.GetAttribute(attrName);
			return float.TryParse(v, out t);
		}

		public bool TryReadChildNodeAttr(XmlElement parent, string childNodeName, string attrName, ref string t)
		{
			XmlElement node =  parent.SelectSingleNode(childNodeName) as XmlElement;
			if (node == null)return false;

			t = node.GetAttribute(attrName);
			return true;
		}

		/// <summary>
		/// 获取名字一样的数组
		/// </summary>
		/// <returns></returns>
		public bool TryReadChildNodesAttr(XmlElement parent, string childNodeName, string attrName, List<string> list) {
			XmlNodeList nodes = parent.SelectNodes(childNodeName);
			if (nodes == null) return false;

			foreach (var node in nodes) {
				string t = (node as XmlElement).GetAttribute(attrName);
				list.Add(t);
			}
			
            return true;
		}

		public void Close()
		{
			m_xml = null;
		}
	}
}