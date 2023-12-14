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

            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;


            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_clawattack = new SimpleAttackInfo();
            public SimpleAttackInfo clawattack => m_clawattack;
            [SerializeField]
            private SimpleAttackInfo m_jump = new SimpleAttackInfo();
            public SimpleAttackInfo jump => m_jump;
            [SerializeField]
            private BasicAnimationInfo m_landingAnimation;
            public BasicAnimationInfo landingAnimation => m_landingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_backgroundLandingAnimation;
            public BasicAnimationInfo backgroundLandingAnimation => m_backgroundLandingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_backgroundJumpAnimation;
            public BasicAnimationInfo backgroundJumpAnimation => m_backgroundJumpAnimation;
            [SerializeField]
            private BasicAnimationInfo m_petalBackgroundLeft;
            public BasicAnimationInfo petalBackgroundLeft => m_petalBackgroundLeft;
            [SerializeField]
            private BasicAnimationInfo m_petalBackgroundRight;
            public BasicAnimationInfo petalBackgroundRight => m_petalBackgroundRight;
            [SerializeField]
            private BasicAnimationInfo m_petalBackgroundBoth;
            public BasicAnimationInfo petalBackgroundBoth => m_petalBackgroundBoth;

            [SerializeField]
            private float m_seedAmmount;
            public float seedAmmount => m_seedAmmount;

            [Title("Spawned Objects")]
            [SerializeField]
            private GameObject m_FlowerBulb;
            public GameObject flowerBulb => m_FlowerBulb;
            [SerializeField]
            private GameObject m_spikeVines;
            public GameObject spikeVines => m_spikeVines;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            //Animations
            [SerializeField]
            private BasicAnimationInfo m_rageAnimation;
            public BasicAnimationInfo rageAnimation => m_rageAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idlephase1Animation;
            public BasicAnimationInfo idlephase1Animation => m_idlephase1Animation;
            [SerializeField]
            private BasicAnimationInfo m_idlephase2Animation;
            public BasicAnimationInfo idlephase2Animation => m_idlephase2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idlephase3Animation;
            public BasicAnimationInfo idlephase3Animation => m_idlephase3Animation;
            [SerializeField]
            private BasicAnimationInfo m_backgroundidleAnimation;
            public BasicAnimationInfo backgroundidleAnimation => m_backgroundidleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;

            [Title("Projectiles")]
           
            [SerializeField]
            private SimpleProjectileAttackInfo m_petalProjectile;
            public SimpleProjectileAttackInfo petalProjectile => m_petalProjectile;
            

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_moveLowHP.SetData(m_skeletonDataAsset);

                m_clawattack.SetData(m_skeletonDataAsset);
                m_jump.SetData(m_skeletonDataAsset);
                m_landingAnimation.SetData(m_skeletonDataAsset);
                m_backgroundLandingAnimation.SetData(m_skeletonDataAsset);
                m_backgroundJumpAnimation.SetData(m_skeletonDataAsset);
                m_petalProjectile.SetData(m_skeletonDataAsset);
                m_petalBackgroundLeft.SetData(m_skeletonDataAsset);
                m_petalBackgroundRight.SetData(m_skeletonDataAsset);
                m_petalBackgroundBoth.SetData(m_skeletonDataAsset);

                m_rageAnimation.SetData(m_skeletonDataAsset);
                m_idlephase1Animation.SetData(m_skeletonDataAsset);
                m_idlephase2Animation.SetData(m_skeletonDataAsset);
                m_idlephase3Animation.SetData(m_skeletonDataAsset);
                m_backgroundidleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);

#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            
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
            ClawAttack,
            JumpAttack,
            PetalAttack,
            StalagmiteAttack4,
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
        private GameObject m_damageCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_modelTransform;
        [SerializeField, TabGroup("Reference")]
        private float m_petalAmount;
        [SerializeField, TabGroup("Reference")]
        private SkeletonAnimation m_skeleton;
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
        private ParticleFX m_landingCueFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_flinchFX;
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
        private Transform m_backgroundSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_stalagmiteLandingSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_stalagmiteLandingSpawnPoint2;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_petalProjectileSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointA;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointB;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointC;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointD;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointE;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointF;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_flowerSpawnPointG;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_StalagmiteSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform[] m_StalagmiteSpawnPoint2;


        private float m_groundPosition;
        private List<Vector2> m_targetPositions;

        private bool m_stickToGround;
        private bool m_seedSpawning;
        private float m_currentCD;

        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern1;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern2;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern3;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern4;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern5;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern6;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern7;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern8;
        [SerializeField, TabGroup("PatternTracker")]
        private int[] FlowerPattern9;




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

        private void ApplyPhaseData(PhaseInfo obj)
        {

            m_currentPhaseIndex = obj.phaseIndex;
            
        }

        private void ChangeState()
        {
            m_phaseHandle.ApplyChange();

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

        

        private IEnumerator ChangePhaseRoutine()
        {
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitbox.SetInvulnerability(Invulnerability.MAX); //wasTrue
            m_currentCD = 0;
            m_bodyCollider.SetActive(false);
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.rageAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rageAnimation.animation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
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
            m_movement.Stop();
            m_isDetecting = false;
        }

        #region Attacks

        #region PetalAttack
        private void LaunchPetalProjectile(Vector2 target, Transform spawnPoint)
        {
         
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
            m_stateHandle.Wait(State.Cooldown);
            m_petalStartFX.Play();
            yield return new WaitForSeconds(1.25f);
            m_petalEndFX.Play();
            for (int i = 0; i < m_currentPetalAmount; i++)
            {
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
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

       
        private IEnumerator BackGroundDlowerRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
           
            
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion
        private IEnumerator ClawRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.clawattack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.clawattack.animation);
            m_animation.SetAnimation(0, m_info.idlephase1Animation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #region LeapAttack


        private IEnumerator LeapAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX); //wasTrue
            m_animation.SetAnimation(0, m_info.jump.animation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector2(m_targetInfo.position.x, transform.position.y-5);
            m_landingCueFX.Play();
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.landingAnimation, false);
            m_landFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landingAnimation.animation); 
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idlephase1Animation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        private IEnumerator LeapFlowerAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX); //wasTrue
            m_damageCollider.SetActive(false);
            m_skeleton.GetComponent<MeshRenderer>().sortingLayerName = "Background";
            m_animation.SetAnimation(0, m_info.jump.animation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = m_backgroundSpawnPoint.position;
            m_animation.SetAnimation(0, m_info.backgroundLandingAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.backgroundLandingAnimation.animation);
            m_animation.SetAnimation(0, m_info.backgroundidleAnimation, false);
            yield return new WaitForSeconds(1.5f);
            int randomFlowerPatternNumber = UnityEngine.Random.Range(0, 5);
           Debug.Log("mantis random :"+ randomFlowerPatternNumber);
            if (randomFlowerPatternNumber == 0)
            {
                
                for (int i = 0; i < FlowerPattern1.Length; i++)
                {
                    if (FlowerPattern1[i] == 0)
                    {
                        for (int x = 0; x < m_flowerSpawnPointA.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundRight, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointA[x].position, m_flowerSpawnPointA[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    if (FlowerPattern1[i] == 1)
                    {
                        for (int x = 0; x < m_flowerSpawnPointB.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundLeft, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointB[x].position, m_flowerSpawnPointB[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    m_animation.SetAnimation(0, m_info.backgroundidleAnimation, true);
                    yield return new WaitForSeconds(2f);
                }

                
            }
            if (randomFlowerPatternNumber == 1)
            {
                
                for (int i = 0; i < FlowerPattern2.Length; i++)
                {
                    if (FlowerPattern2[i] == 0)
                    {
                        for (int x = 0; x < m_flowerSpawnPointA.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundRight, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointA[x].position, m_flowerSpawnPointA[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    if (FlowerPattern2[i] == 1)
                    {
                        for (int x = 0; x < m_flowerSpawnPointB.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundLeft, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointB[x].position, m_flowerSpawnPointB[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    m_animation.SetAnimation(0, m_info.backgroundidleAnimation, true);
                    yield return new WaitForSeconds(2f);
                }
                
            }
            if (randomFlowerPatternNumber == 2)
            {
               
                for (int i = 0; i < FlowerPattern3.Length; i++)
                {
                    if (FlowerPattern3[i] == 0)
                    {
                        for (int x = 0; x < m_flowerSpawnPointA.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundRight, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointA[x].position, m_flowerSpawnPointA[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    if (FlowerPattern3[i] == 1)
                    {
                        for (int x = 0; x < m_flowerSpawnPointB.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundLeft, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointB[x].position, m_flowerSpawnPointB[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    m_animation.SetAnimation(0, m_info.backgroundidleAnimation, true);
                    yield return new WaitForSeconds(2f);
                }
                
            }
            if (randomFlowerPatternNumber == 3)
            {
                
                for (int i = 0; i < FlowerPattern4.Length; i++)
                {
                    if (FlowerPattern4[i] == 0)
                    {
                        for (int x = 0; x < m_flowerSpawnPointA.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundRight, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointA[x].position, m_flowerSpawnPointA[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    if (FlowerPattern4[i] == 1)
                    {
                        for (int x = 0; x < m_flowerSpawnPointB.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundLeft, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointB[x].position, m_flowerSpawnPointB[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    m_animation.SetAnimation(0, m_info.backgroundidleAnimation, true);
                    yield return new WaitForSeconds(2f);
                }
                
            }
            if (randomFlowerPatternNumber == 4)
            {
                
                for (int i = 0; i < FlowerPattern5.Length; i++)
                {
                    if (FlowerPattern5[i] == 0)
                    {
                        for (int x = 0; x < m_flowerSpawnPointA.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundRight, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointA[x].position, m_flowerSpawnPointA[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    if (FlowerPattern5[i] == 1)
                    {
                        for (int x = 0; x < m_flowerSpawnPointB.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundLeft, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointB[x].position, m_flowerSpawnPointB[x].rotation);
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    m_animation.SetAnimation(0, m_info.backgroundidleAnimation, true);
                    yield return new WaitForSeconds(2f);
                }
                
            }
           
            m_animation.SetAnimation(0, m_info.backgroundJumpAnimation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector2(m_targetInfo.position.x, transform.position.y - 5);
            m_landingCueFX.Play();
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.landingAnimation, false);
            m_landFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landingAnimation.animation);
            m_skeleton.GetComponent<MeshRenderer>().sortingLayerName = "PlayableGround";
            m_damageCollider.SetActive(true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            if (m_currentPhaseIndex == 2)
            {
                m_animation.SetAnimation(0, m_info.idlephase2Animation, true);
            }
            if (m_currentPhaseIndex == 3)
            {
                m_animation.SetAnimation(0, m_info.idlephase3Animation, true);
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator LeapFlowerAttack2Routine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX); //wasTrue
            m_damageCollider.SetActive(false);
            m_animation.SetAnimation(0, m_info.jump.animation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = m_backgroundSpawnPoint.position;
            m_animation.SetAnimation(0, m_info.backgroundLandingAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.backgroundLandingAnimation.animation);
            m_animation.SetAnimation(0, m_info.backgroundidleAnimation, false);
            yield return new WaitForSeconds(1.5f);
            int randomFlowerPatternNumber = UnityEngine.Random.Range(0, 4);
            Debug.Log("mantis random :" + randomFlowerPatternNumber);
            if (randomFlowerPatternNumber == 0)
            {

                for (int i = 0; i < FlowerPattern6.Length; i++)
                {
                    if (FlowerPattern6[i] == 2)
                    {
                        for (int x = 0; x < m_flowerSpawnPointC.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointC[x].position, m_flowerSpawnPointC[x].rotation);
                          
                        }
                    }
                    if (FlowerPattern6[i] == 3)
                    {
                        for (int x = 0; x < m_flowerSpawnPointD.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointD[x].position, m_flowerSpawnPointD[x].rotation);
                        }
                    }
                    if (FlowerPattern6[i] == 4)
                    {
                        for (int x = 0; x < m_flowerSpawnPointE.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointE[x].position, m_flowerSpawnPointE[x].rotation);
                        }
                    }
                    if (FlowerPattern6[i] == 5)
                    {
                        for (int x = 0; x < m_flowerSpawnPointF.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointF[x].position, m_flowerSpawnPointF[x].rotation);
                        }
                    }
                    if (FlowerPattern6[i] == 6)
                    {
                        for (int x = 0; x < m_flowerSpawnPointG.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointG[x].position, m_flowerSpawnPointG[x].rotation);
                        }
                    }
                    yield return new WaitForSeconds(3f);
                }


            }
            if (randomFlowerPatternNumber == 1)
            {

                for (int i = 0; i < FlowerPattern7.Length; i++)
                {
                    if (FlowerPattern7[i] == 2)
                    {
                        for (int x = 0; x < m_flowerSpawnPointC.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointC[x].position, m_flowerSpawnPointC[x].rotation);

                        }
                    }
                    if (FlowerPattern7[i] == 3)
                    {
                        for (int x = 0; x < m_flowerSpawnPointD.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointD[x].position, m_flowerSpawnPointD[x].rotation);
                        }
                    }
                    if (FlowerPattern7[i] == 4)
                    {
                        for (int x = 0; x < m_flowerSpawnPointE.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointE[x].position, m_flowerSpawnPointE[x].rotation);
                        }
                    }
                    if (FlowerPattern7[i] == 5)
                    {
                        for (int x = 0; x < m_flowerSpawnPointF.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointF[x].position, m_flowerSpawnPointF[x].rotation);
                        }
                    }
                    if (FlowerPattern7[i] == 6)
                    {
                        for (int x = 0; x < m_flowerSpawnPointG.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointG[x].position, m_flowerSpawnPointG[x].rotation);
                        }
                    }
                    yield return new WaitForSeconds(3f);
                }


            }
            if (randomFlowerPatternNumber == 2)
            {

                for (int i = 0; i < FlowerPattern8.Length; i++)
                {
                    if (FlowerPattern8[i] == 2)
                    {
                        for (int x = 0; x < m_flowerSpawnPointC.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointC[x].position, m_flowerSpawnPointC[x].rotation);

                        }
                    }
                    if (FlowerPattern8[i] == 3)
                    {
                        for (int x = 0; x < m_flowerSpawnPointD.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointD[x].position, m_flowerSpawnPointD[x].rotation);
                        }
                    }
                    if (FlowerPattern8[i] == 4)
                    {
                        for (int x = 0; x < m_flowerSpawnPointE.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointE[x].position, m_flowerSpawnPointE[x].rotation);
                        }
                    }
                    if (FlowerPattern8[i] == 5)
                    {
                        for (int x = 0; x < m_flowerSpawnPointF.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointF[x].position, m_flowerSpawnPointF[x].rotation);
                        }
                    }
                    if (FlowerPattern8[i] == 6)
                    {
                        for (int x = 0; x < m_flowerSpawnPointG.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointG[x].position, m_flowerSpawnPointG[x].rotation);
                        }
                    }
                    yield return new WaitForSeconds(3f);
                }


            }
            if (randomFlowerPatternNumber == 3)
            {

                for (int i = 0; i < FlowerPattern9.Length; i++)
                {
                    if (FlowerPattern9[i] == 2)
                    {
                        for (int x = 0; x < m_flowerSpawnPointC.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointC[x].position, m_flowerSpawnPointC[x].rotation);

                        }
                    }
                    if (FlowerPattern9[i] == 3)
                    {
                        for (int x = 0; x < m_flowerSpawnPointD.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointD[x].position, m_flowerSpawnPointD[x].rotation);
                        }
                    }
                    if (FlowerPattern9[i] == 4)
                    {
                        for (int x = 0; x < m_flowerSpawnPointE.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointE[x].position, m_flowerSpawnPointE[x].rotation);
                        }
                    }
                    if (FlowerPattern9[i] == 5)
                    {
                        for (int x = 0; x < m_flowerSpawnPointF.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointF[x].position, m_flowerSpawnPointF[x].rotation);
                        }
                    }
                    if (FlowerPattern9[i] == 6)
                    {
                        for (int x = 0; x < m_flowerSpawnPointG.Length; x++)
                        {
                            m_animation.SetAnimation(0, m_info.petalBackgroundBoth, true);
                            Instantiate(m_info.flowerBulb, m_flowerSpawnPointG[x].position, m_flowerSpawnPointG[x].rotation);
                        }
                    }
                    yield return new WaitForSeconds(3f);
                }


            }


            m_animation.SetAnimation(0, m_info.backgroundJumpAnimation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector2(m_targetInfo.position.x, transform.position.y - 5);
            m_landingCueFX.Play();
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.landingAnimation, false);
            m_landFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landingAnimation.animation);
            m_damageCollider.SetActive(true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            if (m_currentPhaseIndex == 2)
            {
                m_animation.SetAnimation(0, m_info.idlephase2Animation, true);
            }
            if (m_currentPhaseIndex == 3)
            {
                m_animation.SetAnimation(0, m_info.idlephase3Animation, true);
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator StalagmiteLeapAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX); //wasTrue
            m_animation.SetAnimation(0, m_info.jump.animation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = m_stalagmiteLandingSpawnPoint.position;
            m_landingCueFX.Play();
            int randomStalagmitepattern = UnityEngine.Random.Range(0, 2);
            Debug.Log("mantis stalagmite pattern :" + randomStalagmitepattern);
            if (randomStalagmitepattern == 0)
            {
                
            }
            else
            {
                
            }
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.landingAnimation, false);
            m_landFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landingAnimation.animation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idlephase3Animation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        #endregion
        #region Movement
        private void MoveToTarget()
        {
            if (!IsTargetInRange(m_info.clawattack.range) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_moveAnim, true);
                //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_moveSpeed);
            }
            else
            {
                m_movement.Stop();
                if (m_currentPhaseIndex == 1)
                {
                    m_animation.SetAnimation(0, m_info.idlephase1Animation, true);
                }
                if (m_currentPhaseIndex == 2)
                {
                    m_animation.SetAnimation(0, m_info.idlephase2Animation, true);
                }
                if (m_currentPhaseIndex == 3)
                {
                    m_animation.SetAnimation(0, m_info.idlephase3Animation, true);
                }
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

 
        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.PetalAttack, m_info.petalProjectile.range),
                                    new AttackInfo<Attack>(Attack.JumpAttack, m_info.
                                    jump.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
          
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
            AddToAttackCache( Attack.JumpAttack, Attack.PetalAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.petalProjectile.range, m_info.jump.range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentPetalAmount = m_petalAmount;
        }

        protected override void Start()
        {
            base.Start();
            //m_flinchHandle.gameObject.SetActive(false);
            //m_spineListener.Subscribe(m_info.mantisEvent, LaunchProjectile);
            m_currentCooldownSpeed = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
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
           
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    if (m_currentPhaseIndex == 1)
                    {
                        m_animation.SetAnimation(0, m_info.idlephase1Animation, true);
                    }
                    if (m_currentPhaseIndex == 2)
                    {
                        m_animation.SetAnimation(0, m_info.idlephase2Animation, true);
                    }
                    if (m_currentPhaseIndex == 3)
                    {
                        m_animation.SetAnimation(0, m_info.idlephase3Animation, true);
                    }
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {

                        m_animation.SetAnimation(0, m_info.idlephase1Animation, true);
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
                    if (m_currentPhaseIndex == 1)
                    {
                        m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idlephase1Animation.animation);
                    }
                    if (m_currentPhaseIndex == 2)
                    {
                        m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idlephase2Animation.animation);
                    }
                    if (m_currentPhaseIndex == 3)
                    {
                        m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idlephase3Animation.animation);
                    }
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (IsFacingTarget())
                    {

                        if (IsTargetInRange(m_info.clawattack.range)&& m_currentPhaseIndex == 1)   
                        {
                            StartCoroutine(ClawRoutine());
                        }
                        else 
                        {
                            switch (m_currentAttack)
                            {
                                
                                case Attack.JumpAttack:
                                    m_groundPosition = transform.position.y;
                                    if (m_currentPhaseIndex == 1)
                                    {
                                        StartCoroutine(LeapAttackRoutine());
                                    }
                                    if (m_currentPhaseIndex == 2)
                                    {
                                        StartCoroutine(LeapFlowerAttackRoutine());
                                    }
                                    if (m_currentPhaseIndex == 3)
                                    {
                                        //StartCoroutine(LeapFlowerAttack2Routine());
                                        int randomFlowerattackNumber = UnityEngine.Random.Range(0, 2);
                                       Debug.Log("mantis flower attack :" + randomFlowerattackNumber);
                                        if(randomFlowerattackNumber == 0)
                                        {
                                            StartCoroutine(LeapFlowerAttackRoutine());
                                        }
                                        else
                                        {
                                            StartCoroutine(LeapFlowerAttack2Routine());
                                        }
                                        
                                    }
                                    
                                    break;
                                
                                case Attack.PetalAttack:
                                    if (m_currentPhaseIndex == 3)
                                    {
                                        int randomStalagmiteattackNumber = UnityEngine.Random.Range(0, 2);
                                        Debug.Log("mantis stalagmite attack :" + randomStalagmiteattackNumber);
                                        if (randomStalagmiteattackNumber == 0)
                                        {
                                            for (int i = 0; i < m_currentPetalAmount; i++)
                                            {
                                                m_targetPositions.Add(CalculatePositions());
                                            }
                                            StartCoroutine(PetalFXRoutine(m_targetInfo.position));
                                        }
                                        else
                                        {
                                            StartCoroutine(StalagmiteLeapAttackRoutine());
                                        }

                                    }
                                    else
                                    {
                                        for (int i = 0; i < m_currentPetalAmount; i++)
                                        {
                                            m_targetPositions.Add(CalculatePositions());
                                        }
                                        StartCoroutine(PetalFXRoutine(m_targetInfo.position));
                                    }
                                    
                                    break;
                               
                            }
                        }
                        
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    m_attackDecider.hasDecidedOnAttack = false;
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
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            if (m_currentPhaseIndex == 1)
                            {
                                m_animation.SetAnimation(0, m_info.idlephase1Animation, true);
                            }
                            if (m_currentPhaseIndex == 2)
                            {
                                m_animation.SetAnimation(0, m_info.idlephase2Animation, true);
                            }
                            if (m_currentPhaseIndex == 3)
                            {
                                m_animation.SetAnimation(0, m_info.idlephase3Animation, true);
                            }
                        }
                    }

                    if (m_currentCD <= m_currentCooldownSpeed)
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
