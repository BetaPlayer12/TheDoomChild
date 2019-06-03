using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BladeMarrionette3Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "attack";
        public const string ANIMATION_ATTACK_LONG = "attack long";
        public const string ANIMATION_ATTACK_TWITCH = "attack twitch";
        public const string ANIMATION_DAMAGE = "damage";
        public const string ANIMATION_DEATH1 = "death";
        public const string ANIMATION_IDLE1 = "idle";
        public const string ANIMATION_MOVE = "move";
        public const string ANIMATION_TURN = "turn";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoAttackLong()
        {
            SetAnimation(0, ANIMATION_ATTACK_LONG, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoAttackTwitch()
        {
            SetAnimation(0, ANIMATION_ATTACK_TWITCH, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_IDLE1, true);
        }

        public void DoDeath1()
        {
            SetAnimation(0, ANIMATION_DEATH1, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
