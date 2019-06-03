using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class MummyAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_WHIP_ATTACK = "whip";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_FLINCH = "damage";
        public new const string ANIMATION_DEATH = "death";

        public void DoWhip() => SetAnimation(0, ANIMATION_WHIP_ATTACK, false);
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoFlinch() => SetAnimation(0, ANIMATION_FLINCH, false);
        public void DoMove() => SetAnimation(0, "move1", true);

        public override void DoDeath() => SetAnimation(0, ANIMATION_DEATH, false);        
    }
}
