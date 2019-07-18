using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MushroomMan : Minion, IFlinch
    {
        [SerializeField]
        [LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        private MushroomManAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private SpineRootMotion m_rootMotion;

        private ISensorFaceRotation[] m_sensorRotator;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(true, true, false);
            m_animation.DoMove();
        }

        public void MoveTo()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(true, true, false);
            m_animation.DoMove();
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(false, true, false);
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Attack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoAttack();
            m_behaviour.SetActiveBehaviour(StartCoroutine(AttackRoutine()));
        }

        public void Turn()
        {
            if (m_facing == HorizontalDirection.Left)
            {
                SetFacing(HorizontalDirection.Left);
            }
            else
            {
                SetFacing(HorizontalDirection.Right);
            }
            TurnCharacter();
            RotateSensor();
        }

        public void Flinch(RelativeDirection damageSource, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator AttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack();
            yield return new WaitForAnimationComplete(animation.animationState, MushroomManAnimation.ANIMATION_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, MushroomManAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, MushroomManAnimation.ANIMATION_DEATH);
            Destroy(this.gameObject);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void EnableRootMotion(bool enable, bool useX, bool useY)
        {
            m_rootMotion.enabled = enable;
            if (enable)
            {
                m_rootMotion.useX = useX;
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
            m_animation = GetComponent<MushroomManAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
        }
    }
}
