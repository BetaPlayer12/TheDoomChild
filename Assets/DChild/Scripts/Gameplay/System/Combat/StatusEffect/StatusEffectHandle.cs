using Holysoft.Event;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public struct StatusEffectReferenceEventArgs : IEventActionArgs
    {
        public StatusEffectReferenceEventArgs(StatusEffectHandle statusEffect) : this()
        {
            this.statusEffect = statusEffect;
        }

        public StatusEffectHandle statusEffect { get; }
    }

    public class StatusEffectHandle
    {
        [ShowInInspector, ReadOnly]
        private StatusEffectType m_type;
        [ShowInInspector, ReadOnly]
        private float m_duration;
        private IStatusEffectModule[] m_modules;
        private IStatusEffectUpdatableModule[] m_updatableModules;
        private bool m_hasUpdatableModules;
        private int m_moduleSize;
        [ShowInInspector, ReadOnly]
        private bool m_isActive;

        private Character m_character;
        public bool isActive { set => m_isActive = value; }

        public StatusEffectHandle(StatusEffectType m_type, float m_duration, IStatusEffectModule[] m_modules, IStatusEffectUpdatableModule[] m_updatableModules)
        {
            this.m_type = m_type;
            this.m_duration = m_duration;
            this.m_modules = m_modules;
            this.m_updatableModules = m_updatableModules;
            m_hasUpdatableModules = m_updatableModules != null || m_updatableModules.Length > 0;
            m_moduleSize = m_modules?.Length ?? 0;
        }

        public event EventAction<StatusEffectReferenceEventArgs> DurationExpired;

        public StatusEffectType type => m_type;

        public void Initialize(Character character)
        {
            m_character = character;
            for (int i = 0; i < m_updatableModules.Length; i++)
            {
                m_updatableModules[i].Initialize(character);
            }
        }

        public void StopEffect()
        {
            m_isActive = false;
            for (int i = 0; i < m_moduleSize; i++)
            {
                m_modules[i].Stop(m_character);
            }
        }

        public void StartEffect()
        {
            m_isActive = true;
            for (int i = 0; i < m_moduleSize; i++)
            {
                m_modules[i].Start(m_character);
            }
        }

        public void Update(float deltaTime)
        {
            if (m_isActive)
            {
                if (m_hasUpdatableModules)
                {
                    for (int i = 0; i < m_updatableModules.Length; i++)
                    {
                        m_updatableModules[i].Update(deltaTime);
                    }
                }

                if (m_duration >= 0)
                {
                    m_duration -= deltaTime;
                    if (m_duration <= 0)
                    {
                        DurationExpired?.Invoke(this, new StatusEffectReferenceEventArgs(this));
                    }
                }
            }
        }
    }
}