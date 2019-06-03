using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MeleeBeeDrone : BeeDrone
    {
        private MeleeBeeDroneAnimation m_anim;
        protected override BeeDroneAnimation m_animation => m_anim;

        public void RapidSting()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            
            m_behaviour.SetActiveBehaviour(StartCoroutine(RapidStingRoutine()));
        }

        public void StingerDive()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            
            m_behaviour.SetActiveBehaviour(StartCoroutine(StingerDiveRoutine()));
        }

        private IEnumerator RapidStingRoutine()
        {
            m_waitForBehaviourEnd = true;
            EnableRoot(true, true, false);
            m_movement.Stop();
            m_anim.DoRapidSting();
            yield return new WaitForAnimationComplete(m_animation.animationState, MeleeBeeDroneAnimation.ANIMATION_RAPIDSTING);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);  
        }

        private IEnumerator StingerDiveRoutine()
        {
            m_waitForBehaviourEnd = true;
            EnableRoot(true, true, false);
            m_movement.Stop();
            m_anim.DoStingerDive();
            yield return new WaitForAnimationComplete(m_animation.animationState, MeleeBeeDroneAnimation.ANIMATION_STINGERDIVE);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void Awake()
        {
            base.Awake();
            m_anim = GetComponent<MeleeBeeDroneAnimation>();   
        }
    }
}