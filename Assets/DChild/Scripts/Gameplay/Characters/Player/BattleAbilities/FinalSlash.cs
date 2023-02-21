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
        private Info m_finalSlashInfo;
        //TEST
        [SerializeField]
        private CharacterState m_characterState;
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

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canFinalSlash;
        private bool m_canMove;
        private bool m_canDash;
        private IPlayerModifer m_modifier;
        private int m_finalSlashStateAnimationParameter;
        private int m_finalSlashDashAnimationParameter;
        private float m_finalSlashCooldownTimer;
        private float m_finalSlashMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanFinalSlash() => m_canFinalSlash;
        public bool CanMove() => m_canMove;
        private bool m_hasExecuted;

        private Coroutine m_finalSlashChargingRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_finalSlashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FinalSlash);
            m_finalSlashDashAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FinalSlashDash);
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
            StopAllCoroutines();
            m_finalSlashChargingRoutine = StartCoroutine(FinalSlashChargingRoutine());
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
            //m_finalSlashInfo.ShowCollider(false);
            //m_canFinalSlash = true;
            //m_canMove = true;
            m_canDash = false;
            StopAllCoroutines();
            if (m_finalSlashChargingRoutine != null)
            {
                StopCoroutine(m_finalSlashChargingRoutine);
                m_finalSlashChargingRoutine = null;
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
                m_fxAnimator.Play("Buffer");
                m_characterState.isChargingFinalSlash = false;
                StopAllCoroutines();
                if (m_finalSlashChargingRoutine != null)
                {
                    StopCoroutine(m_finalSlashChargingRoutine);
                    m_finalSlashChargingRoutine = null;
                }
                m_animator.SetBool(m_finalSlashStateAnimationParameter, false);
                base.Cancel();
            }
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_finalSlashInfo.ShowCollider(value);
            m_attackFX.transform.position = m_finalSlashInfo.fxPosition.position;
            if (!value)
                m_fxAnimator.Play("Buffer");
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
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_physics.velocity.y);
                timer -= Time.deltaTime;
                if (!m_edgeSensor.isDetecting || m_enemySensor.isDetecting)
                    timer = -1;
                yield return null;
            }
            m_canDash = false;
            //m_animator.SetBool(m_finalSlashStateAnimationParameter, false);
            m_fxAnimator.Play("SlashCombo2");
            //yield return new WaitForSeconds(m_dashDuration);
            m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            m_animator.SetBool(m_finalSlashDashAnimationParameter, false);
            //m_state.waitForBehaviour = false;
            yield return null;
        }

        private IEnumerator FinalSlashChargingRoutine()
        {
            while (true)
            {
                m_state.waitForBehaviour = false;
                yield return null;
            }
        }
    }
}
