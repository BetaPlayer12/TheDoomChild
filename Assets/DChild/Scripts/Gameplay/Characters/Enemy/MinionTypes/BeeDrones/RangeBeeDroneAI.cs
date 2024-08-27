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
    public class RangeBeeDroneAI : CombatAIBrain<RangeBeeDroneAI.Info>, ISummonedEnemy
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
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idle2Animation = new BasicAnimationInfo();
            public BasicAnimationInfo idle2Animation => m_idle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

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

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idle2Animation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }
       

        private enum State
        {
            Idle,
            Patrol,
            Detect,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroSensor;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
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
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        private ProjectileLauncher m_stingerLauncher;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isDetecting;

        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_RangeAttackClip;
        //[SerializeField]
        //private AudioClip m_RangeBeeDroneDeadClip;


        //stored timer
        private float postAtan2;
        
        private float timeCounter;
        // player position
        private Vector2 playerPosition;

        private float m_currentCD;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
            //m_aggroSensor.enabled = true;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            //else
            //{
            //    //m_enablePatience = true;
            //}
        }
        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            enabled = false;
            transform.position = position;
            m_character.physics.simulateGravity = false;
            m_hitbox.Enable();
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            Awake();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
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
                //StopAllCoroutines();
                //m_targetInfo.Set(null, null);
                //m_enablePatience = false;
                //m_stateHandle.SetState(State.Patrol);
                ResetBrain();
            }
        }

        private void ResetBrain()
        {
            StopAllCoroutines();
            m_currentCD = 0;
            m_aggroSensor.enabled = true;
            m_targetInfo.Set(null, null);
            m_enablePatience = false;
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.Patrol);
        }

        private IEnumerator DetectRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(2f);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RangeAttackRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_aggroSensor.enabled = false;
            m_animation.SetAnimation(0, m_info.rangeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rangeAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_aggroSensor.enabled = true;
            if (!m_aggroSensor.IsTouchingLayers(LayerMask.NameToLayer("Player")) /*&& m_stateHandle.currentState == State.ReevaluateSituation*/)
            {
                ResetBrain();
                //Debug.Log("Contain'ts Player");
            }
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
            //m_Audiosource.clip = m_RangeBeeDroneDeadClip;
            //m_Audiosource.Play();
           
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
            if (IsFacingTarget())
            {
                var target = m_targetInfo.position; //No Parabola      
                Vector2 spitPos = m_stingerPos.position;
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                if (m_info.delayShotTimer <= timeCounter)
                {
                    postAtan2 = atan2;
                    timeCounter = 0;
                    Debug.Log("update Check");
                }

                m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
                GameObject burst = Instantiate(m_info.burstGO, spitPos, m_stingerPos.rotation);
                m_stingerLauncher.LaunchProjectile();
                //m_Audiosource.clip = m_RangeAttackClip;
                //m_Audiosource.Play();
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }
        
        protected override void Awake()
        {
           
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
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
            timeCounter += 1 * Time.deltaTime;

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    if (IsFacingTarget())
                    {
                        StartCoroutine(DetectRoutine());
                        //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                      m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    if (m_agent.hasPath)
                    {
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idle2Animation, true);
                    }
                   
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    m_agent.Stop();
                   
                    break;
                case State.Attacking:
                    playerPosition = m_targetInfo.position;
                    m_stateHandle.Wait(State.Cooldown);
                    m_aggroSensor.enabled = false;
                    m_attackHandle.ExecuteAttack(m_info.rangeAttack.animation, m_info.idleAnimation.animation);
                    //StartCoroutine(RangeAttackRoutine());
                    break;
                case State.Cooldown:
                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        if (!m_aggroSensor.IsTouchingLayers(LayerMask.NameToLayer("Player")) /*&& m_stateHandle.currentState == State.ReevaluateSituation*/)
                        {
                            ResetBrain();
                            //Debug.Log("Contain'ts Player");
                        }
                        m_currentCD = 0;
                        m_aggroSensor.enabled = true;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }
                    break;
                case State.Chasing:
                   
                        if (IsFacingTarget())
                        {

                            if (IsTargetInRange(m_info.stingerProjectile.range))
                            {
                                m_agent.Stop();
                                m_stateHandle.SetState(State.Attacking);
                            }
                        //else
                        //{

                        //    var target = m_targetInfo.position;
                        //    //target.y -= 0.5f;
                        //    m_animation.DisableRootMotion();
                        //    if (m_character.physics.velocity != Vector2.zero)
                        //    {
                        //        m_animation.SetAnimation(0, m_info.move.animation, true);
                        //    }
                        //    else
                        //    {
                        //        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        //    }
                        //    m_agent.SetDestination(target);

                        //        m_agent.Move(m_info.move.speed);

                        //}

                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
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



            //if (m_enablePatience)
            //{
            //    Patience();
            //}

        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
        public void DestroyObject()
        {
        }
        
    }
}
