using System;
using System.Collections.Generic;

namespace DarkRoom.Core
{
	public class CEvent
	{
		public string Type { get; private set; }
		public Object Data { get; private set; }

		public CEvent(string type)
		{
			Type = type;
		}

		public CEvent(string type, Object data)
		{
			Type = type;
			Data = data;
		}
	}
}
