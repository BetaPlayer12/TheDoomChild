using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class KrakenRage : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        //[SerializeField]
        //private float m_krakenRageCooldown;
        //[SerializeField]
        //private float m_krakenRageMovementCooldown;
        [SerializeField]
        private Info m_krakenRageInfo;
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

        //[SerializeField]
        //private Vector2 m_pushForce;

        //private bool m_canKrakenRage;
        //private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_krakenRageStateAnimationParameter;
        //private float m_krakenRageCooldownTimer;
        //private float m_krakenRageMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        //public bool CanKrakenRage() => m_canKrakenRage;
        //public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_krakenRageStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.KrakenRage);
            //m_canKrakenRage = true;
            //m_canMove = true;
            //m_krakenRageMovementCooldownTimer = m_krakenRageMovementCooldown;
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
            base.Reset();
            m_krakenRageInfo.ShowCollider(false);
            m_animator.SetBool(m_krakenRageStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
            m_physics.velocity = Vector2.zero;
            //m_canKrakenRage = false;
            //m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_krakenRageStateAnimationParameter, true);
            //m_krakenRageCooldownTimer = m_krakenRageCooldown;
            //m_krakenRageMovementCooldownTimer = m_krakenRageMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            base.AttackOver();
            m_state.waitForBehaviour = false;
            m_krakenRageInfo.ShowCollider(false);
            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
            m_animator.SetBool(m_krakenRageStateAnimationParameter, false);
        }

        public override void Cancel()
        {
            base.Cancel();
            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
            m_krakenRageInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_krakenRageInfo.ShowCollider(value);
            m_attackFX.transform.position = m_krakenRageInfo.fxPosition.position;
            m_physics.velocity = Vector2.zero;

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

        //public void HandleAttackTimer()
        //{
        //    if (m_krakenRageCooldownTimer > 0)
        //    {
        //        m_krakenRageCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canKrakenRage = false;
        //    }
        //    else
        //    {
        //        m_krakenRageCooldownTimer = m_krakenRageCooldown;
        //        m_state.isAttacking = false;
        //        m_canKrakenRage = true;
        //    }
        //}

        //public void HandleMovementTimer()
        //{
        //    if (m_krakenRageMovementCooldownTimer > 0)
        //    {
        //        m_krakenRageMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_krakenRageMovementCooldownTimer = m_krakenRageMovementCooldown;
        //        m_canMove = true;
        //    }
        //}
    }
}
