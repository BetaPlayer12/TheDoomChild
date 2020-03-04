using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TukkoAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_AIR_DAMAGE = "Air_Damage";
        public const string ANIMATION_AIR_DAMAGE_IMPACT = "Air_Damage_Impact";
        public const string ANIMATION_AIR_DAMAGE_IMPACTIDLE = "Air_Damage_ImpactIdle";
        public const string ANIMATION_AIR_DAMAGE_IMPACTRECOVER = "Air_Damage_ImpactRecover";
        public const string ANIMATION_ATTACK1_STABFULL = "Attack1_StabFULL";
        public const string ANIMATION_ATTACK1_STABRETURN = "Attack1_StabReturn";
        public const string ANIMATION_ATTACK1_STABSTART = "Attack1_StabStart";
        public const string ANIMATION_ATTACK2_STAB_COMBO = "Attack2_Stab_Combo";
        public const string ANIMATION_ATTACK3_AIRBORNE_GRENADES = "Attack3_Airborne_Grenades";
        public const string ANIMATION_ATTACK4_AIRBORNE_SLAM = "Attack4_Airborne_Slam";
        public const string ANIMATION_ATTACK5_JAB = "Attack5_Jab";
        public const string ANIMATION_DAMAGE = "Damage";
        //public const string ANIMATION_DEATH = "Death";
        public const string ANIMATION_DEATH2 = "Death 2";
        public const string ANIMATION_HOP_BACKWARDS = "Hop_Backwards";
        //public const string ANIMATION_IDLE = "Idle";
        public const string ANIMATION_JUMP_UP = "Jump_Up";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_MOVE_STATIONARY = "Move_Stationary";
        public const string ANIMATION_SMOKE_BOMB = "Smoke_Bomb";
        public const string ANIMATION_AA = "aa";
        public const string ANIMATION_ASDFASDF = "asdfasdf";
        public const string ANIMATION_EYES = "eyes";
        #endregion
        
        //public override void DoIdle()
        //{
        //    SetAnimation(0, ANIMATION_IDLE, true);
        //}

        //public override void DoDeath()
        //{
        //    SetAnimation(0, ANIMATION_DEATH, false);
        //}

        public void DoDamageAnim()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeath2()
        {
            SetAnimation(0, ANIMATION_DEATH2, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoMoveStationary()
        {
            SetAnimation(0, ANIMATION_MOVE_STATIONARY, true);
        }

        public void DoAirDamage()
        {
            SetAnimation(0, ANIMATION_AIR_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAirDamageImpact()
        {
            SetAnimation(0, ANIMATION_AIR_DAMAGE_IMPACT, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAirDamageImpactIdle()
        {
            SetAnimation(0, ANIMATION_AIR_DAMAGE_IMPACTIDLE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAirDamageImpactRecover()
        {
            SetAnimation(0, ANIMATION_AIR_DAMAGE_IMPACTRECOVER, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack1StabFull()
        {
            SetAnimation(0, ANIMATION_ATTACK1_STABFULL, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack1StabReturn()
        {
            SetAnimation(0, ANIMATION_ATTACK1_STABRETURN, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack1StabStart()
        {
            SetAnimation(0, ANIMATION_ATTACK1_STABSTART, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack2StabCombo()
        {
            SetAnimation(0, ANIMATION_ATTACK2_STAB_COMBO, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack3AirborneGrenades()
        {
            SetAnimation(0, ANIMATION_ATTACK3_AIRBORNE_GRENADES, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack4AirborneSlam()
        {
            SetAnimation(0, ANIMATION_ATTACK4_AIRBORNE_SLAM, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttackJab()
        {
            SetAnimation(0, ANIMATION_ATTACK5_JAB, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoHopBackwards()
        {
            SetAnimation(0, ANIMATION_HOP_BACKWARDS, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoJumpUp()
        {
            SetAnimation(0, ANIMATION_JUMP_UP, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoSmokeBomb()
        {
            SetAnimation(0, ANIMATION_SMOKE_BOMB, false);
            //AddAnimation(0, "Idle", true, 0);
        }

        public void DoAA()
        {
            SetAnimation(0, ANIMATION_AA, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoASDF()
        {
            SetAnimation(0, ANIMATION_ASDFASDF, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoEyes()
        {
            SetAnimation(0, ANIMATION_EYES, false);
            AddAnimation(0, "Idle", true, 0);
        }
    }
}
