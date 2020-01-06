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
            private SimpleAttackInfo m_spearChargeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearChargeAttack => m_spearChargeAttack;
            [SerializeField]
            private SimpleAttackInfo m_spearMeleeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearMeleeAttack => m_spearMeleeAttack;
            [SerializeField]
            private SimpleAttackInfo m_spearThrowAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearThrowAttack => m_spearThrowAttack;
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
            private int m_tombVolley;
            public int tombVolley => m_tombVolley;
            [SerializeField]
            private int m_tombSize;
            public int tombSize => m_tombSize;
            [SerializeField]
            private int m_skeletonNum;
            public int skeletonNum => m_skeletonNum;
            [SerializeField, ValueDropdown("GetSkins")]
            private string m_skin;
            public string skin => m_skin;
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
            Idle,
            Patrol,
            Turning,
            AttackTurn,
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
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
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
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        [SerializeField]
        private SpineEventListener m_spineListener;

        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_previousAttack;

        private ProjectileLauncher m_launcher;

        [SerializeField, TabGroup("Move Points")]
        private Transform m_tripleDronePoint;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_returnPoint;
        [SerializeField, TabGroup("Move Points")]
        private Transform m_spearThrowPoint;

        [SerializeField]
        private List<Transform> m_spawnPoints;
        [SerializeField]
        private Transform m_spearSpawnPoint;

        [SpineBone]
        public string m_boneName;
        [SerializeField]
        private Bone m_bone;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            Debug.Log("Change Phase");
            //m_currentTombVolleys = obj.tombVolley;
            //m_currentTombSize = obj.tombSize;
            //m_currentSkeletonSize = obj.skeletonNum;
            //m_currentSkin = obj.skin;
            //m_currentPhaseIndex = obj.phaseIndex;
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
            m_animation.SetAnimation(0, m_info.flinchBackwardAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

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
            m_stateHandle.ApplyQueuedState();
        }

        private void MoveToAttackPosition(Attack attack/*, Vector2 target*/)
        {
            //StopAllCoroutines();
            //Debug.Log("Triple Attack!");
            switch (attack)
            {
                case Attack.DroneAttack:
                    StartCoroutine(HorizontalDronesRoutine());
                    //StartCoroutine(SpearMeleeRoutine());
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.SpearCharge:
                    StartCoroutine(SpearChargeRoutine());
                    //StartCoroutine(SpearMeleeRoutine());
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.SpearMelee:
                    StartCoroutine(SpearMeleeRoutine());
                    //StartCoroutine(SpearThrowRoutine());
                    break;
                case Attack.SpearThrow:
                    StartCoroutine(SpearThrowRoutine());
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

                if (velocityX != 0 && velocityY > 5f)
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
                        Debug.Log("Move Forward");
                        m_animation.SetAnimation(0, m_info.moveForward.animation, true);
                        //m_agent.Move(m_info.moveForward.speed);
                    }
                    else
                    {
                        Debug.Log("Move Backward");
                        m_animation.SetAnimation(0, m_info.moveBackward.animation, true);
                        //m_agent.Move(m_info.moveBackward.speed);
                    }
                }
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    m_stateHandle.OverrideState(State.AttackTurn);
            }
        }

        private IEnumerator HorizontalDronesRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            CustomTurn();
            while (Vector2.Distance(transform.position, m_tripleDronePoint.position) > 1.5)
            {
                DynamicMovement(m_tripleDronePoint.position);
                yield return null;
            }
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.summonDroneAnimation, false).TimeScale = 2f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDroneAnimation);
            m_animation.SetAnimation(0, m_info.orderDroneAttackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.orderDroneAttackAnimation);
            m_animation.SetAnimation(0, m_info.orderDroneAttackLoopAnimation, true);
            yield return new WaitForSeconds(1f);
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
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            CustomTurn();
            while (Vector2.Distance(transform.position, m_tripleDronePoint.position) > 1.5)
            {
                DynamicMovement(m_tripleDronePoint.position);
                yield return null;
            }
            m_agent.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.phase2AtkChargeStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase2AtkChargeStartAnimation);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.phase2AtkChargeLoopAnimation, false);

            for (int i = 0; i < /*UnityEngine.Random.Range(1,3)*/ 3; i++)
            {
                GetComponent<IsolatedPhysics2D>().SetVelocity(100 * transform.localScale.x, 0);
                yield return new WaitForSeconds(i == 0 ? 1.25f : 2f);
                CustomTurn();
                m_agent.Stop();
                yield return new WaitForSeconds(.25f);
                transform.position = new Vector2(transform.position.x, m_targetInfo.position.y);
            }

            transform.position = m_returnPoint.position;
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SpearMeleeRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            //CustomTurn();
            while (Vector2.Distance(transform.position, m_targetInfo.position) > m_info.spearMeleeAttack.range)
            {
                //Debug.Log("Facing Target " + IsFacingTarget());
                DynamicMovement(m_targetInfo.position);
                yield return null;
            }
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.phase1AtkMeleeAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phase1AtkMeleeAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SpearThrowRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            //CustomTurn();
            var targetPos = new Vector2(transform.position.x, m_spearThrowPoint.position.y);
            while (Vector2.Distance(transform.position, targetPos) > 1.5f)
            {
                //Debug.Log("Facing Target " + IsFacingTarget());
                DynamicMovement(targetPos);
                yield return null;
            }
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.spearThrowAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spearThrowAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }

        private void LaunchBeeProjectile()
        {
            StartCoroutine(LaunchBeeProjectileRoutine());
        }

        private void LaunchSpearProjectile()
        {
            m_launcher = new ProjectileLauncher(m_info.spearProjectile.projectileInfo, m_spawnPoints[0]);
            if (!IsFacingTarget())
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            m_launcher.AimAt(m_targetInfo.position);
            m_launcher.LaunchProjectile();
        }

        private IEnumerator LaunchBeeProjectileRoutine()
        {
            if (IsFacingTarget())
            {
                for (int i = 0; i < m_spawnPoints.Count; i++)
                {
                    float rotation = transform.localScale.x < 1 ? 180 : 0;
                    m_spawnPoints[i].localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                    GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[i].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                    m_launcher = new ProjectileLauncher(m_info.beeProjectile.projectileInfo, m_spawnPoints[i]);
                    m_launcher.LaunchProjectile();
                    yield return new WaitForSeconds(.25f);

                }
                //int num = UnityEngine.Random.Range(0, m_spawnPoints.Count);
                //float rotation = transform.localScale.x < 1 ? 180 : 0;
                //m_spawnPoints[num].localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                //GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[num].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                //m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_spawnPoints[num]);
                //m_stingerLauncher.LaunchProjectile();
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
                yield return null;
            }
        }

        void SkeletonAnimation_UpdateLocal(ISkeletonAnimation animated)
        {
            Debug.Log("FKING AIM");
            //Vector3 pos = m_target;
            //var bonePosition = m_bone.GetWorldPosition(this.transform);
            //var direction = pos - bonePosition;
            //float rotation = DirectionToRotation(direction, this.transform);
            ////float parentRotation = m_bone.parent.WorldRotationX;
            //float parentRotation = m_bone.Parent.WorldRotationX;
            //m_bone.RotateWorld(rotation - parentRotation);
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
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
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
            m_spineListener.Subscribe(m_info.spearProjectile.launchOnEvent, LaunchSpearProjectile);
            m_spineListener.Subscribe(m_info.beeProjectile.launchOnEvent, LaunchBeeProjectile);
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Phasing:
                    break;
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.Patrol:

                    //m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    //var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    //m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);

                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_agent.Stop();

                    break;
                case State.AttackTurn:
                    //Debug.Log("Do Attack Turn Now");
                    m_stateHandle.Wait(State.Attacking);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_agent.Stop();

                    break;
                case State.Attacking:

                    //StartCoroutine(TripleBeeDroneRoutine());
                    MoveToAttackPosition(m_attackDecider.chosenAttack.attack);

                    break;
                case State.Chasing:

                    if (IsFacingTarget())
                    {
                        //Debug.Log("Commence Attacking Deciding Phase");
                        m_attackDecider.DecideOnAttack();
                        if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_attackDecider.chosenAttack.range)*/ && m_attackDecider.chosenAttack.attack != m_previousAttack)
                        {
                            //m_agent.Stop();
                            m_previousAttack = m_attackDecider.chosenAttack.attack;
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            m_agent.Stop();
                            m_attackDecider.hasDecidedOnAttack = false;
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                        //else
                        //{

                        //    var target = m_targetInfo.position;
                        //    target.y += 2f;
                        //    m_animation.DisableRootMotion();
                        //    if (m_character.physics.velocity != Vector2.zero)
                        //    {
                        //        m_animation.SetAnimation(0, m_info.move.animation, true);
                        //    }
                        //    else
                        //    {
                        //        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        //    }
                        //    m_agent.SetDestination(target);

                        //    m_agent.Move(m_info.move.speed);

                        //}

                    }
                    else
                    {
                        m_stateHandle.SetState(State.Turning);
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


    }
}
