using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DeformedCultAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "CULT_MINION_Attack1";
        public const string ANIMATION_MINION_DEATH = "CULT_MINION_Death";
        public const string ANIMATION_DETECT1 = "CULT_MINION_Detect1";
        public const string ANIMATION_DETECT2 = "CULT_MINION_Detect2";
        public const string ANIMATION_FLINCH = "CULT_MINION_Flinch";
        public const string ANIMATION_MINION_IDLE = "CULT_MINION_Idle";
        public const string ANIMATION_TURN = "CULT_MINION_Turn";
        public const string ANIMATION_WALK = "CULT_MINION_Walk";
        public const string ANIMATION_WALK2 = "CULT_MINION_Walk2";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
            AddAnimation(0, "CULT_MINION_Idle", true, 0);
        }

        public void DoMinionDeath()
        {
            SetAnimation(0, ANIMATION_MINION_DEATH, false);
        }

        public void DoDetect1()
        {
            SetAnimation(0, ANIMATION_DETECT1, false);
            AddAnimation(0, "CULT_MINION_Idle", true, 0);
        }

        public void DoDetect2()
        {
            SetAnimation(0, ANIMATION_DETECT2, false);
            AddAnimation(0, "CULT_MINION_Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "CULT_MINION_Idle", true, 0);
        }

        public void DoMinionIdle()
        {
            SetAnimation(0, ANIMATION_MINION_IDLE, true);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoWalk()
        {
            SetAnimation(0, ANIMATION_WALK, true);
        }

        public void DoWalk2()
        {
            SetAnimation(0, ANIMATION_WALK2, true);
        }
    }
}
