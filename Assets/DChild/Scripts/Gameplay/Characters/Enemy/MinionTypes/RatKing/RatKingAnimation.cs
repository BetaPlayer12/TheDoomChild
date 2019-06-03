using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Characters.Enemies
{
    public class RatKingAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK_STAB = "Attack_Stab";
        public const string ANIMATION_ATTACK_THROW = "Attack_Throw";
        public const string ANIMATION_AIR_DAMAGE = "Air_Damage";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        //public const string ANIMATION_DEATH = "Death";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_HOP = "Hop";
        //public const string ANIMATION_IDLE = "Idle";
        public const string ANIMATION_IDLE2 = "Idle2";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_SUMMON_RAT = "Summon_Rat";
        public const string ANIMATION_TURN = "Turn";
        #endregion

        public void DoAttackStab()
        {
            SetAnimation(0, ANIMATION_ATTACK_STAB, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttackThrow()
        {
            SetAnimation(0, ANIMATION_ATTACK_THROW, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAirDamage()
        {
            SetAnimation(0, ANIMATION_AIR_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDamageAnim()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDamageNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoHop()
        {
            SetAnimation(0, ANIMATION_HOP, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoIdle2()
        {
            SetAnimation(0, ANIMATION_IDLE2, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoSummonRat()
        {
            SetAnimation(0, ANIMATION_SUMMON_RAT, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
