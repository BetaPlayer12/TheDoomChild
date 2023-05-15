using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class ReaperHarvest : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_reaperHarvestCooldown;
        [SerializeField]
        private float m_reaperHarvestMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private Info m_reaperHarvestInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("Animations")]
        private ReaperHarvestAnimation m_reaperHarvestAnimation;
        [SerializeField]
        private Hitbox m_hitbox;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canReaperHarvest;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_reaperHarvestStateAnimationParameter;
        private float m_reaperHarvestCooldownTimer;
        private float m_reaperHarvestMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanReaperHarvest() => m_canReaperHarvest;
        public bool CanMove() => m_canMove;
        private bool m_hasExecuted;

        private ReaperHarvestState m_currentState;

        public enum ReaperHarvestState
        {
            Grounded,
            Midair,
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_reaperHarvestStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ReaperHarvest);
            m_canReaperHarvest = true;
            m_canMove = true;
            m_reaperHarvestMovementCooldownTimer = m_reaperHarvestMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_cacheGravity = m_physics.gravityScale;
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            m_canReaperHarvest = true;
            m_canMove = true;
            m_reaperHarvestInfo.ShowCollider(false);
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute(ReaperHarvestState state)
        {
            m_hasExecuted = true;
            m_state.waitForBehaviour = true;
            m_currentState = state;
            StopAllCoroutines();
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canReaperHarvest = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, true);
            m_reaperHarvestCooldownTimer = m_reaperHarvestCooldown;
            m_reaperHarvestMovementCooldownTimer = m_reaperHarvestMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));

            m_physics.velocity = Vector2.zero;
            m_reaperHarvestAnimation.gameObject.SetActive(true);
            switch (m_currentState)
            {
                case ReaperHarvestState.Grounded:
                    m_reaperHarvestAnimation.StartGrounded();
                    break;
                case ReaperHarvestState.Midair:
                    m_cacheGravity = m_physics.gravityScale;
                    m_physics.gravityScale = 0;
                    m_reaperHarvestAnimation.StartMidair();
                    break;
            }
        }

        public void EndExecution()
        {
            m_reaperHarvestInfo.ShowCollider(false);
            //m_canReaperHarvest = true;
            m_canMove = true;
            m_reaperHarvestAnimation.Disable();
            m_physics.gravityScale = m_cacheGravity;
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                m_hitbox.Enable();
            }
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            m_reaperHarvestInfo.ShowCollider(false);
            m_canMove = true;
            m_fxAnimator.Play("Buffer");
            StopAllCoroutines();
            m_reaperHarvestAnimation.Disable();
            m_physics.gravityScale = m_cacheGravity;
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                m_hitbox.Enable();
            }
            m_animator.SetBool(m_reaperHarvestStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_reaperHarvestInfo.ShowCollider(value);
            m_attackFX.transform.position = m_reaperHarvestInfo.fxPosition.position;
        }

        public void StartDash()
        {
            StartCoroutine(DashRoutine());
        }

        public void HandleAttackTimer()
        {
            if (m_reaperHarvestCooldownTimer > 0)
            {
                m_reaperHarvestCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canReaperHarvest = false;
            }
            else
            {
                m_reaperHarvestCooldownTimer = m_reaperHarvestCooldown;
                //m_state.isAttacking = false;
                m_canReaperHarvest = true;
            }
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_reaperHarvestMovementCooldownTimer > 0)
        //    {
        //        m_reaperHarvestMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_reaperHarvestMovementCooldownTimer = m_reaperHarvestMovementCooldown;
        //        m_canMove = true;
        //    }
        //}

        private IEnumerator DashRoutine()
        {
            m_state.waitForBehaviour = true;
            switch (m_currentState)
            {
                case ReaperHarvestState.Grounded:
                    m_reaperHarvestAnimation.ImpactGrounded();
                    break;
                case ReaperHarvestState.Midair:
                    m_reaperHarvestAnimation.ImpactMidair();
                    break;
            }
            //m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            m_hitbox.Disable();
            var timer = m_dashDuration;
            while (timer >= 0 && !m_wallSensor.allRaysDetecting /*&& m_edgeSensor.isDetecting*/)
            {
                m_wallSensor.Cast();
                m_edgeSensor.Cast();
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, 0);
                timer -= Time.deltaTime;
                if (m_currentState == ReaperHarvestState.Grounded)
                {
                    if (!m_edgeSensor.isDetecting)
                    {
                        timer = -1;
                    }
                }
                if (LookTransform(m_character.centerMass, 5f)?.GetComponent<LocationSwitcher>())
                    timer = -1;
                yield return null;
            }
            //yield return new WaitForSeconds(m_dashDuration);
            if (!LookTransform(m_character.centerMass, 5f)?.GetComponent<LocationSwitcher>() && m_hasExecuted)
            {
                m_hitbox.Enable();
                m_hasExecuted = false;
            }
            m_physics.velocity = Vector2.zero;
            //m_reaperHarvestAnimation.Disable();
            //m_state.waitForBehaviour = false;
            yield return null;
        }

        protected Transform LookTransform(Transform startPoint, float distance)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint.position, startPoint.right, distance, false, out hitCount, true);
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
