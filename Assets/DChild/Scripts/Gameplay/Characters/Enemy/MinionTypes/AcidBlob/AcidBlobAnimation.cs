using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class AcidBlobAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_ATTACK = "Blobacid_Attack_Acid_smoke";
        public const string ANIMATION_FLINCH = "Blobacid_Flinch";
        public const string ANIMATION_DETECTPLAYER = "Blobacid_Idle_Sleep_Then_Detect";

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "Blobacid_Walk_Inplacerightside", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "Blobacid_Idle", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, "Blobacid_Death", false);
        }
    }
}
