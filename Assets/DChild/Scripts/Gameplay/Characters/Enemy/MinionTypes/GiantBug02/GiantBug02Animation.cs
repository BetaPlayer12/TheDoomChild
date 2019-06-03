using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBug02Animation : CombatCharacterAnimation
    {
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_MOVE = "Walk";
        public const string ANIMATION_FLINCH_FRONT = "Flinch";
        public const string ANIMATION_FLINCH_BACK = "Flinch(back)";
        public const string ANIMATION_DETECT_PLAYER = "Player_detect";

        public const string ANIMATION_ACID_SPIT_ATTACK = "Attack01";
        public const string ANIMATION_PRE_ATTACK = "Attack02pre";
        public const string ANIMATION_JUMP_ATTACK = "Attack02";
        public const string ANIMATION_JUMP_TURN = "Attack02(turn)(inplace)";
       
        //Basic Movements
        public void DoPlayerDetect() => SetAnimation(0, ANIMATION_DETECT_PLAYER, false);
        public void DoMove() => SetAnimation(0, ANIMATION_MOVE, true).TimeScale = 2f;
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoFrontFlinch() => SetAnimation(0, ANIMATION_FLINCH_FRONT, false); 
        public void DoBackFlinch() => SetAnimation(0, ANIMATION_FLINCH_BACK, false);


        //Attacks
        public void DoAcidSpitAttack() => SetAnimation(0, ANIMATION_ACID_SPIT_ATTACK, false);
        public void DoPreJumpAttack() => SetAnimation(0, ANIMATION_PRE_ATTACK, false);
        public void DoJumpAttack() => SetAnimation(0, ANIMATION_JUMP_ATTACK, false);
        public void DoJumpTurn() => SetAnimation(0, ANIMATION_JUMP_TURN, false);
     
    }
}