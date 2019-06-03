using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies.Collections;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GrassMonsterBrain : MinionAIBrain<GrassMonster>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;
        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private Transform m_heightReference;

        [SerializeField]
        private bool m_isHiding;
        [SerializeField]
        private bool m_shouldMoveInGrass;

        private GrassDetector m_grassDetector;
        private GrassSensor m_grassSensor;

        private Vector2 m_grassPos;

        private bool m_isResting;
        private bool m_isAttacking;
        private bool m_isSearchingforGrass;
        private bool m_hasLocatedGrass;

        private void Attack(Vector2 targetPos)
        {
            if (targetPos.y > m_heightReference.position.y)
            {
                m_minion.DoAttack1();
            }
            else
            {
                m_minion.DoAttack2();
            }
            m_isAttacking = true;
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
            m_isAttacking = false;
            m_attackRest.EndTime(false);
            m_isResting = false;
            m_isHiding = true;
            m_isSearchingforGrass = false;
            m_hasLocatedGrass = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
            
            if (m_isHiding && m_target != null)
            {
                m_minion.PlayerDetected();
                m_isHiding = false;
            }
        }

        private float DistanceToTarget(Vector2 minionPos, Vector2 targetPos)
        {
            return Vector2.Distance(minionPos, targetPos);
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        private void MoveToTarget()
        {
            if (CanWalk())
            {
                m_minion.MoveTo(m_target.position);
            }
            else
            {
                m_minion.Stay();
                m_isSearchingforGrass = true;
            }
        }

        private bool CanWalk()
        {
            if (m_shouldMoveInGrass)
            {
                return m_grassSensor.Walkable();
            }

            return true;
        }

        private void Turn(Vector2 target)
        {
            if (target.x < m_minion.transform.position.x)
            {
                m_minion.TurnLeft();
            }
            else
            {
                m_minion.TurnRight();
            }
        }

        private void MoveToGrass()
        {
            var distanceToTarget = DistanceToTarget(transform.position, m_grassPos);
            if (IsLookingAt(m_grassPos))
            {
                if (distanceToTarget <= 1f)
                {
                    m_hasLocatedGrass = false;
                    Turn(GameplaySystem.playerManager.player.position);
                }
                else
                {
                    m_minion.MoveTo(m_grassPos);
                }
            }
            else
            {
                m_minion.StopAllCoroutines();
                Turn(m_grassPos);
            }
        }

        private void LocateGrass()
        {
            if (m_grassDetector.isGrassDetected())
            {
                m_grassPos = m_grassDetector.grassLocation;
                m_grassSensor.enabled = false;
                m_hasLocatedGrass = true;
                m_isSearchingforGrass = false;
            }
            else
            {
                m_isSearchingforGrass = false;
                m_grassSensor.enabled = false;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_grassDetector = GetComponentInChildren<GrassDetector>();
            m_grassSensor = GetComponentInChildren<GrassSensor>();
            m_grassSensor.enabled = false;
            m_attackRest.CountdownEnd += OnAttackRestEnd;
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
            else if (m_isSearchingforGrass)
            {
                m_target = null;   
                m_isAttacking = false;
                m_isResting = false;
                m_grassSensor.enabled = true;
                LocateGrass();
                m_minion.Stay();
            }
            else if (m_hasLocatedGrass)
            {
                MoveToGrass();
            }
            else
            {
                if (m_target == null)
                {
                    m_minion.Stay();
                    m_isHiding = true;
                }
                else
                {
                    //var distanceToTarget = DistanceToTarget(m_heightReference.position, targetPos);
                    var targetPos = m_target.position;
                    if (IsLookingAt(targetPos))
                    {
                        if (IsTargetInRange(m_attackRange))
                        {
                            Attack(targetPos);
                        }
                        else
                        {
                            MoveToTarget();
                        }
                    }
                    else
                    {
                        Turn(targetPos);
                    }

                }
            }
        }

#if UNITY_EDITOR
        public float attackRange => m_attackRange;
#endif
    }
}
