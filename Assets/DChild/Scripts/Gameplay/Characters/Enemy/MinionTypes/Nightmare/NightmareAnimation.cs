using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class NightmareAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK_BREAK = "Attack_break";
        public const string ANIMATION_ATTACK_CHARGE = "Attack_charge";
        public const string ANIMATION_DETECT_PLAYER = "Detect_player";
        public const string ANIMATION_Flinch = "Flinch";
        public const string ANIMATION_Flinch2 = "Flinch_2";
        public const string ANIMATION_Flinch2_BACK = "Flinch_2back";
        public const string ANIMATION_PREP_ATTACK = "Prep_attack";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttackBreak()
        {
            SetAnimation(0, ANIMATION_ATTACK_BREAK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttackCharge()
        {
            SetAnimation(0, ANIMATION_ATTACK_CHARGE, true);
            //AddAnimation(0, "Idle", true, 0);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT_PLAYER, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_Flinch, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch2()
        {
            SetAnimation(0, ANIMATION_Flinch2, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch2Back()
        {
            SetAnimation(0, ANIMATION_Flinch2_BACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoPrep()
        {
            SetAnimation(0, ANIMATION_PREP_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
            //AddAnimation(0, "Idle", true, 0);
        }
    }
}
