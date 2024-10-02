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
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using UnityEngine.Playables;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/BlackDeath")]
    public class BlackDeathAI : CombatAIBrain<BlackDeathAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;


            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("BladeThrow")]
            private SimpleAttackInfo m_attackDaggers = new SimpleAttackInfo();
            public SimpleAttackInfo attackDaggers => m_attackDaggers;
            [SerializeField, TabGroup("BladeThrow")]
            private SimpleAttackInfo m_attackDaggersIdle = new SimpleAttackInfo();
            public SimpleAttackInfo attackDaggersIdle => m_attackDaggersIdle;
            [SerializeField, TabGroup("BladeThrow")]
            private SimpleAttackInfo m_attackDaggersTurn = new SimpleAttackInfo();
            public SimpleAttackInfo attackDaggersTurn => m_attackDaggersTurn;
            [SerializeField, TabGroup("BladeThrow")]
            private float m_bladeThrowDuration;
            public float bladeThrowDurtation => m_bladeThrowDuration;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_ScytyheSlashComboRootMotion = new SimpleAttackInfo();
            public SimpleAttackInfo ScytyheSlashComboRootMotion => m_ScytyheSlashComboRootMotion;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField, TabGroup("BloodLightning")]
            private SimpleAttackInfo m_bloodLightningAttack = new SimpleAttackInfo();
            public SimpleAttackInfo bloodLightningAttack => m_bloodLightningAttack;
            [SerializeField, TabGroup("BloodLightning")]
            private BasicAnimationInfo m_bloodLightningIdleAnimation;
            public BasicAnimationInfo bloodLightningIdleAnimation => m_bloodLightningIdleAnimation;
            [SerializeField, TabGroup("BloodLightning")]
            private BasicAnimationInfo m_bloodLightningEndAnimation;
            public BasicAnimationInfo bloodLightningEndAnimation => m_bloodLightningEndAnimation;
            [SerializeField, TabGroup("BladeScytheSummon")]
            private SimpleAttackInfo m_attack7 = new SimpleAttackInfo();
            public SimpleAttackInfo attack7 => m_attack7;
            [SerializeField, TabGroup("BladeScytheSummon")]
            private SimpleAttackInfo m_scytheThrow = new SimpleAttackInfo();
            public SimpleAttackInfo scytheThrow => m_scytheThrow;
            [SerializeField, TabGroup("GiantBladeSummon")]
            private float m_giantBladeSummonDurtation;
            public float giantBladeSummonDurtation => m_giantBladeSummonDurtation;
            [SerializeField, TabGroup("ShadowClone")]
            private SimpleAttackInfo m_summonCloneAttack = new SimpleAttackInfo();
            public SimpleAttackInfo summonCloneAttack => m_summonCloneAttack;


            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_absorption;
            public BasicAnimationInfo absorption => m_absorption;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinch2Animation;
            public BasicAnimationInfo flinch2Animation => m_flinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_introAnimation;
            public BasicAnimationInfo introAnimation => m_introAnimation;
            [SerializeField]
            private BasicAnimationInfo m_teleportAppearAnimation;
            public BasicAnimationInfo teleportAppearAnimation => m_teleportAppearAnimation;
            [SerializeField]
            private BasicAnimationInfo m_teleportVanishAnimation;
            public BasicAnimationInfo teleportVanishAnimation => m_teleportVanishAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_introFXAnimation;
            public BasicAnimationInfo introFXAnimation => m_introFXAnimation;

            [Title("Projectiles")]/*
            [SerializeField]
            private GameObject m_tentacle;
            public GameObject tentacle => m_tentacle;*/
            [SerializeField]
            private GameObject m_bloodLightning;
            public GameObject bloodLightning => m_bloodLightning;
            [SerializeField]
            private GameObject m_clone;
            public GameObject clone => m_clone;
            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_deathFXEvent;
            public string deathFXEvent => m_deathFXEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_teleportFXEvent;
            public string teleportFXEvent => m_teleportFXEvent;
            
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_blackDeathSlash1On;
            public string blackDeathSlash1On => m_blackDeathSlash1On;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_blackDeathSlash1Off;
            public string blackDeathSlash1Off => m_blackDeathSlash1Off;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_blackDeathSlash2On;
            public string blackDeathSlash2On => m_blackDeathSlash2On;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_blackDeathSlash2Off;
            public string blackDeathSlash2Off => m_blackDeathSlash2Off;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_blackDeathSlash3On;
            public string blackDeathSlash3On => m_blackDeathSlash3On;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_blackDeathSlash3Off;
            public string blackDeathSlash3Off => m_blackDeathSlash3Off;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_scytheThrowAbove;
            public string scytheThrowAbove => m_scytheThrowAbove;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_scytheThrowGround;
            public string scytheThrowGround => m_scytheThrowGround;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_attackDaggers.SetData(m_skeletonDataAsset);
                m_attackDaggersIdle.SetData(m_skeletonDataAsset);
                m_attackDaggersTurn.SetData(m_skeletonDataAsset);
                m_ScytyheSlashComboRootMotion.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_attack7.SetData(m_skeletonDataAsset);
                m_scytheThrow.SetData(m_skeletonDataAsset);
                m_absorption.SetData(m_skeletonDataAsset);
                m_summonCloneAttack.SetData(m_skeletonDataAsset);
                m_bloodLightningAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_bloodLightningIdleAnimation.SetData(m_skeletonDataAsset);
                m_bloodLightningEndAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinch2Animation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_introAnimation.SetData(m_skeletonDataAsset);
                m_teleportAppearAnimation.SetData(m_skeletonDataAsset);
                m_teleportVanishAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_introFXAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;
            [SerializeField]
            private List<int> m_patternAttackCount;
            public List<int> patternAttackCount => m_patternAttackCount;
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

        private enum Pattern
        {
            AttackPattern1,
            AttackPattern2,
            AttackPattern3,
            AttackPattern4,
            AttackPattern5,
            AttackPattern6,
            AttackPattern7,
            WaitAttackEnd,
        }

        private enum Attack
        {
            //phase1
            ScytheSlashDualBladeThrow,
            ScytheSlashBladeOfDarkness,
            BloodLightningRPattern,
            BladeDarknessRPatternOneTwo,
            BladeThrowABA,
            GuardsEdgeBladeDarkness,
            //phase2
            ScytheSlashSingleBladeThrow,
            ScytheSlashBladeDarknessMurmurGuardsEdge,
            TripleBloodLightningOrBladeDarkness,
            RandomTripleBloodLightning,
            TeleportBladeThrow,
            //phase3
            TeleportGuardsEdgeMurmursMark,
            TeleportScytheSlashGuardsEdge,
            Pattern4TripleBloodLightning,

            TeleportBladeThrowWithRotations,
            TeleportDiagonalsGuardsEdge,
            TeleportShadowBladeThrowWithRotations,
            //phase4
            TeleportMultipleBloodLightningP1,
            TeleportMultipleBloodLightningP2,
            TeleportTargetBloodLightning,
            TeleportSingleBloodLightning,








            /*ScytheSlash,
            TeleportScytheSlash,
            BladeThrow,
            BladeCircleThrow,
            TentacleBlades,
            TentacleAttackUp,
            TentacleAttackFront,
            GiantBlades,
            ShadowClone,
            BloodLightning,
            BloodLightningBarrage,*/
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
        private List<Pattern> m_attackCache;
        private List<GameObject> m_clones;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_deathFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_teleportFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_slashGroundFX;
        [SerializeField, TabGroup("Hit Detector")]
        private BlackDeathHitDetector m_hitDetector;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject SlashCollider;
        [SerializeField, TabGroup("Spawn Points")]
        private BlackDeathSoundMark m_soundMark;
        [SerializeField, TabGroup("Spawn Points")]
        private Collider2D m_randomSpawnCollider;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_lightningChaseContainer;
        [SerializeField, TabGroup("Spawn Points")]
        private RoyalDeathGuardScytheProjectile m_royalDeathGuardScytheProjectile;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_teleportPoints;
        [SerializeField, TabGroup("Spawn Points")]
        private List<BlackDeathBladeOfDarknessSequenceHandle> m_blackDeathBladeOfDarknessSequenceHandle;
        [SerializeField, TabGroup("Spawn Points")]
        private List<ProjectileScatterHandle> m_bladeThrowHandler;
        [SerializeField, TabGroup("Spawn Points")]
        private List<BlackDeathBloodLightingBehaviourHandle> m_BlackDeathBloodLightingBehaviourHandle;
        [SerializeField, TabGroup("Spawn Points")]
        private List<BlackDeathBloodLightingBehaviourHandle> m_BlackDeathBloodLightingBehaviourHandle2;
        [SerializeField, TabGroup("Spawn Points")]
        private List<BlackDeathShadowClone> m_blackDeathClone;
        [SerializeField, TabGroup("Spawn Points")]
        private List<GameObject> BlackDeathSlashFXs;
        

        [SerializeField]
        private PlayableDirector m_phaseChangeAttackTimeline;
        [SerializeField]
        private PlayableDirector m_phaseChangeFloorBreakTimeline;

        private int m_currentPhaseIndex;
        private int m_attackCount;
        private int m_hitCount;
        private int[] m_patternAttackCount;
        private int m_currentBladeLoops;
        private int m_currentLightningCount;
        private int m_currentTentacleCount;
        private float m_currentLightningSummonDuration;
        private bool m_isDetecting;
        private int m_numberOfProjectiles;

        [SerializeField]
        private Transform m_leftBounds;
        [SerializeField]
        private Transform m_rightBounds;


        private void ApplyPhaseData(PhaseInfo obj)
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChangeState()
        {
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_phaseHandle.ApplyChange();
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null && m_stateHandle.currentState == State.Idle)
            {
                base.SetTarget(damageable, m_target);
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.OverrideState(State.Intro);
                    //GameEventMessage.SendEvent("Boss Encounter");
                }

                var chaseLightningBehaviors = m_lightningChaseContainer.GetComponentsInChildren<BlackDeathBloodLightningChaseHandle>();
                for (int i = 0; i < chaseLightningBehaviors.Length; i++)
                {
                    chaseLightningBehaviors[i].SetTarget(m_targetInfo.transform);
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            yield return new WaitForSeconds(2);
            m_animation.SetAnimation(0, m_info.move.animation, true);
            yield return new WaitForSeconds(5);
            GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
            m_animation.SetAnimation(0, m_info.introAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.introAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.teleportVanishAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportVanishAnimation);
            transform.position = new Vector2(m_randomSpawnCollider.bounds.center.x, m_randomSpawnCollider.bounds.center.y - 5);
            m_animation.SetAnimation(0, m_info.teleportAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportAppearAnimation);
            //m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true); //Temp
            //yield return new WaitForSeconds(5f);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true); //Temp
            //Debug.Log("Stop PHase Change");

            switch (m_phaseHandle.currentPhase)
            {
         
                case Phase.PhaseTwo:
                    m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                    yield return PhaseChangeAttackRoutine(3);
                    break;
                case Phase.PhaseThree:
                    m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                    yield return PhaseChangeAttackRoutine(3);
                    yield return PhaseChangeFloorBreakRoutine();
                    break;
                case Phase.PhaseFour:
                    m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                    var random = UnityEngine.Random.Range(0, 4);
                    for (int i = 0; i < 2; i++)
                    {
                        m_animation.SetAnimation(0, m_info.absorption, true);
                        m_BlackDeathBloodLightingBehaviourHandle2[random].Execute();
                        yield return new WaitForSeconds(1f);
                    }
                    break;
          
            }

            /*StartCoroutine(m_phaseHandle.currentPhase == Phase.PhaseThree ? BloodLightningBarragePhase3Routine(m_currentLightningCount) : BloodLightningBarrageRoutine(m_currentLightningCount));*/
            yield return null;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator PhaseChangeAttackRoutine(int loopCount)
        {
            m_phaseChangeAttackTimeline.stopped += OnPhaseChangeAttackStopped;

            bool isTimelineDone = false;
            for (int i = 0; i <= loopCount; i++)
            {
                isTimelineDone = false;
                m_phaseChangeAttackTimeline.Play();
                while (isTimelineDone == false)
                {
                    yield return null;
                }
            }

            void OnPhaseChangeAttackStopped(PlayableDirector obj)
            {
                isTimelineDone = true;
            }
        }

        private IEnumerator PhaseChangeFloorBreakRoutine()
        {
            m_phaseChangeFloorBreakTimeline.stopped += OnPhaseChangeAttackStopped;

            bool isTimelineDone = false;

            m_phaseChangeFloorBreakTimeline.Play();
            while (isTimelineDone == false)
            {
                yield return null;
            }

            void OnPhaseChangeAttackStopped(PlayableDirector obj)
            {
                isTimelineDone = true;
            }
        }



        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_blackDeathClone.Count; i++)
            {
                m_blackDeathClone[i].gameObject.SetActive(false);
            }

            Debug.Log("on na kooooo");
            StopAllCoroutines();
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_isDetecting = false;
            m_hitbox.enabled = false;
            m_deathHandle.enabled = true;
            base.OnDestroyed(sender, eventArgs);
        }

        #region Attacks
        private IEnumerator MurmursMarkRoutine()
        {
            /*m_soundMarkAnticipation.transform.position = m_targetInfo.position;
            m_soundMarkAnticipation.Play();*/
 
            m_soundMark.transform.position = m_targetInfo.position;
            m_soundMark.Activate(m_targetInfo.transform);
            m_animation.SetAnimation(0, m_info.absorption, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.absorption);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //while (m_soundMark.isActivated)
            //{

            //    yield return null;
            //}

        }

        private IEnumerator ScytheSlashDualBladeThrowAttack()
        {
            //teleport to player
            m_stateHandle.Wait(State.ReevaluateSituation);
            var missCounter = 0;
            //var canTeleport = false;
            Debug.Log("Return to top?");
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(20, -5));
            while (!m_isPlayerHit && missCounter != 2)
            {

                // canTeleport = true;
                for (int i = 0; i < 2; i++)
                {
                    Debug.Log("ScytheSlash");
                    yield return ScytheSlashRoutine();
                }
                if (m_isPlayerHit == true)
                {
                    break;
                }
                missCounter++;
                yield return null;
            }
            Debug.Log(missCounter);
            if (m_isPlayerHit == true)
            {
                Debug.Log("Hit");
                m_isPlayerHit = false;
                m_hitCount = 0;
                yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            }
            else
            {
                yield return TeleportToTargetRoutine(CenterPointOfTheArena(), new Vector2(0f, 0f));
                m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                for (int i = 0; i < 2; i++)
                {
                    yield return BladeThrowRoutineBatchARoutine(false, 0);
                    yield return new WaitForSeconds(1f);
                }
                //teleport away from player
                yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(20, -5));
            }

            DecidedOnAttack(false);
            Debug.Log("Attack 1 phase 1");
            m_stateHandle.ApplyQueuedState();

        }// phase 1 attack pattern 1
        private IEnumerator ScytheSlashBladeOfDarknessAttack()
        {
            //teleport to player
            m_stateHandle.Wait(State.ReevaluateSituation);
            var missCounter = 0;
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(20, -5));
            while (!m_isPlayerHit && missCounter != 2)
            {

                // canTeleport = true;
                for (int i = 0; i < 2; i++)
                {
                    Debug.Log("ScytheSlash " + i + " " + missCounter);
                    yield return ScytheSlashRoutine();
                }
                if (m_isPlayerHit == true)
                {
                    missCounter = 0;
                    break;
                }
                missCounter++;
                yield return null;
            }
            Debug.Log(missCounter);
            if (m_isPlayerHit == true)
            {
                Debug.Log("Hit");
                m_isPlayerHit = false;
                m_hitCount = 0;
            }
            else
            {
                Debug.Log("Balde of darkness!!");
                //blade of darkness
                m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
                yield return BladeOfDarknessRoutinePattern(false, 0);
            }
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(20, -5));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//phase 1 attack pattern 2
        private IEnumerator BladeOfDarknessRoutinePattern(bool isPatterned, int pattern = 0)
        {
            var random = UnityEngine.Random.Range(0, 4);
            BlackDeathBladeOfDarknessSequenceHandle cacheHandle = null;
            if (!isPatterned)
            {
                switch (random)
                {
                    case 0:
                        yield return TeleportToTargetRoutine(m_teleportPoints[0].position, new Vector2(20, -5));
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[0];
                        break;
                    case 1:
                        yield return TeleportToTargetRoutine(m_teleportPoints[1].position, new Vector2(20, -5));
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[1];
                        break;
                    case 2:
                        yield return TeleportToTargetRoutine(m_teleportPoints[0].position, new Vector2(20, -5));
                        BladeThrowProjectileLauncher(2);
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[0];
                        break;
                    case 3:
                        yield return TeleportToTargetRoutine(m_teleportPoints[0].position, new Vector2(20, -5));
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[2];
                        break;
                }
                cacheHandle.Execute();
                while (cacheHandle.isExecutingSequence)
                {
                    yield return null;
                }
            }
            else
            {
                switch (pattern)
                {
                    case 0:
                        yield return TeleportToTargetRoutine(m_teleportPoints[0].position, new Vector2(20, -5));
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[0];
                        break;
                    case 1:
                        yield return TeleportToTargetRoutine(m_teleportPoints[1].position, new Vector2(20, -5));
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[1];
                        break;
                    case 2:
                        yield return TeleportToTargetRoutine(m_teleportPoints[0].position, new Vector2(20, -5));
                        BladeThrowProjectileLauncher(2);
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[0];
                        break;
                    case 3:
                        yield return TeleportToTargetRoutine(m_teleportPoints[0].position, new Vector2(20, -5));
                        cacheHandle = m_blackDeathBladeOfDarknessSequenceHandle[2];
                        break;
                }
                cacheHandle.Execute();
                while (cacheHandle.isExecutingSequence)
                {
                    yield return null;
                }
            }




        }


        private IEnumerator TeleportBloodAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            /* for (int i = 0; i < 3; i++)
             {
                 yield return BloodLightningBarrageRoutine(1);

             }*/

            for (int i = 0; i < 3; i++)
            {
                Debug.LogWarning(i);
                var random = UnityEngine.Random.Range(0, 6);
                m_animation.SetAnimation(0, m_info.absorption, true);
                m_BlackDeathBloodLightingBehaviourHandle[random].Execute();
                yield return new WaitForSeconds(2f);
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //yield return BloodLightningBarrageRoutine(1);
            //teleport away from player
            yield return new WaitForSeconds(1f);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//phase 1 attack pattern 3
        private IEnumerator BladeOfDarknessTeleportAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
            yield return BladeOfDarknessRoutinePattern(false);
            //teleport away from player
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//phase 1 attack pattern 4
        private IEnumerator PatternedBladeThrowAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return BladeThrowRoutineBatchARoutine(true);
            yield return new WaitForSeconds(1f);
            BladeThrowRoutineBatchBRoutine(true);
            yield return new WaitForSeconds(1f);
            yield return BladeThrowRoutineBatchARoutine(true);
            yield return new WaitForSeconds(1f);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//phase 1 attack pattern 5

        private IEnumerator GuardsEdgeBladeOfDarknesssAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var bladeOfDarknessPattern1 = 0;
            var bladeOfDarknessPattern2 = 1;
            var random = UnityEngine.Random.Range(0, 1);
            switch (random)
            {
                case 0:
                    m_animation.SetAnimation(0, m_info.attack7, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack7);
                    break;
                case 1:
                    m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                    for (int i = 0; i < 2; i++)
                    {
                        yield return BladeOfDarknessRoutinePattern(true, bladeOfDarknessPattern1);
                        Debug.Log("DakoAkonP2Y");
                    }
                    yield return BladeOfDarknessRoutinePattern(true, bladeOfDarknessPattern2);
                    break;
            }
            yield return null;
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//phase 1 attack pattern 6
        //phase 2
        private IEnumerator ScytheSlashSingleBladeThrowAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var missCounter = 0f;
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(0f, 0f));
            while (!m_isPlayerHit && missCounter != 3)
            {
                yield return ScytheSlashRoutine();
                if (m_isPlayerHit == true)
                {
                    break;
                }
                missCounter++;
                yield return null;
            }
            Debug.Log(missCounter);
            if (m_isPlayerHit == true)
            {
                Debug.Log("Hit");
                m_isPlayerHit = false;
                m_hitCount = 0;
            }
            else
            {
                yield return TeleportToTargetRoutine(CenterPointOfTheArena(), new Vector2(0f, 0f));
                m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                yield return BladeThrowRoutineBatchARoutine(true);

            }
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//phase 2 attack pattern 1
        private IEnumerator ScytheSlashBladeDarknessMurmurGuardsEdgeAttack()
        {

            m_stateHandle.Wait(State.ReevaluateSituation);
            var missCounter = 0f;
            //if hit proceed to chose random attack then teleport awa
            while (!m_isPlayerHit && missCounter != 2)
            {
                yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(20f, -5f));
                yield return ScytheSlashRoutine();
                if (m_isPlayerHit == true)
                {
                    break;
                }
                missCounter++;
                yield return null;
            }
            var randomizedBladeOfDarkness3or4 = UnityEngine.Random.Range(2, 4);
            if (m_isPlayerHit == true)
            {
                m_isPlayerHit = false;
                m_hitCount = 0;
                switch (randomizedBladeOfDarkness3or4)
                {
                    case 2:
                        m_animation.SetAnimation(0, m_info.absorption, true);
                        yield return MurmursMarkRoutine();
                        break;
                    case 3:
                        Debug.Log(randomizedBladeOfDarkness3or4);
                        m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                        yield return BladeOfDarknessRoutinePattern(true, randomizedBladeOfDarkness3or4);
                        break;
                }
            }
            else
            {
                m_animation.SetAnimation(0, m_info.scytheThrow, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrow.animation);
                Debug.Log(" GuardsEdgeRoutine()");
                //yield return GuardsEdgeRoutine();
            }


            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done //phase 2 attack pattern 2
        private IEnumerator TripleBloodLightningOrBladeDarkness()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var random = UnityEngine.Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        Debug.LogWarning(i);
                        var randomForLightning = UnityEngine.Random.Range(0, 6);
                        m_animation.SetAnimation(0, m_info.absorption, true);
                        m_BlackDeathBloodLightingBehaviourHandle[randomForLightning].Execute();
                        yield return new WaitForSeconds(2f);
                    }
                    break;
                case 1:
                    m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                    for (int i = 0; i < 2; i++)
                    {
                        yield return BladeOfDarknessRoutinePattern(true, 2);
                    }
                    yield return BladeOfDarknessRoutinePattern(true, 3);
                    break;

            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done//phase 2 attack pattern 3

        private IEnumerator RandomTripleBloodLightningAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            //randomizeblood lighting
            m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
            for (int i = 0; i < 3; i++)
            {
                var randomForLightning = UnityEngine.Random.Range(0, 6);
                m_animation.SetAnimation(0, m_info.absorption, true);
                m_BlackDeathBloodLightingBehaviourHandle[randomForLightning].Execute();
                yield return new WaitForSeconds(2f);

            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();

        }//done phase 2 attack pattern 4

        private IEnumerator TeleportBladeThrow()
        {
            //change this to center of the scene
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TeleportToTargetRoutine(CenterPointOfTheArena(), new Vector2(0f, 0f));
            for (int i = 0; i < 3; i++)
            {
                m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
                Debug.Log("asdsadaa" + i);
                yield return BladeThrowRoutineBatchARoutine(true);
            }
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();

        }//done phase 2 attack pattern 5
        public void BladeThrowProjectileLauncher(int attackPattern)
        {
            m_bladeThrowHandler[attackPattern].SpawnProjectiles();
            m_bladeThrowHandler[attackPattern].LaunchSpawnedProjectiles();
        }

        private IEnumerator BladeThrowRoutineBatchBRoutine(bool isRandomized, int batchCombo = 0)
        {
            if (isRandomized)
            {
                Debug.Log("Randomized");
                m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                var random = UnityEngine.Random.Range(0, 3);
                if (random == 0)
                {
                    BladeThrowProjectileLauncher(0);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(1);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(0);
                }
                else if (random == 1)
                {
                    BladeThrowProjectileLauncher(1);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(0);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(1);
                }
                else
                {
                    BladeThrowProjectileLauncher(2);
                    yield return new WaitForSeconds(1f);
                }
            }
            else
            {
                Debug.Log("patterned");
                m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                if (batchCombo == 1)
                {
                    BladeThrowProjectileLauncher(0);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(1);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(0);
                }
                else if (batchCombo == 2)
                {
                    BladeThrowProjectileLauncher(1);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(0);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(1);
                }
                else
                {
                    BladeThrowProjectileLauncher(2);
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return new WaitForSeconds(1f);

        }
        private IEnumerator BladeThrowRoutineBatchARoutine(bool isRandomized, int batchCombo = 0)
        {
            if (isRandomized)
            {
                Debug.Log("Randomized");
                m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                var random = UnityEngine.Random.Range(0, 2);
                if (random == 0)
                {
                    BladeThrowProjectileLauncher(0);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(1);
                }
                else
                {
                    BladeThrowProjectileLauncher(1);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(0);
                }
            }
            else
            {
                Debug.Log("patterned");
                m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
                if (batchCombo == 1)
                {
                    BladeThrowProjectileLauncher(0);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(1);
                }
                else
                {
                    BladeThrowProjectileLauncher(1);
                    yield return new WaitForSeconds(1f);
                    BladeThrowProjectileLauncher(0);
                }
            }
            yield return new WaitForSeconds(1f);

        }
        //phase 3 
        private IEnumerator TeleportGuardsEdgeMurmursMarkAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var missCounter = 0;
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            yield return new WaitForSeconds(0.5f);
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(-10f, 0f));
            while (!m_isPlayerHit && missCounter != 3)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                    
                }
                yield return ScytheSlashForwardRoutine();
                if (m_isPlayerHit == true)
                {

                    break;
                }
                missCounter++;
                yield return null;
            }
            if (m_isPlayerHit == true)
            {
                m_isPlayerHit = false;
                m_hitCount = 0;
            }
           
            if (!IsFacingTarget())
            {
                CustomTurn();

            }
            m_animation.SetAnimation(0, m_info.scytheThrow, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrow);
            //teleport away again but above player either front or back with offset
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(-20f, 10f));
            yield return new WaitForSeconds(0.5f);
            if (!IsFacingTarget())
            {
                CustomTurn();

            }
            m_animation.SetAnimation(0, m_info.attack7, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack7);
            yield return MurmursMarkRoutine();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }//done phase 3 attack pattern 1

        private IEnumerator TeleportScytheSlashGuardsEdge()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var missCounter = 0;
            var random = UnityEngine.Random.Range(0, 2);
            while (!m_isPlayerHit && missCounter != 2)
            {
                yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
                //teleport away but teleport front or back of player????
                yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(0f, 0f));
                yield return ScytheSlashForwardRoutine();
                if (m_isPlayerHit == true)
                {
                    break;
                }
                missCounter++;
                yield return null;
            }
            if (m_isPlayerHit)
            {
                m_hitCount = 0;
                m_isPlayerHit = false;
                switch (random)
                {
                    case 0:
                        yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(0f, 0f));
                        yield return ScytheSlashRoutine();
                        break;
                    case 1:
                        yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(-20f, 10f));
                        if (!IsFacingTarget())
                        {
                            CustomTurn();
                        }
                        m_animation.SetAnimation(0, m_info.scytheThrow, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrow);
                        break;
                }

            }
            else
            {

                yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(0f, 0f));
                m_animation.DisableRootMotion();
                yield return ScytheSlashRoutine();
            }


            //teleport away again but above player either front or back with offset
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(-20f, 10f));
            m_animation.SetAnimation(0, m_info.scytheThrow, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrow);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            m_animation.DisableRootMotion();
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done phase 3 attack pattern 2
        private IEnumerator Pattern4TripleBloodLightning()
        {
            var chase = m_BlackDeathBloodLightingBehaviourHandle[4];
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.absorption, true);
            chase.Execute();
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//donephase 3 attack pattern 3 not done
        //re use randomize blood lightning strike 


        private IEnumerator TeleportBladeThrowWithRotations()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var random = UnityEngine.Random.Range(0, 2);
            yield return TeleportToTargetRoutine(CenterPointOfTheArena(), new Vector2(0f, 0f));
            m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
            BladeThrowProjectileLauncher(5);
            yield return new WaitForSeconds(2f);
            switch (random)
            {
                case 0://clockwise rotation and counter clockwise

                    //yield return BladeThrowRoutine(32, 1);
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Right, true);
                    yield return new WaitForSeconds(2f);
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Left, true);
                    break;
                case 1://counter clockwise and clockwise rotation
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Left, true);
                    yield return new WaitForSeconds(2f);
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Right, true);
                    break;
            }
            // All directions?
            yield return new WaitForSeconds(2f);
            BladeThrowProjectileLauncher(5);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            yield return new WaitForSeconds(2f);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done phase 3 attack pattern 5

        private IEnumerator TeleportDiagonalsGuardsEdge()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(-20f, 5f));
            m_animation.SetAnimation(0, m_info.attack7, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack7);
            yield return new WaitForSeconds(1f);
            //Teleport to the player front or back?!
            yield return TeleportToTargetRoutine(m_targetInfo.position, new Vector2(-20f, 0f));
            m_animation.SetAnimation(0, m_info.scytheThrow, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrow);
            //yield return GuardsEdgeRoutine();
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done

        private IEnumerator TeleportShadowBladeThrowWithRotations()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            //teleport to center of the scene 
            yield return TeleportToTargetRoutine(CenterPointOfTheArena(), new Vector2(0f, 0f));

            for (int i = 0; i < m_blackDeathClone.Count; i++)
            {
                if (!m_blackDeathClone[i].isActivated)
                {
                    m_blackDeathClone[i].Appear();
                }

            }

            var random = UnityEngine.Random.Range(0, 2);
            BladeThrowProjectileLauncher(5);
            yield return new WaitForSeconds(2f);
            m_animation.SetAnimation(0, m_info.attackDaggersIdle, true);
            switch (random)
            {
                case 0://clockwise rotation and counter clockwise
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Right, true);
                    yield return new WaitForSeconds(2f);
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Left, true);
                    break;
                case 1://counter clockwise and clockwise rotation
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Left, true);
                    yield return new WaitForSeconds(2f);
                    m_bladeThrowHandler[3].SpawnProjectileInSequence(HorizontalDirection.Right, true);
                    break;
            }
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done
        //phase 4
        private IEnumerator TeleportMultipleBloodLightningP1()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var numberOfTeleports = 2;
            for (int i = 0; i < numberOfTeleports; i++)
            {
                yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
                yield return null;

            }

            for (int i = 0; i < 6; i++)
            {
                var random = UnityEngine.Random.Range(0, 4);
                m_animation.SetAnimation(0, m_info.absorption, true);
                m_BlackDeathBloodLightingBehaviourHandle2[random].Execute();
                yield return new WaitForSeconds(1f);
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 2; i++)
            {
                m_animation.SetAnimation(0, m_info.absorption, true);
                m_BlackDeathBloodLightingBehaviourHandle2[i].Execute();
                yield return new WaitForSeconds(1f);
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(1f);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();


        }//done
        private IEnumerator TeleportMultipleBloodLightningP2()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var numberOfTeleports = 2;

            for (int i = 0; i < numberOfTeleports; i++)
            {
                yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
                yield return null;

            }
            for (int i = 0; i < 4; i++)
            {
                var random = UnityEngine.Random.Range(0, 4);
                m_animation.SetAnimation(0, m_info.absorption, true);
                m_BlackDeathBloodLightingBehaviourHandle2[random].Execute();
                yield return new WaitForSeconds(1f);
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 4; i++)
            {
                m_animation.SetAnimation(0, m_info.absorption, true);
                m_BlackDeathBloodLightingBehaviourHandle2[i].Execute();
                yield return new WaitForSeconds(1f);
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(1f);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();

        }

        private IEnumerator TeleportTargetBloodLightning()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var chase = m_BlackDeathBloodLightingBehaviourHandle[4];
            for (int i = 0; i < 3; i++)
            {
                yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            }

            //sensor chase type lightning? 
            m_animation.SetAnimation(0, m_info.absorption, true);
            chase.Execute();
            //teleport away from player
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
        }//done
        private IEnumerator TeleportSingleBloodLightning()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            m_animation.SetAnimation(0, m_info.absorption, true);
            m_BlackDeathBloodLightingBehaviourHandle[3].Execute();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(2f);
            yield return TeleportToTargetRoutine(RandomTeleportPoint(), new Vector2(0f, 0f));
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();

        }//done

        //re use shadow clone function boss!!!!
        #endregion
        #region Routine
        private IEnumerator BladeThrowRoutine(int numberOfProjectiles, int rotations)
        {
            /*if (m_currentPattern == Pattern.AttackPattern5 && m_attackCount == 3 || m_currentPattern == Pattern.AttackPattern7 && m_attackCount == 3)
            {
                m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.attackDaggers.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackDaggers.animation);
                m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
            }*/
            m_animation.SetAnimation(0, m_info.attackDaggers.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackDaggers.animation);
            m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
            for (int x = 0; x < rotations; x++)
            {
                float angleStep = 360f / numberOfProjectiles;
                float angle = 0f;
                for (int y = 0; y < 2; y++)
                {
                    if (y == 1)
                    {
                        angle = 69; //( ͡° ͜ʖ ͡°) hihi
                    }
                    for (int z = 0; z < numberOfProjectiles; z++)
                    {
                        Vector2 startPoint = new Vector2(m_projectilePoint.position.x, m_projectilePoint.position.y);
                        float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * 5;
                        float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * 5;

                        Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                        Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * m_info.projectile.projectileInfo.speed;

                        GameObject projectile = m_info.projectile.projectileInfo.projectile;
                        var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                        instance.transform.position = m_projectilePoint.position;
                        var component = instance.GetComponent<Projectile>();
                        component.ResetState();
                        component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                        Vector2 v = component.GetComponent<Rigidbody2D>().velocity;
                        var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                        component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);

                        angle += angleStep;
                    }
                    yield return new WaitForSeconds(.5f);
                }
                yield return new WaitForSeconds(m_info.bladeThrowDurtation);
            }
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator BladeThrow2Routine(int numberOfProjectiles, int rotations)
        {
            m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
            var randomAttack = UnityEngine.Random.Range(0, 2);
            float angleStep = 360f / numberOfProjectiles;
            float angle = 0f;
            for (int y = 0; y < rotations; y++)
            {
                if (y == 1)
                {
                    randomAttack = randomAttack == 1 ? 0 : 1;
                }
                for (int z = 0; z < numberOfProjectiles; z++)
                {
                    Vector2 startPoint = new Vector2(m_projectilePoint.position.x, m_projectilePoint.position.y);
                    float projectileDirXposition = startPoint.x + (randomAttack == 1 ? Mathf.Sin((angle * Mathf.PI) / 180) : Mathf.Cos((angle * Mathf.PI) / 180)) * 5;
                    float projectileDirYposition = startPoint.y + (randomAttack == 1 ? Mathf.Cos((angle * Mathf.PI) / 180) : Mathf.Sin((angle * Mathf.PI) / 180)) * 5;

                    Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                    Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * m_info.projectile.projectileInfo.speed;

                    GameObject projectile = m_info.projectile.projectileInfo.projectile;
                    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                    instance.transform.position = m_projectilePoint.position;
                    var component = instance.GetComponent<Projectile>();
                    component.ResetState();
                    component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                    Vector2 v = component.GetComponent<Rigidbody2D>().velocity;
                    var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                    component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);

                    angle += angleStep;
                    yield return new WaitForSeconds(.1f);
                }
                yield return new WaitForSeconds(.5f);
            }
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return null;
        }

        /*private IEnumerator GiantBladesRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack7.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack7.animation);
            m_animation.SetAnimation(0, m_info.attack6B.animation, true);
            yield return new WaitForSeconds(m_info.giantBladeSummonDurtation);
            m_animation.SetAnimation(0, m_info.attack6C.animation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack6C.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }*/

        private IEnumerator ScytheSlashRoutine()
        {
            //m_slashGroundFX.transform.localScale = new Vector3(transform.localScale.x == 1 ? -4.026736f : 4.026736f, m_slashGroundFX.transform.localScale.y, m_slashGroundFX.transform.localScale.z);
            var fxRenderer = m_slashGroundFX.GetComponent<ParticleSystemRenderer>();
            fxRenderer.flip = transform.localScale.x == 1 ? new Vector3(1, 0, 0) : Vector3.zero;
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            if (m_groundSensor.isDetecting)
            {
                m_slashGroundFX.Play();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            if (m_groundSensor.isDetecting)
            {
                m_slashGroundFX.Play();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            if (m_groundSensor.isDetecting)
            {
                m_slashGroundFX.Play();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack3.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);

            //m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void BlackDeathSlash1ColliderFXOn()
        {
            SlashCollider.SetActive(true);
            BlackDeathSlashFXs[0].SetActive(true); 
        }
        private void BlackDeathSlash1ColliderFXOff()
        {
            SlashCollider.SetActive(false);
            BlackDeathSlashFXs[0].SetActive(false);
        }
        private void BlackDeathSlash2ColliderFXOn()
        {
            SlashCollider.SetActive(true);
            BlackDeathSlashFXs[1].SetActive(true);
        }
        private void BlackDeathSlash2ColliderFXOff()
        {
            SlashCollider.SetActive(false);
            BlackDeathSlashFXs[1].SetActive(false);
        }
        private void BlackDeathSlash3ColliderFXOn()
        {
            SlashCollider.SetActive(true);
            BlackDeathSlashFXs[2].SetActive(true);
        }
        private void BlackDeathSlash3ColliderFXOff()
        {
            SlashCollider.SetActive(false);
            BlackDeathSlashFXs[2].SetActive(false);
        }
        public void BlackDeathSlash2ColliderFX(bool choice)
        {
            switch (choice)
            {
                case true:
                    SlashCollider.SetActive(choice);
                    BlackDeathSlashFXs[1].SetActive(choice);
                    break;
                case false:
                    SlashCollider.SetActive(choice);
                    BlackDeathSlashFXs[1].SetActive(choice);
                    break;
            }
        }
        public void BlackDeathSlash3ColliderFX(bool choice)
        {
            switch (choice)
            {
                case true:
                    SlashCollider.SetActive(choice);
                    BlackDeathSlashFXs[2].SetActive(choice);
                    break;
                case false:
                    SlashCollider.SetActive(choice);
                    BlackDeathSlashFXs[2].SetActive(choice);
                    break;
            }
        }


        //public void 
        private IEnumerator ScytheSlashForwardRoutine()
        {
            //m_slashGroundFX.transform.localScale = new Vector3(transform.localScale.x == 1 ? -4.026736f : 4.026736f, m_slashGroundFX.transform.localScale.y, m_slashGroundFX.transform.localScale.z);
            m_animation.EnableRootMotion(true, false);
            var fxRenderer = m_slashGroundFX.GetComponent<ParticleSystemRenderer>();
            fxRenderer.flip = transform.localScale.x == 1 ? new Vector3(1, 0, 0) : Vector3.zero;
            m_animation.SetAnimation(0, m_info.ScytyheSlashComboRootMotion.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ScytyheSlashComboRootMotion.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);

        }
        /*private IEnumerator TentacleBladesRoutine(int tentacleCount)
        {
            if (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.teleportVanishAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportVanishAnimation);
                transform.position = new Vector2(transform.position.x, GroundPosition(m_projectilePoint.position).y);
                m_animation.SetAnimation(0, m_info.teleportAppearAnimation, false);
            }
            m_animation.SetAnimation(0, m_info.attack6A.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack6A.animation);
            m_animation.SetAnimation(0, m_info.attack6B.animation, true);
            for (int y = 0; y < tentacleCount; y++)
            {
                int offset = 5;
                for (int z = 0; z < 20; z++)
                {
                    GameObject instance1 = Instantiate(m_info.tentacle, new Vector2(transform.position.x + offset, GroundPosition(new Vector2(transform.position.x + offset, transform.position.y)).y), Quaternion.identity);
                    GameObject instance2 = Instantiate(m_info.tentacle, new Vector2(transform.position.x - offset, GroundPosition(new Vector2(transform.position.x + offset, transform.position.y)).y), Quaternion.identity);
                    yield return new WaitForSeconds(.1f);
                    offset += 5;
                }
            }
            yield return new WaitForSeconds(m_info.giantBladeSummonDurtation);
            m_animation.SetAnimation(0, m_info.attack6C.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack6C.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }*/

        //private IEnumerator TentacleSummonRoutine()
        //{
        //    yield return null;
        //}

        /*private IEnumerator TentacleUpRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack4.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack4.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator TentacleFrontRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack5.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack5.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }*/



        private IEnumerator BloodLightningBarrageRoutine(int lightningCount)
        {
            m_animation.SetAnimation(0, m_info.bloodLightningAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bloodLightningAttack.animation);
            m_animation.SetAnimation(0, m_info.bloodLightningIdleAnimation, true);
            float skip = 0;
            float skipCache = 0;
            for (int y = 0; y < lightningCount; y++)
            {
                float offset = 0;
                float offsetAdd = 15 + (y * 5);
                while (skipCache == skip)
                {
                    skip = UnityEngine.Random.Range(1, 8);
                    yield return null;
                }
                skipCache = skip;

                for (int z = 0; z < 10; z++)
                {
                    if (z == 5)
                    {
                        offset = 0;
                        offsetAdd = -offsetAdd;
                    }
                    if (z != 0)
                        offset = offset + offsetAdd;
                    if (z != skip)
                    {
                        GameObject instance = Instantiate(m_info.bloodLightning, new Vector2(m_randomSpawnCollider.bounds.center.x + offset, GroundPosition(m_randomSpawnCollider.bounds.center).y), Quaternion.identity);
                        instance.transform.position = new Vector2(instance.transform.position.x, GroundPosition(instance.transform.position).y);
                    }
                }
                yield return new WaitForSeconds(m_currentLightningSummonDuration);
            }
            m_animation.SetAnimation(0, m_info.bloodLightningEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bloodLightningEndAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
        }

        private IEnumerator BloodLightningBarragePhase3Routine(int lightningCount)
        {
            m_animation.SetAnimation(0, m_info.bloodLightningAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bloodLightningAttack.animation);
            m_animation.SetAnimation(0, m_info.bloodLightningIdleAnimation, true);
            for (int y = 0; y < lightningCount; y++)
            {
                float offset = 0;
                float offsetAdd = 10 /*+ (y * 5)*/;
                var startPoint = m_randomSpawnCollider.bounds.center.x + 30;

                for (int z = 0; z < 10; z++)
                {
                    if (z == 5)
                    {
                        offset = 0;
                        offsetAdd = -offsetAdd;
                        startPoint = m_randomSpawnCollider.bounds.center.x - 30;
                    }
                    if (z != 0)
                        offset = offset + offsetAdd;
                    GameObject instance = Instantiate(m_info.bloodLightning, new Vector2(startPoint + offset, GroundPosition(m_randomSpawnCollider.bounds.center).y), Quaternion.identity);
                    instance.transform.position = new Vector2(instance.transform.position.x, GroundPosition(instance.transform.position).y);
                }

                yield return new WaitForSeconds(m_currentLightningSummonDuration);
            }
            m_animation.SetAnimation(0, m_info.bloodLightningEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bloodLightningEndAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private IEnumerator TeleportToTargetRoutine(Vector2 target, Vector2 positionOffset/*, bool isGrounded*/)
        {
            m_animation.SetAnimation(0, m_info.teleportVanishAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportVanishAnimation);
            transform.position = new Vector2(target.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffset.x : positionOffset.x), /*isGrounded ? GroundPosition().y :*/ target.y + positionOffset.y);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.teleportAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportAppearAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //if (attack == Attack.WaitAttackEnd)
            //{
            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //    m_stateHandle.ApplyQueuedState();
            //}
            //else
            //{
            //   /* ExecuteAttack(attack);*/
            //}
            yield return null;
        }
        private Vector3 CenterPointOfTheArena()
        {
            var center = transform.position = (m_leftBounds.position + m_rightBounds.position) / 2f;
            return center;
        }
        private Vector3 RandomTeleportPoint()
        {
            Vector3 randomPos = transform.position;
            while (Vector2.Distance(transform.position, randomPos) <= 50f)
            {
                randomPos = m_randomSpawnCollider.bounds.center + new Vector3(
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.x,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.y,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.z);
            }
            return randomPos;
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
            // m_patternDecider.hasDecidedOnAttack = condition;
            m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            //testing
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.TeleportScytheSlashGuardsEdge, m_info.targetDistanceTolerance));
            /*m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern3, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern4, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern5, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern6, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern7, m_info.targetDistanceTolerance));
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.BladeThrow, m_info.attackDaggers.range)
                                  , new AttackInfo<Attack>(Attack.GiantBlades, m_info.attack7.range)
                                  , new AttackInfo<Attack>(Attack.ScytheSlash, m_info.attack1.range)
                                  , new AttackInfo<Attack>(Attack.ShadowClone, m_info.summonCloneAttack.range)
            //                      , new AttackInfo<Attack>(Attack.TentacleBlades, m_info.attack6A.range));*/
            //switch (m_phaseHandle.currentPhase)
            //{
            //    case Phase.PhaseOne:
            //        m_attackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSlashDualBladeThrow, m_info.targetDistanceTolerance),
            //            new AttackInfo<Attack>(Attack.ScytheSlashBladeOfDarkness, m_info.targetDistanceTolerance),
            //            new AttackInfo<Attack>(Attack.BloodLightningRPattern, m_info.targetDistanceTolerance),
            //            new AttackInfo<Attack>(Attack.BladeDarknessRPatternOneTwo, m_info.targetDistanceTolerance),
            //            new AttackInfo<Attack>(Attack.BladeThrowABA, m_info.targetDistanceTolerance),
            //            new AttackInfo<Attack>(Attack.GuardsEdgeBladeDarkness, m_info.targetDistanceTolerance));
            //        break;
            //    case Phase.PhaseTwo:
            //        m_attackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSlashSingleBladeThrow, m_info.targetDistanceTolerance),
            //           new AttackInfo<Attack>(Attack.ScytheSlashBladeDarknessMurmurGuardsEdge, m_info.targetDistanceTolerance),
            //           new AttackInfo<Attack>(Attack.TripleBloodLightningOrBladeDarkness, m_info.targetDistanceTolerance),
            //           new AttackInfo<Attack>(Attack.RandomTripleBloodLightning, m_info.targetDistanceTolerance),
            //           new AttackInfo<Attack>(Attack.TeleportBladeThrow, m_info.targetDistanceTolerance));
            //        break;
            //    case Phase.PhaseThree:
            //        m_attackDecider.SetList(new AttackInfo<Attack>(Attack.TeleportGuardsEdgeMurmursMark, m_info.targetDistanceTolerance),
            //          new AttackInfo<Attack>(Attack.TeleportScytheSlashGuardsEdge, m_info.targetDistanceTolerance),
            //          new AttackInfo<Attack>(Attack.Pattern4TripleBloodLightning, m_info.targetDistanceTolerance),
            //          new AttackInfo<Attack>(Attack.RandomTripleBloodLightning, m_info.targetDistanceTolerance),
            //          new AttackInfo<Attack>(Attack.TeleportBladeThrowWithRotations, m_info.targetDistanceTolerance),
            //          new AttackInfo<Attack>(Attack.TeleportDiagonalsGuardsEdge, m_info.targetDistanceTolerance),
            //          new AttackInfo<Attack>(Attack.TeleportShadowBladeThrowWithRotations, m_info.targetDistanceTolerance));
            //        break;
            //    case Phase.PhaseFour:
            //        m_attackDecider.SetList(new AttackInfo<Attack>(Attack.TeleportMultipleBloodLightningP1, m_info.targetDistanceTolerance),
            //        new AttackInfo<Attack>(Attack.TeleportMultipleBloodLightningP2, m_info.targetDistanceTolerance),
            //        new AttackInfo<Attack>(Attack.TeleportSingleBloodLightning, m_info.targetDistanceTolerance),
            //        new AttackInfo<Attack>(Attack.TeleportTargetBloodLightning, m_info.targetDistanceTolerance),
            //        new AttackInfo<Attack>(Attack.TeleportShadowBladeThrowWithRotations, m_info.targetDistanceTolerance));
            //        break;

            //}

            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private bool m_isPlayerHit;
        private void AddHitCount(object sender, EventActionArgs eventArgs)
        {
            m_hitCount++;
            m_isPlayerHit = true;
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

        void AddToAttackCache(params Pattern[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }
        void ScytheThrowAbove()
        {
            m_royalDeathGuardScytheProjectile.ExecuteFlight(new Vector3(m_character.centerMass.position.x, m_character.centerMass.position.y + 40f, m_character.centerMass.position.z), m_targetInfo.position);
        }
        void ScytheThrowGround()
        {
            var scytheDestinationOffset = m_character.facing == HorizontalDirection.Left ? Vector3.left : Vector3.right;
            m_royalDeathGuardScytheProjectile.ExecuteFlight(m_character.centerMass.position, m_character.centerMass.position + scytheDestinationOffset);
        }

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_hitDetector.PlayerHit += AddHitCount;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoint);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_clones = new List<GameObject>();
            m_clones.Add(null);

        }
        protected override void Start()
        {
            base.Start();
            //Slash1
            m_spineListener.Subscribe(m_info.blackDeathSlash1On, BlackDeathSlash1ColliderFXOn);
            m_spineListener.Subscribe(m_info.blackDeathSlash1Off, BlackDeathSlash1ColliderFXOff);
            //Slash2
            m_spineListener.Subscribe(m_info.blackDeathSlash2On, BlackDeathSlash2ColliderFXOn);
            m_spineListener.Subscribe(m_info.blackDeathSlash2Off, BlackDeathSlash2ColliderFXOff);
            //Slash3
            m_spineListener.Subscribe(m_info.blackDeathSlash3On, BlackDeathSlash3ColliderFXOn);
            m_spineListener.Subscribe(m_info.blackDeathSlash3Off, BlackDeathSlash3ColliderFXOff);

            m_spineListener.Subscribe(m_info.scytheThrowAbove, ScytheThrowAbove);
            m_spineListener.Subscribe(m_info.scytheThrowGround, ScytheThrowGround);
            m_spineListener.Subscribe(m_info.deathFXEvent, m_deathFX.Play);
            m_spineListener.Subscribe(m_info.teleportFXEvent, m_teleportFX.Play);
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            m_phaseHandle.MonitorPhase();

            if (m_damageable.health.currentValue <= 0f)
            {
                m_deathHandle.enabled = true;

                for (int i = 0; i < m_blackDeathClone.Count; i++)
                {
                    m_blackDeathClone[i].gameObject.SetActive(false);
                }
            }
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        //StartCoroutine(IntroRoutine());
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
                    Debug.Log("Phase Time");
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    Debug.Log("Turning Steet");
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    StopAllCoroutines();
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();
                    }
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.ScytheSlashDualBladeThrow:
                            StartCoroutine(ScytheSlashDualBladeThrowAttack());
                            break;
                        case Attack.ScytheSlashBladeOfDarkness:
                            StartCoroutine(ScytheSlashBladeOfDarknessAttack());
                            break;
                        case Attack.BloodLightningRPattern:
                            StartCoroutine(TeleportBloodAttack());
                            break;
                        case Attack.BladeDarknessRPatternOneTwo:
                            StartCoroutine(BladeOfDarknessTeleportAttack());
                            break;
                        case Attack.BladeThrowABA:
                            StartCoroutine(PatternedBladeThrowAttack());
                            break;
                        case Attack.GuardsEdgeBladeDarkness:
                            StartCoroutine(GuardsEdgeBladeOfDarknesssAttack());
                            break;
                        case Attack.ScytheSlashSingleBladeThrow:
                            StartCoroutine(ScytheSlashSingleBladeThrowAttack());
                            break;
                        case Attack.ScytheSlashBladeDarknessMurmurGuardsEdge:
                            StartCoroutine(ScytheSlashBladeDarknessMurmurGuardsEdgeAttack());
                            break;
                        case Attack.TripleBloodLightningOrBladeDarkness:
                            StartCoroutine(TripleBloodLightningOrBladeDarkness());
                            break;
                        case Attack.RandomTripleBloodLightning:
                            StartCoroutine(RandomTripleBloodLightningAttack());
                            break;
                        case Attack.TeleportBladeThrow:
                            StartCoroutine(TeleportBladeThrow());
                            break;
                        case Attack.TeleportGuardsEdgeMurmursMark:
                            StartCoroutine(TeleportGuardsEdgeMurmursMarkAttack());
                            break;
                        case Attack.TeleportScytheSlashGuardsEdge:
                            StartCoroutine(TeleportScytheSlashGuardsEdge());
                            break;
                        case Attack.Pattern4TripleBloodLightning:
                            StartCoroutine(Pattern4TripleBloodLightning());
                            break;
                        case Attack.TeleportBladeThrowWithRotations:
                            StartCoroutine(TeleportBladeThrowWithRotations());
                            break;
                        case Attack.TeleportDiagonalsGuardsEdge:
                            StartCoroutine(TeleportDiagonalsGuardsEdge());
                            break;
                        case Attack.TeleportShadowBladeThrowWithRotations:
                            StartCoroutine(TeleportShadowBladeThrowWithRotations());
                            break;
                        case Attack.TeleportMultipleBloodLightningP1:
                            StartCoroutine(TeleportMultipleBloodLightningP1());
                            break;
                        case Attack.TeleportMultipleBloodLightningP2:
                            StartCoroutine(TeleportMultipleBloodLightningP2());
                            break;
                        case Attack.TeleportTargetBloodLightning:
                            StartCoroutine(TeleportTargetBloodLightning());
                            break;
                        case Attack.TeleportSingleBloodLightning:
                            StartCoroutine(TeleportSingleBloodLightning());
                            break;

                    }
                    Debug.Log("Ground Sensor Detecting is " + m_groundSensor.isDetecting);
                    //switch(m_attackDecider)
                    #region OldAttackBehavior
                    /* switch (m_currentPattern)
                     {
                         case Pattern.AttackPattern1:
                             if (m_attackCount == 0)
                             {
                                 //m_attackCount += m_groundSensor.isDetecting ? 1 : 0;
                                 if (m_attackCount == 0)
                                 {
                                     switch (m_currentPhaseIndex)
                                     {
                                         case 1:
                                             m_attackCount++;
                                             StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.WaitAttackEnd, new Vector2(20, -5)*//*, true*//*));
                                             break;
                                         default:
                                             m_attackCount++;
                                             m_stateHandle.ApplyQueuedState();
                                             break;
                                     }
                                 }
                                 else
                                 {
                                     m_stateHandle.ApplyQueuedState();
                                 }
                             }
                             else
                             {
                                 if (m_attackCount >= m_patternAttackCount[0] *//*&& m_hitCount == 0*//*)
                                 {
                                     if (m_attackCount == m_patternAttackCount[0] + 1)
                                     {
                                         m_stateHandle.Wait(State.Chasing);
                                         StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                     }
                                     else
                                     {
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 3:
                                                 switch (m_currentAttack)
                                                 {
                                                     case Attack.TentacleAttackFront:
                                                         m_currentAttack = Attack.TentacleAttackUp;
                                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)*//*, true*//*));
                                                         break;
                                                     case Attack.TentacleAttackUp:
                                                         m_attackCount++;
                                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackUp, new Vector2(50, -5)*//*, true*//*));
                                                         break;
                                                 }
                                                 break;
                                             case 4:
                                                 m_attackCount++;
                                                 m_stateHandle.ApplyQueuedState();
                                                 break;
                                             default:
                                                 m_attackCount++;
                                                 m_currentBladeLoops = 2;
                                                 StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)*//*, false*//*));
                                                 break;
                                         }
                                     }
                                 }
                                 else
                                 {
                                     if (m_hitCount > 0)
                                     {
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 1:
                                                 m_stateHandle.Wait(State.Chasing);
                                                 StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                 break;
                                             case 2:
                                                 m_attackCount = m_patternAttackCount[0] + 1;
                                                 m_currentBladeLoops = 2;
                                                 StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)*//*, false*//*));
                                                 break;
                                             case 3:
                                                 m_attackCount = m_patternAttackCount[0];
                                                 m_hitCount = 0;
                                                 m_stateHandle.ApplyQueuedState();
                                                 break;
                                         }
                                     }
                                     else
                                     {
                                         m_attackCount++;
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 1:
                                                 ExecuteAttack(Attack.ScytheSlash);
                                                 break;
                                             case 2:
                                                 ExecuteAttack(Attack.TeleportScytheSlash);
                                                 break;
                                             case 3:
                                                 m_currentAttack = Attack.TentacleAttackFront;
                                                 StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.TeleportScytheSlash, Vector2.zero*//*, false*//*));
                                                 break;
                                             case 4:
                                                 if (m_attackCount <= 3)
                                                 {
                                                     StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                     return;
                                                 }
                                                 else if (m_attackCount == 4)
                                                 {
                                                     m_currentLightningSummonDuration = .5f;
                                                     m_currentLightningCount = 6;
                                                     ExecuteAttack(Attack.BloodLightning);
                                                     return;
                                                 }
                                                 else if (m_attackCount == 5)
                                                 {
                                                     m_currentLightningCount = 2;
                                                     m_currentLightningSummonDuration = 1.5f;
                                                     ExecuteAttack(Attack.BloodLightningBarrage);
                                                     return;
                                                 }
                                                 else
                                                 {
                                                     m_stateHandle.ApplyQueuedState();
                                                 }
                                                 break;
                                         }
                                     }
                                 }
                             }
                             //m_stateHandle.OverrideState(State.Chasing);
                             break;
                         case Pattern.AttackPattern2:
                             if (m_attackCount == 0)
                             {
                                 //m_attackCount += m_groundSensor.isDetecting ? 1 : 0;
                                 if (m_attackCount == 0)
                                 {
                                     m_attackCount++;
                                     StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.WaitAttackEnd, new Vector2(20, -5)*//*, true*//*));
                                 }
                                 else
                                 {
                                     m_stateHandle.ApplyQueuedState();
                                 }
                             }
                             else
                             {
                                 if (m_attackCount >= m_patternAttackCount[1] *//*&& m_hitCount == 0*//*)
                                 {
                                     if (m_attackCount == m_patternAttackCount[1] + 1)
                                     {
                                         m_stateHandle.Wait(State.Chasing);
                                         StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                     }
                                     else
                                     {
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 1:
                                                 m_attackCount++;
                                                 m_currentTentacleCount = 3;
                                                 ExecuteAttack(Attack.TentacleBlades);
                                                 break;
                                             case 2:
                                                 m_attackCount++;
                                                 ExecuteAttack(Attack.TentacleAttackFront);
                                                 break;
                                             case 3:
                                                 switch (m_currentAttack)
                                                 {
                                                     case Attack.TentacleAttackUp:
                                                         m_currentAttack = Attack.TentacleAttackFront;
                                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, randomAttack == 1 ? Attack.ScytheSlash : Attack.TentacleAttackUp, randomAttack == 1 ? new Vector2(25 * randomFacing, -5) : new Vector2(50, -5)*//*, true*//*));
                                                         break;
                                                     case Attack.TentacleAttackFront:
                                                         m_attackCount++;
                                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)*//*, true*//*));
                                                         break;
                                                 }
                                                 break;
                                             case 4:
                                                 m_attackCount++;
                                                 m_stateHandle.ApplyQueuedState();
                                                 break;
                                         }
                                     }
                                 }
                                 else
                                 {
                                     if (m_hitCount > 0)
                                     {
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 1:
                                                 m_stateHandle.Wait(State.Chasing);
                                                 StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                 break;
                                             case 2:
                                                 m_attackCount = m_patternAttackCount[1] + 1;
                                                 m_hitCount = 0;
                                                 m_currentTentacleCount = 3;
                                                 StartCoroutine(TeleportToTargetRoutine(randomAttack == 1 ? randomGroundPos : m_targetInfo.position, randomAttack == 1 ? Attack.TentacleBlades : Attack.TentacleAttackFront, randomAttack == 1 ? Vector2.zero : new Vector2(randomFacing * 20, -5)*//*, randomAttack == 1 ? false : true*//*));
                                                 break;
                                             case 3:
                                                 m_attackCount = m_patternAttackCount[1];
                                                 m_hitCount = 0;
                                                 m_stateHandle.ApplyQueuedState();
                                                 break;
                                         }
                                     }
                                     else
                                     {
                                         m_attackCount++;
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 1:
                                                 ExecuteAttack(Attack.ScytheSlash);
                                                 break;
                                             case 2:
                                                 ExecuteAttack(Attack.TeleportScytheSlash);
                                                 break;
                                             case 3:
                                                 m_currentAttack = Attack.TentacleAttackUp;
                                                 StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.TeleportScytheSlash, Vector2.zero*//*, false*//*));
                                                 break;
                                             case 4:
                                                 if (m_attackCount <= 3)
                                                 {
                                                     StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                     return;
                                                 }
                                                 else if (m_attackCount == 4)
                                                 {
                                                     m_currentLightningCount = 4;
                                                     m_currentLightningSummonDuration = 1.5f;
                                                     ExecuteAttack(Attack.BloodLightningBarrage);
                                                     return;
                                                 }
                                                 else if (m_attackCount == 5)
                                                 {
                                                     m_currentLightningSummonDuration = .5f;
                                                     m_currentLightningCount = 4;
                                                     ExecuteAttack(Attack.BloodLightning);
                                                     return;
                                                 }
                                                 else
                                                 {
                                                     m_stateHandle.ApplyQueuedState();
                                                 }
                                                 break;
                                         }
                                     }
                                 }
                             }
                             //m_stateHandle.OverrideState(State.Chasing);
                             break;
                         case Pattern.AttackPattern3:
                             m_attackCount++;
                             switch (m_currentPhaseIndex)
                             {
                                 case 4:
                                     if (m_attackCount <= 3)
                                     {
                                         StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.WaitAttackEnd, new Vector2(0, -10)*//*, false*//*));
                                         return;
                                     }
                                     else if (m_attackCount == 4)
                                     {
                                         m_currentLightningCount = 1;
                                         m_currentLightningSummonDuration = 1;
                                         ExecuteAttack(Attack.BloodLightning);
                                         return;
                                     }
                                     else
                                     {
                                         m_stateHandle.Wait(State.Chasing);
                                         StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                     }
                                     break;
                                 default:
                                     m_stateHandle.Wait(State.Chasing);
                                     m_currentLightningCount = 3;
                                     m_currentLightningSummonDuration = 2;
                                     StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.BloodLightning, new Vector2(0, -10)*//*, false*//*));
                                     break;
                             }
                             ///////
                             //m_stateHandle.OverrideState(State.Chasing);
                             break;
                         case Pattern.AttackPattern4:
                             m_attackCount++;
                             switch (m_currentPhaseIndex)
                             {
                                 case 1:
                                     switch (m_attackCount)
                                     {
                                         case 1:
                                             StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackUp, new Vector2(50, -5)*//*, true*//*));
                                             break;
                                         case 2:
                                             StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25, -5)*//*, true*//*));
                                             break;
                                         case 3:
                                             m_stateHandle.Wait(State.Chasing);
                                             m_currentTentacleCount = 3;
                                             StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, GroundPosition(m_projectilePoint.position).y), Attack.TentacleBlades, Vector2.zero*//*, false*//*));
                                             break;
                                     }
                                     break;
                                 case 4:
                                     switch (m_attackCount)
                                     {
                                         case 1:
                                             StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.WaitAttackEnd, new Vector2(0, -10)*//*, false*//*));
                                             break;
                                         case 2:
                                             m_currentLightningCount = 1;
                                             m_currentLightningSummonDuration = 1;
                                             ExecuteAttack(Attack.BloodLightningBarrage);
                                             break;
                                         case 3:
                                             m_stateHandle.Wait(State.Chasing);
                                             StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                             break;
                                     }
                                     break;
                                 default:
                                     m_stateHandle.Wait(State.Chasing);
                                     m_currentLightningCount = 4;
                                     m_currentLightningSummonDuration = 1.5f;
                                     StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.BloodLightningBarrage, new Vector2(0, -10)*//*, false*//*));
                                     break;
                             }
                             //m_stateHandle.OverrideState(State.Chasing);
                             break;
                         case Pattern.AttackPattern5:
                             switch (m_currentPhaseIndex)
                             {
                                 case 1:
                                     m_stateHandle.OverrideState(State.Chasing);
                                     break;
                                 case 4:
                                     m_stateHandle.OverrideState(State.Chasing);
                                     break;
                                 default:
                                     if (m_attackCount == 0)
                                     {
                                         m_attackCount++;
                                         m_currentBladeLoops = m_currentPhaseIndex == 2 ? 3 : 1;
                                         StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)*//*, false*//*));
                                     }
                                     else
                                     {
                                         switch (m_currentPhaseIndex)
                                         {
                                             case 3:
                                                 switch (m_attackCount)
                                                 {
                                                     case 1:
                                                         m_attackCount++;
                                                         m_currentBladeLoops = 2;
                                                         ExecuteAttack(Attack.BladeCircleThrow);
                                                         //StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeCircleThrow, new Vector2(0, -10)));
                                                         break;
                                                     case 2:
                                                         m_attackCount++;
                                                         m_currentBladeLoops = 2;
                                                         ExecuteAttack(Attack.BladeThrow);
                                                         break;
                                                     case 3:
                                                         m_stateHandle.Wait(State.Chasing);
                                                         StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                         break;
                                                 }
                                                 break;
                                             default:
                                                 m_stateHandle.Wait(State.Chasing);
                                                 StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                 break;
                                         }
                                     }
                                     break;
                             }
                             //m_stateHandle.OverrideState(State.Chasing);
                             break;
                         case Pattern.AttackPattern6:
                             if (m_currentPhaseIndex == 2 || m_currentPhaseIndex == 3)
                             {
                                 m_attackCount++;
                                 switch (m_attackCount)
                                 {
                                     case 1:
                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackUp, new Vector2(50, -5)*//*, true*//*));
                                         break;
                                     case 2:
                                         m_currentTentacleCount = 1;
                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, randomAttack == 1 ? Attack.TentacleBlades : Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)*//*, true*//*));
                                         break;
                                     case 3:
                                         m_currentTentacleCount = 1;
                                         StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)*//*, true*//*));
                                         break;
                                     case 4:
                                         m_stateHandle.Wait(State.Chasing);
                                         StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                         break;
                                 }
                             }
                             else
                             {
                                 m_stateHandle.OverrideState(State.Chasing);
                             }
                             //m_stateHandle.OverrideState(State.Chasing);
                             break;
                         case Pattern.AttackPattern7:
                             switch (m_currentPhaseIndex)
                             {
                                 case 1:
                                     m_stateHandle.OverrideState(State.Chasing);
                                     break;
                                 case 4:
                                     m_stateHandle.OverrideState(State.Chasing);
                                     break;
                                 default:
                                     m_attackCount++;
                                     switch (m_attackCount)
                                     {
                                         case 1:
                                             Debug.Log("Clone Count " + m_clones.Count);
                                             if (m_clones[m_clones.Count - 1] == null || !m_clones[m_clones.Count - 1].activeSelf)
                                             {
                                                 for (int i = 0; i < m_clones.Count; i++)
                                                 {
                                                     Destroy(m_clones[i]);
                                                     m_clones.RemoveAt(i);
                                                 }
                                                 m_currentBladeLoops = 2;
                                                 StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)*//*, false*//*));
                                                 ExecuteAttack(Attack.ShadowClone);
                                             }
                                             else
                                             {
                                                 m_stateHandle.Wait(State.Chasing);
                                                 m_stateHandle.ApplyQueuedState();
                                             }
                                             break;
                                         case 2:
                                             switch (m_currentPhaseIndex)
                                             {
                                                 case 2:
                                                     m_stateHandle.Wait(State.Chasing);
                                                     StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                                     break;
                                                 case 3:
                                                     m_currentBladeLoops = 2;
                                                     ExecuteAttack(Attack.BladeCircleThrow);
                                                     break;
                                             }
                                             break;
                                         case 3:
                                             m_currentBladeLoops = 1;
                                             ExecuteAttack(Attack.BladeThrow);
                                             break;
                                         case 4:
                                             m_currentBladeLoops = 1;
                                             ExecuteAttack(Attack.BladeCircleThrow);
                                             break;
                                         case 5:
                                             m_stateHandle.Wait(State.Chasing);
                                             StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero*//*, false*//*));
                                             break;
                                     }
                                     break;
                             }*/
                    //m_stateHandle.OverrideState(State.Chasing);

                    //}

                    #endregion
                    break;

                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        if (!m_attackDecider.hasDecidedOnAttack)
                        {
                            m_hitCount = 0;
                            m_attackCount = 0;
                            m_stateHandle.SetState(State.Attacking);
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                            m_stateHandle.SetState(State.Turning);
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
        }
        private Vector2 GroundPositionOfBlackDeath()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }
        protected override void OnTargetDisappeared()
        {
            //m_stickToGround = false;
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