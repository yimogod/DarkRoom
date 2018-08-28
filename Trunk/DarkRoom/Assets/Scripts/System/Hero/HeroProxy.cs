using System;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;

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
            };
            Hero = vo;
            SaveHero();
        }
    }
}

