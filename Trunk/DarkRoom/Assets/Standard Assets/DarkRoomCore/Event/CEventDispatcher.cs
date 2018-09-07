using System;
using System.Collections.Generic;

namespace DarkRoom.Core
{
	public class CEventDispatcher
	{
		private Dictionary<string, List<Action<CEvent>>> m_dict =
			new Dictionary<string, List<Action<CEvent>>>();

		public bool HasEventListener(string type)
		{
			return m_dict.ContainsKey(type);
		}

		public void AddEventListener(string type, Action<CEvent> listener)
		{
			if (!HasEventListener(type)) m_dict[type] = new List<Action<CEvent>>();
			m_dict[type].Add(listener);
		}

		public void RemoveEventListener(string type, Action<CEvent> listener)
		{
			if (!HasEventListener(type)) return;
			m_dict[type].Remove(listener);
		}

		public void RemoveAllListener()
		{
			m_dict.Clear();
		}

		public void DispatchEvent(CEvent e)
		{
			if (!HasEventListener(e.Type)) return;
			foreach (var item in m_dict[e.Type])
			{
				item(e);
			}
		}
	}
}