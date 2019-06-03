using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Mummy01 : Mummy
    {
        private Mummy01Animation m_anim;
        protected override MummyAnimation m_animation => m_anim;

        public void StabAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoWhip();
            m_behaviour.SetActiveBehaviour(StartCoroutine(StabAttackRoutine()));
        }

        private IEnumerator StabAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_anim.DoStab();
            yield return new WaitForAnimationComplete(m_animation.animationState, Mummy01Animation.ANIMATION_STAB_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void Awake()
        {
            base.Awake();
            m_anim = GetComponent<Mummy01Animation>();
        }
    }
}
