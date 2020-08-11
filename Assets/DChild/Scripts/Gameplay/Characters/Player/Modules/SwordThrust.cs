using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SwordThrust : AttackBehaviour, IChargeAttackBehaviour
    {
        [SerializeField]
        private ParticleSystem m_chargeFX;
        [SerializeField, MinValue(0.1f)]
        private float m_chargeDuration;
        [SerializeField]
        private Info m_thrust;

        private float m_chargeTimer;

        public void StartCharge()
        {
            m_chargeTimer = m_chargeDuration;
            m_chargeFX?.Play(true);
            m_state.isAttacking = true;
            m_state.isChargingAttack = true;
        }

        public void HandleCharge()
        {
            if (m_chargeTimer > 0)
            {
                m_chargeTimer -= GameplaySystem.time.deltaTime;
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            m_state.isChargingAttack = false;
            m_chargeFX?.Stop(true);
        }

        public bool IsChargeComplete() => m_chargeTimer <= 0;

        public void Execute()
        {
            m_chargeFX?.Stop(true);
            m_thrust.PlayFX(true);
            m_thrust.ShowCollider(true);
            m_chargeTimer = -1;
            m_state.waitForBehaviour = true;
        }

        public void EndExecution()
        {
            m_thrust.PlayFX(false);
            m_thrust.ShowCollider(false);
            m_state.waitForBehaviour = false;
        }
    }
}
