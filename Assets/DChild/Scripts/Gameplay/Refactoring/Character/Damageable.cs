using DChild.Gameplay.Combat;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Combat
{
    public class Damageable : MonoBehaviour, IDamageable, ITarget, IHealable
    {
        public struct DamageEventArgs : IEventActionArgs
        {
            public DamageEventArgs(int damage, AttackType type) : this()
            {
                this.damage = damage;
                this.type = type;
            }

            public int damage { get; }
            public AttackType type { get; }
        }

        [SerializeField]
        private Transform m_centerMass;
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private AttackResistance m_resistance;
        [SerializeField]
        private FlinchHandler m_flinchHandler;

        protected Hitbox[] m_hitboxes;

        public event EventAction<DamageEventArgs> DamageTaken;
        public event EventAction<EventActionArgs> Destroyed;

        public Vector2 position => m_centerMass.position;

        public bool isAlive => !m_health?.isEmpty ?? true;

        public IAttackResistance attackResistance => m_resistance;

        public IFlinch flinchHandler => m_flinchHandler;

        public void TakeDamage(int totalDamage, AttackType type)
        {
            m_health?.ReduceCurrentValue(totalDamage);
            var eventArgs = new DamageEventArgs(totalDamage, type);
            DamageTaken?.Invoke(this, eventArgs);
            if (m_health?.isEmpty ?? false)
            {
                Destroyed?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void Heal(int health)
        {
            m_health?.AddCurrentValue(health);
        }

        public void SetHitboxActive(bool enable)
        {
            if (enable)
            {
                for (int i = 0; i < m_hitboxes.Length; i++)
                {
                    m_hitboxes[i].Enable();
                }
            }
            else
            {
                for (int i = 0; i < m_hitboxes.Length; i++)
                {
                    m_hitboxes[i].Disable();
                }
            }
        }

        private void Awake()
        {
            m_hitboxes = GetComponentsInChildren<Hitbox>();
            m_health.ResetValueToMax();
        }
    }
}