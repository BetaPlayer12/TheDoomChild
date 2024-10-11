using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WhipAttackCombo : AttackBehaviour
    {
        [SerializeField, HideLabel]
        private WhipAttackComboStatsInfo m_configuration;

        [SerializeField]
        private SkeletonAnimation m_attackFX;
        //[SerializeField]
        //private int m_whipStateAmount;
        //[SerializeField]
        //private float m_whipComboCooldown;
        //[SerializeField]
        //private float m_whipMovementCooldown;
        [SerializeField]
        private List<Info> m_whipComboInfo;

        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        //[SerializeField, BoxGroup("Physics")]
        //private List<Vector2> m_pushForce;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("FX")]
        private Animator m_whipFXAnimator;

        private bool m_canWhipCombo;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_currentWhipState;
        private int m_currentVisualWhipState;
        private float m_comboAttackDelayTimer;
        private float m_comboResetDelayTimer;
        private float m_whipComboCooldownTimer;
        private float m_whipMovementCooldownTimer;
        private bool m_allowAttackDelayHandling;
        private int m_whipAttackAnimationParameter;
        private int m_whipStateAnimationParameter;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanWhipCombo() => m_canWhipCombo;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_whipAttackAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipAttack);
            m_whipStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipState);
            m_currentWhipState = -1;
            m_currentVisualWhipState = 0;
            m_comboAttackDelayTimer = -1;
            m_comboResetDelayTimer = -1;
            m_whipComboCooldownTimer = /*m_whipComboCooldown*/m_configuration.whipComboCooldown;
            m_allowAttackDelayHandling = true;
            m_canWhipCombo = true;
            m_cacheGravity = m_physics.gravityScale;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        public void SetConfiguration(WhipAttackComboStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public override void Reset()
        {
            for (int i = 0; i < m_whipComboInfo.Count; i++)
            {
                m_whipComboInfo[i].ShowCollider(false);
            }

            //Debug.Log("Whip Combo State Reset");
            m_currentWhipState = -1;
            m_currentVisualWhipState = 0;
            m_animator.SetInteger(m_whipStateAnimationParameter, m_currentWhipState);
            base.Reset();
        }

        public void Execute()
        {
            //Debug.Log("Clicked Whip Combo Attack");
            m_currentWhipState += m_currentWhipState >= m_configuration.whipStateAmount - 1 ? 0 : 1;
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_whipAttackAnimationParameter, true);
            m_animator.SetInteger(m_whipStateAnimationParameter, m_currentWhipState);
            m_attacker.SetDamageModifier(m_whipComboInfo[m_currentWhipState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_currentVisualWhipState = m_currentWhipState;

            m_comboResetDelayTimer = m_whipComboInfo[m_currentWhipState].nextAttackDelay;
            m_whipMovementCooldownTimer = /*m_whipMovementCooldown*/m_configuration.whipMovementCooldown;

            switch (m_currentVisualWhipState)
            {
                case 0:
                    m_whipFXAnimator.SetTrigger("WhipCombo1");
                    break;
                case 1:
                    m_whipFXAnimator.SetTrigger("WhipCombo2");
                    break;
                case 2:
                    m_whipFXAnimator.SetTrigger("WhipCombo1");
                    break;
            }
        }

        public override void Cancel()
        {
            for (int i = 0; i < m_whipComboInfo.Count; i++)
            {
                m_whipComboInfo[i].ShowCollider(false);
            }

            if (m_state.isAttacking)
                m_animator.SetBool(m_whipAttackAnimationParameter, false);

            m_state.isDoingCombo = false;
            m_fxAnimator.Play("Buffer");
            base.Cancel();
        }

        public void PlayFX(bool value)
        {
            m_whipComboInfo[m_currentVisualWhipState].PlayFX(value);
        }

        public void EnableCollision(bool value)
        {
            //Debug.Log("Whip Combo Collision");
            m_rigidBody.WakeUp();
            m_whipComboInfo[m_currentVisualWhipState].ShowCollider(value);
            m_attackFX.transform.position = m_whipComboInfo[m_currentVisualWhipState].fxPosition.position;

            //TEST
            if (CanAttack())
            {
                m_physics.AddForce(m_character.facing == HorizontalDirection.Right ? m_configuration.pushForce[m_currentVisualWhipState] : -m_configuration.pushForce[m_currentVisualWhipState], ForceMode2D.Impulse);
            }
        }

        public bool CanAttack()
        {
            m_enemySensor.Cast();
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            return !m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting;
        }

        public void ResetGravity()
        {
            m_physics.gravityScale = m_cacheGravity;
        }

        public override void AttackOver()
        {
            base.AttackOver();
            m_state.canAttack = true;
            m_canWhipCombo = false;
            m_canMove = false;
            m_animator.SetBool(m_whipAttackAnimationParameter, false);

            if (m_currentWhipState >= /*m_whipStateAmount*/m_configuration.whipStateAmount - 1)
            {
                m_currentWhipState = -1;
                m_canWhipCombo = false;
                m_canMove = false;

                m_animator.SetInteger(m_whipStateAnimationParameter, m_currentWhipState);
            }

            //Debug.Log("Whip Attack Over");
            for (int i = 0; i < m_whipComboInfo.Count; i++)
            {
                m_whipComboInfo[i].ShowCollider(false);
            }

            //m_currentWhipState = 0;
            //m_currentVisualWhipState = 0;

            m_fxAnimator.Play("Buffer");
            //base.AttackOver();
            //m_state.canAttack = true;
            //m_animator.SetBool(m_whipAttackAnimationParameter, false);

            //Debug.Log("Whip Attack Over");
            //for (int i = 0; i < m_whipComboInfo.Count; i++)
            //{
            //    m_whipComboInfo[i].ShowCollider(false);
            //}

            //if (m_currentWhipState >= m_whipStateAmount)
            //{
            //    m_currentWhipState = 0;
            //    m_canWhipCombo = false;
            //}

            //m_fxAnimator.Play("Buffer");
        }

        public void ComboEnd()
        {
            //Debug.Log("Whip Combo End");
            //base.AttackOver();
            m_state.canAttack = true;
            m_canWhipCombo = true;
            //m_skeletonAnimation.state.SetEmptyAnimation(0, 0);
            m_animator.SetBool(m_whipAttackAnimationParameter, false);
            //if (m_state.isAttacking == true)
            //{

            //}
            //else
            //{
            //    Debug.Log("Whip Combo End");
            //    //base.AttackOver();
            //    m_state.canAttack = true;
            //    //m_skeletonAnimation.state.SetEmptyAnimation(0, 0);
            //    m_canWhipCombo = false;
            //    m_currentWhipState = 0;
            //    m_currentVisualWhipState = 0;
            //    m_animator.SetInteger(m_whipStateAnimationParameter, m_currentWhipState);
            //    m_animator.SetBool(m_whipAttackAnimationParameter, false);
            //}
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

        public void HandleComboTimer()
        {
            if (m_whipComboCooldownTimer > 0)
            {
                m_whipComboCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canWhipCombo = false;
            }
            else
            {
                //Debug.Log("Whip Cooldown Done");
                m_whipComboCooldownTimer = /*m_whipComboCooldown*/m_configuration.whipComboCooldown;
                m_canWhipCombo = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_whipMovementCooldownTimer > 0)
            {
                m_whipMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_whipMovementCooldownTimer = /*m_whipMovementCooldown*/m_configuration.whipMovementCooldown;
                m_canMove = true;
            }
        }
    }
}