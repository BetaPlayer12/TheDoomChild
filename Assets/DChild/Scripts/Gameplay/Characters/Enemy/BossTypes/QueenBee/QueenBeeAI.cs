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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_afterMoveForwardAnimation;
            public string afterMoveForwardAnimation => m_afterMoveForwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fallRecoverAnimation;
            public string fallRecoverAnimation => m_fallRecoverAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBackwardAnimation;
            public string flinchBackwardAnimation => m_flinchBackwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchFallLoopAnimation;
            public string flinchFallLoopAnimation => m_flinchFallLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchFallStartAnimation;
            public string flinchFallStartAnimation => m_flinchFallStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchForwardAnimation;
            public string flinchForwardAnimation => m_flinchForwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_introAnimation;
            public string introAnimation => m_introAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_orderDroneAttackAnimation;
            public string orderDroneAttackAnimation => m_orderDroneAttackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_orderDroneAttackLoopAnimation;
            public string orderDroneAttackLoopAnimation => m_orderDroneAttackLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase1AtkMeleeAnimation;
            public string phase1AtkMeleeAnimation => m_phase1AtkMeleeAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase2AtkChargeLoopAnimation;
            public string phase2AtkChargeLoopAnimation => m_phase2AtkChargeLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase2AtkChargeStartAnimation;
            public string phase2AtkChargeStartAnimation => m_phase2AtkChargeStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase3AtkSpearThrowAnimation;
            public string phase3AtkSpearThrowAnimation => m_phase3AtkSpearThrowAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase4AtkStingerImpactAnimation;
            public string phase4AtkStingerImpactAnimation => m_phase4AtkStingerImpactAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase4AtkStingerLoopAnimation;
            public string phase4AtkStingerLoopAnimation => m_phase4AtkStingerLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase4TransitionAnimation;
            public string phase4TransitionAnimation => m_phase4TransitionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stuckRecoverAnimation;
            public string stuckRecoverAnimation => m_stuckRecoverAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stuckStateAnimation;
            public string stuckStateAnimation => m_stuckStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stuckStateFlinchBackwardAnimation;
            public string stuckStateFlinchBackwardAnimation => m_stuckStateFlinchBackwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stuckStateFlinchForwardAnimation;
            public string stuckStateFlinchForwardAnimation => m_stuckStateFlinchForwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_summonDroneAnimation;
            public string summonDroneAnimation => m_summonDroneAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

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
        private Attack m_previousAttack;
        private Attack m_chosenAttack;

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
        private List<Transform> m_spawnPoints;
        [SerializeField]
        private Transform m_spearSpawnPoint;

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
            if (IsFacingTarget())
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
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation && GetComponent<IsolatedPhysics2D>().velocity.y <= 0 && m_stateHandle.currentState != State.Phasing)
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
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            while (Vector2.Distance(transform.position, m_tripleDronePoint.position) > 1.5)
            {
                if (IsFacingTarget())
                {
                    var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                    var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                    //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                    m_agent.SetDestination(m_tripleDronePoint.position);
                    m_agent.Move(m_info.moveForward.speed);
                    m_animation.SetAnimation(0, m_info.introAnimation, false);

                }
                else
                {
                    m_turnState = State.Intro;
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        m_stateHandle.OverrideState(State.Turning);
                }
                yield return null;
            }
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.introAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
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
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.EnableRootMotion(false, false);
            if (m_currentPhaseIndex >= 3 && !m_isFinalPhase)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_isFinalPhase = true;
                m_chosenAttack = Attack.GroundStingerAttack;
                var spear = Instantiate(m_info.spearDrop, transform.position, Quaternion.identity);
                m_RightArmFX.Play();
                m_LeftArmFX.Play();
                spear.GetComponent<Rigidbody2D>().AddForce(new Vector2(15 * transform.localScale.x, 10f), ForceMode2D.Impulse);
                m_animation.SetAnimation(0, m_info.phase4TransitionAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase4TransitionAnimation);
                //yield return new WaitForSeconds(5);
                //m_animation.EnableRootMotion(false, false);
                StartCoroutine(GroundStingerRoutine());
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            else
            {
                m_chosenAttack = Attack.SpearThrow;
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
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_flinchHandle.m_autoFlinch = m_currentPhaseIndex == 2 ? true : false;
            m_agent.Stop();
            m_droneSpointsGO.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_animation.SetAnimation(0, m_info.summonDroneAnimation, false).TimeScale = 2f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDroneAnimation);
            for (int i = 0; i < m_currentDroneBatches; i++)
            {
                //LaunchBeeProjectile();
                m_animation.SetAnimation(0, m_info.orderDroneAttackAnimation, false).TimeScale = m_currentSummonSpeed;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.orderDroneAttackAnimation);
                m_animation.SetAnimation(0, m_info.orderDroneAttackLoopAnimation, true);
                yield return new WaitForSeconds(1);
            }
            m_flinchHandle.m_autoFlinch = false;
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
            //CustomTurn();
            //var target = new Vector2(m_targetInfo.position.x - 5, m_targetInfo.position.y);
            //bool isInRange = Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range;
            //Debug.Log("X Distance In Range " + xTargetInRange);
            //Debug.Log("Y Distance In Range " + yTargetInRange);
            bool testing = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            while (!testing)
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < m_info.spearMeleeAttack.range ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < 3 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    testing = true;
                }
                //Debug.Log("Facing Target " + IsFacingTarget());
                DynamicMovement(m_targetInfo.position);
                yield return null;
            }
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.phase1AtkMeleeAnimation, false);
            m_bodyCollider.SetActive(true);
            if (m_currentPhaseIndex != 0)
            {
                yield return new WaitForSeconds(2.25f);
                m_animation.DisableRootMotion();
                //m_character.physics.SetVelocity(Vector2.zero);
                m_character.physics.AddForce(new Vector2(5f * transform.localScale.x, 0), ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.25f);
            }
            m_agent.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase1AtkMeleeAnimation);
            m_bodyCollider.SetActive(false);
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
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
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                m_agent.SetDestination(targetPos);
                m_agent.Move(m_info.moveForward.speed);

                if (velocityX != 0 && velocityY > 5f)
                {
                    //Debug.Log("Move Upward");
                    m_animation.SetAnimation(0, m_info.moveAscend.animation, true);
                }
                yield return null;
            }
            m_flinchHandle.m_autoFlinch = true;
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
            for (int i = 0; i < m_currentSummonAmmount; i++)
            {
                float rotation = transform.localScale.x < 1 ? 180 : 0;
                int rng = UnityEngine.Random.Range(0, m_spawnPoints.Count);
                m_spawnPoints[rng].localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                //GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[rng].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                m_launcher = new ProjectileLauncher(m_info.beeProjectile.projectileInfo, m_spawnPoints[rng]);
                m_launcher.LaunchProjectile();
                yield return new WaitForSeconds(.25f);
            }
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

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
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
                    if (m_info.groundStingerRecoverTime > m_currentRecoverTime)
                    {
                        if (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.stuckStateAnimation, true);
                        }
                        m_currentRecoverTime += Time.deltaTime;
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                        StartCoroutine(GroundStingerRecoverRoutine());
                        m_currentRecoverTime = 0;
                    }
                    break;

                case State.Fall:
                    StartCoroutine(FallingRoutine());
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_animation.animationState.TimeScale = 2f;
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_agent.Stop();
                    break;
                case State.Attacking:

                    //StartCoroutine(TripleBeeDroneRoutine());
                    MoveToAttackPosition(m_chosenAttack);

                    break;
                case State.Chasing:

                    //Debug.Log("Commence Attacking Deciding Phase");
                    if (m_previousAttack == Attack.SpearMelee)
                    {
                        //Debug.Log("Decide ANothat BEE ATACK");
                        m_attackDecider.DecideOnAttack();
                        m_chosenAttack = m_attackDecider.chosenAttack.attack;

                    }
                    else
                    {
                        //Debug.Log("Spear Spear");
                        m_chosenAttack = Attack.SpearMelee;
                        m_attackDecider.hasDecidedOnAttack = true;

                    }
                    //m_chosenAttack = m_previousAttack == Attack.SpearMelee ? m_attackDecider.chosenAttack.attack : Attack.SpearMelee;

                    if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_attackDecider.chosenAttack.range)*/ && m_chosenAttack != m_previousAttack)
                    {
                        //m_agent.Stop();
                        StopAllCoroutines();
                        m_movePointsGO.transform.localScale = new Vector3(UnityEngine.Random.Range(-1, 1), 1, 1);
                        m_movePointsGO.transform.localScale = new Vector3(m_movePointsGO.transform.localScale.x == 0 ? 1 : m_movePointsGO.transform.localScale.x, 1, 1);
                        m_previousAttack = m_chosenAttack;
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

        protected override void OnBecomePassive()
        {
        }
    }
}
