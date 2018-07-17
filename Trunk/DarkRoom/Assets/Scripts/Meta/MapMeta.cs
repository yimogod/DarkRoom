using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword{
	#region class
	public class MapMeta : CBaseMeta{
		public int Cols;
		public int Rows;
		public int type; // rush or comman
		public string MapRoot;// 地图的路径父目录 "Forest"

		//师傅随机地形高度
		public bool randomTerrainHeight = true;
		//用于产生树的自动机系数
		public int treeCellularPercent = 55;
		//障碍物的个数
		public int decoBlockNum = 60;
		//可被破坏的障碍物个数
		public int decoDestroyNum = 60;

		#region common_info
		public int monster_0;
		public int monster_0_lv;
		public int monster_0_ai;
		public int monster_0_num;

		public int monster_1;
		public int monster_1_lv;
		public int monster_1_ai;
		public int monster_1_num;

		public int monster_2;
		public int monster_2_lv;
		public int monster_2_ai;
		public int monster_2_num;

		public int monster_3;
		public int monster_3_lv;
		public int monster_3_ai;
		public int monster_3_num;

		public int boss_0;
		public int boss_0_lv;
		public int boss_0_ai;
		public int boss_1;
		public int boss_1_lv;
		public int boss_1_ai;

		public int trigger_0;
		public int trigger_0_num;
		public int trigger_1;
		public int trigger_1_num;
		public int trigger_2;
		public int trigger_2_num;

		public int chest_0;
		public int chest_0_num;

		public int chest_1;
		public int chest_1_num;

		//chest2是黄金宝箱. 只有能一个.且会有boss_0在它旁边
		public int chest_2;
		public int chest_2_num = 1;
		#endregion

		public MapMeta(string id):base(id){}
	}
	#endregion

	#region manager
	public class MapMetaManager{
		//关卡信息
		public static Dictionary<string, MapMeta> m_dict = new Dictionary<string, MapMeta>();

		public MapMetaManager (){}

		public static void AddMeta(MapMeta meta){
			m_dict.Add(meta.Id, meta);
		}

		public static MapMeta GetMeta(string id){
			return m_dict[id];
		}
	}
	#endregion

	#region parser
	public class MapMetaParser: CMetaParser{
		public override void Execute (string content){
			base.Execute(content);

			for(int i = 0; i < m_reader.row; ++i){
				m_reader.MarkRow(i);

				MapMeta meta = new MapMeta(m_reader.ReadString());
				meta.NameKey = m_reader.ReadString(); 
				meta.MapRoot = m_reader.ReadString();

				meta.Cols = m_reader.ReadInt();
				meta.Rows = m_reader.ReadInt();

				meta.type = m_reader.ReadInt();
				meta.randomTerrainHeight = m_reader.ReadBool();
				meta.treeCellularPercent = m_reader.ReadInt();
				meta.decoBlockNum = m_reader.ReadInt();
				meta.decoDestroyNum = m_reader.ReadInt();

				int maxBlockNum = (int)(meta.Cols * meta.Rows * 0.7f);
				if((meta.decoBlockNum + meta.decoDestroyNum) > maxBlockNum) {
					UnityEngine.Debug.LogError("Deco block num > map col x row in " + meta.Id);
					continue;
				}

				MapMetaManager.AddMeta(meta);
			}
		}
	}
	#endregion
}