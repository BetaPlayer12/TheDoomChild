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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/MotherMantis")]
    public class MotherMantisAI : CombatAIBrain<MotherMantisAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_moveLowHP = new MovementInfo();
            public MovementInfo moveLowHP => m_moveLowHP;
            //[SerializeField]
            //private MovementInfo m_moveAscend = new MovementInfo();
            //public MovementInfo moveAscend => m_moveAscend;
            //[SerializeField]
            //private MovementInfo m_moveDescend = new MovementInfo();
            //public MovementInfo moveDescend => m_moveDescend;
            //[SerializeField]
            //private MovementInfo m_moveBackward = new MovementInfo();
            //public MovementInfo moveBackward => m_moveBackward;


            //Attack Behaviours
            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private SimpleAttackInfo m_attack2StepBack = new SimpleAttackInfo();
            public SimpleAttackInfo attack2StepBack => m_attack2StepBack;
            [SerializeField]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField]
            private SimpleAttackInfo m_attack4 = new SimpleAttackInfo();
            public SimpleAttackInfo attack4 => m_attack4;
            [SerializeField]
            private SimpleAttackInfo m_attack4b = new SimpleAttackInfo();
            public SimpleAttackInfo attack4b => m_attack4b;
            [SerializeField]
            private SimpleAttackInfo m_attack5 = new SimpleAttackInfo();
            public SimpleAttackInfo attack5 => m_attack5;

            [SerializeField]
            private float m_groundStingerRecoverTime;
            public float groundStingerRecoverTime => m_groundStingerRecoverTime;
            [SerializeField]
            private float m_chargeLoops;
            public float chargeLoops => m_chargeLoops;
            //

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBackAnimation;
            public string flinchBackAnimation => m_flinchBackAnimation;


            //[Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_mantisEvent;
            //public string mantisEvent => m_mantisEvent;
            //[SerializeField]
            //private SimpleProjectileAttackInfo m_spearProjectile;
            //public SimpleProjectileAttackInfo spearProjectile => m_spearProjectile;
            //[SerializeField]
            //private SimpleProjectileAttackInfo m_beeProjectile;
            //public SimpleProjectileAttackInfo beeProjectile => m_beeProjectile;
            //[SerializeField]
            //private GameObject m_burstGO;
            //public GameObject burstGO => m_burstGO;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_moveLowHP.SetData(m_skeletonDataAsset);
                //m_spearProjectile.SetData(m_skeletonDataAsset);
                //m_beeProjectile.SetData(m_skeletonDataAsset);
                //
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack2StepBack.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_attack4.SetData(m_skeletonDataAsset);
                m_attack4b.SetData(m_skeletonDataAsset);
                m_attack5.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_droneStrikeBatches;
            public int droneStrikeBatches => m_droneStrikeBatches;
            [SerializeField]
            private float m_droneStrikeSummonSpeed;
            public float droneStrikeSummonSpeed => m_droneStrikeSummonSpeed;
            [SerializeField]
            private int m_droneSummonAmmount;
            public int droneSummonAmmount => m_droneSummonAmmount;
            //[SerializeField]
            //private int m_tombSize;
            //public int tombSize => m_tombSize;
            //[SerializeField]
            //private int m_skeletonNum;
            //public int skeletonNum => m_skeletonNum;
            //[SerializeField, ValueDropdown("GetSkins")]
            //private string m_skin;
            //public string skin => m_skin;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;

            //[SerializeField, PreviewField]
            //protected SkeletonDataAsset m_skeletonDataAsset;

            //protected IEnumerable GetSkins()
            //{
            //    ValueDropdownList<string> list = new ValueDropdownList<string>();
            //    var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
            //    for (int i = 0; i < reference.Length; i++)
            //    {
            //        list.Add(reference[i].Name);
            //    }
            //    return list;
            //}
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Flinch,
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
            Attack2StepBack,
            Attack3,
            Attack4,
            Attack4b,
            Attack5,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_modelTransform;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        //[SerializeField, TabGroup("Modules")]
        //private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_mantisFX;
        [SerializeField]
        private SpineEventListener m_spineListener;

        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        //[ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_previousAttack;
        private Attack m_chosenAttack;

        private ProjectileLauncher m_launcher;
        private ProjectileLauncher m_spearLauncher;

        //[SerializeField, TabGroup("Move Points")]
        //private Transform m_tripleDronePoint;
        //[SerializeField, TabGroup("Move Points")]
        //private Transform m_tripleDronePhase3Point;
        //[SerializeField, TabGroup("Move Points")]
        //private Transform m_returnPoint;
        //[SerializeField, TabGroup("Move Points")]
        //private Transform m_spearThrowPoint;
        //[SerializeField, TabGroup("Move Points")]
        //private Transform m_stingerDivePoint;
        //[SerializeField, TabGroup("Move Points")]
        //private Transform m_GroundPoint;

        private int m_currentPhaseIndex;
        private int m_currentDroneBatches;
        private float m_currentSummonSpeed;
        private int m_currentSummonAmmount;
        //private float m_currentDroneSummonSpeed;
        float m_currentRecoverTime;
        bool m_isFinalPhase;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            //Debug.Log("Change Phase");
            //m_currentTombVolleys = obj.tombVolley;
            //m_currentTombSize = obj.tombSize;
            //m_currentSkeletonSize = obj.skeletonNum;
            //m_currentSkin = obj.skin;
            m_currentPhaseIndex = obj.phaseIndex;
            m_currentDroneBatches = obj.droneStrikeBatches;
            m_currentSummonSpeed = obj.droneStrikeSummonSpeed;
            m_currentSummonAmmount = obj.droneSummonAmmount;
        }

        private void ChangeState()
        {
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //if (/*m_animation.GetCurrentAnimation(0).ToString() == m_info.spearThrowAttack.animation*/ m_currentPhaseIndex != 3)
            //{
            //    m_stateHandle.OverrideState(State.Fall);
            //}
            //else /*if (m_stateHandle.currentState != State.Fall)*/
            //{
            //    m_animation.SetAnimation(0, IsFacingTarget() ? m_info.stuckStateFlinchForwardAnimation : m_info.stuckStateFlinchBackwardAnimation, false);
            //    m_stateHandle.OverrideState(State.Stucc);
            //}
        }

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    //m_stateHandle.OverrideState(State.Stucc);
        //    m_animation.SetAnimation(0, m_info.stuckStateAnimation, true);
        //}

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
            m_animation.animationState.TimeScale = 1f;
            if (m_stateHandle.currentState != State.Phasing)
                m_stateHandle.ApplyQueuedState();
        }

        private void MoveToAttackPosition(Attack attack/*, Vector2 target*/)
        {
            //StopAllCoroutines();
            //Debug.Log("Triple Attack!");
            switch (attack)
            {
                case Attack.Attack1:
                    //if (m_currentPhaseIndex < 3)
                    break;
                case Attack.Attack2:
                    //if (m_currentPhaseIndex == 1)
                    break;
                case Attack.Attack2StepBack:
                    //if (m_currentPhaseIndex < 3)
                    //    StartCoroutine(SpearMeleeRoutine());
                    //else
                    //    m_stateHandle.ApplyQueuedState();
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.Attack3:
                    //if (m_currentPhaseIndex == 2)
                    //    StartCoroutine(SpearThrowRoutine());
                    //else
                    //    m_stateHandle.ApplyQueuedState();
                    break;
                case Attack.Attack4:
                    //if (m_currentPhaseIndex >= 3)
                    //    StartCoroutine(GroundStingerRoutine());
                    //else
                    //    m_stateHandle.ApplyQueuedState();
                    break;
                case Attack.Attack4b:
                    //if (m_currentPhaseIndex >= 3)
                    //    StartCoroutine(GroundStingerRoutine());
                    //else
                    //    m_stateHandle.ApplyQueuedState();
                    break;
                case Attack.Attack5:
                    //if (m_currentPhaseIndex >= 3)
                    //    StartCoroutine(GroundStingerRoutine());
                    //else
                    //    m_stateHandle.ApplyQueuedState();
                    break;
            }
        }

        private void CustomTurn()
        {
            if (!IsFacingTarget())
            {
                //m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
        }

        private IEnumerator IntroRoutine()
        {
            //CustomTurn();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            //m_stateHandle.SetState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_phaseHandle.ApplyChange();
            m_bodyCollider.SetActive(false);
            m_stateHandle.Wait(State.Phasing);
            m_animation.animationState.TimeScale = 1f;
            //m_turnState = State.Phasing;
            m_movement.Stop();
            m_hitbox.SetInvulnerability(true);
            m_animation.EnableRootMotion(false, false);
            if (m_currentPhaseIndex >= 3 && !m_isFinalPhase)
            {
                //m_isFinalPhase = true;
                //m_chosenAttack = Attack.GroundStingerAttack;
                //var spear = Instantiate(m_info.spearDrop, transform.position, Quaternion.identity);
                //m_RightArmFX.Play();
                //m_LeftArmFX.Play();
                //spear.GetComponent<Rigidbody2D>().AddForce(new Vector2(15 * transform.localScale.x, 10f), ForceMode2D.Impulse);
                //m_animation.SetAnimation(0, m_info.phase4TransitionAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase4TransitionAnimation);
                ////yield return new WaitForSeconds(5);
                ////m_animation.EnableRootMotion(false, false);
                //StartCoroutine(GroundStingerRoutine());
                ////m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            else
            {
                //m_chosenAttack = Attack.SpearThrow;
                //var flinch = IsFacingTarget() ? m_info.flinchForwardAnimation : m_info.flinchBackwardAnimation;
                //m_animation.SetAnimation(0, flinch, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, flinch);
                ////m_animation.AddAnimation(0, m_info.idleAnimation, false, 0);
                ////yield return new WaitForSeconds(2);
                //StartCoroutine(SpearThrowRoutine());
            }
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        private void LaunchProjectile()
        {
            //if (!IsFacingTarget())
            //{
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //    m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            //}
            //m_spearLauncher = new ProjectileLauncher(m_info.spearProjectile.projectileInfo, m_spearSpawnPoint);
            //m_spearLauncher.AimAt(m_targetInfo.position);
            //m_spearLauncher.LaunchProjectile();
        }

        private void UpdateAttackDeciderList()
        {
            //Debug.Log("Update attack list trigger");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range),
                                    new AttackInfo<Attack>(Attack.Attack2StepBack, m_info.attack2StepBack.range),
                                    new AttackInfo<Attack>(Attack.Attack3, m_info.attack3.range),
                                    new AttackInfo<Attack>(Attack.Attack4, m_info.attack4.range),
                                    new AttackInfo<Attack>(Attack.Attack4b, m_info.attack4b.range),
                                    new AttackInfo<Attack>(Attack.Attack5, m_info.attack5.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        public override void ApplyData()
        {
            //Debug.Log("Apply Data Queen Bee");
            if (m_attackDecider != null)
            {
                //Debug.Log("Update attack list trigger function");
                UpdateAttackDeciderList();
            }
            //if (m_info != null)
            //{
            //    m_spineListener.Unsubcribe(m_info.spearProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            //}
            //if (m_stingerLauncher != null)
            //{
            //    m_stingerLauncher.SetProjectile(m_info.spea.projectileInfo);
            //    m_spineListener.Subscribe(m_info.stingerProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            //}
            base.ApplyData();
        }

        protected override void Awake()
        {
            base.Awake();
            //m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }

        protected override void Start()
        {
            base.Start();
            m_flinchHandle.gameObject.SetActive(false);
            //m_spineListener.Subscribe(m_info.mantisEvent, LaunchProjectile);

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        StartCoroutine(IntroRoutine());
                    }
                    else
                    {
                        m_turnState = State.Intro;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                    //StartCoroutine(ChangePhaseRoutine());
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    //m_animation.animationState.TimeScale = 2f;
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    //StartCoroutine(TripleBeeDroneRoutine());
                    //MoveToAttackPosition(m_chosenAttack);
                    m_stateHandle.Wait(State.ReevaluateSituation);

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack2:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack2StepBack:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack2StepBack.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack3:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack3.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack4:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack4.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack4b:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack4b.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack5:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack5.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;
                case State.Chasing:

                    if (IsFacingTarget())
                    {
                        m_attackDecider.DecideOnAttack();
                        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) /*&& !m_wallSensor.allRaysDetecting*/)
                        {
                            m_movement.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            if (/*!m_wallSensor.isDetecting && m_edgeSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                            {
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, m_info.move.animation, true);
                                //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                            }
                            else
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        m_stateHandle.SetState(State.Turning);
                    }

                    ////Debug.Log("Commence Attacking Deciding Phase");
                    //if (m_previousAttack == Attack.Attack1)
                    //{
                    //    //Debug.Log("Decide ANothat BEE ATACK");
                    //    m_attackDecider.DecideOnAttack();
                    //    m_chosenAttack = m_attackDecider.chosenAttack.attack;

                    //}
                    //else
                    //{
                    //    //Debug.Log("Spear Spear");
                    //    //m_chosenAttack = Attack.SpearMelee;
                    //    m_attackDecider.hasDecidedOnAttack = true;

                    //}
                    ////m_chosenAttack = m_previousAttack == Attack.SpearMelee ? m_attackDecider.chosenAttack.attack : Attack.SpearMelee;

                    //if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_attackDecider.chosenAttack.range)*/ && m_chosenAttack != m_previousAttack)
                    //{
                    //    //m_agent.Stop();
                    //    //m_movePointsGO.transform.localScale = new Vector3(UnityEngine.Random.Range(-1, 1), 1, 1);
                    //    //m_movePointsGO.transform.localScale = new Vector3(m_movePointsGO.transform.localScale.x == 0 ? 1 : m_movePointsGO.transform.localScale.x, 1, 1);
                    //    //m_previousAttack = m_chosenAttack;
                    //    //m_stateHandle.SetState(State.Attacking);
                    //}
                    //else
                    //{
                    //    m_movement.Stop();
                    //    m_attackDecider.hasDecidedOnAttack = false;
                    //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //}

                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
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

        }
    }
}
