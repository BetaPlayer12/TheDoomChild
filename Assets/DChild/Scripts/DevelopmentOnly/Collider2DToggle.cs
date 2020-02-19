
using UnityEngine;

namespace DChild.StateMachine
{
    public class Collider2DToggle : StateMachineBehaviour
    {
        public enum WhenType
        {
            Enter,
            Exit
        }

        public WhenType whenToChange;
        public bool enableColliders;

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (whenToChange == WhenType.Enter)
            {
                ToggleColliderStates(animator);
            }
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (whenToChange == WhenType.Exit)
            {
                ToggleColliderStates(animator);

            }
        }

        private void ToggleColliderStates(Animator animator)
        {
            var colliders = animator.GetComponentsInChildren<Collider2D>();
            var length = colliders?.Length ?? 0;
            for (int i = 0; i < length; i++)
            {
                colliders[i].enabled = enableColliders;
            }
        }
    }
}