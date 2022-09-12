using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.Utilities;

namespace DChild.Gameplay
{
    public class ExplosionEffects : MonoBehaviour
    {
        [SerializeField]
        private ExplosionData m_data;
        [SerializeField]
        private FX m_fx;

        private Rigidbody2D m_rigidbody;
        public event EventAction<AOETargetsEventArgs> OnDetonate;

        private List<IDamageable> m_toDamage;
        private List<Hitbox> m_cacheHitboxList;
        private Hitbox m_cacheHitbox;

        public void SetData(ExplosionData data) => m_data = data;

        //public virtual void Detonate()
        //{
        //    m_cacheHitboxList.Clear();
        //    //m_cacheHitboxList = GameplaySystem.combatManager.GetValidTargetsOfCircleAOE(m_rigidbody.position, m_data.damageRadius, Physics2D.GetLayerCollisionMask(gameObject.layer));
        //    m_cacheHitboxList = GameplaySystem.combatManager.GetValidTargetsOfCircleAOE(m_rigidbody.position, 5, Physics2D.GetLayerCollisionMask(gameObject.layer), m_data.ignoreInvulerability);
        //    m_toDamage.Clear();
        //    for (int i = 0; i < m_cacheHitboxList.Count; i++)
        //    {
        //        if (IsValidToDamage(m_cacheHitboxList[i]))
        //        {
        //            m_toDamage.Add(m_cacheHitboxList[i].damageable);
        //        }
        //    }
        //    DamageTargets();
        //    using (Cache<AOETargetsEventArgs> cacheEventArgs = Cache<AOETargetsEventArgs>.Claim())
        //    {
        //        cacheEventArgs.Value.Initialize(m_toDamage);
        //        OnDetonate?.Invoke(this, cacheEventArgs.Value);
        //        cacheEventArgs.Release();
        //    }
        //    //m_rigidbody.CastExplosiveForce(m_data.explosivePower, m_data.explosiveRadius);
        //}

        //private void DamageTargets()
        //{
        //    if (m_cacheHitboxList.Count > 0)
        //    {
        //        using (Cache<AttackerCombatInfo> info = Cache<AttackerCombatInfo>.Claim())
        //        {
        //            using (Cache<TargetInfo> targetInfo = Cache<TargetInfo>.Claim())
        //            {
        //                for (int i = 0; i < m_cacheHitboxList.Count; i++)
        //                {
        //                    m_cacheHitbox = m_cacheHitboxList[i];
        //                    var damage = m_data.explosivePower;
        //                    for (int j = 0; j < damage.Length; j++)
        //                    {
        //                        info.Value.Initialize(transform.position, 0, 1, damage[j]);
        //                        targetInfo.Value.Initialize(m_cacheHitbox.damageable, m_cacheHitbox.defense.damageReduction);
        //                        using (Cache<AttackInfo> cacheAttackInfo = GameplaySystem.combatManager.ResolveConflict(info, targetInfo.Value))
        //                        {
        //                            cacheAttackInfo.Release();
        //                        }
        //                    }
        //                }
        //                targetInfo.Release();
        //            }
        //            info.Release();
        //        }
        //    }
        //}

        //protected virtual bool IsValidToDamage(Hitbox hitbox)
        //{
        //    return m_toDamage.Contains(hitbox.damageable) == false;
        //}

        //private void OnFXDone(object sender, EventActionArgs eventArgs)
        //{
        //    //CallPoolRequest();
        //}

        private void Awake()
        {
            m_cacheHitboxList = new List<Hitbox>();
            m_toDamage = new List<IDamageable>();
            m_rigidbody = GetComponent<Rigidbody2D>();

            //if (m_fx != null)
            //    m_fx.Done += OnFXDone;
        }
    }
}
