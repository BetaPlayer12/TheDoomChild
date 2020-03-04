using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RatAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        //public const string ANIMATION_DEATH = "Death";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_IDLE1 = "Idle1";
        public const string ANIMATION_IDLE2 = "Idle2";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE_STATIONARY = "Move_Stationary";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "Idle1", true, 0);
        }

        public void DoDamge()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle1", true, 0);
        }

        public void DoDamgeNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle1", true, 0);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
            AddAnimation(0, "Idle1", true, 0);
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

        public void DoMoveStationary()
        {
            SetAnimation(0, ANIMATION_MOVE_STATIONARY, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
