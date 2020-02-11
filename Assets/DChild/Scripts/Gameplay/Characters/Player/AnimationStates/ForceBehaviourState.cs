using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class ForceBehaviourState : StateMachineBehaviour
    {
        public string m_landParameter;

        private IBehaviourState m_behaviourState;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            m_behaviourState = animator.GetComponentInParent<IBehaviourState>();

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            m_behaviourState.waitForBehaviour = false;
            animator.ResetTrigger(m_landParameter);

        }
    }
}