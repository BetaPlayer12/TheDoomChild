using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class LeprechaunAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_SUMMON_GOLD_ATTACK = "Attack1";
        public const string ANIMATION_CANE_WHACK_ATTACK = "Attack2";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_DETECTPLAYER = "Detects_Death"; 
        public new const string ANIMATION_DEATH = "death";

        public const string EVENT_SUMMON_POT = "Pot_of_Gold_Appear";

        public void DoSummonGoldAttack()
        {
            SetAnimation(0, ANIMATION_SUMMON_GOLD_ATTACK, false);
        }

        public void DoCaneWhackAttack()
        {
            SetAnimation(0, ANIMATION_CANE_WHACK_ATTACK, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoDetectPlayer()
        {
            SetAnimation(0, ANIMATION_DETECTPLAYER, false);
        }

        public override void DoDeath()
        {
            SetAnimation(0, ANIMATION_DEATH, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "Walk", true);
        }
    }
}
