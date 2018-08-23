using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 模版数据, 我们的所有Pawn之类的原始数据都来源于此
	/// </summary>
	public class CBaseMeta {
		/// <summary>
		/// name display
		/// </summary>
		public string NameKey;

	    public string sId { get; set; } = string.Empty;

	    public int Id { get; set; } = -1;

        public CBaseMeta(int id)
	    {
	        Id = id;
	    }

	    public CBaseMeta(string id)
	    {
	        sId = id;

	        int nid = -1;
	        int.TryParse(id, out nid);
	        Id = nid;
	    }

		public virtual string Name{
			get{ return NameKey; }
		}
	}
}