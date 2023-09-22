using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class AirSlashCombo : AttackBehaviour
    {
        //[SerializeField, HideLabel]
        //private SlashComboStatsInfo m_configuration;
        [SerializeField]
        private int m_airSlashStateAmount;
        //[SerializeField]
        //private float m_airSlashComboCooldown;
        [SerializeField]
        private float m_airlashMovementCooldown;
        [SerializeField]
        private float m_comboResetDelay;

        [SerializeField]
        private SkeletonAnimation m_attackFX;
        //[SerializeField]
        //private int m_slashStateAmount;
        //[SerializeField]
        //private float m_slashComboCooldown;
        //[SerializeField]
        //private float m_slashMovementCooldown;
        [SerializeField]
        private List<Info> m_airSlashComboInfo;

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

        [SerializeField]
        private List<Vector2> m_pushForce;
        [SerializeField]
        private float m_pushForceDuration;

        private bool m_canAirSlashCombo;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_currentAirSlashState;
        private int m_currentVisualAirSlashState;
        private float m_comboAttackDelayTimer;
        private float m_comboResetDelayTimer;
        //private float m_airSlashComboCooldownTimer;
        private float m_airSlashMovementCooldownTimer;
        private bool m_allowAttackDelayHandling;
        private int m_airSlashComboStateAnimationParameter;
        private int m_airSlashStateAnimationParameter;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanAirSlashCombo() => m_canAirSlashCombo;
        public bool CanMove() => m_canMove;

        private Coroutine m_airSlashDashCoroutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_airSlashComboStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AirSlashCombo);
            m_airSlashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AirSlashState);
            m_currentAirSlashState = -1;
            m_currentVisualAirSlashState = 0;
            m_comboAttackDelayTimer = -1;
            m_comboResetDelayTimer = -1;
            //m_airSlashComboCooldownTimer = m_airlashMovementCooldown;
            m_allowAttackDelayHandling = true;
            m_canAirSlashCombo = true;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_cacheGravity = m_physics.gravityScale;

            m_animator.SetInteger(m_airSlashStateAnimationParameter, m_currentAirSlashState);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();

            m_currentAirSlashState = -1;
            m_currentVisualAirSlashState = 0;
            m_animator.SetBool(m_airSlashComboStateAnimationParameter, false);
            m_animator.SetInteger(m_airSlashStateAnimationParameter, m_currentAirSlashState);
            m_physics.gravityScale = m_cacheGravity;
            m_fxAnimator.Play("Buffer");
            m_physics.velocity = Vector2.zero;
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canMove = false;
            m_comboResetDelayTimer = m_comboResetDelay;
            m_currentAirSlashState += m_currentAirSlashState >= m_airSlashStateAmount - 1 ? 0 : 1;
            m_comboAttackDelayTimer = m_airSlashComboInfo[m_currentAirSlashState].nextAttackDelay;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_airSlashComboStateAnimationParameter, true);
            m_animator.SetInteger(m_airSlashStateAnimationParameter, m_currentAirSlashState);
            m_attacker.SetDamageModifier(m_airSlashComboInfo[m_currentAirSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_currentVisualAirSlashState = m_currentAirSlashState;
            m_physics.gravityScale = 0;

            m_airSlashMovementCooldownTimer = /*m_slashMovementCooldown*/m_airlashMovementCooldown;
        }

        public override void Cancel()
        {
            if (m_airSlashDashCoroutine != null)
            {
                StopCoroutine(m_airSlashDashCoroutine);
                m_airSlashDashCoroutine = null;
            }
            m_state.isDoingCombo = false;
            for (int i = 0; i < m_airSlashComboInfo.Count; i++)
            {
                m_airSlashComboInfo[i].ShowCollider(false);
            }
            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
            m_fxAnimator.Play("Buffer");
            base.Cancel();
        }

        public void PlayFX(bool value)
        {
            m_airSlashComboInfo[m_currentVisualAirSlashState].PlayFX(value);
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_airSlashComboInfo[m_currentVisualAirSlashState].ShowCollider(value);
            m_attackFX.transform.position = m_airSlashComboInfo[m_currentVisualAirSlashState].fxPosition.position;

            switch (m_currentVisualAirSlashState)
            {
                case 0:
                    m_fxAnimator.Play("SlashCombo2");
                    break;
                case 1:
                    m_fxAnimator.Play("JumpSlash");
                    break;
                case 2:
                    m_fxAnimator.Play("SlashCombo2");
                    break;
                default:
                    break;
            }

            //TEST
            m_enemySensor.Cast();
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            if (!m_enemySensor.isDetecting /*&& !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting*/)
            {
                if (m_airSlashDashCoroutine != null)
                {
                    StopCoroutine(m_airSlashDashCoroutine);
                    m_airSlashDashCoroutine = null;
                }
                m_airSlashDashCoroutine = StartCoroutine(AirSlashDashRoutine());
                //m_physics.AddForce(m_character.facing == HorizontalDirection.Right ? m_pushForce[m_currentVisualAirSlashState] : -m_pushForce[m_currentVisualAirSlashState], ForceMode2D.Impulse);
            }
        }

        public override void AttackOver()
        {
            for (int i = 0; i < m_airSlashComboInfo.Count; i++)
            {
                m_airSlashComboInfo[i].ShowCollider(false);
            }

            if (m_currentAirSlashState >= m_airSlashStateAmount - 1)
            {
                //m_currentAirSlashState = 0;
                //m_canAirSlashCombo = false;
                m_canMove = false;
                //m_physics.gravityScale = m_cacheGravity;

                //Reset();

                m_canAirSlashCombo = false;
                m_currentAirSlashState = -1;
                //m_currentVisualAirSlashState = 0;
                //m_animator.SetBool(m_airSlashComboStateAnimationParameter, false);
                //m_animator.SetInteger(m_airSlashStateAnimationParameter, m_currentAirSlashState);
                //m_physics.gravityScale = m_cacheGravity;
            }
            //m_physics.velocity = Vector2.zero;
            m_physics.gravityScale = m_cacheGravity;
            //base.AttackOver();
            //m_state.canAttack = true;
            m_animator.SetBool(m_animationParameter, false);
            m_state.isAttacking = false;
            m_state.waitForBehaviour = false;

            //m_fxAnimator.Play("Buffer");
        }

        public void ComboEnd()
        {
            if (m_state.isAttacking == true)
            {

            }
            else
            {
                base.AttackOver();
                if (m_airSlashDashCoroutine != null)
                {
                    StopCoroutine(m_airSlashDashCoroutine);
                    m_airSlashDashCoroutine = null;
                }
                m_state.canAttack = true;
                m_canAirSlashCombo = false;
                m_currentAirSlashState = -1;
                m_currentVisualAirSlashState = 0;
                m_physics.gravityScale = m_cacheGravity;
                m_animator.SetBool(m_airSlashComboStateAnimationParameter, false);
                m_animator.SetInteger(m_airSlashStateAnimationParameter, m_currentAirSlashState);
                m_fxAnimator.Play("Buffer");
                m_physics.velocity = Vector2.zero;
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
                    //Reset();
                    ComboEnd();
                }
            }
        }

        //public void HandleAirSlashComboTimer()
        //{
        //    if (m_airSlashComboCooldownTimer > 0)
        //    {
        //        m_airSlashComboCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canAirSlashCombo = false;
        //    }
        //    else
        //    {
        //        m_airSlashComboCooldownTimer = m_airlashMovementCooldown;
        //        m_canAirSlashCombo = true;
        //    }
        //}

        public void ResetAirSlashCombo()
        {
            m_canAirSlashCombo = true;
            //m_currentAirSlashState = -1;
            Reset();
        }

        public void HandleMovementTimer()
        {
            if (m_airSlashMovementCooldownTimer > 0)
            {
                m_airSlashMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_airSlashMovementCooldownTimer = m_airlashMovementCooldown;
                m_canMove = true;
            }
        }

        private IEnumerator AirSlashDashRoutine()
        {
            m_physics.velocity = Vector2.zero;
            var timer = 0f;
            while (timer <= m_pushForceDuration && !m_enemySensor.isDetecting && !m_wallSensor.isDetecting)
            {
                timer += Time.deltaTime;
                m_physics.velocity = m_character.facing == HorizontalDirection.Right ? m_pushForce[m_currentVisualAirSlashState] : -m_pushForce[m_currentVisualAirSlashState];
                yield return null;
            }
            //m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
        }
    }
}