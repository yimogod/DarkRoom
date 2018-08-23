using System;

namespace DarkRoom.Game
{
	public class CMetaParser
	{
		protected bool m_useXml = false;
		protected CTabReader m_reader = new CTabReader();
		protected CXmlReader m_xreader = new CXmlReader();

		public CMetaParser()
		{
			m_useXml = false;
		}

		public CMetaParser(bool useXml)
		{
			m_useXml = useXml;
		}

		public virtual void Execute(string content)
		{
			if (m_useXml){
				m_xreader.Parse(content);
				return;
			}

			m_reader.Parse(content);
		}

		public void Dispose()
		{
			m_reader.Close();
			m_xreader.Close();
		}
	}
}