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
        [SerializeField]
        private Camera m_camera;
        [SerializeField]
        private LayerMask m_mask;

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
        [SerializeField, BoxGroup("Projectile")]
        private float m_summonRange;


        private bool m_canNinthCircleSanction;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_ninthCircleSanctionStateAnimationParameter;
        private float m_ninthCircleSanctionCooldownTimer;
        private float m_ninthCircleSanctionMovementCooldownTimer;

        //[SerializeField, BoxGroup("FX")]
        //private Animator m_fxAnimator;
        //[SerializeField, BoxGroup("FX")]
        //private float m_fxDuration;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_pooledFX;
        [SerializeField, BoxGroup("FX")]
        private Transform m_fxStartpoint;
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

            //m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();

            m_startPointCache = m_startPoint.localPosition;
            m_launcher = new ProjectileLauncher(m_projectileInfo, m_startPoint);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public void ActivateCullingMask()
        {
            m_camera.cullingMask = m_mask;
        }

        public override void Reset()
        {
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            m_canNinthCircleSanction = true;
            m_canMove = true;
            //m_ninthCircleSanctionInfo.ShowCollider(false);
            //StopCircleFX();
            m_animator.SetBool(m_ninthCircleSanctionStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            //StopAllCoroutines();
            //StartCoroutine(CircleFXDurationRoutine());
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canNinthCircleSanction = false;
            m_canMove = false;
            SummonFX();
            //m_fxAnimator.SetTrigger("CircleMagicTrigger");
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
            //StopCircleFX();
            //m_ninthCircleSanctionInfo.ShowCollider(false);
            //m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_ninthCircleSanctionStateAnimationParameter, false);
            base.Cancel();
        }

        //private IEnumerator CircleFXDurationRoutine()
        //{
        //    m_ninthCircleSanctionInfo.PlayFX(true);
        //    ActivateCullingMask();
        //    yield return new WaitForSeconds(m_fxDuration);
        //    StopCircleFX();
        //    yield return null;
        //}

        //public void StopCircleFX()
        //{
        //    m_ninthCircleSanctionInfo.PlayFX(false);
        //    m_ninthCircleSanctionInfo.ClearFX();
        //    m_fxAnimator.Play("CircleWait");
        //}

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
                //m_state.isAttacking = false;
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

        protected Vector2 GroundPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.down, m_summonRange, true, out hitCount, true);
            if (hit != null)
            {
                Debug.DrawRay(startPoint, hit[0].point);
                return hit[0].point;
            }
            return Vector2.zero;
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
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
            return hitCount == 0 ? null : m_hitResults;
        }

        public void SummonFX()
        {
            var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_pooledFX);
            instance.transform.position = m_fxStartpoint.position;
            instance.transform.localScale = m_character.facing == HorizontalDirection.Right ? Vector3.one : new Vector3(-1, 1, 1);
        }

        public void Summon()
        {

            m_enemySensor.Cast();
            var hits = m_enemySensor.GetHits();
            var target = m_enemySensor.isDetecting ? hits[0].point : Vector2.zero;
            m_startPoint.position = target;
            //m_launcher.AimAt(new Vector2(target.x, GroundPosition(target).y));
            if (target != Vector2.zero)
            {
                //m_launcher.LaunchProjectile();
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
                instance.transform.position = new Vector2(m_startPoint.position.x, GroundPosition(m_startPoint.position).y);
                instance.transform.rotation = Quaternion.identity;
                instance.GetComponent<Attacker>().SetParentAttacker(m_attacker);
            }
            m_startPoint.localPosition = m_startPointCache;
        }
    }
}
