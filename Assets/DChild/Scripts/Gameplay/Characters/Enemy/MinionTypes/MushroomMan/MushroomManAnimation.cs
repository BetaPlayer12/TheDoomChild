using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MushroomManAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "attack";
        public const string ANIMATION_FLINCH = "damage";
        public new const string ANIMATION_DEATH = "death";

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "walk", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "idle", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, ANIMATION_DEATH, false);
        }
    }
}

