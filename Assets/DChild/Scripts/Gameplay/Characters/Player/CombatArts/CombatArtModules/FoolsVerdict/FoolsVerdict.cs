using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class FoolsVerdict : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_foolsVerdictCooldown;
        [SerializeField]
        private float m_foolsVerdictMovementCooldown;
        [SerializeField]
        private Info m_foolsVerdictInfo;
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


        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectileInfo;

        private ProjectileLauncher m_launcher;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canFoolsVerdict;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_foolsVerdictStateAnimationParameter;
        private float m_foolsVerdictCooldownTimer;
        private float m_foolsVerdictMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanFoolsVerdict() => m_canFoolsVerdict;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_foolsVerdictStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FoolsVerdict);
            m_canFoolsVerdict = true;
            m_canMove = true;
            m_foolsVerdictMovementCooldownTimer = m_foolsVerdictMovementCooldown;

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
            //m_foolsVerdictInfo.ShowCollider(false);
            StopAllCoroutines();
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canFoolsVerdict = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, true);
            m_foolsVerdictCooldownTimer = m_foolsVerdictCooldown;
            m_foolsVerdictMovementCooldownTimer = m_foolsVerdictMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            //Summon();
        }

        public void EndExecution()
        {
            //m_foolsVerdictInfo.ShowCollider(false);
            //m_canFoolsVerdict = true;
            m_canMove = true;
            StopAllCoroutines();
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_foolsVerdictInfo.ShowCollider(false);
            StopAllCoroutines();
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_foolsVerdictInfo.ShowCollider(value);
            m_attackFX.transform.position = m_foolsVerdictInfo.fxPosition.position;

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
            if (m_foolsVerdictCooldownTimer > 0)
            {
                m_foolsVerdictCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canFoolsVerdict = false;
            }
            else
            {
                m_foolsVerdictCooldownTimer = m_foolsVerdictCooldown;
                //m_state.isAttacking = false;
                m_canFoolsVerdict = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_foolsVerdictMovementCooldownTimer > 0)
            {
                m_foolsVerdictMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_foolsVerdictMovementCooldownTimer = m_foolsVerdictMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
            instance.transform.position = m_startPoint.position;
            instance.GetComponent<Attacker>().SetParentAttacker(m_attacker);

            m_launcher.AimAt(new Vector2(m_startPoint.position.x + (m_character.facing == HorizontalDirection.Right ? 10 : -10), m_startPoint.position.y));
            m_launcher.LaunchProjectile(m_startPoint.right, instance.gameObject);
        }
    }
}
