using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TrollAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_ATTACK3 = "Attack3";
        public const string ANIMATION_DETECT = "Detect_Player";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE_ROOT = "Move_RootMotion";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack3()
        {
            SetAnimation(0, ANIMATION_ATTACK3, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT, false);
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

        public void DoMoveRoot()
        {
            SetAnimation(0, ANIMATION_MOVE_ROOT, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
