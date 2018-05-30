namespace DarkRoom.Game {
	/// <summary>
	/// 模版数据, 我们的所有Pawn之类的原始数据都来源于此
	/// </summary>
	public class CBaseMeta {
		//有些meta需要int的id 
		public int Id = 0;
		private string m_idKey;

		/// <summary>
		/// name display
		/// </summary>
		public string NameKey;

		public CBaseMeta(int id){ Id = id; }
		public CBaseMeta(string id) { IdKey = id; }

		//有些需要string的id
		public string IdKey {
			get { return m_idKey; }
			set {
				m_idKey = value;
				int.TryParse(m_idKey, out Id);
			}

		}

		public string Name{
			get{ return NameKey; }
		}
	}
}