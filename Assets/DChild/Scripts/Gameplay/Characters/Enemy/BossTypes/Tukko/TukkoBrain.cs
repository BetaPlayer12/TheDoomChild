using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TukkoBrain : BossAIBrain<Tukko>, IAITargetingBrain
    {
        [SerializeField]
        private Player m_character;
        private int m_behaviorCount;
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_groundSensor;
        [SerializeField]
        private GameObject m_bomb;
        [SerializeField]
        private Transform m_bombTF;
        [SerializeField]
        private float m_idleRange;
        [SerializeField]
        private float m_invisDuration;
        private float m_currentDuration;
        [SerializeField]
        private float m_oogaBoogaRange;
        
        private int m_behaviorChance;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_jabRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_airborneGrenadesRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_stabComboRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_airborneSlamRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_smokeBombRange;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_jabChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_airborneGrenadeChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_airborneSlamChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_stabComboChance;
        [SerializeField]
        [TabGroup("Ability Chance")]
        private int m_smokeBombChance;

        private TeleportationHandler m_teleporter;

        private bool m_hasTurned;
        private bool m_facingPlayer;
        private int m_currentBehavior;
        private bool m_bombThrown;
        private bool m_smokeBombMode;
        private bool m_lockBehavior;

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
            if(target != null)
            {
                m_target = target;
            }
        }

        public float GetDistance()
        {
            return Vector2.Distance(m_target.position, transform.position);
        }

        private void Turn()
        {
            if(m_currentBehavior == 0)
            {
                if (m_target.position.x < transform.position.x && transform.localScale.x == -1)
                {
                    m_facingPlayer = false;
                    if (!m_smokeBombMode)
                    {
                        m_boss.Idle();
                        if (m_boss.IsIdle())
                        {
                            m_boss.Turn();
                        }
                    }
                    else
                    {
                        m_boss.Turn();
                    }
                }
                else if (m_target.position.x > transform.position.x && transform.localScale.x == 1)
                {
                    m_facingPlayer = false;
                    if (!m_smokeBombMode)
                    {
                        m_boss.Idle();
                        if (m_boss.IsIdle())
                        {
                            m_boss.Turn();
                        }
                    }
                    else
                    {
                        m_boss.Turn();
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
                        if (!m_smokeBombMode)
                        {
                            if (GetDistance() < m_jabRange && m_behaviorChance < m_jabChance && GetDistance() > 10)
                            {
                                m_boss.Jab();
                                m_currentBehavior = 0;
                            }
                            else if (GetDistance() > m_jabRange && m_facingPlayer)
                            {
                                if (m_groundSensor.isDetecting && GetDistance() > m_idleRange)
                                {
                                    m_boss.Move();
                                }
                                else
                                {
                                    m_currentBehavior = 0;
                                }
                            }
                            else
                            {
                                m_currentBehavior = 0;
                            }
                        }
                        else
                            m_boss.Jab();
                    }
                    break;
                case 2:
                    if (m_facingPlayer)
                    {
                        SetBehavior(2);
                    }
                    if (m_currentBehavior == 2 )
                    {
                        if (!m_smokeBombMode)
                        {
                            if (GetDistance() < m_airborneGrenadesRange && m_behaviorChance < m_airborneGrenadeChance && GetDistance() > 10)
                            {
                                m_boss.AirborneGrenades();
                                m_currentBehavior = 0;
                            }
                            else if (GetDistance() > m_airborneGrenadesRange && m_facingPlayer)
                            {
                                if (m_groundSensor.isDetecting && GetDistance() > m_idleRange)
                                {
                                    m_boss.Move();
                                }
                                else
                                {
                                    m_currentBehavior = 0;
                                }
                            }
                            else
                            {
                                m_currentBehavior = 0;
                            }
                        }
                        else
                            m_boss.ShadowGrenades();
                    }
                    break;
                case 3:
                    if (m_facingPlayer)
                    {
                        SetBehavior(3);
                    }
                    if (m_currentBehavior == 3)
                    {
                        if (!m_smokeBombMode)
                        {
                            if (GetDistance() < m_stabComboRange && m_behaviorChance < m_stabComboChance && GetDistance() > 10)
                            {
                                m_boss.StabFull();
                                m_currentBehavior = 0;
                            }
                            else if (GetDistance() > m_stabComboRange && m_facingPlayer)
                            {
                                if (m_groundSensor.isDetecting && GetDistance() > m_idleRange)
                                {
                                    m_boss.Move();
                                }
                                else
                                {
                                    m_currentBehavior = 0;
                                }
                            }
                            else
                            {
                                m_currentBehavior = 0;
                            }
                        }
                        else
                            m_boss.StabFull();
                    }
                    break;
                case 4:
                    if (m_facingPlayer)
                    {
                        SetBehavior(4);
                    }
                    if (m_currentBehavior == 4)
                    {
                        if (!m_smokeBombMode)
                        {
                            if (GetDistance() < m_airborneSlamRange && m_behaviorChance < m_airborneSlamChance && GetDistance() > 10)
                            {
                                m_boss.AirborneSlam();
                                m_currentBehavior = 0;
                            }
                            else if (GetDistance() > m_airborneSlamRange && m_facingPlayer)
                            {
                                if (m_groundSensor.isDetecting && GetDistance() > m_idleRange)
                                {
                                    m_boss.Move();
                                }
                                else
                                {
                                    m_currentBehavior = 0;
                                }
                            }
                            else
                            {
                                m_currentBehavior = 0;
                            }
                        }
                        else
                            m_boss.ShadowSlam();
                    }
                    break;
                case 5:
                    if (m_facingPlayer)
                    {
                        SetBehavior(5);
                    }
                    if (m_currentBehavior == 5 && !m_smokeBombMode)
                    {
                        if (GetDistance() < m_smokeBombRange && m_behaviorChance < m_smokeBombChance && GetDistance() > 10)
                        {
                            if (m_boss.IsIdle())
                            {
                                m_boss.SmokeBomb();
                                m_boss.DisableHitboxes();
                                m_lockBehavior = true;
                            }
                            else if (!m_boss.IsThrowingSmoke())
                            {
                                m_boss.Idle();
                            }
                        }
                        else if (GetDistance() > m_smokeBombRange && m_facingPlayer)
                        {
                            if (m_groundSensor.isDetecting && GetDistance() > m_idleRange)
                            {
                                m_boss.Move();
                            }
                            else
                            {
                                m_currentBehavior = 0;
                            }
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
            if (m_target == null)
            {
                m_target = m_character;
            }
            else if (m_target != null)
            {
                GetDistance();
                if (GetDistance() > 5)
                {
                    Turn();
                }

                m_behaviorChance = Random.Range(0, 100);
                if (m_boss.IsIdle() && GetDistance() < 10 && m_behaviorChance < 5 && m_currentBehavior == 0)
                {
                    m_boss.HopBackwards();
                }
                if (!m_lockBehavior)
                {
                    m_behaviorCount = Random.Range(1, 6);
                    Behaviors();
                }

                if (m_boss.IsIdle())
                {
                    m_boss.SetRootMotionY(false);
                    m_bombThrown = false;
                }

                if (m_boss.IsInvisible())
                {
                    m_smokeBombMode = true;
                }
            }
        }

        private void ShadowMode()
        {
            if(m_currentDuration < m_invisDuration)
            {
                m_currentDuration += Time.deltaTime;
                m_currentBehavior = 0;
                m_behaviorCount = 0;
            }
            else
            {
                if(m_behaviorCount == 0)
                {
                    transform.position = m_teleporter.SetDestination(transform.position, m_target.position, m_oogaBoogaRange, 0);
                    Turn();
                    m_behaviorCount = Random.Range(1, 5);
                }
                else
                {
                    m_boss.EnableHitboxes();
                    m_facingPlayer = true;
                    Behaviors();
                    EndShadowMode();
                }
            }
        }

        private void EndShadowMode()
        {
            m_boss.Idle();
            m_currentBehavior = 0;
            m_currentDuration = 0;
            m_lockBehavior = false;
            m_smokeBombMode = false;
        }

        private void SpawnBombs(float startPos, float bombInterval)
        {
            GameObject[] bombs = new GameObject[3];

            for (int i = 0; i < bombs.Length; i++)
            {
                startPos += bombInterval;
                bombs[i] = Instantiate(m_bomb, m_bombTF.position, Quaternion.Euler(new Vector3(0, 0, startPos)));
            }
        }

        void Start()
        {
            if(transform.localScale.x == -1)
            {
                m_wallSensor.SetRotation(180f);
                m_groundSensor.SetRotation(90f);
            }
            else if(transform.localScale.x == 1)
            {
                m_wallSensor.SetRotation(0f);
                m_groundSensor.SetRotation(-90f);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_teleporter = GetComponent<TeleportationHandler>();
        }

        void Update()
        {
            if (m_boss.waitForBehaviourEnd)
                return;

            if (m_currentBehavior == 0 && m_boss.IsIdle() && m_facingPlayer && GetDistance() > m_idleRange)
            {
                m_boss.Move();
            }

            if (!m_smokeBombMode)
            {
                NormalMode();
            }
            else
            {
                ShadowMode();
            }

            if (m_boss.IsThrownBomb() && !m_bombThrown)
            {
                m_bombThrown = true;
                if (transform.localScale.x == 1)
                {
                    SpawnBombs(-95, -20);
                }
                else
                {
                    SpawnBombs(-85, 20);
                }
            }
        }
    }
}