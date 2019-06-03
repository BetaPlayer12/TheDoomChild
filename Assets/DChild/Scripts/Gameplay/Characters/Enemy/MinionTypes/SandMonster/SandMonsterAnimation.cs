using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SandMonsterAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_TURN = "Turn";

        public void DoAttack() => SetAnimation(0, ANIMATION_ATTACK, false);
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoMove() => SetAnimation(0, "Move", true).TimeScale = 2f;
        public void DoPatrol() => SetAnimation(0, "Move", true);
    }
}
