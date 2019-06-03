using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Spiderwoman02Brain : MinionAIBrain<Spiderwoman02>, IAITargetingBrain
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
        private RaySensor m_backSensor;
        [SerializeField]
        private RaySensor m_edgewallSensor;
        [SerializeField]
        private float m_idleRange;
        [SerializeField]
        private float m_castRange;
        [SerializeField]
        private Collider2D m_aggroSensor;

        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_attackRange;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private float m_attackChance;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_lockBehavior;
        private bool m_hasThrown;
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

        private void PlayerDetector()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_edgewallSensor.transform.position, -Vector2.up, 1000, LayerMask.GetMask("Player"));

            // If it hits something...
            if (hit.collider != null)
            {
                SetTarget(hit.collider.GetComponentInParent<IEnemyTarget>());
                Debug.Log("Hitting " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            }
            else
            {
                Debug.Log("Not Hitting Anythaaaang");
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
                        if (GetDistance() < m_attackRange && m_behaviorChance < m_attackChance && GetDistance() > m_castRange)
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
            if (!m_detected)
            {
                m_detected = true;
                m_minion.SetState();
                GetComponent<ShiftingCharacterPhysics2D>().SetOrientation(WorldOrientation.Up);
                transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            }
            else
            {
                GetDistance();
                if (GetDistance() > 5)
                {
                    Turn();
                }
            }

            m_behaviorChance = Random.Range(0, 100);
            if (m_groundSensor.isDetecting)
            {

                if (m_wallSensor.isDetecting && m_enablePatience)
                {
                    Debug.Log("Terrain Sensor Detecting");
                    //m_enablePatience = true;
                    m_currentPatience = m_patience;
                }

                if (!m_minion.IsTurning() && GetDistance() < m_idleRange && m_behaviorChance < 100 && m_currentBehavior == 0 && !m_backSensor.isDetecting)
                {
                    m_minion.MoveBackwards(true);
                    m_backSensor.gameObject.SetActive(true);
                    m_lockBehavior = true;
                }
                else if (m_currentBehavior == 0 /*&& m_minion.IsIdle()*/ && m_facingPlayer && GetDistance() > m_idleRange && !m_wallSensor.isDetecting)
                {
                    //Vector2 pos = transform.position;
                    m_minion.MovetoDestination(true);
                    m_backSensor.gameObject.SetActive(false);
                    m_lockBehavior = false;
                    //m_minion.Move();
                }
                else if (m_backSensor.isDetecting || m_wallSensor.isDetecting)
                {
                    m_lockBehavior = false;
                    m_minion.StopMoving();
                }
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

                if (m_sesnorCount < 3)
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
                    m_minion.MovetoDestination(false);
                    //m_minion.Move();
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
                m_groundSensor.SetRotation(90f);
                m_backSensor.SetRotation(180f);
                m_edgewallSensor.SetRotation(-90f);
            }
            else if (transform.localScale.x == 1)
            {
                m_wallSensor.SetRotation(180f);
                m_groundSensor.SetRotation(-90f);
                m_backSensor.SetRotation(0f);
                m_edgewallSensor.SetRotation(90f);
            }

            //Debug.Log(GetComponent<ShiftingCharacterPhysics2D>().groundAngle);
            //if (GetComponent<CharacterPhysics2D>())
            //{
            //    transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.y);
            //}
            ////else if(GetComponent<ShiftingCharacterPhysics2D>().velocity.y < 0)
            //else
            //{
            //    transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.y);
            //}

            if (m_target == null)
            {
                //Debug.Log("Target is Null");
                //m_target = m_character;
                //m_minion.Idle2();
                m_detected = false;
                m_minion.SetHostility(false);
                PlayerDetector();
                Patrol();
            }
            else if (m_target != null)
            {
                m_minion.SetHostility(true);
                NormalMode();
                if (m_enablePatience)
                {
                    Patience();
                }
            }
        }
    }
}
