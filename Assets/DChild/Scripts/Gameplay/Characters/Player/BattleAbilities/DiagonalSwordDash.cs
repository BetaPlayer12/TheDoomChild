using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class DiagonalSwordDash : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_diagonalSwordDashCooldown;
        [SerializeField]
        private float m_diagonalSwordDashMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private Info m_diagonalSwordDashInfo;

        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_groundSensor;

        [SerializeField]
        private Vector2 m_pushForce;
        [SerializeField]
        private Vector2 m_backForce;

        private bool m_canDiagonalSwordDash;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_diagonalSwordDashStateAnimationParameter;
        private float m_diagonalSwordDashCooldownTimer;
        private float m_diagonalSwordDashMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanDiagonalSwordDash() => m_canDiagonalSwordDash;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            
            m_modifier = info.modifier;
            m_diagonalSwordDashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.DiagonalSwordDash);
            m_canDiagonalSwordDash = true;
            m_canMove = true;
            m_diagonalSwordDashMovementCooldownTimer = m_diagonalSwordDashMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_cacheGravity = m_physics.gravityScale;
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            m_diagonalSwordDashInfo.ShowCollider(false);
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            StopAllCoroutines();
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canDiagonalSwordDash = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, true);
            m_diagonalSwordDashCooldownTimer = m_diagonalSwordDashCooldown;
            m_diagonalSwordDashMovementCooldownTimer = m_diagonalSwordDashMovementCooldown;
            m_physics.velocity = Vector2.zero;
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
        }

        public void EndExecution()
        {
            m_diagonalSwordDashInfo.ShowCollider(false);
            m_canMove = true;
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, false);
            m_physics.gravityScale = m_cacheGravity;
            base.AttackOver();
        }

        public override void Cancel()
        {
            m_diagonalSwordDashInfo.ShowCollider(false);
            m_canMove = true;
            m_fxAnimator.Play("Buffer");
            StopAllCoroutines();
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, false);
            m_physics.gravityScale = m_cacheGravity;
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_diagonalSwordDashInfo.ShowCollider(value);
            m_attackFX.transform.position = m_diagonalSwordDashInfo.fxPosition.position;
        }

        public void StartDash()
        {
            StartCoroutine(DashRoutine());
        }

        public void HandleAttackTimer()
        {
            if (m_diagonalSwordDashCooldownTimer > 0)
            {
                m_diagonalSwordDashCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canDiagonalSwordDash = false;
            }
            else
            {
                m_diagonalSwordDashCooldownTimer = m_diagonalSwordDashCooldown;
                m_state.isAttacking = false;
                m_canDiagonalSwordDash = true;
            }
        }

        private IEnumerator DashRoutine()
        {
            m_state.waitForBehaviour = true;
            var timer = m_dashDuration;
            m_wallSensor.Cast();
            m_groundSensor.Cast();
            while (/*timer >= 0 &&*/ !m_wallSensor.isDetecting && !m_groundSensor.isDetecting)
            {
                m_wallSensor.Cast();
                m_groundSensor.Cast();
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y);
                timer -= Time.deltaTime;
                yield return null;
            }
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, false);
            //Debug.Log("End Diagon Sword Dash");
            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? -m_backForce.x : m_backForce.x, m_backForce.y);
            //m_physics.velocity = Vector2.zero;
            yield return null;
        }
    }
}
