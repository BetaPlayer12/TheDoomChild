using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class OwlBrain : MinionAIBrain<Owl>, IAITargetingBrain
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
        private Collider2D m_aggroSensor;

        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_basicAttackRange;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_basicAttackChance;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_lockBehavior;
        private bool m_hasThrown;
        private bool m_enablePatience;
        private float m_sesnorCount;

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
                    m_minion.Idle();
                    if (m_minion.IsIdle())
                    {
                        //Debug.Log("Do Turn Squence");
                        m_minion.Turn();
                    }
                }
                else if (m_target.position.x > transform.position.x && transform.localScale.x == 1)
                {
                    m_facingPlayer = false;
                    m_minion.Idle();
                    if (m_minion.IsIdle())
                    {
                        //Debug.Log("Do Turn Squence");
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
                        if (GetDistance() < m_basicAttackRange && m_behaviorChance < m_basicAttackChance && GetDistance() > m_castRange)
                        {
                            m_minion.Attack();
                            m_currentBehavior = 0;
                        }
                        else
                        {
                            m_currentBehavior = 0;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void NormalMode()
        {
            GetDistance();
            if (GetDistance() > 5)
            {
                Turn();
            }

            if (m_terrainSensor.isDetecting && m_enablePatience)
            {
                Debug.Log("Terrain Sensor Detecting");
                //m_enablePatience = true;
                m_currentPatience = m_patience;
            }

            //Debug.Log("Ratking moving: " + GetDistance());
            if (m_currentBehavior == 0 /*&& m_minion.IsIdle()*/ && m_facingPlayer && GetDistance() > m_idleRange && !m_terrainSensor.isDetecting && m_aggroSensor.enabled)
            {
                Vector2 pos = new Vector2(m_target.position.x, m_target.position.y + m_targetFollowOffset);
                m_minion.MovetoDestination(pos, true);
                //m_minion.Move();
            }
            else
            {
                //Debug.Log("Stop Moving");
                m_minion.StopMoving();
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
                m_aggroSensor.enabled = false;
            }
        }

        public void Patrol()
        {
            Vector2 pos = transform.position;
            var destination = m_patrol.GetInfo(pos).destination;
            if (pos != destination)
            {
                //var currentPath = m_navigationTracker.currentPathSegment;
                //m_aggroSensor.enabled = Mathf.RoundToInt(transform.position.y) < destination.y + 2 && Mathf.RoundToInt(transform.position.y) > destination.y - 2 ? m_aggroSensor.enabled = true : m_aggroSensor.enabled = false;
                
                if(m_sesnorCount < 3)
                {
                    m_sesnorCount += Time.deltaTime;
                }
                else
                {
                    m_sesnorCount = 0;
                    //Debug.Log("WAKANDA");
                    m_aggroSensor.enabled = true;
                }

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
                Patrol();
            }
            else if (m_target != null)
            {
                NormalMode();
                if (m_enablePatience)
                {
                    Patience();
                }
            }
        }
    }
}
