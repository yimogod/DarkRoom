using System;
using System.Collections.Generic;
using DarkRoom.UI;

namespace Sword
{
    public class CharacterChoose_Procedure : CProcedureBase
    {
        public const string NAME = "CharacterChoose";

        public CharacterChoose_Procedure() : base(NAME)
        {
            m_targetSceneName = SwordConst.CHARACTER_CHOOSE_SCENE;
        }
    }
}
