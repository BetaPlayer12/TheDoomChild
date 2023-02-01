using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class BackDiver : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_backDiverCooldown;
        [SerializeField]
        private float m_backDiverMovementCooldown;
        [SerializeField]
        private Info m_backDiverInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_enemySensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_wallSensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_edgeSensor;
        [SerializeField]
        private Hitbox m_hitbox;
        [SerializeField]
        private float m_hitboxDuration;

        [SerializeField]
        private Vector2 m_pushForce;


        private bool m_canBackDiver;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_backDiverStateAnimationParameter;
        private float m_backDiverCooldownTimer;
        private float m_backDiverMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanBackDiver() => m_canBackDiver;
        //public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_backDiverStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.BackDiver);
            m_canBackDiver = true;
            m_canMove = true;
            m_backDiverMovementCooldownTimer = m_backDiverMovementCooldown;

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
            //m_backDiverInfo.ShowCollider(false);
            m_animator.SetBool(m_backDiverStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canBackDiver = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_backDiverStateAnimationParameter, true);
            m_backDiverCooldownTimer = m_backDiverCooldown;
            m_backDiverMovementCooldownTimer = m_backDiverMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Left ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            StartCoroutine(HitboxRoutine());
        }

        public void EndExecution()
        {
            base.AttackOver();
            //m_backDiverInfo.ShowCollider(false);
            //m_hitbox.Enable();
            m_animator.SetBool(m_backDiverStateAnimationParameter, false);
            //m_canBackDiver = true;
            //m_canMove = true;
            m_state.waitForBehaviour = false;
        }

        public override void Cancel()
        {
            base.Cancel();
            //m_backDiverInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            m_hitbox.Enable();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_backDiverInfo.ShowCollider(value);
            m_attackFX.transform.position = m_backDiverInfo.fxPosition.position;

            //TEST
            //m_enemySensor.Cast();
            //m_wallSensor.Cast();
            //m_edgeSensor.Cast();
            //if (!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting && value)
            //{
            //    m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //}
        }

        public void ResetBackDiver()
        {
            m_canBackDiver = true;
        }

        public void HandleAttackTimer()
        {
            if (m_backDiverCooldownTimer > 0)
            {
                m_backDiverCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canBackDiver = false;
            }
            else
            {
                m_backDiverCooldownTimer = m_backDiverCooldown;
                m_state.isAttacking = false;
                m_canBackDiver = true;
            }
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_backDiverMovementCooldownTimer > 0)
        //    {
        //        m_backDiverMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_backDiverMovementCooldownTimer = m_backDiverMovementCooldown;
        //        m_canMove = true;
        //    }
        //}

        private IEnumerator HitboxRoutine()
        {
            m_hitbox.Disable();
            yield return new WaitForSeconds(m_hitboxDuration);
            m_hitbox.Enable();
            yield return null;
        }
    }
}
