using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    /// <summary>
    /// Decides What the Minion will do,
    /// Has Booleans for States
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BossTemplate))]
    public abstract class BossAIBrain<T> : MonoBehaviour, IAIBrain where T : BossTemplate
    {
        protected T m_boss;
        protected IEnemyTarget m_target;

        public abstract void Enable(bool value);
        public abstract void ResetBrain();

        protected bool IsTargetInRange(float range) => Vector2.Distance(transform.position, m_target.position) <= range;
        protected Vector2 GetDirectionToTarget() => (m_target.position - (Vector2)transform.position).normalized;

        protected virtual bool IsLookingAt(Vector2 target)
        {
            if (m_boss.position.x < target.x)
            {
                return m_boss.currentFacingDirection == HorizontalDirection.Right;
            }
            else
            {
                return m_boss.currentFacingDirection == HorizontalDirection.Left;
            }
        }

        protected virtual void Awake()
        {
            m_boss = GetComponent<T>();
        }
    }
}