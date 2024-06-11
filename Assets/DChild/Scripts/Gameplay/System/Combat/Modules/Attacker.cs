using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [SelectionBase]
    [AddComponentMenu("DChild/Gameplay/Combat/Attacker")]
    public class Attacker : MonoBehaviour, IAttacker, IDamageDealer
    {
        [SerializeField]
        private Transform m_centerMass;
        [SerializeField, OnValueChanged("ApplyData")]
        private AttackData m_data;

#if UNITY_EDITOR
        [SerializeField, OnValueChanged("ApplyDamageModification", true)]
#endif
        private AttackDamageInfo m_currentAttackInfo;
        private Damage m_baseDamage;


        [ShowInInspector, HideInEditorMode, MinValue(0), OnValueChanged("ApplyDamageModification")]
        private float m_damageModifier;

        private bool m_isInstantiated;

        public Vector2 position => m_centerMass.position;

        public Invulnerability ignoreInvulnerability => m_currentAttackInfo.ignoreInvulnerability;

        public bool ignoresBlock => m_currentAttackInfo.ignoresBlock;

        public IAttacker parentAttacker { get; private set; }

        public IAttacker rootParentAttacker { get; private set; }

        public float modifier => m_damageModifier;

        public event EventAction<CombatConclusionEventArgs> TargetDamaged;
        public event EventAction<BreakableObjectEventArgs> BreakableObjectDamage;

        public void Damage(TargetInfo targetInfo, Collider2D colliderThatDealtDamage)
        {
            if (m_currentAttackInfo.ignoreInvulnerability >= targetInfo.bodyDefense.invulnerabilityLevel)
            {
                if (targetInfo.isBreakableObject)
                {
                    using (Cache<BreakableObjectEventArgs> cacheEventArgs = Cache<BreakableObjectEventArgs>.Claim())
                    {
                        cacheEventArgs.Value.Initialize(targetInfo.breakableObject);
                        BreakableObjectDamage?.Invoke(this, cacheEventArgs.Value);
                        cacheEventArgs.Release();
                    }
                }
                var position = transform.position;
                using (Cache<AttackerCombatInfo> cacheInfo = Cache<AttackerCombatInfo>.Claim())
                {
                    var owner = gameObject;

                    if (parentAttacker != null)
                    {
                        owner = parentAttacker.gameObject;
                    }

                    if (rootParentAttacker != null)
                    {
                        owner = rootParentAttacker.gameObject;
                    }

                    cacheInfo.Value.Initialize(owner, position, m_currentAttackInfo, colliderThatDealtDamage, m_data?.damageFX ?? null);
                    AttackSummaryInfo cacheResult = GameplaySystem.combatManager.ResolveConflict(cacheInfo.Value, targetInfo);
                    using (Cache<CombatConclusionEventArgs> cacheEventArgs = Cache<CombatConclusionEventArgs>.Claim())
                    {
                        cacheEventArgs.Value.Initialize(cacheInfo, targetInfo, cacheResult);
                        TargetDamaged?.Invoke(this, cacheEventArgs.Value);
                        cacheEventArgs.Release();
                    }
                    cacheInfo.Release();
                }
            }
        }

        public void SetDamage(Damage damage)
        {
            m_baseDamage = damage;
            ApplyDamageModification(damage);
        }

        public void SetCrit(CriticalDamageInfo criticalDamageInfo)
        {
            m_currentAttackInfo.criticalDamageInfo = criticalDamageInfo;
        }

        public void SetData(AttackData data)
        {
            m_data = data;
            m_currentAttackInfo = data.info;
            m_baseDamage = data.info.damage;
            ApplyDamageModification(m_baseDamage);
        }

        public void SetDamageModifier(float value)
        {
            if (m_damageModifier != value)
            {
                m_damageModifier = Mathf.Max(0, value);
                ApplyDamageModification(m_baseDamage);
            }
        }

        public void SetIgnoresBlock(bool ignoresBlock)
        {
            m_currentAttackInfo.ignoresBlock = ignoresBlock;
        }

        public void SetParentAttacker(IAttacker damageDealer)
        {
            parentAttacker = damageDealer;
            //Debug.Log($"parentAttacker: {parentAttacker}");
        }

        public void SetRootParentAttacker(IAttacker damageDealer)
        {
            rootParentAttacker = damageDealer;
            //Debug.Log($"rootParentAttacker: {rootParentAttacker}");
        }

        private void ApplyDamageModification(Damage baseDamage)
        {
            var damage = m_currentAttackInfo.damage;
            damage.type = baseDamage.type;
            damage.value = Mathf.CeilToInt(baseDamage.value * m_damageModifier);
            m_currentAttackInfo.damage = damage;
        }

        private void Awake()
        {
            m_damageModifier = 1;
            if (m_data != null)
            {
                m_currentAttackInfo = m_data.info;
            }
        }

#if UNITY_EDITOR
        [Button]
        private void UseSelfAsCenterMass()
        {
            m_centerMass = transform;
        }

        public void InitializeField(Transform centerMass)
        {
            m_centerMass = centerMass;
        }

        public void PassParentAttacker(IAttacker damageDealer)
        {
            if (damageDealer.rootParentAttacker == null)
            {
                SetParentAttacker(damageDealer);
            }

            else
            {

                SetParentAttacker(damageDealer);
                SetRootParentAttacker(damageDealer.rootParentAttacker);
            }
        }

        private void ApplyDamageModification()
        {
            ApplyDamageModification(m_data.info.damage);
        }

        private void ApplyData()
        {
            SetData(m_data);
        }
#endif
    }
}