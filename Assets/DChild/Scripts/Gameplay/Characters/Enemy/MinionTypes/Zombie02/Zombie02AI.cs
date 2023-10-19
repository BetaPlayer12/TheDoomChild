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
using Spine.Unity.Examples;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Zombie02")]
    public class Zombie02AI : CombatAIBrain<Zombie02AI.Info>, IResetableAIBrain, IBattleZoneAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;
            [SerializeField]
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;

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
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_counterFlinchAnimation;
            public BasicAnimationInfo counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_spawn1Animation;
            public BasicAnimationInfo spawn1Animation => m_spawn1Animation;
            [SerializeField]
            private BasicAnimationInfo m_spawn2Animation;
            public BasicAnimationInfo spawn2Animation => m_spawn2Animation;
            [SerializeField]
            private BasicAnimationInfo m_spawn3Animation;
            public BasicAnimationInfo spawn3Animation => m_spawn3Animation;
            [SerializeField]
            private BasicAnimationInfo m_vomitAnimation;
            public BasicAnimationInfo vomitAnimation => m_vomitAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_counterFlinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_spawn1Animation.SetData(m_skeletonDataAsset);
                m_spawn2Animation.SetData(m_skeletonDataAsset);
                m_spawn3Animation.SetData(m_skeletonDataAsset);
                m_vomitAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Spawning,
            Detect,
            Patrol,
            Idle,
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
            Attack1,
            Attack2,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
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

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentMoveSpeed;
        private float m_currentRunAnticDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private bool m_battlezonemode;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;


        [SerializeField, TabGroup("Cannon Values")]
        private Transform m_projectilePoint;
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
        private Vector2 m_targetLastPos;

        [SerializeField]
        private BoneLocalOverride m_targetPointIK;

        [SerializeField]
        private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_targetPointIK.overridePosition = false;
            m_targetPointIK.localPosition = Vector2.zero;
            m_flinchHandle.m_autoFlinch = true;
            m_character.physics.UseStepClimb(true);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.enabled = false;
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

        public void SetAI(AITargetInfo targetInfo)
        {
            m_isDetecting = true;
            m_targetInfo = targetInfo;
            m_stateHandle.OverrideState(State.Spawning);
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
                m_selfCollider.enabled = false;
                m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }
        //private IEnumerator PatienceRoutine()
        //{
        //    //if (m_enablePatience)
        //    //{
        //    //    while (m_currentPatience < m_info.patience)
        //    //    {
        //    //        m_currentPatience += m_character.isolatedObject.deltaTime;
        //    //        yield return null;
        //    //    }
        //    //}
        //    yield return new WaitForSeconds(m_info.patience);
        //    m_targetInfo.Set(null, null);
        //    m_isDetecting = false;
        //    m_enablePatience = false;
        //    m_stateHandle.SetState(State.Patrol);
        //}

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            if (!IsFacingTarget())
                CustomTurn();
            base.OnDestroyed(sender, eventArgs);
            
            m_targetPointIK.overridePosition = false;
            m_targetPointIK.localPosition = Vector2.zero;
            m_selfCollider.enabled = false;
            m_hitbox.Disable();
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            StopAllCoroutines();
            m_movement.Stop();
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitWhile(() => m_animation.animationState.GetCurrent(0).AnimationTime < (m_animation.animationState.GetCurrent(0).AnimationEnd * 0.2f));
            m_legCollider.enabled = false;
            m_bodyCollider.enabled = false;
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                m_selfCollider.enabled = false;
                m_currentCD += m_currentCD + 0.5f;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
                    m_animation.SetAnimation(0, m_info.idleAnimation, false);
                m_stateHandle.ApplyQueuedState();
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),*/
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator SpawnRoutine()
        {
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            var spawn = UnityEngine.Random.Range(0, 2) == 0 ? m_info.spawn1Animation : m_info.spawn2Animation;
            m_animation.SetAnimation(0, spawn, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, spawn);
            m_animation.SetAnimation(0, m_info.vomitAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.vomitAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        //private IEnumerator Attack2Routine()
        //{
        //    m_targetLastPos = m_targetInfo.position;
        //    m_animation.SetAnimation(0, m_info.attack2.animation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        private Vector2 BallisticVel()
        {
            Vector2 targetCenterMass = m_targetLastPos;
            m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;

            m_targetDistance = Vector2.Distance(targetCenterMass, m_projectilePoint.position);
            var dir = (targetCenterMass - new Vector2(m_projectilePoint.position.x, m_projectilePoint.position.y));
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;

            var currentSpeed = m_speed;

            var vel = Mathf.Sqrt(dist * m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale);
            return (vel * new Vector3(dir.x * m_posOffset.x, dir.y * m_posOffset.y).normalized) * m_targetOffset.sqrMagnitude; //closest to accurate
        }

        private float GroundDistance()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            if (hit.collider != null)
            {
                return hit.distance;
            }

            return 0;
        }

        private void SpitProjectile()
        {
            if (!ShotBlocked())
            {
                //if (!IsFacingTarget())
                //{
                //    CustomTurn();
                //}
                //Dirt FX
                //GameObject obj = Instantiate(m_info.mouthSpitFX, m_seedSpitTF.position, Quaternion.identity);
                //obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                //obj.transform.parent = m_seedSpitTF;
                //obj.transform.localPosition = new Vector2(4, -1.5f);
                //


                //Shoot Spit
                //m_muzzleFX.Play();
                //m_targetPointIK.transform.position = new Vector2(m_targetLastPos.x, m_targetLastPos.y + (Vector2.Distance(transform.position, m_targetInfo.position) / 5f));
                var overrideTarget = new Vector2(m_targetLastPos.x, m_targetLastPos.y + (Vector2.Distance(transform.position, m_targetInfo.position) / 5f));
                overrideTarget = new Vector2(Mathf.Abs(m_targetLastPos.x), Mathf.Abs(m_targetLastPos.y));
                overrideTarget = new Vector2(Mathf.Abs(overrideTarget.x - Mathf.Abs(m_character.centerMass.position.x)), Mathf.Abs(overrideTarget.y - Mathf.Abs(m_character.centerMass.position.y)));
                m_targetPointIK.localPosition = overrideTarget;
                Vector2 target = m_targetLastPos;
                target = new Vector2(target.x, target.y - 2);
                Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_projectilePoint.position.x - 1.5f : m_projectilePoint.position.x + 1.5f, m_projectilePoint.position.y - 0.75f);
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.projectile.projectileInfo.projectile);
                instance.transform.position = m_projectilePoint.position;
                var component = instance.GetComponent<Projectile>();
                component.ResetState();
                //component.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
                component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVel());
                //return instance.gameObject;
            }
        }
        private bool ShotBlocked()
        {
            Vector2 wat = m_projectilePoint.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
#if UNITY_EDITOR
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
#endif
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }
        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        protected override void Start()
        {
            base.Start();
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, SpitProjectile);
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.run.speed * .75f, m_info.run.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Spawning:
                    m_movement.Stop();
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(SpawnRoutine());
                    break;

                case State.Detect:
                    m_flinchHandle.m_autoFlinch = false;
                    m_movement.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.walk.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.walk.speed, characterInfo);
                        //if (m_groundSensor.allRaysDetecting)
                        //{
                        //    transform.position = new Vector2(transform.position.x, GroundPosition().y + 0.35f);
                        //}
                    }
                    else
                    {
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        {
                            m_movement.Stop();
                        }
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_character.physics.UseStepClimb(false);


                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                            m_animation.EnableRootMotion(true, true);
                            m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.Attack2:
                            var lastTargetPos = m_targetInfo.position;
                            m_targetLastPos = lastTargetPos;
                            m_targetPointIK.overridePosition = true;
                            m_animation.EnableRootMotion(true, true);
                            m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation.animation);
                            //StartCoroutine(Attack2Routine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    
                    if (m_currentCD <= m_currentFullCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        if (IsFacingTarget())
                        {
                            m_currentCD = 0;
                            m_stateHandle.OverrideState(State.ReevaluateSituation);
                        }
                    }

                    break;
                case State.Chasing:
                    {
                        m_flinchHandle.m_autoFlinch = false;
                        var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && /*IsTargetInRange(m_attackDecider.chosenAttack.range)*/Mathf.Abs(m_targetInfo.position.x - transform.position.x) <= m_attackDecider.chosenAttack.range && !m_wallSensor.allRaysDetecting)
                            {
                                m_movement.Stop();
                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting && Mathf.Abs(m_targetInfo.position.x - transform.position.x) > m_info.attack1.range -1)
                                {
                                    var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                                    m_animation.EnableRootMotion(false, false);
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.run.animation : m_info.walk.animation, true);
                                    m_character.physics.SetVelocity(toTarget.normalized.x * (distance >= m_info.targetDistanceTolerance ? m_currentMoveSpeed : m_info.walk.speed), m_character.physics.velocity.y);
                                    //if (m_groundSensor.allRaysDetecting)
                                    //{
                                    //    transform.position = new Vector2(transform.position.x, (GroundPosition().y + 0.35f) - m_legCollider.offset.y);
                                    //}
                                }
                                else
                                {
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    {
                                        m_movement.Stop();
                                    }
                                    m_animation.EnableRootMotion(true, m_groundSensor.isDetecting ? true : false);
                                    m_selfCollider.enabled = true;
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
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

            if (m_enablePatience && !m_battlezonemode)
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
            m_selfCollider.enabled = false;
            m_legCollider.enabled = true;
            m_bodyCollider.enabled = true;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_legCollider.enabled = true;
            m_bodyCollider.enabled = true;
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
            m_hitbox.Enable();
        }

        public void SwitchToBattleZoneAI()
        {
            m_battlezonemode = true;
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_battlezonemode = false;
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void HandleKnockback(float resumeAIDelay)
        {
            StopAllCoroutines();
            m_stateHandle.Wait(State.ReevaluateSituation);
            StartCoroutine(KnockbackRoutine(resumeAIDelay));
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.DisableRootMotion();
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
