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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/AngelStatue3")]
    public class AngelStatue3AI : CombatAIBrain<AngelStatue3AI.Info>, IResetableAIBrain, IBattleZoneAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_backMove = new MovementInfo();
            public MovementInfo backMove => m_backMove;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField]
            private SimpleAttackInfo m_windBladeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo windBladeAttack => m_windBladeAttack;
            [SerializeField]
            private SimpleAttackInfo m_swordCleaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo swordCleaveAttack => m_swordCleaveAttack;
            [SerializeField]
            private SimpleAttackInfo m_stabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo stabAttack => m_stabAttack;
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
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_platformAnimation;
            public BasicAnimationInfo platformAnimation => m_platformAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wingBlockPushAnimaton;
            public BasicAnimationInfo wingBlockPushAnimaton => m_wingBlockPushAnimaton;
            [SerializeField]
            private BasicAnimationInfo m_wingShieldAnimaton;
            public BasicAnimationInfo wingShieldAnimaton => m_wingShieldAnimaton;
            [SerializeField]
            private BasicAnimationInfo m_wingShieldHitAnimation;
            public BasicAnimationInfo wingShieldHitAnimation => m_wingShieldHitAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wingShieldToIdleAnimation;
            public BasicAnimationInfo wingShieldToIdleAnimation => m_wingShieldToIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wingShieldAnimatonIdle;
            public BasicAnimationInfo wingShieldAnimatonIdle => m_wingShieldAnimatonIdle;
           

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_attackSlashEvent;
            public string attackSlashEvent => m_attackSlashEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_attackStabEvent;
            public string attackStabEvent => m_attackStabEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_windProjectileEvent;
            public string windProjectileEvent => m_windProjectileEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordCleaveEvent;
            public string swordCleaveEvent => m_swordCleaveEvent;
            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_backMove.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_windBladeAttack.SetData(m_skeletonDataAsset);
                m_stabAttack.SetData(m_skeletonDataAsset);
                m_swordCleaveAttack.SetData(m_skeletonDataAsset);
                m_wingBlockPushAnimaton.SetData(m_skeletonDataAsset);
                m_wingShieldAnimaton.SetData(m_skeletonDataAsset);
                m_wingShieldAnimatonIdle.SetData(m_skeletonDataAsset);
                m_wingShieldHitAnimation.SetData(m_skeletonDataAsset);
                m_wingShieldToIdleAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_platformAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            ReturnToSpawnPoint,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            SlashStabComboAttack,
            SwordCleaveAttack,
            WingShieldAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bgStatue;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_boundBoxGO;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

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
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private bool m_isDetectedOnce;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        
        [SerializeField]
        private float m_healthPercentageWingShieldActivate;

        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

       // private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_stateHandle.OverrideState(State.Detect);
                    m_isDetecting = true;
                   
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
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.ReturnToSpawnPoint);
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
            StopAllCoroutines();
            m_selfCollider.SetActive(false);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            m_boundBoxGO.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //StopAllCoroutines();
            ////m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            //    m_animation.SetEmptyAnimation(0, 0);
            ////m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        [SerializeField]
        private Attacker m_swordAttacker;
        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Idle);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.SlashStabComboAttack, m_info.attack.range),
                                    new AttackInfo<Attack>(Attack.SwordCleaveAttack, m_info.swordCleaveAttack.range), 
                                    new AttackInfo<Attack>(Attack.WingShieldAttack, m_info.swordCleaveAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_bgStatue.transform.SetParent(null);
           
            if (!m_isDetectedOnce)
            {
                m_isDetectedOnce = true;
                m_animation.SetAnimation(0, m_info.detectAnimation, false);
                m_animation.animationState.TimeScale = 1;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
                m_hitbox.Enable();
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
        [SerializeField]
        private GameObject m_origWeapon;
        [SerializeField]
        private GameObject m_swordCleave;

        [SerializeField]
        private SpineEventListener m_spineListener;

        private IEnumerator SwordCleaveRoutine()
        {

            m_animation.SetAnimation(0, m_info.swordCleaveAttack, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordCleaveAttack);
            
    
        }

        private IEnumerator ColliderAttackSlashRoutine()
        {
            m_origWeapon.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            m_origWeapon.SetActive(false);
        }

        private IEnumerator ColliderAttackCleaveRoutine()
        {
            m_swordCleave.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            m_swordCleave.SetActive(false);
        }

        private void ColliderForAttackEvent()
        {
            StartCoroutine(ColliderAttackSlashRoutine());
        }
        private void ColliderForCleaveEvent()
        {
            StartCoroutine(ColliderAttackCleaveRoutine());
        }
        private IEnumerator SwordCleaveAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return SwordCleaveRoutine();
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator AttackSlashRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
           
        }
        private IEnumerator AttackStabRoutine()
        {
            m_animation.SetAnimation(0, m_info.stabAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.stabAttack.animation);
            
        }
        private IEnumerator WingPushRoutine()
        {
            m_animation.SetAnimation(0, m_info.wingShieldAnimaton.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wingShieldAnimaton.animation);
            m_animation.SetAnimation(0, m_info.wingShieldAnimatonIdle, false);
            yield return new WaitForSeconds(3f);
            m_animation.SetAnimation(0, m_info.wingShieldToIdleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wingShieldToIdleAnimation.animation);
        }
        private IEnumerator SlashStabComboAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return AttackSlashRoutine();
            yield return AttackStabRoutine();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator WingPushBehaviour()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            //m_flinchHandle.SetAnimation(m_info.wingShieldHitAnimation.animation);
            yield return WingPushRoutine();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }
        private IEnumerator ReturnToStartingPoint()
        {
            m_stateHandle.Wait(State.Idle);
            var direction = (int)m_character.facing * Vector2.right;
            if(!IsFacing(m_startPoint))
            {
                CustomTurn();
            }
            do
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                Debug.Log("Return to spot");
                yield return null;

            } while (Vector2.Distance(transform.position, m_startPoint) > 1f);
            Debug.Log("outside loop");
            m_stateHandle.ApplyQueuedState();
        }
        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.attackStabEvent, ColliderForAttackEvent);
            m_spineListener.Subscribe(m_info.attackSlashEvent, ColliderForAttackEvent);
            m_spineListener.Subscribe(m_info.swordCleaveEvent, ColliderForCleaveEvent);
            m_selfCollider.SetActive(false);
            m_startPoint = transform.position;
        }

        private double CalculatePercentage(int currentValue, int maxValue)
        {
            double percentage = ((double)currentValue / maxValue) * 100;
            return Math.Round(percentage, 2);
        }
        protected override void Awake()
        {
            base.Awake();
            m_hitbox.Disable();
            //m_patrolHandle.TurnRequest += OnTurnRequest;
            //m_attackHandle.AttackDone += OnAttackDone;
            //m_turnHandle.TurnDone += OnTurnDone;

            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_movement.Stop();
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Idle:
                    m_movement.Stop();
                    if (!m_isDetectedOnce)
                    {
                        m_animation.SetAnimation(0, m_info.detectAnimation, true).AnimationStart = 0.15f;
                        m_animation.animationState.TimeScale = 0;
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }    
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.detectAnimation)
                    //{
                    //}
                    break;
                case State.ReturnToSpawnPoint:
                    StopAllCoroutines();

                    StartCoroutine(ReturnToStartingPoint());
                    break;
                //case State.Turning:
                //    m_stateHandle.Wait(m_turnState);
                //    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                //    break;

                case State.Attacking:  
                    var healthPercentage = CalculatePercentage(m_damageable.health.currentValue, m_damageable.health.maxValue);
                    Debug.Log(healthPercentage);
                    StopAllCoroutines();
                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();
                    }
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.SlashStabComboAttack:
                            //m_animation.EnableRootMotion(true, false);
                            //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation.animation);
                           StartCoroutine(SlashStabComboAttack());
                            break;
                        case Attack.SwordCleaveAttack:
                            StartCoroutine(SwordCleaveAttack());
                          
                            break;
                        case Attack.WingShieldAttack:
                            if (healthPercentage < m_healthPercentageWingShieldActivate)
                            {
                                StartCoroutine(WingPushBehaviour());
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_stateHandle.SetState(State.ReevaluateSituation);
                            }
                            break;
                    }
                


                    break;

                //case State.Cooldown:
                //    m_stateHandle.Wait(State.ReevaluateSituation);
                //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                //    if (IsTargetInRange(100))
                //    {
                //        if (!IsFacingTarget())
                //        {
                //            m_turnState = State.Cooldown;
                //            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                //                m_stateHandle.SetState(State.Turning);
                //            CustomTurn();
                //        }
                //        else
                //        {
                //            m_animation.EnableRootMotion(true, false);
                //            m_animation.SetAnimation(0, m_info.backMove.animation, true);
                //            m_movement.MoveTowards(Vector2.one * -transform.localScale.x, m_info.backMove.speed);
                //        }
                //    }
                //    else
                //    {
                //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //    }

                //    if (m_currentCD <= m_info.attackCD)
                //    {
                //        m_currentCD += Time.deltaTime;
                //    }
                //    else
                //    {
                //        m_currentCD = 0;
                //        m_stateHandle.OverrideState(State.ReevaluateSituation);
                //    }

                    //break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_animation.EnableRootMotion(true, false);
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_movement.Stop();
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            //m_turnState = State.ReevaluateSituation;
                            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            //    m_stateHandle.SetState(State.Turning);
                            CustomTurn();
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
                        m_stateHandle.SetState(State.Idle);
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
            m_stateHandle.OverrideState(State.Idle);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_boundBoxGO.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(true);
        }

        public void SwitchToBattleZoneAI()
        {
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
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
    }
}
