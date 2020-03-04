using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ZombieChildAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_SCRATCH_ATTACK = "ZOMBIE2_Attack1";
        public const string ANIMATION_VOMIT_ATTACK = "ZOMBIE2_Attack2";
        public const string ANIMATION_VOMIT = "ZOMBIE2_Vomit";
        public const string ANIMATION_PLAYER_DETECT = "ZOMBIE2_Detect";
        public const string ANIMATION_TURN = "ZOMBIE2_Turn";
        public const string ANIMATION_FLINCH = "ZOMBIE2_Flinch";
        public new const string ANIMATION_DEATH = "ZOMBIE2_Death2";

        public void DoScratchAttack()
        {
            SetAnimation(0, ANIMATION_SCRATCH_ATTACK, false);
        }

        public void DoVomitAttack()
        {
            SetAnimation(0, ANIMATION_VOMIT_ATTACK, false);
        }

        public void DoVomit()
        {
            SetAnimation(0, ANIMATION_VOMIT, false);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_PLAYER_DETECT, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "ZOMBIE2_Walk", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "ZOMBIE2_Idle", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, ANIMATION_DEATH, false);
        }
    }
}
