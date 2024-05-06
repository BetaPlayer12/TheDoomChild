using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters;
using UnityEngine;
using Sirenix.OdinInspector;
using Holysoft.Gameplay;

namespace DChild.Gameplay.Combat
{
    [SelectionBase]
    [AddComponentMenu("DChild/Gameplay/Combat/Damageable")]
    public class Damageable : MonoBehaviour, IDamageable, ITarget, IHealable
    {
        public struct DamageEventArgs : IEventActionArgs
        {
            public DamageEventArgs(int damage, DamageType type) : this()
            {
                this.damage = damage;
                this.type = type;
            }

            public int damage { get; }
            public DamageType type { get; }
        }

        [SerializeField]
        private Transform m_centerMass;
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private AttackResistance m_resistance;

        protected Hitbox[] m_hitboxes;

        public event EventAction<DamageEventArgs> DamageTaken;
        public event EventAction<DamageEventArgs> DamageBlock;
        public event EventAction<EventActionArgs> Destroyed;
        public event EventAction<EventActionArgs> Healed;

        public Vector2 position => m_centerMass.position;

        public bool isAlive => !m_health?.isEmpty ?? true;

        public IAttackResistance attackResistance => m_resistance;

        public Health health => m_health;

        ICappedStatInfo IDamageable.health => m_health;

        public virtual void TakeDamage(int totalDamage, DamageType type)
        {
            m_health?.ReduceCurrentValue(totalDamage);
            CallDamageTaken(totalDamage, type);
            if (m_health?.isEmpty ?? false)
            {
                CallDestroyed();
            }
        }

        public virtual void BlockDamage(int totalDamage, DamageType type)
        {
            CallDamageBlock(totalDamage, type);
        }

        public void Heal(int health)
        {
            m_health?.AddCurrentValue(health);
            Healed?.Invoke(this, EventActionArgs.Empty);
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

        public Hitbox[] GetHitboxes() => m_hitboxes;

        public void SetInvulnerability(Invulnerability level)
        {
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].SetInvulnerability(level);
            }
        }

        protected void CallDamageTaken(int totalDamage, DamageType type)
        {
            var eventArgs = new DamageEventArgs(totalDamage, type);
            DamageTaken?.Invoke(this, eventArgs);
        }

        protected void CallDamageBlock(int totalDamage, DamageType type)
        {
            var eventArgs = new DamageEventArgs(totalDamage, type);
            DamageBlock?.Invoke(this, eventArgs);
        }

        protected void CallDestroyed()
        {
            Destroyed?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_hitboxes = GetComponentsInChildren<Hitbox>();
            if (m_health != null)
            {
                m_health.ResetValueToMax();
            }
        }

        [Button, ShowIf("isAlive"), HideInEditorMode]
        public void KillSelf()
        {
            TakeDamage(999999999, DamageType.True);
        }

#if UNITY_EDITOR
        public void InitializeField(Transform centermass, Health health)
        {
            m_centerMass = centermass;
            m_health = health;
        }

        public void InitializeField(AttackResistance resistance)
        {
            m_resistance = resistance;
        }

        [Button, HideIf("isAlive"), HideInEditorMode]
        public void RessurectSelf()
        {
            Heal(999999999);
        }
        //public void RecieveParentAttacker()
        //{
        //    var recieveParentAttacker = GetComponent<Attacker>();
        //    if(recieveParentAttacker.parentAttacker != null)
        //    {
        //        if(recieveParentAttacker.rootParentAttacker != null)
        //        {
        //            var rootParentAttacker = recieveParentAttacker.rootParentAttacker;
        //        }
        //        else
        //        {
        //            var parentAttacker = recieveParentAttacker.parentAttacker;
        //        }
        //    }
        //}


#endif
    }
}