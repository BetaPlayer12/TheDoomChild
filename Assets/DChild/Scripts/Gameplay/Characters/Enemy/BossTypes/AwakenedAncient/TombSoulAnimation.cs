using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TombSoulAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_CHARGE = "Charge";
        public const string ANIMATION_POP = "Pop";
        #endregion

        public void DoCharge()
        {
            SetAnimation(0, ANIMATION_CHARGE, true);
        }

        public void DoPop()
        {
            SetAnimation(0, ANIMATION_POP, true);
        }
    }
}
