using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SlugAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_DAMAGE = "Damage";
        public const string ANIMATION_MOVE = "Move";
        public const string ANIMATION_SPIKE_PROJECTILES = "Spike_Projectiles";
        public const string ANIMATION_SPIT = "Spit";
        public const string ANIMATION_TURN = "Turn";
        public const string EVENT_PROJECTILESPIKES = "";
        public const string EVENT_PROJECTILESPIT = "";
        #endregion

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_DAMAGE, false);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }

        public void DoHostileMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true).TimeScale = 1.5f;
        }

        public void DoProjectiles()
        {
            SetAnimation(0, ANIMATION_SPIKE_PROJECTILES, false);
        }

        public void DoSpit()
        {
            SetAnimation(0, ANIMATION_SPIT, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }
    }
}