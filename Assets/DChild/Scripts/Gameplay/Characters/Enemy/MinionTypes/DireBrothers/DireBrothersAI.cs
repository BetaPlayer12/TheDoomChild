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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/DireBrothers")]
    public class DireBrothersAI : CombatAIBrain<DireBrothersAI.Info>, IResetableAIBrain, IBattleZoneAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            //Basic Behaviours
            [SerializeField, TitleGroup("Movement")]
            private MovementInfo m_sh_walk1 = new MovementInfo();
            public MovementInfo sh_walk1 => m_sh_walk1;
            [SerializeField, TitleGroup("Movement")]
            private MovementInfo m_sh_walk2 = new MovementInfo();
            public MovementInfo sh_walk2 => m_sh_walk2;
            [SerializeField, TitleGroup("Movement")]
            private MovementInfo m_sh_run = new MovementInfo();
            public MovementInfo sh_run => m_sh_run;
            [SerializeField, TitleGroup("Movement")]
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;

            //Attack Behaviours
            [SerializeField, TitleGroup("Attacks")]
            private SimpleAttackInfo m_heavyGroundStabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStabAttack => m_heavyGroundStabAttack;
            [SerializeField, TitleGroup("Attacks")]
            private SimpleAttackInfo m_heavyGroundBashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundBashAttack => m_heavyGroundBashAttack;
            //[SerializeField, TitleGroup("Attacks"), ValueDropdown("GetAnimations")]
            //private string m_heavyGroundStabStuckAnimation;
            //public string heavyGroundStabStuckAnimation => m_heavyGroundStabStuckAnimation;
            //[SerializeField, TitleGroup("Attacks"), ValueDropdown("GetAnimations")]
            //private string m_heavyGroundStabRecoverAnimation;
            //public string heavyGroundStabRecoverAnimation => m_heavyGroundStabRecoverAnimation;
            [SerializeField, TitleGroup("Attacks")]
            private SimpleAttackInfo m_spearAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearAttack => m_spearAttack;
            [SerializeField, TitleGroup("Attacks")]
            private SimpleAttackInfo m_shieldDashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shieldDashAttack => m_shieldDashAttack;
            [SerializeField, TitleGroup("Attacks")]
            private SimpleAttackInfo m_shieldBashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shieldBashAttack => m_shieldBashAttack;
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField, MinValue(0)]
            private float m_shieldDashDuration;
            public float shieldDashDuration => m_shieldDashDuration;

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_sh_death1Animation;
            public string sh_death1Animation => m_sh_death1Animation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_sh_death2Animation;
            public string sh_death2Animation => m_sh_death2Animation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_sh_stopAnimation;
            public string sh_stopAnimation => m_sh_stopAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_sh_shieldBashAnimation;
            public string sh_shieldBashAnimation => m_sh_shieldBashAnimation;
            //Animations
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_heavyFlinchAnimation;
            public string heavyFlinchAnimation => m_heavyFlinchAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_lightFlinchAnimation;
            public string lightFlinchAnimation => m_lightFlinchAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_backFlinchAnimation;
            public string backFlinchAnimation => m_backFlinchAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_counterFlinchAnimation;
            public string counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_defenceStanceAnimation;
            public string defenceStanceAnimation => m_defenceStanceAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_defenceWhenHitAnimation;
            public string defenceWhenHitAnimation => m_defenceWhenHitAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_exhaustedAnimation;
            public string exhaustedAnimation => m_exhaustedAnimation;
            [SerializeField, TitleGroup("Animations"), ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_sh_walk1.SetData(m_skeletonDataAsset);
                m_sh_walk2.SetData(m_skeletonDataAsset);
                m_sh_run.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_heavyGroundStabAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundBashAttack.SetData(m_skeletonDataAsset);
                m_spearAttack.SetData(m_skeletonDataAsset);
                m_shieldDashAttack.SetData(m_skeletonDataAsset);
                m_shieldBashAttack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private SkeletonDataAsset m_skeletonDataAsset;

            public void SetData(SkeletonDataAsset skeletonData) => m_skeletonDataAsset = skeletonData;

            protected IEnumerable GetAnimations()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            }

            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_walkAnimation;
            public string walkAnimation => m_walkAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_runAnimation;
            public string runAnimation => m_runAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
        }

        private enum State
        {
            Detect,
            Patrol,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            HeavyGroundAttack,
            HeavyGroundBashAttack,
            SpearAttack,
            ShieldDash,
            ShieldBash,
            [HideInInspector]
            _COUNT
        }

        public enum Phase
        {
            BothAlive,
            SpearDead,
            Wait,
        }
        
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_boundBoxGO;
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
        private FlinchHandler m_flinch1Handle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinch2Handle;

        private FlinchHandler m_currentFlinchHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentShieldDashAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("Hurtbox")]
        private Transform m_slamBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Transform m_chargeBB;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_chargePhaseFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_effectsHolderFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_shieldGoingUpFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_dashFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_dashEndFX;

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

        private State m_turnState;

        private string m_currentIdleAnimation;
        private string m_currentWalkAnimation;
        private string m_currentRunAnimation;
        private string m_currentTurnAnimation;

        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_changePhaseCoroutine;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentIdleAnimation = obj.idleAnimation;
            m_currentWalkAnimation = m_phaseHandle.currentPhase == Phase.BothAlive ? obj.walkAnimation : (UnityEngine.Random.Range(0, 2) == 0 ? m_info.sh_walk1.animation : m_info.sh_walk2.animation);
            m_currentRunAnimation = obj.runAnimation;
            m_currentTurnAnimation = obj.turnAnimation;
            m_currentFlinchHandle = m_phaseHandle.currentPhase == Phase.BothAlive ? m_flinch1Handle : m_flinch2Handle;

            switch (m_phaseHandle.currentPhase)
            {
                case Phase.BothAlive:
                    m_flinch1Handle.gameObject.SetActive(true);
                    m_flinch2Handle.gameObject.SetActive(false);
                    break;
                case Phase.SpearDead:
                    m_flinch1Handle.gameObject.SetActive(false);
                    m_flinch2Handle.gameObject.SetActive(true);
                    break;
            }
            m_phaseHandle.allowPhaseChange = true;
        }

        private void ChangeState()
        {
            if (m_changePhaseCoroutine == null)
            {
                StartCoroutine(SmartChangePhaseRoutine());
            }
        }

        private IEnumerator SmartChangePhaseRoutine()
        {
            yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(1, 0);
            m_animation.SetEmptyAnimation(2, 0);
            StopAllCoroutines();
            m_phaseHandle.allowPhaseChange = false;
            m_wallSensor.gameObject.SetActive(true);
            m_chargePhaseFX.Stop();
            m_effectsHolderFX.Stop();
            m_shieldGoingUpFX.Stop();
            m_dashFX.Stop();
            m_dashEndFX.Stop();
            Debug.Log("DireBrothers Change State");
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
            }
            //SetAIToPhasing();
            m_changePhaseCoroutine = StartCoroutine(ChangePhaseRoutine());
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_currentFlinchHandle.gameObject.SetActive(false);
            m_attackCache.Clear();
            AddToAttackCache(Attack.ShieldDash/*, Attack.ShieldBash*/);
            m_attackRangeCache.Clear();
            AddToRangeCache(m_info.shieldDashAttack.range/*, m_info.shieldBashAttack.range*/);
            m_attackUsed = new bool[m_attackCache.Count];
            m_hitbox.gameObject.SetActive(false);
            m_movement.Stop();
            var sp_deathAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.sh_death1Animation : m_info.sh_death2Animation;
            m_animation.SetAnimation(0, sp_deathAnim, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, sp_deathAnim);
            m_currentFlinchHandle.gameObject.SetActive(true);
            m_hitbox.gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_phaseHandle.ApplyChange();
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_changePhaseCoroutine = null;
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.SetState(State.Turning);
            m_phaseHandle.allowPhaseChange = false;
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

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_wallSensor.gameObject.SetActive(true);
            m_stateHandle.ApplyQueuedState();
            m_phaseHandle.allowPhaseChange = true;
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
                m_selfCollider.SetActive(false);
                m_targetInfo.Set(null, null);
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_animation.EnableRootMotion(true, false);
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
            }
            m_selfCollider.SetActive(false);
            m_hitbox.gameObject.SetActive(false);
            m_boundBoxGO.SetActive(false);
            m_movement.Stop();
            m_dashFX.Stop();
            m_dashFX.gameObject.SetActive(false);
            m_dashEndFX.Stop();
            m_dashEndFX.gameObject.SetActive(false);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_currentIdleAnimation && m_changePhaseCoroutine == null)
            {
                m_currentFlinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_currentFlinchHandle.m_autoFlinch)
            {
                m_currentFlinchHandle.m_autoFlinch = false;
                m_stateHandle.ApplyQueuedState();
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

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.HeavyGroundAttack, m_info.heavyGroundStabAttack.range),
                                    new AttackInfo<Attack>(Attack.HeavyGroundBashAttack, m_info.heavyGroundBashAttack.range),
                                    new AttackInfo<Attack>(Attack.SpearAttack, m_info.spearAttack.range),
                                    new AttackInfo<Attack>(Attack.ShieldDash, m_info.shieldDashAttack.range)/*,
                                    new AttackInfo<Attack>(Attack.ShieldBash, m_info.shieldBashAttack.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator HeavyGroundAttackRoutine()
        {
            m_chargePhaseFX.Play();
            m_effectsHolderFX.Play();
            m_shieldGoingUpFX.Play();
            m_currentFlinchHandle.m_enableMixFlinch = false;
            m_animation.SetAnimation(0, m_info.heavyGroundStabAttack.animation, false);
            yield return new WaitForSeconds(1.5f); 
            m_character.physics.SetVelocity(50 * transform.localScale.x, 0);
            yield return new WaitForSeconds(.6f);
            m_animation.SetEmptyAnimation(1, 0);
            m_currentFlinchHandle.m_enableMixFlinch = true;
            m_movement.Stop();
            //m_character.physics.SetVelocity(Vector2.zero);
            //m_animation.EnableRootMotion(true, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundStabAttack.animation);
            //m_animation.SetAnimation(0, m_info.heavyGroundStabStuckAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundStabStuckAnimation);
            //m_animation.SetAnimation(0, m_info.heavyGroundStabRecoverAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundStabRecoverAnimation);
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator HeavyGroundBashAttackRoutine()
        {
            m_chargePhaseFX.Play();
            m_effectsHolderFX.Play();
            m_shieldGoingUpFX.Play();
            m_slamBB.gameObject.SetActive(true);
            m_chargeBB.gameObject.SetActive(false);
            m_currentFlinchHandle.m_enableMixFlinch = false;
            m_animation.SetAnimation(0, m_info.heavyGroundBashAttack.animation, false);
            yield return new WaitForSeconds(1.5f);
            m_character.physics.SetVelocity(50 * transform.localScale.x, 0);
            yield return new WaitForSeconds(.6f);
            m_animation.SetEmptyAnimation(1, 0);
            m_currentFlinchHandle.m_enableMixFlinch = true;
            m_movement.Stop();
            //m_character.physics.SetVelocity(Vector2.zero);
            //m_animation.EnableRootMotion(true, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundBashAttack.animation);
            //m_animation.SetAnimation(0, m_info.heavyGroundStabStuckAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundStabStuckAnimation);
            //m_animation.SetAnimation(0, m_info.heavyGroundStabRecoverAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundStabRecoverAnimation);
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ShieldDashAttackRoutine(Vector2 targetPos)
        {
            m_dashFX.gameObject.SetActive(true);
            m_dashFX.Play();
            m_slamBB.gameObject.SetActive(false);
            m_chargeBB.gameObject.SetActive(true);
            //m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.sh_run.animation, true);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttack.animation);
            while (m_currentShieldDashAttackDuration < m_info.shieldDashDuration && !m_wallSensor.isDetecting /*Vector2.Distance(targetPos, transform.position) > m_info.shieldBashAttack.range*/)
            {
                //m_currentRunAttackDuration += Time.deltaTime;
                m_currentShieldDashAttackDuration += Time.deltaTime;
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.sh_run.speed);
                yield return null;
            }
            m_dashFX.gameObject.SetActive(false);
            m_dashFX.Stop();
            m_dashEndFX.gameObject.SetActive(true);
            m_dashEndFX.Play();
            m_currentShieldDashAttackDuration = 0;
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.sh_stopAnimation, false);
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.5f;
            yield return new WaitForSeconds(waitTime);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.sh_stopAnimation);
            m_animation.SetAnimation(0, m_currentIdleAnimation, true).MixDuration = 0.25f;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        //private IEnumerator ShieldBashAttackRoutine()
        //{
        //    m_animation.SetAnimation(0, m_info.shieldBashAttack.animation, false);
        //    //m_character.physics.SetVelocity(Vector2.zero);
        //    //m_animation.EnableRootMotion(true, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shieldBashAttack.animation);
        //    m_animation.SetAnimation(0, m_currentIdleAnimation, true);
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

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
            m_selfCollider.SetActive(false);
            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.BothAlive, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_currentFlinchHandle.FlinchStart += OnFlinchStart;
            m_currentFlinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache(/*Attack.HeavyGroundAttack,*/ Attack.HeavyGroundBashAttack, Attack.SpearAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(/*m_info.heavyGroundStabAttack.range,*/ m_info.heavyGroundBashAttack.range, m_info.spearAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_currentWalkAnimation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.sh_walk1.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_wallSensor.gameObject.SetActive(false);
                    m_dashEndFX.Stop();
                    m_dashEndFX.gameObject.SetActive(false);
                    m_turnHandle.Execute(m_currentTurnAnimation, m_currentIdleAnimation);
                    break;

                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        if (IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting)
                        {
                            m_stateHandle.Wait(State.Cooldown);
                            m_animation.SetAnimation(0, m_currentIdleAnimation, true);

                            switch (/*m_attackDecider.chosenAttack.attack*/ m_currentAttack)
                            {
                                case Attack.HeavyGroundAttack:
                                    m_currentAttackCoroutine = StartCoroutine(HeavyGroundAttackRoutine());
                                    break;
                                case Attack.HeavyGroundBashAttack:
                                    m_currentAttackCoroutine = StartCoroutine(HeavyGroundBashAttackRoutine());
                                    break;
                                case Attack.SpearAttack:
                                    m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.spearAttack.animation, m_currentIdleAnimation);
                                    break;
                                case Attack.ShieldDash:
                                    m_currentAttackCoroutine = StartCoroutine(ShieldDashAttackRoutine(m_targetInfo.position));
                                    break;
                                case Attack.ShieldBash:
                                    //m_currentAttackCoroutine = StartCoroutine(ShieldBashAttackRoutine());

                                    m_currentAttackCoroutine = null;
                                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                                    break;
                            }
                            m_attackDecider.hasDecidedOnAttack = false;
                        }
                        else
                        {
                            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                            {
                                m_slamBB.gameObject.SetActive(false);
                                m_chargeBB.gameObject.SetActive(true);
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, m_currentRunAnimation, true);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_phaseHandle.currentPhase == Phase.BothAlive ? m_info.run.speed : m_info.sh_run.speed);
                            }
                            else
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                            }
                        }
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (IsTargetInRange(100))
                    {
                        if (!IsFacingTarget())
                        {
                            m_turnState = State.Cooldown;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                                m_stateHandle.SetState(State.Turning);
                        }
                        else
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                        }
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                    }

                    if (m_currentCD <= m_info.attackCD && !m_wallSensor.isDetecting)
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
                        //m_attackDecider.DecideOnAttack();
                        m_attackDecider.hasDecidedOnAttack = false;
                        ChooseAttack();
                        if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                        {
                            m_movement.Stop();
                            m_stateHandle.SetState(State.Attacking);
                        }
                        //else
                        //{
                        //    m_attackDecider.hasDecidedOnAttack = false;
                        //    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                        //    {
                        //        var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                        //        m_animation.EnableRootMotion(false, false);
                        //        m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
                        //        m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                        //    }
                        //    else
                        //    {
                        //        m_movement.Stop();
                        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        //    }
                        //}
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
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_targetInfo.isValid)
            {
                if (m_enablePatience && TargetBlocked())
                {
                    Patience();
                }
                //StartCoroutine(PatienceRoutine());
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_hitbox.Enable();
            m_boundBoxGO.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(true);
        }

        public void SwitchToBattleZoneAI()
        {
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_stateHandle.SetState(State.ReevaluateSituation);
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
