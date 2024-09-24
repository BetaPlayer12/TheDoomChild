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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Imp")]
    public class ImpAI : CombatAIBrain<ImpAI.Info>
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
            private SimpleAttackInfo m_plantBomb = new SimpleAttackInfo();
            public SimpleAttackInfo plantBomb => m_plantBomb;
            [SerializeField]
            private SimpleAttackInfo m_plantQuickBomb = new SimpleAttackInfo();
            public SimpleAttackInfo plantQuickBomb => m_plantQuickBomb;
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathBeginAnimation;
            public string deathBeginAnimation => m_deathBeginAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallLoopAnimation;
            public string deathFallLoopAnimation => m_deathFallLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathHitFloorAnimation;
            public string deathHitFloorAnimation => m_deathHitFloorAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_laughAnimation;
            public string laughAnimation => m_laughAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_disappearing1Animation;
            public string disappearing1Animation => m_disappearing1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_disappearing2Animation;
            public string disappearing2Animation => m_disappearing2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_reappearing1Animation;
            public string reappearing1Animation => m_reappearing1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_reappearing2Animation;
            public string reappearing2Animation => m_reappearing2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_spawnBarrelEvent;
            public string spawnBarrelEvent => m_spawnBarrelEvent;

            [SerializeField]
            private GameObject m_barrel;
            public GameObject barrel => m_barrel;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_plantBomb.SetData(m_skeletonDataAsset);
                m_plantQuickBomb.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            ReturnToPatrol,
            Patrol,
            Cooldown,
            Turning,
            Despawned,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            PlantBomb,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private SkeletonAnimation m_skeletomAnimation;
        [SerializeField, TabGroup("Reference")]
        private IsolatedObjectPhysics2D m_objectPhysics2D;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_spriteMask;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
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
        [SerializeField, TabGroup("Spawn Points")]
        private Collider2D m_randomSpawnCollider;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        //private Attack m_currentAttack;
        //private float m_currentAttackRange;

        //private bool[] m_attackUsed;
        //private List<Attack> m_attackCache;
        //private List<float> m_attackRangeCache;

        private float m_currentCD;
        private bool m_isDetecting;
        private bool m_isDoingAction;
        private GameObject m_barrelCache;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            if (!m_isDoingAction)
            {
                m_stateHandle.OverrideState(State.Despawned);
                m_animation.DisableRootMotion();
                //m_stateHandle.ApplyQueuedState();
                StartCoroutine(DespawnRoutine(false));
            }
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
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.plantBomb.animation || m_animation.GetCurrentAnimation(0).ToString() == m_info.plantQuickBomb.animation)
            {
                m_isDoingAction = true;
                m_stateHandle.Wait(State.Cooldown);
                m_agent.Stop();
                StopAllCoroutines();
                StartCoroutine(FlinchRoutine());
            }
            //else
            //{
            //    if (m_animation.animationState.GetCurrent(1).IsComplete)
            //        m_animation.SetAnimation(1, m_info.flinchAnimation, false);
            //}
        }

        private IEnumerator FlinchRoutine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_hitbox.Disable();
            m_bodyCollider.enabled = false;
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_hitbox.Enable();
            m_isDoingAction = false;
            if (m_animation.animationState.GetCurrent(0).IsComplete)
            {
                var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idle1Animation : m_info.idle2Animation;
                m_animation.SetAnimation(0, chosenMoveAnim, true).MixDuration = 0;
            }
            StartCoroutine(DespawnRoutine(true));
            yield return null;
        }

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    m_animation.DisableRootMotion();
        //    m_stateHandle.OverrideState(State.Cooldown);
        //}

        private void SpawnBarrel()
        {
            var barrelPos = new Vector3(transform.position.x + (2 * transform.localScale.x), transform.position.y - 5, 0);
            var barrel = Instantiate(m_info.barrel, barrelPos, Quaternion.identity);
            barrel.GetComponent<ImpBarrel>().SetImp(this.gameObject.GetComponent<ImpAI>());
            m_barrelCache = barrel;
        }

        public void Laugh()
        {
            if (!m_isDoingAction)
            {
                m_isDoingAction = true;
                m_stateHandle.Wait(State.Cooldown);
                StopAllCoroutines();
                //StartCoroutine(LaughRoutine());
                StartCoroutine(ReappearRoutine());

            }
        }

        //private IEnumerator LaughRoutine()
        //{
        //    if (m_stateHandle.currentState == State.Despawned)
        //    {
        //        m_animation.SetAnimation(0, m_info.reappearing1Animation, false);
        //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.reappearing1Animation);
        //        m_hitbox.Enable();
        //    }
        //    m_animation.SetAnimation(0, m_info.laughAnimation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.laughAnimation);
        //    m_isDoingAction = false;
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //    yield return null;
        //}

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
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
            m_agent.Stop();
            if (m_animation.animationState.GetCurrent(0).IsComplete)
            {
                var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idle1Animation : m_info.idle2Animation;
                m_animation.SetAnimation(0, chosenMoveAnim, true);
            }
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_skeletomAnimation.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            m_spriteMask.SetActive(true);
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.PlantBomb, m_info.plantBomb.range)
                                  /*, new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private Vector3 RandomTeleportPoint(Vector3 transformPos)
        {
            Vector3 randomPos = transformPos;
            while (Vector2.Distance(transformPos, randomPos) >= UnityEngine.Random.Range(25f, 50f))
            {
                randomPos = m_randomSpawnCollider.bounds.center + new Vector3(
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.x,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.y,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.z);
            }
            return randomPos;
        }

        private IEnumerator DetectRoutine()
        {
            m_hitbox.Disable();
            m_skeletomAnimation.maskInteraction = SpriteMaskInteraction.None;
            m_spriteMask.SetActive(false);
            var reappearAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.reappearing1Animation : m_info.reappearing2Animation;
            m_animation.SetAnimation(0, reappearAnim, false).TimeScale = 1.5f;
            yield return new WaitForAnimationComplete(m_animation.animationState, reappearAnim);
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            StartCoroutine(DespawnRoutine(true));
            yield return null;
        }

        private IEnumerator DespawnRoutine(bool willAttack)
        {
            var disappearAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.disappearing1Animation : m_info.disappearing2Animation;
            m_animation.SetAnimation(0, disappearAnim, false).TimeScale = 1.5f;
            m_agent.Stop();
            m_hitbox.Disable();
            m_selfCollider.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, disappearAnim);
            m_spriteMask.SetActive(false);
            var random = UnityEngine.Random.Range(0, 2);
            //transform.position = new Vector2(m_targetInfo.position.x + (random == 0 ? 10 : -10), GroundPosition().y + 20);
            yield return new WaitForSeconds(0.75f);
            transform.position = new Vector2(RandomTeleportPoint(/*transform.position*/m_targetInfo.position).x, m_targetInfo.position.y + 10);
            yield return new WaitForSeconds(0.25f);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_spriteMask.SetActive(true);
            m_selfCollider.SetActive(true);
            m_hitbox.Enable();
            if (willAttack)
            {
                m_animation.SetAnimation(0, m_info.plantQuickBomb.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.plantQuickBomb.animation);
            }
            else
            {
                var reappearAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.reappearing1Animation : m_info.reappearing2Animation;
                m_animation.SetAnimation(0, reappearAnim, false).TimeScale = 1.5f;
                yield return new WaitForAnimationComplete(m_animation.animationState, reappearAnim);
            }
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ReappearRoutine()
        {
            if (m_spriteMask.activeSelf)
            {
                var disappearAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.disappearing1Animation : m_info.disappearing2Animation;
                m_animation.SetAnimation(0, disappearAnim, false).TimeScale = 1.5f;
                m_hitbox.Disable();
                yield return new WaitForAnimationComplete(m_animation.animationState, disappearAnim);
            }
            var random = UnityEngine.Random.Range(0, 2);
            //transform.position = new Vector2(m_targetInfo.position.x + (random == 0 ? 10 : -10), GroundPosition().y + 20);
            transform.position = new Vector2(RandomTeleportPoint(/*transform.position*/m_targetInfo.position).x, m_targetInfo.position.y + 10);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            //yield return new WaitForSeconds(1f);
            var reappearAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.reappearing1Animation : m_info.reappearing2Animation;
            m_animation.SetAnimation(0, reappearAnim, false).TimeScale = 1.5f;
            yield return new WaitForAnimationComplete(m_animation.animationState, reappearAnim);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_isDoingAction = false;
            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
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
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_animation.DisableRootMotion();
            var rb2d = GetComponent<Rigidbody2D>();
            rb2d.isKinematic = false;
            m_bodyCollider.enabled = true;
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitbox.Disable();
            m_animation.SetEmptyAnimation(0, 0);
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_agent.Stop();
            Debug.Log("DIE HERE");
            m_animation.SetAnimation(0, m_info.deathBeginAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathBeginAnimation);
            m_objectPhysics2D.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathFallLoopAnimation, true);
            m_bodyCollider.enabled = true;
            yield return new WaitUntil(() => m_bodyCollider.IsTouchingLayers(DChildUtility.GetEnvironmentMask()));
            m_animation.SetAnimation(0, m_info.deathHitFloorAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathHitFloorAnimation);
            enabled = false;
            this.gameObject.SetActive(false);
            yield return null;
        }

        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            switch (m_attack)
            {
                case Attack.PlantBomb:
                    m_animation.EnableRootMotion(true, true);
                    m_attackHandle.ExecuteAttack(m_info.plantBomb.animation, m_info.idle1Animation);
                    break;
            }
        }

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            while (!inRange)
            {
                newPos = new Vector2(m_targetInfo.position.x, /*GroundPosition().y + 20*/m_targetInfo.position.y + 10);
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < attackRange ? true : false;
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

        private void DynamicMovement(Vector2 target, float movespeed)
        {
            var rb2d = GetComponent<Rigidbody2D>();
            m_agent.SetDestination(target);
            if (IsFacing(target))
            {
                m_agent.Move(movespeed);
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
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_spineEventListener.Subscribe(m_info.spawnBarrelEvent, SpawnBarrel);
            m_hitbox.Disable();
            m_startPos = transform.position;
            //m_randomSpawnCollider.transform.SetParent(null);
            //m_selfCollider.SetActive(false);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            //m_deathHandle.SetAnimation(m_info.deathBeginAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
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

                case State.Idle:
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idle1Animation : m_info.idle2Animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }
                    m_agent.Stop();
                    break;

                case State.ReturnToPatrol:
                    if (Vector2.Distance(m_startPos, transform.position) > 10f)
                    {
                        m_turnState = State.ReturnToPatrol;
                        DynamicMovement(m_startPos, m_info.move.speed);
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.Patrol);
                    }
                    break;

                case State.Patrol:
                    //if (m_animation.GetCurrentAnimation(0).ToString() == m_info.patrol.animation)
                    //{
                    //}
                    m_turnState = State.ReevaluateSituation;
                    m_animation.DisableRootMotion();
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idle1Animation : m_info.idle2Animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }
                    m_turnHandle.Execute();
                    break;

                case State.Despawned:
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idle1Animation : m_info.idle2Animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_info.plantBomb.range, Attack.PlantBomb));
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
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.targetDistanceTolerance * 4)
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                            DynamicMovement(m_targetInfo.position, m_info.move.speed);
                        }
                        else if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance * 2)
                        {
                            m_agent.Stop();
                            if (m_animation.animationState.GetCurrent(0).IsComplete)
                            {
                                var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idle1Animation : m_info.idle2Animation;
                                m_animation.SetAnimation(0, chosenMoveAnim, true).TimeScale = 1;
                            }
                        }
                    }

                    if (m_currentCD <= m_info.attackCD /*&& m_barrelCache != null*/)
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
                    m_agent.Stop();
                    m_hitbox.Enable();
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
            m_isDetecting = false;
            m_isDoingAction = false;
            m_hitbox.Enable();
            m_skeletomAnimation.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            m_spriteMask.SetActive(true);
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            ResetAI();
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}