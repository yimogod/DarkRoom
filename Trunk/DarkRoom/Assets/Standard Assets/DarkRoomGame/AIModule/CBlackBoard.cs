using System;
using System.Collections.Generic;

namespace DarkRoom.AI {
	/// <summary>
	/// 黑板, 存储着AI需要的一些数据.从一些文章得知一个游戏一个黑板足够
	/// </summary>
	public class CBlackBoard {
		//存储int值的数据
		private Dictionary<string, int> m_intDict = new Dictionary<string, int>();

		//存储bool值的数据
		private Dictionary<string, bool> m_boolDict = new Dictionary<string, bool>();

		//存储object对象的数据
		private Dictionary<string, Object> m_objDict = new Dictionary<string, Object>();

		public void SetValue(string key, int value)
		{
			m_intDict[key] = value;
		}

		public int GetInt(string key)
		{
			int v = 0;
			m_intDict.TryGetValue(key, out v);
			return v;
		}

		public void SetValue(string key, bool value) {
			m_boolDict[key] = value;
		}

		public bool GetBool(string key) {
			bool v = false;
			m_boolDict.TryGetValue(key, out v);
			return v;
		}

		public void SetValue(string key, Object value) {
			m_objDict[key] = value;
		}

		public Object GetObject(string key) {
			Object v = null;
			m_objDict.TryGetValue(key, out v);
			return v;
		}


	}
}
