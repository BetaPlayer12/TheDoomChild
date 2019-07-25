
using UnityEngine;
using Spine.Unity.Modules;

namespace DChild
{
    public class SpineRootMotionState : StateMachineBehaviour
    {
        public enum Option
        {
            X_Only,
            Y_Only,
            Both_X_Y
        }

        public Option option = Option.Both_X_Y;

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var rootMotion = animator.GetComponentInChildren<SpineRootMotion>();
            rootMotion.enabled = true;

            switch (option)
            {
                case Option.Both_X_Y:
                    rootMotion.useX = true;
                    rootMotion.useY = true;
                    break;
                case Option.X_Only:
                    rootMotion.useX = true;
                    rootMotion.useY = false;
                    break;
                case Option.Y_Only:
                    rootMotion.useX = false;
                    rootMotion.useY = true;
                    break;
            }

        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var rootMotion = animator.GetComponentInChildren<SpineRootMotion>();
            rootMotion.enabled = false;
        }
    }
}