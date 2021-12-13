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
using UnityEngine.Playables;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/UndeadGeneral")]
    public class UndeadGeneralAI : CombatAIBrain<UndeadGeneralAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_moveSlow = new MovementInfo();
            public MovementInfo moveSlow => m_moveSlow;
            [SerializeField]
            private MovementInfo m_moveMedium = new MovementInfo();
            public MovementInfo moveMedium => m_moveMedium;
            [SerializeField]
            private MovementInfo m_moveFast = new MovementInfo();
            public MovementInfo moveFast => m_moveFast;
            [SerializeField]
            private MovementInfo m_moveForwardGuardStance = new MovementInfo();
            public MovementInfo moveForwardGuardStance => m_moveForwardGuardStance;
            [SerializeField]
            private MovementInfo m_moveBackGuardStance = new MovementInfo();
            public MovementInfo moveBackGuardStance => m_moveBackGuardStance;
            [SerializeField]
            private MovementInfo m_dodgeHop = new MovementInfo();
            public MovementInfo dodgeHop => m_dodgeHop;

            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_normalAttack = new SimpleAttackInfo();
            public SimpleAttackInfo normalAttack => m_normalAttack;
            [SerializeField, BoxGroup("Earth Shaker")]
            private SimpleAttackInfo m_earthShakerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo earthShakerAttack => m_earthShakerAttack;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerCancelAnimation;
            public string earthShakerCancelAnimation => m_earthShakerCancelAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerStunnedLoopAnimation;
            public string earthShakerStunnedLoopAnimation => m_earthShakerStunnedLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerRecoverAnimation;
            public string earthShakerRecoverAnimation => m_earthShakerRecoverAnimation;
            [SerializeField, BoxGroup("Special Thrust")]
            private SimpleAttackInfo m_specialThrustAttack = new SimpleAttackInfo();
            public SimpleAttackInfo specialThrustAttack => m_specialThrustAttack;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustStartAnimation;
            public string specialThrustStartAnimation => m_specialThrustStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustHitAnimation;
            public string specialThrustHitAnimation => m_specialThrustHitAnimation;
            [SerializeField, BoxGroup("Dodge Attack")]
            private SimpleAttackInfo m_dodgeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo dodgeAttack => m_dodgeAttack;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro1Animation;
            public string intro1Animation => m_intro1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro2Animation;
            public string intro2Animation => m_intro2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro3Animation;
            public string intro3Animation => m_intro3Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleCombatAnimation;
            public string idleCombatAnimation => m_idleCombatAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleGuardAnimation;
            public string idleGuardAnimation => m_idleGuardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleToCombatTransitionAnimation;
            public string idleToCombatTransitionAnimation => m_idleToCombatTransitionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeatStartAnimation;
            public string defeatStartAnimation => m_defeatStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeatLoopAnimation;
            public string defeatLoopAnimation => m_defeatLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_victory1Animation;
            public string victory1Animation => m_victory1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_victory2Animation;
            public string victory2Animation => m_victory2Animation;

            [Title("FX")]
            [SerializeField]
            private GameObject m_fx;
            public GameObject fx => m_fx;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dustLandEvent;
            public string dustLandEvent => m_dustLandEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dustTakeOffEvent;
            public string dustTakeOffEvent => m_dustTakeOffEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_leftFootstepSoundEvent;
            public string leftFootstepSoundEvent => m_leftFootstepSoundEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rightFootstepSoundEvent;
            public string rightFootstepSoundEvent => m_rightFootstepSoundEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rootMoveEndEvent;
            public string rootMoveEndEvent => m_rootMoveEndEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rootMoveStartEvent;
            public string rootMoveStartEvent => m_rootMoveStartEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordSlash1Event;
            public string swordSlash1Event => m_swordSlash1Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordSlash2Event;
            public string swordSlash2Event => m_swordSlash2Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordSlash3Event;
            public string swordSlash3Event => m_swordSlash3Event;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveSlow.SetData(m_skeletonDataAsset);
                m_moveMedium.SetData(m_skeletonDataAsset);
                m_moveFast.SetData(m_skeletonDataAsset);
                m_moveForwardGuardStance.SetData(m_skeletonDataAsset);
                moveBackGuardStance.SetData(m_skeletonDataAsset);
                m_dodgeHop.SetData(m_skeletonDataAsset);
                m_normalAttack.SetData(m_skeletonDataAsset);
                m_earthShakerAttack.SetData(m_skeletonDataAsset);
                m_earthShakerAttack.SetData(m_skeletonDataAsset);
                m_specialThrustAttack.SetData(m_skeletonDataAsset);
                m_dodgeAttack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_patternCount;
            public List<float> patternCount => m_patternCount;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;
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

        //private enum Pattern
        //{
        //    AttackPattern1,
        //    AttackPattern2,
        //    AttackPattern3,
        //    WaitAttackEnd,
        //}

        private enum Attack
        {
            Normal,
            EarthShaker,
            SpecialThrust,
            Dodge,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            Wait,
        }

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Cinematic")]
        private BlackDeathCinematicPlayah m_cinematic;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_fx;
        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        //[ShowInInspector]
        //private RandomAttackDecider<Pattern> m_patternDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack>[] m_attackDecider;
        //private Pattern m_chosenPattern;
        //private Pattern m_previousPattern;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private int m_currentPhaseIndex;
        private float m_attackCount;
        private float[] m_patternCount;
        //private bool m_hasPhaseChanged;
        private Coroutine m_currentAttackCoroutine;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentPhaseIndex = obj.phaseIndex;
            for (int i = 0; i < m_patternCount.Length; i++)
            {
                m_patternCount[i] = obj.patternCount[i];
            }
        }

        private void ChangeState()
        {
            //StopCurrentAttackRoutine();
            //SetAIToPhasing();
            StartCoroutine(SmartChangePhaseRoutine());
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing /*&& !m_hasPhaseChanged*/)
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing /*&& !m_hasPhaseChanged*/)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
            m_phaseHandle.allowPhaseChange = true;
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_cinematic.PlayCinematic(1, false);
            m_animation.animationState.TimeScale = 1;
            //int introCount = UnityEngine.Random.Range(1, 4);
            //var introAnim = m_info.intro1Animation;
            //switch (introCount)
            //{
            //    case 1:
            //        introAnim = m_info.intro1Animation;
            //        break;
            //    case 2:
            //        introAnim = m_info.intro2Animation;
            //        break;
            //    case 3:
            //        introAnim = m_info.intro3Animation;
            //        break;
            //}
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.intro1Animation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.DisableRootMotion();
            m_hitbox.Enable();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SmartChangePhaseRoutine()
        {
            yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            StopCurrentAttackRoutine();
            SetAIToPhasing();
            yield return null;
        }

        private void SetAIToPhasing()
        {
            //m_hasPhaseChanged = true;
            m_hitbox.SetInvulnerability(Invulnerability.Level_1);
            m_phaseHandle.ApplyChange();
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
        }

        private void StopCurrentAttackRoutine()
        {
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
            }
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            //m_hitbox.SetInvulnerability(Invulnerability.None);
            //m_hasPhaseChanged = false;
            string phaseAnim = m_info.intro1Animation;
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    phaseAnim = m_info.intro1Animation;
                    break;
                case Phase.PhaseTwo:
                    phaseAnim = m_info.intro2Animation;
                    break;
                case Phase.PhaseThree:
                    phaseAnim = m_info.intro3Animation;
                    break;
            }
            m_animation.SetAnimation(0, phaseAnim, false).MixDuration = 0;
            //yield return new WaitForSeconds(3.9f);
            yield return new WaitForAnimationComplete(m_animation.animationState, phaseAnim);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #region Attacks

        private IEnumerator NormalAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.normalAttack.animation, false).MixDuration = 0;
            //yield return new WaitForSeconds(0.5f);
            //m_character.physics.SetVelocity(m_info.shoulderBashVelocity.x * transform.localScale.x, 0);
            //yield return new WaitForSeconds(0.15f);
            //m_movement.Stop();
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator EarthShakerAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.earthShakerAttack.animation, false).MixDuration = 0;
            //yield return new WaitForSeconds(0.5f);
            //m_character.physics.SetVelocity(m_info.shoulderBashVelocity.x * transform.localScale.x, 0);
            //yield return new WaitForSeconds(0.15f);
            //m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.earthShakerAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }
        
        private IEnumerator SpecialThrustAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_animation.EnableRootMotion(true, false);
            //m_animation.SetAnimation(0, m_info.specialThrustStartAnimation, false);
            m_animation.SetAnimation(0, m_info.specialThrustAttack.animation, false);
            m_animation.AddAnimation(0, m_info.specialThrustHitAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.specialThrustHitAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            //m_deathFX.Play();
            m_movement.Stop();
        }

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_info.moveMedium.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.moveMedium.speed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }
        #endregion

        private bool AllowAttack(int phaseIndex, State state)
        {
            if (m_currentPhaseIndex >= phaseIndex)
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
            //m_patternDecider.hasDecidedOnAttack = condition;
            for (int i = 0; i < m_attackDecider.Length; i++)
            {
                m_attackDecider[i].hasDecidedOnAttack = condition;
            }
        }

        private void UpdateAttackDeciderList()
        {
            //m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
            //                        new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance),
            //                        new AttackInfo<Pattern>(Pattern.AttackPattern3, m_info.targetDistanceTolerance));
            m_attackDecider[0].SetList(new AttackInfo<Attack>(Attack.Normal, m_info.normalAttack.range)
                                    , new AttackInfo<Attack>(Attack.EarthShaker, m_info.earthShakerAttack.range)
                                    , new AttackInfo<Attack>(Attack.SpecialThrust, m_info.specialThrustAttack.range)
                                    , new AttackInfo<Attack>(Attack.Dodge, m_info.dodgeAttack.range));
            m_attackDecider[1].SetList(new AttackInfo<Attack>(Attack.Normal, m_info.normalAttack.range)
                                    , new AttackInfo<Attack>(Attack.EarthShaker, m_info.earthShakerAttack.range)
                                    , new AttackInfo<Attack>(Attack.SpecialThrust, m_info.specialThrustAttack.range)
                                    , new AttackInfo<Attack>(Attack.Dodge, m_info.dodgeAttack.range));
            m_attackDecider[2].SetList(new AttackInfo<Attack>(Attack.Normal, m_info.normalAttack.range)
                                    , new AttackInfo<Attack>(Attack.EarthShaker, m_info.earthShakerAttack.range)
                                    , new AttackInfo<Attack>(Attack.SpecialThrust, m_info.specialThrustAttack.range)
                                    , new AttackInfo<Attack>(Attack.Dodge, m_info.dodgeAttack.range));
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            //if (m_patternDecider != null)
            //{
            //}
            //for (int i = 0; i < m_attackDecider.Length; i++)
            //{
            //    if (m_attackDecider[i] != null)
            //    {
            //        UpdateAttackDeciderList();
            //    }
            //}
            base.ApplyData();
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        private void ChooseAttack(int patternIndex)
        {
            if (!m_attackDecider[patternIndex].hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider[patternIndex].DecideOnAttack();
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

        private void ExecuteAttack(int patternIndex)
        {
            if (m_attackCount < m_patternCount[patternIndex])
            {
                ChooseAttack(patternIndex);
                if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                {
                    m_stateHandle.Wait(State.Attacking);
                    switch (m_currentAttack)
                    {
                        case Attack.Normal:
                            if (patternIndex == 0 || patternIndex == 1)
                            {
                                if (m_currentPhaseIndex != 3)
                                {
                                    m_attackCount++;
                                    m_currentAttackCoroutine = StartCoroutine(NormalAttackRoutine());
                                }
                                else
                                {
                                    DecidedOnAttack(false);
                                    m_stateHandle.ApplyQueuedState();
                                }
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.EarthShaker:
                            if (m_currentPhaseIndex == 3)
                            {
                                m_attackCount++;
                                m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.SpecialThrust:
                            if (patternIndex == 0 || patternIndex == 2)
                            {
                                if (AllowAttack(0, State.Attacking))
                                {
                                    m_attackCount++;
                                    m_currentAttackCoroutine = StartCoroutine(SpecialThrustAttackRoutine());
                                }
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Dodge:
                            if (AllowAttack(3, State.Attacking))
                            {
                                m_attackCount++;
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();

                            }
                            break;
                    }
                }
                else
                {
                    MoveToTarget(m_currentAttackRange);
                }
            }
            else
            {
                m_stateHandle.OverrideState(State.Chasing);
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

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.defeatStartAnimation);
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>[3];
            for (int i = 0; i < 3; i++)
            {
                m_attackDecider[i] = new RandomAttackDecider<Attack>();
            }
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_patternCount = new float[3];
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Normal, Attack.EarthShaker, Attack.SpecialThrust, Attack.Dodge);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.normalAttack.range, m_info.earthShakerAttack.range, m_info.specialThrustAttack.range, m_info.dodgeAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.phaseEvent, PhaseFX);
            m_animation.DisableRootMotion();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            //if (!m_hasPhaseChanged && m_stateHandle.currentState != State.Phasing)
            //{
            //}
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        StartCoroutine(IntroRoutine());
                        //m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        CustomTurn();
                        //m_turnState = State.Intro;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        //    m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        switch (m_phaseHandle.currentPhase)
                        {
                            case Phase.PhaseOne:
                                UpdateRangeCache(m_info.normalAttack.range, m_info.specialThrustAttack.range);
                                ExecuteAttack(0);
                                break;
                            case Phase.PhaseTwo:
                                UpdateRangeCache(m_info.specialThrustAttack.range, m_info.specialThrustAttack.range);
                                ExecuteAttack(1);
                                break;
                            case Phase.PhaseThree:
                                UpdateRangeCache(m_info.earthShakerAttack.range, m_info.earthShakerAttack.range);
                                ExecuteAttack(2);
                                break;
                        }
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }

                    break;

                case State.Chasing:
                    m_attackCount = 0;
                    DecidedOnAttack(false);
                    //m_patternDecider.DecideOnAttack();
                    //m_chosenPattern = m_patternDecider.chosenAttack.attack;
                    //if (m_chosenPattern == m_previousPattern)
                    //{
                    //    m_patternDecider.hasDecidedOnAttack = false;
                    //}
                    //if (m_patternDecider.hasDecidedOnAttack)
                    //{
                    //    m_previousPattern = m_chosenPattern;
                    //    m_stateHandle.SetState(State.Attacking);
                    //}
                    m_stateHandle.SetState(State.Attacking);
                    break;

                case State.ReevaluateSituation:
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
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