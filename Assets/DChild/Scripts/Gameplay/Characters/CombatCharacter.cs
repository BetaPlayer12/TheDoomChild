using Holysoft;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.WorldComponents;
using Sirenix.OdinInspector;
using UnityEngine;
using Holysoft.Event;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using PixelCrushers;

namespace DChild.Gameplay.Characters
{
    public abstract class CombatCharacter : Actor, IAttacker, ITarget, IFacing, IFacingConfigurator,
                                            ICombatCharacterInfo, IDamageable
    {
        protected Hitbox[] m_hitboxes;

        [SerializeField, TitleGroup("References"), Indent]
        protected Transform m_model;
        protected HorizontalDirection m_facing;
        protected IsolatedObject m_objectTime;

        public event EventAction<CombatConclusionEventArgs> TargetDamaged;
        public event EventAction<EventActionArgs> Attacks;
        public event EventAction<EventActionArgs> Damaged;
        public event EventAction<Damageable.DamageEventArgs> DamageTaken;
        public event EventAction<EventActionArgs> Destroyed;
        public event EventAction<Damageable.DamageEventArgs> DamageBlock;

        public Vector2 position => m_model.position;
        public HorizontalDirection currentFacingDirection => m_facing;
        public IIsolatedTime time => m_objectTime;

        public virtual float critDamageModifier => 1f;
        public abstract bool isAlive { get; }
        public abstract IAttackResistance attackResistance { get; }
        public abstract void Heal(int health);
        public abstract void TakeDamage(int totalDamage, AttackType type);
        public abstract void SetFacing(HorizontalDirection facing);

        public abstract void DisableController();
        public abstract void EnableController();

        public void EnableHitboxes()
        {
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].Enable();
            }
        }

        public void DisableHitboxes()
        {
            Debug.Log("Hitbox: " + m_hitboxes.Length);
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].Disable();
            }
        }

        public void TurnCharacter()
        {
            if (m_facing == HorizontalDirection.Left)
            {
                SetFacing(HorizontalDirection.Right);
            }
            else
            {
                SetFacing(HorizontalDirection.Left);
            }
        }

        protected void CallAttackerAttacked(CombatConclusionEventArgs eventArgs) => TargetDamaged?.Invoke(this, eventArgs);
        protected void CallDamaged() => Damaged?.Invoke(this, EventActionArgs.Empty);

        public void SetHitboxActive(bool enable)
        {
            throw new System.NotImplementedException();
        }

        public void SetInvulnerability(bool isInvulnerable)
        {
            throw new System.NotImplementedException();
        }

        public void SetInvulnerability(Invulnerability level)
        {
            throw new System.NotImplementedException();
        }

        public void BlockDamage(int totalDamage, AttackType type)
        {
            throw new System.NotImplementedException();
        }

        public Hitbox[] GetHitboxes()
        {
            throw new System.NotImplementedException();
        }
    }
}
