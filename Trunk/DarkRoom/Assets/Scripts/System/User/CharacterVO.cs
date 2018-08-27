using System;
using System.Collections.Generic;

namespace Sword
{
    public class CharacterVO
    {
        public static string GetSaveSlot(string name){
            return $"char_{name}";
        }

        public string Name;
        public int Level;
        public int Class;
        public int Race;

        public void Save()
        {
            var slot = GetSaveSlot(Name);
            ES3.Save<CharacterVO>(slot, this);
        }

        public static CharacterVO LoadOrCreate(string name)
        {
            var slot = GetSaveSlot(name);
            CharacterVO vo = null;

            if (ES3.KeyExists(slot))
            {
                vo = new CharacterVO {Name = name};
                vo.Save();
            }
            else
            {
                vo = ES3.Load<CharacterVO>(slot);
            }

            return vo;
        }
    }
}
