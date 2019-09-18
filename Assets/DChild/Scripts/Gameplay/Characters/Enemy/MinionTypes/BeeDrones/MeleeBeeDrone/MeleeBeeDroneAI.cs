using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MeleeBeeDroneAI : CombatAIBrain<MeleeBeeDroneAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [SerializeField]
            private int m_timePause;
            public int timePause => m_timePause;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_meleeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo meleeAttack => m_meleeAttack;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_chargeAnimation;
            public string chargeAnimation => m_chargeAnimation;

            [SerializeField]
            private GameObject m_burstGO;
            public GameObject burstGO => m_burstGO;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                meleeAttack.SetData(m_skeletonDataAsset);
              
#endif
            }
        }

        private enum State
        {
            Idle,
            chargeIdle,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        //Patience Handler
        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private ProjectileLauncher m_stingerLauncher;
        private float m_currentPatience;
        private bool m_enablePatience;
        private float timeCounter;
        private bool m_chargeOnce;
        private bool m_chargeFacing;
      
        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                
                base.SetTarget(damageable, m_target);
                
                m_currentPatience = 0;
                m_enablePatience = false;

                if(m_chargeOnce == false)
                {
                    m_chargeOnce = true;
                    m_stateHandle.SetState(State.chargeIdle);
                }
                else
                {
                    m_stateHandle.SetState(State.Chasing);
                }

               
            }
            else
            {
                if (!IsTargetInRange(m_info.targetDistanceTolerance))
                {
                    m_enablePatience = true;
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                m_targetInfo.Set(null, null);
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

       
        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            //if (e.Data.Name == m_eventName[0])
            //{
            //    //Debug.Log(m_eventName[0]);
            //    ////Spawn Projectile

            //    if (IsFacingTarget())
            //    {
            //        var target = m_targetInfo.position; //No Parabola
            //        target = new Vector2(target.x, target.y - 2);
            //        Vector2 spitPos = m_stingerPos.position;
            //        Vector3 v_diff = (target - spitPos);
            //        float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            //        GameObject burst = Instantiate(m_info.burstGO, spitPos, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg)); //No Parabola
            //        GameObject shoot = Instantiate(m_info.stingerProjectile, spitPos, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg)); //No Parabola
            //        shoot.GetComponent<Rigidbody2D>().AddForce((m_stingerSpeed + (Vector2.Distance(target, transform.position) * 0.35f)) * shoot.transform.right, ForceMode2D.Impulse);
            //    }
            //}
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }

    


       

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            timeCounter = 0;
            m_chargeOnce = false;
            m_chargeFacing = false;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            
        }

        protected override void Start()
        {
            base.Start();
            m_animation.animationState.Event += HandleEvent;
         
        }

        private void Update()
        {
           Debug.Log("Before Check State: " + m_stateHandle.currentState);
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.chargeIdle:
                   if (IsFacingTarget())
                   {
                        m_animation.SetAnimation(0, m_info.chargeAnimation, true);
                    if (m_info.timePause <= timeCounter)
                    {
                            m_chargeFacing = false;
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_agent.Stop();
                        timeCounter += Time.deltaTime;
                        Debug.Log("pausing");
                    }
                   }
                    else
                    {
                        m_chargeFacing = true;
                        m_stateHandle.SetState(State.Turning);
                       
                        Debug.Log("sensor test");
                    }
                    break;
                

                case State.Patrol:
                    Debug.Log("patrol mode");
                    // if (!m_wallSensor.isDetecting && !m_floorSensor.isDetecting && !m_cielingSensor.isDetecting) //This means that as long as your sensors are detecting something it will patrol
                    // {
                    m_chargeOnce = false;
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    timeCounter = 0;
                    // break;
                    // }
                    // else
                    // {
                    //    m_stateHandle.SetState(State.Turning);
                    //   Debug.Log("sensor test patrol");
                    //  }
                    break;

                case State.Turning:
                   
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    Debug.Log("Turn Bee Drone");
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation);
                    break;
                case State.Attacking:
                    Debug.Log("attacker check4");
                    m_agent.Stop();
                    m_animation.EnableRootMotion(false, false);
                    m_attackHandle.ExecuteAttack(m_info.meleeAttack.animation);
                    m_animation.SetAnimation(0, m_info.meleeAttack.animation, true);
                    m_stateHandle.Wait(State.WaitBehaviourEnd);

                    break;
                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        Debug.Log("phase 2");

                       

                            var target = m_targetInfo.position;
                            target.y -= 0.5f;
                            m_animation.DisableRootMotion();

                            if (IsTargetInRange(m_info.meleeAttack.range))
                            {
                               // m_agent.Stop();
                                Debug.Log("Attack check 45645");
                                m_stateHandle.SetState(State.Attacking);
                               
                            }
                            else
                            {
                               
                                if (m_character.physics.velocity != Vector2.zero)
                                {
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                }
                                else
                                {
                                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                                }
                                m_agent.SetDestination(target);
                                
                                    m_agent.Move(m_info.move.speed);
                                
                            }

                           


                           
                           
                      
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Turning);
                        Debug.Log("sensor test");
                    }

                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_chargeFacing)
                    {
                        m_stateHandle.SetState(State.chargeIdle);
                    }
                    else
                    if (m_targetInfo.isValid)
                    {
                        Debug.Log("phase1");
                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                   
                    {
                        m_stateHandle.SetState(State.Patrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    Debug.Log("on loop");
                     return;
                    
                  
                   

                   
            }
            Debug.Log("Check State: " + m_stateHandle.currentState);



            if (m_enablePatience)
            {
                Patience();
            }
        }


    }
}

