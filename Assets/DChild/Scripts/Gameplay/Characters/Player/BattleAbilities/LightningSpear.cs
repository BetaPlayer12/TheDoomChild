using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class LightningSpear : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_lightningSpearCooldown;
        [SerializeField]
        private float m_lightningSpearMovementCooldown;
        [SerializeField]
        private float m_lightningSpearResetCooldown;
        [SerializeField]
        private Info m_lightningSpearInfo;
        //TEST
        [SerializeField]
        private CharacterState m_characterState;
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
        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectileInfo;

        [SerializeField]
        private Vector2 m_pushForce;

        private ProjectileLauncher m_launcher;

        private bool m_canLightningSpear;
        private bool m_canMove;
        private bool m_canReset;
        private IPlayerModifer m_modifier;
        private int m_lightningSpearStateAnimationParameter;
        private float m_lightningSpearCooldownTimer;
        private float m_lightningSpearMovementCooldownTimer;
        private float m_lightningSpearResetCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanLightningSpear() => m_canLightningSpear;
        public bool CanMove() => m_canMove;
        public bool CanReset() => m_canReset;
        private bool m_hasExecuted;

        private Coroutine m_lightningSpearChargingRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_lightningSpearStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LightningSpear); ;
            m_canLightningSpear = true;
            m_canMove = true;
            m_lightningSpearMovementCooldownTimer = m_lightningSpearMovementCooldown;
            m_lightningSpearResetCooldownTimer = m_lightningSpearResetCooldown;

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
            //m_lightningSpearInfo.ShowCollider(false);
            m_animator.SetBool(m_lightningSpearStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_hasExecuted = true;
            m_state.waitForBehaviour = true; // Temporary (Delete after)
            //m_lightningSpearChargingRoutine = StartCoroutine(LightningSpearChargingRoutine()); //Temporary Disabled
            //m_state.waitForBehaviour = false; //Temporary Disabled
            m_state.isAttacking = true;
            //m_characterState.isChargingLightningSpear = true; //Temporary Disabled
            m_canLightningSpear = false;
            m_canMove = false;
            //m_canReset = true; //Temporary Disabled
            m_canReset = false; // Temporary (Delete after)
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
            m_physics.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_lightningSpearStateAnimationParameter, true);
            m_lightningSpearCooldownTimer = m_lightningSpearCooldown;
            m_lightningSpearMovementCooldownTimer = m_lightningSpearMovementCooldown;
            m_lightningSpearResetCooldownTimer = m_lightningSpearResetCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage)); //Temporary Disabled
        }

        public void ReleaseHold()
        {
            if (m_lightningSpearChargingRoutine != null)
            {
                StopCoroutine(m_lightningSpearChargingRoutine);
                m_lightningSpearChargingRoutine = null;
            }
            m_state.waitForBehaviour = true;
            m_characterState.isChargingLightningSpear = false;
            m_animator.SetBool(m_lightningSpearStateAnimationParameter, false);

            m_lightningSpearResetCooldownTimer = m_lightningSpearResetCooldown;
            m_canReset = false;
        }

        public void SummonLightning()
        {
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
            instance.transform.position = m_startPoint.position;
            instance.GetComponent<Attacker>().SetParentAttacker(m_attacker);

            m_physics.velocity = Vector2.zero;
            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            m_physics.gravityScale = m_cacheGravity;

            m_launcher.AimAt(new Vector2(m_startPoint.position.x + (m_character.facing == HorizontalDirection.Right ? 10 : -10), m_startPoint.position.y - 10));
            m_launcher.LaunchProjectile(m_startPoint.right, instance.gameObject);
        }

        public void EndExecution()
        {
            m_hasExecuted = false;
            m_state.canAttack = true;
            m_state.isAttacking = false;
            m_physics.gravityScale = m_cacheGravity;
            m_characterState.isChargingLightningSpear = false;
            //m_lightningSpearInfo.ShowCollider(false);
            //m_canMove = true;
            if (m_lightningSpearChargingRoutine != null)
            {
                StopCoroutine(m_lightningSpearChargingRoutine);
                m_lightningSpearChargingRoutine = null;
            }
            m_animator.SetBool(m_lightningSpearStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                //m_lightningSpearInfo.ShowCollider(false);
                m_fxAnimator.Play("Buffer");
                StopAllCoroutines();
                m_physics.gravityScale = m_cacheGravity;
                m_characterState.isChargingLightningSpear = false;
                if (m_lightningSpearChargingRoutine != null)
                {
                    StopCoroutine(m_lightningSpearChargingRoutine);
                    m_lightningSpearChargingRoutine = null;
                }
                m_animator.SetBool(m_lightningSpearStateAnimationParameter, false);
                base.Cancel();
            }
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_lightningSpearInfo.ShowCollider(value);
            m_attackFX.transform.position = m_lightningSpearInfo.fxPosition.position;
            if (!value)
                m_fxAnimator.Play("Buffer");
        }

        public void HandleAttackTimer()
        {
            if (m_lightningSpearCooldownTimer > 0)
            {
                m_lightningSpearCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canLightningSpear = false;
            }
            else
            {
                m_lightningSpearCooldownTimer = m_lightningSpearCooldown;
                //m_state.isAttacking = false;
                m_canLightningSpear = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_lightningSpearMovementCooldownTimer > 0)
            {
                m_lightningSpearMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                m_lightningSpearMovementCooldownTimer = m_lightningSpearMovementCooldown;
                m_canMove = true;
            }
        }

        public void HandleResetTimer()
        {
            if (m_lightningSpearResetCooldownTimer > 0)
            {
                m_lightningSpearResetCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canReset = true;
            }
            else
            {
                m_lightningSpearResetCooldownTimer = m_lightningSpearResetCooldown;
                m_canReset = false;
                EndExecution();
            }
        }

        private IEnumerator LightningSpearChargingRoutine()
        {
            while (true)
            {
                m_state.waitForBehaviour = false;
                m_state.isAttacking = true;
                yield return null;
            }
        }
    }
}
