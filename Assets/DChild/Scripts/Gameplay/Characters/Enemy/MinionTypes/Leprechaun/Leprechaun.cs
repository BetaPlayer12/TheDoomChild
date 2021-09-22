using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Leprechaun : Minion, IFlinch
    {
        [SerializeField, TitleGroup("References")]
        private GameObject m_potOfGold;

        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        private LeprechaunAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void MoveTo(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoMove();
            m_movement.MoveOnGround(targetPos, m_moveSpeed);
        }

        public void CaneWhackAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(CaneWhackAttackRoutine()));
        }

        public void SummonGoldAttack(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            var target = new Vector2(targetPos.x, targetPos.y + 20f);
            m_behaviour.SetActiveBehaviour(StartCoroutine(SummonGoldAttackRoutine(target)));
        }

        public void Detect()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(DetectRoutine()));
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

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
            yield return new WaitForAnimationComplete(m_animation.animationState, LeprechaunAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, LeprechaunAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DetectRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDetectPlayer();
            yield return new WaitForAnimationComplete(m_animation.animationState, LeprechaunAnimation.ANIMATION_DETECTPLAYER);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator CaneWhackAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoCaneWhackAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, LeprechaunAnimation.ANIMATION_CANE_WHACK_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SummonGoldAttackRoutine(Vector2 targetSpawn)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoSummonGoldAttack();
            yield return new WaitForAnimationEvent(m_animation.animationState, LeprechaunAnimation.EVENT_SUMMON_POT);
            var potGold = (PotofGold)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_potOfGold);
            potGold.transform.parent = null;
            potGold.SpawnAt(targetSpawn);
            yield return new WaitForAnimationComplete(m_animation.animationState, LeprechaunAnimation.ANIMATION_SUMMON_GOLD_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, LeprechaunAnimation.ANIMATION_DEATH);
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
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_animation = GetComponent<LeprechaunAnimation>();
        }      
    }
}
