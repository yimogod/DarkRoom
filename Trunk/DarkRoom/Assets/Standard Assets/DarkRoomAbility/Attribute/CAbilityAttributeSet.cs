using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// 血, 蓝, 力, 敏, 耐等的属性定义
    /// </summary>
    public class CAbilityAttribute
    {

    }

    public class CAbilityAttributeSet
    {
        /// <summary>
        /// 在属性修改之前执行
        /// </summary>
        public virtual bool PreAttributeExecute()
        {
            return false;
        }

        /// <summary>
        /// 对属性修改的实际操作
        /// </summary>
        public virtual bool AttributeExecute()
        {
            return false;
        }

        public virtual void PreAttributeChange(CAbilityAttribute attribute, float newValue)
        {

        }
    }
}
