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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/AngelStatue2")]
    public class AngelStatue2AI : CombatAIBrain<AngelStatue2AI.Info>, IResetableAIBrain, IBattleZoneAIBrain
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
            private BasicAnimationInfo m_brokeIdleAnimation;
            public BasicAnimationInfo brokeIdleAnimation => m_brokeIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_brokeIdle2Animation;
            public BasicAnimationInfo brokeIdle2Animation => m_brokeIdle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_brokeToIdleAnimation;
            public BasicAnimationInfo brokeToIdleAnimation => m_brokeToIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_stillAnimation;
            public BasicAnimationInfo stillAnimation => m_stillAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinch2Animation;
            public BasicAnimationInfo flinch2Animation => m_flinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;


            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_onColliderAttackSmash;
            public string onColliderAttackSmash => m_onColliderAttackSmash;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_offColliderAttackSmash;
            public string offColliderAttackSmash => m_offColliderAttackSmash;
            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_brokeIdleAnimation.SetData(m_skeletonDataAsset);
                m_brokeIdle2Animation.SetData(m_skeletonDataAsset);
                m_brokeToIdleAnimation.SetData(m_skeletonDataAsset);
                m_stillAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinch2Animation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
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
            ReturnToSpawnPoint,
            Chasing,
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
        private GameObject m_selfCollider;
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
        private Vector2 m_startpoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private GameObject m_bgStatue;
        [SerializeField]
        private CapsuleCollider2D m_stillLegCollider;
        [SerializeField]
        private CapsuleCollider2D m_LegCollider;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;
        private bool m_isDetectedOnce;
        private State m_turnState;

        private string m_brokeIdleAnimation;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        //private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

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

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
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
          //  StartCoroutine(FlinchRoutine());
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
        }

        private IEnumerator FlinchRoutine()
        {
            var flinch = m_info.flinchAnimation;
            if (m_targetInfo.isValid)
            {
                flinch = IsFacingTarget() ? m_info.flinchAnimation : m_info.flinch2Animation;
            }
            m_animation.SetAnimation(0, flinch, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, flinch);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            //m_hitbox.Disable();
            // var randomAnimation = UnityEngine.Random.Range(0, 2);
            //m_brokeIdleAnimation = randomAnimation == 0 ? m_info.brokeIdleAnimation.animation : m_info.brokeIdle2Animation.animation;
            if (!m_isDetectedOnce)
            {
                m_isDetectedOnce = true;
                m_animation.SetAnimation(0, m_info.brokeIdleAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.brokeIdleAnimation);
                m_stillLegCollider.enabled = false;
                m_LegCollider.enabled = true;
                m_bgStatue.SetActive(true);
                m_bgStatue.transform.SetParent(null);
                m_hitbox.Enable();
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                //m_animation.SetAnimation(0, m_info.brokeToIdleAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.brokeToIdleAnimation);
            } 
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ReturnToStartingPoint()
        {
            m_stateHandle.Wait(State.Idle);
            var direction = (int)m_character.facing * Vector2.right;
            if (!IsFacing(m_startpoint))
            {
                CustomTurn();
            }
            do
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                Debug.Log("Return to spot");
                yield return null;

            } while (Vector2.Distance(transform.position, m_startpoint) > 1f);
            Debug.Log("outside loop");
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator AttackTwoRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack2, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2);
            
        }
        private IEnumerator AttackTwo()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return AttackTwoRoutine();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator AttackOneRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack1, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1);

        }
        private IEnumerator AttackOne()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return AttackOneRoutine();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }
        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        m_currentAttackRange = m_attackRangeCache[i];
                        return;
                    }
                }
            }
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }

        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }

        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.onColliderAttackSmash,OnAttackCollider);
            m_spineListener.Subscribe(m_info.offColliderAttackSmash, OffAttackCollider);
            m_selfCollider.SetActive(false);
            m_animation.SetAnimation(0, m_info.stillAnimation, true);
            m_startpoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            m_hitbox.Disable();
           
      
           // m_flinchHandle.enabled = false;
            //m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            //m_attackCache = new List<Attack>();
            //AddToAttackCache(Attack.Attack1, Attack.Attack2);
            //m_attackRangeCache = new List<float>();
            //AddToRangeCache(m_info.attack1.range, m_info.attack2.range);
            //m_attackUsed = new bool[m_attackCache.Count];
        }

        [SerializeField]
        private GameObject m_attackCollider;
 
        public void OnAttackCollider()
        {
            m_attackCollider.SetActive(true);
        }

        public void OffAttackCollider()
        {
            m_attackCollider.SetActive(false);
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
                    //if (m_animation.GetCurrentAnimation(0).ToString() == m_info.stillAnimation.animation)
                    //{
                    //    m_stateHandle.Wait(State.ReevaluateSituation);
                    //    m_movement.Stop();
                    //    StartCoroutine(DetectRoutine());
                    //}
                    //else
                    //{
                    //    m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //}
                    break;
                case State.ReturnToSpawnPoint:
                    StopAllCoroutines();
                    StartCoroutine(ReturnToStartingPoint());
                    break;
                case State.Idle:
                    m_movement.Stop();
                    if (!m_isDetectedOnce)
                    {
                        m_animation.SetAnimation(0, m_info.stillAnimation.animation, true);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.stillAnimation.animation)
                    //{
                    //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //}
                    break;

                //case State.Turning:
                //    m_stateHandle.Wait(m_turnState);
                //    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    //break;

                case State.Attacking:
                    StopAllCoroutines();
                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();
                    }
                    switch (/*m_attackDecider.chosenAttack.attack*/ m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                           // m_animation.EnableRootMotion(true, false);
                            StartCoroutine(AttackOne());
                           // m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.Attack2:
                           // m_animation.EnableRootMotion(true, false);
                            StartCoroutine(AttackTwo()); 
                           // m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation.animation);
                            break;
                    }
                  
                    //if (IsFacingTarget())
                    //{
                    //    if (IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting)
                    //    {
                    //        //m_stateHandle.Wait(State.Cooldown);
                    //        Debug.Log("Attacking state state");
                    //        m_animation.SetAnimation(0, m_info.idleAnimation, true);

                    //        switch (/*m_attackDecider.chosenAttack.attack*/ m_currentAttack)
                    //        {
                    //            case Attack.Attack1:
                    //                m_animation.EnableRootMotion(true, false);

                    //                m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation.animation);
                    //                break;
                    //            case Attack.Attack2:
                    //                m_animation.EnableRootMotion(true, false);
                    //                m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation.animation);
                    //                break;
                    //        }
                    //        m_attackDecider.hasDecidedOnAttack = false;
                    //    }
                    //    else
                    //    {
                            
                    //        if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                    //        {
                    //            m_animation.EnableRootMotion(true, false);
                    //            m_animation.SetAnimation(0, m_info.move.animation, true);
                    //            m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                    //            //m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                    //        }
                    //        else
                    //        {
                    //            Debug.Log("Attacking state else state");
                    //            m_movement.Stop();
                    //            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    CustomTurn();
                    //}
                    break;

                //case State.Cooldown:
                //    //m_stateHandle.Wait(State.ReevaluateSituation);
                //    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                //    if (!IsFacingTarget())
                //    {
                //        m_turnState = State.Cooldown;
                //        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                //            m_stateHandle.SetState(State.Turning);
                //    }
                //    else
                //    {
                //        m_animation.EnableRootMotion(false, false);
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

                //    break;
                case State.Chasing:
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
                        //Debug.Log("Chasing state");
                        ////m_attackDecider.DecideOnAttack();
                        //m_attackDecider.hasDecidedOnAttack = false;
                        //m_attackDecider.DecideOnAttack();
                        ////ChooseAttack();
                        //if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                        //{
                        //    Debug.Log("Chasing state chosen attack");
                        //    m_movement.Stop();
                        //    m_stateHandle.SetState(State.Attacking);
                        //}
                        //else
                        //{
                        //    CustomTurn();
                        //}
                        //else
                        //{
                        //    m_attackDecider.hasDecidedOnAttack = false;
                        //    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                        //    {
                        //        var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                        //        m_animation.EnableRootMotion(false, false);
                        //        m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
                        //        m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                        //    }
                        //    else
                        //    {
                        //        m_movement.Stop();
                        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        //    }
                        //}
                    
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
            transform.position = m_startpoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
