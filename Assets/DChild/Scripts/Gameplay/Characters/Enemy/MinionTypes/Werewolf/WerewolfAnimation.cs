using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class WerewolfAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_ATTACK_SCRATCH2 = "attack_scratch2";
        public const string ANIMATION_DEATH2 = "death2";
        public const string ANIMATION_DETECTS_MAIN_CHARACTER = "detects_main character";
        public const string ANIMATION_FLINCH = "flinch";
        public const string ANIMATION_IDLE1 = "idle";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_WALK2 = "walk2";
        public const string ANIMATION_WALK3 = "walk3";
        public const string ANIMATION_WALK4 = "walk4";
        #endregion

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoAttackScratch2()
        {
            SetAnimation(0, ANIMATION_ATTACK_SCRATCH2, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDeath2()
        {
            SetAnimation(0, ANIMATION_DEATH2, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECTS_MAIN_CHARACTER, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_IDLE1, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoWalk2()
        {
            SetAnimation(0, ANIMATION_WALK2, true);
        }

        public void DoWalk3()
        {
            SetAnimation(0, ANIMATION_WALK3, true);
        }

        public void DoWalk4()
        {
            SetAnimation(0, ANIMATION_WALK4, true);
        }
    }
}
