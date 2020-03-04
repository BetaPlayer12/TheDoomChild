using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ZombieAdultAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_SCRATCH_JUMP_ATTACK = "ZOMBIE1_Attack1";
        public const string ANIMATION_SCRATCH_ATTACK = "ZOMBIE1_Attack3";
        public const string ANIMATION_BITE_ATTACK = "ZOMBIE1_Attack2";
        public const string ANIMATION_RUN_ANTICIPATION = "ZOMBIE1_Antic_Before_Run";
        public const string ANIMATION_RUN = "ZOMBIE1_Run";
        public const string ANIMATION_WALK = "ZOMBIE1_Walk";
        public const string ANIMATION_JUMP = "ZOMBIE1_Jump";
        public const string ANIMATION_DETECT = "ZOMBIE1_Detect";
        public const string ANIMATION_TURN = "ZOMBIE1_Turn";
        public const string ANIMATION_FLINCH = "ZOMBIE1_Flinch";
        public new const string ANIMATION_DEATH = "ZOMBIE1_Death";
        public new const string ANIMATION_IDLE = "ZOMBIE1_Idle";

        public void DoScratchJumpAttack()
        {
            SetAnimation(0, ANIMATION_SCRATCH_JUMP_ATTACK, false);
        }

        public void DoScratchAttack()
        {
            SetAnimation(0, ANIMATION_SCRATCH_ATTACK, false);
        }

        public void DoBiteAttack()
        {
            SetAnimation(0, ANIMATION_BITE_ATTACK, false);
        }

        public void DoRunAnticipation()
        {
            SetAnimation(0, ANIMATION_RUN_ANTICIPATION, false);
        }

        public void DoRun()
        {
            SetAnimation(0, ANIMATION_RUN, true);
        }

        public void DoWalk()
        {
            SetAnimation(0, ANIMATION_WALK, true);
        }

        public void DoJump()
        {
            SetAnimation(0, ANIMATION_JUMP, true);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, true);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, ANIMATION_DEATH, true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, ANIMATION_IDLE, true);
        }
    }
}
