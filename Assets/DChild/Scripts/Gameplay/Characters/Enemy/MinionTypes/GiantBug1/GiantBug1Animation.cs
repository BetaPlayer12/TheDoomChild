using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBug1Animation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_ATTACK = "Buggiant3_Attack";
        public const string ANIMATION_DEATH1 = "Buggiant3_Death";
        public const string ANIMATION_DETECT = "Buggiant3_Detect";
        public const string ANIMATION_FLINCH = "Buggiant3_Flinch";
        public const string ANIMATION_FLYING_TURN2 = "Buggiant3_Flying_Turn2";
        public const string ANIMATION_IDLE_FLYING = "Buggiant3_Idle_Flying";
        public const string ANIMATION_IDLE_GROUND = "Buggiant3_Idle_Ground";
        public const string ANIMATION_IDLE_WITH_MOVEMENT = "Buggiant3_Idle_With_Movement";
        public const string ANIMATION_IDLE_WITH_MOVEMENT2 = "Buggiant3_Idle_With_Movement2";
        public const string ANIMATION_INPLACE_FLYING = "Buggiant3_Inplace_Flying";
        public const string ANIMATION_MOVE_FLYING = "Buggiant3_Move_Flying";
        public const string ANIMATION_MOVE_GROUND = "Buggiant3_Move_Ground";
        #endregion

        public void DoAttack(bool isGrounded)
        {
            SetAnimation(0, ANIMATION_ATTACK, false);
            AddAnimation(0, isGrounded ? ANIMATION_IDLE_GROUND : ANIMATION_IDLE_FLYING, true, 0);
        }

        public void DoDeath1(bool isGrounded)
        {
            SetAnimation(0, ANIMATION_DEATH1, false);
            AddAnimation(0, isGrounded ? ANIMATION_IDLE_GROUND : ANIMATION_IDLE_FLYING, true, 0);
        }

        public void DoDetect(bool isGrounded)
        {
            SetAnimation(0, ANIMATION_DETECT, false);
            AddAnimation(0, isGrounded ? ANIMATION_IDLE_GROUND : ANIMATION_IDLE_FLYING, true, 0);
        }

        public void DoFlinch(bool isGrounded)
        {
            SetAnimation(0, ANIMATION_FLINCH, false);
            AddAnimation(0, isGrounded ? ANIMATION_IDLE_GROUND : ANIMATION_IDLE_FLYING, true, 0);
        }

        public void DoFlyingTurn2()
        {
            SetAnimation(0, ANIMATION_FLYING_TURN2, false);
        }

        public void DoIdleState(bool isGrounded)
        {
            SetAnimation(0,isGrounded ? ANIMATION_IDLE_GROUND : ANIMATION_IDLE_FLYING, true);
        }

        public void DoIdleFlying()
        {
            SetAnimation(0, ANIMATION_IDLE_FLYING, true);
        }

        public void DoIdleGround()
        {
            SetAnimation(0, ANIMATION_IDLE_GROUND, true);
        }

        public void DoIdleMovement()
        {
            SetAnimation(0, ANIMATION_IDLE_WITH_MOVEMENT, true);
        }

        public void DoIdleMovement2()
        {
            SetAnimation(0, ANIMATION_IDLE_WITH_MOVEMENT2, true);
        }

        public void DoInplaceFlying()
        {
            SetAnimation(0, ANIMATION_INPLACE_FLYING, true);
        }

        public void DoMove(bool isGrounded)
        {
            SetAnimation(0, isGrounded ? ANIMATION_MOVE_GROUND : ANIMATION_INPLACE_FLYING, true);
        }

        public void DoMoveFlying()
        {
            SetAnimation(0, ANIMATION_MOVE_FLYING, true);
        }

        public void DoMoveGround()
        {
            SetAnimation(0, ANIMATION_MOVE_GROUND, true);
        }
    }
}
