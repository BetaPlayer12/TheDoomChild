using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ButcherAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_MOVE = "Walk_Test";
        public const string ANIMATION_SLASH_ATTACK = "Attack";
        public const string ANIMATION_HEAVYSLASH_ATTACK = "Attack_Combo";
        public const string ANIMATION_SLASH_CHOP_ATTACK = "Attack_Combo2";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_FLINCH = "Flinch";

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoSlashAttack()
        {
            SetAnimation(0, ANIMATION_SLASH_ATTACK, false);
        }

        public void DoHeavySlashAttack()
        {
            SetAnimation(0, ANIMATION_HEAVYSLASH_ATTACK, false);
        }

        public void DoSlashChopAttack()
        {
            SetAnimation(0, ANIMATION_SLASH_CHOP_ATTACK, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }
    }
}
