using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Scorpion02Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_DETECT_PLAYER = "Detect_Player";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE2 = "Move2";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDetectPlayer()
        {
            SetAnimation(0, ANIMATION_DETECT_PLAYER, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoMove2()
        {
            SetAnimation(0, ANIMATION_MOVE2, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
