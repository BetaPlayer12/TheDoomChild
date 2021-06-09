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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Eyebat")]
    public class EyebatAI : CombatAIBrain<EyebatAI.Info>
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
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swoopAnimation;
            public string swoopAnimation => m_swoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swoopStartAnimation;
            public string swoopStartAnimation => m_swoopStartAnimation;

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
#endif
            }
        }

        private enum State
        {
            Detect,
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
        private Hitbox m_hitbox;
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
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("Lazer")]
        private LineRenderer m_lineRenderer;
        [SerializeField, TabGroup("Lazer")]
        private EdgeCollider2D m_edgeCollider;
        [SerializeField, TabGroup("Lazer")]
        private GameObject m_muzzleFXGO;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleLoopFX;
        //[SerializeField, TabGroup("Lazer")]
        //private Gradient m_telegraphGradient;
        //[SerializeField, TabGroup("Lazer")]
        //private Color m_lazerColor;

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
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_lastTargetPos;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_currentAttack = Attack.Attack;
            m_flinchHandle.m_autoFlinch = true;
            m_flinchHandle.gameObject.SetActive(true);
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private void VelocityTurn()
        {
            if (m_character.physics.velocity.x > 10)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (m_character.physics.velocity.x < -10)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
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

        public void SetAI(AITargetInfo targetInfo)
        {
            m_isDetecting = true;
            m_targetInfo = targetInfo;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_agent.Stop();
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                StopAllCoroutines();
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idle2Animation, true);
            m_stateHandle.OverrideState(State.Cooldown); 
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.idle2Animation, true);
            //m_stateHandle.ApplyQueuedState();
        }
        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, LayerMask.GetMask("Environment"));
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
                StopAllCoroutines();
                m_agent.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
                m_enablePatience = false;
                m_isDetecting = false;
                m_stateHandle.SetState(State.Patrol);
            }
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
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
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
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.SetPosition(0, Vector3.zero);
            m_lineRenderer.SetPosition(1, Vector3.zero);
            m_Points.Clear();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                m_Points.Add(Vector2.zero);
            }
            m_edgeCollider.points = m_Points.ToArray();
            StopAllCoroutines();
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
        }

        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_bodycollider.enabled = true;
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
                m_lineRenderer.SetPosition(0, m_lineRenderer.transform.position);
                yield return null;
            }
        }

        private IEnumerator LazerRoutine()
        {
            m_muzzleLoopFX.Play();
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            //m_lineRenderer.startWidth = .1f;
            //m_lineRenderer.startColor = m_telegraphColor;
            //m_lineRenderer.endColor = m_telegraphColor;
            m_lineRenderer.useWorldSpace = true;
            m_lineRenderer.SetPosition(1, ShotPosition());
            var hitPointFX = this.InstantiateToScene(m_muzzleLoopFX.gameObject, ShotPosition(), Quaternion.identity);
            hitPointFX.GetComponent<ParticleFX>().Play();
            StartCoroutine(m_aimRoutine);
            yield return new WaitForSeconds(1f);
            StopCoroutine(m_aimRoutine);
            //LaunchProjectile();
            //m_lineRenderer.startWidth = .5f;
            //m_lineRenderer.startColor = m_lazerColor;
            //m_lineRenderer.endColor = m_lazerColor;
            var muzzleFX = this.InstantiateToScene(m_muzzleFXGO, m_muzzleLoopFX.transform.position, Quaternion.identity);
            m_muzzleLoopFX.Stop();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                var pos = m_lineRenderer.GetPosition(i) - m_edgeCollider.transform.position;
                pos = new Vector2(m_character.facing == HorizontalDirection.Right ? pos.x : -pos.x, pos.y);
                m_Points.Add(pos);
            }
            m_edgeCollider.points = m_Points.ToArray();
            yield return new WaitForSeconds(.2f);
            hitPointFX.GetComponent<ParticleFX>().Stop();
            Destroy(hitPointFX.gameObject);
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.SetPosition(0, Vector3.zero);
            m_lineRenderer.SetPosition(1, Vector3.zero);
            m_Points.Clear();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                m_Points.Add(Vector2.zero);
            }
            m_edgeCollider.points = m_Points.ToArray();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.animationState.GetCurrent(0).MixDuration = 0;
            m_bodycollider.enabled = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
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
            while (!inRange)
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < attackRange/*1*/ ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                DynamicMovement(new Vector2(m_targetInfo.position.x, m_targetInfo.position.y), moveSpeed);
                yield return null;
            }
            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target, float moveSpeed)
        {
            if (ShotBlocked())
            {
                VelocityTurn();
            }
            else
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
            }

            var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
            var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;

            bool isCloseToGround = false;
            bool isCloseToRoof = false;

            if (m_targetInfo.position.y < transform.position.y && m_groundSensor.allRaysDetecting)
            {
                isCloseToGround = Vector2.Distance(transform.position, GroundPosition()) < 2.5f ? true : false;
            }
            if (m_targetInfo.position.y > transform.position.y && m_roofSensor.allRaysDetecting)
            {
                isCloseToRoof = Vector2.Distance(transform.position, RoofPosition()) < 5f ? true : false;
            }

            if (!m_selfSensor.isDetecting && !isCloseToGround && !isCloseToRoof /*&& !GetComponentInChildren<NavigationTracker>().IsCurrentDestination(transform.position)*/)
            {
                if (Mathf.Abs(m_targetInfo.position.y - transform.position.y) > 5f /*&& !m_groundSensor.isDetecting*/)
                {
                    m_agent.SetDestination(new Vector2(transform.position.x, target.y/* + 5*/));
                }
                else
                {
                    m_agent.SetDestination(target);
                }
                //m_agent.SetDestination(target);
                m_agent.Move(moveSpeed);

                if (m_character.physics.velocity.y > .25f || m_character.physics.velocity.y < -.25f)
                {
                    m_animation.SetAnimation(0, m_info.idleAnimation, true)/*.TimeScale = 2*/;
                }
                else
                {
                    m_animation.SetAnimation(0, m_info.patrol.animation, true)/*.TimeScale = 2*/;
                }
            }
            else
            {
                m_agent.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }

            //if (IsFacingTarget())
            //{
            //}
            //else
            //{
            //    m_turnState = State.Attacking;
            //    m_stateHandle.OverrideState(State.Turning);
            //}
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            return hit.point;
        }

        private Vector2 RoofPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1000, LayerMask.GetMask("Environment"));
            return hit.point;
        }

        private bool ShotBlocked()
        {
            Vector2 wat = m_selfSensor.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Environment", "Player"));
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private Vector2 ShotPosition()
        {
            Vector2 startPoint = m_selfSensor.transform.position;
            Vector2 direction = (m_targetInfo.position - startPoint).normalized;

            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, LayerMask.GetMask("Environment"));
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
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_animation.DisableRootMotion();
            m_bodycollider.enabled = false;
            m_aimRoutine = AimRoutine();
            //m_selfCollider.SetActive(false);
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
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_muzzleLoopFX.transform);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
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
                    m_flinchHandle.m_autoFlinch = false;
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

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    if (m_character.physics.velocity.y > 1 || m_character.physics.velocity.y < -1)
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    }
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_agent.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    StartCoroutine(ExecuteMove(m_currentAttackRange, m_currentAttack));
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
                            if (m_character.physics.velocity.y > 1 || m_character.physics.velocity.y < -1)
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.patrol.animation, true);
                            }
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
                        m_flinchHandle.m_autoFlinch = false;
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
                        m_stateHandle.SetState(State.Patrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_hitbox.gameObject.SetActive(true);
            m_enablePatience = false;
            m_isDetecting = false;
        }

        public void ResetAI()
        {
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