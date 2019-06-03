using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DarkPriestLadyAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK_ENRAGED = "Attack_Enraged Mode_Scratch_Kick";
        public const string ANIMATION_DETECT_PLAYER = "DetectPLayer";
        public const string ANIMATION_FLINCH1 = "Flinch1";
        public const string ANIMATION_FLINCH2 = "Flinch2";
        public const string ANIMATION_IDLE_MOVE = "Idle/Move";
        public const string ANIMATION_IDLE2_MOVE = "Idle2/Move";
        public const string ANIMATION_STUN = "Stun";
        public const string ANIMATION_TURN = "Turn";
        #endregion
        
        public void DoAttack1(bool isHostile)
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
        }

        public void DoAttackEnraged(bool isHostile)
        {
            SetAnimation(0, ANIMATION_ATTACK_ENRAGED, false);
            AddAnimation(0, isHostile == true ? ANIMATION_IDLE2_MOVE : ANIMATION_IDLE_MOVE, true, 0);
        }

        public void DoDetect(bool isHostile)
        {
            SetAnimation(0, ANIMATION_DETECT_PLAYER, false);
            AddAnimation(0, isHostile == true ? ANIMATION_IDLE2_MOVE : ANIMATION_IDLE_MOVE, true, 0);
        }

        public void DoFlinch1(bool isHostile)
        {
            SetAnimation(0, ANIMATION_FLINCH1, false);
            AddAnimation(0, isHostile == true ? ANIMATION_IDLE2_MOVE : ANIMATION_IDLE_MOVE, true, 0);
        }

        public void DoFlinch2(bool isHostile)
        {
            SetAnimation(0, ANIMATION_FLINCH2, false);
            AddAnimation(0, isHostile == true ? ANIMATION_IDLE2_MOVE : ANIMATION_IDLE_MOVE, true, 0);
        }

        public void DoIdleMove()
        {
            SetAnimation(0, ANIMATION_IDLE_MOVE, true);
        }

        public void DoIdle2Move()
        {
            SetAnimation(0, ANIMATION_IDLE2_MOVE, true);
        }

        public void DoStun()
        {
            SetAnimation(0, ANIMATION_STUN, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
