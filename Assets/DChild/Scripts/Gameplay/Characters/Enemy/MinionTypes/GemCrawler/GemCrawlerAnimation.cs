using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GemCrawlerAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_FLINCH = "Damage";
        public const string EVENT_CRYSTALSPIKE = "Projectile Spawn";

        public void DoCrystalSpike()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "idle", true, 0);
        }

        public void DoTurn()
        {
            SetAnimation(0, ANIMATION_TURN, false);
        }

        public void DoMove()
        {
            SetAnimation(0, "Move", true);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "idle", true);
        }

        public override void DoDeath()
        {
            SetAnimation(0, "Death", true);
        }

        public void DoFlinch()
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
        }
    }
}
