﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Projectiles;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;

namespace DChild.Gameplay
{

    public class AOEExplosion : PoolableObject
    {
        [SerializeField]
        private AOEExplosionData m_data;
        [SerializeField]
        private FX m_fx;

        private Rigidbody2D m_rigidbody;
        public event EventAction<AOETargetsEventArgs> OnDetonate;

        private List<IDamageable> m_toDamage;
        private List<Hitbox> m_cacheHitboxList;
        private Hitbox m_cacheHitbox;

        public void SetData(AOEExplosionData data) => m_data = data;

        public virtual void Detonate()
        {
            m_cacheHitboxList.Clear();
            //m_cacheHitboxList = GameplaySystem.combatManager.GetValidTargetsOfCircleAOE(m_rigidbody.position, m_data.damageRadius, Physics2D.GetLayerCollisionMask(gameObject.layer));
            m_cacheHitboxList = GameplaySystem.combatManager.GetValidTargetsOfCircleAOE(m_rigidbody.position, 5, Physics2D.GetLayerCollisionMask(gameObject.layer));
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
            //m_rigidbody.CastExplosiveForce(m_data.explosivePower, m_data.explosiveRadius);
        }

        private void DamageTargets()
        {
            for (int i = 0; i < m_cacheHitboxList.Count; i++)
            {
                m_cacheHitbox = m_cacheHitboxList[i];
                var damage = m_data.damage;
                for (int j = 0; j < damage.Length; j++)
                {
                    AttackerInfo info = new AttackerInfo(transform.position, 0, 1, damage[j]);
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
            m_toDamage = new List<IDamageable>();
            m_rigidbody = GetComponent<Rigidbody2D>();

            if(m_fx != null)
                m_fx.Done += OnFXDone;
        }
    }
}
