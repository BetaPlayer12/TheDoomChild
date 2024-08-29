using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/JellyfishBlackBlood")]
    public class JellyfishBlackBloodAI : CombatAIBrain<JellyfishBlackBloodAI.Info>
    {
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

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation = new BasicAnimationInfo();

            public BasicAnimationInfo idleAnimation => m_idleAnimation;

            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();

            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();

            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;

            public SimpleProjectileAttackInfo projectile => m_projectile;

            [SerializeField]
            private GameObject m_blackBloodPuddle;

            public GameObject blackBloodPuddle => m_blackBloodPuddle;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            ReturnToPatrol,
            Patrol,
            Cooldown,

            //Turning,
            Attacking,

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
        private IsolatedObjectPhysics2D m_objectPhysics;

        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;

        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodycollider;

        [SerializeField, TabGroup("Reference")]
        private Collider2D m_explosionBB;

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

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private ProjectileLauncher m_projectileLauncher;
        private ProjectileLauncher m_projectileLauncherOnDeath;

        private float m_currentCD;
        private bool m_isDetecting;
        private Vector2 m_lastTargetPos;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.gameObject.SetActive(true);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
        }

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
            }
        }

        public void SetAI(AITargetInfo targetInfo)
        {
            m_isDetecting = true;
            m_targetInfo = targetInfo;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation || m_stateHandle.currentState == State.Cooldown)
            {
                m_flinchHandle.m_autoFlinch = true;
                m_agent.Stop();
                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_stateHandle.Wait(State.ReevaluateSituation);
                //StopAllCoroutines();
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
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
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
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
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();

            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }

            m_agent.Stop();
            m_bodycollider.enabled = false;
            m_selfCollider.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(DeathRoutine());
            //m_muzzleLoopFX.Stop();
        }

        private IEnumerator DeathRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation.animation);
            var targetground = new Vector2(GroundPosition().x, GroundPosition().y);
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.blackBloodPuddle);
            instance.SpawnAt(targetground, Quaternion.identity);
            yield return null;
        }

        #region Attack

        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_bodycollider.enabled = true;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            switch (/*m_attack*/ m_attackDecider.chosenAttack.attack)
            {
                case Attack.Attack:
                    m_lastTargetPos = m_targetInfo.position;
                    StartCoroutine(AttackRoutine());
                    break;
            }
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_bodycollider.enabled = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                m_projectileLauncher.AimAt(m_character.centerMass.position);
                m_projectileLauncher.LaunchProjectile();
            }
        }

        #endregion Attack

        #region Movement

        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            bool inRange = false;
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            while (!inRange || TargetBlocked())
            {
                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < attackRange/*1*/ ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
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
            m_bodycollider.enabled = true;
            var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
            var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
            m_agent.SetDestination(target);
            m_agent.Move(moveSpeed);
        }

        #endregion Movement

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_animation.DisableRootMotion();
            m_bodycollider.enabled = false;
            m_startPos = transform.position;

            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchProjectile);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();

            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            //m_deathHandle.BodyDestroyed += OnBodyDestroyed;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_character.centerMass);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }

        private void OnBodyDestroyed(object sender, DeathHandle.DisposingEventArgs eventArgs)
        {
            var targetground = new Vector2(GroundPosition().x, GroundPosition().y);
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.blackBloodPuddle);
            instance.SpawnAt(targetground, Quaternion.identity);
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                    break;

                case State.ReturnToPatrol:
                    if (Vector2.Distance(m_startPos, transform.position) > 10f)
                    {
                        DynamicMovement(m_startPos, m_info.move.speed);
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.Patrol);
                    }
                    //if (Vector2.Distance(m_startPos, transform.position) > 10f)
                    //{
                    //    var rb2d = GetComponent<Rigidbody2D>();
                    //    m_bodycollider.enabled = false;
                    //    m_agent.Stop();
                    //    Vector3 dir = (m_startPos - (Vector2)rb2d.transform.position).normalized;
                    //    rb2d.MovePosition(rb2d.transform.position + dir * m_info.move.speed * Time.fixedDeltaTime);
                    //    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    //}
                    //else
                    //{
                    //    m_stateHandle.OverrideState(State.Patrol);
                    //}
                    break;

                case State.Patrol:
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idleAnimation.animation : m_info.move.animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }

                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_attackDecider.chosenAttack.range, m_attackDecider.chosenAttack.attack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;

                case State.Cooldown:
                    if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                    {
                        if (m_objectPhysics.velocity.y > 1 || m_objectPhysics.velocity.y < -1)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                        else
                        {
                            m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        }
                        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        CalculateRunPath();
                        m_agent.Move(m_info.move.speed);
                    }
                    else
                    {
                        m_agent.Stop();
                        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                        m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
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
                    m_attackDecider.hasDecidedOnAttack = false;
                    m_attackDecider.DecideOnAttack();
                    if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                    {
                        m_agent.Stop();
                        m_stateHandle.SetState(State.Attacking);
                    }
                    break;

                case State.ReevaluateSituation:

                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else

                    {
                        m_stateHandle.SetState(State.Patrol);
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
    }
}