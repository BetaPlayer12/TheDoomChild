using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class GrassMonsterAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_DETECT_PLAYER = "Detect_Player";
        public const string ANIMATION_HURT = "Flinch";
        public const string ANIMATION_TURN_LEFT = "Turn_Right_To_Left";
        public const string ANIMATION_TURN_RIGHT = "Turn_Left_To_Right";

        public void DoMove() => SetAnimation(0, ANIMATION_MOVE, true);
        public void DoPlayerDetected() => SetAnimation(0, ANIMATION_DETECT_PLAYER, false);
        public void DoFlinch() => SetAnimation(0, ANIMATION_HURT, false);
        public void DoTurnLeft() => SetAnimation(0, ANIMATION_TURN_LEFT, false);
        public void DoTurnRight() => SetAnimation(0, ANIMATION_TURN_RIGHT, false);

        public void DoAttack1() => SetAnimation(0, ANIMATION_ATTACK1, false);
        public void DoAttack2() => SetAnimation(0, ANIMATION_ATTACK2, false);
    }

}