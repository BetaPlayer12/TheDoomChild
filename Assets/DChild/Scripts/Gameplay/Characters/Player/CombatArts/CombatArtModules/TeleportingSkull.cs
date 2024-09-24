using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DG.Tweening.DOTweenModuleUtils;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class TeleportingSkull : AttackBehaviour
    {
        [SerializeField]
        private Info m_teleportingSkullInfo;

        [SerializeField, BoxGroup("Reference")]
        private SpineRootAnimation m_spineRootAnimation;
        [SerializeField, BoxGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, BoxGroup("Reference")]
        private Crouch m_crouch;
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        [SerializeField, BoxGroup("Physics")]
        private RaySensor m_groundSensor;
        private float m_cacheGravity;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectile;
        public ProjectileInfo projectile => m_projectile;
        private Projectile m_spawnedProjectile;
        public Projectile spawnedProjectile => m_spawnedProjectile;
        private bool m_canTeleport;
        private bool m_hasExecuted;
        public bool canTeleport => m_canTeleport;
        //[SerializeField, BoxGroup("FX")]
        //private ParticleSystem m_teleportFX;
        [SerializeField, BoxGroup("FX")]
        private MaterialReplacementExample m_materialReplacement;

        private int m_teleportingSkullStateAnimationParameter;
        private string m_animationName = "TeleportingSkullEnd";
        private float m_cacheMixDuration;

        public event EventAction<EventActionArgs> Teleported;

        private Coroutine m_teleportingSkullRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_teleportingSkullStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.TeleportingSkull);
            //m_canMove = true;
            //m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
            m_cacheGravity = m_physics.gravityScale;
            m_cacheMixDuration = m_spineRootAnimation.animationState.GetCurrent(0).MixDuration;
        }

        public void DisableTeleport(/*object sender, EventActionArgs eventArgs*/)
        {
            m_canTeleport = false;
            m_spawnedProjectile = null;
        }

        private void TeleportToImpactPoint(object sender, EventActionArgs eventArgs)
        {
            TeleportToProjectile();
        }

        public void GetSpawnedProjectile(Projectile spawnedProjectile)
        {
            if (m_spawnedProjectile == null && m_canTeleport)
            {
                m_spawnedProjectile = spawnedProjectile;
                //m_spawnedProjectile.Impacted += DisableTeleport;
                m_spawnedProjectile.Impacted += TeleportToImpactPoint;
            }
        }

        public void TeleportToProjectile()
        {
            if (m_spawnedProjectile != null && m_canTeleport)
            {
                Teleported?.Invoke(this, EventActionArgs.Empty);
                m_canTeleport = false;
                m_hasExecuted = true;
                m_hitbox.Disable();
                m_spineRootAnimation.EnableRootMotion(true, true);
                m_materialReplacement.replacementEnabled = true;
                m_teleportingSkullInfo.PlayFX(true);
                //m_cacheGravity = m_physics.gravityScale;
                m_physics.gravityScale = 0;
                m_physics.velocity = Vector2.zero;
                m_character.transform.position = Mathf.Abs(RoofPosition(new Vector2(m_spawnedProjectile.transform.position.x, m_spawnedProjectile.transform.position.y + m_spawnedProjectile.GetComponent<CircleCollider2D>().radius)).y - m_spawnedProjectile.transform.position.y) < m_character.height && CanTeleportHere(new Vector2(m_spawnedProjectile.transform.position.x, m_spawnedProjectile.transform.position.y)) ? new Vector3(m_spawnedProjectile.transform.position.x, m_spawnedProjectile.transform.position.y - m_character.height) : m_spawnedProjectile.transform.position;
                m_spawnedProjectile.CallPoolRequest();
                m_spineRootAnimation.animationState.GetCurrent(0).MixDuration = 0f;
                m_teleportingSkullRoutine = StartCoroutine(TeleportingSkullRoutine());
            }
        }

        public void Execute()
        {
            m_canTeleport = true;
        }

        public void EndExecution()
        {
            if (m_teleportingSkullRoutine != null)
            {
                StopCoroutine(m_teleportingSkullRoutine);
                m_teleportingSkullRoutine = null;
            }
            m_spineRootAnimation.DisableRootMotion();
            m_materialReplacement.replacementEnabled = false;
            m_canTeleport = false;
            m_state.waitForBehaviour = false;
            m_physics.gravityScale = m_cacheGravity;
            //m_physics.velocity = Vector2.zero;
            m_teleportingSkullInfo.ShowCollider(false);
            m_spawnedProjectile = null;
            m_spineRootAnimation.animationState.GetCurrent(0).MixDuration = m_cacheMixDuration;
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                m_hitbox.Enable();
            }
            m_groundSensor.Cast();
            if (m_crouch.IsCrouchingPossible() && m_groundSensor.allRaysDetecting)
            {
                m_crouch.Execute();
            }
            base.AttackOver();
            m_animator.SetBool(m_teleportingSkullStateAnimationParameter, false);
        }

        public override void Cancel()
        {
            //m_edgedFuryInfo.PlayFX(false);
            //m_fx.gameObject.SetActive(false);
            //m_fx.Stop();
            if (m_spawnedProjectile == null && m_canTeleport)
            {
                if (m_teleportingSkullRoutine != null)
                {
                    StopCoroutine(m_teleportingSkullRoutine);
                    m_teleportingSkullRoutine = null;
                }
                m_spineRootAnimation.DisableRootMotion();
                m_materialReplacement.replacementEnabled = false;
                m_physics.gravityScale = m_cacheGravity;
                //m_physics.velocity = Vector2.zero;
                m_canTeleport = false;
                m_teleportingSkullInfo.ShowCollider(false);
                m_animator.SetBool(m_teleportingSkullStateAnimationParameter, false);
                m_spawnedProjectile = null;
                m_spineRootAnimation.animationState.GetCurrent(0).MixDuration = m_cacheMixDuration;
                base.Cancel();
                if (m_hasExecuted)
                {
                    m_hasExecuted = false;
                    m_hitbox.Enable();
                }
            }
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_teleportingSkullInfo.ShowCollider(value);
        }

        private IEnumerator TeleportingSkullRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_teleportingSkullStateAnimationParameter, true);
            m_animator.Play(m_animationName);
            yield return null;
        }

        protected Vector2 RoofPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.up, 100f, true, out hitCount, true);
            if (hit != null)
            {
                Debug.DrawRay(startPoint, hit[0].point);
                return hit[0].point;
            }
            return Vector2.zero;
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
        }
        
        protected bool CanTeleportHere(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hitUp = Cast(startPoint, Vector2.up, 100f, true, out hitCount, true);
            RaycastHit2D[] hitDown = Cast(startPoint, Vector2.down, 100f, true, out hitCount, true);
            if (hitUp != null && hitDown != null)
            {
                if ((Vector2.Distance(hitUp[0].point, hitDown[0].point) < m_character.height) && MathF.Abs(m_spawnedProjectile.transform.position.y - hitDown[0].point.y) < m_character.height)
                {
                    return false;
                }
            }

            return true;
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
    }
}
