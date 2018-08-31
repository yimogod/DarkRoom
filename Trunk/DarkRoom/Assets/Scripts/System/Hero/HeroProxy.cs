using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using UnityEngine;

namespace Sword
{
    public class HeroProxy : Proxy, IProxy
    {
        public const string NAME = "HeroProxy";

        public static string GetSaveSlot(string name)
        {
            return $"hero_{name}";
        }

        /// <summary>
        /// 玩家当前的英雄
        /// </summary>
        public HeroVO Hero = null;

        public HeroProxy() : base(NAME) { }

        /// <summary>
        /// 保存英雄到数据库
        /// </summary>
        public void SaveHero()
        {
            var slot = GetSaveSlot(Hero.Name);
            ES3.Save<HeroVO>(slot, Hero);
        }

        public static HeroVO Load(string name)
        {
            var slot = GetSaveSlot(name);
            HeroVO vo = null;

            if (ES3.KeyExists(slot))
            {
                vo = ES3.Load<HeroVO>(slot);
            }

            return vo;
        }

        public void CreateHero(string name, int metaClass, int race)
        {
            HeroVO vo = new HeroVO
            {
                Name = name,
                Class = metaClass,
                Race = race,
                Level = 1,
                AttributePoint = 5,
                SkillPoint = 0,
            };
            Hero = vo;
            SaveHero();
        }

        /// <summary>
        /// 创建英雄的GO, 永不销毁
        /// </summary>
        public void CreateHeroEntity()
        {
            GameObject go = new GameObject("__" + Hero.Name);
            go.AddComponent<CGlobalExistComp>();
            var entity = go.AddComponent<HeroEntity>();
            entity.Team = CUnitEntity.TeamSide.Red;

            var attr = entity.AttributeSet;
            attr.InitAttr((ActorClass)Hero.Class, (ActorRace)Hero.Race);
            attr.SetPrimaryAttrPersistentValue(Hero.Strength, Hero.Dexterity, Hero.Constitution,
                                              Hero.Magic, Hero.Willpower, Hero.Cunning, Hero.Luck);
            attr.InitSubAttr();
            attr.InitLevel(Hero.Level);

            //attr.InitHealthAndMana();

            //目前我们仅仅英雄会有不同的武器
            //gameObject.AddComponent<HeroFSMComp>();
            //gameObject.AddComponent<HeroControlKeyboard>();
        }
    }
}

