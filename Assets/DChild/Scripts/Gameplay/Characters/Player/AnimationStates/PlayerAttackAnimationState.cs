using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerAttackAnimationState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
            base.OnStateExit(animator, stateInfo, layerIndex);

            var state = animator.GetComponentInParent<ICombatState>();
            state.canAttack = true;
            state.waitForBehaviour = false;
        }
    }
}