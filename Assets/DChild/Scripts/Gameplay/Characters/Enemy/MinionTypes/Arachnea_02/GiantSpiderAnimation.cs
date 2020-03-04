using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantSpiderAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_WEBATTACK = "Attack1_Web";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_MOVEHOP = "Move_Hop";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_TURN = "Turn";

        public void DoAttack() => SetAnimation(0, ANIMATION_WEBATTACK, false);
        public void DoFlinch() => SetAnimation(0, ANIMATION_DAMAGE, false);
        public void DoMoveHop() => SetAnimation(0, ANIMATION_MOVEHOP, false);
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);

        public void DoPatrol() => SetAnimation(0, ANIMATION_MOVE, true).TimeScale = 1f;
        public void DoMove() => SetAnimation(0, ANIMATION_MOVE, true).TimeScale = 2f;
           
    }
}
