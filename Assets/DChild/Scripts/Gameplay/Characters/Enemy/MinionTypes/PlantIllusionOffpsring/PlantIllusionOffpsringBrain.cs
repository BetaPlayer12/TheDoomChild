using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PlantIllusionOffpsringBrain : MinionAIBrain<PlantIllusionOffpsring>, IAITargetingBrain
    {
        [SerializeField]
        private float m_patience;
        private float m_currentPatience;
        private int m_behaviorCount;
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_groundSensor;
        [SerializeField]
        private float m_idleRange;
        [SerializeField]
        private float m_castRange;
        [SerializeField]
        private Collider2D m_aggroSensor;

        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_jumpAttackRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_spitRange;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_jumpAttackChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_spitChance;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_lockBehavior;
        private bool m_enablePatience;
        private bool m_detected;
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
            if (m_currentBehavior == 0)
            {
                if (m_target.position.x < transform.position.x && transform.localScale.x == -1)
                {
                    m_facingPlayer = false;
                    m_minion.Idle();
                    if (m_minion.IsIdle())
                    {
                        m_minion.Turn();
                    }
                }
                else if (m_target.position.x > transform.position.x && transform.localScale.x == 1)
                {
                    m_facingPlayer = false;
                    m_minion.Idle();
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
                        if (GetDistance() < m_jumpAttackRange && m_behaviorChance < m_jumpAttackChance && GetDistance() > m_castRange)
                        {
                            m_minion.JumpAttack();
                            m_currentBehavior = 0;
                        }
                        else
                        {
                            m_currentBehavior = 0;
                        }
                    }
                    break;
                case 2:
                    if (m_facingPlayer)
                    {
                        SetBehavior(2);
                    }
                    if (m_currentBehavior == 2)
                    {
                        if (GetDistance() < m_spitRange && m_behaviorChance < m_spitChance && GetDistance() > m_castRange)
                        {
                            m_minion.SpitAttack();
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
            if (!m_detected)
            {
                m_detected = true;
                m_minion.BurrowReveal();
            }
            else
            {
                GetDistance();
                if (GetDistance() > m_idleRange)
                {
                    Turn();
                }
            }

            if (m_wallSensor.isDetecting && m_enablePatience)
            {
                Debug.Log("Terrain Sensor Detecting");
                //m_enablePatience = true;
                m_currentPatience = m_patience;
                m_minion.Burrow();
            }

            if (m_currentBehavior == 0 /*&& m_minion.IsIdle()*/ && m_facingPlayer && GetDistance() > m_idleRange && !m_wallSensor.isDetecting)
            {
                m_minion.MovetoDestination(true);
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
                m_target = null;
                m_enablePatience = false;
                m_detected = false;
                m_aggroSensor.enabled = false;
                m_minion.Burrow();
            }
        }

        public void Patrol()
        {
            Vector2 pos = transform.position;
            var destination = m_patrol.GetInfo(pos).destination;
            if (pos != destination)
            {
                //var currentPath = m_navigationTracker.currentPathSegment;

                if (FacingDestination(destination) && Vector2.Distance(destination, transform.position) > 6f)
                {
                    m_minion.MovetoDestination(false);
                    Debug.Log(Vector2.Distance(destination, transform.position));
                }
                else if (Vector2.Distance(destination, transform.position) < 6f)
                {
                    m_minion.BurrowedIdle();
                }
                else
                {
                    m_minion.Turn();
                }
            }
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
                m_wallSensor.SetRotation(0f);
                m_groundSensor.SetRotation(-90f);
            }
            else if (transform.localScale.x == 1)
            {
                m_wallSensor.SetRotation(180f);
                m_groundSensor.SetRotation(90f);
            }

            if (m_target == null)
            {
                //Debug.Log("Target is Null");

                if (m_sesnorCount < 10)
                {
                    m_sesnorCount += Time.deltaTime;
                }
                else
                {
                    m_sesnorCount = 0;
                    //Debug.Log("WAKANDA");
                    m_aggroSensor.enabled = true;
                }

                //m_minion.BurrowedIdle();
                Patrol();
            }
            else if (m_target != null)
            {
                //m_minion.SetTarget(m_target.position);
                NormalMode();
                if (m_enablePatience)
                {
                    Patience();
                }
            }
        }
    }
}
