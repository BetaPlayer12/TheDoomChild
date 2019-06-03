using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class VenusFlytrapAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_FLINCH = "Hit";
        public const string ANIMATION_BITEATTACK = "Attack1";
        public const string ANIMATION_WHIPATTACK = "Attack2";

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoBiteAttack()
        {
            SetAnimation(0, ANIMATION_BITEATTACK, false);
        }

        public void DoWhipAttack()
        {
            SetAnimation(0, ANIMATION_WHIPATTACK, false);
        }

        public override void DoDeath()
        {
            SetAnimation(0, "Death2", false);
        }
    }
}
