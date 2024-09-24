using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SwordThrust : AttackBehaviour, IChargeAttackBehaviour
    {
        [SerializeField, HideLabel]
        private SwordThrustStatsInfo m_configuration;
        [SerializeField]
        private ParticleSystem m_chargeFX;
        [SerializeField]
        private ParticleSystem m_finishedChargeFX;
        [SerializeField]
        private Info m_thrust;


        private float m_cooldownTimer;
        private float m_durationTimer;
        private Character m_character;
        private IPlayerModifer m_modifier;
        private float m_chargeTimer;
        private int m_swordThrustAnimationParameter;
        private int m_chargingAnimationParameter;

        public event EventAction<EventActionArgs> OnThrust;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_character = info.character;
            m_modifier = info.modifier;
            m_swordThrustAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SwordTrust);
            m_chargingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCharging);
            m_durationTimer = m_configuration.duration;
        }

        public void SetConfiguration(SwordThrustStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void ResetCooldownTimer() => m_cooldownTimer = m_configuration.cooldown * m_modifier.Get(PlayerModifier.Cooldown_Dash);

        public void HandleDurationTimer() => m_durationTimer -= GameplaySystem.time.deltaTime;

        public void ResetDurationTimer() => m_durationTimer = m_configuration.duration;

        public bool IsSwordThrustDurationOver() => m_durationTimer <= 0;

        public void StartCharge()
        {
            m_chargeTimer = m_configuration.chargeDuration;
            m_chargeFX?.Play(true);
            m_state.isAttacking = true;
            m_state.isChargingAttack = true;
            m_animator.SetBool(m_swordThrustAnimationParameter, true);
            m_animator.SetBool(m_chargingAnimationParameter, true);
            m_attacker.SetDamageModifier(m_thrust.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void HandleCharge()
        {
            if (m_chargeTimer > 0)
            {
                m_chargeTimer -= GameplaySystem.time.deltaTime;
            }
            else
            {
                m_chargeFX?.Stop(true);
                m_finishedChargeFX?.Play(true);
            }
        }

        public override void Cancel()
        {
            if (m_state.isAttacking)
            {
                m_animator.SetBool(m_animationParameter, false);
                m_state.isAttacking = false;
                m_state.canAttack = true;
            }

            m_rigidBody.velocity = Vector2.zero;
            m_thrust.ShowCollider(false);
            m_state.isChargingAttack = false;
            m_chargeFX?.Stop(true);
            m_thrust.PlayFX(false);
            m_finishedChargeFX?.Stop(true);
            m_animator.SetBool(m_swordThrustAnimationParameter, false);
            m_animator.SetBool(m_chargingAnimationParameter, false);

            ResetDurationTimer();
        }

        public bool IsChargeComplete() => m_chargeTimer <= 0;

        public void Execute()
        {
            if (m_state.isDoingSwordThrust == false)
            {
                m_state.isChargingAttack = false;
                m_state.isDoingSwordThrust = true;
                m_rigidBody.WakeUp();
                m_chargeFX?.Stop(true);
                m_finishedChargeFX?.Stop(true);
                m_thrust.PlayFX(true);
                m_thrust.ShowCollider(true);
                m_chargeTimer = -1;
                m_animator.SetBool(m_chargingAnimationParameter, false);
            }

            var direction = (float)m_character.facing;
            m_rigidBody.velocity = Vector2.zero;
            m_rigidBody.velocity = new Vector2(direction * m_configuration.thrustVelocity * m_modifier.Get(PlayerModifier.Dash_Distance), 0);

            OnThrust?.Invoke(this, EventActionArgs.Empty);
        }

        public void EndSwordThrust()
        {
            if (m_state.isAttacking)
            {
                m_animator.SetBool(m_animationParameter, false);
                m_state.isAttacking = false;
                m_state.canAttack = true;
                m_state.waitForBehaviour = true;
            }

            m_rigidBody.velocity = Vector2.zero;
            m_thrust.ShowCollider(false);
            m_state.isChargingAttack = false;
            m_chargeFX?.Stop(true);
            m_thrust.PlayFX(false);
            m_finishedChargeFX?.Stop(true);
            m_animator.SetBool(m_swordThrustAnimationParameter, false);
            m_animator.SetBool(m_chargingAnimationParameter, false);

            ResetDurationTimer();
        }

        public void EndExecution()
        {
            m_thrust.PlayFX(false);
            m_thrust.ShowCollider(false);
            m_state.isChargingAttack = false;
            m_state.waitForBehaviour = false;
            m_state.isDoingSwordThrust = false;
            m_animator.SetBool(m_swordThrustAnimationParameter, false);
        }

        public void Push()
        {
            var direction = (float)m_character.facing;
            m_rigidBody.velocity = Vector2.zero;
            m_rigidBody.velocity = new Vector2(direction * m_configuration.thrustVelocity * m_modifier.Get(PlayerModifier.Dash_Distance), 0);
        }
    }
}
