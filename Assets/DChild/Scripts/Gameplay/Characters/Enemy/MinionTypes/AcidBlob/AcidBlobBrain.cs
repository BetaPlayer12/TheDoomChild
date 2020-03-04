using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class AcidBlobBrain : MinionAIBrain<AcidBlob>, IAITargetingBrain
    {
        private WayPointPatroler m_patrol;

        private bool m_isAttacking;

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;
            m_minion.Patrol(destination);
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
            m_isAttacking = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (target != null)
            {
                m_target = target;
            }
            else
            {
                m_target = null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_patrol.Initialize();
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isAttacking = false;
            }
            else
            {
                if (m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;

                    m_minion.Attack();
                    m_isAttacking = true;
                }
            }
        }
    }
}
