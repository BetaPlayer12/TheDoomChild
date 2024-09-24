using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class BackDiver : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_backDiverCooldown;
        [SerializeField]
        private float m_backDiverMovementCooldown;
        [SerializeField]
        private Info m_backDiverInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_enemySensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_backWallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField]
        private Hitbox m_hitbox;
        [SerializeField]
        private float m_hitboxDuration;

        [SerializeField]
        private Vector2 m_pushForce;
        [SerializeField]
        private float m_landDuration;

        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectileInfo;
        public ProjectileInfo projectile => m_projectileInfo;
        private Projectile m_spawnedProjectile;
        public Projectile spawnedProjectile => m_spawnedProjectile;
        [SerializeField, BoxGroup("Projectile")]
        private Vector2 m_targetOffset;

        private ProjectileLauncher m_launcher;


        private bool m_canBackDiver;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_backDiverStateAnimationParameter;
        private int m_groundStateAnimationParameter;
        private float m_backDiverCooldownTimer;
        private float m_backDiverMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        private Coroutine m_landDurationRoutine;

        public bool CanBackDiver() => m_canBackDiver;
        //public bool CanMove() => m_canMove;
        private bool m_hasExecuted;
        public bool HaveSpacetoExecute()
        {
            m_backWallSensor.Cast();
            return !m_backWallSensor.isDetecting;
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_backDiverStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.BackDiver);
            m_groundStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrounded);
            m_canBackDiver = true;
            m_canMove = true;
            m_backDiverMovementCooldownTimer = m_backDiverMovementCooldown;

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
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            m_canBackDiver = true;
            m_canMove = true;
            //m_backDiverInfo.ShowCollider(false);
            m_animator.SetBool(m_backDiverStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            m_hasExecuted = true;
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canBackDiver = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_backDiverStateAnimationParameter, true);
            m_backDiverCooldownTimer = m_backDiverCooldown;
            m_backDiverMovementCooldownTimer = m_backDiverMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_physics.velocity = Vector2.zero;
            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Left ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            StartCoroutine(HitboxRoutine());
        }

        public void EndExecution()
        {
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
            }
            if (m_landDurationRoutine != null)
            {
                StopCoroutine(m_landDurationRoutine);
                m_landDurationRoutine = null;
                m_physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            //m_backDiverInfo.ShowCollider(false);
            //m_hitbox.Enable();
            //m_canBackDiver = true;
            //m_canMove = true;
            m_animator.SetBool(m_backDiverStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_backDiverInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                m_hitbox.Enable();
            }
            if (m_landDurationRoutine != null)
            {
                StopCoroutine(m_landDurationRoutine);
                m_landDurationRoutine = null;
                m_physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            m_animator.SetBool(m_backDiverStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_backDiverInfo.ShowCollider(value);
            m_attackFX.transform.position = m_backDiverInfo.fxPosition.position;
        }

        public void CheckGround()
        {
            m_groundSensor.Cast();
            if (m_groundSensor.allRaysDetecting)
            {
                m_animator.SetBool(m_groundStateAnimationParameter, true);
            }
        }

        public void LandOnGround()
        {
            m_landDurationRoutine = StartCoroutine(LandRoutine());
        }

        public void ResetBackDiver()
        {
            m_canBackDiver = true;
        }

        public void HandleAttackTimer()
        {
            if (m_backDiverCooldownTimer > 0)
            {
                m_backDiverCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canBackDiver = false;
            }
            else
            {
                m_backDiverCooldownTimer = m_backDiverCooldown;
                //m_state.isAttacking = false;
                m_canBackDiver = true;
            }
        }

        private IEnumerator HitboxRoutine()
        {
            m_hitbox.Disable();
            var timer = m_hitboxDuration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                if (LookTransform(m_character.centerMass, 5f)?.GetComponent<LocationSwitcher>())
                    timer = 0;
            }
            //yield return new WaitForSeconds(m_hitboxDuration);
            if (!LookTransform(m_character.centerMass, 5f)?.GetComponent<LocationSwitcher>() && m_hasExecuted)
            {
                m_hitbox.Enable();
                m_hasExecuted = false;
            }
            yield return null;
        }

        private IEnumerator LandRoutine()
        {
            m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            m_physics.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            yield return new WaitForSeconds(m_landDuration);
            m_physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            yield return null;
        }

        public void Summon()
        {
            m_spawnedProjectile = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
            m_spawnedProjectile.transform.position = m_startPoint.position;
            m_spawnedProjectile.GetComponent<Attacker>().SetParentAttacker(m_attacker);

            //LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
            m_launcher.AimAt(new Vector2(m_startPoint.position.x + (m_character.facing == HorizontalDirection.Right ? m_targetOffset.x : -m_targetOffset.x), m_startPoint.position.y + m_targetOffset.y));
            m_launcher.LaunchProjectile(m_startPoint.right, m_spawnedProjectile.gameObject);
        }

        protected Transform LookTransform(Transform startPoint, float distance)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint.position, -startPoint.right, distance, false, out hitCount, true);
            Debug.DrawRay(startPoint.position, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].transform;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(LayerMask.GetMask("PlayerOnly"));
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        protected static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        }
    }
}
