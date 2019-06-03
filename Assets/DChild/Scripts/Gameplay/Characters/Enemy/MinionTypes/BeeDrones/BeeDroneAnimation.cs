using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class BeeDroneAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_FORWARD = "Flight_Forward";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_DAMAGE = "Damage";

        public void DoForward() => SetAnimation(0, ANIMATION_FORWARD, true);
        public void DoFlinch() => SetAnimation(0, ANIMATION_DAMAGE, false, 0);
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoIdle2() => SetAnimation(0, "Idle2", true);        
    }
}
