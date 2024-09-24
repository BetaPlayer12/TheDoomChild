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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Eyebat")]
    public class EyebatAI : CombatAIBrain<EyebatAI.Info>, ISummonedEnemy
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
            private SimpleAttackInfo m_attackMove = new SimpleAttackInfo();
            public SimpleAttackInfo attackMove => m_attackMove;
            [SerializeField]
            private SimpleAttackInfo m_attackLazer = new SimpleAttackInfo();
            public SimpleAttackInfo attackLazer => m_attackLazer;
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
            private BasicAnimationInfo m_sleepingAnimation;
            public BasicAnimationInfo sleepingAnimation => m_sleepingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_awakenAnimation;
            public BasicAnimationInfo awakenAnimation => m_awakenAnimation;
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
            private BasicAnimationInfo m_swoopAnimation;
            public BasicAnimationInfo swoopAnimation => m_swoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_swoopStartAnimation;
            public BasicAnimationInfo swoopStartAnimation => m_swoopStartAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_attackMove.SetData(m_skeletonDataAsset);
                m_attackLazer.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
                m_sleepingAnimation.SetData(m_skeletonDataAsset);
                m_awakenAnimation.SetData(m_skeletonDataAsset);

                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_swoopAnimation.SetData(m_skeletonDataAsset);
                m_swoopStartAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Sleeping,
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
            //Attack,
            Lazer,
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
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Lazer")]
        private LineRenderer m_lineRenderer;
        [SerializeField, TabGroup("Lazer")]
        private LineRenderer m_telegraphLineRenderer;
        [SerializeField, TabGroup("Lazer")]
        private EdgeCollider2D m_edgeCollider;
        [SerializeField, TabGroup("Lazer")]
        private GameObject m_muzzleFXGO;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleLoopFX;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleTelegraphFX;
        [SerializeField, TabGroup("Lazer")]
        private Transform m_lazerOrigin;
        [SerializeField, TabGroup("Audio")]
        private EventSounds m_lazerAudio;

        private List<Vector2> m_Points;
        private IEnumerator m_aimRoutine;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private ProjectileLauncher m_projectileLauncher;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private float m_currentCD;
        private bool m_isDetecting;
        private Vector2 m_lastTargetPos;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;
        [SerializeField]
        private bool m_isSleeping;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_currentAttack = Attack.Attack;
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
                    m_stateHandle.SetState(State.Detect);
                }
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
            }
        }

        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            //Lefix commento 3====D----
            enabled = false;
            m_targetInfo = target;
            m_isDetecting = true;
            transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y + 10f);
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

        //public void SetAI(AITargetInfo targetInfo)
        //{
        //    m_isDetecting = true;
        //    m_targetInfo = targetInfo;
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //}

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation || m_stateHandle.currentState == State.Cooldown)
            {
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_flinchHandle.m_autoFlinch = true;
                m_agent.Stop();
                ResetLaser();
                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_lazerAudio.enabled = false;
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
            enabled = false;
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            ResetLaser();
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.SetState(State.ReturnToPatrol);
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
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Attack, m_info.attack.range),*/
                                    new AttackInfo<Attack>(Attack.Lazer, m_info.attackLazer.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            if (m_isSleeping)
            {
                m_animation.SetAnimation(0, m_info.awakenAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
            }
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            if (!IsFacingTarget())
                CustomTurn();
            m_isSleeping = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
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
            base.OnDestroyed(sender, eventArgs);
            
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            ResetLaser();
            m_agent.Stop();
            m_lazerAudio.enabled = false;
            m_bodycollider.enabled = false;
            m_selfCollider.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            //m_muzzleLoopFX.Stop();
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
        }

        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_bodycollider.enabled = true;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            switch (/*m_attack*/ m_currentAttack)
            {
                //case Attack.Attack:
                //    StartCoroutine(AttackRoutine());
                //    break;
                case Attack.Lazer:
                    m_lastTargetPos = m_targetInfo.position;
                    StartCoroutine(LazerRoutine());
                    break;
            }
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.swoopStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swoopStartAnimation);
            m_animation.SetAnimation(0, m_info.swoopAnimation, false);
            m_character.physics.SetVelocity(25 * transform.localScale.x, -25f);
            yield return new WaitForSeconds(.25f);
            m_agent.Stop();
            m_character.physics.SetVelocity(15 * transform.localScale.x, 0);
            m_animation.SetAnimation(0, m_info.attack.animation, false).AnimationStart = 0.25f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_animation.animationState.GetCurrent(0).MixDuration = 0;
            m_bodycollider.enabled = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator AimRoutine()
        {
            while (true)
            {
                m_telegraphLineRenderer.SetPosition(0, m_telegraphLineRenderer.transform.position);
                m_lineRenderer.SetPosition(0, m_lineRenderer.transform.position);
                m_lineRenderer.SetPosition(1, m_lineRenderer.transform.position);
                yield return null;
            }
        }

        private IEnumerator TelegraphLineRoutine()
        {
            //float timer = 0;
            m_muzzleTelegraphFX.Play();
            m_telegraphLineRenderer.useWorldSpace = true;
            m_lineRenderer.useWorldSpace = true;
            m_telegraphLineRenderer.SetPosition(1, ShotPosition());
            var timerOffset = m_telegraphLineRenderer.startWidth;
            while (m_telegraphLineRenderer.startWidth > 0)
            {
                m_telegraphLineRenderer.startWidth -= Time.deltaTime * timerOffset;
                yield return null;
            }
            yield return null;
        }

        private IEnumerator LazerRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            m_lazerAudio.enabled = true;
            StartCoroutine(TelegraphLineRoutine());
            StartCoroutine(m_aimRoutine);
            yield return new WaitForSeconds(1f);
            StopCoroutine(m_aimRoutine);
            //m_muzzleLoopFX.Play();
            m_lineRenderer.SetPosition(1, m_telegraphLineRenderer.GetPosition(1));
            //var hitPointFX = this.InstantiateToScene(m_muzzleLoopFX.gameObject, m_telegraphLineRenderer.GetPosition(1), Quaternion.identity);
            //hitPointFX.GetComponent<ParticleFX>().Play();

            var muzzleLoopFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_muzzleFXGO.gameObject);
            muzzleLoopFX.transform.position = m_telegraphLineRenderer.GetPosition(1);
            //LaunchProjectile();
            //var muzzleFX = this.InstantiateToScene(m_muzzleFXGO, m_muzzleLoopFX.transform.position, Quaternion.identity);
            var muzzleFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_muzzleFXGO.gameObject);
            muzzleFX.transform.position = muzzleLoopFX.transform.position;
            //m_muzzleLoopFX.Stop();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                var pos = m_lineRenderer.GetPosition(i) - m_edgeCollider.transform.position;
                pos = new Vector2(m_character.facing == HorizontalDirection.Right ? pos.x : -pos.x, pos.y);
                if (i > 0)
                {
                    pos = pos * 0.7f;
                }
                m_Points.Add(pos);
            }
            m_edgeCollider.points = m_Points.ToArray();
            yield return new WaitForSeconds(.2f);
            //hitPointFX.GetComponent<ParticleFX>().Stop();
            //Destroy(hitPointFX.gameObject);
            ResetLaser();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_lazerAudio.enabled = false;
            m_animation.animationState.GetCurrent(0).MixDuration = 0;
            m_bodycollider.enabled = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void ResetLaser()
        {
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.SetPosition(0, Vector3.zero);
            m_lineRenderer.SetPosition(1, Vector3.zero);
            m_telegraphLineRenderer.SetPosition(0, Vector3.zero);
            m_telegraphLineRenderer.SetPosition(1, Vector3.zero);
            m_telegraphLineRenderer.startWidth = 1;
            m_Points.Clear();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                m_Points.Add(Vector2.zero);
            }
            m_edgeCollider.points = m_Points.ToArray();
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                //m_targetPointIK.transform.position = m_lastTargetPos;
                m_projectileLauncher.AimAt(m_lastTargetPos);
                m_projectileLauncher.LaunchProjectile();
            }
        }
        #endregion

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            while (!inRange || TargetBlocked() || m_environmentCollider.IsTouchingLayers(DChildUtility.GetEnvironmentMask()))
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < attackRange/*1*/ ? true : false;
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

        private void DynamicMovement(Vector2 target, float moveSpeed)
        {
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

        private Vector2 ShotPosition()
        {
            Vector2 startPoint = m_lazerOrigin.position;
            Vector2 direction = (m_targetInfo.position - startPoint).normalized;

            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
            Debug.DrawRay(startPoint, direction);
            return hit.point;
        }
        #endregion

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
            var startState = m_info.patrol.animation;
            if (m_isSleeping)
            {
                startState = m_info.sleepingAnimation.animation;
            }
            else
            {
                startState = m_info.patrol.animation;
            }
            m_animation.SetAnimation(0, startState, true);
            m_animation.DisableRootMotion();
            m_bodycollider.enabled = false;
            m_aimRoutine = AimRoutine();
            m_startPos = transform.position;
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            var startState = State.Patrol;
            if (m_isSleeping)
            {
                startState = State.Sleeping;
            }
            else
            {
                startState = State.Patrol;
            }
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_muzzleLoopFX.transform);
            m_stateHandle = new StateHandle<State>(startState, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_Points = new List<Vector2>();

            m_attackCache = new List<Attack>();
            AddToAttackCache(/*Attack.Attack,*/ Attack.Lazer);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attack.range, m_info.attackLazer.range);
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
                case State.Sleeping:
                    m_agent.Stop();
                    m_animation.SetAnimation(0, m_info.sleepingAnimation, true);/*
                    if (IsTargetInRange(m_info.attackLazer.range))
                    {
                        m_stateHandle.OverrideState(State.Detect);
                    }*/
                    break;
                case State.ReturnToPatrol:
                    //if (IsFacing(m_startPos))
                    //{
                        if (Vector2.Distance(m_startPos, transform.position) > 10f)
                        {
                            //var rb2d = GetComponent<Rigidbody2D>();
                            //m_bodycollider.enabled = false;
                            //m_agent.Stop();
                            //Vector3 dir = (m_startPos - (Vector2)m_rigidbody2D.transform.position).normalized;
                            //Debug.Log("Return to Patrol Direction: " + dir);
                            //m_rigidbody2D.MovePosition(m_rigidbody2D.transform.position + dir * m_info.move.speed * Time.fixedDeltaTime);
                            //m_animation.SetAnimation(0, m_info.patrol.animation, true);
                            m_turnState = State.ReturnToPatrol;
                            DynamicMovement(m_startPos, m_info.move.speed);
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.Patrol);
                        }
                    //}
                    //else
                    //{
                    //    m_turnState = State.ReturnToPatrol;
                    //    m_stateHandle.SetState(State.Turning);
                    //}
                    break;

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idleAnimation.animation : m_info.move.animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }

                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_currentAttackRange, m_currentAttack));
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
                            var chosenMoveAnim = UnityEngine.Random.Range(0, 50) > 10 ? m_info.idleAnimation.animation : m_info.move.animation;
                            m_animation.SetAnimation(0, chosenMoveAnim, true);
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
                                m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
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
                    //m_attackDecider.DecideOnAttack();
                    m_attackDecider.hasDecidedOnAttack = false;
                    ChooseAttack();
                    m_currentAttack = Attack.Lazer;
                    m_currentAttackRange = m_info.attackLazer.range;
                    if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
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
                        if (m_isSleeping)
                        {
                            m_stateHandle.SetState(State.Sleeping);
                        }
                        else
                        {
                            m_stateHandle.SetState(State.Patrol);
                        }
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