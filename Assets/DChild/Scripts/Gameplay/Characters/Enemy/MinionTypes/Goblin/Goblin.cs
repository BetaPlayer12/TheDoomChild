using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Goblin : Minion, IFlinch
    {
        [SerializeField]
        [MinValue(1f)]
        [TabGroup("Movement")]
        private float m_retreatSpeed;

        [SerializeField]
        [MinValue(1f)]
        [TabGroup("Movement")]
        private float m_moveSpeed;

        [SerializeField]
        [MinValue(0f)]
        [TabGroup("Attack")]
        private float m_attackFinishRest;

        [SerializeField]
        private Damage m_damage;

        private GoblinAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private ITurnHandler m_turn;

        protected override CombatCharacterAnimation animation => m_animation;

        public bool isAttacking => m_animation.isAttackingAnimationPlaying;

        protected override Damage startDamage => m_damage;

        public void LookAt(Vector2 target) => m_turn.LookAt(target);

        public void Retreat(Vector2 destination)
        {
            m_movement.MoveTo(destination, m_retreatSpeed);
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(RetreatRoutine(destination)));
        }

        public void TossCoin()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TossCoinRoutine()));
        }

        public void HeadButt()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(HeadButtRoutine()));
        }

        public void Stay()
        {
            m_movement.Stop();
            //IdleAnim
        }
        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(WaitRoutine()));
        }

        private IEnumerator WaitRoutine()
        {
            m_waitForBehaviourEnd = true;
            yield return new WaitForSeconds(1f);
            m_behaviour.SetActiveBehaviour(null);
            m_waitForBehaviourEnd = false;
        }

        private IEnumerator RetreatRoutine(Vector2 destination)
        {
            yield return new WaitForSeconds(1f);
            StopActiveBehaviour();
            m_waitForBehaviourEnd = true;
            yield return null;
        }

        private IEnumerator TossCoinRoutine()
        {
            //HeadBUtt
            yield return null;
        }

        private IEnumerator HeadButtRoutine()
        {
            //HeadButt Animation
            yield return null;
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            //Do animation death
            yield return null;
            m_behaviour.SetActiveBehaviour(null);
            m_waitForBehaviourEnd = false;
        }


        protected override void OnDeath()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }


        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<ObjectPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<GoblinAnimation>();
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
