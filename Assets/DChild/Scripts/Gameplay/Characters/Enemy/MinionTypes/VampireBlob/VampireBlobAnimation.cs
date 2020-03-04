using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class VampireBlobAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "attack";
        public const string ANIMATION_FLINCH = "damage no red";
        public const string ANIMATION_MOVE = "move";
        public const string ANIMATION_JUMP = "jump root";
        public new const string ANIMATION_DEATH = "death";

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoJump()
        {
            SetAnimation(0, ANIMATION_JUMP, true);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "idle", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, "death", false);
        }
    }
}


