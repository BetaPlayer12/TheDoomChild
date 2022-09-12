using System.Collections.Generic;

namespace DChild.Gameplay
{
    public abstract class GameplayModuleManager<T> : IGameplayModuleManager
    {
        protected static List<T> m_list = new List<T>();

        private static bool m_hasInstance;

        public static bool hasInstance => m_hasInstance;

        public abstract string name { get; }

        void IGameplayModuleManager.SetInstance(IGameplayModuleManager instance)
        {
            m_hasInstance = true;
        }

        public static void Register(T instance)
        {
            if (m_list.Contains(instance) == false)
            {
                m_list.Add(instance);
            }
        }

        public static void Unregister(T instance)
        {
            if (m_list.Contains(instance))
            {
                m_list.Remove(instance);
            }
        }

    }
}
