using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies.Collections;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Slug : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        private float m_scoutDuration;

        [SerializeField]
        [TabGroup("Spike Projectile Reference")]
        private GameObject m_projectileGO;
        [SerializeField]
        [TabGroup("Spike Projectile Reference")]
        private Transform m_projectileSpawnPosition;

        [SerializeField]
        [TabGroup("Acid Spit Reference")]
        private GameObject m_acidSpitGO;
        [SerializeField]
        [TabGroup("Acid Spit Reference")]
        private Transform m_spitSpawn;

        [SerializeField]
        [TabGroup("Slime Reference")]
        private Transform m_slimePos;
        [SerializeField]
        [TabGroup("Slime Reference")]
        private GameObject m_slimeCollider;

        private SlugAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private PhysicsMovementHandler2D m_movement;
        private WaitForWorldSeconds m_scoutTime;
        private ISensorFaceRotation[] m_sensorRotator;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        #region "Basic Behaviors"

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableRootMotion(false, false);
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Move()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableRootMotion(true, false);
            m_animation.DoMove();
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Scout()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(ScoutRoutine()));
        }
        #endregion

        #region "Attack Behaviors"
        public void Spit(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(SpitRoutine(targetPos)));
        }

        public void SpikeProjectiles()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(SpikeProjectileRoutine()));
        }
        #endregion

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, SlugAnimation.ANIMATION_DAMAGE);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, SlugAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            RotateSensor();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ScoutRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoIdle();
            yield return m_scoutTime;
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SpitRoutine(Vector2 targetPos)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoSpit();
            yield return new WaitForWorldSeconds(0.5f);//needs event for spitting
            var acidSpit = (AcidSpitProjectile)GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_acidSpitGO);
            acidSpit.transform.parent = null;
            var direction = targetPos - (Vector2)m_spitSpawn.position;
            acidSpit.ChangeTrajectory(direction.normalized);
            acidSpit.Launch(direction, 2f);
            yield return new WaitForAnimationComplete(m_animation.animationState, SlugAnimation.ANIMATION_SPIT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SpikeProjectileRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoProjectiles();
            yield return new WaitForWorldSeconds(0.5f);//needs event for projectile
            var spike = (SpikeProjectile)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_projectileGO);
            spike.transform.parent = null;
            spike?.SpawnAt(m_projectileSpawnPosition.position, currentFacingDirection);
            yield return new WaitForAnimationComplete(m_animation.animationState, SlugAnimation.ANIMATION_SPIKE_PROJECTILES);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, SlugAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        public void EnableRootMotion(bool enable, bool useY)
        {
            m_rootMotion.enabled = enable;

            if (enable)
            {
                m_rootMotion.useY = useY;
            }
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<ShiftingCharacterPhysics2D>(), transform);
            m_animation = GetComponent<SlugAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
        }

        protected override void Start()
        {
            base.Start();
            m_scoutTime = new WaitForWorldSeconds(m_scoutDuration);
        }
    }
}
