using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Spectre3Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Attack";
        public const string ANIMATION_ATTACK2 = "Attack2";
        public const string ANIMATION_ATTACK_ANTICIPATION = "Attack Anticipation";
        public const string ANIMATION_DETECT_PLAYER = "Detect Player";
        public const string ANIMATION_FADE_IN = "Fade In";
        public const string ANIMATION_FADE_OUT = "Fade Out";
        public const string ANIMATION_HURT = "Hurt";
        public const string ANIMATION_MOVE = "Move";
        #endregion

        public void DoAttack()
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttack2()
        {
            SetAnimation(0, ANIMATION_ATTACK2, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoAttackAnticipation()
        {
            SetAnimation(0, ANIMATION_ATTACK_ANTICIPATION, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT_PLAYER, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFadeIn()
        {
            SetAnimation(0, ANIMATION_FADE_IN, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoFadeOut()
        {
            SetAnimation(0, ANIMATION_FADE_OUT, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoHurt()
        {
            SetAnimation(0, ANIMATION_HURT, false);
            AddAnimation(0, "Idle", true, 0);
        }

        public void DoMove()
        {
            SetAnimation(0, ANIMATION_MOVE, true);
        }
    }
}