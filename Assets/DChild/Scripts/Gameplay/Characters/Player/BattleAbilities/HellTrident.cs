using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class HellTrident : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_hellTridentCooldown;
        [SerializeField]
        private float m_hellTridentMovementCooldown;
        [SerializeField]
        private Info m_hellTridentInfo;
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

        [SerializeField, BoxGroup("HellTrident")]
        private GameObject m_hellTridentGO;
        [SerializeField, BoxGroup("HellTrident")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("HellTrident")]
        private SpineFX m_hellTridentStartAnimation;
        [SerializeField, BoxGroup("HellTrident")]
        private ProjectileInfo m_projectileInfo;

        [SerializeField]
        private Vector2 m_pushForce;

        private ProjectileLauncher m_launcher;

        private bool m_canHellTrident;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_hellTridentStateAnimationParameter;
        private float m_hellTridentCooldownTimer;
        private float m_hellTridentMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanHellTrident() => m_canHellTrident;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_hellTridentStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.HellTrident);
            m_canHellTrident = true;
            m_canMove = true;
            m_hellTridentMovementCooldownTimer = m_hellTridentMovementCooldown;

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
            m_hellTridentInfo.ShowCollider(false);
            m_animator.SetBool(m_hellTridentStateAnimationParameter, false);
            if (m_hellTridentGO.activeSelf)
                m_hellTridentStartAnimation.Stop();
            m_hellTridentGO.SetActive(false);
        }

        public void Execute()
        {
            //m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canHellTrident = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_hellTridentStateAnimationParameter, true);
            m_hellTridentCooldownTimer = m_hellTridentCooldown;
            m_hellTridentMovementCooldownTimer = m_hellTridentMovementCooldown;
            m_hellTridentGO.SetActive(true);
            m_hellTridentStartAnimation.Play();
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            base.AttackOver();
            m_hellTridentInfo.ShowCollider(false);
            m_canHellTrident = true;
            m_canMove = true;
            //m_state.waitForBehaviour = false;
            m_animator.SetBool(m_hellTridentStateAnimationParameter, false);
            if (m_hellTridentGO.activeSelf)
                m_hellTridentStartAnimation.Stop();
            m_hellTridentGO.SetActive(false);
        }

        public override void Cancel()
        {
            base.Cancel();
            m_hellTridentInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            if (m_hellTridentGO.activeSelf)
                m_hellTridentStartAnimation.Stop();
            m_hellTridentGO.SetActive(false);
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_hellTridentInfo.ShowCollider(value);
            m_attackFX.transform.position = m_hellTridentInfo.fxPosition.position;

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
            if (m_hellTridentCooldownTimer > 0)
            {
                m_hellTridentCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canHellTrident = false;
            }
            else
            {
                m_hellTridentCooldownTimer = m_hellTridentCooldown;
                m_state.isAttacking = false;
                m_canHellTrident = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_hellTridentMovementCooldownTimer > 0)
            {
                m_hellTridentMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_hellTridentMovementCooldownTimer = m_hellTridentMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            //LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
            m_launcher.AimAt(new Vector2(m_startPoint.position.x + (m_character.facing == HorizontalDirection.Right ? 10 : -10), m_startPoint.position.y));
            m_launcher.LaunchProjectile();
        }
    }
}
