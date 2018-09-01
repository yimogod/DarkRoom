using System;
using System.Collections.Generic;

namespace DarkRoom.Core
{
	public class CEventDispatcher
	{
		private Dictionary<string, Action<CEvent>> m_dict;

		public bool HasEventListener(string type)
		{
			return false;
		}

		public void AddEventListener(string type, Action<CEvent> listener)
		{

		}

		public void RemoveEventListener(string type, Action<CEvent> listener)
		{

		}

		public void RemoveAllListener()
		{

		}

		public void DispatchEvent(CEvent e)
		{

		}

	}
}
