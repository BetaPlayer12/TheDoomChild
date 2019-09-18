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
    public class RangeBeeDroneAI : CombatAIBrain<RangeBeeDroneAI.Info>
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

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_rangeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo rangeAttack => m_rangeAttack;
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

            [SerializeField]
            private SimpleProjectileAttackInfo m_stingerProjectile;
            public SimpleProjectileAttackInfo stingerProjectile => m_stingerProjectile;
            [SerializeField]
            private GameObject m_burstGO;
            public GameObject burstGO => m_burstGO;


            [SerializeField]
            private float m_delayShotTime;
            public float delayShotTimer => m_delayShotTime;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_rangeAttack.SetData(m_skeletonDataAsset);
                m_stingerProjectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
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

        //stored timer
        private float postAtan2;

        //
        private float timeCounter;

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
                m_stateHandle.SetState(State.Chasing);
                m_currentPatience = 0;
                m_enablePatience = false;
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

        private IEnumerator RangeAttackRoutine()
        {
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.rangeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rangeAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
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

        public override void ApplyData()
        {
            if (m_info != null)
            {
                m_spineListener.Unsubcribe(m_info.stingerProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            }
            base.ApplyData();
            if (m_stingerLauncher != null)
            {
                m_stingerLauncher.SetProjectile(m_info.stingerProjectile.projectileInfo);
                m_spineListener.Subscribe(m_info.stingerProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            }

        }

        // benjo's alteration
        public Vector2 GetPlayerTransform()
        {
            var m_playerTransform = m_targetInfo.position;
            return m_playerTransform;
        }

        public float GetProjectileSpeed()
        {
            var m_bulletSpeed = m_info.stingerProjectile.projectileInfo.speed;
            return m_bulletSpeed;
        }

        private void LaunchStingerProjectile()
        {
            var target = m_targetInfo.position; //No Parabola      
            Vector2 spitPos = m_stingerPos.position;
            Vector3 v_diff = (target - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            if (m_info.delayShotTimer <= timeCounter)
            {
                postAtan2 = atan2;
                timeCounter = 0;
                Debug.Log("Replacement trigger");
            }
           
            m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
            GameObject burst = Instantiate(m_info.burstGO, spitPos, m_stingerPos.rotation);
            m_stingerLauncher.LaunchProjectile();
        }
        
        protected override void Awake()
        {
            Debug.Log("Update override trigger");
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_stingerPos);
        }

        protected override void Start()
        {
            base.Start();
            timeCounter = m_info.delayShotTimer + 1;
            m_animation.animationState.Event += HandleEvent;
            m_spineListener.Subscribe(m_info.stingerProjectile.launchOnEvent, LaunchStingerProjectile );
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                      m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.Patrol:

                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                   
                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation);
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(RangeAttackRoutine());
                    break;
                case State.Chasing:
                   
                        if (IsFacingTarget())
                        {

                            if (IsTargetInRange(m_info.stingerProjectile.range))
                            {
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {

                                var target = m_targetInfo.position;
                                target.y -= 0.5f;
                                m_animation.DisableRootMotion();
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
                        }
                    
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }

            timeCounter += 1 * Time.deltaTime;

           
        }
       

    }
}
