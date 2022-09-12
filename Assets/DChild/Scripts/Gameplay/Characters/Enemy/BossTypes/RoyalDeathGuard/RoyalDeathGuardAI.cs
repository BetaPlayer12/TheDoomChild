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
using Doozy.Engine;
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/RoyalDeathGuard")]
    public class RoyalDeathGuardAI : CombatAIBrain<RoyalDeathGuardAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [SerializeField]
            private MovementInfo m_move2 = new MovementInfo();
            public MovementInfo move2 => m_move2;


            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("Attack 1")]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField, TabGroup("Attack 2")]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField, TabGroup("Attack 3")]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField, TabGroup("Attack 4")]
            private SimpleAttackInfo m_attack4 = new SimpleAttackInfo();
            public SimpleAttackInfo attack4 => m_attack4;
            [SerializeField, TabGroup("Attack 4"), ValueDropdown("GetAnimations")]
            private string m_attack4bAnimation;
            public string attack4bAnimation => m_attack4bAnimation;
            [SerializeField, TabGroup("Attack 4"), ValueDropdown("GetAnimations")]
            private string m_attack4FinalAnimation;
            public string attack4FinalAnimation => m_attack4FinalAnimation;
            [SerializeField, TabGroup("Scythe Spin")]
            private SimpleAttackInfo m_scytheSpinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheSpinAttack => m_scytheSpinAttack;


            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeatAnimation;
            public string defeatAnimation => m_defeatAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeat2Animation;
            public string defeat2Animation => m_defeat2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBlackAnimation;
            public string flinchBlackAnimation => m_flinchBlackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_healAnimation;
            public string healAnimation => m_healAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleTransition1to2Animation;
            public string idleTransition1to2Animation => m_idleTransition1to2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleTransition2to1Animation;
            public string idleTransition2to1Animation => m_idleTransition2to1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_playerDetectAnimation;
            public string playerDetectAnimation => m_playerDetectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_energyAbsorbAnimation;
            public string energyAbsorbAnimation => m_energyAbsorbAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private GameObject m_tentacle;
            public GameObject tentacle => m_tentacle;
            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            //[Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_deathFXEvent;
            //public string deathFXEvent => m_deathFXEvent;
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_teleportFXEvent;
            //public string teleportFXEvent => m_teleportFXEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_move2.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_attack4.SetData(m_skeletonDataAsset);
                m_scytheSpinAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_attackCount;
            public int attackCount => m_attackCount;
            //[SerializeField]
            //private List<int> m_patternAttackCount;
            //public List<int> patternAttackCount => m_patternAttackCount;
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Pattern
        {
            AttackPattern1,
            AttackPattern2,
            AttackPattern3,
            WaitAttackEnd,
        }

        private enum Attack
        {
            Attack1,
            Attack2,
            Attack3,
            Attack4,
            ScytheSpinAttack,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            Wait,
        }

        private bool[] m_attackUsed;
        private List<Pattern> m_attackCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        //[SerializeField, TabGroup("Modules")]
        //private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_slashGroundFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_scytheSpinFX;

        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_scytheSpinBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_groundStabBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_scytheStabBB;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Pattern> m_patternDecider;
        private Pattern m_currentPattern;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        
        private int m_attackCount;
        private int m_randomAttack;
        private float m_startGroundPos;
        private bool m_hasHealed;
        private bool m_hasPhaseChanged;
        private PhaseInfo m_phaseInfo;
        //private int m_hitCount;
        //private int[] m_patternAttackCount;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_phaseInfo = obj;
            //for (int i = 0; i < m_patternAttackCount.Length; i++)
            //{
            //    m_patternAttackCount[i] = obj.patternAttackCount[i];
            //}
        }

        private void ChangeState()
        {
            if (!m_hasPhaseChanged)
            {
                m_hasPhaseChanged = true;
                StopAllCoroutines();
                m_scytheSpinFX.Stop();
                m_animation.DisableRootMotion();
                m_animation.SetEmptyAnimation(0, 0);
                m_stateHandle.OverrideState(State.Phasing);
                m_phaseHandle.ApplyChange();
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Chasing);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            //m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //CustomTurn();
            //m_animation.SetAnimation(0, m_info.playerDetectAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.playerDetectAnimation);
            //m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            //SelfHeal
            m_hitbox.SetInvulnerability(Invulnerability.None);
            var flinch = IsFacingTarget() ? m_info.flinchAnimation : m_info.flinchBlackAnimation;
            m_animation.SetAnimation(0, flinch, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinch);
            m_animation.SetAnimation(0, m_info.healAnimation, false);
            Debug.Log("health percentage " + (float)m_health.currentValue / m_health.maxValue);
            var healPercentage = (((float)m_health.currentValue / m_health.maxValue) + .2f);
            yield return new WaitForSeconds(1);
            m_health.SetHealthPercentage(healPercentage);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator HealingRoutine()
        {
            m_hasHealed = true;
            m_animation.SetAnimation(0, m_info.healAnimation, false);
            var healPercentage = (((float)m_health.currentValue / m_health.maxValue) + .2f);
            //Debug.Log("heal points " + healPercentage);
            yield return new WaitForSeconds(1);
            m_health.SetHealthPercentage(healPercentage);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_scytheSpinFX.Stop();
            m_agent.Stop();
            m_hitbox.Disable();
            //this.gameObject.SetActive(false); //TEMP
            if (!m_deathHandle.gameObject.activeSelf)
            {
                this.enabled = false;
                StartCoroutine(DefeatRoutine());
                StartCoroutine(DeathStickRoutine());
            }
            else
            {
                StartCoroutine(DeathStickRoutine());
            }
        }

        private IEnumerator DefeatRoutine()
        {
            m_animation.SetAnimation(0, m_info.defeatAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.defeatAnimation);
            this.gameObject.SetActive(false);
        }

        private IEnumerator DeathStickRoutine()
        {
            var yAxis = transform.position.y;
            while (true)
            {
                transform.position = new Vector2(transform.position.x, yAxis);
                yield return null;
            }
        }

        #region Attacks
        private IEnumerator Attack1Routine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Attack2Routine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            yield return new WaitForSeconds(.5f);
            m_groundStabBB.transform.position = new Vector2(m_targetInfo.position.x, GroundPosition().y);
            yield return new WaitForSeconds(1.75f);
            m_slashGroundFX.Play();
            m_groundStabBB.enabled = true;
            m_scytheStabBB.enabled = true;
            yield return new WaitForSeconds(1f);
            m_groundStabBB.enabled = false;
            m_scytheStabBB.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Attack3Routine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            yield return new WaitForSeconds(3f);
            //m_scytheSpinFX.gameObject.SetActive(true);
            m_scytheSpinFX.Play(); //m_scytheSpinFX.GetComponent<ParticleSystem>().Play();
            m_scytheSpinBB.enabled = true;
            yield return new WaitForSeconds(1.5f);
            m_scytheSpinFX.Stop();
            m_scytheSpinBB.enabled = false;
            //m_scytheSpinFX.gameObject.SetActive(false); //m_scytheSpinFX.GetComponent<ParticleSystem>().Stop();
            //yield return new WaitForSeconds(1.3f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack3.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Attack4Routine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack4.animation, false);
            m_animation.AddAnimation(0, m_info.attack4bAnimation, true, 0)/*.MixDuration = 1*/;
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            //yield return new WaitForSeconds(1.3f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack4bAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private IEnumerator ExecuteMove(Vector2 target, float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            while (!inRange)
            {

                bool xTargetInRange = Mathf.Abs(target.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(target.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                //Debug.Log("Facing Target " + IsFacingTarget());
                DynamicMovement(new Vector2(m_targetInfo.position.x, target.y));
                yield return null;
            }
            m_agent.Stop();

            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target)
        {
            if (IsFacingTarget())
            {
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                m_agent.SetDestination(target);
                m_agent.Move(m_info.move.speed);

                m_animation.SetAnimation(0, m_info.move.animation, true);
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation && GetComponent<IsolatedPhysics2D>().velocity.y <= 0 && m_stateHandle.currentState != State.Phasing)
                {
                    m_turnState = State.Attacking;
                    m_stateHandle.OverrideState(State.Turning);
                }
            }
        }
        #endregion

        private bool AllowAttack(Phase phase, State state)
        {
            if (m_phaseHandle.currentPhase >= phase)
            {
                return true;
            }
            else
            {
                DecidedOnAttack(false);
                m_stateHandle.OverrideState(state);
                return false;
            }
        }

        private void DecidedOnAttack(bool condition)
        {
            m_patternDecider.hasDecidedOnAttack = condition;
            m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern3, m_info.targetDistanceTolerance));
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_patternDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChoosePattern()
        {
            if (!m_patternDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_patternDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentPattern && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentPattern = m_attackCache[i];
                        return;
                    }
                }
            }
        }

        private void ExecuteAttack(Attack m_attack)
        {
            switch (m_attack)
            {
                case Attack.Attack1:
                    StartCoroutine(Attack1Routine());
                    break;
                case Attack.Attack2:
                    StartCoroutine(Attack2Routine());
                    break;
                case Attack.Attack3:
                    StartCoroutine(Attack3Routine());
                    break;
                case Attack.Attack4:
                    StartCoroutine(Attack4Routine());
                    break;
                case Attack.ScytheSpinAttack:
                    m_stateHandle.OverrideState(State.Chasing);
                    break;
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

        void AddToAttackCache(params Pattern[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoint);
            m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            //for (int i = 0; i < 3; i++)
            //{
            //    m_attackDecider[i] = new RandomAttackDecider<Attack>();
            //}
            m_stateHandle = new StateHandle<State>(State.WaitBehaviourEnd, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            //m_patternCount = new float[4];
            m_attackCache = new List<Pattern>();
            AddToAttackCache(Pattern.AttackPattern1, Pattern.AttackPattern2, Pattern.AttackPattern3);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.deathFXEvent, m_deathFX.Play);
            m_animation.DisableRootMotion();
            m_startGroundPos = GroundPosition().y;

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            if (!m_hasPhaseChanged)
            {
                m_phaseHandle.MonitorPhase();
            }
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idle1Animation, true);
                    break;
                case State.Intro:
                    StartCoroutine(IntroRoutine());
                    //if (IsFacingTarget())
                    //{
                    //    StartCoroutine(IntroRoutine());
                    //    //m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //}
                    //else
                    //{
                    //    m_turnState = State.Intro;
                    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    //        m_stateHandle.SetState(State.Turning);
                    //}
                    break;
                case State.Phasing:
                    Debug.Log("Phase Time");
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    Debug.Log("Turning Steet");
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idle1Animation);
                    //m_animation.animationState.GetCurrent(0).MixDuration = 1;
                    //m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Attacking);
                    var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                    var target = new Vector2(m_targetInfo.position.x, GroundPosition().y /*m_startGroundPos*/);
                    m_attackCount++;
                    switch (m_currentPattern)
                    {
                        case Pattern.AttackPattern1:
                            if (m_randomAttack == 1) //do dis tomorow
                            {
                                if (m_attackCount <= m_phaseInfo.attackCount)
                                {
                                    StartCoroutine(ExecuteMove(target, m_info.attack1.range, /*0,*/ Attack.Attack1));
                                }
                                else
                                {
                                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                                }
                            }
                            else
                            {
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    switch (m_attackCount)
                                    {
                                        case 1:
                                            StartCoroutine(ExecuteMove(target, m_info.attack1.range, /*0,*/ Attack.Attack1));
                                            break;
                                        case 2:
                                            //StartCoroutine(ExecuteMove(m_targetInfo.position, m_info.attack2.range, /*0,*/ Attack.Attack2));
                                            ExecuteAttack(Attack.Attack2);
                                            break;
                                        case 3:
                                            m_stateHandle.OverrideState(State.ReevaluateSituation);
                                            break;
                                    }
                                }
                                else
                                {
                                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                                }
                            }
                            ///////
                            //m_stateHandle.OverrideState(State.ReevaluateSituation);
                            break;
                        case Pattern.AttackPattern2:
                            switch (m_attackCount)
                            {
                                case 1:
                                    StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack2));
                                    break;
                                case 2:
                                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                                    break;
                            }
                            ///////
                            //m_stateHandle.OverrideState(State.ReevaluateSituation);
                            break;
                        case Pattern.AttackPattern3:
                            switch (m_attackCount)
                            {
                                case 1:
                                    StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack3));
                                    break;
                                case 2:
                                    Debug.Log("pattern 3 condition for health: " + (m_health.maxValue * .6f));
                                    if (m_phaseHandle.currentPhase == Phase.PhaseOne ? m_health.currentValue <= (m_health.maxValue * .6f) : m_health.currentValue <= (m_health.maxValue * .1f))
                                    {
                                        StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack3));
                                    }
                                    else
                                    {
                                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                                    }
                                    break;
                                case 3:
                                    if (m_phaseHandle.currentPhase == Phase.PhaseOne ? m_health.currentValue <= (m_health.maxValue * .6f) : m_health.currentValue <= (m_health.maxValue * .1f))
                                    {
                                        m_randomAttack = m_phaseHandle.currentPhase == Phase.PhaseOne ? 1 : m_randomAttack;
                                        if (m_randomAttack == 1)
                                        {
                                            StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack3));
                                        }
                                        else
                                        {
                                            StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack4));
                                        }
                                    }
                                    else
                                    {
                                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                                    }
                                    break;
                                case 4:
                                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                                    break;
                            }
                            ///////
                            //m_stateHandle.OverrideState(State.ReevaluateSituation);
                            break;
                    }
                    break;

                case State.Chasing:
                    DecidedOnAttack(false);
                    ChoosePattern();
                    //if (IsTargetInRange(m_info.attack1.range))
                    //{
                    //    m_currentPattern = Pattern.AttackPattern1;
                    //}
                    if (IsFacingTarget())
                    {
                        if (m_patternDecider.hasDecidedOnAttack)
                        {
                            m_attackCount = 0;
                            m_randomAttack = UnityEngine.Random.Range(0, 2);
                            m_stateHandle.SetState(State.Attacking);
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReevaluateSituation:
                    Debug.Log("20% of health is: " + m_health.maxValue * .2f);
                    if (m_health.currentValue <= m_health.maxValue * .2f && !m_hasHealed)
                    {
                        Debug.Log("Current health is: " + m_health.currentValue);
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(HealingRoutine());
                        return;
                    }
                    m_stateHandle.SetState(State.Chasing);
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            //m_stickToGround = false;
            //m_currentCD = 0;
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}