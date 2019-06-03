using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class WhiteLadyAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_SUMMON_SPECTRES = "WhiteLady_Attack2_B";
        public const string ANIMATION_FLINCH = "WhiteLady_Flinch";
        public const string ANIMATION_TURN = "WhiteLady_Turn";
        public new const string ANIMATION_DEATH = "WhiteLady_Death";

        public const string EVENT_SUMMON_FX_START = "Summoning PFX Start";
        public const string EVENT_SUMMON_1ST_SPECTRE = "1st Monster Appears";
        public const string EVENT_SUMMON_2ND_SPECTRE = "2nd Monster Appears";
        public const string EVENT_SUMMON_3RD_SPECTRE = "3rd Monster Appears";

        public void DoSummon()
        {
            SetAnimation(0, ANIMATION_SUMMON_SPECTRES, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "WhiteLady_Movement_No_Root_Movement", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, ANIMATION_DEATH, false);
        }

        public override void DoIdle ()
        {
            SetAnimation(0, "WhiteLady_Battle_Idle", true);
        }
    }
}
