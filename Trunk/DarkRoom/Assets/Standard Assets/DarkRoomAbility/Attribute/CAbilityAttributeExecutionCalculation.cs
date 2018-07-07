﻿using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// 具体计算属性修改的类
    /// Buff/Effect通过本类的子类来实现对属性的修改
    /// </summary>
    public class CAbilityAttributeExecutionCalculation
    {
        public string HashName
        {
            get { return "BaseCalculation"; }
        }

        public virtual void Execute(IGameplayAbilityUnit instigator, IGameplayAbilityUnit victim)
        {

        }
    }
}
