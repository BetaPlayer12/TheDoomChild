using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class AwakenedAncientAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_BURROW = "Burrow";
        public const string ANIMATION_BURROW_IDLE = "Burrow_Idle";
        public const string ANIMATION_GROUND_SLAM = "Ground_Slam";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE_STATIONARY = "Move_Stationary";
        public const string ANIMATION_SPIT = "Spit";
        public const string ANIMATION_SPIT_SKELETON = "Spit_Skeleton";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_UNBURROW = "Unburrow";
        public const string ANIMATION_TEST = "test";
        #endregion

        public void DoBurrow()
        {
            SetAnimation(0, ANIMATION_BURROW, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0);
        }

        public void DoBurrowIdle()
        {
            SetAnimation(0, ANIMATION_BURROW_IDLE, true);
        }

        public void DoGroundSlam()
        {
            SetAnimation(0, ANIMATION_GROUND_SLAM, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoMoveStationary()
        {
            SetAnimation(0, ANIMATION_MOVE_STATIONARY, true);
        }

        public void DoSpit()
        {
            SetAnimation(0, ANIMATION_SPIT, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0);
        }

        public void DoSpitSkeleton()
        {
            SetAnimation(0, ANIMATION_SPIT_SKELETON, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoUnburrow()
        {
            SetAnimation(0, ANIMATION_UNBURROW, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0);
        }

        public void DoTest()
        {
            SetAnimation(0, ANIMATION_TEST, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0);
        }
    }
}
