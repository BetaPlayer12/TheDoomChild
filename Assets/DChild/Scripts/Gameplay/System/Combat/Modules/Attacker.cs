﻿using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class Attacker : MonoBehaviour, IAttacker, IDamageDealer
    {
        [SerializeField]
        private Transform m_centerMass;
        [SerializeField, OnValueChanged("ApplyData")]
        private AttackerData m_data;

#if UNITY_EDITOR
        [SerializeField, OnValueChanged("ApplyDamageModification")]
#endif
        private AttackerInfo m_info;

        [ShowInInspector, HideInEditorMode, MinValue(0), OnValueChanged("ApplyDamageModification")]
        private int m_damageModifier;

        private bool m_isInstantiated;

        private List<AttackDamage> m_currentDamage;
        public event EventAction<CombatConclusionEventArgs> TargetDamaged;
        public event EventAction<BreakableObjectEventArgs> BreakableObjectDamage;

        public void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            if (m_info.ignoreInvulnerability || !targetDefense.isInvulnerable)
            {
                if (targetInfo.isBreakableObject)
                {
                    BreakableObjectDamage?.Invoke(this, new BreakableObjectEventArgs(targetInfo.breakableObject));
                }
                var position = transform.position;
                AttackerCombatInfo info = new AttackerCombatInfo(position, 0, 1, m_currentDamage.ToArray());
                var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, targetInfo, result));
            }
        }

        public void SetDamage(params AttackDamage[] damage)
        {
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
            m_info.Copy(m_data.info);
            if (m_isInstantiated == false)
            {
                m_damageModifier = 1;
                m_currentDamage = new List<AttackDamage>();
                m_isInstantiated = true;
            }
            ApplyDamageModification();
        }

        public void SetDamageModifier(int value)
        {
            if (m_damageModifier != value)
            {
                m_damageModifier = Mathf.Max(0, value);
                ApplyDamageModification();
            }
        }

        private void ApplyDamageModification()
        {
            m_currentDamage.Clear();
            for (int i = 0; i < m_info.damage.Count; i++)
            {
                var damage = m_info.damage[i];
                damage.value *= m_damageModifier;
                m_currentDamage.Add(damage);
            }
        }

        private void Awake()
        {
            m_info = new AttackerInfo();
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
            ApplyDamageModification();
        }
#endif
    }
}