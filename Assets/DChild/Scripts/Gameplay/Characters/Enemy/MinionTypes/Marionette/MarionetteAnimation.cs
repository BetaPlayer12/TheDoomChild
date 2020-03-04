using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MarionetteAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_MARIONETTE3_ASSEMBLE = "MARIONETTE3_Assemble";
        public const string ANIMATION_MARIONETTE3_ATTACK1 = "MARIONETTE3_Attack1";
        public const string ANIMATION_MARIONETTE3_ATTACK2 = "MARIONETTE3_Attack2";
        public const string ANIMATION_MARIONETTE3_DEATH1 = "MARIONETTE3_Death1";
        public const string ANIMATION_MARIONETTE3_DEATH2 = "MARIONETTE3_Death2";
        public const string ANIMATION_MARIONETTE3_DEATH3 = "MARIONETTE3_Death3";
        public const string ANIMATION_MARIONETTE3_FLINCH = "MARIONETTE3_Flinch";
        public const string ANIMATION_MARIONETTE3_IDLE = "MARIONETTE3_Idle";
        public const string ANIMATION_MARIONETTE3_TURN = "MARIONETTE3_Turn";
        #endregion

        public void DoAssemble()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_ASSEMBLE, false);
            AddAnimation(0, "MARIONETTE3_Idle", true, 0);
        }

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_ATTACK1, false);
            AddAnimation(0, "MARIONETTE3_Idle", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_ATTACK2, false);
            AddAnimation(0, "MARIONETTE3_Idle", true, 0);
        }

        public void DoDeath1()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_DEATH1, false);
        }

        public void DoDeath2()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_DEATH2, false);
        }

        public void DoDeath3()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_DEATH3, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_FLINCH, false);
            AddAnimation(0, "MARIONETTE3_Idle", true, 0);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_IDLE, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_MARIONETTE3_TURN, false);
        }
    }
}
