using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Spiderwoman02Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_DAMAGE_NO_RED = "Damage_No_Red";
        public const string ANIMATION_DEATH_NO_RED = "Death_No_Red";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_WALK = "Walk";
        public const string ANIMATION_WALK_BACKWARDS = "Walk_Backwards";
        public const string ANIMATION_TURN_TEST = "turn test";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDamageNoRed()
        {
            SetAnimation(0, ANIMATION_DAMAGE_NO_RED, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDeathWithRed()
        {
            SetAnimation(0, ANIMATION_DEATH, false);
        }

        public void DoDeathNoRed()
        {
            SetAnimation(0, ANIMATION_DEATH_NO_RED, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoWalk()
        {
            SetAnimation(0, ANIMATION_WALK, true);
        }

        public void DoWalkBackwards()
        {
            SetAnimation(0, ANIMATION_WALK_BACKWARDS, true);
        }

        public void DoTurnTest()
        {
            SetAnimation(0, ANIMATION_TURN_TEST, false);
        }
    }
}
