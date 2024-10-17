using DChild.Gameplay.Characters.Players.Behaviour;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SlashCombo : AttackBehaviour
    {
        [SerializeField, HideLabel]
        private SlashComboStatsInfo m_configuration;

        [SerializeField]
        private SkeletonAnimation m_attackFX;
        //[SerializeField]
        //private int m_slashStateAmount;
        //[SerializeField]
        //private float m_slashComboCooldown;
        //[SerializeField]
        //private float m_slashMovementCooldown;
        [SerializeField]
        private List<Info> m_slashComboInfo;

        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        //[SerializeField, BoxGroup("Physics")]
        //private Rigidbody2D m_physics;
        //[SerializeField, BoxGroup("Physics")]
        //private List<Vector2> m_pushForce;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_enemySensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_wallSensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_edgeSensor;

        private bool m_canSlashCombo;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_currentSlashState;
        private int m_currentVisualSlashState;
        private float m_comboAttackDelayTimer;
        private float m_comboResetDelayTimer;
        private float m_slashComboCooldownTimer;
        private float m_slashMovementCooldownTimer;
        private bool m_allowAttackDelayHandling;
        private int m_slashStateAnimationParameter;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public event EventAction<EventActionArgs> OnSlash;

        public bool CanSlashCombo() => m_canSlashCombo;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_slashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SlashState);
            m_currentSlashState = -1;
            m_currentVisualSlashState = 0;
            m_comboAttackDelayTimer = -1;
            m_comboResetDelayTimer = -1;
            m_slashComboCooldownTimer = /*m_slashComboCooldown*/m_configuration.slashComboCooldown;
            m_allowAttackDelayHandling = true;
            m_canSlashCombo = true;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();

            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
        }

        public void SetConfiguration(SlashComboStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public override void Reset()
        {

            for (int i = 0; i < m_slashComboInfo.Count; i++)
            {
                m_slashComboInfo[i].ShowCollider(false);
            }
            m_currentSlashState = -1;
            m_currentVisualSlashState = 0;
            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
            base.Reset();
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_currentSlashState += m_currentSlashState >= m_configuration.slashStateAmount - 1 ? 0 : 1;
            m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
            m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_currentVisualSlashState = m_currentSlashState;

            m_comboResetDelayTimer = m_slashComboInfo[m_currentSlashState].nextAttackDelay;
            m_slashMovementCooldownTimer = /*m_slashMovementCooldown*/m_configuration.slashMovementCooldown;
            OnSlash?.Invoke(this, EventActionArgs.Empty);
        }

        public override void Cancel()
        {

            m_state.isDoingCombo = false;
            m_fxAnimator.Play("Buffer");

            for (int i = 0; i < m_slashComboInfo.Count; i++)
            {
                m_slashComboInfo[i].ShowCollider(false);
            }

            m_canSlashCombo = false;
            base.Cancel();
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

            //TEST
            //m_enemySensor.Cast();
            //m_wallSensor.Cast();
            //m_edgeSensor.Cast();
            //if (!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting)
            //{
            //    m_physics.AddForce(m_character.facing == HorizontalDirection.Right ? m_configuration.pushForce[m_currentVisualSlashState] : -m_configuration.pushForce[m_currentVisualSlashState], ForceMode2D.Impulse);
            //}
        }

        public override void AttackOver()
        {
            m_state.canAttack = true;

            for (int i = 0; i < m_slashComboInfo.Count; i++)
            {
                m_slashComboInfo[i].ShowCollider(false);
            }

            if (m_currentSlashState >= m_configuration.slashStateAmount)
            {
                m_currentSlashState = -1;
                m_canSlashCombo = false;
                m_canMove = false;
            }

            m_fxAnimator.Play("Buffer");
            base.AttackOver();
        }

        public void ComboEnd()
        {
            if (m_state.isAttacking == true)
            {

            }
            else
            {
                //Debug.Log("Attack Over");
                //m_skeletonAnimation.state.SetEmptyAnimation(0, 0);
                m_canSlashCombo = false;
                m_currentSlashState = -1;
                m_currentVisualSlashState = 0;
                m_animator.SetInteger(m_slashStateAnimationParameter, m_currentSlashState);
                //base.AttackOver();
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
                    m_comboAttackDelayTimer = 1;
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
                m_slashComboCooldownTimer = m_configuration.slashComboCooldown;
                m_canSlashCombo = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_slashMovementCooldownTimer > 0)
            {
                m_slashMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_slashMovementCooldownTimer = m_configuration.slashMovementCooldown;
                m_canMove = true;
            }
        }
    }
}