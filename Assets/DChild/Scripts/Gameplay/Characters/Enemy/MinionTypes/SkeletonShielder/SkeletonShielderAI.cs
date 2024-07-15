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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SkeletonShielder")]
    public class SkeletonShielderAI : CombatAIBrain<SkeletonShielderAI.Info>, IResetableAIBrain, IBattleZoneAIBrain
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
            private SimpleAttackInfo m_ShieldBash = new SimpleAttackInfo();
            public SimpleAttackInfo ShieldBash => m_ShieldBash;
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            #region NoGUARD
            //Animations
            [SerializeField]
            private BasicAnimationInfo m_shieldChargeReciolAnimation;
            public BasicAnimationInfo shieldChargeReciolAnimation => m_shieldChargeReciolAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleGuardAnimation;
            public BasicAnimationInfo idleGuardAnimation => m_idleGuardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinch1Animation;
            public BasicAnimationInfo flinch1Animation => m_flinch1Animation;
            [SerializeField]
            private BasicAnimationInfo m_flinch2Animation;
            public BasicAnimationInfo flinch2Animation => m_flinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation2;
            public BasicAnimationInfo deathAnimation2 => m_deathAnimation2;

            [SerializeField]
            private BasicAnimationInfo m_shieldDestroyed;
            public BasicAnimationInfo shieldDestroyed => m_shieldDestroyed;


            #endregion

            #region Guard
            [SerializeField]
            private MovementInfo m_march;
            public MovementInfo march => m_march;
            #endregion

            #region NoShield
            [SerializeField]
            private MovementInfo m_noShieldRun;
            public MovementInfo noShieldRun => m_noShieldRun;
            [SerializeField]
            private BasicAnimationInfo m_noShieldCrouch;
            public BasicAnimationInfo noShieldCrouch => m_noShieldCrouch;
            [SerializeField]
            private BasicAnimationInfo m_noShieldTurn;
            public BasicAnimationInfo noShieldTurn => m_noShieldTurn;
            [SerializeField]
            private BasicAnimationInfo m_noShieldIdle;
            public BasicAnimationInfo noShieldIdle => m_noShieldIdle;

            [SerializeField]
            private MovementInfo m_noShieldMove = new MovementInfo();
            public MovementInfo noShieldMove => m_noShieldMove;
            [SerializeField]
            private BasicAnimationInfo m_noShieldFlinch;
            public BasicAnimationInfo noShieldFlinch => m_noShieldFlinch;
            #endregion


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                ShieldBash.SetData(m_skeletonDataAsset);

                m_shieldChargeReciolAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleGuardAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinch1Animation.SetData(m_skeletonDataAsset);
                m_flinch2Animation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_noShieldCrouch.SetData(m_skeletonDataAsset);
                m_noShieldRun.SetData(m_skeletonDataAsset);
                m_shieldDestroyed.SetData(m_skeletonDataAsset);
                m_noShieldTurn.SetData(m_skeletonDataAsset);
                m_noShieldIdle.SetData(m_skeletonDataAsset);
                m_noShieldMove.SetData(m_skeletonDataAsset);
                m_march.SetData(m_skeletonDataAsset);
                m_deathAnimation2.SetData(m_skeletonDataAsset);
                m_noShieldFlinch.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum MinionMode
        {
            NoGuard,
            Guard,
            NoShield,
            SwitchingToOtherMode
        }
        private enum State
        {
            Patrol,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            Flee,
            Crouching,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            ShieldBash,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_boundBoxGO;
        [SerializeField, TabGroup("Reference")]
        private Hitbox hitbox;
        [SerializeField, TabGroup("Reference")]
        private Damageable m_shieldDamageable;
        [SerializeField, TabGroup("Reference")]
        private Attacker m_shieldAttacker;
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
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_playerSensor;

        [SerializeField, TabGroup("VFX")]
        private ParticleSystem m_shieldGlow;
        [SerializeField, TabGroup("VFX")]
        private ParticleSystem m_shieldBreakVFX;
        [SerializeField, TabGroup("VFX")]
        private ParticleSystem m_deathFx;
        [SerializeField, TabGroup("VFX")]
        private ParticleSystem m_targetHitVfx;

        [ShowInInspector]
        private MinionMode m_minionMode;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        [SerializeField, TabGroup("Skins")]
        protected SkeletonDataAsset m_skeletonDataAsset;
        [SerializeField, TabGroup("Skins")]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, ValueDropdown("GetSkins"), TabGroup("Skins")]
        private List<string> m_skins;

        private bool m_shieldActive;
        private bool m_targetHit;

        private IEnumerable GetSkins()
        {
            ValueDropdownList<string> list = new ValueDropdownList<string>();
            var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
            for (int i = 0; i < reference.Length; i++)
            {
                list.Add(reference[i].Name);
            }
            return list;
        }

        private State m_turnState;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip; 
        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting && m_minionMode != MinionMode.NoShield)
                {
                    m_isDetecting = true;
                    m_shieldGlow.gameObject.SetActive(true);
                    SwitchModeTo(MinionMode.Guard);
                    m_stateHandle.SetState(State.ReevaluateSituation);
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
            m_stateHandle.ApplyQueuedState();
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
                if (m_minionMode == MinionMode.NoShield)
                {
                    SwitchModeTo(MinionMode.NoShield);
                }
                else
                {
                    m_selfCollider.SetActive(false);
                    m_targetInfo.Set(null, null);
                    m_isDetecting = false;
                    m_enablePatience = false;
                    m_shieldGlow.gameObject.SetActive(false);
                    m_minionMode = MinionMode.NoGuard;
                    m_stateHandle.SetState(State.Patrol);
                }

            }
        }
        //private IEnumerator PatienceRoutine()
        //{
        //    //if (m_enablePatience)
        //    //{
        //    //    while (m_currentPatience < m_info.patience)
        //    //    {
        //    //        m_currentPatience += m_character.isolatedObject.deltaTime;
        //    //        yield return null;
        //    //    }
        //    //}
        //    yield return new WaitForSeconds(m_info.patience);
        //    m_targetInfo.Set(null, null);
        //    m_isDetecting = false;
        //    m_enablePatience = false;
        //    m_stateHandle.SetState(State.Patrol);
        //}

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            m_selfCollider.SetActive(false);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            m_boundBoxGO.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
            StartCoroutine(DeathRoutine());
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (IsFacingTarget() && m_minionMode != MinionMode.NoShield)
            {
                hitbox.SetInvulnerability(Invulnerability.Level_1);
            }
            else
            {
                hitbox.SetInvulnerability(Invulnerability.None);
                StopAllCoroutines();
                m_animation.animationState.TimeScale = .5f;
                if(m_minionMode != MinionMode.NoShield)
                {
                    m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                }
                else
                {
                    m_animation.SetAnimation(0, m_info.noShieldFlinch, false);
                }
               
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_animation.animationState.TimeScale = 1f;
            //hitbox.SetInvulnerability(Invulnerability.None);
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            //    m_animation.SetEmptyAnimation(0, 0);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            if (m_minionMode == MinionMode.NoShield)
            {
                SwitchModeTo(MinionMode.NoShield);
            }
            else
            {
                SwitchModeTo(MinionMode.Guard);
                m_stateHandle.OverrideState(State.ReevaluateSituation);
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
            m_shieldGlow.gameObject.SetActive(false);
            m_minionMode = MinionMode.NoGuard;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.ShieldBash, m_info.ShieldBash.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator ShieldBashRoutine()
        {
            
            if (!IsFacingTarget() && !m_wallSensor.isDetecting && m_edgeSensor.isDetecting)
            {
                //Call Turn;
                m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.turnAnimation.animation);
            }
            var distancefromplayer = Vector2.Distance(m_targetInfo.position, transform.position);
            var isPlayerNear = distancefromplayer < m_info.ShieldBash.range;
            if (!isPlayerNear)
            {
                do
                {
                    distancefromplayer = Vector2.Distance(m_targetInfo.position, transform.position);
                    isPlayerNear = distancefromplayer < m_info.ShieldBash.range;
                    m_animation.SetAnimation(0,/* !isPlayerNear ? m_info.noShieldMove.animation :*/ m_info.march.animation, true);
                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.march.speed);
                    yield return null;
                } while (!isPlayerNear && !m_wallSensor.isDetecting && m_edgeSensor.isDetecting);
                m_movement.Stop();

            }
            m_animation.SetAnimation(0, m_info.idleGuardAnimation, false);
            yield return null;
            m_shieldAttacker.gameObject.SetActive(true);
            yield return null;
            m_animation.SetAnimation(0, m_info.ShieldBash.animation, false);
            //yield return new WaitUntil(() => m_wallSensor.isDetecting && m_playerSensor.isDetecting);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ShieldBash);
            if (m_targetHit)
            {
                m_targetHitVfx.Play();
                m_targetHit = false;
            }
            yield return null;
            m_targetHitVfx.Stop();
            m_shieldAttacker.gameObject.SetActive(false);
            yield return null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ShieldDestroyedRoutine()
        {
            //m_stateHandle.Wait(State.Flee);

            m_animation.SetAnimation(0, m_info.shieldDestroyed, false);
            m_shieldGlow.Stop();
            m_shieldBreakVFX.Play();
            m_skeletonAnimation.skeleton.SetSkin(m_skins[0]);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shieldDestroyed);
            m_shieldDamageable.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            m_shieldGlow.gameObject.SetActive(false);
            m_shieldBreakVFX.gameObject.SetActive(false);
            yield return null;
            yield return NoShieldRunRoutine();
        }

        private IEnumerator CrouchInFearRoutine()
        {
            m_animation.SetAnimation(0, m_info.noShieldCrouch, true);
            yield return null;
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        private IEnumerator NoShieldRunRoutine()
        {
            if (IsFacingTarget() && !m_wallSensor.isDetecting && m_edgeSensor.isDetecting)
            {
                //Call Turn Away;
                m_turnHandle.Execute(m_info.noShieldTurn.animation, m_info.noShieldIdle.animation);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.noShieldTurn);
            }

            //var distanceFromPlayer = 0f;
            //bool isPlayerNear = true;
            do
            {
                //distanceFromPlayer = Vector2.Distance(m_targetInfo.position, transform.position);
                //isPlayerNear = distanceFromPlayer <= m_info.targetDistanceTolerance;
                m_animation.SetAnimation(0,/* !isPlayerNear ? m_info.noShieldMove.animation :*/ m_info.noShieldRun.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.noShieldRun.speed);
                yield return null;
            } while (/*isPlayerNear &&*/ !m_wallSensor.isDetecting && m_edgeSensor.isDetecting);
            m_movement.Stop();
            yield return null;
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        private IEnumerator ChangeModeNoGuardRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return null;
            m_minionMode = MinionMode.NoGuard;
            m_stateHandle.SetState(State.Patrol);
        }
        private IEnumerator ChangeModeGuardRoutine()
        {
            if (!IsFacingTarget() && !m_wallSensor.isDetecting && m_edgeSensor.isDetecting)
            {
                m_stateHandle.SetState(State.Turning);
            }
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.idleGuardAnimation, true);
            yield return null;
            m_minionMode = MinionMode.Guard;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator DeathRoutine()
        {
            StopAllCoroutines();
            if(m_minionMode != MinionMode.NoShield)
            {
                m_shieldGlow.Stop();
                m_animation.SetAnimation(0, m_info.deathAnimation, false);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.deathAnimation2, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation2);
                m_deathFx.Play();
            }

            yield return null;
            
        }

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
            m_startPoint = transform.position;
            m_shieldDamageable.Destroyed += OnShieldDestroyed;
            m_shieldAttacker.TargetDamaged += OnTargetDamaged;
            m_shieldActive = true;
        }

        private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
        {
            m_targetHit = true;
        }

        private void OnShieldDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_shieldActive = false;
            m_skeletonAnimation.skeleton.SetSkin(m_skins[0]);
            if(hitbox.invulnerabilityLevel == Invulnerability.Level_1)
            {
                hitbox.SetInvulnerability(Invulnerability.None);
            }
            SwitchModeTo(MinionMode.NoShield);
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }

        private void HandleNoGuardStates()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:

                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }


                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);

                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);


                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        SwitchModeTo(MinionMode.Guard);

                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        private void HandleGuardStates()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleGuardAnimation.animation);

                    break;

                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        
                        m_stateHandle.Wait(State.Cooldown);
                        m_animation.SetAnimation(0, m_info.idleGuardAnimation, true);

                        switch (m_attackDecider.chosenAttack.attack)
                        {
                            case Attack.ShieldBash:
                                m_attackDecider.hasDecidedOnAttack = true;
                                StartCoroutine(ShieldBashRoutine());
                                break;
                        }

                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)

                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.idleGuardAnimation, true);
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.SetState(State.Attacking);

                    }

                    else
                    {
                        SwitchModeTo(MinionMode.NoGuard);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;

            }
        }

        private void HandleNoShieldState()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Flee:

                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        StartCoroutine(NoShieldRunRoutine());
                    }

                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.noShieldTurn.animation, m_info.noShieldIdle.animation);
                    break;

                case State.Crouching:
                    StartCoroutine(CrouchInFearRoutine());
                    m_animation.SetAnimation(0, m_info.noShieldCrouch, true);
                    m_stateHandle.SetState(State.ReevaluateSituation);
                    break;

                case State.ReevaluateSituation:

                    if (m_targetInfo.isValid)
                    {
                       if(Vector2.Distance(m_targetInfo.position,transform.position) >= m_info.targetDistanceTolerance)
                        {
                            m_stateHandle.SetState(State.Crouching);
                        }
                        else
                        {
                            m_stateHandle.SetState(State.Flee);
                        }
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.noShieldIdle, true);
                        m_stateHandle.SetState(State.WaitBehaviourEnd);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        private void SwitchModeTo(MinionMode nextMode)
        {
            //Stop Current Behaviour
            m_stateHandle.Wait(State.ReevaluateSituation);
            switch (nextMode)
            {
                case MinionMode.NoGuard:
                    StartCoroutine(ChangeModeNoGuardRoutine());
                    break;
                case MinionMode.Guard:
                    StartCoroutine(ChangeModeGuardRoutine());
                    break;
                case MinionMode.NoShield:
                    StartCoroutine(ShieldDestroyedRoutine());
                    break;
                case MinionMode.SwitchingToOtherMode:

                    break;
            }
            m_minionMode = nextMode;
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            ;
            switch (m_minionMode)
            {
                case MinionMode.NoGuard:
                    HandleNoGuardStates();
                    break;
                case MinionMode.Guard:
                    HandleGuardStates();
                    break;
                case MinionMode.NoShield:
                    HandleNoShieldState();
                    break;
                case MinionMode.SwitchingToOtherMode:
                    break;
            }
            #region Delete Later
            //switch (m_stateHandle.currentState)
            //{
            //    case State.Patrol:
            //        if (m_shieldActive == true)
            //        {
            //            if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
            //            {
            //                m_turnState = State.ReevaluateSituation;
            //                m_animation.EnableRootMotion(false, false);
            //                m_animation.SetAnimation(0, m_info.patrol.animation, true);
            //                var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
            //                m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
            //            }
            //            else
            //            {
            //                m_movement.Stop();
            //                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //            }
            //        }
            //        else
            //        {
            //            m_stateHandle.SetState(State.NoShield);
            //        }

            //        break;

            //    case State.Turning:
            //        m_stateHandle.Wait(m_turnState);
            //        if (m_shieldActive == true)
            //        {
            //            m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
            //        }
            //        else
            //        {
            //            m_turnHandle.Execute(m_info.noShieldTurn.animation, m_info.noShieldIdle.animation);
            //        }

            //        break;

            //    case State.Attacking:
            //        if (m_shieldActive == true)
            //        {
            //            if (IsFacingTarget())
            //            {
            //                if (IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
            //                {
            //                    m_stateHandle.Wait(State.Cooldown);
            //                    m_animation.SetAnimation(0, m_info.idleAnimation, true);

            //                    switch (m_attackDecider.chosenAttack.attack)
            //                    {
            //                        case Attack.ShieldCharge:
            //                            StartCoroutine(ShieldChargeRoutine());
            //                            break;
            //                    }
            //                    m_attackDecider.hasDecidedOnAttack = false;
            //                }
            //                else
            //                {
            //                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
            //                    {
            //                        var distance = Vector2.Distance(m_targetInfo.position, transform.position);
            //                        m_animation.EnableRootMotion(false, false);
            //                        m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
            //                        m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
            //                    }
            //                    else
            //                    {
            //                        m_movement.Stop();
            //                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                m_turnState = State.ReevaluateSituation;
            //                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
            //                    m_stateHandle.SetState(State.Turning);
            //            }
            //        }
            //        else
            //        {
            //            m_stateHandle.SetState(State.NoShield);
            //        }


            //        break;

            //    case State.Cooldown:
            //        //m_stateHandle.Wait(State.ReevaluateSituation);
            //        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
            //        if (m_shieldActive == true)
            //        {
            //            if (!IsFacingTarget())
            //            {
            //                m_turnState = State.Cooldown;
            //                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
            //                    m_stateHandle.SetState(State.Turning);
            //            }
            //            else
            //            {
            //                m_animation.EnableRootMotion(false, false);
            //                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //            }

            //            if (m_currentCD <= m_info.attackCD)
            //            {
            //                m_currentCD += Time.deltaTime;
            //            }
            //            else
            //            {
            //                m_currentCD = 0;
            //                m_stateHandle.OverrideState(State.ReevaluateSituation);
            //            }

            //        }
            //        else
            //        {
            //            m_stateHandle.SetState(State.NoShield);
            //        }


            //        break;
            //    case State.Chasing:
            //        {
            //            if (m_shieldActive == true)
            //            {
            //                m_attackDecider.DecideOnAttack();
            //                //m_attackDecider.hasDecidedOnAttack = false;
            //                if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
            //                {
            //                    m_movement.Stop();
            //                    m_stateHandle.SetState(State.Attacking);
            //                }
            //            }
            //            else
            //            {
            //                m_stateHandle.SetState(State.NoShield);
            //            }

            //        }

            //        break;

            //    case State.ReevaluateSituation:
            //        //How far is target, is it worth it to chase or go back to patrol
            //        if (m_targetInfo.isValid)
            //        {
            //            if (m_shieldActive == true)
            //            {
            //                m_stateHandle.SetState(State.Chasing);
            //            }
            //            else
            //            {
            //                m_stateHandle.SetState(State.NoShield);
            //            }

            //        }
            //        if (m_shieldActive == false)
            //        {
            //            m_stateHandle.SetState(State.NoShield);
            //        }
            //        else
            //        {
            //            if (m_shieldActive == true)
            //            {
            //                m_stateHandle.SetState(State.Patrol);
            //            }
            //            else
            //            {
            //                m_stateHandle.SetState(State.NoShield);
            //            }

            //        }
            //        break;
            //    case State.WaitBehaviourEnd:
            //        return;

            //    case State.NoShield:
            //        StartCoroutine(CrouchInFearRoutine());
            //        break;
            //}
            #endregion

            if (m_enablePatience)
            {
                Patience();
                //StartCoroutine(PatienceRoutine());
            }
        }

        protected override void OnTargetDisappeared()
        {
            SwitchModeTo(MinionMode.NoGuard);
            m_stateHandle.OverrideState(State.Patrol);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_boundBoxGO.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(true);
        }

        public void SwitchToBattleZoneAI()
        {
            SwitchModeTo(MinionMode.Guard);
            m_stateHandle.SetState(State.Attacking);
        }

        public void SwitchToBaseAI()
        {
            SwitchModeTo(MinionMode.NoGuard);
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
            SwitchModeTo(MinionMode.NoGuard);
            m_stateHandle.OverrideState(State.Patrol);
        }
    }
}
