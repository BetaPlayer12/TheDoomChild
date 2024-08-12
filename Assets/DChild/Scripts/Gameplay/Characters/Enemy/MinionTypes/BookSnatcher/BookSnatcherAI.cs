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
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BookSnatcher")]
    public class BookSnatcherAI : CombatAIBrain<BookSnatcherAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;

            //Attack Behaviours
            [SerializeField, BoxGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, BoxGroup("Attack")]
            private SimpleAttackInfo m_spellAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spellAttack => m_spellAttack;
            [SerializeField, BoxGroup("Attack")]
            private SimpleAttackInfo m_summonAttack = new SimpleAttackInfo();
            public SimpleAttackInfo summonAttack => m_summonAttack;
            [SerializeField, MinValue(0), TabGroup("Attack")]
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
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_bookOpenAnimation;
            public BasicAnimationInfo bookOpenAnimation => m_bookOpenAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [SerializeField]
            private List<SimpleProjectileAttackInfo> m_projectiles;
            public List<SimpleProjectileAttackInfo> projectiles => m_projectiles;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_bookSummonEvent;
            public string bookSummonEvent => m_bookSummonEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_bookSummonEvent2;
            public string bookSummonEvent2 => m_bookSummonEvent2;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_spellAttack.SetData(m_skeletonDataAsset);
                m_summonAttack.SetData(m_skeletonDataAsset);
                for (int i = 0; i < m_projectiles.Count; i++)
                {
                    m_projectiles[i].SetData(m_skeletonDataAsset);
                }

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_bookOpenAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Patrol,
            Standby,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack,
            SpellAttack,
            SummonAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentMoveSpeed;
        private float m_currentFullCD;
        //private float m_currentMoveSpeed;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;
        private Vector2 m_lastTargetPos;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Projectile")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Summons")]
        private List<GameObject> m_minions;
        private List<ISummonedEnemy> m_summons;
        private int m_currentSummonID;

        private ProjectileLauncher m_projectileLauncher;

        [SerializeField]
        private bool m_willPatrol;
        private int m_minionSummon = 0;
        [SerializeField]
        private float m_cooldownToSummon;
        [SerializeField]
        private List<ParticleFX> m_summonElement;
        //public Stack<int> m_summoned = new Stack<int>();


        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        //private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomIdleRoutine;
        private Coroutine m_randomTurnRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.m_enableMixFlinch = true;
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null /*&& !ShotBlocked()*/)
            {
                base.SetTarget(damageable);
                //if (m_stateHandle.currentState != State.Chasing 
                //    && m_stateHandle.currentState != State.RunAway 
                //    && m_stateHandle.currentState != State.Turning 
                //    && m_stateHandle.currentState != State.WaitBehaviourEnd)
                //{
                //}
                if (!TargetBlocked() && !m_enablePatience)
                {
                    m_selfCollider.enabled = false;

                    if (!m_isDetecting)
                    {
                        if (m_randomIdleRoutine != null)
                        {
                            StopCoroutine(m_randomIdleRoutine);
                            m_randomIdleRoutine = null;
                        }
                        if (m_randomTurnRoutine != null)
                        {
                            StopCoroutine(m_randomTurnRoutine);
                            m_randomTurnRoutine = null;
                        }
                        m_isDetecting = true;
                        m_stateHandle.SetState(State.Detect);
                    }
                    m_currentPatience = 0;
                    //m_randomIdleRoutine = null;
                    //var patienceRoutine = PatienceRoutine();
                    //StopCoroutine(patienceRoutine);
                    m_enablePatience = false;
                }
            }
            else
            {
                m_enablePatience = true;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        //Patience Handler
        private void Patience()
        {
            if (m_patienceRoutine == null)
            {
                m_patienceRoutine = StartCoroutine(PatienceRoutine());
            }
            if (TargetBlocked())
            {
                if (IsFacingTarget())
                {
                    if (m_sneerRoutine == null)
                    {
                        m_sneerRoutine = StartCoroutine(SneerRoutine());
                    }
                    //else if ()
                    //{
                    //}
                }
                else
                {
                    if (m_sneerRoutine != null)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    //m_enablePatience = false;
                    m_turnState = State.Standby;
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                        m_stateHandle.SetState(State.Turning);
                }
            }
            else if (!TargetBlocked())
            {
                if (m_sneerRoutine != null)
                {
                    if (m_patienceRoutine != null)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }
                    StopCoroutine(m_sneerRoutine);
                    m_sneerRoutine = null;
                    m_enablePatience = false;
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                }
            }
        }
        private IEnumerator PatienceRoutine()
        {
            //if (m_enablePatience)
            //{
            //    while (m_currentPatience < m_info.patience)
            //    {
            //        m_currentPatience += m_character.isolatedObject.deltaTime;
            //        yield return null;
            //    }
            //}
            yield return new WaitForSeconds(m_info.patience);
            m_selfCollider.enabled = false;
            m_enablePatience = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.SetState(State.Patrol);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);

            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            //if (m_attackRoutine != null)
            //{
            //    StopCoroutine(m_attackRoutine);
            //}
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
            {
                StopAllCoroutines();
                m_flinchHandle.m_autoFlinch = true;
                m_currentCD += m_currentCD + 0.5f;
                m_selfCollider.enabled = true;
                m_animation.DisableRootMotion();
                m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
                    m_animation.SetEmptyAnimation(0, 0);
                m_selfCollider.enabled = false;
                m_stateHandle.ApplyQueuedState();
            }
        }
        [SerializeField]
        private Transform m_summonMinionLocation;
        private int m_projectileElement;
        private int m_projectileID;
        #region Attacks
        private void LaunchAttack()
        {
            switch (m_attackDecider.chosenAttack.attack)
            {
                case Attack.SpellAttack:
                    m_projectileLauncher = new ProjectileLauncher(m_info.projectiles[m_projectileID].projectileInfo, m_projectilePoint.transform);
                    m_projectileLauncher.AimAt(m_lastTargetPos);
                    m_projectileLauncher.LaunchProjectile();
                    break;
                case Attack.SummonAttack:
                    var minions = Instantiate(m_minions[m_currentSummonID], m_summonLocation, Quaternion.identity);
                    minions.GetComponent<Damageable>().Destroyed += OnMinionSummonedDestroyed;
                    //m_summons[m_currentSummonID].SummonAt(m_currentSummonID == 0 ? (Vector2)transform.position : new Vector2(m_lastTargetPos.x, m_lastTargetPos.y + 10f), m_targetInfo);
                    break;
            }
        }
        private Vector2 m_summonLocation;
        private void LaunchMinion()
        {
            m_currentSummonID = UnityEngine.Random.Range(0, m_summons.Count);
            switch (m_currentSummonID)
            {
                case 0:
                    if(m_character.facing == HorizontalDirection.Left){
                        m_summonLocation = new Vector2(m_summonMinionLocation.position.x - 1f, m_summonMinionLocation.position.y);
                    }
                    else
                    {
                        m_summonLocation = new Vector2(m_summonMinionLocation.position.x + 1f, m_summonMinionLocation.position.y);
                    }
                    break;
                case 1:
                    if (m_character.facing == HorizontalDirection.Left)
                    {
                        m_summonLocation = new Vector2(m_summonMinionLocation.position.x -1f, m_summonMinionLocation.position.y - 5f);
                    }
                    else
                    {
                        m_summonLocation = new Vector2(m_summonMinionLocation.position.x + 1f, m_summonMinionLocation.position.y - 5f);
                    }
                    break;
                case 2:
                    if (m_character.facing == HorizontalDirection.Left)
                    {
                        m_summonLocation = new Vector2(m_summonMinionLocation.position.x - 1f, m_summonMinionLocation.position.y);
                    }
                    else
                    {
                        m_summonLocation = new Vector2(m_summonMinionLocation.position.x + 1f, m_summonMinionLocation.position.y);
                    }
                    break;
            }
            var projectileID = UnityEngine.Random.Range(0, m_info.projectiles.Count);
            switch (projectileID)
            {
                case 0:
                    m_projectileElement = 0;
                    break;
                case 1:
                    m_projectileElement = 1;
                    break;
                case 2:
                    m_projectileElement = 2;
                    break;
            }
            m_projectileID = projectileID;
            switch (m_attackDecider.chosenAttack.attack)
            {
                case Attack.SpellAttack:
                    m_summonElement[m_projectileElement].Play();
                    break;
                case Attack.SummonAttack:
                    m_summonElement[3].Play();
                    break;
            }
        }
        private void OnMinionSummonedDestroyed(object sender, EventActionArgs eventArgs)
        {
            StartCoroutine(Cooldown());
            //throw new NotImplementedException();
        }

        #endregion
        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(m_cooldownToSummon);
            m_minionSummon--;
            yield return null;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range)
                                  , new AttackInfo<Attack>(Attack.SpellAttack, m_info.spellAttack.range)
                                  , new AttackInfo<Attack>(Attack.SummonAttack, m_info.summonAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RandomIdleRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                yield return null;
            }
        }

        private IEnumerator RandomTurnRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                m_turnState = State.Idle;
                m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            while (true)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        //private IEnumerator AttackRoutine()
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        m_animation.SetAnimation(0, m_info.attack.animation, false);
        //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
        //        m_animation.SetEmptyAnimation(0, 0);
        //    }
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.run.speed * .75f, m_info.run.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            m_spineEventListener.Subscribe(m_info.bookSummonEvent, LaunchAttack);
            m_spineEventListener.Subscribe(m_info.bookSummonEvent2, LaunchMinion);

            if (!m_willPatrol)
            {
                m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
                m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
            }

            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();

            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_summons = new List<ISummonedEnemy>();
            for (int i = 0; i < m_minions.Count; i++)
            {
                m_summons.Add(m_minions[i].GetComponent<ISummonedEnemy>());
            }
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        m_movement.Stop();

                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Idle:
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.SetAnimation(0, m_info.walk.animation, true).TimeScale = 2f;
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.walk.speed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    m_selfCollider.enabled = false;
                    m_flinchHandle.m_enableMixFlinch = false;
                    m_animation.EnableRootMotion(true, false);
                    m_lastTargetPos = m_targetInfo.position;
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation.animation);
                            //m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                        case Attack.SpellAttack:
                            m_attackHandle.ExecuteAttack(m_info.spellAttack.animation, m_info.idleAnimation.animation);
                            //m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                        case Attack.SummonAttack:
                            m_minionSummon++;
                            //m_summoned.Push(m_minionSummon);
                            if (m_minionSummon <= 1)
                            {
                                m_attackHandle.ExecuteAttack(m_info.summonAttack.animation, m_info.idleAnimation.animation);
                            }
                            if (m_minionSummon >= 1)
                            {
                                m_minionSummon = 1;
                                if (m_minionSummon == 1)
                                {
                                    m_stateHandle.SetState(State.Cooldown);
                                }
                                //m_stateHandle.SetState(State.Attacking);
                            }
                            m_animation.DisableRootMotion();
                            m_attackDecider.hasDecidedOnAttack = false;
                            m_stateHandle.ApplyQueuedState();
                            //m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                    }
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
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }

                    if (m_currentCD <= m_currentFullCD)
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
                    {
                        var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    m_movement.Stop();

                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.run.animation, true);
                                    m_character.physics.SetVelocity(toTarget.normalized.x * m_currentMoveSpeed, m_character.physics.velocity.y);
                                }
                                else
                                {
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                        m_movement.Stop();

                                    m_selfCollider.enabled = true;
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }

                    if (m_patienceRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }

                    if (m_sneerRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience && m_stateHandle.currentState != State.Standby)
            {
                //Patience();
                if (TargetBlocked())
                {
                    m_stateHandle.OverrideState(State.Standby);
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
