using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MantisAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_DAMAGE = "damage";
        public const string ANIMATION_DAMAGE_NO_RED = "damage no red";
        public const string ANIMATION_DEATH1 = "death";
        public const string ANIMATION_IDLE1 = "idle";
        public const string ANIMATION_LEAP = "leap";
        public const string ANIMATION_LEAP_PREPARATION = "preparation";
        public const string ANIMATION_LEAP_ROOT = "leap root";
        public const string ANIMATION_MOVE = "move";
        public const string ANIMATION_MOVE_ROOT = "move root";
        public const string ANIMATION_SCRATCH_ATTACK = "scratch (attack)";
        public const string ANIMATION_SCRATCH_DEFLECT = "scratch (deflect projectile)";
        public const string ANIMATION_TURN = "turn";
        #endregion

        public void DoDamageAnim()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDamageNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDeath1()
        {
            SetAnimation(0, ANIMATION_DEATH1, false);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_IDLE1, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoLeap()
        {
            SetAnimation(0, ANIMATION_LEAP, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoLeapPrep()
        {
            SetAnimation(0, ANIMATION_LEAP_PREPARATION, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoLeapRoot()
        {
            SetAnimation(0, ANIMATION_LEAP_ROOT, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoMoveRoot()
        {
            SetAnimation(0, ANIMATION_MOVE_ROOT, true);
        }

        public void DoScratchAttack()
        {
            SetAnimation(0, ANIMATION_SCRATCH_ATTACK, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoScratchDeflect()
        {
            SetAnimation(0, ANIMATION_SCRATCH_DEFLECT, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
