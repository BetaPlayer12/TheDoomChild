using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBlobAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "Attack_Stretch";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_TURN = "Turn2";

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
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
            SetAnimation(0, "Move_inplace", true);
        }
    }
}
