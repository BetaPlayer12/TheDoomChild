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
        private AttackerData m_data;

#if UNITY_EDITOR
        [SerializeField, OnValueChanged("ApplyDamageModification", true)]
#endif
        private AttackerInfo m_info;

        [ShowInInspector, HideInEditorMode, MinValue(0), OnValueChanged("ApplyDamageModification")]
        private float m_damageModifier;

        private bool m_isInstantiated;

        private List<AttackDamage> m_currentDamage;

        public Vector2 position => m_centerMass.position;

        public Invulnerability ignoreInvulnerability => m_info.ignoreInvulnerability;

        public bool ignoresBlock => m_info.ignoresBlock;

        public IAttacker parentAttacker { get; private set; }

        public IAttacker rootParentAttacker { get; private set; }

        public event EventAction<CombatConclusionEventArgs> TargetDamaged;
        public event EventAction<BreakableObjectEventArgs> BreakableObjectDamage;

        public void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            if (m_info.ignoreInvulnerability >= targetDefense.invulnerabilityLevel)
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
                    cacheInfo.Value.Initialize(gameObject,position, 0, 1, m_info.ignoresBlock, m_currentDamage.ToArray());
                    using (Cache<AttackInfo> cacheResult = GameplaySystem.combatManager.ResolveConflict(cacheInfo.Value, targetInfo))
                    {
                        using (Cache<CombatConclusionEventArgs> cacheEventArgs = Cache<CombatConclusionEventArgs>.Claim())
                        {
                            cacheEventArgs.Value.Initialize(cacheInfo, targetInfo, cacheResult);
                            TargetDamaged?.Invoke(this, cacheEventArgs.Value);
                            cacheEventArgs.Release();
                        }
                        cacheResult.Release();
                    }
                    cacheInfo.Release();
                }
            }
        }

        public void SetDamage(params AttackDamage[] damage)
        {
            if (m_info == null)
            {
                m_info = new AttackerInfo();
            }
            m_info.damage.Clear();
            m_info.damage.AddRange(damage);
            if (m_isInstantiated == false)
            {
                m_damageModifier = 1;
                m_currentDamage = new List<AttackDamage>();
                m_isInstantiated = true;
            }
            ApplyDamageModification();
        }

        public void SetCrit(int critChance, int critModifier)
        {
            m_info.critChance = critChance;
            m_info.critDamageModifier = critModifier;
        }

        public void SetData(AttackerData data)
        {
            m_data = data;
            if (m_info != null)
            {
                m_info.Copy(m_data.info);
                if (m_isInstantiated == false)
                {
                    m_damageModifier = 1;
                    m_currentDamage = new List<AttackDamage>();
                    m_isInstantiated = true;
                }
                ApplyDamageModification();
            }
        }

        public void SetDamageModifier(float value)
        {
            if (m_damageModifier != value)
            {
                m_damageModifier = Mathf.Max(0, value);
                ApplyDamageModification();
            }
        }

        public void SetIgnoresBlock(bool ignoresBlock)
        {
            m_info.ignoresBlock = ignoresBlock;
        }

        public void SetParentAttacker(IAttacker damageDealer)
        {
            parentAttacker = damageDealer;
        }

        public void SetRootParentAttacker(IAttacker damageDealer)
        {
            rootParentAttacker = damageDealer;
        }

        private void ApplyDamageModification()
        {
            m_currentDamage.Clear();
            for (int i = 0; i < m_info.damage.Count; i++)
            {
                var damage = m_info.damage[i];
                damage.value = Mathf.CeilToInt(damage.value * m_damageModifier);
                m_currentDamage.Add(damage);
            }
        }

        private void Awake()
        {
            if (m_info == null)
            {
                m_info = new AttackerInfo();
            }
            if (m_data != null)
            {
                m_info.Copy(m_data.info);
            }

            if (m_isInstantiated == false)
            {
                m_damageModifier = 1;
                m_currentDamage = new List<AttackDamage>();
                ApplyDamageModification();
                m_isInstantiated = true;
            }
        }

#if UNITY_EDITOR
        [Button]
        private void UseSelfAsCenterMass()
        {
            m_centerMass = transform;
        }

        private void ApplyData()
        {
            m_info.Copy(m_data.info);
            if (m_currentDamage != null)
            {
                ApplyDamageModification();
            }
        }

        public void InitializeField(Transform centerMass)
        {
            m_centerMass = centerMass;
        }


#endif
    }
}