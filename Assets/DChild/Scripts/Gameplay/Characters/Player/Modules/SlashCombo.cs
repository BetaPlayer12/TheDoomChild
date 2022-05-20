using DChild.Gameplay.Characters.Players.Behaviour;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SlashCombo : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;
        [SerializeField]
        private int m_slashStateAmount;
        [SerializeField]
        private float m_slashComboCooldown;
        [SerializeField]
        private List<Info> m_slashComboInfo;

        private bool m_canSlashCombo;
        private IPlayerModifer m_modifier;
        private int m_currentSlashState;
        private int m_currentVisualSlashState;
        private float m_comboAttackDelayTimer;
        private float m_comboResetDelayTimer;
        private float m_slashComboCooldownTimer;
        private bool m_allowAttackDelayHandling;
        private int m_slashStateAnimationParameter;

        private Animator m_fxAnimator;

        public bool CanSlashCombo() => m_canSlashCombo;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_slashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SlashState);
            m_currentSlashState = 0;
            m_currentVisualSlashState = 0;
            m_comboAttackDelayTimer = -1;
            m_comboResetDelayTimer = -1;
            m_slashComboCooldownTimer = m_slashComboCooldown;
            m_allowAttackDelayHandling = true;
            m_canSlashCombo = true;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
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
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
            m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_currentVisualSlashState = m_currentSlashState;
            m_currentSlashState++;
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
            m_attackFX.transform.position = m_slashComboInfo[m_currentVisualSlashState].fxPosition.position;

            switch (m_currentVisualSlashState)
            {
                case 0:
                    m_fxAnimator.Play("SlashCombo1");
                    break;
                case 1:
                    m_fxAnimator.Play("SlashCombo2");
                    break;
                case 2:
                    m_fxAnimator.Play("SlashCombo3");
                    break;
                default:
                    break;
            }
        }

        public override void AttackOver()
        {
            base.AttackOver();
            m_state.canAttack = true;

            for (int i = 0; i < m_slashComboInfo.Count; i++)
            {
                m_slashComboInfo[i].ShowCollider(false);
            }

            if (m_currentSlashState >= m_slashStateAmount)
            {
                m_currentSlashState = 0;
                m_canSlashCombo = false;
            }

            m_fxAnimator.Play("Buffer");
        }

        public void ComboEnd()
        {
            if (m_state.isAttacking == true)
            {

            }
            else
            {
                Debug.Log("Attack Over");
                base.AttackOver();
                m_state.canAttack = true;

                m_canSlashCombo = false;
                m_currentSlashState = 0;
                m_currentVisualSlashState = 0;
                m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
            }
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
            if (m_comboResetDelayTimer >= 0)
            {
                m_comboResetDelayTimer -= GameplaySystem.time.deltaTime;

                if (m_comboResetDelayTimer <= 0)
                {
                    Reset();
                }
            }
        }

        public void HandleSlashComboTimer()
        {
            if (m_slashComboCooldownTimer > 0)
            {
                m_slashComboCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canSlashCombo = false;
            }
            else
            {
                m_slashComboCooldownTimer = m_slashComboCooldown;
                m_canSlashCombo = true;
            }
        }
    }
}