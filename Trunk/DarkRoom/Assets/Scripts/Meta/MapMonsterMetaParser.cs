using System;
using DarkRoom.Game;

namespace Sword{
	//自动生成关卡的怪物配置
	public class MapMonsterMetaParser:CMetaParser{
		public override void Execute (string content){
			base.Execute(content);

			for(int i = 0; i < m_reader.row; ++i){
				m_reader.MarkRow(i);

				MapMeta meta = MapMetaManager.GetMeta(m_reader.ReadString());

				meta.monster_0 = m_reader.ReadInt();
				meta.monster_0_lv = m_reader.ReadInt();
				meta.monster_0_ai = m_reader.ReadInt();
				meta.monster_0_num = m_reader.ReadInt();

				meta.monster_1 = m_reader.ReadInt();
				meta.monster_1_lv = m_reader.ReadInt();
				meta.monster_1_ai = m_reader.ReadInt();
				meta.monster_1_num = m_reader.ReadInt();

				meta.monster_2 = m_reader.ReadInt();
				meta.monster_2_lv = m_reader.ReadInt();
				meta.monster_2_ai = m_reader.ReadInt();
				meta.monster_2_num = m_reader.ReadInt();

				meta.monster_3 = m_reader.ReadInt();
				meta.monster_3_lv = m_reader.ReadInt();
				meta.monster_3_ai = m_reader.ReadInt();
				meta.monster_3_num = m_reader.ReadInt();

				meta.boss_0 = m_reader.ReadInt();
				meta.boss_0_lv = m_reader.ReadInt();
				meta.boss_0_ai = m_reader.ReadInt();

				meta.boss_1 = m_reader.ReadInt();
				meta.boss_1_lv = m_reader.ReadInt();
				meta.boss_1_ai = m_reader.ReadInt();
			}

		}
	}
}