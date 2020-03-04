using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MummySwarmerAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "attack_WIP";
        public const string ANIMATION_FLINCH = "hit";
        public const string ANIMATION_TURN = "turn";
        public new const string ANIMATION_DEATH = "death";

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
