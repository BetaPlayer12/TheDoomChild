using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Marrionette2Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_BODY_SLAM_END = "Body_Slam_End";
        public const string ANIMATION_BODY_SLAM_END_NO_RED = "Body_Slam_Body_Slam_No_Red";
        public const string ANIMATION_BODY_SLAM_LOOP = "Body_Slam_Loop";
        public const string ANIMATION_BODY_SLAM_START = "Body_Slam_Start";
        public const string ANIMATION_BODY_MOVE = "Move";
        public const string ANIMATION_PIECES_END = "Pieces_End";
        public const string ANIMATION_PIECES_LOOP = "Pieces_Loop";
        public const string ANIMATION_PIECES_START = "Pieces_Start";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoBodySlamEnd()
        {
            SetAnimation(0, ANIMATION_BODY_SLAM_END, false);
        }

        public void DoBodySlamEndNoRed()
        {
            SetAnimation(0, ANIMATION_BODY_SLAM_END_NO_RED, false);
        }

        public void DoBodySlamLoop()
        {
            SetAnimation(0, ANIMATION_BODY_SLAM_LOOP, true);
        }

        public void DoBodySlamStart()
        {
            SetAnimation(0, ANIMATION_BODY_SLAM_START, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_BODY_MOVE, false);
        }

        public void DoPiecesEnd()
        {
            SetAnimation(0, ANIMATION_PIECES_END, false);
        }

        public void DoPiecesLoop()
        {
            SetAnimation(0, ANIMATION_PIECES_LOOP, true);
        }

        public void DoPiecesStart()
        {
            SetAnimation(0, ANIMATION_PIECES_START, false);
        }

        public void DoAssemble()
        {
            SetAnimation(0, ANIMATION_PIECES_END, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_IDLE, false);
            AddAnimation(0, "Idle", true, 0);
        }
    }
}
