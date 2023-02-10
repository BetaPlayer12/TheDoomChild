using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class SoulFireBlast : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_soulFireBlastCooldown;
        [SerializeField]
        private float m_soulFireBlastMovementCooldown;
        [SerializeField]
        private Info m_soulFireBlastInfo;
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

        [SerializeField, BoxGroup("SoulFireBlast")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("SoulFireBlast")]
        private ProjectileInfo m_projectileInfo;

        [SerializeField]
        private Vector2 m_pushForce;

        private ProjectileLauncher m_launcher;

        private bool m_canSoulFireBlast;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_soulFireBlastStateAnimationParameter;
        private float m_soulFireBlastCooldownTimer;
        private float m_soulFireBlastMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanSoulFireBlast() => m_canSoulFireBlast;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_soulFireBlastStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SoulFireBlast);
            m_canSoulFireBlast = true;
            m_canMove = true;
            m_soulFireBlastMovementCooldownTimer = m_soulFireBlastMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();

            m_launcher = new ProjectileLauncher(m_projectileInfo, m_startPoint);

            m_cacheGravity = m_physics.gravityScale;
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            //m_soulFireBlastInfo.ShowCollider(false);
            m_animator.SetBool(m_soulFireBlastStateAnimationParameter, false);
            //m_soulFireBlastStartAnimation.Stop();
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canSoulFireBlast = false;
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_soulFireBlastStateAnimationParameter, true);
            m_soulFireBlastCooldownTimer = m_soulFireBlastCooldown;
            m_soulFireBlastMovementCooldownTimer = m_soulFireBlastMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            //m_soulFireBlastInfo.ShowCollider(false);
            m_physics.gravityScale = m_cacheGravity;
            //m_canSoulFireBlast = true;
            m_canMove = true;
            m_animator.SetBool(m_soulFireBlastStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            base.Cancel();
            //m_soulFireBlastInfo.ShowCollider(false);
            m_physics.gravityScale = m_cacheGravity;
            m_fxAnimator.Play("Buffer");
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_soulFireBlastInfo.ShowCollider(value);
            m_attackFX.transform.position = m_soulFireBlastInfo.fxPosition.position;

            //TEST
        }

        public void HandleAttackTimer()
        {
            if (m_soulFireBlastCooldownTimer > 0)
            {
                m_soulFireBlastCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canSoulFireBlast = false;
            }
            else
            {
                m_soulFireBlastCooldownTimer = m_soulFireBlastCooldown;
                m_state.isAttacking = false;
                m_canSoulFireBlast = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_soulFireBlastMovementCooldownTimer > 0)
            {
                m_soulFireBlastMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_soulFireBlastMovementCooldownTimer = m_soulFireBlastMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            m_physics.velocity = Vector2.zero;
            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
            m_launcher.AimAt(new Vector2(m_startPoint.position.x + (m_character.facing == HorizontalDirection.Right ? 10 : -10), m_startPoint.position.y));
            m_launcher.LaunchProjectile();
        }
    }
}
