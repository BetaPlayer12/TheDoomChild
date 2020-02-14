using UnityEngine;
using System.Collections;

namespace PlayerNew
{
    public class Jump : PlayerBehaviour
    {

        public float jumpSpeed = 200f;
        public float jumpDelay = .1f;
        public int jumpCount = 2;

        private Jog jogging;

        protected float lastJumpTime = 0;
        protected int jumpsRemaining = 0;

        // Use this for initialization
        void Start()
        {
            jogging = GetComponent<Jog>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {

            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);



            if (collisionState.grounded)
            {
               
                if (canJump && holdTime < 0.1f)
                {
                    jumpsRemaining = jumpCount - 1;
                    OnJump();
                }
            }
            else
            {
                

                if (canJump && holdTime < 0.1f && Time.time - lastJumpTime > jumpDelay)
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


        }
    }
}

