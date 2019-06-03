using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    /// <summary>
    /// Decides What the Minion will do,
    /// Has Booleans for States
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Minion))]
    public abstract class MinionAIBrain<T> : MonoBehaviour, IAIBrain where T : Minion
    {
        protected T m_minion;
        protected IEnemyTarget m_target;

        public abstract void Enable(bool value);
        public abstract void ResetBrain();

        protected bool IsTargetInRange(float range) => Vector2.Distance(transform.position, m_target.position) <= range;
        protected Vector2 GetDirectionToTarget() => (m_target.position - (Vector2)transform.position).normalized;

        protected virtual bool IsLookingAt(Vector2 target)
        {
            if (m_minion.position.x < target.x)
            {
                return m_minion.currentFacingDirection == HorizontalDirection.Right;
            }
            else
            {
                return m_minion.currentFacingDirection == HorizontalDirection.Left;
            }
        }

        protected virtual void Awake()
        {
            m_minion = GetComponent<T>();
        }
    }
}