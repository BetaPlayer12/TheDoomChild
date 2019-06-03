using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BellBrain : MinionAIBrain<Bell>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(1)]
        private float m_spitAttackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        private bool m_isResting;
        private bool m_isAttacking;

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Idle();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Idle();
            m_target = null;
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;
      

        protected override void Awake()
        {
            base.Awake();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
        }

        private void SpawnVine(Vector2 targetPos)
        {
            var spikePosition = targetPos;
            int hitCount;
            Raycaster.SetLayerCollisionMask(LayerMask.NameToLayer("EnvironmentOnly"));
            var hits = Raycaster.Cast(targetPos, Vector2.down, true, out hitCount);
            m_minion.VineSpawnAttack(hits[0].point);
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isAttacking = false;
                m_attackRest.Reset();
                m_isResting = true;
            }
            else if (m_isResting)
            {
                m_attackRest.Tick(m_minion.time.deltaTime);
                m_minion.Idle();
            }
            else
            {
                if(m_target == null)
                {
                    m_minion.Idle();
                }
                else
                {
                    var targetPos = m_target.position;
                    if (IsLookingAt(targetPos))
                    {
                        var distanceToTarget = Vector2.Distance(m_minion.position, targetPos);
                        if (distanceToTarget <= m_spitAttackRange)
                        {
                            m_minion.AcidSpitAttack(targetPos);
                            m_isAttacking = true;
                        }
                        else
                        {
                            SpawnVine(targetPos);
                            m_isAttacking = true;
                        }
                    }
                    else
                    {
                        m_minion.Turn();
                    }              
                }
            }
        }
    }
}
