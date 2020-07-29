using UnityEngine;
using System.Collections;

namespace PlayerNew
{
    public class Jump : PlayerBehaviour
    {
        public float jumpSpeed = 200f;
        public float jumpDelay = .1f;
        public int jumpCount = 2;

        private Dock crouching;
        private Jog jogging;

        protected float lastJumpTime = 0;
        public int jumpsRemaining = 0;

        void Start()
        {
            jogging = GetComponent<Jog>();
            crouching = GetComponent<Dock>();
        }

        protected virtual void Update()
        {
            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);

            if (stateManager.isGrounded)
            {
                stateManager.isJumping = false;

                if (canJump && holdTime < 0.1f && !stateManager.onWall && !stateManager.onWallLeg && !crouching.crouching)
                {
                    jumpsRemaining = jumpCount - 1;
                    OnJump();
                }
            }
            else
            {
                if (canJump && holdTime < 0.1f && Time.time - lastJumpTime > jumpDelay && !stateManager.onWall && !stateManager.onWallLeg && !crouching.crouching)
                {
                    if (jumpsRemaining > 0)
                    {
                        OnJump();
                        jumpsRemaining--;
                    }
                }
            }
        }

        protected virtual void OnJump()
        {
            stateManager.isIdle = false;
            stateManager.isJumping = true;

            var vel = rigidBody.velocity;
            lastJumpTime = Time.time;
            rigidBody.velocity = new Vector2(vel.x, jumpSpeed);
            rigidBody.sharedMaterial.friction = 0f;
        }
    }
}

