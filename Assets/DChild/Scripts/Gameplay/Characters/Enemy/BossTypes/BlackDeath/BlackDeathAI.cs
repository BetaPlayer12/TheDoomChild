﻿using System;
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
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

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
            private float m_bladeThrowDurtation;
            public float bladeThrowDurtation => m_bladeThrowDurtation;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField, TabGroup("ScytheSlash")]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField, TabGroup("BladeSpin")]
            private SimpleAttackInfo m_attack4 = new SimpleAttackInfo();
            public SimpleAttackInfo attack4 => m_attack4;
            [SerializeField, TabGroup("BladeSpin")]
            private SimpleAttackInfo m_attack5 = new SimpleAttackInfo();
            public SimpleAttackInfo attack5 => m_attack5;
            [SerializeField, TabGroup("TentacleBlades")]
            private SimpleAttackInfo m_attack6A = new SimpleAttackInfo();
            public SimpleAttackInfo attack6A => m_attack6A;
            [SerializeField, TabGroup("TentacleBlades")]
            private SimpleAttackInfo m_attack6B = new SimpleAttackInfo();
            public SimpleAttackInfo attack6B => m_attack6B;
            [SerializeField, TabGroup("TentacleBlades")]
            private SimpleAttackInfo m_attack6C = new SimpleAttackInfo();
            public SimpleAttackInfo attack6C => m_attack6C;
            [SerializeField, TabGroup("BloodLightning")]
            private SimpleAttackInfo m_bloodLightningAttack = new SimpleAttackInfo();
            public SimpleAttackInfo bloodLightningAttack => m_bloodLightningAttack;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("BloodLightning")]
            private string m_bloodLightningIdleAnimation;
            public string bloodLightningIdleAnimation => m_bloodLightningIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("BloodLightning")]
            private string m_bloodLightningEndAnimation;
            public string bloodLightningEndAnimation => m_bloodLightningEndAnimation;
            [SerializeField, TabGroup("GiantBladeSummon")]
            private SimpleAttackInfo m_attack7 = new SimpleAttackInfo();
            public SimpleAttackInfo attack7 => m_attack7;
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2Animation;
            public string flinch2Animation => m_flinch2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_introAnimation;
            public string introAnimation => m_introAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_teleportAppearAnimation;
            public string teleportAppearAnimation => m_teleportAppearAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_teleportVanishAnimation;
            public string teleportVanishAnimation => m_teleportVanishAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_introFXAnimation;
            public string introFXAnimation => m_introFXAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private GameObject m_tentacle;
            public GameObject tentacle => m_tentacle;
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

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_attackDaggers.SetData(m_skeletonDataAsset);
                m_attackDaggersIdle.SetData(m_skeletonDataAsset);
                m_attackDaggersTurn.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_attack4.SetData(m_skeletonDataAsset);
                m_attack5.SetData(m_skeletonDataAsset);
                m_attack6A.SetData(m_skeletonDataAsset);
                m_attack6B.SetData(m_skeletonDataAsset);
                m_attack6C.SetData(m_skeletonDataAsset);
                m_attack7.SetData(m_skeletonDataAsset);
                m_summonCloneAttack.SetData(m_skeletonDataAsset);
                m_bloodLightningAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
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
            ScytheSlash,
            TeleportScytheSlash,
            BladeThrow,
            BladeCircleThrow,
            TentacleBlades,
            TentacleAttackUp,
            TentacleAttackFront,
            GiantBlades,
            ShadowClone,
            BloodLightning,
            BloodLightningBarrage,
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
        [SerializeField, TabGroup("Cinematic")]
        private BlackDeathCinematicPlayah m_cinematic;
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
        private RandomAttackDecider<Pattern> m_patternDecider;
        private Pattern m_currentPattern;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private ProjectileLauncher m_projectileLauncher;
        
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Collider2D m_randomSpawnCollider;

        private int m_currentPhaseIndex;
        private int m_attackCount;
        private int m_hitCount;
        private int[] m_patternAttackCount;
        private int m_currentBladeLoops;
        private int m_currentLightningCount;
        private int m_currentTentacleCount;
        private float m_currentLightningSummonDuration;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentPhaseIndex = obj.phaseIndex;
            for (int i = 0; i < m_patternAttackCount.Length; i++)
            {
                m_patternAttackCount[i] = obj.patternAttackCount[i];
            }
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
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
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

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
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
            transform.position = new Vector2(m_randomSpawnCollider.bounds.center.x, m_randomSpawnCollider.bounds.center.y -5);
            m_animation.SetAnimation(0, m_info.teleportAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportAppearAnimation);
            //m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true); //Temp
            //yield return new WaitForSeconds(5f);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true); //Temp
            //Debug.Log("Stop PHase Change");
            switch (m_currentPhaseIndex)
            {
                case 2:
                    m_currentLightningCount = 3;
                    m_currentLightningSummonDuration = 2;
                    break;
                case 3:
                    m_currentLightningCount = 4;
                    m_currentLightningSummonDuration = 2;
                    break;
            }
            StartCoroutine(m_phaseHandle.currentPhase == Phase.PhaseThree ? BloodLightningBarragePhase3Routine(m_currentLightningCount) : BloodLightningBarrageRoutine(m_currentLightningCount));
            yield return null;
        }

        private Vector2 GroundPosition(Vector2 startPoint)
        {
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            return hit.point;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            if (m_clones.Count > 0)
            {
                if (m_clones[m_clones.Count - 1] != null && m_clones[m_clones.Count - 1].activeSelf)
                {
                    for (int i = 0; i < m_clones.Count + 1; i++)
                    {
                        Destroy(m_clones[i]);
                        m_clones.RemoveAt(i);
                    }
                }
            }
            StopAllCoroutines();
            m_movement.Stop();
        }

        #region Attacks
        private IEnumerator BladeThrowRoutine(int numberOfProjectiles, int rotations)
        {
            if (m_currentPattern == Pattern.AttackPattern5 && m_attackCount == 3 || m_currentPattern == Pattern.AttackPattern7 && m_attackCount == 3)
            {
                m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.attackDaggers.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackDaggers.animation);
                m_animation.SetAnimation(0, m_info.attackDaggersIdle.animation, true);
            }
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
            m_stateHandle.ApplyQueuedState();
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
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator GiantBladesRoutine()
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
        }

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
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator TentacleBladesRoutine(int tentacleCount)
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
        }

        //private IEnumerator TentacleSummonRoutine()
        //{
        //    yield return null;
        //}

        private IEnumerator TentacleUpRoutine()
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
        }

        private IEnumerator BloodLightningRoutine(int lightningCount/*, float lightningXPos*/)
        {
            m_animation.SetAnimation(0, m_info.bloodLightningAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bloodLightningAttack.animation);
            m_animation.SetAnimation(0, m_info.bloodLightningIdleAnimation, true);
            for (int z = 0; z < lightningCount; z++)
            {
                if (m_currentPhaseIndex == 4 && m_currentPattern != Pattern.AttackPattern3)
                {
                    GameObject instance = Instantiate(m_info.bloodLightning, new Vector2(RandomTeleportPoint().x, GroundPosition(m_randomSpawnCollider.bounds.center).y), Quaternion.identity);
                    instance.transform.position = new Vector2(instance.transform.position.x, GroundPosition(instance.transform.position).y);
                }
                else
                {

                    GameObject instance = Instantiate(m_info.bloodLightning, new Vector2(m_targetInfo.position.x, GroundPosition(m_randomSpawnCollider.bounds.center).y), Quaternion.identity);
                    instance.transform.position = new Vector2(instance.transform.position.x, GroundPosition(instance.transform.position).y);
                }
                yield return new WaitForSeconds(m_currentLightningSummonDuration);
            }
            m_animation.SetAnimation(0, m_info.bloodLightningEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bloodLightningEndAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

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
            m_stateHandle.ApplyQueuedState();
            yield return null;
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
                var startPoint =  m_randomSpawnCollider.bounds.center.x + 30;

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
                if (m_cinematic != null)
                {
                    if (y == lightningCount - 1)
                    {
                        m_cinematic.PlayCinematic(2, true);
                    }
                    else
                    {
                        m_cinematic.PlayCinematic(1, true);
                    }
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
        private IEnumerator TeleportToTargetRoutine(Vector2 target, Attack attack, Vector2 positionOffset/*, bool isGrounded*/)
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
            if (attack == Attack.WaitAttackEnd)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                ExecuteAttack(attack);
            }
            yield return null;
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
            m_patternDecider.hasDecidedOnAttack = condition;
            m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
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
                                  , new AttackInfo<Attack>(Attack.TentacleBlades, m_info.attack6A.range));
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_patternDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChoosePattern()
        {
            if (!m_patternDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_patternDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentPattern && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentPattern = m_attackCache[i];
                        return;
                    }
                }
            }
        }

        private void ExecuteAttack(Attack m_attack)
        {
            var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
            switch (m_attack)
            {
                case Attack.BladeThrow:
                    StartCoroutine(BladeThrowRoutine(8, m_currentBladeLoops));
                    break;
                case Attack.BladeCircleThrow:
                    StartCoroutine(BladeThrow2Routine(32, m_currentBladeLoops));
                    break;
                case Attack.GiantBlades:
                    StartCoroutine(GiantBladesRoutine());
                    break;
                case Attack.ScytheSlash:
                    StartCoroutine(ScytheSlashRoutine());
                    break;
                case Attack.TeleportScytheSlash:
                    StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.ScytheSlash, new Vector2(randomFacing * 20, -5)/*, true*/));
                    break;
                case Attack.ShadowClone:
                    int cloneCount = m_currentPhaseIndex == 2 ? 1 : 2;
                    for (int i = 0; i < cloneCount; i++)
                    {
                        var clonePos = m_randomSpawnCollider.bounds.center + new Vector3(75 * randomFacing, 0, 0);
                        var clone = Instantiate(m_info.clone, clonePos, Quaternion.identity);
                        clone.GetComponent<BlackDeathCloneAI>().SetFacing(-randomFacing);
                        randomFacing = -randomFacing;
                        m_clones.Add(clone);
                    }
                    break;
                case Attack.TentacleBlades:
                    StartCoroutine(TentacleBladesRoutine(m_currentTentacleCount));
                    break;
                case Attack.TentacleAttackUp:
                    StartCoroutine(TentacleUpRoutine());
                    break;
                case Attack.TentacleAttackFront:
                    StartCoroutine(TentacleFrontRoutine());
                    break;
                case Attack.BloodLightning:
                    StartCoroutine(BloodLightningRoutine(m_currentLightningCount/*, m_currentLightningXPos*/));
                    break;
                case Attack.BloodLightningBarrage:
                    StartCoroutine(BloodLightningBarrageRoutine(m_currentLightningCount));
                    break;
            }
        }

        private void AddHitCount(object sender, EventActionArgs eventArgs)
        {
            m_hitCount++;
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

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_hitDetector.PlayerHit += AddHitCount;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoint);
            m_patternAttackCount = new int[2];
            m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            //for (int i = 0; i < 3; i++)
            //{
            //    m_attackDecider[i] = new RandomAttackDecider<Attack>();
            //}
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            //m_patternCount = new float[4];
            m_attackCache = new List<Pattern>();
            m_clones = new List<GameObject>();
            m_clones.Add(null);
            AddToAttackCache(Pattern.AttackPattern1, Pattern.AttackPattern2, Pattern.AttackPattern3, Pattern.AttackPattern4, Pattern.AttackPattern5, Pattern.AttackPattern6, Pattern.AttackPattern7);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Attacking);
                    var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                    var randomAttack = UnityEngine.Random.Range(0, 2);
                    var randomGroundPos = new Vector2(RandomTeleportPoint().x, GroundPosition(m_projectilePoint.position).y);
                    Debug.Log("Ground Sensor Detecting is " + m_groundSensor.isDetecting);
                    switch (m_currentPattern)
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
                                            StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.WaitAttackEnd, new Vector2(20, -5)/*, true*/));
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
                                if (m_attackCount >= m_patternAttackCount[0] /*&& m_hitCount == 0*/)
                                {
                                    if (m_attackCount == m_patternAttackCount[0] + 1)
                                    {
                                        m_stateHandle.Wait(State.Chasing);
                                        StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)/*, true*/));
                                                        break;
                                                    case Attack.TentacleAttackUp:
                                                        m_attackCount++;
                                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackUp, new Vector2(50, -5)/*, true*/));
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
                                                StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)/*, false*/));
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
                                                StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
                                                break;
                                            case 2:
                                                m_attackCount = m_patternAttackCount[0] + 1;
                                                m_currentBladeLoops = 2;
                                                StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)/*, false*/));
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
                                                StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.TeleportScytheSlash, Vector2.zero/*, false*/));
                                                break;
                                            case 4:
                                                if (m_attackCount <= 3)
                                                {
                                                    StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                    StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.WaitAttackEnd, new Vector2(20, -5)/*, true*/));
                                }
                                else
                                {
                                    m_stateHandle.ApplyQueuedState();
                                }
                            }
                            else
                            {
                                if (m_attackCount >= m_patternAttackCount[1] /*&& m_hitCount == 0*/)
                                {
                                    if (m_attackCount == m_patternAttackCount[1] + 1)
                                    {
                                        m_stateHandle.Wait(State.Chasing);
                                        StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, randomAttack == 1 ? Attack.ScytheSlash : Attack.TentacleAttackUp, randomAttack == 1 ? new Vector2(25 * randomFacing, -5) : new Vector2(50, -5)/*, true*/));
                                                        break;
                                                    case Attack.TentacleAttackFront:
                                                        m_attackCount++;
                                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)/*, true*/));
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
                                                StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
                                                break;
                                            case 2:
                                                m_attackCount = m_patternAttackCount[1] + 1;
                                                m_hitCount = 0;
                                                m_currentTentacleCount = 3;
                                                StartCoroutine(TeleportToTargetRoutine(randomAttack == 1 ? randomGroundPos : m_targetInfo.position, randomAttack == 1 ? Attack.TentacleBlades : Attack.TentacleAttackFront, randomAttack == 1 ? Vector2.zero : new Vector2(randomFacing * 20, -5)/*, randomAttack == 1 ? false : true*/));
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
                                                StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.TeleportScytheSlash, Vector2.zero/*, false*/));
                                                break;
                                            case 4:
                                                if (m_attackCount <= 3)
                                                {
                                                    StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                        StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.WaitAttackEnd, new Vector2(0, -10)/*, false*/));
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
                                        StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
                                    }
                                    break;
                                default:
                                    m_stateHandle.Wait(State.Chasing);
                                    m_currentLightningCount = 3;
                                    m_currentLightningSummonDuration = 2;
                                    StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.BloodLightning, new Vector2(0, -10)/*, false*/));
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
                                            StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackUp, new Vector2(50, -5)/*, true*/));
                                            break;
                                        case 2:
                                            StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25, -5)/*, true*/));
                                            break;
                                        case 3:
                                            m_stateHandle.Wait(State.Chasing);
                                            m_currentTentacleCount = 3;
                                            StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, GroundPosition(m_projectilePoint.position).y), Attack.TentacleBlades, Vector2.zero/*, false*/));
                                            break;
                                    }
                                    break;
                                case 4:
                                    switch (m_attackCount)
                                    {
                                        case 1:
                                            StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.WaitAttackEnd, new Vector2(0, -10)/*, false*/));
                                            break;
                                        case 2:
                                            m_currentLightningCount = 1;
                                            m_currentLightningSummonDuration = 1;
                                            ExecuteAttack(Attack.BloodLightningBarrage);
                                            break;
                                        case 3:
                                            m_stateHandle.Wait(State.Chasing);
                                            StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
                                            break;
                                    }
                                    break;
                                default:
                                    m_stateHandle.Wait(State.Chasing);
                                    m_currentLightningCount = 4;
                                    m_currentLightningSummonDuration = 1.5f;
                                    StartCoroutine(TeleportToTargetRoutine(new Vector2(RandomTeleportPoint().x, m_randomSpawnCollider.bounds.center.y), Attack.BloodLightningBarrage, new Vector2(0, -10)/*, false*/));
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
                                        StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)/*, false*/));
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
                                                        StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
                                                        break;
                                                }
                                                break;
                                            default:
                                                m_stateHandle.Wait(State.Chasing);
                                                StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackUp, new Vector2(50, -5)/*, true*/));
                                        break;
                                    case 2:
                                        m_currentTentacleCount = 1;
                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, randomAttack == 1 ? Attack.TentacleBlades : Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)/*, true*/));
                                        break;
                                    case 3:
                                        m_currentTentacleCount = 1;
                                        StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, Attack.TentacleAttackFront, new Vector2(25 * randomFacing, -5)/*, true*/));
                                        break;
                                    case 4:
                                        m_stateHandle.Wait(State.Chasing);
                                        StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                                StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center, Attack.BladeThrow, new Vector2(0, -20)/*, false*/));
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
                                                    StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
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
                                            StartCoroutine(TeleportToTargetRoutine(RandomTeleportPoint(), Attack.WaitAttackEnd, Vector2.zero/*, false*/));
                                            break;
                                    }
                                    break;
                            }
                            //m_stateHandle.OverrideState(State.Chasing);
                            break;
                    }
                    break;

                case State.Chasing:
                    DecidedOnAttack(false);
                    ChoosePattern();
                    if (IsFacingTarget())
                    {
                        if (m_patternDecider.hasDecidedOnAttack)
                        {
                            m_hitCount = 0;
                            m_attackCount = 0;
                            m_stateHandle.SetState(State.Attacking);
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
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

        protected override void OnTargetDisappeared()
        {
            //m_stickToGround = false;
            //m_currentCD = 0;
        }

        protected override void OnBecomePassive()
        {

        }
    }
}