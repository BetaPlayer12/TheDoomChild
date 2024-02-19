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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

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

            [Title("Attack Cooldown")]
            [SerializeField]
            private float m_universalAttackCD;
            public float universalAttackCD => m_universalAttackCD;
            [Title("Attack Chances")]
            [SerializeField, Range(0,100)]
            private float m_universalAttackChance;
            public float universalAttackChance => m_universalAttackChance;
            [SerializeField, Range(0, 100)]
            private float m_meleeAttackChance;
            public float meleeAttackChance => m_meleeAttackChance;
            
            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private BasicAnimationInfo m_stuckAnimation;
            public BasicAnimationInfo stuckAnimation => m_stuckAnimation;
            [SerializeField]
            private BasicAnimationInfo m_unstuckAnimation;
            public BasicAnimationInfo unstuckAnimation => m_unstuckAnimation;
            [SerializeField, Range(0, 100)]
            private float m_stuckDuration;
            public float stuckDuration => m_stuckDuration;
            [SerializeField]
            private SimpleAttackInfo m_attack2StepBack = new SimpleAttackInfo();
            public SimpleAttackInfo attack2StepBack => m_attack2StepBack;
            [SerializeField]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField]
            private float m_bulbAmount;
            public float bulbAmount => m_bulbAmount;
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
            private float m_seedAmmount;
            public float seedAmmount => m_seedAmmount;

            [Title("Spawned Objects")]
            [SerializeField]
            private GameObject m_larvaBulb;
            public GameObject larvaBulb => m_larvaBulb;
            [SerializeField]
            private GameObject m_seedProjectile;
            public GameObject seedProjectile => m_seedProjectile;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            //Animations
            [SerializeField]
            private BasicAnimationInfo m_introAnimation;
            public BasicAnimationInfo introAnimation => m_introAnimation;
            [SerializeField]
            private BasicAnimationInfo m_intro2Animation;
            public BasicAnimationInfo intro2Animation => m_intro2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchBackAnimation;
            public BasicAnimationInfo flinchBackAnimation => m_flinchBackAnimation;

            [Title("Projectiles")]
            //[Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_mantisEvent;
            //public string mantisEvent => m_mantisEvent;
            [SerializeField]
            private SimpleProjectileAttackInfo m_petalProjectile;
            public SimpleProjectileAttackInfo petalProjectile => m_petalProjectile;
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
                m_petalProjectile.SetData(m_skeletonDataAsset);

                m_stuckAnimation.SetData(m_skeletonDataAsset);
                m_unstuckAnimation.SetData(m_skeletonDataAsset);
                m_introAnimation.SetData(m_skeletonDataAsset);
                m_intro2Animation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinchBackAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private float m_petalAmount;
            public float petalAmount => m_petalAmount;
            [SerializeField]
            private float m_cooldownSpeed;
            public float cooldownSpeed => m_cooldownSpeed;
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
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            Attack2,
            Attack3,
            Attack4,
            Attack5,
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

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_modelTransform;
        [SerializeField, TabGroup("Reference")]
        private GameObject Attackbb;
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
        //[SerializeField, TabGroup("Modules")]
        //private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_deathFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_petalStartFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_petalLoopFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_petalEndFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_seedLaunchFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_landFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_flinchFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_landingVisualFX;
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
        private float m_currentAttackRange;

        private ProjectileLauncher m_petalLauncher;


        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_currentSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_petalProjectileSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_bulbSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_seedSpawnPoint;

        private float m_groundPosition;
        private List<Vector2> m_targetPositions;

        private bool m_stickToGround;
        public bool m_seedSpawning;
        private float m_currentCD;

        [ReadOnly, TabGroup("PatternTracker")]
        public int m_attack1Use;
        [ReadOnly, TabGroup("PatternTracker")]
        public int m_attack2Use;
        [ReadOnly, TabGroup("PatternTracker")]
        public int m_attack3Use;
        [ReadOnly, TabGroup("PatternTracker")]
        public int m_attack4Use;
        [ReadOnly, TabGroup("PatternTracker")]
        public int m_attack5Use;



        private int m_currentPhaseIndex;
        private float m_currentPetalAmount;
        private float m_currentCooldownSpeed;
        private int m_currentSummonAmmount;
        //private float m_currentDroneSummonSpeed;
        float m_currentRecoverTime;
        //bool m_isPhasing;

        private string m_moveAnim;
        private float m_moveSpeed;
        private bool m_isDetecting;
            
        public EventAction<EventActionArgs> OnPetalRain;


        private void ApplyPhaseData(PhaseInfo obj)
        {
            //Debug.Log("Change Phase");
            //m_currentTombVolleys = obj.tombVolley;
            //m_currentTombSize = obj.tombSize;
            //m_currentSkeletonSize = obj.skeletonNum;
            //m_currentSkin = obj.skin;
            m_currentPhaseIndex = obj.phaseIndex;
            m_currentPetalAmount = obj.petalAmount;
            m_currentCooldownSpeed = obj.cooldownSpeed;
        }

        private void ChangeState()
        {
            m_phaseHandle.ApplyChange();
            //if (!m_isPhasing)
            //{
            //    m_isPhasing = true;
            //}
            StopAllCoroutines();
            m_animation.SetEmptyAnimation(0, 0);
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            //m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.OverrideState(State.Cooldown);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

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

                //m_testTarget = m_targetInfo.position;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.animationState.TimeScale = 1f;
            //if (!m_isPhasing)
                m_stateHandle.ApplyQueuedState();
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
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.EnableRootMotion(true, false);
            yield return new WaitForSeconds(2);
            m_animation.SetAnimation(0, m_info.move.animation, true);
            yield return new WaitForSeconds(5);
            GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.introAnimation);
            m_animation.SetAnimation(0, m_info.intro2Animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.intro2Animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            //m_stateHandle.SetState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitbox.SetInvulnerability(Invulnerability.None); //wasTrue
            m_currentCD = 0;
            //m_isPhasing = true;
            //m_stateHandle.Wait(State.ReevaluateSituation);
            //m_animation.animationState.TimeScale = 1f;
            //m_movement.Stop();
            m_bodyCollider.SetActive(false);
            m_animation.EnableRootMotion(true, false);
            //m_turnState = State.Phasing;
            var flinchAnim = IsFacingTarget() ? m_info.flinchAnimation : m_info.flinchBackAnimation;
            m_animation.SetAnimation(0, flinchAnim, false);
            m_flinchFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, flinchAnim);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            //m_isPhasing = false;
            if (m_currentPhaseIndex == 4)
            {
                StartCoroutine(SeedLaunchRoutine());
                StartCoroutine(SeedFXRoutine());
            }
            else
            {
                m_animation.SetAnimation(0, m_info.attack1.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
                m_bodyCollider.SetActive(true);
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                m_stateHandle.ApplyQueuedState();
            }
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            //transform.position = new Vector2(transform.position.x, m_groundPosition);
            m_stickToGround = true;
            m_seedLaunchFX.Stop();
            m_deathFX.Play();
            StartCoroutine(LeapStickToGroundRoutine(m_groundPosition));
            m_movement.Stop();
            m_isDetecting = false;
        }

        #region Attacks

        #region PetalAttack
        private void LaunchPetalProjectile(Vector2 target, Transform spawnPoint)
        {
            //if (!IsFacingTarget())
            //{
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //    m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            //}
            m_petalLauncher = new ProjectileLauncher(m_info.petalProjectile.projectileInfo, spawnPoint);
            m_petalLauncher.AimAt(target);
            m_petalLauncher.LaunchProjectile();
        }

        private Vector2 CalculatePositions()
        {
            var target = m_targetInfo.position;
            var point = new Vector2(UnityEngine.Random.Range(-20, 20) + target.x, UnityEngine.Random.Range(-20, 20) + target.y); //Locked to Ground
            return point;
        }

        private IEnumerator PetalFXRoutine(Vector2 target)
        {
            m_petalStartFX.Play();
            yield return new WaitForSeconds(1.25f);
            m_petalEndFX.Play();
            for (int i = 0; i < m_currentPetalAmount; i++)
            {
                //if (IsFacing(m_targetInfo.position))
                //{
                //    m_targetPositions.Add(CalculatePositions());
                //}
                //var point = new Vector2(UnityEngine.Random.Range(-10, 10) + target.x, UnityEngine.Random.Range(-10, 10) + target.y); //Precise
                var xOffset = (m_targetPositions[i].x - target.x) * .2f;
                //var yOffset = point.y - target.y; //Precise
                var yOffset = (m_targetPositions[i].y - transform.position.y) * .2f; //Locked to Ground
                //m_currentSpawnPoint.position = new Vector2(UnityEngine.Random.Range(-5, 5) + m_petalProjectileSpawnPoint.position.x, UnityEngine.Random.Range(-5, 5) + m_petalProjectileSpawnPoint.position.y); //Random
                m_currentSpawnPoint.position = new Vector2(xOffset + m_petalProjectileSpawnPoint.position.x, yOffset + m_petalProjectileSpawnPoint.position.y); //In a straight path
                yield return new WaitForSeconds(.05f);
                LaunchPetalProjectile(m_targetPositions[i], m_currentSpawnPoint);
            }
            m_targetPositions.Clear();
            //LaunchPetalProjectile(target);
            yield return null;
        }

        private IEnumerator PetalLaunchRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            //StartCoroutine(PetalFXRoutine());
            var animation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.attack4.animation : m_info.attack4b.animation;
            m_animation.SetAnimation(0, animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region LeapAttack
        private IEnumerator LeapStickToGroundRoutine(float groundPoint)
        {
            while (m_stickToGround)
            {
                //Debug.Log("Sticking to Ground");
                transform.position = new Vector2(transform.position.x, groundPoint);
                yield return null;
            }
        }

        private IEnumerator LeapAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            Attackbb.SetActive(false);
            m_hitbox.SetInvulnerability(Invulnerability.None); //wasTrue
            m_stickToGround = true;
            //var animation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.attack2.animation : m_info.attack2StepBack.animation;
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            yield return new WaitForSeconds(1.5f);
            m_landingVisualFX.Play();
            Attackbb.SetActive(true);
            transform.position = new Vector2(m_targetInfo.position.x, transform.position.y - 5);
            //yield return new WaitUntil(() => m_groundSensor.isDetecting);
            yield return new WaitForSeconds(.825f);
            m_landFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
            m_animation.SetAnimation(0, m_info.stuckAnimation, true);
            yield return new WaitForSeconds(m_info.stuckDuration);
            m_animation.SetAnimation(0, m_info.unstuckAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.unstuckAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stickToGround = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region LarvaBulbAttack
        private IEnumerator SpawnLarvaRoutine()
        {
            yield return new WaitForSeconds(1f); //m_larvaSpawnPoint.position
            for (int i = 0; i < m_info.bulbAmount; i++)
            {
                var position = new Vector2(UnityEngine.Random.Range(-50, 50) + m_bulbSpawnPoint.position.x, m_bulbSpawnPoint.position.y);
                var bulb = Instantiate(m_info.larvaBulb, position, Quaternion.identity);
                bulb.GetComponent<MotherMantisBulb>().GetTarget(m_targetInfo);
                yield return new WaitForSeconds(3f);
            }
            yield return null;
        }

        private IEnumerator SpawnLarvaBulbRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack3.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region SeedLaunch
        private IEnumerator SeedFXRoutine()
        {
            //yield return new WaitForSeconds(.6f);
            //m_seedLaunchFX.Play();
            //yield return new WaitForSeconds(1.4f);
            //m_seedLaunchFX.Play();
            //m_seedLaunchFX.Stop();
            //m_seedLaunchFX.Play();
            //m_seedLaunchFX.Stop();
            StartCoroutine(SeedSpawnRoutine());
            yield return null;
        }

        private IEnumerator SeedSpawnRoutine()
        {
            m_seedSpawning = true;
            for (int i = 0; i < m_info.seedAmmount; i++)
            {
                var spawnPoint = new Vector2(m_seedSpawnPoint.position.x + (UnityEngine.Random.Range(-50, 50)), m_seedSpawnPoint.position.y);
                //var projectile = Instantiate(m_info.seedProjectile, spawnPoint, Quaternion.identity);

                GameObject projectile = m_info.seedProjectile;
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                instance.transform.position = spawnPoint;
                var component = instance.GetComponent<Projectile>();
                component.ResetState();
                yield return new WaitForSeconds(.5f);
            }
            m_seedSpawning = false;
            yield return null;
        }


        private IEnumerator SeedLaunchRoutine()
        {
            OnPetalRain?.Invoke(this, EventActionArgs.Empty);
            m_stateHandle.Wait(State.Cooldown);
            m_animation.SetAnimation(0, m_info.attack2StepBack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2StepBack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        } 
        #endregion

        #endregion
        #region Movement
        private void MoveToTarget()
        {
            if (!IsTargetInRange(m_info.attack1.range) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_moveAnim, true);
                //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_moveSpeed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }
        #endregion

        private bool AllowAttack(int phaseIndex)
        {
            if (m_currentPhaseIndex >= phaseIndex)
            {
                return true;
            }
            else
            {
                m_attackDecider.hasDecidedOnAttack = false;
                m_stateHandle.OverrideState(State.ReevaluateSituation);
                return false;
            }
        }

        private void AttackPattern()
        {
            m_attackDecider.hasDecidedOnAttack = true;
            if (m_attackDecider.chosenAttack.attack == Attack.Attack1 && m_attack1Use <= 2 && m_currentPhaseIndex == 1)
            {
                m_attack1Use++;
                m_currentAttack = Attack.Attack1;
            }
            else if (m_attackDecider.chosenAttack.attack == Attack.Attack2 && m_attack2Use <= 2 && m_currentPhaseIndex == 2)
            {
                m_attack2Use++;
                m_currentAttack = Attack.Attack2;
            }
        }


        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range),
                                    new AttackInfo<Attack>(Attack.Attack3, m_info.attack3.range),
                                    new AttackInfo<Attack>(Attack.Attack4, m_info.attack4.range),
                                    new AttackInfo<Attack>(Attack.Attack5, m_info.attack2StepBack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
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
            //m_patrolHandle.TurnRequest += OnTurnRequest;
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Attack1, Attack.Attack2, Attack.Attack3, Attack.Attack4, Attack.Attack5);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attack1.range, m_info.attack2.range, m_info.attack3.range, m_info.attack4.range, m_info.attack5.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            //m_flinchHandle.gameObject.SetActive(false);
            //m_spineListener.Subscribe(m_info.mantisEvent, LaunchProjectile);
            m_animation.DisableRootMotion();
            m_moveAnim = m_info.move.animation;
            m_moveSpeed = m_info.move.speed;
            m_targetPositions = new List<Vector2>();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            //if (!m_isPhasing)
            //{
            //}
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        //StartCoroutine(IntroRoutine());
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        m_hitbox.SetInvulnerability(Invulnerability.None);
                        m_animation.DisableRootMotion();
                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        m_turnState = State.Intro;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    //m_animation.animationState.TimeScale = 2f;
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (IsFacingTarget())
                    {
                        float chance = UnityEngine.Random.Range(0, 100);
                        float meleeChance = UnityEngine.Random.Range(0, 100);
                        if (chance > m_info.universalAttackChance && meleeChance < m_info.meleeAttackChance && IsTargetInRange(m_info.attack1.range))
                        {
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation.animation);
                        }
                        else if (chance < m_info.universalAttackChance)
                        {
                            //Debug.Log("Current Chance to Use Skill: " + chance);
                            //Debug.Log("Chance needed to Use Skill: " + m_info.universalAttackChance);
                            switch (m_currentAttack)
                            {
                                case Attack.Attack1:
                                    //m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation.animation);
                                    break;
                                case Attack.Attack2:
                                    //m_animation.EnableRootMotion(true, false);
                                    //m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation);
                                    m_groundPosition = transform.position.y;
                                    StartCoroutine(LeapAttackRoutine());
                                    StartCoroutine(LeapStickToGroundRoutine(m_groundPosition));
                                    break;
                                case Attack.Attack3:
                                    //m_animation.EnableRootMotion(true, false);
                                    if (AllowAttack(3))
                                    {
                                        StartCoroutine(SpawnLarvaRoutine());
                                        StartCoroutine(SpawnLarvaBulbRoutine());
                                    }
                                    break;
                                case Attack.Attack4:
                                    //var target = m_targetInfo.position;
                                    if (AllowAttack(2))
                                    {
                                        for (int i = 0; i < m_currentPetalAmount; i++)
                                        {
                                            m_targetPositions.Add(CalculatePositions());
                                        }
                                        StartCoroutine(PetalFXRoutine(m_targetInfo.position));
                                        StartCoroutine(PetalLaunchRoutine());
                                    }
                                    break;
                                case Attack.Attack5:
                                    //m_animation.EnableRootMotion(true, false);
                                    if (AllowAttack(4) && !m_seedSpawning)
                                    {
                                        //m_attackHandle.ExecuteAttack(m_info.attack5.animation, m_info.idleAnimation);
                                        StartCoroutine(SeedLaunchRoutine());
                                        StartCoroutine(SeedFXRoutine());
                                    }
                                    else
                                    {
                                        m_attackDecider.hasDecidedOnAttack = false;
                                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            MoveToTarget();
                        }
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }

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
                        if (!IsTargetInRange(m_info.targetDistanceTolerance))
                        {
                            MoveToTarget();
                        }
                        else
                        {
                            m_movement.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }

                    if (m_currentCD <= m_currentCooldownSpeed)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_attackDecider.hasDecidedOnAttack = false;
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:

                    if (IsFacingTarget())
                    {
                        //m_attackDecider.DecideOnAttack();
                        //if (m_attackDecider.chosenAttack.attack == m_previousAttack)
                        //{
                        //    m_attackDecider.hasDecidedOnAttack = false;
                        //}
                        ChooseAttack();
                        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) /*&& !m_wallSensor.allRaysDetecting*/)
                        {
                            //StopAllCoroutines();
                            //m_previousAttack = m_attackDecider.chosenAttack.attack;
                            //m_movement.Stop();
                            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            m_attackDecider.hasDecidedOnAttack = false;
                            MoveToTarget();
                        }
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
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
                        m_stateHandle.SetState(State.Idle);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stickToGround = false;
            m_currentCD = 0;
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}
