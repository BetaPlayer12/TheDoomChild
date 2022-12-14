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
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using UnityEngine.Playables;
using DChild.Temp;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/TheOneThirdForm")]
    public class TheOneThirdFormAI : CombatAIBrain<TheOneThirdFormAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [TitleGroup("Phase Info")]

            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_moveSideways = new MovementInfo();
            public MovementInfo moveSideways => m_moveSideways;
            
            [TitleGroup("Pattern Ranges")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern2Range;
            public float phase1Pattern2Range => m_phase1Pattern2Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern3Range;
            public float phase1Pattern3Range => m_phase1Pattern3Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern4Range;
            public float phase1Pattern4Range => m_phase1Pattern4Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern5Range;
            public float phase1Pattern5Range => m_phase1Pattern5Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern3Range;
            public float phase2Pattern3Range => m_phase2Pattern3Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern4Range;
            public float phase2Pattern4Range => m_phase2Pattern4Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern5Range;
            public float phase2Pattern5Range => m_phase2Pattern5Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern6Range;
            public float phase2Pattern6Range => m_phase2Pattern6Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern1Range;
            public float phase3Pattern1Range => m_phase3Pattern1Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern2Range;
            public float phase3Pattern2Range => m_phase3Pattern2Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern3Range;
            public float phase3Pattern3Range => m_phase3Pattern3Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern4Range;
            public float phase3Pattern4Range => m_phase3Pattern4Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern5Range;
            public float phase3Pattern5Range => m_phase3Pattern5Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern6Range;
            public float phase3Pattern6Range => m_phase3Pattern6Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern7Range;
            public float phase3Pattern7Range => m_phase3Pattern7Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern1Range;
            public float phase4Pattern1Range => m_phase4Pattern1Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern2Range;
            public float phase4Pattern2Range => m_phase4Pattern2Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern3Range;
            public float phase4Pattern3Range => m_phase4Pattern3Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern4Range;
            public float phase4Pattern4Range => m_phase4Pattern4Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern5Range;
            public float phase4Pattern5Range => m_phase4Pattern5Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern6Range;
            public float phase4Pattern6Range => m_phase4Pattern6Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern7Range;
            public float phase4Pattern7Range => m_phase4Pattern7Range;
            [SerializeField, BoxGroup("Phase 4")]
            private float m_phase4Pattern8Range;
            public float phase4Pattern8Range => m_phase4Pattern8Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern1Range;
            public float phase5Pattern1Range => m_phase5Pattern1Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern2Range;
            public float phase5Pattern2Range => m_phase5Pattern2Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern3Range;
            public float phase5Pattern3Range => m_phase5Pattern3Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern4Range;
            public float phase5Pattern4Range => m_phase5Pattern4Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern5Range;
            public float phase5Pattern5Range => m_phase5Pattern5Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern6Range;
            public float phase5Pattern6Range => m_phase5Pattern6Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern7Range;
            public float phase5Pattern7Range => m_phase5Pattern7Range;
            [SerializeField, BoxGroup("Phase 5")]
            private float m_phase5Pattern8Range;
            public float phase5Pattern8Range => m_phase5Pattern8Range;

            [TitleGroup("Attack Pattern Cooldown States")]
            [SerializeField, MinValue(0)]
            private List<float> m_phase1PatternCooldown;
            public List<float> phase1PatternCooldown => m_phase1PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase2PatternCooldown;
            public List<float> phase2PatternCooldown => m_phase2PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase3PatternCooldown;
            public List<float> phase3PatternCooldown => m_phase3PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase4PatternCooldown;
            public List<float> phase4PatternCooldown => m_phase4PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase5PatternCooldown;
            public List<float> phase5PatternCooldown => m_phase5PatternCooldown;

            #region Animation
            [TitleGroup("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeShakeAnimation;
            public string eyeShakeAnimation => m_eyeShakeAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeArmoredAnimation;
            public string eyeArmoredAnimation => m_eyeArmoredAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeClosedAnimation;
            public string eyeClosedAnimation => m_eyeClosedAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeDefaultAnimation;
            public string eyeDefaultAnimation => m_eyeDefaultAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeMouthAnimation;
            public string eyeMouthAnimation => m_eyeMouthAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeMouthBlastAnimation;
            public string eyeMouthBlastAnimation => m_eyeMouthBlastAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeMouthBlastAnticipationAnimation;
            public string eyeMouthBlastAnticipationAnimation => m_eyeMouthBlastAnticipationAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_mouthIdleAnimation;
            public string mouthIdleAnimation => m_mouthIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeSquintAnimation;
            public string eyeSquintAnimation => m_eyeSquintAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeSquintTwoAnimation;
            public string eyeSquintTwoAnimation => m_eyeSquintTwoAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeWideOpenAnimation;
            public string eyeWideOpenAnimation => m_eyeWideOpenAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_eyeWideOpenTwoAnimation;
            public string eyeWideOpenTwoAnimation => m_eyeWideOpenTwoAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_mouthBlastBodyLoopAnimation;
            public string mouthBlastBodyLoopAnimation => m_mouthBlastBodyLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_animationAnimation;
            public string animationAnimation => m_animationAnimation;

            #endregion

            public override void Initialize()
            {
                
            }


        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_fullCooldown;
            public List<float> fullCooldown => m_fullCooldown;
            //[SerializeField]
            //private List<float> m_patternCooldown;
            //public List<float> patternCooldown => m_patternCooldown;
        }

        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Attacking,
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Phase1Pattern1,
            Phase1Pattern2,
            Phase1Pattern3,
            Phase1Pattern4,
            Phase1Pattern5,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            Phase2Pattern5,
            Phase2Pattern6,
            Phase3Pattern1,
            Phase3Pattern2,
            Phase3Pattern3,
            Phase3Pattern4,
            Phase3Pattern5,
            Phase3Pattern6,
            Phase3Pattern7,
            Phase4Pattern1,
            Phase4Pattern2,
            Phase4Pattern3,
            Phase4Pattern4,
            Phase4Pattern5,
            Phase4Pattern6,
            Phase4Pattern7,
            Phase4Pattern8,
            Phase5Pattern1,
            Phase5Pattern2,
            Phase5Pattern3,
            Phase5Pattern4,
            Phase5Pattern5,
            Phase5Pattern6,
            Phase5Pattern7,
            Phase5Pattern8,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            PhaseFive,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_model;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private List<float> m_currentFullCooldown;
        private List<float> m_patternCooldown;

        private Vector2 m_lastTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;

        #region Behavior Coroutines
        private Coroutine m_changePhaseCoroutine;
        private Coroutine m_currentAttackCoroutine;
        #endregion

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern4, m_info.phase1Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern5, m_info.phase1Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern5, m_info.phase2Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern6, m_info.phase2Pattern6Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern1, m_info.phase3Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern2, m_info.phase3Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern3, m_info.phase3Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern4, m_info.phase3Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern5, m_info.phase3Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern6, m_info.phase3Pattern6Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern7, m_info.phase3Pattern7Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern1, m_info.phase4Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern2, m_info.phase4Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern3, m_info.phase4Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern4, m_info.phase4Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern5, m_info.phase4Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern6, m_info.phase4Pattern6Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern7, m_info.phase4Pattern7Range)
                                    , new AttackInfo<Attack>(Attack.Phase4Pattern8, m_info.phase4Pattern8Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern1, m_info.phase5Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern2, m_info.phase5Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern3, m_info.phase5Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern4, m_info.phase5Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern5, m_info.phase5Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern6, m_info.phase5Pattern6Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern7, m_info.phase5Pattern7Range)
                                    , new AttackInfo<Attack>(Attack.Phase5Pattern8, m_info.phase5Pattern8Range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            //m_movement.Stop();
            //m_hitbox.Disable();
            //m_animation.animationState.TimeScale = 1;
            //m_animation.EnableRootMotion(true, false);
            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;
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

        protected override void Awake()
        {
            base.Awake();
            //m_damageable.DamageTaken += OnDamageTaken;
            //m_damageable.DamageTaken += OnDamageBlocked;
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(
                  Attack.Phase1Pattern1
                , Attack.Phase1Pattern2
                , Attack.Phase1Pattern3
                , Attack.Phase1Pattern4
                , Attack.Phase1Pattern5
                , Attack.Phase2Pattern1
                , Attack.Phase2Pattern2
                , Attack.Phase2Pattern3
                , Attack.Phase2Pattern4
                , Attack.Phase2Pattern5
                , Attack.Phase2Pattern6
                , Attack.Phase3Pattern1
                , Attack.Phase3Pattern2
                , Attack.Phase3Pattern3
                , Attack.Phase3Pattern4
                , Attack.Phase3Pattern5
                , Attack.Phase3Pattern6
                , Attack.Phase3Pattern7
                , Attack.Phase4Pattern1
                , Attack.Phase4Pattern2
                , Attack.Phase4Pattern3
                , Attack.Phase4Pattern4
                , Attack.Phase4Pattern5
                , Attack.Phase4Pattern6
                , Attack.Phase4Pattern7
                , Attack.Phase4Pattern8
                , Attack.Phase5Pattern1
                , Attack.Phase5Pattern2
                , Attack.Phase5Pattern3
                , Attack.Phase5Pattern4
                , Attack.Phase5Pattern5
                , Attack.Phase5Pattern6
                , Attack.Phase5Pattern7
                , Attack.Phase5Pattern8);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(
                  m_info.phase1Pattern1Range
                , m_info.phase1Pattern2Range
                , m_info.phase1Pattern3Range
                , m_info.phase1Pattern4Range
                , m_info.phase1Pattern5Range
                , m_info.phase2Pattern1Range
                , m_info.phase2Pattern2Range
                , m_info.phase2Pattern3Range
                , m_info.phase2Pattern4Range
                , m_info.phase2Pattern5Range
                , m_info.phase2Pattern6Range
                , m_info.phase3Pattern1Range
                , m_info.phase3Pattern2Range
                , m_info.phase3Pattern3Range
                , m_info.phase3Pattern4Range
                , m_info.phase3Pattern5Range
                , m_info.phase3Pattern6Range
                , m_info.phase3Pattern7Range
                , m_info.phase4Pattern1Range
                , m_info.phase4Pattern2Range
                , m_info.phase4Pattern3Range
                , m_info.phase4Pattern4Range
                , m_info.phase4Pattern5Range
                , m_info.phase4Pattern6Range
                , m_info.phase4Pattern7Range
                , m_info.phase4Pattern8Range
                , m_info.phase5Pattern1Range
                , m_info.phase5Pattern2Range
                , m_info.phase5Pattern3Range
                , m_info.phase5Pattern4Range
                , m_info.phase5Pattern5Range
                , m_info.phase5Pattern6Range
                , m_info.phase5Pattern7Range
                , m_info.phase5Pattern8Range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();

        }

        protected override void Start()
        {
            //base.Start();

            //m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        protected override void LateUpdate()
        {
            //base.LateUpdate();
        }

        public override void Enable()
        {
            //temp solution
        } 

        public override void Disable()
        {
            //temp solution
        }

        //private enum State
        //{
        //    Intro, 
        //    Phasing,
        //    Attacking,
        //    Idle,
        //    ReevaluateSituation,
        //    WaitBehaviourEnd,
        //}

        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;

        [SerializeField]
        private TheOneThirdFormAttacks m_theOneThirdFormAttacks;

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                //GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void ChangeState()
        {
            //StopCurrentAttackRoutine();
            //SetAIToPhasing();
            StartCoroutine(SmartChangePhaseRoutine());
        }

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            if (m_patternCooldown.Count != 0)
                m_patternCooldown.Clear();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4, Attack.Phase1Pattern5);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range, m_info.phase1Pattern5Range);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5, Attack.Phase2Pattern6);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, m_info.phase2Pattern6Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                    break;
                case Phase.PhaseThree:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase3Pattern1, Attack.Phase3Pattern2, Attack.Phase3Pattern3, Attack.Phase3Pattern4, Attack.Phase3Pattern5, Attack.Phase3Pattern6, Attack.Phase3Pattern7);
                    AddToRangeCache(m_info.phase3Pattern1Range, m_info.phase3Pattern2Range, m_info.phase3Pattern3Range, m_info.phase3Pattern4Range, m_info.phase3Pattern5Range, m_info.phase3Pattern6Range, m_info.phase3Pattern7Range);
                    for (int i = 0; i < m_info.phase3PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase3PatternCooldown[i]);
                    break;
                case Phase.PhaseFour:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase4Pattern1, Attack.Phase4Pattern2, Attack.Phase4Pattern3, Attack.Phase4Pattern4, Attack.Phase4Pattern5, Attack.Phase4Pattern6, Attack.Phase4Pattern7, Attack.Phase4Pattern8);
                    AddToRangeCache(m_info.phase4Pattern1Range, m_info.phase4Pattern2Range, m_info.phase4Pattern3Range, m_info.phase4Pattern4Range, m_info.phase4Pattern5Range, m_info.phase4Pattern6Range, m_info.phase4Pattern7Range, m_info.phase4Pattern8Range);
                    for (int i = 0; i < m_info.phase4PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase4PatternCooldown[i]);
                    break;
                case Phase.PhaseFive:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase5Pattern1, Attack.Phase5Pattern2, Attack.Phase5Pattern3, Attack.Phase5Pattern4, Attack.Phase5Pattern5, Attack.Phase5Pattern6, Attack.Phase5Pattern7, Attack.Phase5Pattern8);
                    AddToRangeCache(m_info.phase5Pattern1Range, m_info.phase5Pattern2Range, m_info.phase5Pattern3Range, m_info.phase5Pattern4Range, m_info.phase5Pattern5Range, m_info.phase5Pattern6Range, m_info.phase5Pattern7Range, m_info.phase5Pattern8Range);
                    for (int i = 0; i < m_info.phase5PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase5PatternCooldown[i]);
                    break;
            }
            m_attackUsed = new bool[m_attackCache.Count];
            if (m_currentFullCooldown.Count != 0)
            {
                m_currentFullCooldown.Clear();
            }
            for (int i = 0; i < obj.fullCooldown.Count; i++)
            {
                m_currentFullCooldown.Add(obj.fullCooldown[i]);
            }
        }

        private IEnumerator SmartChangePhaseRoutine()
        {
            yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            //StopCurrentBehaviorRoutine();
            //StopComboCounts();
            //ResetCounterCounts();
            SetAIToPhasing();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            m_stateHandle.Wait(State.Chasing);
            if (IsFacingTarget())
                CustomTurn();

            m_hitbox.Disable();
            m_animation.EnableRootMotion(true, false);
            //m_animation.SetAnimation(0, m_info.staggerAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staggerAnimation);
            //m_animation.SetAnimation(0, m_info.summonSwordsAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonSwordsAnimation);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.Enable();
            m_hitbox.SetCanBlockDamageState(false);
            m_changePhaseCoroutine = null;
            yield return null;
            enabled = true;
        }

        private void SetAIToPhasing()
        {
            m_phaseHandle.ApplyChange();
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_stateHandle.OverrideState(State.Phasing);
        }

        #region MouthBlastOne Attack
        private IEnumerator MouthBlastOneStart()
        {
            m_animation.SetAnimation(0, m_info.eyeClosedAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.eyeClosedAnimation);
            m_animation.SetAnimation(0, m_info.eyeMouthBlastAnticipationAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.eyeMouthBlastAnticipationAnimation);

            yield return SetPositionForMouthBlast();
        }

        private IEnumerator SetPositionForMouthBlast()
        {
            int side = Random.Range(0, 2);
            if (side == 0)
            {
                m_model.transform.position = new Vector2(m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneLeftSide.position.x, m_model.transform.position.y);
            }
            else if (side == 1)
            {
                m_model.transform.position = new Vector2(m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneRightSide.position.x, m_model.transform.position.y);
            }
            yield return MoveMouthBlast(side);
        }

        private IEnumerator MoveMouthBlast(int side)
        {
            m_animation.SetAnimation(0, m_info.eyeMouthBlastAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.eyeMouthBlastAnimation);

            if(side == 0)
            {
                StartCoroutine(m_theOneThirdFormAttacks.mouthBlastOneAttack.ExecuteAttack());
                while (m_model.transform.position.x < m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneRightSide.position.x)
                {
                    m_model.transform.position = Vector2.MoveTowards(m_model.transform.position,
                        new Vector2(m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneRightSide.position.x,
                        m_model.transform.position.y), m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneMoveSpeed);                    
                    yield return new WaitForSeconds(0.002f*GameplaySystem.time.deltaTime);
                }
            }
            else if(side == 1)
            {
                StartCoroutine(m_theOneThirdFormAttacks.mouthBlastOneAttack.ExecuteAttack());
                while (m_model.transform.position.x > m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneLeftSide.position.x)
                {
                    m_model.transform.position = Vector2.MoveTowards(m_model.transform.position,
                        new Vector2(m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneLeftSide.position.x,
                        m_model.transform.position.y), m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneMoveSpeed);
                    yield return new WaitForSeconds(0.002f * GameplaySystem.time.deltaTime);
                }
            }
        }

        private IEnumerator MouthBlastEnd(Vector2 OriginalPosition)
        {
            StartCoroutine(m_theOneThirdFormAttacks.mouthBlastOneAttack.EndMouthBlast());
            while (m_model.transform.position.x != OriginalPosition.x)
            {
                m_model.transform.position = Vector2.MoveTowards(m_model.transform.position,
                    OriginalPosition, m_theOneThirdFormAttacks.mouthBlastOneAttack.mouthBlastOneMoveSpeed);
                yield return new WaitForSeconds(0.002f * GameplaySystem.time.deltaTime);
            }
        }

        private IEnumerator FullMouthBlastOneSequence()
        {
            Vector2 originalPosition = m_model.transform.position;
            yield return MouthBlastOneStart();
            yield return MouthBlastEnd(originalPosition);
        }

        [Button]
        private void TestMouthBlastI()
        {
            StartCoroutine(FullMouthBlastOneSequence());
        }
        #endregion

        #region AttackCoroutines
        private IEnumerator TentacleGroundStab(float cooldown)
        {
            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.TentacleGroundStab(m_targetInfo));

            yield return new WaitForSeconds(cooldown);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ChasingGroundTentacle(float cooldown)
        {
            var monolithPlatformsPresent = FindObjectOfType<ObstacleChecker>().monolithSlamObstacleList;

            if(monolithPlatformsPresent != null)
            {
                yield return null;
            }

            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.ChasingGroundTentacle());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TentacleBlastOne(float cooldown)
        {
            if (!m_targetInfo.isCharacterGrounded)
            {
                yield return null;
            }

            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.TentacleBlastOne(m_targetInfo));
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TentacleBlastTwo(float cooldown)
        {
            if (!m_targetInfo.isCharacterGrounded)
            {
                yield return null;
            }

            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.TentacleBlastTwo());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator MonolithSlam(float cooldown)
        {
            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.MonolithSlam(m_targetInfo));
            yield return new WaitForSeconds(cooldown);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator MouthBlastWall(float cooldown)
        {
            var monolithPlatformsPresent = FindObjectOfType<ObstacleChecker>().monolithSlamObstacleList;

            if (monolithPlatformsPresent != null)
            {
                yield return null;
            }

            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.MouthBlastWall());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TentacleCeiling(float cooldown)
        {
            if (!m_targetInfo.isCharacterGrounded)
            {
                yield return null;
            }

            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.TentacleCeilingAttack());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator MovingTentacleGround(float cooldown)
        {
            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.MovingTentacleGround());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator MouthBlastOne(float cooldown)
        {
            m_currentAttackCoroutine = StartCoroutine(FullMouthBlastOneSequence());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator SlidingWall(float cooldown)
        {
            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.SlidingStoneWallAttack(m_targetInfo));
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ScriptedTentacleGrab(float cooldown)
        {
            if (!m_targetInfo.isCharacterGrounded)
            {
                yield return null;
            }

            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.TentacleGrab());
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator BubbleImprisonment(float cooldown)
        {
            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.BubbleImprisonment(m_targetInfo));
            yield return new WaitForSeconds(cooldown);
            //Temporary
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
        }
        #endregion

        void Update()
        { 
            m_phaseHandle.MonitorPhase();

            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    StartCoroutine(IntroRoutine());
                    break;
                case State.Phasing:
                    if (m_changePhaseCoroutine == null)
                    {
                        m_changePhaseCoroutine = StartCoroutine(ChangePhaseRoutine());
                    }
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;

                    m_animation.SetAnimation(0, m_info.eyeSquintAnimation, true);

                    Debug.Log("CURRENT ATTACK PATTERN " + m_currentAttack);
                    switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_pickedCooldown = m_currentFullCooldown[0];

                            Debug.Log("Tentacle Stab Attack");

                            StartCoroutine(TentacleGroundStab(m_info.phase1PatternCooldown[0]));

                            break;
                        case Attack.Phase1Pattern2:
                            m_pickedCooldown = m_currentFullCooldown[1];

                            Debug.Log("Chasing Ground Tentacle");

                            StartCoroutine(ChasingGroundTentacle(m_info.phase1PatternCooldown[1]));
 
                            break;
                        case Attack.Phase1Pattern3:
                            m_pickedCooldown = m_currentFullCooldown[2];

                            Debug.Log("TENTACLE BLAST I ATTACK");

                            StartCoroutine(TentacleBlastOne(m_info.phase1PatternCooldown[2]));
         
                            break;
                        case Attack.Phase1Pattern4:
                            m_pickedCooldown = m_currentFullCooldown[3];

                            Debug.Log("MONILITH SLAM ATTACK");

                            StartCoroutine(MonolithSlam(m_info.phase1PatternCooldown[3]));

                            break;
                        case Attack.Phase1Pattern5:
                            m_pickedCooldown = m_currentFullCooldown[4];

                            Debug.Log("MOUTH BLAST WALL");

                            StartCoroutine(MouthBlastWall(m_info.phase1PatternCooldown[4]));

                            break;
                        case Attack.Phase2Pattern1:
                            m_pickedCooldown = m_currentFullCooldown[0];

                            Debug.Log("TENTACLE GROUND STAB");

                            m_currentAttackCoroutine = StartCoroutine(TentacleGroundStab(m_info.phase2PatternCooldown[0]));

                            break;
                        case Attack.Phase2Pattern2:
                            m_pickedCooldown = m_currentFullCooldown[1];

                            Debug.Log("CHASING GROUND TENTACLE");

                            m_currentAttackCoroutine = StartCoroutine(ChasingGroundTentacle(m_info.phase2PatternCooldown[1]));

                            break;
                        case Attack.Phase2Pattern3:
                            m_pickedCooldown = m_currentFullCooldown[2];

                            Debug.Log("TENTACLE BLAST II ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(TentacleBlastTwo(m_info.phase2PatternCooldown[2]));

                            break;
                        case Attack.Phase2Pattern4:
                            m_pickedCooldown = m_currentFullCooldown[3];

                            Debug.Log("MONOLITH SLAM ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(MonolithSlam(m_info.phase2PatternCooldown[3]));

                            break;
                        case Attack.Phase2Pattern5:
                            m_pickedCooldown = m_currentFullCooldown[4];

                            Debug.Log("MOUTH BLAST WALL");

                            m_currentAttackCoroutine = StartCoroutine(MouthBlastWall(m_info.phase2PatternCooldown[4]));

                            break;
                        case Attack.Phase2Pattern6:
                            m_pickedCooldown = m_currentFullCooldown[5];

                            Debug.Log("TENTACLE CEILING");

                            m_currentAttackCoroutine = StartCoroutine(TentacleCeiling(m_info.phase2PatternCooldown[5]));

                            break;
                        case Attack.Phase3Pattern1:
                            m_pickedCooldown = m_currentFullCooldown[0];

                            Debug.Log("TENTACLE GROUND STAB");

                            m_currentAttackCoroutine = StartCoroutine(TentacleGroundStab(m_info.phase3PatternCooldown[0]));

                            break;
                        case Attack.Phase3Pattern2:
                            m_pickedCooldown = m_currentFullCooldown[1];

                            //Debug.Log("TENTACLE GARDEN / CHASING GROUND TENTACLE ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(ChasingGroundTentacle(m_info.phase3PatternCooldown[0]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase3Pattern3:
                            m_pickedCooldown = m_currentFullCooldown[2];

                            //Debug.Log("TENTACLE BLAST I/II ATTACK");

                            //int randomTentacleBlast = Random.Range(0, 2);
                            //if(randomTentacleBlast == 0)
                            //    m_currentAttackCoroutine = StartCoroutine(TentacleBlastOne(m_info.phase3PatternCooldown[2]));
                            //else if(randomTentacleBlast == 1)
                            //    m_currentAttackCoroutine = StartCoroutine(TentacleBlastTwo(m_info.phase3PatternCooldown[2]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase3Pattern4:
                            m_pickedCooldown = m_currentFullCooldown[3];

                            //Debug.Log("MONILITH SLAM ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(MonolithSlam(m_info.phase3PatternCooldown[3]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase3Pattern5:
                            m_pickedCooldown = m_currentFullCooldown[4];

                            Debug.Log("MOUTH BLAST WALL");

                            m_currentAttackCoroutine = StartCoroutine(MouthBlastWall(m_info.phase3PatternCooldown[4]));

                            break;
                        case Attack.Phase3Pattern6:
                            m_pickedCooldown = m_currentFullCooldown[5];

                            Debug.Log("TENTACLE CEILING");

                            m_currentAttackCoroutine = StartCoroutine(TentacleCeiling(m_info.phase3PatternCooldown[5]));

                            break;
                        case Attack.Phase3Pattern7:
                            m_pickedCooldown = m_currentFullCooldown[6];

                            Debug.Log("BUBBLE IMPRISONMENT ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(BubbleImprisonment(m_info.phase3PatternCooldown[6]));

                            break;
                        case Attack.Phase4Pattern1:
                            m_pickedCooldown = m_currentFullCooldown[0];

                            Debug.Log("TENTACLE GROUND STAB ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(TentacleGroundStab(m_info.phase4PatternCooldown[0]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase4Pattern2:
                            m_pickedCooldown = m_currentFullCooldown[1];

                            Debug.Log("TENTACLE GARDEN / CHASING GROUND TENTACLE ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(ChasingGroundTentacle(m_info.phase4PatternCooldown[1]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase4Pattern3:
                            m_pickedCooldown = m_currentFullCooldown[2];

                            Debug.Log("TENTACLE BLAST II ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(TentacleBlastTwo(m_info.phase4PatternCooldown[2]));

                            break;
                        case Attack.Phase4Pattern4:
                            m_pickedCooldown = m_currentFullCooldown[3];

                            Debug.Log("MONOLITH SLAM ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(MonolithSlam(m_info.phase4PatternCooldown[3]));
                            
                            break;
                        case Attack.Phase4Pattern5:
                            m_pickedCooldown = m_currentFullCooldown[4];

                            Debug.Log("MOUTH BLAST II ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(MouthBlastWall(m_info.phase4PatternCooldown[4]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase4Pattern6:
                            m_pickedCooldown = m_currentFullCooldown[5];

                            Debug.Log("Tentacle Stab Attack");

                            m_currentAttackCoroutine = StartCoroutine(TentacleGroundStab(m_info.phase4PatternCooldown[5]));

                            break;
                        case Attack.Phase4Pattern7:
                            m_pickedCooldown = m_currentFullCooldown[6];

                            Debug.Log("BUBBLE IMPRISONMENT ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(BubbleImprisonment(m_info.phase4PatternCooldown[6]));

                            break;
                        case Attack.Phase4Pattern8:
                            m_pickedCooldown = m_currentFullCooldown[7];

                            Debug.Log("GRABBER SWIPE + WALL SLAM ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(m_theOneThirdFormAttacks.TentacleGrab());

                            //Temporary
                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            //Temporary
                            break;
                        case Attack.Phase5Pattern1:
                            m_pickedCooldown = m_currentFullCooldown[0];

                            Debug.Log("Tentacle Stab Attack");

                            m_currentAttackCoroutine = StartCoroutine(TentacleGroundStab(m_info.phase5PatternCooldown[0]));

                            break;
                        case Attack.Phase5Pattern2:
                            m_pickedCooldown = m_currentFullCooldown[1];

                            Debug.Log("TENTACLE GARDEN / CHASING GROUND TENTACLE ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(ChasingGroundTentacle(m_info.phase5PatternCooldown[1]));

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase5Pattern3:
                            m_pickedCooldown = m_currentFullCooldown[2];

                            Debug.Log("TENTACLE BLAST II ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(TentacleBlastTwo(m_info.phase5PatternCooldown[2]));

                            break;
                        case Attack.Phase5Pattern4:
                            m_pickedCooldown = m_currentFullCooldown[3];

                            Debug.Log("MONOLITH SLAM ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(MonolithSlam(m_info.phase5PatternCooldown[3]));

                            break;
                        case Attack.Phase5Pattern5:
                            m_pickedCooldown = m_currentFullCooldown[4];

                            Debug.Log("MOUTH BLAST ATTACK");

                            //m_currentAttackCoroutine = StartCoroutine(FullMouthBlastOneSequence());

                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            m_stateHandle.ApplyQueuedState();
                            break;
                        case Attack.Phase5Pattern6:
                            m_pickedCooldown = m_currentFullCooldown[5];

                            Debug.Log("TENTACLE CEILING");

                            m_currentAttackCoroutine = StartCoroutine(TentacleCeiling(m_info.phase5PatternCooldown[5]));

                            break;
                        case Attack.Phase5Pattern7:
                            m_pickedCooldown = m_currentFullCooldown[6];

                            Debug.Log("BUBBLE IMPRISONMENT ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(BubbleImprisonment(m_info.phase5PatternCooldown[6]));

                            break;
                        case Attack.Phase5Pattern8:
                            m_pickedCooldown = m_currentFullCooldown[7];

                            Debug.Log("GRABBER SWIPE + WALL SLAM ATTACK");

                            m_currentAttackCoroutine = StartCoroutine(ScriptedTentacleGrab(m_info.phase5PatternCooldown[7]));

                            break;
                    }
                    break;

                case State.Cooldown:
                    //m_animation.SetAnimation(0, m_idleAnimation, true).TimeScale = 1;

                    if (m_currentCooldown <= m_pickedCooldown)
                    {
                        m_currentCooldown += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCooldown = 0;
                        m_attackDecider.hasDecidedOnAttack = false;
                        m_currentAttackCoroutine = null;
                        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    if (!m_hitbox.canBlockDamage)
                    {
                        ChooseAttack();
                        if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                    }
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

            //if (m_doMouthBlastIAttack)
            //{
            //    //var rollSide = Random.Range(0, 2);
            //    m_doMouthBlastIAttack = false;
            //    StartCoroutine(MouthBlastOneAttack(m_SideToStart));
            //    //
            //}

            //if (m_moveMouth)
            //{
            //    StartCoroutine(MoveMouthBlast(m_SideToStart));
            //}


            //transform.position = Vector2.MoveTowards(transform.position, m_mouthBlastLeftSide.position, m_mouthBlastMoveSpeed);
        }

        protected override void OnForbidFromAttackTarget()
        {
            
        }

        protected override void OnTargetDisappeared()
        {
            
        }

        public override void ReturnToSpawnPoint()
        {
            
        }
    }
}

