using System.Collections;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBeetle : Minion
    {
        [SerializeField]
        [TabGroup("Movement")]
        private float m_patrolSpeed;
        [SerializeField]
        [TabGroup("Movement")]
        private float m_moveSpeed;

        [SerializeField]
        [TabGroup("Attack")]
        [MinValue(0)]
        private float m_attackFinishRest;

        [SerializeField]
        private Damage m_damage;

        private PhysicsMovementHandler2D m_movement;
        private ITurnHandler m_turn;
        private GiantBeetleAnimation m_animation;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void LookAt(Vector2 target) => m_turn.LookAt(target);

        public void Patrol(Vector2 destination)
        {
            m_movement.MoveTo(destination, m_patrolSpeed);
            //PatrolAnim
        }

        public void MoveToDestination(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
            //FastWalkAnimation
        }

        public void Attack(ITarget target)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(BiteAttackRoutine(target)));
        }

        private void FlinchPatrol()
        {
            m_movement.Stop();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator BiteAttackRoutine(ITarget target)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            yield return null;
            StopActiveBehaviour();
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            //m_animation.DoDeath();
            yield return null;
            StopActiveBehaviour();
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            //m_animation.DoDeath();
            yield return null;
            StopActiveBehaviour();

        }

        protected override void OnDeath()
        {
             StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected override void Awake()
        {
            base.Awake();
            m_animation = GetComponentInChildren<GiantBeetleAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedObjectPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
        }
    }

}
