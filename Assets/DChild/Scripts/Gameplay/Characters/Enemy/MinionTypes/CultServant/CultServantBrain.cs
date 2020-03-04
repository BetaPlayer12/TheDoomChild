using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CultServantBrain : MinionAIBrain<CultServant>, IAITargetingBrain
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
        private float m_teleportAwayRange;

        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_attackCastingRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_attackConjureRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_teleportRange;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_attackCastingChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_attackConjureChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_teleportChance;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_lockBehavior;
        private bool m_hasThrown;
        private bool m_enablePatience;
        private bool m_isHostile;
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
                    m_minion.Idle(m_target != null ? m_isHostile : !m_isHostile);
                    if (m_minion.IsIdle())
                    {
                        Debug.Log("Do Turn Squence");
                        m_minion.Turn();
                    }
                }
                else if (m_target.position.x > transform.position.x && transform.localScale.x == 1)
                {
                    m_facingPlayer = false;
                    m_minion.Idle(m_target != null ? m_isHostile : !m_isHostile);
                    if (m_minion.IsIdle())
                    {
                        Debug.Log("Do Turn Squence");
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
                        if (GetDistance() < m_attackCastingRange && m_behaviorChance < m_attackCastingChance && GetDistance() > m_castRange)
                        {
                            m_minion.AttackCastingSpell();
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
                        if (GetDistance() < m_attackConjureRange && m_behaviorChance < m_attackConjureChance && GetDistance() > m_castRange)
                        {
                            m_minion.AttackConjureBooks();
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
                        if (GetDistance() < m_teleportRange && m_behaviorChance < m_teleportChance && GetDistance() > m_castRange)
                        {
                            m_minion.Teleport();
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
                m_minion.Detect();
            }
            else
            {
                GetDistance();
                if (GetDistance() > 5)
                {
                    Turn();
                }
            }

            //Debug.Log("Ratking moving: " + GetDistance());
            if (m_currentBehavior == 0 /*&& m_minion.IsIdle()*/ && m_facingPlayer && GetDistance() > m_idleRange)
            {
                Vector2 pos = transform.position;
                m_minion.MovetoTarget(pos);
            }
            else
            {
                Debug.Log("Stop Moving");
                m_minion.StopMoving(m_isHostile);
            }

            m_behaviorChance = Random.Range(0, 100);
            
            if (/*m_minion.IsIdle() &&*/ GetDistance() < m_castRange && m_behaviorChance < 5 && m_currentBehavior == 0)
            {
                m_minion.TeleportAway();
            }

            if (m_minion.HasTeleportedAway())
            {
                transform.position = new Vector3(transform.position.x + (m_teleportAwayRange + transform.localScale.x), transform.position.y, transform.position.z);
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
                if (FacingDestination(destination))
                {
                    //Debug.Log("Destination: " + destination + "Current Pos: " + pos + "_" + FacingDestination(destination));
                    //Debug.Log("Ratking patrolling: " + pos);
                    m_minion.MovetoDestination(destination);
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
                m_wallSensor.SetRotation(0f);
                m_groundSensor.SetRotation(90f);
            }
            else if (transform.localScale.x == 1)
            {
                m_wallSensor.SetRotation(180f);
                m_groundSensor.SetRotation(-90f);
            }

            if (m_target == null)
            {
                //Debug.Log("Target is Null");
                //m_target = m_character;
                //m_minion.Idle2();
                m_isHostile = false;
                m_detected = false;
                Patrol();
            }
            else if (m_target != null)
            {
                //Debug.Log("Target is not Null: " + m_target);
                m_isHostile = true;
                NormalMode();
                if (m_enablePatience)
                {
                    Patience();
                }
            }
        }
    }
}
