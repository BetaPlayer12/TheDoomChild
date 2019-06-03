using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SnakeManAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_VENOM_ATTACK = "attack1";
        public const string ANIMATION_TAIL_ATTACK = "attack2";
        public const string ANIMATION_DETECTPLAYER = "detect_player";
        public const string ANIMATION_FLINCH = "flinch";
        public const string ANIMATION_FLINCHBACK = "flinch back";
        public const string ANIMATION_IDLE_TO_MOVE = "idletomove";
        public const string ANIMATION_TURN = "turn";

        public void DoVenomAttack() => SetAnimation(0, ANIMATION_VENOM_ATTACK, false);
        public void DoTailAttack() => SetAnimation(0, ANIMATION_TAIL_ATTACK, false);
        public void DoDetectPlayer() => SetAnimation(0, ANIMATION_DETECTPLAYER, false);
        public void DoDamageFront() => SetAnimation(0, ANIMATION_FLINCH, false);
        public void DoDamageBack() => SetAnimation(0, ANIMATION_FLINCHBACK, false);
        public void DoIdleToMove() => SetAnimation(0, ANIMATION_IDLE_TO_MOVE, false);
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoMove() => SetAnimation(0, "move", true);
        public new void DoIdle() => SetAnimation(0, "idle", true);
    }
}

