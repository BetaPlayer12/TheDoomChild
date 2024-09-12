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
using DChild.Gameplay.Pathfinding;
using DarkTonic.MasterAudio;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/MimicPustuleBomb")]
    public class MimicPustuleBombAI : CombatAIBrain<MimicPustuleBombAI.Info>, IResetableAIBrain
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
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
           
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
            [SerializeField]
            private float m_distanceToOriginReturnTolerance = 10f;
            public float distanceToOriginReturnTolerance => m_distanceToOriginReturnTolerance;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAggroAnimation1 = new BasicAnimationInfo();
            public BasicAnimationInfo idleAggroAnimation1 => m_idleAggroAnimation1;
            [SerializeField]
            private BasicAnimationInfo m_idleAggroAnimation2 = new BasicAnimationInfo();
            public BasicAnimationInfo idleAggroAnimation2 => m_idleAggroAnimation2;
            [SerializeField]
            private BasicAnimationInfo m_idleUnAggroAnimation1 = new BasicAnimationInfo();
            public BasicAnimationInfo idleUnAggroAnimation1 => m_idleUnAggroAnimation1;
            [SerializeField]
            private BasicAnimationInfo m_idleUnAggroAnimation2 = new BasicAnimationInfo();
            public BasicAnimationInfo idleUnAggroAnimation2 => m_idleUnAggroAnimation2;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_bounceUnAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo bounceUnAggroAnimation => m_bounceUnAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_bounceAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo bounceAggroAnimation => m_bounceAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAggroAnimation => m_flinchAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchUnAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchUnAggroAnimation => m_flinchUnAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_transformAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo transformAggroAnimation => m_transformAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_transformUnAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo transformUnAggroAnimation => m_transformUnAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAggroAnimation => m_deathAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathUnAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathUnAggroAnimation => m_deathUnAggroAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
               

                m_idleAggroAnimation1.SetData(m_skeletonDataAsset);
                m_idleAggroAnimation2.SetData(m_skeletonDataAsset);
                m_idleUnAggroAnimation1.SetData(m_skeletonDataAsset);
                m_idleUnAggroAnimation2.SetData(m_skeletonDataAsset);
                m_bounceUnAggroAnimation.SetData(m_skeletonDataAsset);
                m_bounceAggroAnimation.SetData(m_skeletonDataAsset);
                m_flinchAggroAnimation.SetData(m_skeletonDataAsset);
                m_flinchUnAggroAnimation.SetData(m_skeletonDataAsset);
                m_transformAggroAnimation.SetData(m_skeletonDataAsset);
                m_transformUnAggroAnimation.SetData(m_skeletonDataAsset);
                m_deathAggroAnimation.SetData(m_skeletonDataAsset);
                m_deathUnAggroAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            ReturnToPatrol,
            Patrol,
            Cooldown,
            Turning,
            Attacking,
            Transforming,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_environmentCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodycollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_aggroGroup;
        [SerializeField, TabGroup("Reference")]
        private SpringJoint2D m_mimicPustuleBombChain;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_parentObject;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_aggroHitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_attackBB;
        [SerializeField, TabGroup("Reference")]
        private Transform m_pushDirection;
        [SerializeField, TabGroup("Reference")]
        private List<GameObject> m_PustuleBombsPosition;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_ExplosionBB;

        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        //[SerializeField, TabGroup("Modules")]
        //private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private WayPointPatrol m_wayPointPatrol;
        [SerializeField, TabGroup("Modules")]
        private DamageContactLocator m_damageContactLocator;
        [SerializeField, TabGroup("Modules")]
        private CollisionEventActionArgs collisionEvent;
        [SerializeField,TabGroup("Modules")]
        NavigationTracker navigationTracker = null;
        [SerializeField, TabGroup("Modules")]
        MovementHandle2D movementOverrideHandle;

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
        private float m_randomRangeMin = 150f;
        private float m_randomRangeMax = 250f;
        private bool m_pathChecked;

        private bool m_isDetecting;
        private bool m_isAggro;
        private bool m_isFirstPatrol = true;
        private bool m_returnToOriginalPos;
        private Vector2 m_startPos;
        private Vector2 m_OutOfBoundMoveDir;

        private List<Vector2> m_Points;

        private Coroutine m_executeMoveCoroutine;
        private Coroutine m_detectRoutine;
        private float m_AwayfromOrigPosinSeconds;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_currentAttack = Attack.Attack;
            m_flinchHandle.gameObject.SetActive(true);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
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
                    m_stateHandle.OverrideState(State.Detect);
                }
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
            }
        }


        public void SetAI(AITargetInfo targetInfo)
        {
            m_isDetecting = true;
            m_targetInfo = targetInfo;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_isAggro)
            {
                //m_mimicPustuleBombChain.enabled = true;
                m_flinchHandle.SetAnimation(m_info.flinchAggroAnimation.animation);
                m_flinchHandle.SetIdleAnimation(m_info.idleAggroAnimation1.animation);

            }
            else
            {
                m_mimicPustuleBombChain.enabled = false;
                m_flinchHandle.SetAnimation(m_info.flinchUnAggroAnimation.animation);
                m_flinchHandle.SetIdleAnimation(m_info.idleUnAggroAnimation1.animation);
                //StopCoroutine("ReturnToOriginalPosition");
                StartCoroutine(ReturnToOriginalPosition(5f));
            }
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAggroAnimation1.animation || m_stateHandle.currentState == State.Cooldown)
            {
                
                m_flinchHandle.m_autoFlinch = true;
                m_agent.Stop();
                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_stateHandle.OverrideState(State.ReevaluateSituation);
                StopAllCoroutines();
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_flinchHandle.m_autoFlinch = false;
                m_stateHandle.ApplyQueuedState();
            }
            if (!m_isAggro)
            {
                m_stateHandle.OverrideState(State.ReturnToPatrol);
            }
        }

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());

            return hit.point;
        }

        //Patience Handler
        private void Patience()
        {
            enabled = false;
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();

            if (m_isAggro)
            {
                m_animation.SetAnimation(0, m_info.idleAggroAnimation1, true);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.transformUnAggroAnimation, false);
            }
            Debug.Log("?????????AHHHHHHHHHHHH");
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_returnToOriginalPos = true;
            enabled = true;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_returnToOriginalPos = false;
            m_aggroGroup.SetActive(true);
            m_isAggro = true;
            m_mimicPustuleBombChain.enabled = false;
            if (!IsFacingTarget())
            {
                CustomTurn();
            }

            m_animation.SetAnimation(0, m_info.detectAnimation, false); ;
            yield return new WaitForSeconds(.25f);
                
            //m_animation.SetAnimation(0, m_info.idleAggroAnimation2, true);
            m_animation.SetAnimation(0, m_info.idleAggroAnimation1, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private void CalculateRunPath()
        {
            Debug.Log("????!??????????");
            bool isRight = m_targetInfo.position.x >= transform.position.x;
            var movePos = new Vector2(transform.position.x + (isRight ? -3 : 3), m_targetInfo.position.y +10);
            while (Vector2.Distance(transform.position, WallPosition()) <= 5)
            {

                movePos = new Vector2(movePos.x + 0.1f, movePos.y);
                break;
            }
            m_agent.SetDestination(movePos);
        }

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;
        

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_animation.SetEmptyAnimation(0, 0);
            /*if (m_isAggro)
            {
                m_deathHandle.SetAnimation(m_info.deathAggroAnimation.animation);
            }*/
            StartCoroutine(DeathRoutine());
            m_agent.Stop();
            m_bodycollider.enabled = false;
            m_selfCollider.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
        }

        private IEnumerator DeathRoutine()
        {
            m_hitbox.enabled = false;
            m_aggroGroup.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (m_isAggro)
            {
                m_animation.SetAnimation(0, m_info.deathAggroAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAggroAnimation);
                m_animation.SetAnimation(0, m_info.deathUnAggroAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathUnAggroAnimation);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.deathUnAggroAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathUnAggroAnimation);
            }
            yield return new WaitForSeconds(.75f);
            /*
            m_deathHandle.enabled = true;
            enabled = false;*/
            yield return new WaitForSeconds(1.25f);
            m_animation.EnableRootMotion(true, false);
            m_parentObject.SetActive(false);
            Debug.Log("Die Mimic");
            yield return null;
        }
        private IEnumerator TransformUnAggroRoutine()
        {
           
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.transformUnAggroAnimation.animation)
            {
                m_animation.SetAnimation(0, m_info.transformUnAggroAnimation, false);
                yield return new WaitForSeconds(1f);
                m_animation.SetAnimation(0, m_info.idleUnAggroAnimation1, true);
                m_isAggro = false;
                m_isDetecting = false;
            }
            
            //yield return null;
        }


        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_bodycollider.enabled = true;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            switch (/*m_attack*/ m_currentAttack)
            {
                case Attack.Attack:
                    StartCoroutine(AttackRoutine());
                    break;
            }
        }

        private IEnumerator AttackRoutine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_aggroHitbox.Disable();
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            m_animation.EnableRootMotion(true, false);
            m_hitbox.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_animation.SetAnimation(0, m_info.idleAggroAnimation1, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
            m_aggroHitbox.Enable();
        }

        #endregion

        #region Movement


        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            bool inRange = false;
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            while (!inRange || TargetBlocked())
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < attackRange -20 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }

                m_turnState = State.ReevaluateSituation;
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

        private IEnumerator DelayChainActivation(float delay)
        {
            yield return new WaitForSeconds(delay);
            if(Vector2.Distance(m_startPos, transform.position) > m_info.distanceToOriginReturnTolerance)
            {
                m_AwayfromOrigPosinSeconds = 9.5f;
                Debug.Log("How much is this called?");
                //m_returnToOriginalPos = true;
                m_stateHandle.OverrideState(State.ReturnToPatrol);
                //m_mimicPustuleBombChain.enabled = true;
            }
        }

        private void DynamicMovement(Vector2 target, float moveSpeed)
        {
            m_agent.SetDestination(target);
            if (!m_pathChecked)
            {
                m_agent.Move(moveSpeed); // needed to run at least once to let the agent make a path
                if (!navigationTracker.wasEntityInStartingPathSegment)
                {
                    m_agent.Stop();
                    m_OutOfBoundMoveDir = ( navigationTracker.previousPathSegment- transform.position).normalized;
                    //Move Towards navigationTracker.previousPathSegment; using movementOverrideHandle.MoveTowards
                }
                m_pathChecked = true;
            }
            if (m_agent.hasPath)
            {
                {
                    if(Vector3.Distance(navigationTracker.previousPathSegment, transform.position) > 2f&&!navigationTracker.wasEntityInStartingPathSegment)
                    {
                        Debug.Log(m_OutOfBoundMoveDir);
                        //StartCoroutine(ReturnToOriginalPosition(1.5f));
                        //m_returnToOriginalPos = true;
                        movementOverrideHandle.MoveTowards(m_OutOfBoundMoveDir, m_info.move.speed);
                    }
                    else
                    {
                        m_rigidbody2D.velocity = Vector2.zero;
                        if (IsFacing(target))
                        {
                            m_agent.Move(moveSpeed);
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.Turning);
                        }
                    }
                }
            }
        }

        private IEnumerator ReturnToOriginalPosition(float delay)
        {
            //m_returnToOriginalPos = false;
            m_AwayfromOrigPosinSeconds = (10 - delay);
            yield return null;
            /*
            yield return new WaitForSeconds(delay);
            m_returnToOriginalPos=true;*/
        }

        #endregion

        public void ExplodeDamage()
        {
            StartCoroutine(ExplodeRoutine());
        }

        IEnumerator ExplodeRoutine()
        {
            m_ExplosionBB.SetActive(true);
            yield return new WaitForSeconds(.25f);
            m_ExplosionBB.SetActive(false);
        }

        private void SwapPustuleBombPosition()
        {
            if(m_PustuleBombsPosition.Count<=0)
            {
                return;
            }
            var randomSwap = UnityEngine.Random.Range(1, 100);
            var shouldSwap = randomSwap <= 50 ? true : false;
            if (shouldSwap) 
            {
                var random = UnityEngine.Random.Range(1, 100);
                var pustuleBombPosition = m_startPos;
                Vector3 randomPustuleBombPosition;
                var index = random % m_PustuleBombsPosition.Count;
                randomPustuleBombPosition = m_PustuleBombsPosition[index].transform.position;
                m_PustuleBombsPosition[index].transform.position = pustuleBombPosition;
                m_parentObject.transform.position = randomPustuleBombPosition;
            }
            m_wayPointPatrol.wayPoints[0].Set(m_parentObject.transform.position.x, m_parentObject.transform.position.y);
            m_stateHandle.OverrideState(State.Patrol);
            
        }

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_animation.DisableRootMotion();
            m_bodycollider.enabled = false;
            SwapPustuleBombPosition();
            m_startPos = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();

            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            //m_deathHandle.SetAnimation(m_info.deathAggroAnimation.animation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_Points = new List<Vector2>();

            m_attackCache = new List<Attack>();
            m_attackUsed = new bool[m_attackCache.Count];
        }
        IEnumerator delayedAgentStopperRoutine()
        {
            // this is a failsafe
            
            yield return new WaitForSeconds(3.5f);
            Debug.Log("THIS IS A FAILSAFE// SOMETHING GONE WRONG SO RESET");
            m_agent.Stop();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_rigidbody2D.AddForce((Vector2.up * 30f) + (Vector2.right * 35f), ForceMode2D.Force);
            ResetAI();
            Patience();
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
        }
        private void Update()
        {
            if(m_AwayfromOrigPosinSeconds<=10)
            {
                m_AwayfromOrigPosinSeconds += Time.deltaTime;
                m_returnToOriginalPos = false;
            }
            else if(m_AwayfromOrigPosinSeconds>10)
            {
                m_returnToOriginalPos = true;
            }
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();

                    if (IsFacingTarget() && m_detectRoutine == null)
                    {

                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_detectRoutine = StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        m_stateHandle.SetState(State.Turning);
                        
                    }

                    break;

                case State.ReturnToPatrol:

                    if (m_detectRoutine != null)
                    {
                        m_detectRoutine = null;
                    }

                    if (Vector2.Distance(m_startPos, transform.position) > 3f)
                    {
                        if (m_isAggro)
                        {
                            StartCoroutine(TransformUnAggroRoutine());
                        }

                        DynamicMovement(m_startPos, m_info.move.speed);
                        m_aggroGroup.SetActive(false);
                        //m_rigidbody2D.velocity = Vector2.zero;
                        m_turnState = State.ReturnToPatrol;

                    }
                    else
                    {
                        if (m_isAggro)
                        {
                            StartCoroutine(TransformUnAggroRoutine());
                        }

                        if (m_returnToOriginalPos)//&& Vector2.Distance(m_startPos, transform.position) < 0.2f)
                        {
                            if(m_isDetecting)
                            {
                                m_isDetecting = false;
                            }
                            Debug.Log(Vector2.Distance(m_startPos, transform.position) + " AAAAAAAAAAAAAAAAAAAA");
                            m_mimicPustuleBombChain.enabled = true;
                            m_returnToOriginalPos = false;
                            m_rigidbody2D.velocity = Vector2.up*3f;
                            m_pathChecked = false;
                        }
                        else
                        {
                            //DynamicMovement(m_startPos, 1f);
                            m_rigidbody2D.velocity = Vector2.up * 3f;
                        }
                        m_aggroGroup.SetActive(false);
                        m_stateHandle.OverrideState(State.Patrol);
                    }

                    break;

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idleUnAggroAnimation1.animation : m_info.idleUnAggroAnimation2.animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }

                    if (m_isFirstPatrol) 
                    {
                        m_isFirstPatrol = false;
                        //m_mimicPustuleBombChain.enabled = true;

                    }
                    else
                    {
                        m_randomRangeMin = 10f;
                        m_randomRangeMax = 20f;
                        if(!m_isAggro)
                        {
                            
                            StartCoroutine(DelayChainActivation(5f));
                        }
                    }
                    Vector3 v_diff = new Vector3(UnityEngine.Random.Range(m_randomRangeMin, m_randomRangeMax), UnityEngine.Random.Range(m_randomRangeMin, m_randomRangeMax), UnityEngine.Random.Range(m_randomRangeMin, m_randomRangeMax));   
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x); 
                    m_pushDirection.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                    m_rigidbody2D.AddForce(-m_pushDirection.right * m_info.patrol.speed, ForceMode2D.Force);
                    m_agent.Stop();
                    if (m_rigidbody2D.constraints == RigidbodyConstraints2D.FreezeAll)
                    {
                        Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH HOW DID YOU GET HERE!!!!!!!!!!!!!!!!!");
                        /*//m_rigidbody2D.constraints = RigidbodyConstraints2D.None;
                        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        StopAllCoroutines();
                        DynamicMovement(m_startPos, m_info.move.speed);
                        m_agent.Stop();
                        //m_rigidbody2D.constraints = ~RigidbodyConstraints2D.FreezePosition; /// FFS sometimes the entity just returns with a frozen position
                        m_rigidbody2D.AddForce((Vector2.up * 30f) + (Vector2.right * 15f), ForceMode2D.Force);
                        //m_rigidbody2D.AddForce((m_startPos - (Vector2)transform.position).normalized*30f, ForceMode2D.Force);
                        //m_rigidbody2D.velocity = Vector2.up * 3f;
                        Patience();
                        //ResetAI();
                        //OnFlinchStart();
                        m_stateHandle.OverrideState(State.Cooldown);*/
                        StartCoroutine(delayedAgentStopperRoutine());
                        //break;
                    }
                        m_stateHandle.Wait(State.ReevaluateSituation);
                    
                    m_isFirstPatrol = false;
                    
                    break;


                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    //m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    if(m_mimicPustuleBombChain.enabled)
                    {
                        m_mimicPustuleBombChain.enabled = false;
                    }
                    m_stateHandle.Wait(State.Cooldown);
                    //m_animation.SetAnimation(0, m_info.idleAggroAnimation2, true);
                    m_animation.SetAnimation(0, m_info.idleAggroAnimation1, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(25f, m_currentAttack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            //var chosenMoveAnim = UnityEngine.Random.Range(0, 50) > 10 ? m_info.idleAggroAnimation1.animation : m_info.idleAggroAnimation2.animation;
                            m_animation.SetAnimation(0, m_info.idleAggroAnimation1, true);
                        }

                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else
                        {
                            if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.targetDistanceTolerance + 10)
                            {
                                m_agent.SetDestination(m_targetInfo.position);
                                m_agent.Move(m_info.move.speed);
                            }
                            else
                            {
                                m_agent.Stop();
                                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                                m_animation.SetAnimation(0, m_info.idleAggroAnimation1, true).TimeScale = 1f;
                            }
                        }
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    m_agent.Stop();
                    m_stateHandle.SetState(State.Attacking);
                    break;

                case State.ReevaluateSituation:
                    var timer = 0f;
                    timer++;
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else if(!m_targetInfo.isValid && timer == 10f)

                    {
                        m_aggroGroup.SetActive(false);
                        m_AwayfromOrigPosinSeconds = 9.5f;
                        m_isAggro = false;
                        m_stateHandle.SetState(State.ReturnToPatrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_targetInfo.isValid)
            {
                if (TargetBlocked())
                {
                    if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.patienceDistanceTolerance)
                    {
                        Patience();
                    }
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_hitbox.gameObject.SetActive(true);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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