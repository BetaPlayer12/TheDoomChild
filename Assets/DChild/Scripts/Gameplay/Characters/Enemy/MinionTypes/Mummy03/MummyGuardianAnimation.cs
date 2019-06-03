using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MummyGuardianAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_CHARGE_ATTACK = "Mummy3_Charge1";
        public const string ANIMATION_CHARGE_PHASE1 = "Mummy3_Charge1_Phase1";
        public const string ANIMATION_CHARGE_PHASE2 = "Mummy3_Charge1_Phase2";
        public const string ANIMATION_CHARGE_PHASE3 = "Mummy3_Charge1_Phase3";
        public new const string ANIMATION_DEATH = "";

        public void DoChargeAttack()
        {
            SetAnimation(0, ANIMATION_CHARGE_ATTACK, false);
        }

        public void DoChargePhase1()
        {
            SetAnimation(0, ANIMATION_CHARGE_PHASE1, false);
        }

        public void DoChargePhase2()
        {
            SetAnimation(0, ANIMATION_CHARGE_PHASE2, false);
        }

        public void DoChargePhase3()
        {
            SetAnimation(0, ANIMATION_CHARGE_PHASE3, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "Mummy3_Walk", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "Mummy3_Idle2", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, "Mummy3_Death", false);
        }
    }
}
