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
    public class CombatAIBrainManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private List<ICombatAIBrain> m_combatAI;
        private bool m_allArePassive;

        public void Add(ICombatAIBrain instance)
        {
            if (m_combatAI.Contains(instance) == false)
            {
                m_combatAI.Add(instance);
                instance.ForbidFromAttackTarget(m_allArePassive);
            }
        }

        public void Remove(ICombatAIBrain instance)
        {
            if (m_combatAI.Contains(instance))
            {
                m_combatAI.Remove(instance);
                instance.ForbidFromAttackTarget(false);
            }
        }

        public void MakeAllPassive(bool value)
        {
            m_allArePassive = value;
            for (int i = 0; i < m_combatAI.Count; i++)
            {
                m_combatAI[i].ForbidFromAttackTarget(m_allArePassive);
            }
        }
    }

    public abstract class CombatAIBrain<T> : AIBrain<T>, ICombatAIBrain, IController where T : IAIInfo
    {
        [Flags]
        protected enum Restriction
        {
            ForbiddenFromAttackingTarget,
            IgnoreTarget
        }

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

        protected Restriction m_currentRestrictions;

        public virtual void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (m_targetInfo == null)
            {
                m_targetInfo = new AITargetInfo();
            }
            m_targetInfo.Set(damageable, m_target);
        }



        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }

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

        /// <summary>
        /// AI Essentially Resets as its Returns to Spawn Point
        /// </summary>
        public abstract void ReturnToSpawnPoint();

        /// <summary>
        /// Will still be Notice the Target but cannot attack it
        /// </summary>
        /// <param name="value"></param>
        public void ForbidFromAttackTarget(bool value)
        {
            if (value)
            {
                m_currentRestrictions |= Restriction.IgnoreTarget;
                OnForbidFromAttackTarget();
            }
            else
            {
                m_currentRestrictions |= Restriction.IgnoreTarget;
               
            }
        }

        /// <summary>
        /// Target is null but still can accept new Targets
        /// </summary>
        public void IgnoreCurrentTarget()
        {
            SetTarget(null);
        }

        /// <summary>
        /// Target is null and cannot accept new Targets if True
        /// </summary>
        /// <param name="value"></param>
        public void IgnoreAllTargets(bool value)
        {
            if (value)
            {
                m_aggroBoundary.gameObject.SetActive(false);
                SetTarget(null);
                m_currentRestrictions |= Restriction.IgnoreTarget;
            }
            else
            {
                m_aggroBoundary.gameObject.SetActive(true);
                m_currentRestrictions &= ~Restriction.IgnoreTarget;
            }
        }


        /// <summary>
        /// When its told that it cant attack target
        /// </summary>
        protected abstract void OnForbidFromAttackTarget();

        protected bool HasRestriction(Restriction restriction) => m_currentRestrictions.HasFlag(restriction);

        protected bool IsFacingTarget() => IsFacing(m_targetInfo.position);
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
        public CharacterStatsData statsData { get => m_statsData; set => m_statsData = value; }

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