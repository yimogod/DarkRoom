using System.Collections.Generic;

namespace Sword
{
    public class UserVO
    {
        private static string m_saveSlot = "sword_user";

        public string CurrentCharacterName;
        public List<string> CharacterNameList = new List<string>();

        public int coin;
        public int gold;
        public int energy;

        /// <summary>
        /// 是否有之前玩的玩家
        /// </summary>
        public bool HasCurrentCharacter => CharacterNameList.Count > 0;

        public UserVO()
        {
        }

        public void Save()
        {
            ES3.Save<UserVO>(m_saveSlot, this);
        }

        public static UserVO LoadOrCreate()
        {
            UserVO vo = null;
            if (ES3.KeyExists(m_saveSlot))
            {
                vo = new UserVO();
                vo.Save();
            }
            else
            {
                vo = ES3.Load<UserVO>(m_saveSlot);
                if (string.IsNullOrEmpty(vo.CurrentCharacterName) && vo.HasCurrentCharacter)
                {
                    vo.CurrentCharacterName = vo.CharacterNameList[0];
                }
            }

            return vo;
        }
    }
}
