using DChild.Gameplay.Characters.Players.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SlashCombo : AttackBehaviour
    {
        [SerializeField]
        private int m_slashStateAmount;
        [SerializeField]
        private float m_comboResetDelay;
        [SerializeField]
        private List<Info> m_slashComboInfo;

        private IPlayerModifer m_modifier;
        private int m_currentSlashState;
        private int m_currentVisualSlashState;
        private float m_comboAttackDelayTimer;
        private float m_comboResetDelayTimer;
        private bool m_allowAttackDelayHandling;
        private int m_slashStateAnimationParameter;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_slashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SlashState);
            m_currentSlashState = 0;
            m_currentVisualSlashState = 0;
            m_comboAttackDelayTimer = -1;
            m_comboResetDelayTimer = -1;
            m_allowAttackDelayHandling = true;
        }

        public override void Reset()
        {
            base.Reset();

            m_currentSlashState = 0;
            m_currentVisualSlashState = 0;
            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_state.isDoingCombo = true;

            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);

            m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));

            m_comboResetDelayTimer = m_comboResetDelay;
            m_comboAttackDelayTimer = m_slashComboInfo[m_currentSlashState].nextAttackDelay;
            m_currentVisualSlashState = m_currentSlashState;
            m_currentSlashState++;

            if (m_currentSlashState >= m_slashStateAmount)
            {
                m_currentSlashState = 0;
            }
        }

        public override void Cancel()
        {
            base.Cancel();

            m_state.isDoingCombo = false;
        }

        public void PlayFX(bool value)
        {
            m_slashComboInfo[m_currentVisualSlashState].PlayFX(value);
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_slashComboInfo[m_currentVisualSlashState].ShowCollider(value);
        }

        public override void AttackOver()
        {
            m_state.isAttacking = false;
            m_state.waitForBehaviour = false;

            for (int i = 0; i < m_slashComboInfo.Count; i++)
            {
                m_slashComboInfo[i].ShowCollider(false);
            }
        }

        public void ComboAttackOver()
        {
            m_state.isDoingCombo = false;
            m_currentSlashState = 0;
            m_currentVisualSlashState = 0;
            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
            //Reset();
        }

        public void AllowNextAttackDelay()
        {
            m_allowAttackDelayHandling = true;
        }

        public void HandleComboAttackDelay()
        {
            if (m_comboAttackDelayTimer >= 0)
            {
                m_comboAttackDelayTimer -= GameplaySystem.time.deltaTime;

                if (m_comboAttackDelayTimer <= 0)
                {
                    m_comboAttackDelayTimer = -1;
                    m_state.canAttack = true;
                    m_allowAttackDelayHandling = false;
                }
            }
        }

        public void HandleComboResetTimer()
        {
            if(m_comboResetDelayTimer >= 0)
            {
                m_comboResetDelayTimer -= GameplaySystem.time.deltaTime;

                if(m_comboResetDelayTimer <= 0)
                {
                    Reset();
                }
            }
        }
    }
}