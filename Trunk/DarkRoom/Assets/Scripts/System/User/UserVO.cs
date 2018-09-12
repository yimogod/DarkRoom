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
        public bool HasAnyCharacter => CharacterNameList.Count > 0;

        public UserVO()
        {
        }

        /// <summary>
        /// 如果当前角色为空, 就找一个
        /// </summary>
        public void FindCurrentCharacter(){
            if (!HasAnyCharacter) return;
            if (!string.IsNullOrEmpty(CurrentCharacterName)) return;

            CurrentCharacterName = CharacterNameList[0];
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
                vo = ES3.Load<UserVO>(m_saveSlot);
                vo.FindCurrentCharacter();
            }
            else
            {
                vo = new UserVO();
            }

            vo.Save();
            return vo;
        }
    }
}
