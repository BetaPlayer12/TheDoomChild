using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Refactor.DChild.Gameplay.Characters.AI
{
    public abstract class CombatAIBrain<T> : AIBrain<T>, ICombatAIBrain where T : IAIInfo
    {
        protected AITargetInfo m_targetInfo;

        public virtual void SetTarget(IDamageable damageable, Character m_target = null)
        {
            m_targetInfo.Set(damageable, m_target);
        }

        protected bool IsFacingTarget()
        {
            if (m_targetInfo.position.x > m_character.transform.position.x)
            {
                return m_character.facing == HorizontalDirection.Right;
            }
            else
            {
                return m_character.facing == HorizontalDirection.Left;
            }
        }

        protected bool IsTargetInRange(float distance) => Vector2.Distance(m_targetInfo.position, m_character.transform.position) <= distance;
        protected Vector2 DirectionToTarget() => (m_targetInfo.position - (Vector2)m_character.transform.position).normalized;


        protected virtual void Start()
        {
            m_targetInfo = new AITargetInfo();
        }
    }
}