using System;
using DarkRoom.Game;

namespace Sword{

//自动生成关卡的trigger配置
public class MapTriggerMetaParser:CMetaParser{
	public override void Execute (string content){
		base.Execute(content);

		for(int i = 0; i < m_reader.row; ++i){
			m_reader.MarkRow(i);

			MapMeta meta = MapMetaManager.GetMeta(m_reader.ReadString());

			meta.chest_0 = m_reader.ReadInt();
			meta.chest_0_num = m_reader.ReadInt();
			meta.chest_1 = m_reader.ReadInt();
			meta.chest_1_num = m_reader.ReadInt();
			meta.chest_2 = m_reader.ReadInt();

			meta.trigger_0 = m_reader.ReadInt();
			meta.trigger_0_num = m_reader.ReadInt();
			meta.trigger_1 = m_reader.ReadInt();
			meta.trigger_1_num = m_reader.ReadInt();
			meta.trigger_2 = m_reader.ReadInt();
			meta.trigger_2_num = m_reader.ReadInt();
		}

	}
}

}