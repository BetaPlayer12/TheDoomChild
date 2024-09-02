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
using Unity.Mathematics;
using NSubstitute;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Wraith")]
    public class WraithAI : CombatAIBrain<WraithAI.Info>, ISummonedEnemy, IBattleZoneAIBrain
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
            private SimpleAttackInfo m_singleAttack = new SimpleAttackInfo();
            public SimpleAttackInfo singleAttack => m_singleAttack;
            [SerializeField]
            private SimpleAttackInfo m_comboAttack = new SimpleAttackInfo();
            public SimpleAttackInfo comboAttack => m_comboAttack;
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private float m_attackOffset;
            public float attackOffset => m_attackOffset;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //AttackData
            [SerializeField]
            private AttackData m_HeavySlashAttackData;
            public AttackData heavySlashAttackData => m_HeavySlashAttackData;
            [SerializeField]
            private AttackData m_SlashComboAttackData;
            public AttackData SlashComboAttackData => m_SlashComboAttackData;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathStartAnimation;
            public BasicAnimationInfo deathStartAnimation => m_deathStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathLoopAnimation;
            public BasicAnimationInfo deathLoopAnimation => m_deathLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathEndAnimation;
            public BasicAnimationInfo deathEndAnimation => m_deathEndAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathCombinedAnimation;
            public BasicAnimationInfo deathCombinedAnimation => m_deathCombinedAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_goingUpLoopAnimation;
            public BasicAnimationInfo goingUpLoopAnimation => m_goingUpLoopAnimation;


            [SerializeField]
            private float m_attackDistance;
            public float attackDistance => m_attackDistance;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_singleAttack.SetData(m_skeletonDataAsset);
                m_comboAttack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_deathStartAnimation.SetData(m_skeletonDataAsset);
                m_deathLoopAnimation.SetData(m_skeletonDataAsset);
                m_deathEndAnimation.SetData(m_skeletonDataAsset);
                m_deathCombinedAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_goingUpLoopAnimation.SetData(m_skeletonDataAsset);

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
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            SingleAttack,
            ComboAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private RaySensor m_rightWallSensor;
        [SerializeField, TabGroup("Reference")]
        private RaySensor m_lefttWallSensor;
        [SerializeField, TabGroup("Reference")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_transformTurnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private Attacker m_attacker;
        [SerializeField, TabGroup("Modules")]
        private WayPointPatrol m_wayPointPatrol;

        [SerializeField]
        private SpineEventListener m_spineListener;

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
        private Vector2 m_startPos;

        private float m_currentCD;
        private bool m_isDetecting;
        private bool m_battleZoneMode;

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            //m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

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
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation || m_stateHandle.currentState == State.Cooldown)
            {
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_flinchHandle.m_autoFlinch = true;
                m_agent.Stop();
                m_stateHandle.Wait(State.ReevaluateSituation);
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
            //StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
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
            Debug.Log("Apply Attacker Data");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.SingleAttack, m_info.singleAttack.range)
                                  , new AttackInfo<Attack>(Attack.ComboAttack, m_info.comboAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void CalculateRunPath()
        {
            bool isRight = m_targetInfo.position.x >= transform.position.x;
            var movePos = new Vector2(transform.position.x + (isRight ? -3 : 3), m_targetInfo.position.y + 10);
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
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            m_animation.animationState.ClearTrack(1);
            m_flinchHandle.m_enableModule = false;
            m_stateHandle.Wait(State.WaitBehaviourEnd);
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);

            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_hitbox.Disable();
            m_flinchHandle.gameObject.SetActive(false);
            m_agent.Stop();
            m_selfCollider.SetActive(false);
            //m_bodyCollider.SetActive(true);
            //m_animation.SetAnimation(0, m_info.deathAnimation, false);
            //m_character.physics.simulateGravity = true;
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            //m_animation.SetAnimation(0, m_info.deathCombinedAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathCombinedAnimation);
            var track = m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            yield return new WaitForSpineAnimationComplete(track);
            m_animation.DisableRootMotion();
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            //yield return new WaitForSeconds(1.6f);
            //m_animation.DisableRootMotion();
            // m_character.physics.simulateGravity = true;
            GetComponent<IsolatedObjectPhysics2D>().simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathLoopAnimation, true);

            while (!m_groundSensor.isDetecting)
            {
                yield return null;
            }

            m_bodyCollider.enabled = true;
            track = m_animation.SetAnimation(0, m_info.deathEndAnimation, false);
            yield return new WaitForSpineAnimationComplete(track);
            m_animation.DisableRootMotion();
            enabled = false;
            this.gameObject.SetActive(false);
            yield return null;
        }

        private void CustomDecideOnAttack()
        {
            //single attack is AttackCache[0]
            //Combo Attack is AttackCache[1]

            int attackChance = UnityEngine.Random.Range(0, 100);
            if (attackChance <= 60)
            {
                //Choose AttackCache[0]
                m_attackDecider.DecideOnAttack(Attack.SingleAttack);
                m_currentAttack = Attack.SingleAttack;
                m_currentAttackRange = m_attackRangeCache[0];
                //m_attackDecider.hasDecidedOnAttack = true;
                Debug.Log("Chosen Attack: " + m_attackCache[0]);
            }
            else
            {
                //Choose AttackCache[1]
                m_attackDecider.DecideOnAttack(Attack.ComboAttack);
                m_currentAttack = Attack.ComboAttack;
                m_currentAttackRange = m_attackRangeCache[1];
                //m_attackDecider.hasDecidedOnAttack = true;
                Debug.Log("Chosen Attack: " + m_attackCache[1]);
            }
        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                //IsAllAttackComplete();
                //for (int i = 0; i < m_attackCache.Count; i++)
                //{
                //    //m_attackDecider.DecideOnAttack();
                //    CustomDecideOnAttack();
                //    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                //    {
                //        m_attackUsed[i] = true;
                //        m_currentAttack = m_attackCache[i];
                //        m_currentAttackRange = m_attackRangeCache[i];
                //        return;
                //    }
                //}
                CustomDecideOnAttack();
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
            switch (m_attack)
            {
                case Attack.SingleAttack:
                    StartCoroutine(HeavySlashAttackRoutine());
                    break;
                case Attack.ComboAttack:
                    StartCoroutine(ComboAttackRoutine());
                    break;
            }
        }

        private IEnumerator HeavySlashAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);

            m_agent.Stop();
            m_animation.EnableRootMotion(true, true);

            m_attacker.SetData(m_info.heavySlashAttackData);
            m_animation.SetAnimation(0, m_info.singleAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.singleAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            m_animation.DisableRootMotion();

            m_attackDecider.hasDecidedOnAttack = false;

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ComboAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);

            m_agent.Stop();
            m_animation.EnableRootMotion(true, true);

            m_attacker.SetData(m_info.SlashComboAttackData);
            m_animation.SetAnimation(0, m_info.comboAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.comboAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            m_animation.DisableRootMotion();

            m_attackDecider.hasDecidedOnAttack = false;

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(m_targetInfo.position.x - ((Vector2.Distance(m_targetInfo.position, transform.position) > attackRange ? attackRange : 0f) * transform.localScale.x), /*GroundPosition().y + 20*/m_targetInfo.position.y - m_info.attackOffset);
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < (Vector2.Distance(m_targetInfo.position, transform.position) > attackRange ? 1 : attackRange) ? true : false;
                bool yTargetInRange = Mathf.Abs(/*m_targetInfo.position.y*/newPos.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                //DynamicMovement(/*new Vector2(m_targetInfo.position.x, m_targetInfo.position.y)*/ newPos);
                DynamicMovement(newPos, moveSpeed);
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

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            m_startPos = transform.position;
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();

            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_transformTurnHandle.TurnDone += OnTurnDone;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.SingleAttack, Attack.ComboAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.singleAttack.range, m_info.comboAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
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
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReturnToPatrol:
                    if (IsFacing(m_startPos))
                    {
                        if (Vector2.Distance(m_startPos, transform.position) > 5f)
                        {
                            var rb2d = GetComponent<Rigidbody2D>();
                            m_agent.Stop();
                            Vector3 dir = (m_startPos - (Vector2)rb2d.transform.position).normalized;
                            rb2d.MovePosition(rb2d.transform.position + dir * m_info.patrol.speed * Time.fixedDeltaTime);
                            m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.Patrol);
                        }
                    }
                    else
                    {
                        m_turnState = State.ReturnToPatrol;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    m_turnState = State.Patrol;
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.flinchAnimation.animation)
                    {
                        m_stateHandle.Wait(m_turnState);
                        if (m_executeMoveCoroutine != null)
                        {
                            StopCoroutine(m_executeMoveCoroutine);
                            m_executeMoveCoroutine = null;
                        }
                        m_agent.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        switch (m_turnState)
                        {
                            case State.Patrol:
                                m_transformTurnHandle.Execute();
                                break;
                            default:
                                m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                                break;
                        }
                    }
                    break;
                case State.Attacking:
                    //m_stateHandle.Wait(State.Cooldown);
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //m_agent.Stop();
                    //m_executeMoveCoroutine = StartCoroutine(ExecuteMove(/*m_currentAttackRange*/ m_attackRangeCache[0], m_currentAttack));
                    //m_attackDecider.hasDecidedOnAttack = false;

                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        CustomDecideOnAttack();
                    }

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.SingleAttack:
                            StartCoroutine(HeavySlashAttackRoutine());
                            break;
                        case Attack.ComboAttack:
                            StartCoroutine(ComboAttackRoutine());
                            break;
                        default:
                            StartCoroutine(HeavySlashAttackRoutine());
                            break;
                    }
                    break;
                case State.Cooldown:
                    StartCoroutine(CooldownRoutine());

                    break;
                case State.Chasing:
                    //if (m_attackDecider.hasDecidedOnAttack /*&& !ShotBlocked()*/)
                    //{
                    //    m_agent.Stop();
                    //    m_stateHandle.SetState(State.Attacking);
                    //}
                    StartCoroutine(ChasePlayerRoutine());
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else

                    {
                        m_stateHandle.SetState(State.Patrol);
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

        private void FacePlayer()
        {
            if (!IsFacingTarget())
            {
                //m_turnState = State.Cooldown;
                //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                //    m_stateHandle.SetState(State.Turning);
                m_turnHandle.ForceTurnImmidiately();
            }
        }

        private Vector2 SetRunawayPosition()
        {
            Vector2 destination = transform.position;
            bool isCloseToWall = false;

            switch (m_character.facing)
            {
                case HorizontalDirection.Left:
                    {
                        if (m_rightWallSensor.isDetecting)
                        {
                            isCloseToWall = true;
                            destination = new Vector2(transform.position.x + 3, m_targetInfo.position.y + 1);
                        }
                    }
                    break;
                case HorizontalDirection.Right:
                    {
                        if (m_rightWallSensor.isDetecting)
                        {
                            isCloseToWall = true;
                            destination = new Vector2(transform.position.x - 3, m_targetInfo.position.y + 1);
                        }
                    }
                    break;
            }

            if (!isCloseToWall)
            {
                if (m_targetInfo.position.x >= transform.position.x)
                {
                    destination = new Vector2(transform.position.x - 3, m_targetInfo.position.y + 1);
                }
                else if(m_targetInfo.position.x < transform.position.x)
                {
                    destination = new Vector2(transform.position.x + 3, m_targetInfo.position.y + 1);
                }
            }
            
            return destination;
        }

        private IEnumerator CooldownRoutine()
        {
            //run away while cooldown is > 0
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_animation.SetAnimation(0, m_info.goingUpLoopAnimation.animation, true);

            FacePlayer();

            m_currentCD = 0;

            while (m_currentCD <= m_info.attackCD)
            {
                FacePlayer();

                if(m_lefttWallSensor.isDetecting)
                {
                    m_currentCD = m_info.attackCD;
                }

                //var movePos = new Vector2(transform.position.x + (targetIsAtRight ? -3 : 3), m_targetInfo.position.y + 1);
                var movePos = SetRunawayPosition();
                m_agent.SetDestination(movePos);
                m_agent.Move(m_info.move.speed);

                m_currentCD += Time.deltaTime;
                yield return null;
            }

            m_agent.Stop();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChasePlayerRoutine()
        {
            m_stateHandle.Wait(State.Attacking);

            m_animation.SetAnimation(0, m_info.move.animation, true);

            while (!IsInRange(m_targetInfo.position, m_info.attackDistance))
            {
                FacePlayer();
                m_agent.SetDestination(m_targetInfo.position);
                m_agent.Move(m_info.move.speed);
                yield return null;
            }

            m_agent.Stop();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            enabled = true;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public override void ReturnToSpawnPoint()
        {
            Patience();
        }

        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            enabled = false;
            m_targetInfo = target;
            m_isDetecting = true;
            transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y + 10f);
            Vector2 summonedPosStart = position;
            Vector2 summonedPosEnd = new Vector2(position.x + 10, position.y);
            Vector2[] summonablePatrolPoints = { summonedPosStart, summonedPosEnd };
            m_wayPointPatrol.SetWayPoints(summonablePatrolPoints);
            m_character.physics.simulateGravity = false;
            m_hitbox.Enable();
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            if (!IsFacingTarget())
                CustomTurn();
            Awake();
            m_stateHandle.OverrideState(State.Detect);
            enabled = true;
        }

        public void DestroyObject()
        {
            throw new NotImplementedException();
        }

        public void SwitchToBattleZoneAI()
        {
            m_battleZoneMode = true;
            Vector2 summonedPosStart = transform.position;
            Vector2 summonedPosEnd = new Vector2(transform.position.x + 10, transform.position.y);
            Vector2[] summonablePatrolPoints = { summonedPosStart, summonedPosEnd };
            m_stateHandle.OverrideState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_battleZoneMode = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }
    }
}