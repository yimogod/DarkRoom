using System;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;

namespace Sword
{
    public class UserProxy : Proxy, IProxy
    {
        public const string NAME = "UserProxy";

        public UserVO User = null;

        /// <summary>
        /// 玩家当前或者说上一局玩的角色
        /// </summary>
        public CharacterVO Character = null;

        /// <summary>
        /// 玩家创建所有的角色
        /// </summary>
        public List<CharacterVO> CharacterList = null;

        public UserProxy() : base(NAME) { }

        public bool HasEnoughCoin(int coin)
        {
            return User.coin >= coin;
        }

        public bool HasEnoughGold(int gold)
        {
            return User.gold >= gold;
        }

        public void AddCoin(int value)
        {
            User.coin += value;
        }

        //消费
        public void ConsumeCoin(int value)
        {
            User.coin -= value;
            if (User.coin < 0) User.coin = 0;
        }
    }
}

