
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class AOEProjectile : Projectile 
    {
        [SerializeField, FoldoutGroup("Explosion"), MinValue(1f)]
        protected float m_damageRadius = 3;
        [SerializeField, FoldoutGroup("Explosion"), MinValue(1f), Tooltip("Radius on how far objects react to the explosion")]
        protected float m_explosiveRadius;
        [SerializeField, FoldoutGroup("Explosion")]
        protected float m_explosivePower;

        [SerializeField, HideInInspector]
        protected Rigidbody2D m_rigidbody;
        [SerializeField, HideInInspector]
        private CircleCollider2D m_collider;

        private List<ITarget> m_toDamage;
        private static List<Hitbox> m_cacheHitboxList;
        private static Hitbox m_cacheHitbox;

        public event EventAction<AOETargetsEventArgs> OnDetonate;

        public virtual void Detonate()
        {
            m_cacheHitboxList = GameplaySystem.combatManager.GetValidTargetsOfCircleAOE(m_rigidbody.position, m_damageRadius, Physics2D.GetLayerCollisionMask(gameObject.layer));
            m_toDamage.Clear();
            for (int i = 0; i < m_cacheHitboxList.Count; i++)
            {
                if (IsValidToDamage(m_cacheHitboxList[i]))
                {
                    m_toDamage.Add(m_cacheHitboxList[i].damageable);
                }
            }
            DamageTargets();
            OnDetonate?.Invoke(this, new AOETargetsEventArgs(m_toDamage));
            m_rigidbody.CastExplosiveForce(m_explosivePower, m_explosiveRadius);
            CallPoolRequest();
        }

        protected virtual bool IsValidToDamage(Hitbox hitbox)
        {
            return m_toDamage.Contains(hitbox.damageable) == false;
        }

        private void DamageTargets()
        {
            for (int i = 0; i < m_cacheHitboxList.Count; i++)
            {
                m_cacheHitbox = m_cacheHitboxList[i];

                for (int j = 0; j < m_damage.Length; j++)
                {
                    AttackInfo info = new AttackInfo(transform.position, 0, 1, m_damage[j]);
                    var result = GameplaySystem.combatManager.ResolveConflict(info,new TargetInfo(m_cacheHitbox.damageable, m_cacheHitbox.defense.damageReduction));
                    CallAttackerAttacked(new CombatConclusionEventArgs(info, m_toDamage[j], result)); 
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_toDamage = new List<ITarget>();
        }

        private void OnValidate()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}