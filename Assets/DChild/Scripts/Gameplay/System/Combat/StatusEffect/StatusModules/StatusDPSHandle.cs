using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public class StatusDPSHandle : IStatusEffectUpdatableModule
    {
        [SerializeField, MinValue(0.1)]
        private float m_interval;
        [SerializeField]
        private Damage m_damagePerInterval;

        private IDamageable m_damageable;
        private float m_currentTimer;

        public StatusDPSHandle()
        {
            m_interval = 1;
            m_damagePerInterval = new Damage(DamageType.Physical, 1);
        }

        public StatusDPSHandle(float m_interval, Damage m_damagePerInterval)
        {
            this.m_interval = m_interval;
            this.m_damagePerInterval = m_damagePerInterval;
        }

        public void Initialize(Character character)
        {
            m_damageable = character.GetComponent<IDamageable>();
            m_currentTimer = 0;
        }

        public void Update(float delta)
        {
            if (m_currentTimer <= 0)
            {
                GameplaySystem.combatManager.Damage(m_damageable, m_damagePerInterval);
                m_currentTimer += m_interval;
            }
            else
            {
                m_currentTimer -= delta;
            }
        }

        public IStatusEffectUpdatableModule CreateCopy() => new StatusDPSHandle(m_interval, m_damagePerInterval);

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly]
        private int m_totalDamage;

        public void CalculateWithDuration(float duration)
        {
            m_totalDamage = Mathf.FloorToInt(duration / m_interval) * m_damagePerInterval.value;
        }
#endif
    }
}