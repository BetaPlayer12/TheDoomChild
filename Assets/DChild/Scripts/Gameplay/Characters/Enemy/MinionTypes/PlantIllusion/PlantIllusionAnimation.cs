using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PlantIllusionAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK1 = "Attack1";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_ATTACK1_HIDDEN = "Attack1_with_hide";
        public const string ANIMATION_ATTACK2_HIDDEN = "Attac2_with_hide";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_SURPRISE = "Surprise";
        public const string ANIMATION_TURN = "Turn";
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

        public void DoAttack1Hidden()
        {
            SetAnimation(0, ANIMATION_ATTACK1_HIDDEN, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack2Hidden()
        {
            SetAnimation(0, ANIMATION_ATTACK2_HIDDEN, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoSurprise()
        {
            SetAnimation(0, ANIMATION_SURPRISE, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}
