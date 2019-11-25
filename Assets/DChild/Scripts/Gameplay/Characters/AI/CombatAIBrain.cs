using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
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

        protected AITargetInfo m_targetInfo;

        public virtual void SetTarget(IDamageable damageable, Character m_target = null)
        {
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
    }
}