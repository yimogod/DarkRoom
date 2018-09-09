using System;
using System.Collections.Generic;
using  DarkRoom.Game;
using DarkRoom.PCG;
using UnityEngine;

namespace Sword
{
    public class SwordMetaParserManager : CMetaParserManager
    {
        public override void Initialize()
        {
            //AddPaser("Meta/ability_meta", new CAbilityMetaParser());
            //AddPaser("Meta/effect_meta", new CEffectMetaParser());
            //AddPaser("Meta/buff_meta", new CBuffMetaParser());

            AddPaser("meta/race_meta.txt", new RaceMetaParser(), true);
            AddPaser("meta/class_meta.txt", new ClassMetaParser(), true);
            AddPaser("meta/actor_meta.txt", new ActorMetaParser(), true);

            //AddPaser("Meta/forest_room_meta", new CForestRoomMetaParser());
            //AddPaser("Meta/map_meta", new MapMetaParser());
        }
    }
}