using DChild.Gameplay.Characters.Players.Modules;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Characters.Players
{
    public class OnExitBehaviourState : StateMachineBehaviour
    {
        private enum Command
        {
            EndAttack,
            EndEarthShaker,
            EndLedgeGrab,
            EndShadowMorphCharge,
            EndComboAttack,
            EndCombo,
            EndSwordThrust
        }

        [SerializeField]
        private Command m_toExecute;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            var player = animator.GetComponent<PlayerFunctions>();
            switch (m_toExecute)
            {
                case Command.EndAttack:
                    player.FinishAttackAnim();
                    break;
                case Command.EndEarthShaker:
                    player.EarthShakerEnd();
                    break;
                case Command.EndLedgeGrab:
                    player.EndLedgeGrab();
                    break;
                case Command.EndShadowMorphCharge:
                    player.EndShadowMorphCharge();
                    break;
                case Command.EndComboAttack:
                    player.ComboAttackEnd();
                    break;
                case Command.EndCombo:
                    player.ComboEnd();
                    break;
                case Command.EndSwordThrust:
                    player.SwordThrustEnd();
                    break;
            }
        }
    }

    //public class ForceEndAfterDurationBehaviourState : StateMachineBehaviour
    //{
    //    public AnimationReferenceAsset m_animation;
    //    private Coroutine m_routine;
    //    private PlayerFunctions m_reference;

    //    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //    {
    //        base.OnStateExit(animator, stateInfo, layerIndex);
    //        if (m_reference != null)
    //        {
    //            m_reference = animator.GetComponent<PlayerFunctions>();
    //        }
    //        m_reference.StartCoroutine()
    //    }

    //    private IEnumerator DurationRoutine()
    //    {
    //        yield return new m_animation.
    //    }
    //}
}