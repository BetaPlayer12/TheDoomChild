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
    public class AirSlashRange : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_airSlashRangeCooldown;
        [SerializeField]
        private float m_airSlashRangeMovementCooldown;
        [SerializeField]
        private float m_airSlashRangeResetCooldown;
        [SerializeField]
        private Info m_airSlashRangeInfo;
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

        private bool m_canAirSlashRange;
        private bool m_canMove;
        private bool m_canReset;
        private IPlayerModifer m_modifier;
        private int m_airSlashRangeStateAnimationParameter;
        private float m_airSlashRangeCooldownTimer;
        private float m_airSlashRangeMovementCooldownTimer;
        private float m_airSlashRangeResetCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanAirSlashRange() => m_canAirSlashRange;
        public bool CanMove() => m_canMove;
        public bool CanReset() => m_canReset;
        private bool m_hasExecuted;

        private Coroutine m_airSlashRangeChargingRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_airSlashRangeStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AirSlashRange); ;
            m_canAirSlashRange = true;
            m_canMove = true;
            m_airSlashRangeMovementCooldownTimer = m_airSlashRangeMovementCooldown;
            m_airSlashRangeResetCooldownTimer = m_airSlashRangeResetCooldown;

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
            //m_airSlashRangeInfo.ShowCollider(false);
            m_animator.SetBool(m_airSlashRangeStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_hasExecuted = true;
            m_state.waitForBehaviour = true; // Temporary (Delete after)
            //m_airSlashRangeChargingRoutine = StartCoroutine(airSlashRangeChargingRoutine()); //Temporary Disabled
            //m_state.waitForBehaviour = false; //Temporary Disabled
            m_state.isAttacking = true;
            //m_characterState.isChargingairSlashRange = true; //Temporary Disabled
            m_canAirSlashRange = false;
            m_canMove = false;
            //m_canReset = true; //Temporary Disabled
            m_canReset = false; // Temporary (Delete after)
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
            m_physics.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_airSlashRangeStateAnimationParameter, true);
            m_airSlashRangeCooldownTimer = m_airSlashRangeCooldown;
            m_airSlashRangeMovementCooldownTimer = m_airSlashRangeMovementCooldown;
            m_airSlashRangeResetCooldownTimer = m_airSlashRangeResetCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage)); //Temporary Disabled
        }

        public void ReleaseHold()
        {
            if (m_airSlashRangeChargingRoutine != null)
            {
                StopCoroutine(m_airSlashRangeChargingRoutine);
                m_airSlashRangeChargingRoutine = null;
            }
            m_state.waitForBehaviour = true;
            //m_characterState.isChargingAirSlashRange = false;
            m_animator.SetBool(m_airSlashRangeStateAnimationParameter, false);

            m_airSlashRangeResetCooldownTimer = m_airSlashRangeResetCooldown;
            m_canReset = false;
        }

        public void SummonAirSlash()
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
            //m_characterState.isChargingAirSlashRange = false;
            //m_airSlashRangeInfo.ShowCollider(false);
            //m_canMove = true;
            if (m_airSlashRangeChargingRoutine != null)
            {
                StopCoroutine(m_airSlashRangeChargingRoutine);
                m_airSlashRangeChargingRoutine = null;
            }
            m_animator.SetBool(m_airSlashRangeStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                //m_airSlashRangeInfo.ShowCollider(false);
                m_fxAnimator.Play("Buffer");
                StopAllCoroutines();
                m_physics.gravityScale = m_cacheGravity;
                //m_characterState.isChargingAirSlashRange = false;
                if (m_airSlashRangeChargingRoutine != null)
                {
                    StopCoroutine(m_airSlashRangeChargingRoutine);
                    m_airSlashRangeChargingRoutine = null;
                }
                m_animator.SetBool(m_airSlashRangeStateAnimationParameter, false);
                base.Cancel();
            }
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_airSlashRangeInfo.ShowCollider(value);
            m_attackFX.transform.position = m_airSlashRangeInfo.fxPosition.position;
            if (!value)
                m_fxAnimator.Play("Buffer");
        }

        public void HandleAttackTimer()
        {
            if (m_airSlashRangeCooldownTimer > 0)
            {
                m_airSlashRangeCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canAirSlashRange = false;
            }
            else
            {
                m_airSlashRangeCooldownTimer = m_airSlashRangeCooldown;
                //m_state.isAttacking = false;
                m_canAirSlashRange = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_airSlashRangeMovementCooldownTimer > 0)
            {
                m_airSlashRangeMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                m_airSlashRangeMovementCooldownTimer = m_airSlashRangeMovementCooldown;
                m_canMove = true;
            }
        }

        public void HandleResetTimer()
        {
            if (m_airSlashRangeResetCooldownTimer > 0)
            {
                m_airSlashRangeResetCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canReset = true;
            }
            else
            {
                m_airSlashRangeResetCooldownTimer = m_airSlashRangeResetCooldown;
                m_canReset = false;
                EndExecution();
            }
        }

        private IEnumerator AirSlashRangeChargingRoutine()
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
