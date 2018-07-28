using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// 血, 蓝, 力, 敏, 耐等的属性定义
    /// </summary>
    public class CAbilityAttribute
    {
        //属性定义的唯一值
        public int Propery { get; private set; }
        //属性当前的值
        public float Value { get; private set; }

        public CAbilityAttribute(int property, float defaultValue = 0)
        {
            Propery = property;
            Value = defaultValue;
        }

        public void SetValue(float value)
        {
            Value = value;
        }
    }

    public class CAbilityAttributeSet
    {
        /// <summary>
        /// 对属性的预处理
        /// </summary>
        public virtual void PreAttributeChange(CAbilityAttribute attribute, float newValue)
        {

        }

        /// <summary>
        /// 对属性修改的实际操作
        /// </summary>
        public virtual void PostAttributeExecute(CAbilityAttribute attribute)
        {
        }
    }
}
