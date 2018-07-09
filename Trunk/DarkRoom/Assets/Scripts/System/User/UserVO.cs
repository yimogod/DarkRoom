namespace Sword
{
    public class UserVO
    {
        public int id;
        public string name;
        public string pwd;
        public string token;

        public int coin;
        public int gold;
        public int energy;

        /* if first user */
        public bool created;

        public UserVO() { }
    }
}

