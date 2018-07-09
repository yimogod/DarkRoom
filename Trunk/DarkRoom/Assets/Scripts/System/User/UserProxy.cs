using System;

using PureMVC.Patterns;
using PureMVC.Interfaces;

namespace Sword
{
    public class UserProxy : Proxy, IProxy
    {
        public const string NAME = "UserProxy";

        public UserVO user = new UserVO();

        public UserProxy() : base(NAME) { }

        public bool HasEnoughCoin(int coin)
        {
            return user.coin >= coin;
        }

        public bool HasEnoughGold(int gold)
        {
            return user.gold >= gold;
        }

        public void AddCoin(int value)
        {
            user.coin += value;
        }

        //消费
        public void ConsumeCoin(int value)
        {
            user.coin -= value;
            if (user.coin < 0) user.coin = 0;
        }
    }
}

