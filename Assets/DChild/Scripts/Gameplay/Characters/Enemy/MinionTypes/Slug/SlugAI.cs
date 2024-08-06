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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Slug")]
    public class SlugAI : CombatAIBrain<SlugAI.Info>, IResetableAIBrain, IKnockbackable
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
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private SimpleAttackInfo m_spikeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spikeAttack => m_spikeAttack;
            [SerializeField]
            private SimpleAttackInfo m_spitAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spitAttack => m_spitAttack;
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
            private BasicAnimationInfo m_damageAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo damageAnimation => m_damageAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_spikesEvent;
            public string spikesEvent => m_spikesEvent;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;


            //[SerializeField]
            //private GameObject m_spitProjectile;
            //public GameObject spitProjectile => m_spitProjectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_spikeAttack.SetData(m_skeletonDataAsset);
                m_spitAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_damageAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Patrol,
            Detect,
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
            Spike,
            Spit,
            Slap,
            [HideInInspector]
            _COUNT
        }

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

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private IsolatedCharacterPhysics2D m_charcterPhysics;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_muzzleFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_spikeFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_glow;

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

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private ProjectileLauncher m_projectileLauncher;

        [SerializeField]
        private Transform m_throwPoint;
        [SerializeField]
        private GameObject m_spikeDamage;

        private bool m_isDetecting;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private Vector2 m_targetLastPos;
        private Vector2 m_startPoint;

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            //m_spineEventListener.Subscribe(m_info.spitAttackEvent, SpitProjectile);
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, SpitProjectile);
            m_spineEventListener.Subscribe(m_info.spikesEvent, SpikeFX);
            m_startPoint = transform.position;
        }

        private Vector2 BallisticVel()
        {
            Vector2 targetCenterMass = m_targetLastPos;
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

        private float GroundDistance()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_throwPoint.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            if (hit.collider != null)
            {
                return hit.distance;
            }

            return 0;
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private void SpitProjectile()
        {
            if (m_targetInfo.isValid)
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
                m_muzzleFX.Play();
                Vector2 target = m_targetLastPos;
                target = new Vector2(target.x, target.y - 2);
                Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_throwPoint.position.x - 1.5f : m_throwPoint.position.x + 1.5f, m_throwPoint.position.y - 0.75f);
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.projectile.projectileInfo.projectile);
                instance.transform.position = m_throwPoint.position;
                var component = instance.GetComponent<Projectile>();
                component.ResetState();
                //component.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
                component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVel());
                //return instance.gameObject;
            }
        }

        private void SpikeFX()
        {
            m_spikeFX.Play();
            m_spikeDamage.SetActive(true);
            //StartCoroutine(SpikeRoutine());
        }

        //private IEnumerator SpikeRoutine()
        //{
        //    m_spikeFX.Play();
        //    m_spikeDamage.SetActive(true);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeAttack.animation);
        //    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_spikeDamage.SetActive(false);
        //    yield return null;
        //}

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.enabled = false;
                m_currentPatience = 0;
                m_enablePatience = false;
                //StopCoroutine(PatienceRoutine()); //for latur
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                if (transform.localRotation.z != 0)
                {
                    transform.localRotation = Quaternion.Euler(Vector3.zero);
                    GetComponent<IsolatedPhysics2D>().simulateGravity = true;
                    m_animation.DisableRootMotion();
                }
            }
            else
            {
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
        //    yield return new WaitForSeconds(m_info.patience);
        //    m_targetInfo.Set(null, null);
        //    m_isDetecting = false;
        //    m_stateHandle.SetState(State.Patrol);
        //}

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            
            //m_glow.SetActive(false);
            m_movement.Stop();
            m_selfCollider.enabled = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.idleAnimation, true)/*.MixDuration = 0*/;
            yield return new WaitForSeconds(2f);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SlugProjectileRoutine()
        {
            m_movement.Stop();
            m_targetLastPos = m_targetInfo.transform.GetComponent<Character>().centerMass.position;
            m_animation.SetAnimation(0, m_info.spitAttack.animation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spitAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.m_autoFlinch = true;
            yield return new WaitForSeconds(2f);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.damageAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
                    m_animation.SetEmptyAnimation(0, 0);
                m_selfCollider.enabled = false;
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
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Spike, m_info.spikeAttack.range),*/
                                    new AttackInfo<Attack>(Attack.Spit, m_info.spitAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            //m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_throwPoint);
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Ground Sensor is " + m_groundSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
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
                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        //m_animation.EnableRootMotion(true, transform.localRotation.z != 0 ? true : false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true).TimeScale = 1f;
                        m_animation.animationState.GetCurrent(0).MixDuration = 0;
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        //m_animation.DisableRootMotion();
                        m_animation.SetAnimation(0, m_info.idleAnimation.animation, true).MixDuration = 0;
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_movement.Stop();
                    //m_animation.DisableRootMotion();
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_flinchHandle.m_autoFlinch = false;

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Spike:
                            //m_animation.EnableRootMotion(false, false);
                            m_attackHandle.ExecuteAttack(m_info.spikeAttack.animation, m_info.idleAnimation.animation);
                            m_animation.animationState.GetCurrent(0).MixDuration = 0;
                            break;
                        case Attack.Spit:
                            //m_animation.EnableRootMotion(true, false);
                            //m_attackHandle.ExecuteAttack(m_info.spitAttack.animation, m_info.idleAnimation);
                            StartCoroutine(SlugProjectileRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }

                    if (m_currentCD <= m_currentFullCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_spikeDamage.SetActive(false);
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                m_movement.Stop();
                                m_selfCollider.enabled = true;
                                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting /*&& m_edgeSensor.isDetecting*/)
                                {
                                    //m_animation.EnableRootMotion(true, false);
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = m_currentTimeScale;
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_attackDecider.hasDecidedOnAttack = false;
                                    m_movement.Stop();
                                    m_selfCollider.enabled = true;
                                    m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
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

            if (m_enablePatience)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            //m_glow.SetActive(true);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
            m_charcterPhysics.UseStepClimb(false);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.damageAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.damageAnimation.animation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            m_charcterPhysics.UseStepClimb(true);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
