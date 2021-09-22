using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies.Collections;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GemCrawler : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField, TitleGroup("References")]
        private GameObject m_gemSpike;

        [SerializeField, TitleGroup("Stat")]
        private Damage m_damage;
        [SerializeField, TitleGroup("Enemy"), Min(0)]
        private float m_scoutDuration;

        private GemCrawlerAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;


        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void Turn()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Patrol()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_rootMotion.enabled = true;
            m_animation.DoMove();
        }

        public void Move()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_rootMotion.enabled = true;
            m_animation.DoMove();
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_rootMotion.enabled = true;
            m_animation.DoIdle();
        }

        public void CreateGemSpike(Vector2 target)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(CreateGemSpikeRoutine(target)));
        }

        public void Scout()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(ScoutRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator CreateGemSpikeRoutine(Vector2 target)
        {
            m_waitForBehaviourEnd = true;
            //m_rootMotion.enabled = false;
            //m_movement.Stop();
            m_animation.DoCrystalSpike();
            yield return new WaitForAnimationEvent(m_animation.animationState, GemCrawlerAnimation.EVENT_CRYSTALSPIKE);
            var gemSpike = (GemSpike)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_gemSpike);
            gemSpike.transform.parent = null;
            gemSpike.SpawnAt(target, Quaternion.identity, currentFacingDirection);
            yield return new WaitForAnimationComplete(m_animation.animationState, GemCrawlerAnimation.ANIMATION_ATTACK);
            StopActiveBehaviour();
        }

        private IEnumerator ScoutRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoIdle();
            yield return m_scoutWait;
            StopActiveBehaviour();
        }

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, GemCrawlerAnimation.ANIMATION_DEATH);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, GemCrawlerAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            StopActiveBehaviour();
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, GemCrawlerAnimation.ANIMATION_FLINCH);
            StopActiveBehaviour();
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
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedObjectPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<GemCrawlerAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
