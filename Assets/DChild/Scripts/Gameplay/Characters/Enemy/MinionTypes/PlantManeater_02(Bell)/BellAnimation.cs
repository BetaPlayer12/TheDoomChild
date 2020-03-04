using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BellAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_VINE_ATTACK = "Attack1_Revised";
        public const string ANIMATION_ACID_SPIT = "Attack2";
        public const string EVENT_VINESPAWN = "spikes";
        public const string EVENT_POISONSPIT = "poisonspit";

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoVineAttack()
        {
            SetAnimation(0, ANIMATION_VINE_ATTACK, false);
        }

        public void DoAcidSpit()
        {
            SetAnimation(0, ANIMATION_ACID_SPIT, false);
        }
    }
}
