using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Scarecrow01Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_TURN_DODGE = "Turn_Dodge";
        public const string ANIMATION_TURN_SIMPLE = "Turn_simple";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoTurnDodge()
        {
            SetAnimation(0, ANIMATION_TURN_DODGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoTurnSimple()
        {
            SetAnimation(0, ANIMATION_TURN_SIMPLE, false);
        }
    }
}
