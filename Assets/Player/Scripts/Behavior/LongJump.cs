using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class LongJump : Jump
    {
        [SerializeField]
        private Dock crouch;
        [SerializeField]
        private WallSlide wallSlide;

        public float longJumpDelay = 0.15f;
        public float longJumpMultiplier = 1.5f;
        public float velocityY;

        public bool canLongJump;
        public bool isLongJumping;
        private bool groundJumpExtra = true;

        protected override void Update()
        {
            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
            velocityY = rigidBody.velocity.y;

            if (!canJump)
            {
                canLongJump = false;
            }

            if (stateManager.isGrounded && isLongJumping)
            {
                isLongJumping = false;
            }

            if (wallSlide.extraJump && canJump)
            {
                jumpsRemaining = 2;
                groundJumpExtra = false;
                wallSlide.extraJump = false;
            }

            if (!stateManager.isGrounded && groundJumpExtra)
            {
                jumpsRemaining = 1;
                groundJumpExtra = false;
            }
            else if (stateManager.isGrounded)
            {
                groundJumpExtra = true;
            }

            base.Update();

            if (canLongJump && !stateManager.isGrounded && holdTime > longJumpDelay && !stateManager.onWall && !stateManager.onWallLeg && !crouch.crouching)
            {
                var vel = rigidBody.velocity;
                rigidBody.velocity = new Vector2(vel.x, jumpSpeed * longJumpMultiplier);
                canLongJump = false;
                isLongJumping = true;
            }
        }

        protected override void OnJump()
        {
            base.OnJump();
            canLongJump = true;
        }
    }
}

