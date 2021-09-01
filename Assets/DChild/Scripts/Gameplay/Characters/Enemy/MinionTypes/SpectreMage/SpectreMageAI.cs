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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SpectreMage")]
    public class SpectreMageAI : CombatAIBrain<SpectreMageAI.Info>
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_attackOrbAnimation;
            public string attackOrbAnimation => m_attackOrbAnimation;
            [SerializeField]
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_counterFlinchAnimation;
            public string counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fadeInAnimation;
            public string fadeInAnimation => m_fadeInAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fadeOutAnimation;
            public string fadeOutAnimation => m_fadeOutAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chargeEvent;
            public string chargeEvent => m_chargeEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_fireEvent;
            public string fireEvent => m_fireEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_hitEvent;
            public string hitEvent => m_hitEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
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
            Attack,
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
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
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
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_floorSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("ProjectileInfo")]
        private List<Transform> m_projectilePoints;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        private ProjectileLauncher m_projectileLauncher;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;
        private Vector2 m_startPos;

        private float m_currentCD;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isDetecting;

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Chasing);
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

        private bool TargetBlocked()
        {
            Vector2 wat = m_character.centerMass.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //if (m_flinchHandle.gameObject.activeSelf)
            //{
            //}
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            //m_flinchHandle.gameObject.SetActive(false);
            if (m_flinchHandle.m_autoFlinch && m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_agent.Stop();
                StopAllCoroutines();
                if (m_executeMoveCoroutine != null)
                {
                    StopCoroutine(m_executeMoveCoroutine);
                    m_executeMoveCoroutine = null;
                }
                m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
                if (m_targetInfo.isValid)
                {
                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                    }

                    Debug.Log("FLINCH NOW");
                    StartCoroutine(FlinchRoutine());
                }
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_agent.Stop();
            m_hitbox.gameObject.SetActive(false);
            m_selfCollider.SetActive(false);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            Debug.Log("FLINCH ANIMATION");
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.fadeOutAnimation, false);
            Debug.Log("fadeOutAnimation");
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeOutAnimation);
            yield return new WaitForSeconds(2);
            var random = UnityEngine.Random.Range(0, 2);
            transform.position = new Vector2(m_targetInfo.position.x + (IsFacingTarget() ? -5 : 5), m_targetInfo.position.y);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
            Debug.Log("fadeInAnimation");
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            Debug.Log("idleAnimation");
            m_flinchHandle.m_autoFlinch = true;
            m_hitbox.gameObject.SetActive(true);
            m_selfCollider.SetActive(true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator CounterFlinchRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.counterFlinchAnimation, false).TrackTime = .5f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.counterFlinchAnimation);
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    if (!m_targetInfo.isValid)
        //    {
        //        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
        //            m_animation.SetEmptyAnimation(0, 0).MixDuration = 0;

        //        m_stateHandle.ApplyQueuedState();
        //    }
        //}
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
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                //StopAllCoroutines();
                if (m_executeMoveCoroutine != null)
                {
                    StopCoroutine(m_executeMoveCoroutine);
                    m_executeMoveCoroutine = null;
                }
                m_agent.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
                m_enablePatience = false;
                m_isDetecting = false;
                m_stateHandle.SetState(State.ReturnToPatrol);
            }
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range)
                                  /*, new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)*/);
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
            m_selfCollider.SetActive(false);
            //m_bodyCollider.SetActive(true);
            //m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_agent.Stop();
            //m_character.physics.simulateGravity = true;
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
            switch (m_attack)
            {
                case Attack.Attack:
                    m_animation.EnableRootMotion(true, false);
                    //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    //LaunchProjectile();
                    StartCoroutine(AttackRoutine());
                    break;
            }
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            yield return new WaitForSeconds(1.25f);
            LaunchProjectile();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_selfCollider.SetActive(false);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
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
                newPos = new Vector2(m_targetInfo.position.x, /*GroundPosition().y + 20*/m_targetInfo.position.y);
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(/*m_targetInfo.position.y*/newPos.y - transform.position.y) < attackRange ? true : false;
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

            if (/*m_wallSensor.allRaysDetecting ||*/ m_selfSensor.isDetecting)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                //m_bodyCollider.SetActive(true);
                m_agent.Stop();
                rb2d.isKinematic = false;
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                return;
            }

            if (IsFacing(m_agent.hasPath && TargetBlocked() && !m_floorSensor.allRaysDetecting && !m_roofSensor.allRaysDetecting ? m_agent.segmentDestination : target))
            {
                if (!m_agent.hasPath && !m_wallSensor.allRaysDetecting && (m_floorSensor.allRaysDetecting || m_roofSensor.allRaysDetecting))
                {
                    //if (m_executeMoveCoroutine != null)
                    //{
                    //    StopCoroutine(m_executeMoveCoroutine);
                    //    m_executeMoveCoroutine = null;
                    //}
                    //StartCoroutine(DespawnRoutine());
                    //Vector3.MoveTowards(transform.position, m_targetInfo.position, m_info.move.speed);
                    m_bodyCollider.enabled = false;
                    m_agent.Stop();
                    Vector3 dir = (target - (Vector2)rb2d.transform.position).normalized;
                    rb2d.MovePosition(rb2d.transform.position + dir * movespeed * Time.fixedDeltaTime);

                    m_animation.SetAnimation(0, m_info.move.animation, true);
                    return;
                }
                
                m_bodyCollider.enabled = true;
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                m_agent.SetDestination(target);
                m_agent.Move(movespeed);

                m_animation.SetAnimation(0, m_info.move.animation, true);
            }
            else
            {
                m_turnState = State.Attacking;
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }
        #endregion

        private void LaunchProjectile()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            for (int i = 0; i < m_projectilePoints.Count; i++)
            {
                m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoints[i]);
                m_projectileLauncher.AimAt(m_targetInfo.position);
                m_projectileLauncher.LaunchProjectile();
            }
        }

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_startPos = transform.position;
            //Debug.Log("START OF SPECTRE MAGE");
            //m_spineListener.Subscribe(m_info.projectile.launchOnEvent, LaunchProjectile);
            //m_spineListener.Subscribe(m_info.chargeEvent, LaunchProjectile);
            //m_spineListener.Subscribe(m_info.fireEvent, LaunchProjectile);
            //m_spineListener.Subscribe(m_info.hitEvent, LaunchProjectile);
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
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoints[0]);
            m_projectileLauncher.SetProjectile(m_info.projectile.projectileInfo);

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Attack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
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
                    if (m_animation.GetCurrentAnimation(0).ToString() == m_info.patrol.animation)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    }
                    break;

                case State.Turning:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.flinchAnimation && m_animation.GetCurrentAnimation(0).ToString() != m_info.counterFlinchAnimation)
                    {
                        m_stateHandle.Wait(m_turnState);
                        if (m_executeMoveCoroutine != null)
                        {
                            StopCoroutine(m_executeMoveCoroutine);
                            m_executeMoveCoroutine = null;
                        }
                        m_agent.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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

            if (m_enablePatience && !m_selfSensor.isDetecting)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_bodyCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }
    }
}