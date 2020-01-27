using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class LongJump : Jump
    {
        public float longJumpDelay = 0.15f;
        public float longJumpMultiplier = 1.5f;
        public float velocityY;

        public bool canLongJump;
        public bool isLongJumping;

        protected override void Update()
        {

            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
            velocityY = body2d.velocity.y;


            if (!canJump)
            {
                canLongJump = false;
            }
            if (collisionState.grounded && isLongJumping)
            {
                isLongJumping = false;
            }
            base.Update();
            if (canLongJump && !collisionState.grounded && holdTime > longJumpDelay)
            {
                var vel = body2d.velocity;
                body2d.velocity = new Vector2(vel.x, jumpSpeed * longJumpMultiplier);
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

