using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonArcherAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "Shoot_with_Drawing_Arrow";
        public const string ANIMATION_FLINCH = "Hurt";
        public const string ANIMATION_TURN = "turn";

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
    }
}
