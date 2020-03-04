using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ShellBugAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_CHARGE_PREP = "Charge_Preparation";
        public const string ANIMATION_CHARGE = "Charge_Loop";
        public const string ANIMATION_DAMAGE = "Damage";

        public void DoChargeAnticipation() => SetAnimation(0, ANIMATION_CHARGE_PREP, false);
        public void DoFlinch() => SetAnimation(0, ANIMATION_DAMAGE, false);
        public void DoCharge() => SetAnimation(0, ANIMATION_CHARGE, false);
        public void DoMove() => SetAnimation(0, "Move", true);
    }
}
