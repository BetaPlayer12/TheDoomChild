using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MushroomGiantFXAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK3_POISON_BREATH = "Attack3_Poison_Breath";
        #endregion

        public void DoPoisonBreath()
        {
            SetAnimation(0, ANIMATION_ATTACK3_POISON_BREATH, false);
            animationState.AddEmptyAnimation(0, 0, 0);
        }
    }
}
