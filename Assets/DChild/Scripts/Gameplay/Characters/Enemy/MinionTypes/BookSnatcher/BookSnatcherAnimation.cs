using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BookSnatcherAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_BOOK_OPEN = "Book_Open";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_RUN = "Run";
        public const string ANIMATION_RUN_STATIONARY = "Run_Stationary";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_WALK = "Walk";
        public const string ANIMATION_WALK_STATIONARY = "Walk_Stationary";
        public const string ANIMATION_TEST = "test";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoBookOpen()
        {
            SetAnimation(0, ANIMATION_BOOK_OPEN, false);
            AddAnimation(0, "Idle", true, 0);
        }

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
        }

        public void DoRun()
        {
            SetAnimation(0, ANIMATION_RUN, true);
        }

        public void DoRunStationary()
        {
            SetAnimation(0, ANIMATION_RUN_STATIONARY, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoWalk()
        {
            SetAnimation(0, ANIMATION_WALK, true);
        }

        public void DoWalkStationary()
        {
            SetAnimation(0, ANIMATION_WALK_STATIONARY, true);
        }

        public void DoTest()
        {
            SetAnimation(0, ANIMATION_TEST, true);
        }
    }
}
