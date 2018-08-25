using System;

namespace Sword
{
    public class SwordConfig
    {
        public static string URL = "http://127.0.0.1:8000/";
        public static string URL_LOGIN = URL + "account/login/";

        public static string URL_USER_INFO = URL + "hero/get_hero_info/";
        public static string UPDATE_HERO_INFO = URL + "hero/update/";
        public static string UPGRADE_EQUIP = URL + "hero/upgrade_equip/";

        public static string KILL_MONSTER = URL + "game/kill/";

        public static string USE_ITEM = URL + "item/use/";
        public static string BUY_ITEM = URL + "item/buy/";


        //是否使用本地db
        public static bool USE_LOCAL_DB = true;
    }
}