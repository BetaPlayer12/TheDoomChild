using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CockatriceAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_COCK_ATTACK1 = "Cockatrice_Attack1";
        public const string ANIMATION_COCK_ATTACK2 = "Cockatrice_Attack2";
        public const string ANIMATION_COCK_DEATH = "Cockatrice_DEATH";
        public const string ANIMATION_COCK_FLINCH = "Cockatrice_Flinch";
        public const string ANIMATION_COCK_IDLE = "Cockatrice_Idle";
        public const string ANIMATION_COCK_RUN = "Cockatrice_Run";
        public const string ANIMATION_COCK_RUN_NO_ROOT = "Cockatrice_Run_No_Root_Movement";
        public const string ANIMATION_COCK_RUN_STOP = "Cockatrice_Run_Stop";
        public const string ANIMATION_COCK_RUN_STOP2 = "Cockatrice_Run_Stop2";
        public const string ANIMATION_COCK_TURN = "Cockatrice_Turn";
        public const string ANIMATION_COCK_WALK_NO_ROOT = "Cockatrice_Walk_No_Root_Movement";
        public const string ANIMATION_COCK_WALK_ROOT = "Cockatrice_Walk_Root_Movement";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_COCK_ATTACK1, false);
            AddAnimation(0, "Cockatrice_Idle", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_COCK_ATTACK2, false);
            AddAnimation(0, "Cockatrice_Idle", true, 0);
        }

        public void DoCockDeath()
        {
            SetAnimation(0, ANIMATION_COCK_DEATH, false);
            AddAnimation(0, "Cockatrice_Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_COCK_FLINCH, false);
            AddAnimation(0, "Cockatrice_Idle", true, 0);
        }

        public void DoCockIdle()
        {
            SetAnimation(0, ANIMATION_COCK_IDLE, true);
        }

        public void DoRun()
        {
            SetAnimation(0, ANIMATION_COCK_RUN, true);
        }

        public void DoRunNoRoot()
        {
            SetAnimation(0, ANIMATION_COCK_RUN_NO_ROOT, true);
        }

        public void DoRunStop()
        {
            SetAnimation(0, ANIMATION_COCK_RUN_STOP, false);
            AddAnimation(0, "Cockatrice_Idle", true, 0);
        }

        public void DoRunStop2()
        {
            SetAnimation(0, ANIMATION_COCK_RUN_STOP2, false);
            AddAnimation(0, "Cockatrice_Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_COCK_TURN, false);
        }

        public void DoWalkNoRoot()
        {
            SetAnimation(0, ANIMATION_COCK_WALK_NO_ROOT, true);
        }

        public void DoWalkRoot()
        {
            SetAnimation(0, ANIMATION_COCK_WALK_ROOT, true);
        }
    }
}
