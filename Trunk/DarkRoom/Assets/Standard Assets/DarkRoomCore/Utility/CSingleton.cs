﻿namespace DarkRoom.Core
{
    public abstract class CSingleton<T> where T : new()
    {
        protected static T m_instance;
        private static object m_lock = new object();

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if(m_instance == null)  
                            m_instance = new T();
                    }
                }  
                return m_instance;  
            }  
        }  
    }  
}
