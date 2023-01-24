using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class SovereignImpale : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_sovereignImpaleCooldown;
        [SerializeField]
        private float m_sovereignImpaleMovementCooldown;
        [SerializeField]
        private Info m_sovereignImpaleInfo;
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

        [SerializeField, BoxGroup("Projectile")]
        private Transform m_startPoint;
        [SerializeField, BoxGroup("Projectile")]
        private float m_spikeDuration;
        [SerializeField, BoxGroup("Projectile")]
        private GameObject m_spike;

        private bool m_canSovereignImpale;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_sovereignImpaleStateAnimationParameter;
        private float m_sovereignImpaleCooldownTimer;
        private float m_sovereignImpaleMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanSovereignImpale() => m_canSovereignImpale;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_sovereignImpaleStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SovereignImpale);
            m_canSovereignImpale = true;
            m_canMove = true;
            m_sovereignImpaleMovementCooldownTimer = m_sovereignImpaleMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            //m_sovereignImpaleInfo.ShowCollider(false);
            m_animator.SetBool(m_sovereignImpaleStateAnimationParameter, false);
        }

        public void Execute()
        {
            //m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canSovereignImpale = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_sovereignImpaleStateAnimationParameter, true);
            m_sovereignImpaleCooldownTimer = m_sovereignImpaleCooldown;
            m_sovereignImpaleMovementCooldownTimer = m_sovereignImpaleMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            base.AttackOver();
            //m_sovereignImpaleInfo.ShowCollider(false);
            m_animator.SetBool(m_sovereignImpaleStateAnimationParameter, false);
            m_canSovereignImpale = true;
            m_canMove = true;
            //m_state.waitForBehaviour = false;
        }

        public override void Cancel()
        {
            base.Cancel();
            //m_sovereignImpaleInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_sovereignImpaleInfo.ShowCollider(value);
            m_attackFX.transform.position = m_sovereignImpaleInfo.fxPosition.position;

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
            if (m_sovereignImpaleCooldownTimer > 0)
            {
                m_sovereignImpaleCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canSovereignImpale = false;
            }
            else
            {
                m_sovereignImpaleCooldownTimer = m_sovereignImpaleCooldown;
                m_state.isAttacking = false;
                m_canSovereignImpale = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_sovereignImpaleMovementCooldownTimer > 0)
            {
                m_sovereignImpaleMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_sovereignImpaleMovementCooldownTimer = m_sovereignImpaleMovementCooldown;
                m_canMove = true;
            }
        }

        private Vector2 GroundPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.down, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

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

        public void Summon()
        {
            LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
        }

        private void LaunchSpike(PuedisYnnusSpike.SkinType spikeType, bool isRandom, Quaternion rotation, bool useOffset)
        {
            var randomOffsetX = UnityEngine.Random.Range(10, 20);
            randomOffsetX = UnityEngine.Random.Range(0, 2) == 1 ? randomOffsetX : randomOffsetX * -1;
            var targetPos = useOffset ? new Vector2(m_startPoint.position.x + randomOffsetX, GroundPosition(m_startPoint.position).y) : (Vector2)m_startPoint.position;

            var component = Instantiate(m_spike, targetPos, Quaternion.identity);
            component.GetComponent<PuedisYnnusSpike>().WillPool(false);
            component.GetComponent<PuedisYnnusSpike>().RandomScale();
            component.GetComponent<PuedisYnnusSpike>().SetSkin(spikeType, isRandom);

            component.transform.rotation = rotation;
            var middleSpawn = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
            if (middleSpawn)
            {
                component.transform.position = new Vector2(m_startPoint.position.x, GroundPosition(m_startPoint.position).y);
                component.GetComponent<PuedisYnnusSpike>().MassiveSpikeSpawn(m_spikeDuration);
            }
            else
            {
                if (component.transform.position.x < m_startPoint.position.x)
                    component.GetComponent<PuedisYnnusSpike>().RightSpikeSpawn(m_spikeDuration);
                else
                    component.GetComponent<PuedisYnnusSpike>().LeftSpikeSpawn(m_spikeDuration);
            }
        }
    }
}
