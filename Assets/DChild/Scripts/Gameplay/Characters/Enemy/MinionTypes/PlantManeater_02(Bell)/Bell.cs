using DChild.Gameplay.Characters.Enemies.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Bell : Minion, IFlinch
    {
        [SerializeField]
        [LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        [SerializeField]
        [TabGroup("Reference")]
        private GameObject m_vine;

        [SerializeField]
        [TabGroup("Reference")]
        private GameObject m_acidSpit;

        [SerializeField]
        [TabGroup("Reference")]
        private Transform m_spitSpawn;

        private BellAnimation m_animation;
        private EnemyFacingOnStart m_enemyFacing;
      
        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => this. m_animation;


        public void AcidSpitAttack(Vector2 target)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(AcidSpiteRooutine(target)));
        }

        public void VineSpawnAttack(Vector2 target)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(VineSpawnRoutine(target)));
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoIdle();
        }

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        public void Turn()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, BellAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, BellAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator AcidSpiteRooutine(Vector2 targetPos)
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoAcidSpit();
            yield return new WaitForAnimationEvent(m_animation.animationState, BellAnimation.EVENT_POISONSPIT);
            var acidSpit = (AcidSpitProjectile)GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_acidSpit);
            var direction = targetPos - (Vector2)m_spitSpawn.position;
            acidSpit.ChangeTrajectory(direction.normalized);
            acidSpit.SetVelocity(direction, 1.5f);
            yield return new WaitForAnimationComplete(m_animation.animationState, BellAnimation.ANIMATION_ACID_SPIT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator VineSpawnRoutine(Vector2 target)
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoVineAttack();
            yield return new WaitForAnimationEvent(m_animation.animationState, BellAnimation.EVENT_VINESPAWN);
            var vine = (Vine)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_vine);
            vine.SpawnAt(target, currentFacingDirection);
            yield return new WaitForAnimationComplete(m_animation.animationState, BellAnimation.ANIMATION_VINE_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, BellAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
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
            m_animation = GetComponent<BellAnimation>();
            m_enemyFacing = GetComponent<EnemyFacingOnStart>();
        }

        protected override void Start()
        {
            base.Start();
            m_enemyFacing.enabled = true;
        }
    }
}
