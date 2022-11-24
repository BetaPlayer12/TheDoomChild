using DChild.Gameplay.Characters.Players.State;
using UnityEngine;
using UnityEngine.Animations;

namespace DChild.Gameplay.Characters.Players
{

    public class WaitBehaviourState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            var state = animator.GetComponentInParent<IBehaviourState>();
            state.waitForBehaviour = false;
        }
    }
}