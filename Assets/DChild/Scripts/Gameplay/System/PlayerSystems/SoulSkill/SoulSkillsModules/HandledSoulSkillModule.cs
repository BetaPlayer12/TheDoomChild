using System.Collections.Generic;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public abstract class HandledSoulSkillModule : ISoulSkillModule
    {
        protected abstract class SoulSkillHandleManager
        {
            public abstract void AddHandle(IPlayer refernce, BaseHandle handle);
            public abstract void RemoveHandle(IPlayer reference);
        }

        protected class SinglePlayerMananger : SoulSkillHandleManager
        {
            private BaseHandle m_instance;

            public override void AddHandle(IPlayer refernce, BaseHandle handle)
            {
                m_instance?.Dispose();
                handle.Initialize();
                m_instance = handle;
            }

            public override void RemoveHandle(IPlayer reference)
            {
                m_instance.Dispose();
                m_instance = null;
            }
        }

        protected abstract class BaseHandle
        {
            public abstract void Initialize();
            public abstract void Dispose();
        }

        private SoulSkillHandleManager m_manager;
        private bool m_isInitialized;

        public void AttachTo(IPlayer player)
        {
            if (m_isInitialized == false)
            {
                m_manager = new SinglePlayerMananger();
                m_isInitialized = true;
            }

            var handle = CreateHandle(player);
            m_manager.AddHandle(player, handle);
        }

        public void DetachFrom(IPlayer player)
        {
            m_manager.RemoveHandle(player);
        }

        protected abstract BaseHandle CreateHandle(IPlayer player);
    }
}