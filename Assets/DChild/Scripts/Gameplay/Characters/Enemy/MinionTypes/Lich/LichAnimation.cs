using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class LichAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK1 = "";
        public const string ANIMATION_ATTACK2 = "";
        public const string ANIMATION_FLINCH = "";
        public const string ANIMATION_DETECT = "";
        
        public void DoScratch()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
        }

        public void DoSpell()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "Move_Forward", true);
        }
    }
}
