using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MushroomGiantAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2_DOWNWARD_LAUNCH = "Attack2_Downward_Launch";
        public const string ANIMATION_ATTACK2_UPWARD_LAUNCH = "Attack2_Upward_Launch";
        public const string ANIMATION_ATTACK3_POISON_BREATH = "Attack3_Poison_Breath";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_AAAA = "aaaa";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack2DownwardLaunch()
        {
            SetAnimation(0, ANIMATION_ATTACK2_DOWNWARD_LAUNCH, false);
        }

        public void DoAttack2UpwardLaunch()
        {
            SetAnimation(0, ANIMATION_ATTACK2_UPWARD_LAUNCH, false);
        }

        public void DoPoisonBreath()
        {
            SetAnimation(0, ANIMATION_ATTACK3_POISON_BREATH, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinchNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }
    }
}
