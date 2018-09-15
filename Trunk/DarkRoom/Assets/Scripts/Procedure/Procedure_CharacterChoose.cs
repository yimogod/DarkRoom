using System;
using System.Collections.Generic;
using DarkRoom.UI;

namespace Sword
{
    public class Procedure_CharacterChoose : CProcedureBase
    {
        public const string NAME = "CharacterChoose";

        public Procedure_CharacterChoose() : base(NAME)
        {
            m_targetSceneName = SwordConst.CHARACTER_CHOOSE_SCENE;
        }
    }
}
