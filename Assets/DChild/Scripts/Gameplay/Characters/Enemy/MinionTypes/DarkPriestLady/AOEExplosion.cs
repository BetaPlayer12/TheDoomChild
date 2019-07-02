using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Projectiles;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Holysoft.Event;
using System;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "AOEExplosionData", menuName = "DChild/Gameplay/AOE Explosion Data")]
    public class AOEExplosionData : ScriptableObject
    {
        [SerializeField]
        private AttackDamage[] m_damage;
        [SerializeField, MinValue(1f)]
        private float m_damageRadius;
        [SerializeField]
        private ExplosionData m_explosionData;

        public AttackDamage[] damage => m_damage;
        public float damageRadius => m_damageRadius;
        public float explosiveRadius => m_explosionData.explosiveRadius;
        public float explosivePower => m_explosionData.explosiveRadius;
    }

    public class AOEExplosion : PoolableObject
    {
        [SerializeField]
        private AOEExplosionData m_data;
        [SerializeField]
        private FX m_fx;

        private Rigidbody2D m_rigidbody;
        public event EventAction<AOETargetsEventArgs> OnDetonate;

        private List<ITarget> m_toDamage;
        private List<Hitbox> m_cacheHitboxList;
        private Hitbox m_cacheHitbox;

        public void SetData(AOEExplosionData data) => m_data = data;

        public virtual void Detonate()
        {
            m_cacheHitboxList.Clear();
            m_cacheHitboxList = GameplaySystem.combatManager.GetValidTargetsOfCircleAOE(m_rigidbody.position, m_data.damageRadius, Physics2D.GetLayerCollisionMask(gameObject.layer));
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
            m_rigidbody.CastExplosiveForce(m_data.explosivePower, m_data.explosiveRadius);
        }

        private void DamageTargets()
        {
            for (int i = 0; i < m_cacheHitboxList.Count; i++)
            {
                m_cacheHitbox = m_cacheHitboxList[i];
                var damage = m_data.damage;
                for (int j = 0; j < damage.Length; j++)
                {
                    AttackInfo info = new AttackInfo(transform.position, 0, 1, damage[j]);
                    var result = GameplaySystem.combatManager.ResolveConflict(info, new TargetInfo(m_cacheHitbox.damageable, m_cacheHitbox.defense.damageReduction));
                    //CallAttackerAttacked(new CombatConclusionEventArgs(info, m_toDamage[j], result));
                }
            }
        }

        protected virtual bool IsValidToDamage(Hitbox hitbox)
        {
            return m_toDamage.Contains(hitbox.damageable) == false;
        }

        private void OnFXDone(object sender, EventActionArgs eventArgs)
        {
            CallPoolRequest();
        }

        private void Awake()
        {
            m_cacheHitboxList = new List<Hitbox>();
            m_toDamage = new List<ITarget>();
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_fx.Done += OnFXDone;
        }
    }
}
