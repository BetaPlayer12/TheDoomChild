using System.Collections;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using Spine;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PoisonToad : Minion
    {
        [SerializeField]
        [TabGroup("References")]
        private Hitbox m_attackHitbox;

        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        [MinValue(0)]
        private float m_attackFinishRest;

        private PoisonToadAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private ITurnHandler m_turn;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void LookAt(Vector2 target) => m_turn.LookAt(target);

        public void DoIdle()
        {
            Debug.Log("Do Idle");
        }

        public void Attack(ITarget target)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(AcidAttackRoutine(target)));
            Debug.Log("Attack");
        }

        private IEnumerator AcidAttackRoutine(ITarget target)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            //m_animation.DoAttack();
            yield return null;

            //yield return new WaitForAnimationEvent(m_animation.animationState, EyeBatAnimation.EVENT_SWOOPDOWN);

            //while (m_animation.isAttackAnimationPlaying)
            //{
            //    m_turn.LookAt(target.position);
            //    yield return null;
            //}
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

        private void OnComplete(TrackEntry trackEntry)
        {
            //if (trackEntry.Animation.Name == PoisonToadAnimation.ANIMATION_DEATH)
            //{
            //    gameObject.SetActive(false);
            //}
        }

        protected override void OnDeath()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected override void ResetValues()
        {
            m_attackHitbox.Disable();
        }

        protected override void Awake()
        {
            base.Awake();
            m_animation = GetComponent<PoisonToadAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedObjectPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
        }

        protected override void Start()
        {
            var skeletonData = m_animation.skeletonAnimation.skeleton.Data;
            m_animation.animationState.Complete += OnComplete;
            base.Start();
        }
    }
}
