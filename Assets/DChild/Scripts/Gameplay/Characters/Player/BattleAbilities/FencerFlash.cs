using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class FencerFlash : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_fencerFlashCooldown;
        [SerializeField]
        private float m_fencerFlashMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private Info m_fencerFlashInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_fxParent;
        [SerializeField, BoxGroup("FX")]
        private SpineFX m_fx;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canFencerFlash;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_fencerFlashStateAnimationParameter;
        private float m_fencerFlashCooldownTimer;
        private float m_fencerFlashMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanFencerFlash() => m_canFencerFlash;
        public bool CanMove() => m_canMove;

        private Coroutine m_enemySensorRoutine;

        private FencerFlashState m_currentState;

        public enum FencerFlashState
        {
            Grounded,
            Midair,
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_fencerFlashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FencerFlash);
            m_canFencerFlash = true;
            m_canMove = true;
            m_fencerFlashMovementCooldownTimer = m_fencerFlashMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_cacheGravity = m_physics.gravityScale;
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            m_fencerFlashInfo.ShowCollider(false);
            m_animator.SetBool(m_fencerFlashStateAnimationParameter, false);
        }

        public void Execute(FencerFlashState state)
        {
            m_state.waitForBehaviour = true;
            m_currentState = state;
            StopAllCoroutines();
            m_enemySensorRoutine = StartCoroutine(EnemySensorRoutine());
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canFencerFlash = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_fencerFlashStateAnimationParameter, true);
            m_fencerFlashCooldownTimer = m_fencerFlashCooldown;
            m_fencerFlashMovementCooldownTimer = m_fencerFlashMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));

            m_physics.velocity = Vector2.zero;
            if (m_currentState == FencerFlashState.Midair)
            {
                m_cacheGravity = m_physics.gravityScale;
                m_physics.gravityScale = 0;
            }
        }

        public void EndExecution()
        {
            m_fencerFlashInfo.ShowCollider(false);
            //m_canfencerFlash = true;
            m_canMove = true;
            m_animator.SetBool(m_fencerFlashStateAnimationParameter, false);
            //m_fencerFlashAnimation.gameObject.SetActive(false);
            m_physics.gravityScale = m_cacheGravity;
            StopAllCoroutines();
            if (m_enemySensorRoutine != null)
            {
                StopCoroutine(m_enemySensorRoutine);
                m_enemySensorRoutine = null;
            }
            if (m_fxParent.activeSelf)
            {
                m_fx.Stop();
                m_fxParent.SetActive(false);
            }
            base.AttackOver();
        }

        public override void Cancel()
        {
            m_fencerFlashInfo.ShowCollider(false);
            m_canMove = true;
            m_fxAnimator.Play("Buffer");
            StopAllCoroutines();
            if (m_enemySensorRoutine != null)
            {
                StopCoroutine(m_enemySensorRoutine);
                m_enemySensorRoutine = null;
            }
            m_animator.SetBool(m_fencerFlashStateAnimationParameter, false);
            //m_fencerFlashAnimation.gameObject.SetActive(false);
            m_physics.gravityScale = m_cacheGravity;
            if (m_fxParent.activeSelf)
            {
                m_fx.Stop();
                m_fxParent.SetActive(false);
            }
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_fencerFlashInfo.ShowCollider(value);
            m_attackFX.transform.position = m_fencerFlashInfo.fxPosition.position;
        }

        public void StartDash()
        {
            StartCoroutine(DashRoutine());
        }

        public void HandleAttackTimer()
        {
            if (m_fencerFlashCooldownTimer > 0)
            {
                m_fencerFlashCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canFencerFlash = false;
            }
            else
            {
                m_fencerFlashCooldownTimer = m_fencerFlashCooldown;
                m_state.isAttacking = false;
                m_canFencerFlash = true;
            }
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_fencerFlashMovementCooldownTimer > 0)
        //    {
        //        m_fencerFlashMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_fencerFlashMovementCooldownTimer = m_fencerFlashMovementCooldown;
        //        m_canMove = true;
        //    }
        //}

        private IEnumerator EnemySensorRoutine()
        {
            while (true)
            {
                m_enemySensor.Cast();
                yield return null;
            }
        }

        private IEnumerator DashRoutine()
        {
            m_state.waitForBehaviour = true;
            var timer = m_dashDuration;
            while (timer >= 0 /*&& !m_wallSensor.allRaysDetecting && !m_enemySensor.isDetecting*/&& !m_enemySensor.isDetecting)
            {
                m_wallSensor.Cast();
                //m_enemySensor.Cast();
                m_edgeSensor.Cast();
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, 0);
                timer -= Time.deltaTime;
                if (m_currentState == FencerFlashState.Grounded)
                {
                    if (!m_edgeSensor.isDetecting || m_enemySensor.isDetecting)
                    {
                        timer = -1;
                    }
                }
                yield return null;
            }
            m_fxParent.SetActive(true);
            m_fx.Play();
            //yield return new WaitForSeconds(m_dashDuration);
            m_physics.velocity = Vector2.zero;
            //m_fencerFlashAnimation.gameObject.SetActive(false);
            //m_state.waitForBehaviour = false;
            //m_enemySensor.Cast();
            if (!m_enemySensor.isDetecting)
            {
                m_fencerFlashInfo.ShowCollider(false);
                m_animator.SetBool(m_fencerFlashStateAnimationParameter, false);
                //m_physics.gravityScale = m_cacheGravity;
                if (m_fxParent.activeSelf)
                {
                    m_fx.Stop();
                    m_fxParent.SetActive(false);
                }
            }
            if (m_enemySensorRoutine != null)
            {
                StopCoroutine(m_enemySensorRoutine);
                m_enemySensorRoutine = null;
            }
            yield return null;
        }
    }
}
