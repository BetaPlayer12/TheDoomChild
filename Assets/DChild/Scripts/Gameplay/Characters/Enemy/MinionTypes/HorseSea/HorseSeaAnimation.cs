using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class HorseSeaAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_IDLE_MOVE = "Idle/Move";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_TURN2 = "Turn2";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
            AddAnimation(0, "Idle/Move", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
            AddAnimation(0, "Idle/Move", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "Idle/Move", true, 0);
        }

        public void DoIdleMove()
        {
            SetAnimation(0, ANIMATION_IDLE_MOVE, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoTurn2()
        {
            SetAnimation(0, ANIMATION_TURN2, false);
        }
    }
}
