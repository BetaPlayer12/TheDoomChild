using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class FinalSlash : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_finalSlashCooldown;
        [SerializeField]
        private float m_finalSlashMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private int m_finalSlashLevel;
        [SerializeField]
        private Info m_finalSlashInfo;
        //TEST
        [SerializeField]
        private CharacterState m_characterState;
        [SerializeField]
        private SpineRootAnimation m_animation;
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("FX")]
        private Animator m_finalSlashSwordGlowFXAnimator;
        [SerializeField, BoxGroup("FX")]
        private ParticleSystem m_finalSlashLevelFX;
        [SerializeField, BoxGroup("FX")]
        private ParticleSystem m_swordSmokeTrail_FinalSlashFX;
        [SerializeField, BoxGroup("FX")]
        private AutoStopParticle m_autoStopParticle;
        [SerializeField, BoxGroup("FX")]
        private Animator m_finalSlashDustChargeFXAnimator;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_finalSlashDustFeedbackFX;
        [SerializeField, BoxGroup("FX")]
        private Transform m_finalSlashDustFeedbackFXStartPoint;
        [SerializeField, BoxGroup("FX")]
        private Animator m_finalSlashHolderFXAnimator;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_finalSlashImpactFX;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canFinalSlash;
        private bool m_canMove;
        private bool m_canDash;
        private IPlayerModifer m_modifier;
        private int m_finalSlashStateAnimationParameter;
        private int m_finalSlashDashAnimationParameter;
        private int m_finalSlashLevelAnimationParameter;
        private float m_finalSlashCooldownTimer;
        private float m_finalSlashMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanFinalSlash() => m_canFinalSlash;
        public bool CanMove() => m_canMove;
        private bool m_hasExecuted;

        private Coroutine m_finalSlashChargingRoutine;
        private Coroutine m_finalSlashEnemeyCheckRoutine;
        private Coroutine m_finalSlashLevelChangeRoutine;

        public enum FinalSlashState
        {
            FinalSlash1,
            FinalSlash2,
            FinalSlash3,
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_finalSlashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FinalSlash);
            m_finalSlashDashAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FinalSlashDash);
            m_finalSlashLevelAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FinalSlashLevel);
            m_canFinalSlash = true;
            m_canMove = true;
            m_finalSlashMovementCooldownTimer = m_finalSlashMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            m_finalSlashInfo.ShowCollider(false);
            m_animator.SetBool(m_finalSlashStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_hasExecuted = true;
            m_autoStopParticle.ForceStop(0);
            StopAllCoroutines();
            m_finalSlashChargingRoutine = StartCoroutine(FinalSlashChargingRoutine());
            m_finalSlashLevelChangeRoutine = StartCoroutine(FinalSlashLevelChangeRoutine());
            m_state.waitForBehaviour = false;
            m_state.isAttacking = true;
            m_characterState.isChargingFinalSlash = true;
            //m_state.canAttack = false;
            m_canFinalSlash = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_finalSlashStateAnimationParameter, true);
            m_finalSlashCooldownTimer = m_finalSlashCooldown;
            m_finalSlashMovementCooldownTimer = m_finalSlashMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_autoStopParticle.CheckEffect();
        }

        public void ExecuteDash()
        {
            if (m_canDash)
            {
                StopAllCoroutines();
                if (m_finalSlashChargingRoutine != null)
                {
                    StopCoroutine(m_finalSlashChargingRoutine);
                    m_finalSlashChargingRoutine = null;
                }
                if (m_finalSlashLevelChangeRoutine != null)
                {
                    StopCoroutine(m_finalSlashLevelChangeRoutine);
                    m_finalSlashLevelChangeRoutine = null;
                }
                StartCoroutine(DashRoutine());
            }
            else
            {
                EndExecution();
            }
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            m_hasExecuted = false;
            m_state.waitForBehaviour = false;
            //m_state.canAttack = true;
            //m_state.isAttacking = false;
            m_characterState.isChargingFinalSlash = false;
            m_finalSlashInfo.ShowCollider(false);
            //m_canFinalSlash = true;
            //m_canMove = true;
            m_canDash = false;
            m_animation.DisableRootMotion();
            StopAllCoroutines();
            if (m_finalSlashChargingRoutine != null)
            {
                StopCoroutine(m_finalSlashChargingRoutine);
                m_finalSlashChargingRoutine = null;
            }
            if (m_finalSlashLevelChangeRoutine != null)
            {
                StopCoroutine(m_finalSlashLevelChangeRoutine);
                m_finalSlashLevelChangeRoutine = null;
            }
            if (m_finalSlashEnemeyCheckRoutine != null)
            {
                StopCoroutine(m_finalSlashEnemeyCheckRoutine);
                m_finalSlashEnemeyCheckRoutine = null;
            }
            m_animator.SetBool(m_finalSlashStateAnimationParameter, false);
            base.AttackOver();
        }

        public void EnableDash(bool value)
        {
            m_canDash = value;
        }

        public override void Cancel()
        {
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                m_finalSlashInfo.ShowCollider(false);
                //m_fxAnimator.Play("Buffer");
                m_characterState.isChargingFinalSlash = false;
                StopAllCoroutines();
                if (m_finalSlashChargingRoutine != null)
                {
                    StopCoroutine(m_finalSlashChargingRoutine);
                    m_finalSlashChargingRoutine = null;
                }
                if (m_finalSlashLevelChangeRoutine != null)
                {
                    StopCoroutine(m_finalSlashLevelChangeRoutine);
                    m_finalSlashLevelChangeRoutine = null;
                }
                if (m_finalSlashEnemeyCheckRoutine != null)
                {
                    StopCoroutine(m_finalSlashEnemeyCheckRoutine);
                    m_finalSlashEnemeyCheckRoutine = null;
                }
                m_animator.SetBool(m_finalSlashStateAnimationParameter, false);
                m_animation.DisableRootMotion();
                base.Cancel();
                SetSwordGlowFXAnimator(false);
                SetDustChargeFXAnimator(false);
            }
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_finalSlashInfo.ShowCollider(value);
            m_attackFX.transform.position = m_finalSlashInfo.fxPosition.position;
            //if (!value)
            //    m_fxAnimator.Play("Buffer");
        }

        public void HandleAttackTimer()
        {
            if (m_finalSlashCooldownTimer > 0)
            {
                m_finalSlashCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canFinalSlash = false;
            }
            else
            {
                m_finalSlashCooldownTimer = m_finalSlashCooldown;
                //m_state.isAttacking = false;
                m_canFinalSlash = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_finalSlashMovementCooldownTimer > 0)
            {
                m_finalSlashMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                m_finalSlashMovementCooldownTimer = m_finalSlashMovementCooldown;
                m_canMove = true;
            }
        }

        private IEnumerator DashRoutine()
        {
            m_state.waitForBehaviour = true;
            m_characterState.isChargingFinalSlash = false;
            var timer = m_dashDuration;
            m_animator.SetBool(m_finalSlashDashAnimationParameter, true);
            while (timer >= 0 /*&& !m_enemySensor.isDetecting*/ /*&& !m_wallSensor.allRaysDetecting*/)
            {
                //Debug.Log("Final Slash Dashing!@#");
                m_enemySensor.Cast();
                //m_wallSensor.Cast();
                m_edgeSensor.Cast();
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, -m_physics.velocity.y);
                timer -= Time.deltaTime;
                if (!m_edgeSensor.isDetecting || m_enemySensor.isDetecting)
                    timer = -1;
                yield return null;
            }
            m_canDash = false;
            //m_animator.SetBool(m_finalSlashStateAnimationParameter, false);
            //m_fxAnimator.Play("SlashCombo2");
            //yield return new WaitForSeconds(m_dashDuration);
            m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            m_animator.SetBool(m_finalSlashDashAnimationParameter, false);
            m_animation.EnableRootMotion(true, true);
            //m_state.waitForBehaviour = false;
            yield return null;
        }

        private IEnumerator FinalSlashChargingRoutine()
        {
            while (true)
            {
                m_state.waitForBehaviour = false;
                m_state.isAttacking = true;

                //switch (m_finalSlashLevel)
                //{
                //    case 0:
                //        m_animator.SetInteger(m_finalSlashLevel, 0);
                //        break;
                //    case 1:
                //        m_animator.SetInteger(m_finalSlashLevel, 1);
                //        break;
                //    case 2:
                //        m_animator.SetInteger(m_finalSlashLevel, 3);
                //        break;
                //}
                yield return null;
            }
        }

        private IEnumerator FinalSlashLevelChangeRoutine()
        {
            for (int i = 0; i < m_finalSlashLevel; i++)
            {
                m_animator.SetInteger(m_finalSlashLevelAnimationParameter, i);
                yield return new WaitForSeconds(1f);
                m_finalSlashLevelFX.Play();
            }
            yield return null;
        }

        #region FX Animators
        public void SetSwordGlowFXAnimator(bool state)
        {
            m_finalSlashSwordGlowFXAnimator.SetBool("isCharging", state);

            m_swordSmokeTrail_FinalSlashFX.transform.localRotation = Quaternion.Euler(0, 0, m_character.facing == HorizontalDirection.Right ? 180f : 0f);
            if (state)
                m_swordSmokeTrail_FinalSlashFX.Play();
            else
                m_swordSmokeTrail_FinalSlashFX.Stop();
        }

        public void SetDustChargeFXAnimator(bool state)
        {
            m_finalSlashDustChargeFXAnimator.SetBool("Dust_isCharging", state);
        }

        public void SpawnDustFeedbackFX()
        {
            var instance = /*GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_finalSlashDustFeedbackFX)*/Instantiate(m_finalSlashDustFeedbackFX);
            //instance.GetComponent<ParticleSystem>().Clear();
            instance.transform.position = m_finalSlashDustFeedbackFXStartPoint.position;
            instance.transform.localScale = new Vector3(m_character.facing == HorizontalDirection.Right ? instance.transform.localScale.x : -instance.transform.localScale.x, instance.transform.localScale.y, instance.transform.localScale.z);
            //m_finalSlashDustFeedbackFX.Play();
        }

        public void SetFinalSlashHolderFX(FinalSlashState state)
        {
            m_finalSlashEnemeyCheckRoutine = StartCoroutine(FinalSlashEnemyCheckRoutine());
            switch (state)
            {
                case FinalSlashState.FinalSlash1:
                    m_finalSlashHolderFXAnimator.SetTrigger("FinalSlash1");
                    break;
                case FinalSlashState.FinalSlash2:
                    m_finalSlashHolderFXAnimator.SetTrigger("FinalSlash2");
                    break;
                case FinalSlashState.FinalSlash3:
                    m_finalSlashHolderFXAnimator.SetTrigger("FinalSlash3");
                    break;
            }
        }

        private IEnumerator FinalSlashEnemyCheckRoutine()
        {
            bool hasSpawned = false;
            while (true)
            {
                m_enemySensor.Cast();
                if (m_enemySensor.isDetecting && !hasSpawned)
                {
                    var hits = m_enemySensor.GetHits();
                    //var targetTransform = hits[1].transform;
                    int hitID = 0;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (Vector2.Distance(m_character.centerMass.position, hits[i].transform.position) < 25f)
                        {
                            hitID = i;
                        }
                    }
                    var target = /*m_enemySensor.isDetecting ? hits[0].point : Vector2.zero*/hits[hitID].point;
                    //if (/*target != Vector2.zero &&*/ targetTransform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    //{
                    //    hasSpawned = true;
                    //    var instance = Instantiate(m_finalSlashImpactFX);
                    //    instance.transform.position = target;
                    //}
                    hasSpawned = true;
                    var instance = Instantiate(m_finalSlashImpactFX);
                    instance.transform.position = target;
                }
                yield return null;
            }
            //yield return null;
        }
        #endregion
    }
}
