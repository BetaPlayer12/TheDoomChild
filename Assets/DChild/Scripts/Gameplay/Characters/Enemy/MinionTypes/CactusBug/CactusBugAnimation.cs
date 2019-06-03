using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CactusBugAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_FLINCH = "Damage";
        public const string ANIMATION_SPIT_ATTACK = "Spit_Attack";
        public const string ANIMATION_JUMP_ANTICIPATION = "Jump_Preparation";
        public const string ANIMATION_JUMP_ATTACK = "Jump_Attack_Root";
        public const string ANIMATION_BURROW = "Burrow";
        public const string ANIMATION_BURROW_REVEAL = "Burrow_Reveal";

        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoFlinch() => SetAnimation(0, ANIMATION_FLINCH, false);
        public void DoSpitAttack() => SetAnimation(0, ANIMATION_SPIT_ATTACK, false);
        public void DoJumpAnticipation() => SetAnimation(0, ANIMATION_JUMP_ANTICIPATION, false);
        public void DoJumpAttack() => SetAnimation(0, ANIMATION_JUMP_ATTACK, false);
        public void DoBurrow() => SetAnimation(0, ANIMATION_BURROW, false);
        public void DoBurrowReveal() => SetAnimation(0, ANIMATION_BURROW_REVEAL, false);
        public void DoMove() => SetAnimation(0, "Move", true);
        public void DoBurrowIdle() => SetAnimation(0, "Burrowed_Idle", true);
    }
}
