using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PoisonToadBrain : MinionAIBrain<PoisonToad>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_attackDistance;

        private bool CanHitTarget()
        {
            var attackDirection = (m_minion.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right);
            var hit = Physics2D.Raycast(transform.position, attackDirection, m_attackDistance, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawRay(transform.position, attackDirection * m_attackDistance);
            return hit.collider != null;
        }

        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target != null)
            { 
                if(CanHitTarget())
                {
                    m_minion.Attack(m_target);
                }
                else
                {
                    m_minion.DoIdle();
                }
            }
        }

#if UNITY_EDITOR
        public float attackDistance => m_attackDistance;
#endif
    }
}
