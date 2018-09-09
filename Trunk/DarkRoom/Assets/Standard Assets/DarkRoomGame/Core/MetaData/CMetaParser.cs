using System;

namespace DarkRoom.Game
{
	public class CMetaParser
	{
		protected bool m_useXml = false;
		protected CTabReader m_reader = new CTabReader();
		protected CXmlReader m_xreader = new CXmlReader();

        private Action m_onParseComplete;

        public CMetaParser()
        {
            m_useXml = false;
            m_onParseComplete = null;
        }

        public CMetaParser(Action complete)
		{
			m_useXml = false;
            m_onParseComplete = complete;
        }

		public CMetaParser(bool useXml, Action complete = null)
		{
			m_useXml = useXml;
            m_onParseComplete = complete;
        }

		public virtual void Execute(string content)
		{
			if (m_useXml){
				m_xreader.Parse(content);
                m_onParseComplete?.Invoke();
                return;
			}

			m_reader.Parse(content);
            m_onParseComplete?.Invoke();
        }

		public void Dispose()
		{
			m_reader.Close();
			m_xreader.Close();
		}
	}
}