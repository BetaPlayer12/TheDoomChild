using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RatKingBrain : MinionAIBrain<RatKing>, IAITargetingBrain
    {
        [SerializeField]
        private float m_patience;
        private float m_currentPatience;
        private int m_behaviorCount;
        [SerializeField]
        private GameObject m_knife;
        [SerializeField]
        private Transform m_throwPoint;
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_groundSensor;
        [SerializeField]
        private float m_idleRange;

        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_stabRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_throwRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_summonRange;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_stabChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_throwChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_summonChance;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_lockBehavior;
        private bool m_hasThrown;
        private bool m_enablePatience;


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
                        if (GetDistance() < m_stabRange && m_behaviorChance < m_stabChance && GetDistance() > 10)
                        {
                            m_minion.AttackStab();
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
                        if (GetDistance() < m_throwRange && m_behaviorChance < m_throwChance && GetDistance() > 20)
                        {
                            //Debug.Log("Throw Range: " + GetDistance() + " Default: " + m_throwRange);
                            m_minion.ThrowKnife();
                            m_currentBehavior = 0;
                        }
                        else
                        {
                            m_currentBehavior = 0;
                        }
                    }
                    break;
                case 3:
                    if (m_facingPlayer)
                    {
                        SetBehavior(3);
                    }
                    if (m_currentBehavior == 3)
                    {
                        if (GetDistance() < m_summonRange && m_behaviorChance < m_summonChance && GetDistance() > 10)
                        {
                            m_minion.SummonRat();
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

            if (m_currentBehavior == 0 /*&& m_minion.IsIdle()*/ && m_facingPlayer && GetDistance() > m_idleRange)
            {
                //Debug.Log("Ratking moving");
                m_minion.Move();
            }

            m_behaviorChance = Random.Range(0, 100);
            if (m_minion.IsIdle() && GetDistance() < 10 && m_behaviorChance < 5 && m_currentBehavior == 0)
            {
                m_minion.Hop();
            }
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
            if(m_currentPatience < m_patience)
            {
                m_currentPatience += Time.deltaTime;
            }
            else
            {
                Debug.Log("Nullify Target");
                m_target = null;
                m_enablePatience = false;
            }
        }

        public void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;
            Vector2 pos = transform.position;
            if (pos != destination)
            {
                //var currentPath = m_navigationTracker.currentPathSegment;
                if (FacingDestination(destination))
                {
                    //Debug.Log("Destination: " + destination + "Current Pos: " + pos + "_" + FacingDestination(destination));
                    m_minion.Move();
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
                m_wallSensor.SetRotation(180f);
                m_groundSensor.SetRotation(90f);
            }
            else if (transform.localScale.x == 1)
            {
                m_wallSensor.SetRotation(0f);
                m_groundSensor.SetRotation(-90f);
            }

            if (m_minion.IsIdle())
            {
                //Debug.Log("YES");
                m_hasThrown = false;
            }
            //else if (!m_minion.IsIdle())
            //{
            //    m_minion.IsItReallyIdle();
            //}

            if (m_target == null)
            {
                //Debug.Log("Target is Null");
                //m_target = m_character;
                //m_minion.Idle2();
                Patrol();
            }
            else if (m_target != null)
            {
                //Debug.Log("Target is not Null: " + m_target);
                NormalMode();
                if (m_enablePatience)
                {
                    Patience();
                }
            }

            if (m_minion.IsThrownKnife() && !m_hasThrown)
            {
                m_hasThrown = true;
                if (transform.localScale.x == 1)
                {
                    GameObject knife = Instantiate(m_knife, m_throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
                else
                {
                    GameObject knife = Instantiate(m_knife, m_throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
            }
        }
    }
}
