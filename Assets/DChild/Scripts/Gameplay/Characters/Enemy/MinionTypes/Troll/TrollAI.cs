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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Troll")]
    public class TrollAI : CombatAIBrain<TrollAI.Info>
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
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_poundAttack = new SimpleAttackInfo();
            public SimpleAttackInfo poundAttack => m_poundAttack;
            [SerializeField]
            private SimpleAttackInfo m_punchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo punchAttack => m_punchAttack;
            [SerializeField]
            private SimpleAttackInfo m_oraOraAttack = new SimpleAttackInfo();
            public SimpleAttackInfo oraOraAttack => m_oraOraAttack;
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
            private BasicAnimationInfo m_detectAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchBackwardsAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchBackwardsAnimation => m_flinchBackwardsAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_rootShakeAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo rootShakeAnimation => m_rootShakeAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dirAttackEvent;
            public string dirAttackEvent => m_dirAttackEvent;
            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_oraOraEvent;
            public string oraOraEvent => m_oraOraEvent;
            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_poundEvent;
            public string poundEvent => m_poundEvent;

            [SerializeField]
            private GameObject m_dirtProjectile;
            public GameObject dirtProjectile => m_dirtProjectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_poundAttack.SetData(m_skeletonDataAsset);
                m_punchAttack.SetData(m_skeletonDataAsset);
                m_oraOraAttack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinchBackwardsAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_rootShakeAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Pound,
            Punch,
            OraOra,
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
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_poundFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_oraFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_rockThrowFX;
        [SerializeField, TabGroup("AttackHitbox")]
        private GameObject m_attackHitbox;
        [SerializeField, TabGroup("AttackHitbox")]
        private List<GameObject> m_fistHitboxes;

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
        private float m_currentMoveSpeed;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        private bool m_isDetecting;

        [SerializeField]
        private Transform m_throwPoint;

        private Vector2 m_startPoint;

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.run.speed * .75f, m_info.run.speed * 1.25f);

            m_spineEventListener.Subscribe(m_info.dirAttackEvent, DirtProjectile);
            m_spineEventListener.Subscribe(m_info.poundEvent, MeleeAttack/*m_poundFX.Play*/);
            m_spineEventListener.Subscribe(m_info.oraOraEvent, MeleeAttack/*m_oraFX.Play*/);
            //GameplaySystem.SetBossHealth(m_character);
            m_startPoint = transform.position;
        }

        private void MeleeAttack()
        {
            StartCoroutine(MeleeAttackRoutine());
        }

        private IEnumerator MeleeAttackRoutine()
        {
            m_attackHitbox.SetActive(true);
            m_poundFX.Play();
            yield return new WaitForSeconds(.25f);
            m_attackHitbox.SetActive(false);
            yield return null;
        }

        private Vector2 BallisticVel()
        {
            m_info.dirtProjectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;

            m_targetDistance = Vector2.Distance(m_targetInfo.position, transform.position);
            var dir = (m_targetInfo.position - new Vector2(transform.position.x, transform.position.y));
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;

            var currentSpeed = m_speed;

            var vel = Mathf.Sqrt(dist * m_info.dirtProjectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale);
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

        private void DirtProjectile()
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

                    m_rockThrowFX.Play();
                    //Shoot Spit
                    var target = m_targetInfo.position;
                    target = new Vector2(target.x, target.y - 2);
                    Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_throwPoint.position.x - 1.5f : m_throwPoint.position.x + 1.5f, m_throwPoint.position.y - 0.75f);
                    Vector3 v_diff = (target - spitPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

                    //GameObject projectile = Instantiate(m_info.dirtProjectile, spitPos, Quaternion.identity);
                    //projectile.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);

                    GameObject projectile = m_info.dirtProjectile;
                    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                    instance.transform.position = m_throwPoint.position;
                    var component = instance.GetComponent<Projectile>();
                    component.ResetState();
                    //component.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
                    component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVel());
                }
                else
                {
                    m_turnState = State.ReevaluateSituation;
                    m_stateHandle.OverrideState(State.Turning);
                }
            }
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
            for (int i = 0; i < m_fistHitboxes.Count; i++)
                m_fistHitboxes[i].GetComponent<BoxCollider2D>().enabled = false;
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.enabled = false;
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                //StopCoroutine(PatienceRoutine());
                m_enablePatience = false;
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
                m_enablePatience = false;
                m_isDetecting = false;
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
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            m_selfCollider.enabled = false;
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //StartCoroutine(FlinchShakeRoutine());
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private IEnumerator FlinchShakeRoutine()
        {
            m_animation.SetEmptyAnimation(2, 0);
            m_animation.AddAnimation(2, m_info.rootShakeAnimation.animation, true, 0);
            //m_animation.AddAnimation(1, m_info.flinchAnimation, true, 0);
            m_animation.animationState.GetCurrent(2).TimeScale = 3;
            //m_animation.AddEmptyAnimation(1, 2.5f, 3);
            yield return new WaitForSeconds(.25f);
            m_animation.SetEmptyAnimation(2, 0);
            yield return null;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Pound, m_info.poundAttack.range),
                                    new AttackInfo<Attack>(Attack.Punch, m_info.punchAttack.range),
                                    new AttackInfo<Attack>(Attack.OraOra, m_info.oraOraAttack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator PunchRoutine()
        {
            //m_attackHandle.ExecuteAttack(m_info.punchAttack.animation, m_info.idleAnimation.animation);
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.punchAttack, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchAttack);
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            yield return null;
            m_animation.DisableRootMotion();          
            m_stateHandle.ApplyQueuedState();
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
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_animation.SetEmptyAnimation(0, 0);
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        //m_animation.DisableRootMotion();
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
                    m_groundSensor.Cast();
                    m_wallSensor.Cast();
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        //m_animation.EnableRootMotion(true, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
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
                    m_movement.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    m_animation.animationState.GetCurrent(0).MixDuration = 0;
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    
                    //for (int i = 0; i < m_fistHitboxes.Count; i++)
                    //    m_fistHitboxes[i].SetActive(true);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Pound:
                            //m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.poundAttack.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.Punch:
                            m_wallSensor.Cast();
                            if (!m_wallSensor.isDetecting)
                            {
                                //m_animation.EnableRootMotion(true, false);
                                //m_attackHandle.ExecuteAttack(m_info.punchAttack.animation, m_info.idleAnimation.animation);
                                StartCoroutine(PunchRoutine());
                            }
                            else
                            {
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.OraOra:
                            //m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.oraOraAttack.animation, m_info.idleAnimation.animation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            m_wallSensor.Cast();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
                                m_movement.Stop();
                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_groundSensor.Cast();
                                m_edgeSensor.Cast();
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
                                    //m_animation.EnableRootMotion(false, false);
                                    m_animation.SetAnimation(0, m_info.run.animation, true);
                                    //m_movement.MoveTowards(m_targetInfo.position, m_info.run.speed * transform.localScale.x);
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
                                }
                                else
                                {
                                    GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
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
                        //m_animation.SetAnimation(0, m_info.detectAnimation, true);
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
            m_stateHandle.OverrideState(State.Patrol);
        }
    }
}
