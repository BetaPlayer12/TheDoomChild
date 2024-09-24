using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class EdgedFury : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_edgedFuryCooldown;
        //[SerializeField]
        //private float m_edgedFuryMovementCooldown;
        [SerializeField]
        private Info m_edgedFuryInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_enemySensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_wallSensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("FX")]
        private ParticleSystem m_fx;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_fxGO;

        [SerializeField]
        private Vector2 m_pushForce;
        [SerializeField]
        private EdgedFuryCamera m_edgedFuryCamera;

        private bool m_canEdgedFury;
        //private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_edgedFuryStateAnimationParameter;
        private float m_edgedFuryCooldownTimer;
        //private float m_edgedFuryMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanEdgedFury() => m_canEdgedFury;
        //public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_edgedFuryStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.EdgedFury);
            m_canEdgedFury = true;
            //m_canMove = true;
            //m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
            m_cacheGravity = m_physics.gravityScale;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            //m_edgedFuryInfo.PlayFX(false);
            //m_fx.gameObject.SetActive(false);
            //m_fx.Stop();
            m_fxGO.SetActive(false);
            m_edgedFuryInfo.ShowCollider(false);
            m_animator.SetBool(m_edgedFuryStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            m_edgedFuryCamera.ActivateCullingMask();
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
            //m_physics.velocity = Vector2.zero;
            m_canEdgedFury = false;
            //m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_edgedFuryStateAnimationParameter, true);
            //m_edgedFuryInfo.PlayFX(true);
            //m_fx.gameObject.SetActive(true);
            //m_fx.Play();
            //m_fxGO.SetActive(true);
            //m_edgedFuryCooldownTimer = m_edgedFuryCooldown;
            //m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            m_fxGO.SetActive(false);
            m_edgedFuryInfo.ShowCollider(false);
            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
            m_animator.SetBool(m_edgedFuryStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_edgedFuryInfo.PlayFX(false);
            //m_fx.gameObject.SetActive(false);
            //m_fx.Stop();
            m_fxGO.SetActive(false);
            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
            m_edgedFuryInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_edgedFuryStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_fxGO.SetActive(true);
            m_rigidBody.WakeUp();
            m_edgedFuryInfo.ShowCollider(value);
            m_attackFX.transform.position = m_edgedFuryInfo.fxPosition.position;
            m_physics.velocity = Vector2.zero;

            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //TEST
            //m_enemySensor.Cast();
            //m_wallSensor.Cast();
            //m_edgeSensor.Cast();
            //if (!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting && value)
            //{
            //    m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //}
            //else if (!value)
            //{
            //    m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            //}
        }

        public void HandleAttackTimer()
        {
            if (m_edgedFuryCooldownTimer > 0)
            {
                m_edgedFuryCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canEdgedFury = false;
            }
            else
            {
                m_edgedFuryCooldownTimer = m_edgedFuryCooldown;
                //m_state.isAttacking = false;
                m_canEdgedFury = true;
            }
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_edgedFuryMovementCooldownTimer > 0)
        //    {
        //        m_edgedFuryMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
        //        m_canMove = true;
        //    }
        //}
    }
}
