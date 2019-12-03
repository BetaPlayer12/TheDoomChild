using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class StoneGolemAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "attack";
        public const string ANIMATION_CHARGE_END = "charge end";
        public const string ANIMATION_CHARGE_LOOP = "charge loop";
        public const string ANIMATION_CHARGE_NEW = "charge new";
        public const string ANIMATION_CHARGE_PREPARATION = "charge preparation";
        public const string ANIMATION_CHARGE_TEST = "charge test";
        public const string ANIMATION_DAMAGE = "damage";
        public const string ANIMATION_DAMAGE_NO_RED = "damage no red";
        public const string ANIMATION_DEATH1 = "death";
        public const string ANIMATION_DEATH_NO_RED = "death no red";
        public const string ANIMATION_IDLE1 = "idle";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_WALK = "walk";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoChargeEnd()
        {
            SetAnimation(0, ANIMATION_CHARGE_END, false);
        }

        public void DoChargeLoop()
        {
            SetAnimation(0, ANIMATION_CHARGE_LOOP, true);
        }

        public void DoChargeNew()
        {
            SetAnimation(0, ANIMATION_CHARGE_NEW, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoChargePreparation()
        {
            SetAnimation(0, ANIMATION_CHARGE_PREPARATION, false);
        }

        public void DoChargeTest()
        {
            SetAnimation(0, ANIMATION_CHARGE_TEST, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoFlinchNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDeath1()
        {
            SetAnimation(0, ANIMATION_DEATH1, false);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_IDLE1, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_WALK, true);
        }
    }
}
