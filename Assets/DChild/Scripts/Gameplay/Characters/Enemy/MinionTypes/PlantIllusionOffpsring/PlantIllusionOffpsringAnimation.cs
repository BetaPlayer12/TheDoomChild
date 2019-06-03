using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PlantIllusionOffpsringAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_BURROW = "Burrow";
        public const string ANIMATION_BURROW_REVEAL = "Burrow_Reveal";
        public const string ANIMATION_BURROWED_IDLE = "Burrowed_Idle";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_JUMP_ATTACK = "Jump_Attack";
        public const string ANIMATION_JUMP_ATTACK_ROOT = "Jump_Attack_Root";
        public const string ANIMATION_JUMP_PREPARATION = "Jump_Preparation";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_SPIT_ATTACK = "Spit_Attack";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_AAA = "aaa";
        public const string ANIMATION_ALL = "all";
        #endregion

        public void DoBurrow()
        {
            SetAnimation(0, ANIMATION_BURROW, false);
            AddAnimation(0, "Burrowed_Idle", true, 0);
        }

        public void DoBurrowReveal()
        {
            SetAnimation(0, ANIMATION_BURROW_REVEAL, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoBurrowedIdle()
        {
            SetAnimation(0, ANIMATION_BURROWED_IDLE, true);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoJumpAttack()
        {
            SetAnimation(0, ANIMATION_JUMP_ATTACK, false);
        }

        public void DoJumpAttackRoot()
        {
            SetAnimation(0, ANIMATION_JUMP_ATTACK_ROOT, false);
        }

        public void DoJumpPreparation()
        {
            SetAnimation(0, ANIMATION_JUMP_PREPARATION, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, false);
        }

        public void DoSpitAttack()
        {
            SetAnimation(0, ANIMATION_SPIT_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoAAA()
        {
            SetAnimation(0, ANIMATION_AAA, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAll()
        {
            SetAnimation(0, ANIMATION_ALL, false);
            AddAnimation(0, "Idle", true, 0);
        }
    }
}
