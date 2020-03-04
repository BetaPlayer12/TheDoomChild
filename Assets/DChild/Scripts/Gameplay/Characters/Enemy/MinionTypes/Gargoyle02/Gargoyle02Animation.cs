using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Gargoyle02Animation : CombatCharacterAnimation
    {
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_PLAYERDETECT = "Detects_Enemy";
        public const string ANIMATION_WINGATTACK = "Attack1";
        public const string ANIMATION_CLAWATTACK = "Attack2";

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoPlayerDetect()
        {
            SetAnimation(0, ANIMATION_PLAYERDETECT, false);
        }

        public void DoWingAttack()
        {
            SetAnimation(0, ANIMATION_WINGATTACK, false);
        }

        public void DoClawAttack()
        {
            SetAnimation(0, ANIMATION_CLAWATTACK, false);
        }

        public void DoStone()
        {
            SetAnimation(0, "animation", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "Flight_Forward", true);
        }

        public void DoMove()
        {
            SetAnimation(0, "Flight_Forward", true);
        }
    }
}
