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
using DChild.Temp;
using Spine.Unity.Modules;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/QueenBee")]
    public class QueenBeeAI : CombatAIBrain<QueenBeeAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_moveForward = new MovementInfo();
            public MovementInfo moveForward => m_moveForward;
            [SerializeField]
            private MovementInfo m_moveAscend = new MovementInfo();
            public MovementInfo moveAscend => m_moveAscend;
            [SerializeField]
            private MovementInfo m_moveDescend = new MovementInfo();
            public MovementInfo moveDescend => m_moveDescend;
            [SerializeField]
            private MovementInfo m_moveBackward = new MovementInfo();
            public MovementInfo moveBackward => m_moveBackward;


            //Attack Behaviours
            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_horizontalDroneAttack = new SimpleAttackInfo();
            public SimpleAttackInfo horizontalDroneAttack => m_horizontalDroneAttack;
            [SerializeField]
            private float m_drone3rdPhaseYAim;
            public float drone3rdPhaseYAim => m_drone3rdPhaseYAim;
            [SerializeField]
            private SimpleAttackInfo m_spearChargeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearChargeAttack => m_spearChargeAttack;
            [SerializeField]
            private SimpleAttackInfo m_spearMeleeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearMeleeAttack => m_spearMeleeAttack;
            [SerializeField]
            private SimpleAttackInfo m_spearThrowAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearThrowAttack => m_spearThrowAttack;
            [SerializeField]
            private float m_groundStingerRecoverTime;
            public float groundStingerRecoverTime => m_groundStingerRecoverTime;
            [SerializeField]
            private float m_chargeLoops;
            public float chargeLoops => m_chargeLoops;
            //

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_afterMoveForwardAnimation;
            public BasicAnimationInfo afterMoveForwardAnimation => m_afterMoveForwardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fallRecoverAnimation;
            public BasicAnimationInfo fallRecoverAnimation => m_fallRecoverAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchBackwardAnimation;
            public BasicAnimationInfo flinchBackwardAnimation => m_flinchBackwardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchFallLoopAnimation;
            public BasicAnimationInfo flinchFallLoopAnimation => m_flinchFallLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchFallStartAnimation;
            public BasicAnimationInfo flinchFallStartAnimation => m_flinchFallStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchForwardAnimation;
            public BasicAnimationInfo flinchForwardAnimation => m_flinchForwardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_introAnimation;
            public BasicAnimationInfo introAnimation => m_introAnimation;
            [SerializeField]
            private BasicAnimationInfo m_orderDroneAttackAnimation;
            public BasicAnimationInfo orderDroneAttackAnimation => m_orderDroneAttackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_orderDroneAttackLoopAnimation;
            public BasicAnimationInfo orderDroneAttackLoopAnimation => m_orderDroneAttackLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase1AtkMeleeAnimation;
            public BasicAnimationInfo phase1AtkMeleeAnimation => m_phase1AtkMeleeAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase2AtkChargeLoopAnimation;
            public BasicAnimationInfo phase2AtkChargeLoopAnimation => m_phase2AtkChargeLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase2AtkChargeStartAnimation;
            public BasicAnimationInfo phase2AtkChargeStartAnimation => m_phase2AtkChargeStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase3AtkSpearThrowAnimation;
            public BasicAnimationInfo phase3AtkSpearThrowAnimation => m_phase3AtkSpearThrowAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase4AtkStingerImpactAnimation;
            public BasicAnimationInfo phase4AtkStingerImpactAnimation => m_phase4AtkStingerImpactAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase4AtkStingerLoopAnimation;
            public BasicAnimationInfo phase4AtkStingerLoopAnimation => m_phase4AtkStingerLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase4TransitionAnimation;
            public BasicAnimationInfo phase4TransitionAnimation => m_phase4TransitionAnimation;
            [SerializeField]
            private BasicAnimationInfo m_stuckRecoverAnimation;
            public BasicAnimationInfo stuckRecoverAnimation => m_stuckRecoverAnimation;
            [SerializeField]
            private BasicAnimationInfo m_stuckStateAnimation;
            public BasicAnimationInfo stuckStateAnimation => m_stuckStateAnimation;
            [SerializeField]
            private BasicAnimationInfo m_stuckStateFlinchBackwardAnimation;
            public BasicAnimationInfo stuckStateFlinchBackwardAnimation => m_stuckStateFlinchBackwardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_stuckStateFlinchForwardAnimation;
            public BasicAnimationInfo stuckStateFlinchForwardAnimation => m_stuckStateFlinchForwardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_summonDroneAnimation;
            public BasicAnimationInfo summonDroneAnimation => m_summonDroneAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_spearProjectile;
            public SimpleProjectileAttackInfo spearProjectile => m_spearProjectile;
            [SerializeField]
            private SimpleProjectileAttackInfo m_beeProjectile;
            public SimpleProjectileAttackInfo beeProjectile => m_beeProjectile;
            [SerializeField]
            private GameObject m_burstGO;
            public GameObject burstGO => m_burstGO;

            [SerializeField]
            private GameObject m_shadowTelegraph;
            public GameObject shadowTelegraph => m_shadowTelegraph;
            [SerializeField]
            private GameObject m_spearDrop;
            public GameObject spearDrop => m_spearDrop;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveForward.SetData(m_skeletonDataAsset);
                m_moveAscend.SetData(m_skeletonDataAsset);
                m_moveDescend.SetData(m_skeletonDataAsset);
                m_moveBackward.SetData(m_skeletonDataAsset);
                m_horizontalDroneAttack.SetData(m_skeletonDataAsset);
                m_spearProjectile.SetData(m_skeletonDataAsset);
                m_beeProjectile.SetData(m_skeletonDataAsset);
                //
                m_spearChargeAttack.SetData(m_skeletonDataAsset);
                m_spearMeleeAttack.SetData(m_skeletonDataAsset);
                m_spearThrowAttack.SetData(m_skeletonDataAsset);

                m_afterMoveForwardAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_fallRecoverAnimation.SetData(m_skeletonDataAsset);
                m_flinchBackwardAnimation.SetData(m_skeletonDataAsset);
                m_flinchFallLoopAnimation.SetData(m_skeletonDataAsset);
                m_flinchFallStartAnimation.SetData(m_skeletonDataAsset);
                m_flinchForwardAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_introAnimation.SetData(m_skeletonDataAsset);
                m_orderDroneAttackAnimation.SetData(m_skeletonDataAsset);
                m_orderDroneAttackLoopAnimation.SetData(m_skeletonDataAsset);
                m_phase1AtkMeleeAnimation.SetData(m_skeletonDataAsset);
                m_phase2AtkChargeLoopAnimation.SetData(m_skeletonDataAsset);
                m_phase2AtkChargeStartAnimation.SetData(m_skeletonDataAsset);
                m_phase3AtkSpearThrowAnimation.SetData(m_skeletonDataAsset);
                m_phase4AtkStingerImpactAnimation.SetData(m_skeletonDataAsset);
                m_phase4AtkStingerLoopAnimation.SetData(m_skeletonDataAsset);
                m_phase4TransitionAnimation.SetData(m_skeletonDataAsset);
                m_stuckRecoverAnimation.SetData(m_skeletonDataAsset);
                m_stuckStateAnimation.SetData(m_skeletonDataAsset);
                m_stuckStateFlinchBackwardAnimation.SetData(m_skeletonDataAsset);
                m_stuckStateFlinchForwardAnimation.SetData(m_skeletonDataAsset);
                m_summonDroneAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
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

            [SerializeField, PreviewField]
            protected SkeletonDataAsset m_skeletonDataAsset;

            protected IEnumerable GetSkins()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
                
            }
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Patrol,
            Turning,
            Stucc,
            Fall,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            DroneAttack,
            SpearCharge,
            SpearMelee,
            SpearThrow,
            GroundStingerAttack,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_droneSpointsGO;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_movePointsGO;
        [SerializeField, TabGroup("Reference")]
        private Transform m_modelTransform;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_colliderDamageGO;
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
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_QBStingerChargeFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_QBStingerDiveFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_RightArmFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_LeftArmFX;
        //Patience Handler
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
        //private Attack m_previousAttack;
        //private Attack m_chosenAttack;
        private Attack m_currentAttack;
        //private float m_currentAttackRange;


        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        //private List<float> m_attackRangeCache;

        private ProjectileLauncher m_launcher;
        private ProjectileLauncher m_spearLauncher;

        [SerializeField, TabGroup("Move Points")]
        private Transform m_tripleDronePoint;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_tripleDronePhase3Point;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_returnPoint;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_spearThrowPoint;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_stingerDivePoint;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_GroundPoint;


        [SerializeField]
        private List<Transform> m_wavePattern1;
        [SerializeField]
        private List<Transform> m_wavePattern2;
        [SerializeField]
        private List<Transform> m_wavePattern3;
        [SerializeField]
        private Transform m_spearSpawnPoint;
        [SerializeField]
        private int m_hitCounter;
        [SerializeField]
        private int m_hitsToUnstuck;

        private Dictionary<string, List<Transform>> m_ListOfPatterns = new Dictionary<string, List<Transform>>();

        [SpineBone]
        public string m_boneName;
        [SerializeField]
        private Bone m_bone;
        
        private int m_currentPhaseIndex;
        private int m_currentDroneBatches;
        private float m_currentSummonSpeed;
        private int m_currentSummonAmmount;
        //private float m_currentDroneSummonSpeed;
        float m_currentRecoverTime;
        bool m_isFinalPhase;
        private bool m_isDetecting;

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
            m_animation.SetEmptyAnimation(0, 0);
            m_modelTransform.rotation = Quaternion.identity;
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
            if (m_flinchHandle.autoFlinching)
            {
                StopAllCoroutines();
                if (/*m_animation.GetCurrentAnimation(0).ToString() == m_info.spearThrowAttack.animation*/ m_currentPhaseIndex != 3)
                {
                    m_stateHandle.OverrideState(State.Fall);
                }
            }
            else
            {
                if (m_phaseHandle.currentPhase == Phase.PhaseFour)
                {
                    StopAllCoroutines();
                    m_animation.SetAnimation(0, IsFacingTarget() ? m_info.stuckStateFlinchForwardAnimation : m_info.stuckStateFlinchBackwardAnimation, false);
                    m_stateHandle.OverrideState(State.Stucc);
                    
                }
            }
        }

        private void HitOnBee(object sender, Damageable.DamageEventArgs eventArgs)
        {
            Debug.Log("On me");
            m_hitCounter++;
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
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.OverrideState(State.Intro);

                    //GameEventMessage.SendEvent("Boss Encounter");
                }
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
            //Debug.Log("Triple Attack!");
            switch (attack)
            {
                case Attack.DroneAttack:
                    if (m_currentPhaseIndex < 3)
                        StartCoroutine(HorizontalDronesRoutine());
                    else
                        m_stateHandle.ApplyQueuedState();
                    //StartCoroutine(SpearMeleeRoutine());
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.SpearCharge:
                    if(m_currentPhaseIndex == 1)
                        StartCoroutine(SpearChargeRoutine());
                    else
                        m_stateHandle.ApplyQueuedState();
                    //StartCoroutine(SpearMeleeRoutine());
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.SpearMelee:
                    if (m_currentPhaseIndex < 3)
                        StartCoroutine(SpearMeleeRoutine());
                    else
                        m_stateHandle.ApplyQueuedState();
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.SpearThrow:
                    if (m_currentPhaseIndex == 2)
                        StartCoroutine(SpearThrowRoutine());
                    else
                        m_stateHandle.ApplyQueuedState();
                    break;
                case Attack.GroundStingerAttack:
                    if (m_currentPhaseIndex >= 3)
                        StartCoroutine(GroundStingerRoutine());
                    else
                        m_stateHandle.ApplyQueuedState();
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

        private void DynamicMovement(Vector2 target)
        {
            if (IsFacing(target))
            {
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                m_agent.SetDestination(target);
                m_agent.Move(m_info.moveForward.speed);

                if (velocityX == 0 && velocityY > 5f)
                {
                    //Debug.Log("Move Upward");
                    m_animation.SetAnimation(0, m_info.moveAscend.animation, true);
                    //m_agent.Move(m_info.moveAscend.speed);
                }
                else if (velocityX != 0 && velocityY < -8f)
                {
                    //Debug.Log("Move Downward");
                    m_animation.SetAnimation(0, m_info.moveDescend.animation, true);
                    //m_agent.Move(m_info.moveDescend.speed);
                }
                else if (velocityX != 0)
                {
                    if (IsFacing(target))
                    {
                        //Debug.Log("Move Forward");
                        m_animation.SetAnimation(0, m_info.moveForward.animation, true);
                        //m_agent.Move(m_info.moveForward.speed);
                    }
                    else
                    {
                        //Debug.Log("Move Backward");
                        m_animation.SetAnimation(0, m_info.moveBackward.animation, true);
                        //m_agent.Move(m_info.moveBackward.speed);
                    }
                }
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation && GetComponent<IsolatedPhysics2D>().velocity.y <= 0 && m_stateHandle.currentState != State.Phasing)
                {
                    m_turnState = State.Attacking;
                    m_stateHandle.OverrideState(State.Turning);
                }
            }
        }

        private IEnumerator IntroRoutine()
        {
            //CustomTurn();
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.orderDroneAttackAnimation, false).TimeScale = 2f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.orderDroneAttackAnimation);
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
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.EnableRootMotion(false, false);
            if (m_currentPhaseIndex >= 3 && !m_isFinalPhase)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_isFinalPhase = true;
                m_currentAttack = Attack.GroundStingerAttack;
                var spear = Instantiate(m_info.spearDrop, transform.position, Quaternion.identity);
                m_RightArmFX.Play();
                m_LeftArmFX.Play();
                spear.GetComponent<Rigidbody2D>().AddForce(new Vector2(15 * transform.localScale.x, 10f), ForceMode2D.Impulse);
                m_animation.SetAnimation(0, m_info.phase4TransitionAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase4TransitionAnimation);
                //yield return new WaitForSeconds(5);
                //m_animation.EnableRootMotion(false, false);
                m_damageable.DamageTaken += HitOnBee;
                StartCoroutine(GroundStingerRoutine());
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            else
            {
                m_currentAttack = Attack.SpearThrow;
                var flinch = IsFacingTarget() ? m_info.flinchForwardAnimation : m_info.flinchBackwardAnimation;
                m_animation.SetAnimation(0, flinch, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, flinch);
                //m_animation.AddAnimation(0, m_info.idleAnimation, false, 0);
                //yield return new WaitForSeconds(2);
                StartCoroutine(SpearThrowRoutine());
            }
            yield return null;
        }

        private IEnumerator FallingRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_flinchHandle.m_autoFlinch = false;
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.flinchFallStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchFallStartAnimation);
            var target = new Vector2(transform.position.x, m_tripleDronePoint.position.y);
            while (Vector2.Distance(transform.position, target) > 1.5)
            {
                //transform.position = Vector3.MoveTowards(transform.position, target, .025f);
                m_agent.SetDestination(target);
                m_agent.Move(m_info.moveForward.speed * 3f);
                yield return null;
            }
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.fallRecoverAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fallRecoverAnimation);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator HorizontalDronesRoutine()
        {
            m_agent.Stop();
            //CustomTurn();
            var attackPos = m_currentPhaseIndex != 2 ? m_tripleDronePoint.position : m_tripleDronePhase3Point.position;
            while (Vector2.Distance(transform.position, attackPos) > 1.5)
            {
                DynamicMovement(attackPos);
                yield return null;
            }
            CustomTurn();
            m_stateHandle.Wait(State.ReevaluateSituation);
            //m_flinchHandle.m_autoFlinch = m_currentPhaseIndex == 2 ? true : false;
            m_agent.Stop();
            m_droneSpointsGO.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_animation.SetAnimation(0, m_info.summonDroneAnimation, false).TimeScale = 2f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDroneAnimation);
            for (int i = 0; i < m_currentDroneBatches; i++)
            {
                Debug.Log("horizontal drone !!!");
                //LaunchBeeProjectile();
                m_animation.SetAnimation(0, m_info.orderDroneAttackAnimation, false).TimeScale = m_currentSummonSpeed;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.orderDroneAttackAnimation);
                m_animation.SetAnimation(0, m_info.orderDroneAttackLoopAnimation, true);
                yield return new WaitForSeconds(1);
            }
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            //for (int i = 0; i < /*m_spawnPoints.Count*/4; i++)
            //{
            //}
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            //m_stateHandle.SetState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator SpearChargeRoutine()
        {
            m_agent.Stop();
            //CustomTurn();
            while (Vector2.Distance(transform.position, m_tripleDronePoint.position) > 1.5)
            {
                DynamicMovement(m_tripleDronePoint.position);
                yield return null;
            }
            CustomTurn();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.phase2AtkChargeStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase2AtkChargeStartAnimation);
            m_animation.DisableRootMotion();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_bodyCollider.SetActive(false);
            m_QBStingerChargeFX.gameObject.SetActive(true);
            m_QBStingerChargeFX.Play();
            //int i;
            //var i = 0;
            //while (m_info.chargeLoops > i)
            //{
            //    m_animation.SetAnimation(0, m_info.phase2AtkChargeLoopAnimation, false);
            //    //var chargeFXScale = m_QBStingerChargeFX.GetComponentInParent<Transform>();
            //    var mainFX = m_QBStingerChargeFX.main;
            //    mainFX.startRotation = transform.localScale.x > 0 ? /*180 * Mathf.Deg2Rad*/ (float)Mathf.PI/*nis*/: 0;
            //    //chargeFXScale.localScale = new Vector3(transform.localScale.x > 0 ? -chargeFXScale.localScale.x : chargeFXScale.localScale.x, chargeFXScale.localScale.y, chargeFXScale.localScale.z);
            //    GetComponent<IsolatedPhysics2D>().SetVelocity(100 * transform.localScale.x, 0);
            //    yield return new WaitForSeconds(i == 0 ? 1.25f : 2f);
            //    CustomTurn();
            //    m_agent.Stop();
            //    yield return new WaitForSeconds(.25f);
            //    transform.position = new Vector2(transform.position.x, m_targetInfo.position.y);
            //    i++;
            //    yield return null;
            //}

            for (int i = 0; i < /*UnityEngine.Random.Range(1,3)*/ 3; i++)
            {
                m_animation.SetAnimation(0, m_info.phase2AtkChargeLoopAnimation, true);
                //var chargeFXScale = m_QBStingerChargeFX.GetComponentInParent<Transform>();
                var mainFX = m_QBStingerChargeFX.main;
                mainFX.startRotation = transform.localScale.x > 0 ? /*180 * Mathf.Deg2Rad*/ (float)Mathf.PI/*nis*/: 0;
                //chargeFXScale.localScale = new Vector3(transform.localScale.x > 0 ? -chargeFXScale.localScale.x : chargeFXScale.localScale.x, chargeFXScale.localScale.y, chargeFXScale.localScale.z);
                GetComponent<IsolatedPhysics2D>().SetVelocity(250 * transform.localScale.x, 0);
                yield return new WaitForSeconds(i == 0 ? 1.25f : 2f);
                m_animation.SetEmptyAnimation(0, 0);
                CustomTurn();
                m_agent.Stop();
                yield return new WaitForSeconds(.25f);
                transform.position = new Vector2(transform.position.x, m_targetInfo.position.y);
            }
            m_hitbox.SetInvulnerability(Invulnerability.None);

            transform.position = m_returnPoint.position;
            m_QBStingerChargeFX.gameObject.SetActive(false);
            m_QBStingerChargeFX.Stop();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SpearMeleeRoutine()
        {
            m_agent.Stop();
            //var target = new Vector2(m_targetInfo.position.x - 5, m_targetInfo.position.y);
            //bool isInRange = Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range;
            //Debug.Log("X Distance In Range " + xTargetInRange);
            //Debug.Log("Y Distance In Range " + yTargetInRange);
            bool testing = false;
            while (!testing)
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < m_info.spearMeleeAttack.range ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < 3 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    testing = true;
                }

                DynamicMovement(m_targetInfo.position);
                //DynamicMovement(Mathf.Abs(m_targetInfo.position.y - transform.position.y) > 3f ? new Vector2(transform.position.x + (m_character.facing == HorizontalDirection.Right ? 1 : -1), m_targetInfo.position.y) : m_targetInfo.position);
                yield return null;
            }
            CustomTurn();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.phase1AtkMeleeAnimation, false);
            m_bodyCollider.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            for (int i = 0; i <= m_currentPhaseIndex; i++)
            {
                CustomTurn();
                Vector2 spitPos = transform.position;
                Vector3 v_diff = (new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 2) - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                m_modelTransform.rotation = Quaternion.Euler(0f, 0f, (atan2 * Mathf.Rad2Deg) + (m_character.facing == HorizontalDirection.Right ? 0 : 180));
                
                yield return new WaitForSeconds(1f);
                m_animation.DisableRootMotion();
                float time = 0;
                while (time < .25f)
                {
                    m_character.physics.SetVelocity(/*new Vector2(m_character.facing == HorizontalDirection.Right ? 200f : -200f, 0) */ (m_character.facing == HorizontalDirection.Right ? 300f : -300f) * m_modelTransform.right);
                    time += Time.deltaTime;
                    yield return null;
                }
                m_agent.Stop();
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase1AtkMeleeAnimation);
                m_modelTransform.rotation = Quaternion.identity;
                m_animation.SetEmptyAnimation(0, 0);
                m_animation.SetAnimation(0, m_info.phase1AtkMeleeAnimation, false).AnimationStart = 1.25f;
            }
            m_bodyCollider.SetActive(false);
            m_animation.SetAnimation(0, m_info.idleAnimation, false).MixDuration = 0;
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SpearThrowRoutine()
        {
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //CustomTurn();
            var targetPos = new Vector2(transform.position.x, m_spearThrowPoint.position.y);
            while (Vector2.Distance(transform.position, targetPos) > 1.5f)
            {
                m_agent.Stop();
                Vector3 dir = (targetPos - (Vector2)m_rigidbody2D.transform.position).normalized;
                m_rigidbody2D.MovePosition(m_rigidbody2D.transform.position + dir * m_info.moveForward.speed * Time.fixedDeltaTime);
                
                m_animation.SetAnimation(0, m_info.moveAscend.animation, true);
                yield return null;
            }
            CustomTurn();
            if (m_phaseHandle.currentPhase != Phase.PhaseThree)
            {
                m_flinchHandle.m_autoFlinch = true;
            }
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.spearThrowAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spearThrowAttack.animation);
            m_flinchHandle.m_autoFlinch = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator GroundStingerRecoverRoutine()
        {
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.stuckRecoverAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.stuckRecoverAnimation);
            StartCoroutine(GroundStingerRoutine());
            yield return null;
        }

        private IEnumerator GroundStingerRoutine()
        {
            m_agent.Stop();
            transform.position = m_stingerDivePoint.position;
            m_animation.SetAnimation(0, m_info.phase4AtkStingerLoopAnimation, true);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase4AtkStingerLoopAnimation);
            var shadow = Instantiate(m_info.shadowTelegraph, new Vector2(m_targetInfo.position.x, m_GroundPoint.position.y), Quaternion.identity);
            while (shadow.transform.localScale.x > 0.35f)
            {
                shadow.transform.position = new Vector2(m_targetInfo.position.x, shadow.transform.position.y);
                shadow.transform.localScale = new Vector3(shadow.transform.localScale.x - 0.25f * Time.deltaTime, shadow.transform.localScale.y - 0.25f * Time.deltaTime, shadow.transform.localScale.z - 0.25f * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(.5f);

            while (/*Vector2.Distance(transform.position, target) > .25f*/ !m_groundSensor.isDetecting)
            {
                //var target = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 2);
                var target = new Vector2(shadow.transform.position.x, shadow.transform.position.y);
                Vector2 pos = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, target, 10f);
                Vector2 spitPos = transform.position;
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                m_modelTransform.rotation = Quaternion.Euler(0f, 0f, (atan2 * ((transform.position.x > m_targetInfo.position.x ? 180 : -180) / (Mathf.PI * 2))));
                yield return null;
            }
            Destroy(shadow);
            m_QBStingerDiveFX.Play();
            m_modelTransform.rotation = Quaternion.Euler(Vector3.zero);
            m_stateHandle.Wait(State.Stucc);
            //yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_agent.Stop();
            //m_modelTransform.localRotation = Quaternion.Euler(Vector3.zero);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.phase4AtkStingerImpactAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase4AtkStingerImpactAnimation);
            m_animation.SetAnimation(0, m_info.stuckStateAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            //m_flinchHandle.m_autoFlinch= true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_colliderDamageGO.SetActive(false);
            m_agent.Stop();
            m_isDetecting = false;
            m_damageable.DamageTaken -= HitOnBee;
        }

        private void LaunchBeeProjectile()
        {
            StartCoroutine(LaunchBeeProjectileRoutine());
            //for (int i = 0; i < m_currentSummonAmmount; i++)
            //{
            //    float rotation = transform.localScale.x < 1 ? 180 : 0;
            //    int rng = UnityEngine.Random.Range(0, m_spawnPoints.Count);
            //    m_spawnPoints[rng].localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            //    //GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[rng].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
            //    m_launcher = new ProjectileLauncher(m_info.beeProjectile.projectileInfo, m_spawnPoints[rng]);
            //    m_launcher.LaunchProjectile();
            //    //yield return new WaitForSeconds(.25f);

            //}
        }

        private void LaunchSpearProjectile()
        {
            if (!IsFacingTarget())
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            m_spearLauncher = new ProjectileLauncher(m_info.spearProjectile.projectileInfo, m_spearSpawnPoint);
            m_spearLauncher.AimAt(m_targetInfo.position);
            m_spearLauncher.LaunchProjectile();
        }

        private IEnumerator LaunchBeeProjectileRoutine()
        {
            //leandro spag code 
            float rotation = transform.localScale.x < 1 ? 180 : 0;
            var randomNumber = UnityEngine.Random.Range(0, m_ListOfPatterns.Count);
            Debug.Log(m_ListOfPatterns.Count.ToString());
            string[] patternKeys = new string[m_ListOfPatterns.Count];
            m_ListOfPatterns.Keys.CopyTo(patternKeys, 0);
            var randomPattern = patternKeys[randomNumber];
            List<Transform> randomPatternList = m_ListOfPatterns[randomPattern];
            for (int i = 0; i < m_currentSummonAmmount; i++)
            {
                Debug.Log("Randomly chosen list (" + randomPattern + "):");
                //int rng = UnityEngine.Range(0, m_spawnPoints.Count);
                randomPatternList[i].localRotation = Quaternion.Euler(new Vector3(-rotation, 1 , rotation));
                //GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[rng].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                m_launcher = new ProjectileLauncher(m_info.beeProjectile.projectileInfo, randomPatternList[i]);
                m_launcher.LaunchProjectile();
                //yield return new WaitForSeconds(.25f);
                randomPatternList[i].localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            yield return null;

            //old logic
            //float rotation = transform.localScale.x < 1 ? 180 : 0;
            ////int rng = UnityEngine.Range(0, m_spawnPoints.Count);
            //m_spawnPoints[i].localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            ////GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[rng].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
            //m_launcher = new ProjectileLauncher(m_info.beeProjectile.projectileInfo, m_spawnPoints[i]);
            //m_launcher.LaunchProjectile();
            //yield return new WaitForSeconds(.25f);
        }

        void SkeletonAnimation_UpdateLocal(ISkeletonAnimation animated)
        {
            //Debug.Log("FKING AIM");
            if (m_targetInfo.isValid)
            {
                var localPositon = transform.InverseTransformPoint(m_targetInfo.position);
                localPositon = new Vector2(-localPositon.x, localPositon.y);
                m_bone.SetLocalPosition(localPositon);
            }
        }

        private void UpdateAttackDeciderList()
        {
            //Debug.Log("Update attack list trigger");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.DroneAttack, m_info.horizontalDroneAttack.range),
                                    new AttackInfo<Attack>(Attack.SpearCharge, m_info.spearChargeAttack.range),
                                    new AttackInfo<Attack>(Attack.SpearMelee, m_info.spearMeleeAttack.range),
                                    new AttackInfo<Attack>(Attack.SpearThrow, m_info.spearThrowAttack.range)/**/);
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
                        //m_currentAttackRange = m_attackRangeCache[i];
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

        //void AddToRangeCache(params float[] list)
        //{
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        m_attackRangeCache.Add(list[i]);
        //    }
        //}

        //void UpdateRangeCache(params float[] list)
        //{
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        m_attackRangeCache[i] = list[i];
        //    }
        //}

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);

            m_bone = m_animation.skeletonAnimation.Skeleton.FindBone(m_boneName);
            m_animation.skeletonAnimation.UpdateLocal += SkeletonAnimation_UpdateLocal;
            //m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_spawnPoints[0]);


        }

        protected override void Start()
        {
            base.Start();
            m_flinchHandle.m_autoFlinch = false;
            m_spineListener.Subscribe(m_info.spearProjectile.launchOnEvent, LaunchSpearProjectile);
            m_spineListener.Subscribe(m_info.beeProjectile.launchOnEvent, LaunchBeeProjectile);

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
            
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.DroneAttack, Attack.SpearCharge, Attack.SpearMelee, Attack.SpearThrow, Attack.GroundStingerAttack);
            //m_attackRangeCache = new List<float>();
            //AddToRangeCache(m_info.horizontalDroneAttack.range, m_info.spearChargeAttack.range, m_info.spearMeleeAttack.range, m_info.spearThrowAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];

            m_ListOfPatterns.Add("WavePatterns1", m_wavePattern1);
            m_ListOfPatterns.Add("WavePatterns2", m_wavePattern2);
            m_ListOfPatterns.Add("WavePatterns3", m_wavePattern3);
        }

        private void Update()
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_phaseHandle.MonitorPhase();
            }
            switch (m_stateHandle.currentState)
            {
                case State.Intro:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_animation.DisableRootMotion();
                    StartCoroutine(IntroRoutine());
                    break;
                case State.Phasing:
                    m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                    StopAllCoroutines();
                    //StartCoroutine(ChangePhaseRoutine());
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.Stucc:
                   // hit counter = 3 to unstucc
                    if (m_info.groundStingerRecoverTime > m_currentRecoverTime)
                    {
                        if (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.stuckStateAnimation, true);
                        }
                        m_currentRecoverTime += Time.deltaTime;
                    }
                    if (m_info.groundStingerRecoverTime < m_currentRecoverTime || m_hitCounter == m_hitsToUnstuck)
                    {
                        m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                        StartCoroutine(GroundStingerRecoverRoutine());
                        m_currentRecoverTime = 0;
                        m_hitCounter = 0;
                    }
                    break;

                case State.Fall:
                    StartCoroutine(FallingRoutine());
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_animation.animationState.TimeScale = 2f;
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    m_agent.Stop();
                    break;
                case State.Attacking:

                    //StartCoroutine(TripleBeeDroneRoutine());
                    MoveToAttackPosition(m_currentAttack);

                    break;
                case State.Chasing:

                    //Debug.Log("Commence Attacking Deciding Phase");
                    //if (m_previousAttack == Attack.SpearMelee)
                    //{
                    //    //Debug.Log("Decide ANothat BEE ATACK");
                    //    m_attackDecider.DecideOnAttack();
                    //    m_chosenAttack = m_attackDecider.chosenAttack.attack;

                    //}
                    //else
                    //{
                    //    //Debug.Log("Spear Spear");
                    //    m_chosenAttack = Attack.SpearMelee;
                    //    m_attackDecider.hasDecidedOnAttack = true;

                    //}
                    //m_chosenAttack = m_previousAttack == Attack.SpearMelee ? m_attackDecider.chosenAttack.attack : Attack.SpearMelee;
                    m_attackDecider.hasDecidedOnAttack = false;
                    ChooseAttack();

                    if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_attackDecider.chosenAttack.range)*/ /*&& m_currentAttack != m_previousAttack*/)
                    {
                        //m_agent.Stop();
                        StopAllCoroutines();
                        m_movePointsGO.transform.localScale = new Vector3(UnityEngine.Random.Range(-1, 1), 1, 1);
                        m_movePointsGO.transform.localScale = new Vector3(m_movePointsGO.transform.localScale.x == 0 ? 1 : m_movePointsGO.transform.localScale.x, 1, 1);
                        //m_previousAttack = m_chosenAttack;
                        m_stateHandle.SetState(State.Attacking);
                    }
                    else
                    {
                        m_agent.Stop();
                        m_attackDecider.hasDecidedOnAttack = false;
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
        }

        protected override void OnTargetDisappeared()
        {
            m_colliderDamageGO.SetActive(true);
        }

        protected override void OnForbidFromAttackTarget()
        {
        }

        public override void ReturnToSpawnPoint()
        {
            throw new NotImplementedException();
        }
    }
}