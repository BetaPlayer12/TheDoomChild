using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public class CombatAIBrain : SerializedMonoBehaviour
    {
        [SerializeField]
        private List<ICombatAIBrain> m_combatAI;
        private bool m_allArePassive;

        public void Add(ICombatAIBrain instance)
        {
            if (m_combatAI.Contains(instance) == false)
            {
                m_combatAI.Add(instance);
                instance.BecomePassive(m_allArePassive);
            }
        }

        public void Remove(ICombatAIBrain instance)
        {
            if (m_combatAI.Contains(instance))
            {
                m_combatAI.Remove(instance);
                instance.BecomePassive(false);
            }
        }

        public void MakeAllPassive(bool value)
        {
            m_allArePassive = value;
            for (int i = 0; i < m_combatAI.Count; i++)
            {
                m_combatAI[i].BecomePassive(m_allArePassive);
            }
        }
    }

    public abstract class CombatAIBrain<T> : AIBrain<T>, ICombatAIBrain where T : IAIInfo
    {
        [SerializeField, TabGroup("Reference")]
        protected Damageable m_damageable;
        [SerializeField, TabGroup("Reference")]
        protected Transform m_centerMass;
        [SerializeField]
        private AggroBoundary m_aggroBoundary;
        [SerializeField, TabGroup("Data")]
        protected CharacterStatsData m_statsData;

        protected AITargetInfo m_targetInfo;

        private Attacker m_attacker;
        private AttackResistance m_attackResistance;
        private StatusInflictor m_statusInflictor;
        private StatusEffectResistance m_statusResistance;

        public virtual void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (m_targetInfo == null)
            {
                m_targetInfo = new AITargetInfo();
            }
            m_targetInfo.Set(damageable, m_target);
        }

        protected bool IsFacingTarget() => IsFacing(m_targetInfo.position);

        public bool IsFacing(Vector2 position)
        {
            if (position.x > m_character.transform.position.x)
            {
                return m_character.facing == HorizontalDirection.Right;
            }
            else
            {
                return m_character.facing == HorizontalDirection.Left;
            }
        }

        public void BecomePassive(bool value)
        {
            if (value)
            {
                m_aggroBoundary.gameObject.SetActive(false);
                OnBecomePassive();
            }
            else
            {
                m_aggroBoundary.gameObject.SetActive(true);
            }
        }

        protected abstract void OnBecomePassive();

        protected bool IsTargetInRange(float distance) => Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= distance;
        protected Vector2 DirectionToTarget() => (m_targetInfo.position - (Vector2)m_character.transform.position).normalized;

        protected void LoadCharacterStatData(CharacterStatsData statData)
        {
            m_damageable.health.SetMaxValue(statData.maxHealth);
            m_attacker?.SetData(statData.damage);
            m_attackResistance?.SetData(statData.attackResistance);
            m_attackResistance?.SetData(statData.attackResistance);
            m_statusInflictor?.SetData(statData.statusInfliction);
            m_statusResistance?.SetData(statData.statusResistanceData);
        }

        protected override void Awake()
        {
            if (m_targetInfo == null)
            {
                m_targetInfo = new AITargetInfo();
            }
            base.Awake();
            m_damageable.Destroyed += OnDestroyed;

            m_attacker = GetComponent<Attacker>();
            m_attackResistance = GetComponentInChildren<AttackResistance>();
            m_statusInflictor = GetComponent<StatusInflictor>();
            m_statusResistance = GetComponentInChildren<StatusEffectResistance>();
        }

        protected virtual void OnDestroyed(object sender, EventActionArgs eventArgs) => base.enabled = false;

        protected virtual void Start()
        {
            if (m_statsData != null)
            {
                LoadCharacterStatData(m_statsData);
                m_damageable.health.ResetValueToMax();
            }
        }

        protected virtual void LateUpdate()
        {
            if (m_targetInfo.isValid && m_targetInfo.doesTargetExist == false)
            {
                OnTargetDisappeared();
                m_targetInfo.Set(null);
            }
        }

        protected abstract void OnTargetDisappeared();

#if UNITY_EDITOR
        public Type aiDataType => m_data.GetType();



        public void InitializeField(Character character, SpineRootAnimation spineRoot, Damageable damageable, Transform centerMass)
        {
            m_character = character;
            m_animation = spineRoot;
            m_damageable = damageable;
            m_centerMass = centerMass;
        }
#endif
    }
}