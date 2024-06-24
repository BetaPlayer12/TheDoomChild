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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Wraith")]
    public class WraithAI : CombatAIBrain<WraithAI.Info>
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
        private Collider2D m_hurtbox;
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

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_hurtbox.enabled = false;
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
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
            m_stateHandle.Wait(State.WaitBehaviourEnd);
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_hitbox.Disable();
            m_hurtbox.enabled = false;
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
            m_animation.SetAnimation(0, m_info.deathCombinedAnimation, false);
            m_animation.EnableRootMotion(true, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathCombinedAnimation);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            //yield return new WaitForSeconds(1.6f);
            //m_animation.DisableRootMotion();
            //m_character.physics.simulateGravity = true;
            //m_animation.SetAnimation(0, m_info.deathLoopAnimation, true);
            //m_bodyCollider.enabled = true;
            //m_animation.EnableRootMotion(true, true);
            enabled = false;
            this.gameObject.SetActive(false);
            yield return null;
        }

        private void CustomDecideOnAttack()
        {
            //single attack is AttackCache[0]
            //Combo Attack is AttackCache[1]

            int attackChance = UnityEngine.Random.Range(0, 100);
            if(attackChance < 61)
            {
                //Choose AttackCache[0]
                m_statsData.SetAttackData(m_info.heavySlashAttackData);
                m_attackDecider.ForcedDecideOnAttack(0);
                m_currentAttack = m_attackCache[0];
                m_currentAttackRange = m_attackRangeCache[0];
                Debug.Log("Chosen Attack: " + m_attackCache[0]);
            }
            else
            {
                //Choose AttackCache[1]
                m_statsData.SetAttackData(m_info.SlashComboAttackData);
                m_attackDecider.ForcedDecideOnAttack(1);
                m_currentAttack = m_attackCache[1];
                m_currentAttackRange = m_attackRangeCache[1];
                Debug.Log("Chosen Attack: " + m_attackCache[1]);
            }

            Debug.Log("CSD Damage: " + m_statsData.damage);
            Debug.Log("Chance attack percent value: " + attackChance);
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
            m_hurtbox.enabled = true;
            switch (m_attack)
            {
                case Attack.SingleAttack:
                    m_attackHandle.ExecuteAttack(m_info.singleAttack.animation, m_info.idleAnimation.animation);
                    break;
                case Attack.ComboAttack:
                    StartCoroutine(AttackRoutine());
                    break;
            }
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.comboAttack.animation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation.animation, true, 0).TimeScale = 5f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.comboAttack.animation);
            m_hurtbox.enabled = false;
            if (!IsFacingTarget())
                CustomTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.Chasing);
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
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < (Vector2.Distance(m_targetInfo.position, transform.position) > attackRange ?  1 : attackRange) ? true : false;
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
                            m_bodyCollider.enabled = false;
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
                    m_stateHandle.Wait(State.Cooldown);
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(/*m_currentAttackRange*/ m_attackRangeCache[0], m_currentAttack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else
                        {
                            m_agent.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
                        }
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
                    m_attackDecider.hasDecidedOnAttack = false;
                    ChooseAttack();
                    if (m_attackDecider.hasDecidedOnAttack /*&& !ShotBlocked()*/)
                    {
                        m_agent.Stop();
                        m_stateHandle.SetState(State.Attacking);
                    }
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

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_bodyCollider.enabled = false;
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
    }
}