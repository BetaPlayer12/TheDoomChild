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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Zombie10")]
    public class Zombie10AI : CombatAIBrain<Zombie10AI.Info>
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
            /*[SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;*/
            [SerializeField]
            private SimpleAttackInfo m_swipeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo swipeAttack => m_swipeAttack;
            [SerializeField]
            private float m_swipeAttackCD;
            public float swipeAttackCD => m_swipeAttackCD;
            [SerializeField]
            private SimpleAttackInfo m_leapAttack = new SimpleAttackInfo();
            public SimpleAttackInfo leapAttack => m_leapAttack;
            [SerializeField, MinValue(0)]
            private float m_leapAttackCD;
            public float leapAttackCD => m_leapAttackCD;
            [SerializeField]
            private SimpleAttackInfo m_groundPoundAttack = new SimpleAttackInfo();
            public SimpleAttackInfo groundPoundAttack => m_groundPoundAttack;
            [SerializeField, MinValue(0)]
            private float m_groundPoundAttackCD;
            public float groundPoundAttackCD => m_groundPoundAttackCD;
            [SerializeField, MinValue(0)]
            private float m_leapTime;
            public float leapTime => m_leapTime;
            [SerializeField, MinValue(0)]
            private float m_shockwaveSpeed;
            public float shockwaveSpeed => m_shockwaveSpeed;
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
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_groundPoundEvent;
            public string groundPoundEvent => m_groundPoundEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_innardsThrowEvent;
            public string innardsThrowEvent => m_innardsThrowEvent;
            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            [SerializeField]
            private GameObject m_groundPoundProjectile;
            public GameObject groundPoundProjectile => m_groundPoundProjectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_swipeAttack.SetData(m_skeletonDataAsset);
                m_leapAttack.SetData(m_skeletonDataAsset);
                m_groundPoundAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Patrol,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Swipe,
            Leap,
            GroundPound,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_shockwaveBB;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_innardSlashBB;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_groundPoundBB;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private float m_currentCD;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_shockwaveFX;

        [SerializeField, TabGroup("Cannon Values")]
        private float m_speed;
        [SerializeField, TabGroup("Cannon Values")]
        private float m_gravityScale;
        [SerializeField, TabGroup("Cannon Values")]
        private Vector2 m_posOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private float m_velOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private Vector2 m_targetOffset;

        private float m_targetDistance;


        [SerializeField]
        private SpineEventListener m_spineEventListener;
        [SerializeField]
        private Transform m_throwPoint;
        [SerializeField]
        private Transform m_groundPoundPoint;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private float m_currentAttackCD;
        private State m_turnState;
        private Vector2 m_targetLastPos;

        private Coroutine m_innardsColliderRoutine;
        private Coroutine m_groundPoundColliderRoutine;

        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_selfCollider.SetActive(false);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
                m_enablePatience = false;
            }
            else
            {
                //if (!m_enablePatience)
                //{
                //    m_enablePatience = true;
                //    //Patience();
                //    StartCoroutine(PatienceRoutine());
                //}
                m_enablePatience = true;
                //StartCoroutine(PatienceRoutine());
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
                m_selfCollider.SetActive(false);
                m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                m_movement.Stop();

        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                m_currentCD += m_currentCD + 0.5f;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.Cooldown);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetEmptyAnimation(0, 0);
                m_stateHandle.ApplyQueuedState();
            }
        }

        private Vector2 BallisticVelocity(Vector2 targetCenterMass)
        {
            //Vector2 targetCenterMass = m_targetLastPos;
            m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;

            m_targetDistance = Vector2.Distance(targetCenterMass, m_throwPoint.position);
            var dir = (targetCenterMass - new Vector2(m_throwPoint.position.x, m_throwPoint.position.y));
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;

            var currentSpeed = m_speed;

            var vel = Mathf.Sqrt(dist * m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale);
            return (vel * new Vector3(dir.x * m_posOffset.x, dir.y * m_posOffset.y).normalized) * m_targetOffset.sqrMagnitude; //closest to accurate
        }

        private void LaunchProjectile(Vector2 target)
        {
            if (m_targetInfo.isValid)
            {
                //m_muzzleFX.Play();
                //Vector2 target = m_targetLastPos;
                target = new Vector2(target.x, target.y - 2);
                Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_throwPoint.position.x - 1.5f : m_throwPoint.position.x + 1.5f, m_throwPoint.position.y - 0.75f);
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

                GameObject projectile = m_info.projectile.projectileInfo.projectile;
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                instance.transform.position = m_throwPoint.position;
                var component = instance.GetComponent<Projectile>();
                component.ResetState();
                //component.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
                component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVelocity(target));
                //return instance.gameObject;
            }
        }



        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ThrowGutsRoutine()
        {
            yield return new WaitForSeconds(.5f);
            LoopThrowProjectile(2);
            yield return new WaitForSeconds(.75f);
            LoopThrowProjectile(2);
            yield return null;
        }

        private void LoopThrowProjectile(int loops)
        {
            m_targetLastPos = m_targetInfo.transform.GetComponent<Character>().centerMass.position;
            Vector2 currentPos = m_targetLastPos;
            for (int i = 0; i < loops; i++)
            {
                while (Vector2.Distance(m_targetLastPos, currentPos) <= 15)
                {
                    currentPos = new Vector2(m_targetLastPos.x + UnityEngine.Random.Range(-15 * transform.localScale.x, 20 * transform.localScale.x), m_targetLastPos.y + UnityEngine.Random.Range(-5, 5));
                }
                m_targetLastPos = currentPos;
                LaunchProjectile(currentPos);
            }
        }

        private IEnumerator LeapAttackVelocityRoutine(Vector2 target)
        {
            var targetDistance = Vector2.Distance(target, transform.position);
            var velocity = targetDistance / m_info.leapTime;
            float time = 0;
            float animTime = 1 / (m_info.leapTime / 0.5f);
            yield return new WaitForSeconds(.5f);
            m_animation.animationState.TimeScale = animTime;
            m_legCollider.SetActive(false);
            m_shockwaveBB.enabled = true;
            while (time < m_info.leapTime)
            {
                m_character.physics.SetVelocity(velocity * transform.localScale.x, 0);
                time += Time.deltaTime;
                yield return null;
            }
            StartCoroutine(SpawnShockwaveRoutine());
            m_animation.animationState.TimeScale = 1;
            m_legCollider.SetActive(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                m_movement.Stop();

            yield return null;
        }

        private IEnumerator SpawnShockwaveRoutine()
        {
            var mainFX = m_shockwaveFX.main;
            mainFX.startLifetime = m_info.shockwaveSpeed;
            m_shockwaveFX.Play();
            var shockwaveSpeed = 15 / m_info.shockwaveSpeed;
           
            float time = 0;
            while (time < m_info.shockwaveSpeed)
            {
                //m_shockwaveBB.radius += Time.deltaTime * shockwaveSpeed;
                time += Time.deltaTime;
                yield return null;
            }
            //m_shockwaveBB.radius = 1f;
            m_shockwaveBB.enabled = false;
            yield return null;
        }
        
        private IEnumerator InnardsThrowColliderRoutine()
        {
            m_innardSlashBB.enabled = true;
            yield return new WaitForSeconds(1.5f);
            m_innardSlashBB.enabled = false;
        }

       private IEnumerator GroundPoundBBColliderRoutine()
        {
            m_groundPoundBB.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_groundPoundBB.SetActive(false);
            m_groundPoundColliderRoutine = null;
        }

        private void OnGroundPoundSpawn()
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.groundPoundProjectile, gameObject.scene);
            Vector2 spawnPosition = new Vector2(m_groundPoundPoint.transform.position.x, m_groundPoundPoint.transform.position.y);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            if (m_groundPoundColliderRoutine == null) 
            {
                m_groundPoundColliderRoutine = StartCoroutine(GroundPoundBBColliderRoutine());
            }
            else
            {
                m_groundPoundColliderRoutine = null;
                m_groundPoundColliderRoutine = StartCoroutine(GroundPoundBBColliderRoutine());
            }
        }

        private void OnInnardsThrowEvent()
        {
            if (m_innardsColliderRoutine == null)
            {
                m_innardsColliderRoutine = StartCoroutine(InnardsThrowColliderRoutine());
            }
            else
            {
                m_innardsColliderRoutine = null;
                m_innardsColliderRoutine = StartCoroutine(InnardsThrowColliderRoutine());
            }
        }

       

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
            m_startPoint = transform.position;
            m_spineEventListener.Subscribe(m_info.groundPoundEvent, OnGroundPoundSpawn);
            m_spineEventListener.Subscribe(m_info.innardsThrowEvent, OnInnardsThrowEvent);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + Vector2.Distance(m_targetInfo.position, transform.position));
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                        m_movement.Stop();

                    m_flinchHandle.m_autoFlinch = false;
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                            m_movement.Stop();

                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    switch (m_currentAttack)
                    {
                        case Attack.Swipe:
                            m_animation.EnableRootMotion(false, false);
                            m_attackHandle.ExecuteAttack(m_info.swipeAttack.animation, m_info.idleAnimation);
                            m_currentAttackCD = m_info.swipeAttackCD;
                            StartCoroutine(ThrowGutsRoutine());
                            break;
                        case Attack.Leap:
                            m_animation.EnableRootMotion(false, false);
                            m_attackHandle.ExecuteAttack(m_info.leapAttack.animation, m_info.idleAnimation);
                            m_currentAttackCD = m_info.leapAttackCD;
                            StartCoroutine(LeapAttackVelocityRoutine(m_targetInfo.position));
                            break;
                        case Attack.GroundPound:
                            m_animation.EnableRootMotion(false, false);
                            m_attackHandle.ExecuteAttack(m_info.groundPoundAttack.animation, m_info.idleAnimation);
                            m_currentAttackCD = m_info.groundPoundAttackCD;
                            break;
                    }

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }

                    if (m_currentCD <= m_currentAttackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_selfCollider.SetActive(true);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    {
                        m_flinchHandle.m_autoFlinch = false;
                       
                        if (Vector2.Distance(m_targetInfo.position, transform.position) < m_info.swipeAttack.range * 2)
                        {
                            m_currentAttack = Attack.Swipe;
                            m_currentAttackRange = m_info.swipeAttack.range;
                        }
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.leapAttack.range)
                        {
                            m_currentAttack = Attack.Leap;
                            m_currentAttackRange = m_info.leapAttack.range;
                        }
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.groundPoundAttack.range)
                        {
                            m_currentAttack = Attack.GroundPound;
                            m_currentAttackRange = m_info.groundPoundAttack.range;
                        }
                        if (IsFacingTarget())
                        {
                            if (IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                                    m_movement.Stop();

                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_animation.EnableRootMotion(false, false);
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                    //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    Debug.Log("IDLE CHASING");
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                                        m_movement.Stop();

                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                                m_stateHandle.SetState(State.Turning);
                        }
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
                //StartCoroutine(PatienceRoutine());
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(false);
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
