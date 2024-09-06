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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/CelestialVoid")]
    public class CelestialVoidAI : CombatAIBrain<CelestialVoidAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_counterFlinchAnimation;
            public BasicAnimationInfo counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fadeInAnimation;
            public BasicAnimationInfo fadeInAnimation => m_fadeInAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fadeOutAnimation;
            public BasicAnimationInfo fadeOutAnimation => m_fadeOutAnimation;
            [SerializeField]
            private BasicAnimationInfo m_lifeDrainAnticipationAnimation;
            public BasicAnimationInfo lifeDrainAnticipationAnimation => m_lifeDrainAnticipationAnimation;
            [SerializeField]
            private BasicAnimationInfo m_lifeDrainAnimation;
            public BasicAnimationInfo lifeDrainAnimation => m_lifeDrainAnimation;
            [SerializeField]
            private BasicAnimationInfo m_lifeDrainEndAnimation;
            public BasicAnimationInfo lifeDrainEndAnimation => m_lifeDrainEndAnimation;
            [SerializeField]
            private BasicAnimationInfo m_healAnticipationAnimation;
            public BasicAnimationInfo healAnticipationAnimation => m_healAnticipationAnimation;
            [SerializeField]
            private BasicAnimationInfo m_healAnimation;
            public BasicAnimationInfo healAnimation => m_healAnimation;
            [SerializeField]
            private BasicAnimationInfo m_healEndAnimation;
            public BasicAnimationInfo healEndAnimation => m_healEndAnimation;
            [SerializeField]
            private GameObject m_teleportMarker;
            public GameObject teleportMarker => m_teleportMarker;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);

                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_counterFlinchAnimation.SetData(m_skeletonDataAsset);
                m_fadeInAnimation.SetData(m_skeletonDataAsset);
                m_fadeOutAnimation.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_lifeDrainAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_lifeDrainAnimation.SetData(m_skeletonDataAsset);
                m_lifeDrainEndAnimation.SetData(m_skeletonDataAsset);
                m_healAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_healAnimation.SetData(m_skeletonDataAsset);
                m_healEndAnimation.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Detect,
            ReturnToPatrol,
            Patrol,
            Idle,
            Cooldown,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
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
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainFXline;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainFXline2;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainEnragedFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainEnragedFXline;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainEnragedFXline2;
        [SerializeField, TabGroup("FX")]
        private Transform m_headPosition;
        [SerializeField, TabGroup("FX")]
        private GameObject m_detectionFX;
        [SerializeField]
        private bool m_willPatrol;
        [SerializeField]
        private TeleportPlayer m_teleportPlayer;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private float m_currentCD;
        private bool m_isDetecting;
        private bool m_enablePatience;
        public bool m_willTeleportPlayer;
        private Vector2 m_startPos;
        private LineRenderer lineRenderer1;
        private LineRenderer lineRenderer2;
        private LineRenderer EnragedlineRenderer1;
        private LineRenderer EnragedlineRenderer2;
        private bool m_willReturnToPatrol;
        [SerializeField]
        private Transform m_teleportationMarker;

        private Coroutine m_executeMoveCoroutine;
        private Coroutine m_patienceRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
        }
        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_selfCollider.SetActive(true);
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                if (TargetBlocked())
                {
                    if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.patienceDistanceTolerance)
                    {
                        Patience();
                    }
                }
            }
            else/*if (m_targetInfo.isValid)*/
            {
                /*if (TargetBlocked())
                {
                    if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.patienceDistanceTolerance)
                    {
                        Patience();
                    }
                }*/
                //StopAllCoroutines();
                //m_isDetecting = false; 
                if (m_stateHandle.currentState == State.WaitBehaviourEnd)
                {
                    //StopAllCoroutines();
                    StartCoroutine(PatienceRoutine());
                    m_stateHandle.OverrideState(State.ReturnToPatrol);
                    Debug.Log("State of waitBehaviourEnd");
                }
                else
                {
                    //StopAllCoroutines();
                    StartCoroutine(PatienceRoutine());
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            //StopAllCoroutines();
            m_agent.Stop();
            m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            if (m_targetInfo.isValid)
            {
                /*if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                if (m_animation.GetCurrentAnimation(0).ToString() == m_info.attack1.animation)
                {
                    StartCoroutine(CounterFlinchRoutine());
                }*/
                //else
                //{
                    StartCoroutine(FlinchRoutine());
                //}
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_hitbox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            /*yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);*/
            yield return new WaitForSeconds(0.1f);
            m_hitbox.gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        //Patience Handler
        private void Patience()
        {
            if (m_patienceRoutine == null)
            {
                m_patienceRoutine = StartCoroutine(PatienceRoutine());
            }
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            /*StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.SetState(State.ReturnToPatrol);*/
        }
        private IEnumerator PatienceRoutine()
        {
            yield return new WaitForSeconds(5f);
            //StopAllCoroutines();
            m_enablePatience = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            //StopAllCoroutines();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.SetState(State.ReturnToPatrol);
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range)
                                  /*, new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }
        private void CalculateRunPath()
        {
            bool isRight = m_targetInfo.position.x >= transform.position.x;
            var movePos = new Vector2(transform.position.x + (isRight ? -10 : 10), m_targetInfo.position.y + 10);
            while (Vector2.Distance(transform.position, WallPosition()) <= 10)
            {
                movePos = new Vector2(movePos.x + 0.1f, movePos.y);
                break;
            }
            m_agent.SetDestination(movePos);
        }
        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            Animator anim = m_detectionFX.GetComponent<Animator>();
            anim.SetBool("detected", true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;
        }
        public EventAction<EventActionArgs> OnAttackingState;
        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(.5f);
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.healAnticipationAnimation, false);
            /*if (m_willTeleportPlayer)
            {
                instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.teleportMarker);
                instance.SpawnAt(new Vector2(0, 0), Quaternion.identity);
                instance.gameObject.transform.SetParent(m_targetInfo.transform);
                instance.transform.position = m_targetInfo.position;
                if (shortTime)
                    m_targetInfo.transform.position = m_teleportationMarker.position;
            }*/
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnticipationAnimation);
            m_animation.SetAnimation(0, m_info.healAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnimation);
            m_animation.SetAnimation(0, m_info.healEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healEndAnimation);
            m_hitbox.gameObject.SetActive(true);
            //yield return new WaitForSeconds(2f);
            //yield return PlayerInVisionCounter();
            //m_targetInfo.transform.position = m_teleportationMarker.position;
            /*if (m_willTeleportPlayer && waitAnimation)
            {
                instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.teleportMarker);
                instance.SpawnAt(new Vector2(0, 0), Quaternion.identity);
                instance.gameObject.transform.SetParent(m_targetInfo.transform);
                instance.transform.position = m_targetInfo.position;
                if (waitAnimation)
                    m_targetInfo.transform.position = m_teleportationMarker.position;
            }*/
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_selfCollider.SetActive(false);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        
        /*private bool IsOverlappingWithEnvironment(Vector2 position, float angle)
        {

            Collider2D[] hits = Physics2D.OverlapBoxAll(position, new Vector2(100f, 25f), angle, m_info.mask);

            if (hits.Length > 0)
            {
                return true;
            }

            else
            {
                return false;
            }

        }*/
        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y, transform.position.x), new Vector3(20f, 25f, 0f));
        }*/
        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            var rb2d = GetComponent<Rigidbody2D>();
            /*m_character.physics.simulateGravity = true;*/
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

        private void ExecuteAttack(Attack m_attack)
        {
            m_flinchHandle.m_autoFlinch = false;
            m_agent.Stop();
            m_animation.EnableRootMotion(true, false);
            StartCoroutine(AttackRoutine());
        }
        #region Movement
        private IEnumerator ExecuteMove(/*float attackRange, float heightOffset,*/ Attack attack)
        {
            OnAttackingState?.Invoke(this, EventActionArgs.Empty);
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var newPos = Vector2.zero;
            var moveSpeed = m_info.patrol.speed * 3;
            while (!inRange || TargetBlocked())
            {
                //newPos = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y);
                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < m_currentAttackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < m_currentAttackRange ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                m_animation.SetAnimation(0, m_info.patrol, true);
                DynamicMovement(m_targetInfo.position, moveSpeed);
                yield return null;
            }
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target, float moveSpeed)
        {
            var rb2d = GetComponent<Rigidbody2D>();
            m_agent.SetDestination(target);
            if (IsFacing(target))
            {
                m_agent.Move(moveSpeed);
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            m_startPos = transform.position;
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_teleportPlayer.GetComponent<TeleportPlayer>().OnPlayerStay += OnPlayerStay;
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Attack1);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attack1.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }
        private PoolableObject instance;
        private void OnPlayerStay(object sender, EventActionArgs eventArgs)
        {
            m_willTeleportPlayer = true;
            //instance.SpawnAt(m_targetInfo.position, Quaternion.identity);
            //teleportMarker.transform.position = m_targetInfo.position;
            //m_teleportMarker.Play();
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        CustomTurn();
                    }
                    break;

                case State.ReturnToPatrol:
                    m_stateHandle.OverrideState(State.Patrol);
                    m_enablePatience = false;
                    m_targetInfo.Set(null, null);
                    m_isDetecting = false;
                    break;

                case State.Patrol:
                    m_turnState = State.Patrol;
                    Animator anim = m_detectionFX.GetComponent<Animator>();
                    anim.SetBool("detected", false);
                    m_animation.DisableRootMotion();
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    //StopAllCoroutines();
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_currentAttack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }

                    if (m_currentCD <= m_info.attackCD)
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
                    m_agent.Stop();
                    m_attackDecider.hasDecidedOnAttack = false;
                    //ChooseAttack();
                    m_currentAttackRange = m_info.attack1.range;
                    m_stateHandle.SetState(State.Attacking);
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.ReturnToPatrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }
        public IEnumerator ActivateTeleportFX()
        {
            m_stateHandle.Wait(State.ReturnToPatrol);
            if (m_willTeleportPlayer)
            {
                instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.teleportMarker);
                instance.SpawnAt(new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 2f), Quaternion.identity);
                //instance.transform.SetParent(m_targetInfo.transform.parent); //Attempt 
                GameplaySystem.playerManager.DisableControls();
                GameplaySystem.gamplayUIHandle.ToggleFadeUI(true);
                yield return new WaitForSeconds(.25f);
                m_targetInfo.transform.position = m_teleportationMarker.position;
                //instance.transform.position = m_targetInfo.position;
                /*yield return new WaitForSeconds(.5f);*/
                GameplaySystem.gamplayUIHandle.ToggleFadeUI(false);
                yield return new WaitForSeconds(.5f);
                GameplaySystem.playerManager.EnableControls();
                m_stateHandle.ApplyQueuedState();
                m_willTeleportPlayer = false;
            }
            yield return null;
        }
        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_selfCollider.SetActive(false);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            Patience();
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