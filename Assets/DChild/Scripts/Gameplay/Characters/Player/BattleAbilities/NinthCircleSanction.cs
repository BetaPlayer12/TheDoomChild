using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class NinthCircleSanction : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_ninthCircleSanctionCooldown;
        [SerializeField]
        private float m_ninthCircleSanctionMovementCooldown;
        [SerializeField]
        private Info m_ninthCircleSanctionInfo;
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

        private ProjectileLauncher m_launcher;
        
        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        private Vector2 m_startPointCache;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectileInfo;


        private bool m_canNinthCircleSanction;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_ninthCircleSanctionStateAnimationParameter;
        private float m_ninthCircleSanctionCooldownTimer;
        private float m_ninthCircleSanctionMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanNinthCircleSanction() => m_canNinthCircleSanction;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_ninthCircleSanctionStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.NinthCircleSanction);
            m_canNinthCircleSanction = true;
            m_canMove = true;
            m_ninthCircleSanctionMovementCooldownTimer = m_ninthCircleSanctionMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();

            m_startPointCache = m_startPoint.localPosition;
            m_launcher = new ProjectileLauncher(m_projectileInfo, m_startPoint);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            //m_ninthCircleSanctionInfo.ShowCollider(false);
            m_animator.SetBool(m_ninthCircleSanctionStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canNinthCircleSanction = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_ninthCircleSanctionStateAnimationParameter, true);
            m_ninthCircleSanctionCooldownTimer = m_ninthCircleSanctionCooldown;
            m_ninthCircleSanctionMovementCooldownTimer = m_ninthCircleSanctionMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            //m_ninthCircleSanctionInfo.ShowCollider(false);
            m_animator.SetBool(m_ninthCircleSanctionStateAnimationParameter, false);
            //m_canNinthCircleSanction = true;
            //m_canMove = true;
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_ninthCircleSanctionInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_ninthCircleSanctionStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_ninthCircleSanctionInfo.ShowCollider(value);
            m_attackFX.transform.position = m_ninthCircleSanctionInfo.fxPosition.position;

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
            if (m_ninthCircleSanctionCooldownTimer > 0)
            {
                m_ninthCircleSanctionCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canNinthCircleSanction = false;
            }
            else
            {
                m_ninthCircleSanctionCooldownTimer = m_ninthCircleSanctionCooldown;
                m_state.isAttacking = false;
                m_canNinthCircleSanction = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_ninthCircleSanctionMovementCooldownTimer > 0)
            {
                m_ninthCircleSanctionMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_ninthCircleSanctionMovementCooldownTimer = m_ninthCircleSanctionMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            m_enemySensor.Cast();
            var hits = m_enemySensor.GetHits();
            m_startPoint.position = hits[0].point;
            var target = hits[0].point;
            m_launcher.AimAt(target);
            if (target != Vector2.zero)
                m_launcher.LaunchProjectile();
            m_startPoint.localPosition = m_startPointCache;
        }
    }
}
