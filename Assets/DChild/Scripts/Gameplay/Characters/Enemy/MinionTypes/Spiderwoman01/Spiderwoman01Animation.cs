using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Spiderwoman01Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "attack 1 fall";
        public const string ANIMATION_ATTACK2 = "attack 2  Leap";
        public const string ANIMATION_ATTACK3 = "attack 3 backward spit";
        public const string ANIMATION_DAMAGE = "damage";
        public const string ANIMATION_DEATH1 = "death";
        public const string ANIMATION_IDLE1 = "idle";
        public const string ANIMATION_IDLE2 = "idle 2";
        public const string ANIMATION_SLASH = "slash";
        public const string ANIMATION_TURN = "turn";
        public const string ANIMATION_WALK = "walk";
        public const string ANIMATION_WALK_IN_PLACE = "walk_inPlace";
        #endregion

        public void DoAttack1()
        {
            SetAnimation(0, ANIMATION_ATTACK1, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoAttack3()
        {
            SetAnimation(0, ANIMATION_ATTACK3, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDamage1()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoDeath1()
        {
            SetAnimation(0, ANIMATION_DEATH1, false);
        }

        public void DoIdle1()
        {
            SetAnimation(0, ANIMATION_IDLE1, true);
        }

        public void DoIdle2()
        {
            SetAnimation(0, ANIMATION_IDLE2, true);
        }

        public void DoSlash()
        {
            SetAnimation(0, ANIMATION_SLASH, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoTurn
            ()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoWalk()
        {
            SetAnimation(0, ANIMATION_WALK, true);
        }

        public void DoWalkInPlace()
        {
            SetAnimation(0, ANIMATION_WALK_IN_PLACE, true);
        }
    }
}
