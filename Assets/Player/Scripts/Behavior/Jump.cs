using UnityEngine;
using System.Collections;

namespace PlayerNew
{
    public class Jump : PlayerBehaviour
    {

        public float jumpSpeed = 200f;
        public float jumpDelay = .1f;
        public int jumpCount = 2;

        private Crouch crouching;
        private Jog jogging;

        protected float lastJumpTime = 0;
        protected int jumpsRemaining = 0;

        // Use this for initialization
        void Start()
        {
            jogging = GetComponent<Jog>();
            crouching = GetComponent<Crouch>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {

            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);



            if (collisionState.grounded)
            {
               
               
                if (canJump && holdTime < 0.1f && !collisionState.onWall && !collisionState.onWallLeg && !crouching.crouching)
                {
                   
                    jumpsRemaining = jumpCount - 1;
                    OnJump();
                }
            }
            else
            {
                

                if (canJump && holdTime < 0.1f && Time.time - lastJumpTime > jumpDelay && !collisionState.onWall && !collisionState.onWallLeg && !crouching.crouching)
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
            var vel = body2d.velocity;
            lastJumpTime = Time.time;
            body2d.velocity = new Vector2(vel.x, jumpSpeed);
            body2d.sharedMaterial.friction = 0f;


        }
    }
}

