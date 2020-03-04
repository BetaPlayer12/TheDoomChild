using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PossessedCatAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_POUNCE = "Pounce";
        public const string ANIMATION_POUNCE_PREPARATION = "Pounce_Preparation";
        public const string ANIMATION_SCRATCH = "Scratch";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoDamageAnim()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDamageNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoPounce()
        {
            SetAnimation(0, ANIMATION_POUNCE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoPouncePrep()
        {
            SetAnimation(0, ANIMATION_POUNCE_PREPARATION, false);
        }

        public void DoScratch()
        {
            SetAnimation(0, ANIMATION_SCRATCH, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
