using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MarionetteBrain : MinionAIBrain<Marionette>, IAITargetingBrain
    {
        [SerializeField]
        private float m_patience;
        private float m_currentPatience;
        private int m_behaviorCount;
        [SerializeField]
        private RaySensor m_terrainSensor;
        [SerializeField]
        private float m_idleRange;
        [SerializeField]
        private float m_castRange;
        [SerializeField]
        private float m_targetFollowOffset;
        [SerializeField]
        private float m_reviveCounter;
        private float m_currenCounter;
        [SerializeField]
        private Collider2D m_aggroSensor;

        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_Attack1Range;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_Attack2Range;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_Attack1Chance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_Attack2Chance;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_lockBehavior;
        private bool m_hasThrown;
        private bool m_enablePatience;
        private bool m_detected;

        private WayPointPatroler m_patrol;

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (target != null)
            {
                m_target = target;
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            else
            {
                m_enablePatience = true;
            }
        }

        public float GetDistance()
        {
            return Vector2.Distance(m_target.position, transform.position);
        }

        private void Turn()
        {
            //Debug.Log("Idle is " + m_minion.IsIdle());
            if (m_currentBehavior == 0)
            {
                if (m_target.position.x < transform.position.x && transform.localScale.x == -1)
                {
                    m_facingPlayer = false;
                    m_minion.StopMoving();
                    if (m_minion.IsIdle())
                    {
                        m_minion.Turn();
                    }
                }
                else if (m_target.position.x > transform.position.x && transform.localScale.x == 1)
                {
                    m_facingPlayer = false;
                    m_minion.StopMoving();
                    if (m_minion.IsIdle())
                    {
                        m_minion.Turn();
                    }
                }
                else
                {
                    m_facingPlayer = true;
                }
            }
        }

        private void SetBehavior(int num)
        {
            if (m_currentBehavior == 0)
            {
                m_currentBehavior = num;
            }
        }

        private void Behaviors()
        {
            switch (m_behaviorCount)
            {
                case 1:
                    if (m_facingPlayer)
                    {
                        SetBehavior(1);
                    }
                    if (m_currentBehavior == 1)
                    {
                        if (GetDistance() < m_Attack1Range && m_behaviorChance < m_Attack1Chance && GetDistance() > m_castRange)
                        {
                            m_minion.Attack1();
                            m_currentBehavior = 0;
                        }
                        else
                        {
                            m_currentBehavior = 0;
                        }
                    }
                    break;
                //case 2:
                //    if (m_facingPlayer)
                //    {
                //        SetBehavior(2);
                //    }
                //    if (m_currentBehavior == 2)
                //    {
                //        if (GetDistance() < m_Attack2Range && m_behaviorChance < m_Attack2Chance && GetDistance() > m_castRange)
                //        {
                //            m_minion.Attack2();
                //            m_currentBehavior = 0;
                //        }
                //        else
                //        {
                //            m_currentBehavior = 0;
                //        }
                //    }
                //    break;
                default:
                    break;
            }
        }

        private void NormalMode()
        {
            if (!m_detected)
            {
                m_detected = true;
                m_minion.Assemble();
            }
            else
            {
                GetDistance();
                if (GetDistance() > 5)
                {
                    Turn();
                }
            }

            //if (m_terrainSensor.isDetecting)
            //{
            //    Debug.Log("Terrain Sensor Detecting");
            //    m_enablePatience = true;
            //    m_currentPatience = m_patience;
            //}

            //Debug.Log("Ratking moving: " + GetDistance());
            if (m_currentBehavior == 0 /*&& m_minion.IsIdle()*/ && m_facingPlayer && GetDistance() > m_idleRange && !m_terrainSensor.isDetecting)
            {
                Vector2 pos = new Vector2(m_target.position.x, m_target.position.y + m_targetFollowOffset);
                m_minion.MovetoDestination(pos, true);
                m_aggroSensor.enabled = true;
                //m_minion.Move();
            }
            else
            {
                //Debug.Log("Stop Moving");
                m_minion.StopMoving();
                m_aggroSensor.enabled = false;
            }

            m_behaviorChance = Random.Range(0, 100);

            if (!m_lockBehavior)
            {
                m_behaviorCount = Random.Range(1, 6);
                Behaviors();
            }

            if (m_minion.IsIdle())
            {
                m_minion.SetRootMotionY(false);
            }
        }

        public bool FacingDestination(Vector2 destination)
        {
            Vector2 pos = transform.position;
            if (destination.x > pos.x && transform.localScale.x == -1 || destination.x < pos.x && transform.localScale.x == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Patience()
        {
            if (m_currentPatience < m_patience)
            {
                m_currentPatience += Time.deltaTime;
            }
            else
            {
                //Debug.Log("Nullify Target");
                m_target = null;
                m_enablePatience = false;
            }
        }

        public void Patrol()
        {
            Vector2 pos = transform.position;
            var destination = m_patrol.GetInfo(pos).destination;
            if (pos != destination)
            {
                //var currentPath = m_navigationTracker.currentPathSegment;
                m_aggroSensor.enabled = Mathf.RoundToInt(transform.position.y) < destination.y + 2 && Mathf.RoundToInt(transform.position.y) > destination.y - 2 ? m_aggroSensor.enabled = true : m_aggroSensor.enabled = false;
                if (FacingDestination(destination))
                {
                    //Debug.Log("Destination: " + destination + "Current Pos: " + pos + "_" + FacingDestination(destination));
                    m_minion.MovetoDestination(destination, false);
                    //m_minion.Move();
                }
                else
                {
                    m_minion.Turn();
                }
            }
            //else
            //{
            //    m_navigationTracker.SetDestination(destination);
            //}
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
        }

        void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (transform.localScale.x == -1)
            {
                m_terrainSensor.SetRotation(0f);
            }
            else if (transform.localScale.x == 1)
            {
                m_terrainSensor.SetRotation(180f);
            }

            if (m_target == null)
            {
                Debug.Log("Target Null");
                if (m_detected)
                {
                    m_detected = false;
                    m_minion.Standby();
                }
            }
            else if (m_target != null && !m_minion.IsDead())
            {
                Debug.Log("Target Not Null");
                NormalMode();
                if (m_enablePatience)
                {
                    Patience();
                }
            }

            if (m_minion.IsDead())
            {
                if(m_currenCounter < m_reviveCounter)
                {
                    m_currenCounter += Time.deltaTime;
                }
                else
                {
                    m_currenCounter = 0;
                    m_target = null;
                    m_detected = false;
                    m_minion.ResetAI();
                }
            }
        }
    }
}
