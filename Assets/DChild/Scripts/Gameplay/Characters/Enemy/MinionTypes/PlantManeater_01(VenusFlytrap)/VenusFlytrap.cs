using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class VenusFlytrap : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        private VenusFlytrapAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private ITurnHandler m_turn;
        private EnemyFacingOnStart m_facingOnStart;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => this.m_animation;

        public void BiteAttack(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(BiteAttackRoutine()));
        }

        public void WhipAttack(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(WhipAttackRoutine()));
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
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

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, VenusFlytrapAnimation.ANIMATION_TURN);
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
            yield return new WaitForAnimationComplete(m_animation.animationState, VenusFlytrapAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator WhipAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoWhipAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, VenusFlytrapAnimation.ANIMATION_WHIPATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator BiteAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoBiteAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, VenusFlytrapAnimation.ANIMATION_BITEATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, VenusFlytrapAnimation.ANIMATION_DEATH);
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
            m_animation = GetComponent<VenusFlytrapAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_facingOnStart = GetComponent<EnemyFacingOnStart>();
        }

        protected override void Start()
        {
            base.Start();
            m_facingOnStart.enabled = true;
        }
    }
}
