using System.Collections.Generic;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public abstract class HandledSoulSkillModule : ISoulSkillModule
    {
        protected abstract class BaseHandle
        {
            public abstract void Initialize();
            public abstract void Dispose();
        }

        private Dictionary<IPlayer, BaseHandle> m_handles;
        private bool m_isInitialized;

        public void AttachTo(IPlayer player)
        {
            if (m_isInitialized == false)
            {
                m_handles = new Dictionary<IPlayer, BaseHandle>();
                m_isInitialized = true;
            }

            if (m_handles.ContainsKey(player) == false)
            {
                var handle = CreateHandle(player);
                handle.Initialize();
                m_handles.Add(player, handle);
            }
        }

        public void DetachFrom(IPlayer player)
        {
            m_handles[player].Dispose();
            m_handles.Remove(player);
        }

        protected abstract BaseHandle CreateHandle(IPlayer player);
    }
}