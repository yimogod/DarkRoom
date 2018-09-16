using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.PCG;

namespace Sword
{
	public class SwordMetaParserManager : CMetaParserManager
	{
		public Action OnSingleParseComplete;

		public bool HasInitLite { get; private set; }
		public bool HasInitMain { get; private set; }

		public void InitLite()
		{
			if (HasInitLite) return;

			AddPaser("meta/race_meta.txt", new RaceMetaParser(OnSingleParseComplete));
			AddPaser("meta/class_meta.txt", new ClassMetaParser(OnSingleParseComplete));
			AddPaser("meta/actor_meta.txt", new ActorMetaParser(OnSingleParseComplete));
			HasInitLite = true;
		}

		public void InitMain()
		{
			if (HasInitMain) return;
			//AddPaser("meta/ability_meta.xml", new CAbilityMetaParser(OnSingleParseComplete));
			//AddPaser(meta/effect_meta.xml", new CEffectMetaParser(OnSingleParseComplete));
			//AddPaser("meta/buff_meta.xml", new CBuffMetaParser(OnSingleParseComplete));

			AddPaser("meta/map_meta.txt", new MapMetaParser(OnSingleParseComplete));
			AddPaser("meta/room_meta.txt", new CForestRoomMetaParser(OnSingleParseComplete));
			HasInitMain = true;
		}

		public override void Dispose()
		{
			base.Dispose();
			OnSingleParseComplete = null;
		}
	}
}