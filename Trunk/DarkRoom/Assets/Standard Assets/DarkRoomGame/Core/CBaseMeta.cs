using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 模版数据, 我们的所有Pawn之类的原始数据都来源于此
	/// </summary>
	public class CBaseMeta {
		//有些meta需要int的id 
		private string m_idKey;

		/// <summary>
		/// name display
		/// </summary>
		public string NameKey;

		public CBaseMeta(int id){ m_idKey = id.ToString(); }
		public CBaseMeta(string id) { m_idKey = id; }

		//有些需要string的id
		public string Id {
			get { return m_idKey; }
		}

	    public int nId
	    {
	        get
	        {
	            int v = -1;
	            bool r = int.TryParse(m_idKey, out v);
	            if (!r)Debug.LogWarning(string.Format("{0} can not format to int in {1}", m_idKey, GetType().FullName));

	            return v;
	        }
	    }

		public string Name{
			get{ return NameKey; }
		}
	}
}