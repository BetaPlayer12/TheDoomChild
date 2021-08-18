using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SwordThrust : AttackBehaviour, IChargeAttackBehaviour
    {
        [SerializeField]
        private ParticleSystem m_chargeFX;
        [SerializeField]
        private ParticleSystem m_finishedChargeFX;
        [SerializeField, MinValue(0.1f)]
        private float m_chargeDuration;
        [SerializeField]
        private Info m_thrust;
        [SerializeField]
        private float m_thrustForce;

        private Character m_character;
        private IPlayerModifer m_modifier;
        private float m_chargeTimer;
        private int m_swordThrustAnimationParameter;
        private int m_chargingAnimationParameter;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_character = info.character;
            m_modifier = info.modifier;
            m_swordThrustAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SwordTrust);
            m_chargingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCharging);
        }

        public void StartCharge()
        {
            m_chargeTimer = m_chargeDuration;
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
            base.Cancel();
            m_thrust.ShowCollider(false);
            m_state.isChargingAttack = false;
            m_chargeFX?.Stop(true);
            m_finishedChargeFX?.Stop(true);
            m_animator.SetBool(m_swordThrustAnimationParameter, false);
            m_animator.SetBool(m_chargingAnimationParameter, false);
        }

        public bool IsChargeComplete() => m_chargeTimer <= 0;

        public void Execute()
        {
            m_rigidBody.WakeUp(); //Players rigidbody and targets rigidbody is asleep by the time of execution. When concerned rigid bodies are asleep there are no interactions even if colliders are enable. Capeesh
            m_chargeFX?.Stop(true);
            m_finishedChargeFX?.Stop(true);
            //m_thrust.PlayFX(true);
            m_thrust.ShowCollider(true);
            m_chargeTimer = -1;
            m_state.waitForBehaviour = true;
            m_rigidBody.AddForce(new Vector2((float)m_character.facing * m_thrustForce * m_modifier.Get(PlayerModifier.Dash_Distance), 0), ForceMode2D.Impulse);
            m_animator.SetBool(m_chargingAnimationParameter, false);
        }

        public void EndExecution()
        {
            m_thrust.PlayFX(false);
            m_thrust.ShowCollider(false);
            m_state.isChargingAttack = false;
            m_state.waitForBehaviour = false;
            m_animator.SetBool(m_swordThrustAnimationParameter, false);
        }
    }
}
