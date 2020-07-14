using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public abstract class CombatAIBrain<T> : AIBrain<T>, ICombatAIBrain where T : IAIInfo
    {
        [SerializeField, TabGroup("Reference")]
        private Damageable m_damageable;
        [SerializeField, TabGroup("Reference")]
        protected Transform m_centerMass;
        [SerializeField, ValueDropdown("GetData"), OnValueChanged("InitializeInfo"), TabGroup("Data")]
        protected CharacterStatsData m_statsData;


        protected AITargetInfo m_targetInfo;

        public virtual void SetTarget(IDamageable damageable, Character m_target = null)
        {
            m_targetInfo.Set(damageable, m_target);
            Debug.Log("Murmur has TARGET");
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

        protected bool IsTargetInRange(float distance) => Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= distance;
        protected Vector2 DirectionToTarget() => (m_targetInfo.position - (Vector2)m_character.transform.position).normalized;

        protected override void Awake()
        {
            m_targetInfo = new AITargetInfo();
            base.Awake();
            m_damageable.Destroyed += OnDestroyed;
        }

        protected virtual void OnDestroyed(object sender, EventActionArgs eventArgs) => enabled = false;

        protected virtual void Start()
        {

        }

        protected virtual void LateUpdate()
        {
            if(m_targetInfo.isValid && m_targetInfo.doesTargetExist == false)
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