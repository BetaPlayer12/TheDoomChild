using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public partial class Electicution : IStatusEffectUpdatableModule
    {
        [SerializeField, MinValue(0.1)]
        private float m_interval;
        [SerializeField, MinValue(0.1)]
        private float m_stunDuration;

        private float m_currentTimer;
        private bool m_isStunned;

        public Electicution()
        {
            m_interval = 1;
            m_stunDuration = 1;
        }

        public Electicution(float interval, float stunDuration)
        {
            m_interval = interval;
            m_stunDuration = stunDuration;
        }

        public void Initialize(Character character) 
        {
            m_currentTimer = 0;
            StartStun();
        }

        public void Update(float delta)
        {
            if (m_currentTimer <= 0)
            {
                if (m_isStunned)
                {
                    EndStun();
                    m_currentTimer += m_interval;
                    m_isStunned = false;
                }
                else
                {
                    StartStun();
                    m_currentTimer += m_stunDuration;
                    m_isStunned = true;
                }
            }
            else
            {
                m_currentTimer -= delta;
            }
        }

        public IStatusEffectUpdatableModule CreateCopy() => new Electicution(m_interval, m_stunDuration);

        private void StartStun()
        {
            GameplaySystem.playerManager.OverrideCharacterControls();
        }

        private void EndStun()
        {
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly]
        private int m_totalStuns;

        public void CalculateWithDuration(float duration)
        {
            m_totalStuns = 0;
            var timeElapse = 0f;
            do
            {
                m_totalStuns++;
                timeElapse = m_interval + m_stunDuration;
            } while (timeElapse <= duration);

        }
#endif
    }
}