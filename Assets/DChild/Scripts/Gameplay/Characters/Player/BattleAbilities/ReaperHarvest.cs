using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class ReaperHarvest : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_reaperHarvestCooldown;
        [SerializeField]
        private float m_reaperHarvestMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private Info m_reaperHarvestInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canReaperHarvest;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_reaperHarvestStateAnimationParameter;
        private float m_reaperHarvestCooldownTimer;
        private float m_reaperHarvestMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanReaperHarvest() => m_canReaperHarvest;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_reaperHarvestStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ReaperHarvest);
            m_canReaperHarvest = true;
            m_canMove = true;
            m_reaperHarvestMovementCooldownTimer = m_reaperHarvestMovementCooldown;

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
            m_reaperHarvestInfo.ShowCollider(false);
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, false);
        }

        public void Execute()
        {
            //m_state.waitForBehaviour = true;
            StopAllCoroutines();
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canReaperHarvest = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, true);
            m_reaperHarvestCooldownTimer = m_reaperHarvestCooldown;
            m_reaperHarvestMovementCooldownTimer = m_reaperHarvestMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            //m_state.waitForBehaviour = false;
            base.AttackOver();
            m_reaperHarvestInfo.ShowCollider(false);
            //m_canReaperHarvest = true;
            //m_canMove = true;
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, false);
        }

        public override void Cancel()
        {
            base.Cancel();
            m_reaperHarvestInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            StopAllCoroutines();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_reaperHarvestInfo.ShowCollider(value);
            m_attackFX.transform.position = m_reaperHarvestInfo.fxPosition.position;

            StartCoroutine(DashRoutine());
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
            if (m_reaperHarvestCooldownTimer > 0)
            {
                m_reaperHarvestCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canReaperHarvest = false;
            }
            else
            {
                m_reaperHarvestCooldownTimer = m_reaperHarvestCooldown;
                m_state.isAttacking = false;
                m_canReaperHarvest = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_reaperHarvestMovementCooldownTimer > 0)
            {
                m_reaperHarvestMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_reaperHarvestMovementCooldownTimer = m_reaperHarvestMovementCooldown;
                m_canMove = true;
            }
        }

        private IEnumerator DashRoutine()
        {
            //m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            var timer = m_dashDuration;
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            while (timer >= 0 && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting)
            {
                m_wallSensor.Cast();
                m_edgeSensor.Cast();
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_physics.velocity.y);
                timer -= Time.deltaTime;
                yield return null;
            }
            //yield return new WaitForSeconds(m_dashDuration);
            m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            yield return null;
        }
    }
}
