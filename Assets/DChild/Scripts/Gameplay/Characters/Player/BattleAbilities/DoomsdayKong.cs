using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class DoomsdayKong : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_doomsdayKongCooldown;
        [SerializeField]
        private float m_doomsdayKongMovementCooldown;
        [SerializeField]
        private Info m_doomsdayKongInfo;
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

        private BallisticProjectileLauncher m_launcher;

        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        private Vector2 m_startPointCache;
        [SerializeField, BoxGroup("Projectile")]
        private Transform m_endPoint;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectileInfo;
        [SerializeField, BoxGroup("Projectile")]
        private float m_projectileGravity;


        private bool m_canDoomsdayKong;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_doomsdayKongStateAnimationParameter;
        private float m_doomsdayKongCooldownTimer;
        private float m_doomsdayKongMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanDoomsdayKong() => m_canDoomsdayKong;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_doomsdayKongStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.DoomsdayKong);
            m_canDoomsdayKong = true;
            m_canMove = true;
            m_doomsdayKongMovementCooldownTimer = m_doomsdayKongMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();

            m_startPointCache = m_startPoint.localPosition;
            m_launcher = new BallisticProjectileLauncher(m_projectileInfo, m_startPoint, m_projectileGravity, m_projectileInfo.speed);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            //m_doomsdayKongInfo.ShowCollider(false);
            m_animator.SetBool(m_doomsdayKongStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canDoomsdayKong = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_doomsdayKongStateAnimationParameter, true);
            m_doomsdayKongCooldownTimer = m_doomsdayKongCooldown;
            m_doomsdayKongMovementCooldownTimer = m_doomsdayKongMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            m_state.waitForBehaviour = false;
            //m_doomsdayKongInfo.ShowCollider(false);
            //m_canDoomsdayKong = true;
            //m_canMove = true;
            m_animator.SetBool(m_doomsdayKongStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_doomsdayKongInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_doomsdayKongStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_doomsdayKongInfo.ShowCollider(value);
            m_attackFX.transform.position = m_doomsdayKongInfo.fxPosition.position;

            //TEST
            m_enemySensor.Cast();
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            if (!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting && value)
            {
                m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            }
        }

        public void HandleAttackTimer()
        {
            if (m_doomsdayKongCooldownTimer > 0)
            {
                m_doomsdayKongCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canDoomsdayKong = false;
            }
            else
            {
                m_doomsdayKongCooldownTimer = m_doomsdayKongCooldown;
                m_state.isAttacking = false;
                m_canDoomsdayKong = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_doomsdayKongMovementCooldownTimer > 0)
            {
                m_doomsdayKongMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_doomsdayKongMovementCooldownTimer = m_doomsdayKongMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            m_enemySensor.Cast();
            var hits = m_enemySensor.GetHits();
            var target = hits[0].point;
            m_launcher.AimAt(target != Vector2.zero ? target : (Vector2)m_endPoint.position);
            m_launcher.LaunchBallisticProjectile(target != Vector2.zero ? target : (Vector2)m_endPoint.position);
        }
    }
}
