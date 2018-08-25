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

            AddPaser("Meta/actor_born_attribute_meta", new ActorBornAttributeMetaParser());
            AddPaser("Meta/actor_meta", new ActorMetaParser());

            AddPaser("Meta/forest_room_meta", new CForestRoomMetaParser());
            AddPaser("Meta/map_meta", new MapMetaParser());
        }
    }
}