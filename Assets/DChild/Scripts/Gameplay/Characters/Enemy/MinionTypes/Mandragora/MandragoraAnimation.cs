using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MandragoraAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_DEATH1 = "Death1";
        public const string ANIMATION_DEATH2 = "Death2";
        public const string ANIMATION_DETECT = "DetectPlayer";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_IDLE1 = "Idle1";
        public const string ANIMATION_IDLE2 = "Idle2";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttack(bool isBurried)
        {
            SetAnimation(0, isBurried ? ANIMATION_ATTACK1 : ANIMATION_ATTACK2, false);
            AddAnimation(0, isBurried ? "Idle1" : "Idle2", true, 0);
        }

        public void DoDeath1()
        {
            SetAnimation(0, ANIMATION_DEATH1, false);
        }

        public void DoDeath2()
        {
            SetAnimation(0, ANIMATION_DEATH2, false);
        }

        public void DoDetect(bool isBurried)
        {
            SetAnimation(0, ANIMATION_DETECT, false);
            AddAnimation(0, isBurried ? "Idle1" : "Idle2", true, 0);
        }

        public void DoFlinch(bool isBurried)
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, isBurried ? "Idle1" : "Idle2", true, 0);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_IDLE1, true);
        }

        public void DoIdle2()
        {
            SetAnimation(0, ANIMATION_IDLE2, true);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
