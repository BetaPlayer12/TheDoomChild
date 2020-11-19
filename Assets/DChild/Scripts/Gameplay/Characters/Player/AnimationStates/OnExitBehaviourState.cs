﻿using DChild.Gameplay.Characters.Players.Modules;
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
            EndLedgeGrab
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
            }
        }
    }
}