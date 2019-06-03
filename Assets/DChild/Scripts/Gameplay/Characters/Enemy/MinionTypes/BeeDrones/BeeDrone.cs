using System.Collections;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class BeeDrone : Minion, IMovingEnemy, IFlinch
    {
        [SerializeField]
        protected AttackDamage m_damage;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_flightSpeed;

        protected PhysicsMovementHandler2D m_movement;
        protected SpineRootMotion m_rootMotion;
        protected abstract BeeDroneAnimation m_animation { get; }
        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol(Vector2 destination)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRoot(false, true, true);
            m_animation.DoForward();
            m_movement.MoveTo(destination, m_patrolSpeed);
        }

        public void MoveTo(Vector2 destination)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRoot(false, true, true);
            m_animation.DoForward();
            m_movement.MoveTo(destination, m_flightSpeed);
        }

        
        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoIdle2();
            m_movement.Stop();
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection damageSource, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, BeeDroneAnimation.ANIMATION_DAMAGE);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, BeeDroneAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, BeeDroneAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected void EnableRoot(bool enable, bool useX, bool useY)
        {
            m_rootMotion.enabled = enable;
            if (enable)
            {
                m_rootMotion.useX = useX;
                m_rootMotion.useY = useY;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
        }
    }
}