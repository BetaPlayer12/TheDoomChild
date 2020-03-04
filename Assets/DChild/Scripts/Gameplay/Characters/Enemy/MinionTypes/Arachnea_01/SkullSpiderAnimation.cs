using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkullSpiderAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "ARACHNAE1_Attack1";
        public const string ANIMATION_DETECT = "ARACHNAE1_Detect";
        public const string ANIMATION_FLINCH = "ARACHNAE1_Flinch";
        public const string ANIMATION_TURN = "ARACHNAE1_Turn";
        public new const string ANIMATION_DEATH = "ARACHNAE1_Death1";

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoWalk()
        {
            SetAnimation(0, "ARACHNAE1_Walk", true);
        }

        public void DoRun()
        {
            SetAnimation(0, "ARACHNAE1_Run", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "ARACHNAE1_Idle", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, ANIMATION_DEATH, false);
        }
    }
}
