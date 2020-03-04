using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GoblinBrain : MinionAIBrain<Goblin>, IAITargetingBrain
    {
        [SerializeField]
        [TabGroup("Attack")]
        [MinValue(0f)]
        private float m_attackDistance;

        [SerializeField]
        [TabGroup("Attack")]
        [MinValue(0f)]
        private float m_attackCriticalDistance;

        [SerializeField]
        private float m_retreatDistance;

        private WayPointPatroler m_fullRetreatDistance;

        private void Retreat()
        {
            var destination = m_fullRetreatDistance.GetInfo(transform.position).destination;
            m_minion.Retreat(destination);
            m_minion.LookAt(destination);
        }

        private void MoveTo(Vector2 currentPos)
        {
            var destination = new Vector2(currentPos.x + m_retreatDistance, currentPos.y);
            m_minion.Retreat(destination);
            m_minion.LookAt(destination);
        }

        private bool isTargetWithinAttackRange(float distance)
        {
            var attackDirection = (m_minion.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right);
            var hit = Physics2D.Raycast(transform.position, attackDirection, distance, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawRay(transform.position, attackDirection * distance);
            return hit.collider != null;
        }

        private bool isNearDestination()
        {
            return true;
        }

        private bool CanBeSeenBy(CombatCharacter character)
        {
            if (m_minion.position.x < character.position.x)
            {
                return character.currentFacingDirection == HorizontalDirection.Left;
            }
            else
            {
                return character.currentFacingDirection == HorizontalDirection.Right;
            }
        }

        protected override bool IsLookingAt(Vector2 target)
        {
            return base.IsLookingAt(target);
        }

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Stay();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
            if (m_minion.position.x < target.position.x)
            {
                m_minion.SetFacing(HorizontalDirection.Right);
            }
            else
            {
                m_minion.SetFacing(HorizontalDirection.Left);
            }
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target != null)
            {
               
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_fullRetreatDistance = GetComponent<WayPointPatroler>(); 
        }

#if UNITY_EDITOR
        public float retreatDistance => m_retreatDistance;
        public float attackDistance => m_attackDistance;
        public float attackCriticalDistance => m_attackCriticalDistance;
#endif
    }
}
