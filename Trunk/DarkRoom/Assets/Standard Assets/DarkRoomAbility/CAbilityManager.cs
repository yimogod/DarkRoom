using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// 提供一些全局技能系统需要的数据和方法
    /// </summary>
    public class CAbilityManager : CSingleton<CAbilityManager>
    {
        /// <summary>
        /// 存储我们自定义的计算器
        /// </summary>
        private Dictionary<string, Type> m_calculationTypeDict = new Dictionary<string, Type>();

        public CAbilityAttributeExecutionCalculation FindCalculation(string key)
        {
            if (!m_calculationTypeDict.ContainsKey(key))
            {
                Debug.LogErrorFormat("{0} has not Registered in AbilityManager, Check The Name", key);
                return null;
            }

            var t = m_calculationTypeDict[key];
            var instance = Activator.CreateInstance(t) as CAbilityAttributeExecutionCalculation;
            return instance;
        }

        public bool RegisterCalculation(string key, Type value)
        {
            if (m_calculationTypeDict.ContainsKey(key))
            {
                Debug.LogErrorFormat("{0} has already Registered in AbilityManager", key);
                return false;
            }

            m_calculationTypeDict[key] = value;
            return true;
        }
    }
}
