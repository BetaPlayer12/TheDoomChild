using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CultServantAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK_CASTING_SPELL = "Attack_Casting spell";
        public const string ANIMATION_ATTACK_CONJURE_BOOKS = "Attack_ConjureBooks";
        public const string ANIMATION_DETECT = "Detect";
        public const string ANIMATION_FLINCH_BOOK_MOVE = "Flinch_Book_Move";
        public const string ANIMATION_IDLE_WITH_BOOK = "Idle_With_Book";
        public const string ANIMATION_IDLE_WITHOUT_BOOK = "Idle_Without_Book";
        public const string ANIMATION_MOVE_WITH_BOOK = "Move_With_Book";
        public const string ANIMATION_MOVE_WITHOUT_BOOK = "Move_Without_Book";
        public const string ANIMATION_TELEPORT = "Teleport";
        public const string ANIMATION_TELEPORT_AWAY = "Teleport_Away";
        public const string ANIMATION_TURN_WITH_BOOK = "Turn_With_Book";
        public const string ANIMATION_TURN_WITHOUT_BOOK = "Turn_Withoutbook";
        #endregion

        public void DoAttackCastingSpell()
        {
            SetAnimation(0, ANIMATION_ATTACK_CASTING_SPELL, false);
            AddAnimation(0, "Idle_With_Book", true, 0);
        }

        public void DoAttackConjureBooks()
        {
            SetAnimation(0, ANIMATION_ATTACK_CONJURE_BOOKS, false);
            AddAnimation(0, "Idle_With_Book", true, 0);
        }

        public void DoDetect()
        {
            SetAnimation(0, ANIMATION_DETECT, false);
            AddAnimation(0, "Idle_With_Book", true, 0);
        }

        public void DoFlinchBookMove()
        {
            SetAnimation(0, ANIMATION_FLINCH_BOOK_MOVE, false);
            AddAnimation(0, "Idle_With_Book", true, 0);
        }

        public void DoIdleWithBook()
        {
            SetAnimation(0, ANIMATION_IDLE_WITH_BOOK, true);
        }

        public void DoIdleWithoutBook()
        {
            SetAnimation(0, ANIMATION_IDLE_WITHOUT_BOOK, true);
        }

        public void DoMoveWithBook()
        {
            SetAnimation(0, ANIMATION_MOVE_WITH_BOOK, true);
        }

        public void DoMoveWithoutBook()
        {
            SetAnimation(0, ANIMATION_MOVE_WITHOUT_BOOK, true);
        }

        public void DoTeleport()
        {
            SetAnimation(0, ANIMATION_TELEPORT, false);
            AddAnimation(0, "Idle_With_Book", true, 0);
        }

        public void DoTeleportAway()
        {
            SetAnimation(0, ANIMATION_TELEPORT_AWAY, false);
            AddAnimation(0, "Idle_With_Book", true, 0);
        }

        public void DoTurnWithBook()
        {
            SetAnimation(0, ANIMATION_TURN_WITH_BOOK, false);
        }

        public void DoTurnWithoutBook()
        {
            SetAnimation(0, ANIMATION_TURN_WITHOUT_BOOK, false);
        }
    }
}
