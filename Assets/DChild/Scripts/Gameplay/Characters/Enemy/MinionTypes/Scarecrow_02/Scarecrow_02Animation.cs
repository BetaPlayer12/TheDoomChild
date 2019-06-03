using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Scarecrow_02Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE_STATIONARY = "Move_Stationary";
        public const string ANIMATION_SUMMON_CROWS = "Summon_Crows";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoDamgeAnim()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDamgeNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoMoveStationary()
        {
            SetAnimation(0, ANIMATION_MOVE_STATIONARY, true);
        }

        public void DoSummonCrows()
        {
            SetAnimation(0, ANIMATION_SUMMON_CROWS, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
