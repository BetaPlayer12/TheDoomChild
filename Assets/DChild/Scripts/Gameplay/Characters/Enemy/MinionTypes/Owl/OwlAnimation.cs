using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class OwlAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_DETECT = "Detect_player";
        public const string ANIMATION_DETECT2 = "Detect_player2";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_FLINCH_BACK = "Flinch(back)";
        public const string ANIMATION_IDLE2 = "Idle2";
        public const string ANIMATION_IDLE3 = "Idle3";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDetect2()
        {
            SetAnimation(0, ANIMATION_DETECT2, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinchBack()
        {
            SetAnimation(0, ANIMATION_FLINCH_BACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoIdle2()
        {
            SetAnimation(0, ANIMATION_IDLE2, true);
        }

        public void DoIdle3()
        {
            SetAnimation(0, ANIMATION_IDLE3, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
