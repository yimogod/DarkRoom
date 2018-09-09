using System;
using System.Collections.Generic;
using  DarkRoom.Game;

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
            //AddPaser("Meta/ability_meta", new CAbilityMetaParser());
            //AddPaser("Meta/effect_meta", new CEffectMetaParser());
            //AddPaser("Meta/buff_meta", new CBuffMetaParser());

            //AddPaser("Meta/forest_room_meta", new CForestRoomMetaParser());
            //AddPaser("Meta/map_meta", new MapMetaParser());
            HasInitMain = true;
        }
    }
}