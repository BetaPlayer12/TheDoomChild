using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Scorpion01Animation : CombatCharacterAnimation
    {
        public const string ANIMATION_TAIL_ATTACK = "attack";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_FLINCH = "damage";
        public new const string ANIMATION_DEATH = "death";

        public void DoTailAttack()
        {
            SetAnimation(0, ANIMATION_TAIL_ATTACK, false);
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
            SetAnimation(0, "move", true);      
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
