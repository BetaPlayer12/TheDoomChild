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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SpectreIce")]
    public class SpectreIceAI : CombatAIBrain<SpectreIceAI.Info>
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
            [SerializeField]
            private BasicAnimationInfo m_attackOrbAnimation;
            public BasicAnimationInfo attackOrbAnimation => m_attackOrbAnimation;
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
            private BasicAnimationInfo m_fadeInAnimation;
            public BasicAnimationInfo fadeInAnimation => m_fadeInAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fadeOutAnimation;
            public BasicAnimationInfo fadeOutAnimation => m_fadeOutAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;
            [SerializeField]
            private GameObject m_spike;
            public GameObject spike => m_spike;
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
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_attackOrbAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_fadeInAnimation.SetData(m_skeletonDataAsset);
                m_fadeOutAnimation.SetData(m_skeletonDataAsset);

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
            Attack2,
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
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Sensor")]
        private RaySensor m_upDownSensor;
        [SerializeField, TabGroup("Sensor")]
        private RaySensor m_leftRightSensor;
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
        [SerializeField]
        private GameObject m_hurtFX;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;
        private Vector2 m_startPos;

        private float m_currentCD;
        private bool m_isDetecting;

        private Coroutine m_executeMoveCoroutine;
        private bool m_shardspawned = false;

        public EventAction<EventActionArgs> OnDetection;
        public EventAction<EventActionArgs> OnDamaged;

        [SerializeField]
        private GameObject m_spectreIceShield;

        private Vector2 m_targetPoint;
        [SerializeField]
        private float m_icePlungeCooldown;
        private int m_icePlungeSameAttackCounter;
        private bool m_canUseIcePlunge = true;
        /*
                [SerializeField]
                private GameObject m_spectreIceShieldHitbox;
                [SerializeField]
                private GameObject m_spectreIceShieldHealth;
                [SerializeField]
                private Health m_healthToChange;
        */
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
        private IEnumerator DetectRoutine()
        {
            OnDetection?.Invoke(this, EventActionArgs.Empty);
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_shardspawned = false;
            m_agent.Stop();
            m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            if (m_targetInfo.isValid)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }

                else
                {
                    StartCoroutine(FlinchRoutine());
                }
            }
        }

        private IEnumerator FlinchRoutine()
        {
            OnDamaged?.Invoke(this, EventActionArgs.Empty);
            m_hitbox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            yield return new WaitForSeconds(0.5f);
            //bug #2631, connected with spectreFire, and by extension also the spectreEarth, something gets boken here where this entity gets stucke here on flinch, and it gets fixed on graveyard and laboratory
            m_animation.SetAnimation(0, m_info.fadeOutAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeOutAnimation); //also gets stuck here, needs to be further investigated.
            yield return new WaitForSeconds(0.5f);
            var random = UnityEngine.Random.Range(0, 2);
            transform.position = new Vector2(m_targetInfo.position.x + (IsFacingTarget() ? -30 : 30), m_targetInfo.position.y+30);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);
            m_hitbox.gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (!m_targetInfo.isValid)
            {
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)
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
            Vector2 targetground = new Vector2(m_targetInfo.position.x, this.transform.position.y);
            Vector3 targetgroundv3 = targetground;

            switch (m_attack)
            {
                case Attack.Attack1:
                    m_animation.EnableRootMotion(true, false);
                    StartCoroutine(AttackRoutine2());
                    //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    //LaunchProjectile();
                    /*if (m_character.facing == HorizontalDirection.Right)
                    {
                        if (m_shardspawned == false && m_targetInfo.position.x - 6.5 > this.transform.position.x
                            && m_targetInfo.position.x + 8.3 < WallPosition().x && this.transform.position.y + 20 > GroundPosition().y)
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                        else
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                    }
                    else
                    {
                        if (m_shardspawned == false && m_targetInfo.position.x - 6.5 > WallPosition().x
                            && m_targetInfo.position.x + 8.3 < this.transform.position.x && this.transform.position.y + 20 > GroundPosition().y)
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                        else
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                    }*/
                    break;
                case Attack.Attack2:
                    m_animation.EnableRootMotion(true, false);
                    StartCoroutine(AttackRoutine2());
                    //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    //LaunchProjectile();
                    /*if (m_character.facing == HorizontalDirection.Right)
                    {
                        if (m_shardspawned == false && m_targetInfo.position.x - 6.5 > this.transform.position.x
                            && m_targetInfo.position.x + 8.3 < WallPosition().x && this.transform.position.y > GroundPosition().y)
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                        else
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                    }
                    else
                    {
                        if (m_shardspawned == false && m_targetInfo.position.x - 6.5 > WallPosition().x
                            && m_targetInfo.position.x + 8.3 < this.transform.position.x && this.transform.position.y > GroundPosition().y)
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                        else
                        {
                            StartCoroutine(AttackRoutine2());
                        }
                    }*/
                    break;
            }
        }

        private IEnumerator AttackRoutine1()
        {

            yield return new WaitForSeconds(1.25f);
            LaunchProjectile();
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_selfCollider.SetActive(false);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator AttackRoutine2()
        {
            if (m_icePlungeSameAttackCounter != 3 && m_canUseIcePlunge)
            {
                yield return new WaitForSeconds(1.25f);
                if (m_character.facing == HorizontalDirection.Right)
                {
                    m_targetPoint = new Vector2(m_targetInfo.position.x - 10, m_targetInfo.position.y + 30f);
                }
                else
                {
                    m_targetPoint = new Vector2(m_targetInfo.position.x + 10, m_targetInfo.position.y + 30f);
                }
                /*var direction = (m_targetPoint - (Vector2)transform.position).normalized;
                m_animation.SetAnimation(0, m_info.move, true);
                while (Vector2.Distance(transform.position, m_targetPoint) > 10f)
                {
                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                    }
                    m_agent.MoveTowardsForced(direction, m_info.move.speed);
                    yield return null;
                }
                m_agent.Stop();*/
                //m_movement.MoveTowards(new Vector2(m_targetInfo.position.x, m_targetInfo.position.y + 20), m_info.move.speed);
                yield return new WaitForSeconds(1.25f);
                if (!m_upDownSensor.isDetecting && !m_leftRightSensor.isDetecting || !m_upDownSensor.allRaysDetecting && !m_leftRightSensor.allRaysDetecting)
                {
                    m_icePlungeSameAttackCounter++;
                    SpawnSpike();
                    m_animation.SetAnimation(0, m_info.attack2.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                }
                else
                {
                    yield return null;
                }
                m_selfCollider.SetActive(false);
                m_flinchHandle.m_autoFlinch = true;
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                m_canUseIcePlunge = false;
                StartCoroutine(IcePlungeCooldown());
                while (!m_canUseIcePlunge)
                {
                    if (IsFacingTarget())
                    {
                        if (m_character.facing == HorizontalDirection.Right)
                        {
                            m_targetPoint = new Vector2(m_targetInfo.position.x - 10, m_targetInfo.position.y + 30f);
                        }
                        else
                        {
                            m_targetPoint = new Vector2(m_targetInfo.position.x + 10, m_targetInfo.position.y + 30f);
                        }
                    }
                    else
                    {
                        CustomTurn();
                    }
                    /*var direction = (m_targetPoint - (Vector2)transform.position).normalized;
                    m_animation.SetAnimation(0, m_info.move, true);
                    while (Vector2.Distance(transform.position, m_targetPoint) > 10f)
                    {
                        m_agent.MoveTowardsForced(direction, m_info.move.speed);
                        yield return null;
                    }
                    m_agent.Stop();
                    yield return null;*/
                }
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
            yield return null;
        }
        private IEnumerator IcePlungeCooldown()
        {
            //m_icePlungeCooldown -= Time.deltaTime;
            var duration = m_icePlungeCooldown;
            var timeLeft = 0f;
            while (duration > timeLeft && !m_canUseIcePlunge)
            {
                timeLeft += Time.deltaTime;
                if (timeLeft >= duration)
                {
                    timeLeft = 0f;
                    m_canUseIcePlunge = true;
                    m_icePlungeSameAttackCounter = 0;
                }
                yield return null;
            }
            yield return null;
        }
        private IEnumerator ShardDelay()
        {
            yield return new WaitForSeconds(5);
            m_shardspawned = false;
            yield return null;
        }
        #region Movement
        private IEnumerator ExecuteMove(/*float attackRange, *//*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(m_character.facing == HorizontalDirection.Left? m_targetInfo.position.x + 10 : m_targetInfo.position.x - 10, /*GroundPosition().y + 20*/m_targetInfo.position.y + 40f);
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < m_currentAttackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(/*m_targetInfo.position.y*/newPos.y - transform.position.y) < m_currentAttackRange ? true : false;
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
        private void SpawnSpike()
        {
            //Vector2 targetground = new Vector2(m_targetInfo.position.x, this.transform.position.y);
            /*Vector3 targetgroundv3;
            if (m_character.facing == HorizontalDirection.Right)
            {
                targetgroundv3 = new Vector3(m_targetPoint.x + 10f, m_targetPoint.y, 0);
            }
            else
            {
                targetgroundv3 = new Vector3(m_targetPoint.x - 10f, m_targetPoint.y, 0);
            }*//*
            Vector3 targetgroundv3 = new Vector3(m_targetPoint.x + 10f, m_targetPoint.y, 0);*/
            Instantiate(m_info.spike, m_projectilePoints[0].position, Quaternion.identity);
            m_shardspawned = true;
            StartCoroutine(ShardDelay());
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
            //GetComponent<Damageable>().DamageTaken += SpectreIceAI_DamageTaken;
            /*m_hitbox = m_spectreIceShieldHitbox.GetComponent<Hitbox>();
            m_damageable = m_spectreIceShieldHealth.GetComponent<Damageable>();*/
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_spectreIceShield.GetComponent<SpectreIceShield>().OnActivate += OnActivate;
            m_spectreIceShield.GetComponent<SpectreIceShield>().OnShieldDestroy += OnShieldDestroy;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoints[0]);
            m_projectileLauncher.SetProjectile(m_info.projectile.projectileInfo);

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Attack1, Attack.Attack2);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attack1.range, m_info.attack2.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void OnActivate(object sender, EventActionArgs eventArgs)
        {
            m_hitbox.SetCanBlockDamageState(true);
            m_hitbox.damageFXInfo.fx = null;
            //throw new NotImplementedException();
        }

        private void OnShieldDestroy(object sender, EventActionArgs eventArgs)
        {
            m_hitbox.SetCanBlockDamageState(false);
            m_hitbox.damageFXInfo.fx = m_hurtFX;
            //throw new NotImplementedException();
        }

        /*private void SpectreIceAI_DamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            OnDamaged?.Invoke(this, EventActionArgs.Empty);
            //throw new NotImplementedException();
        }*/

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
                            rb2d.MovePosition(rb2d.transform.position + dir * m_info.move.speed * Time.fixedDeltaTime);
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
                    m_stateHandle.Wait(m_turnState);
                    if (m_shardspawned != true)
                    StopAllCoroutines();
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
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(/*m_currentAttackRange*//* m_currentAttackRange, */m_currentAttack));
                    //StartCoroutine(AttackRoutine2());
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

                        m_agent.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;

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
                    //OnDetection?.Invoke(this, EventActionArgs.Empty);
                    m_agent.Stop();
                    m_attackDecider.hasDecidedOnAttack = false;
                    ChooseAttack();
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
