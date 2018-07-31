using System;
using System.Collections.Generic;
using System.Xml;
using DarkRoom.Game;
using DarkRoom.PCG;
using UnityEngine;

namespace Sword{
	#region class
	public class MapMeta : CBaseMeta{
		public int Cols;
		public int Rows;

        //x = id, y = lv  z = num
        public List<Vector3Int> Monsters = new List<Vector3Int>();
	    public List<Vector3Int> Bosses = new List<Vector3Int>();
	    public List<Vector3Int> Triggers = new List<Vector3Int>();
        //第一个应该是黄金宝箱, 在boss旁边
	    public List<Vector2Int> Chests = new List<Vector2Int>();

		public MapMeta(string id):base(id){}
	}
	#endregion

	#region manager
	public class MapMetaManager{
		//关卡信息
		public static Dictionary<int, MapMeta> m_dict = new Dictionary<int, MapMeta>();

		public static void AddMeta(MapMeta meta){
			m_dict.Add(meta.Id, meta);
		}

		public static MapMeta GetMeta(int id){
			if(!m_dict.ContainsKey(id)){
				Debug.LogError(id + " map meta does not exist!");
				return null;
			}
			return m_dict[id];
		}
	}
	#endregion

	#region parser
	public class MapMetaParser: CMetaParser{
		public override void Execute (string content){
			base.Execute(content);
		    m_xreader.ReadRootNode();

		    foreach (XmlElement node in m_xreader.rootChildNodes)
		    {
		        MapMeta meta = new MapMeta(node.GetAttribute("id"));
		        meta.NameKey = node.GetAttribute("name");
                meta.Cols = int.Parse(node.GetAttribute("cols"));
		        meta.Rows = int.Parse(node.GetAttribute("rows"));

		        var monsterRoot = node.SelectSingleNode("Monsters");
                m_xreader.TryReadChildNodesAttr(monsterRoot, "Monster", meta.Monsters);

		        MapMetaManager.AddMeta(meta);
		    }

        }
	}
	#endregion
}