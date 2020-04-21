using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [SelectionBase]
    [AddComponentMenu("DChild/Gameplay/Combat/Damageable")]
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

        protected Hitbox[] m_hitboxes;

        public event EventAction<DamageEventArgs> DamageTaken;
        public event EventAction<EventActionArgs> Destroyed;

        public Vector2 position => m_centerMass.position;

        public bool isAlive => !m_health?.isEmpty ?? true;

        public IAttackResistance attackResistance => m_resistance;

        public Health health => m_health;

        public void TakeDamage(int totalDamage, AttackType type)
        {
            m_health?.ReduceCurrentValue(totalDamage);
            var eventArgs = new DamageEventArgs(totalDamage, type);
            DamageTaken?.Invoke(this, eventArgs);
            if (m_health?.isEmpty ?? false)
            {
                Debug.Log("dead");
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

        public void SetInvulnerability(bool enable)
        {
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].SetInvulnerability(enable);
            }
        }

        private void Awake()
        {
            m_hitboxes = GetComponentsInChildren<Hitbox>();
            m_health.ResetValueToMax();
        }

#if UNITY_EDITOR
        public void InitializeField(Transform centermass,Health health)
        {
            m_centerMass = centermass;
            m_health = health;
        }

        public void InitializeField(AttackResistance resistance)
        {
            m_resistance = resistance;
        }
#endif
    }
}