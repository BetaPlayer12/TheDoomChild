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
    public class SlugAI : CombatAIBrain<SlugAI.Info>
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_damageAnimation;
            public string damageAnimation => m_damageAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

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
        private GameObject m_selfCollider;
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

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);

            //m_spineEventListener.Subscribe(m_info.spitAttackEvent, SpitProjectile);
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, SpitProjectile);
            m_spineEventListener.Subscribe(m_info.spikesEvent, SpikeFX);
        }

        private Vector2 BallisticVel()
        {
            Vector2 targetCenterMass = m_targetInfo.transform.GetComponent<Character>().centerMass.position;
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
            RaycastHit2D hit = Physics2D.Raycast(m_throwPoint.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            if (hit.collider != null)
            {
                return hit.distance;
            }

            return 0;
        }

        private void SpitProjectile()
        {
            if (m_targetInfo.isValid)
            {
                if (IsFacingTarget())
                {
                    //Dirt FX
                    //GameObject obj = Instantiate(m_info.mouthSpitFX, m_seedSpitTF.position, Quaternion.identity);
                    //obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                    //obj.transform.parent = m_seedSpitTF;
                    //obj.transform.localPosition = new Vector2(4, -1.5f);
                    //


                    //Shoot Spit
                    m_muzzleFX.Play();
                    Vector2 target = m_targetInfo.transform.GetComponent<Character>().centerMass.position;
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
                    component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVel());
                    //return instance.gameObject;
                }
                else
                {
                    m_stateHandle.OverrideState(State.Turning);
                }
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
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.SetActive(true);
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
                m_selfCollider.SetActive(false);
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
            m_movement.Stop();
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(2f);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SlugProjectileRoutine()
        {
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.spitAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spitAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetEmptyAnimation(0, 0);
                m_stateHandle.OverrideState(State.ReevaluateSituation);
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
            m_deathHandle.SetAnimation(m_info.deathAnimation);
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, transform.localRotation.z != 0 ? true : false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true).TimeScale = 1f;
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
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
                    m_flinchHandle.m_autoFlinch = false;
                    m_animation.animationState.GetCurrent(0).MixDuration = 0;

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Spike:
                            m_animation.EnableRootMotion(false, false);
                            m_attackHandle.ExecuteAttack(m_info.spikeAttack.animation, m_info.idleAnimation);
                            break;
                        case Attack.Spit:
                            m_animation.EnableRootMotion(false, false);
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

                    if (m_currentCD <= m_info.attackCD)
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
                                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting /*&& m_edgeSensor.isDetecting*/)
                                {
                                    m_animation.EnableRootMotion(true, false);
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 2f;
                                }
                                else
                                {
                                    m_attackDecider.hasDecidedOnAttack = false;
                                    m_movement.Stop();
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
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }
    }
}
