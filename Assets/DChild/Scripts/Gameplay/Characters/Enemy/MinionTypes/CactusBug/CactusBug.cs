using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CactusBug : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_attackDamage;

        private ISensorFaceRotation[] m_sensorRotator;
        private CactusBugAnimation m_animation;
        private SpineRootMotion m_spineRoot;
        protected override Damage startDamage => m_attackDamage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Move()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_spineRoot.SetSourceBone("master controller");
            m_animation.DoMove();
        }

        public void SpitAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(SpitAttackRoutine()));
        }

        public void JumpAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_spineRoot.SetSourceBone("root");
            m_behaviour.SetActiveBehaviour(StartCoroutine(JumpAttackRoutine()));
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoIdle();
        }

        public void Burrow()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(BurrowRoutine()));
        }

        public void BurrowIdle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoBurrowIdle();
        }

        public void BurrowReveal()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(BurrowRevealRoutine()));
        }

        public void Flinch(RelativeDirection damageSource, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SpitAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoSpitAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_SPIT_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator JumpAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoJumpAnticipation();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_JUMP_ANTICIPATION);
            m_animation.DoJumpAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_JUMP_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            RotateSensor();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator BurrowRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoBurrow();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_BURROW);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator BurrowRevealRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoBurrowReveal();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_BURROW_REVEAL);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, CactusBugAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
            }
        }

        protected override void Start()
        {
            base.Start();
            m_animation = GetComponent<CactusBugAnimation>();
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
            m_spineRoot = GetComponentInChildren<SpineRootMotion>();
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
