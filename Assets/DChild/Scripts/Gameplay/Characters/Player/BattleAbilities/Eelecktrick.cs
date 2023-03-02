using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class Eelecktrick : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_eelecktrickCooldown;
        [SerializeField]
        private float m_eelecktrickMovementCooldown;
        [SerializeField]
        private Info m_eelecktrickInfo;
        //TEST
        [SerializeField]
        private CharacterState m_characterState;
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
        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectileInfo;

        [SerializeField]
        private Vector2 m_pushForce;

        private ProjectileLauncher m_launcher;

        private bool m_canEelecktrick;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_eelecktrickStateAnimationParameter;
        private float m_eelecktrickCooldownTimer;
        private float m_eelecktrickMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanEelecktrick() => m_canEelecktrick;
        public bool CanMove() => m_canMove;
        private bool m_hasExecuted;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_eelecktrickStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Eelecktrick);;
            m_canEelecktrick = true;
            m_canMove = true;
            m_eelecktrickMovementCooldownTimer = m_eelecktrickMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();

            m_launcher = new ProjectileLauncher(m_projectileInfo, m_startPoint);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            //m_eelecktrickInfo.ShowCollider(false);
            m_animator.SetBool(m_eelecktrickStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_hasExecuted = true;
            //m_state.waitForBehaviour = true;
            //m_state.isAttacking = true;
            m_characterState.isChargingEelecktrick = true;
            //m_state.canAttack = false;
            m_canEelecktrick = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_eelecktrickStateAnimationParameter, true);
            m_eelecktrickCooldownTimer = m_eelecktrickCooldown;
            m_eelecktrickMovementCooldownTimer = m_eelecktrickMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void ReleaseHold()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_characterState.isChargingEelecktrick = false;
            m_animator.SetBool(m_eelecktrickStateAnimationParameter, false);
            m_animator.SetBool(m_animationParameter, true);
        }

        public void SummonWhip()
        {
            m_physics.velocity = Vector2.zero;
            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
            m_launcher.AimAt(new Vector2(m_startPoint.position.x + (m_character.facing == HorizontalDirection.Right ? 10 : -10), m_startPoint.position.y));
            m_launcher.LaunchProjectile();
        }

        public void EndExecution()
        {
            m_hasExecuted = false;
            m_animator.SetBool(m_eelecktrickStateAnimationParameter, false);
            m_state.canAttack = true;
            m_state.isAttacking = false;
            m_characterState.isChargingEelecktrick = false;
            base.AttackOver();
            //m_eelecktrickInfo.ShowCollider(false);
            //m_canEelecktrick = true;
            //m_canMove = true;
        }

        public override void Cancel()
        {
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                base.Cancel();
                //m_eelecktrickInfo.ShowCollider(false);
                m_fxAnimator.Play("Buffer");
                StopAllCoroutines();
                m_characterState.isChargingEelecktrick = false;
                m_animator.SetBool(m_eelecktrickStateAnimationParameter, false);
            }
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_eelecktrickInfo.ShowCollider(value);
            m_attackFX.transform.position = m_eelecktrickInfo.fxPosition.position;
            if (!value)
                m_fxAnimator.Play("Buffer");
        }

        public void HandleAttackTimer()
        {
            if (m_eelecktrickCooldownTimer > 0)
            {
                m_eelecktrickCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canEelecktrick = false;
            }
            else
            {
                m_eelecktrickCooldownTimer = m_eelecktrickCooldown;
                //m_state.isAttacking = false;
                m_canEelecktrick = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_eelecktrickMovementCooldownTimer > 0)
            {
                m_eelecktrickMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                m_eelecktrickMovementCooldownTimer = m_eelecktrickMovementCooldown;
                m_canMove = true;
            }
        }
    }
}
