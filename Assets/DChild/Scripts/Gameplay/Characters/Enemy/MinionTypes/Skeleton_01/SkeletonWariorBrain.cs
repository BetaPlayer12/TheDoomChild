using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonWariorBrain : MinionAIBrain<SkeletonWarrior>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private RaySensor m_groundSensor;

        [SerializeField]
        private RaySensor m_wallSensor;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_hasDetected;

        private void Patrol()
        {
            var patrolInfo = m_patrol.GetInfo(m_minion.position);
            var destination = patrolInfo.destination;
            if (IsLookingAt(destination))
            {
                m_minion.Patrol(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void MoveTo(Vector2 targetPos)
        {
            m_wallSensor.Cast();
            m_groundSensor.Cast();

            m_wallSensor.enabled = true;
            if (m_groundSensor.isDetecting && m_wallSensor.isDetecting == false)
            {
                m_minion.MoveTo(targetPos);
            }
            else
            {
                if (!m_groundSensor.isDetecting)
                {
                    m_minion.Stay();
                }
                else
                {
                    m_target = null;
                }
                m_wallSensor.enabled = false;
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        private void Attack()
        {
            var attackIndex = Mathf.Abs(Random.Range(1, 3));

            m_isAttacking = true;

            switch (attackIndex)
            {
                case 1:
                    m_minion.Slash();
                    break;
                case 2:
                    m_minion.Stab();
                    break;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
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
            m_minion.Stay();
            m_target = null;
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;

            if (m_target != null && IsLookingAt(m_target.position) && !m_hasDetected)
            {
                m_minion.PlayerDetect();
                m_hasDetected = true;
            }
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
                m_minion.Stay();
            }
            else
            {
                if (m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPosition = m_target.position;

                    if (IsLookingAt(targetPosition))
                    {
                        if (Vector2.Distance(m_minion.position, targetPosition) <= m_attackRange)
                        {
                            Attack();
                        }
                        else
                        {
                            MoveTo(targetPosition);
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
