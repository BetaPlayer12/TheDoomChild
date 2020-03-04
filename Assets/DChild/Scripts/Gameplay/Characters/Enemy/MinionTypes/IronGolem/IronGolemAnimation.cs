using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class IronGolemAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        public const string ANIMATION_DEATH2 = "Death2";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_L_CHAIN = "L_Chain";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE_STATIONARY = "Move_Stationary";
        public const string ANIMATION_R_CANNON = "R_Cannon";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinchNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeath2()
        {
            SetAnimation(0, ANIMATION_DEATH2, false);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoLeftChain()
        {
            SetAnimation(0, ANIMATION_L_CHAIN, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoRightCannon()
        {
            SetAnimation(0, ANIMATION_R_CANNON, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
