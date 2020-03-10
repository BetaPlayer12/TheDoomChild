using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallSlide : WallStick
    {
        private FaceDirection facing;
        private LongJump jumping;
        public float slideVelocity = -5f;
        public float slideMultiplier = 5f;
        public float velocityX;
        public float forceX;
        public float forceY;
                

        private void Start()
        {
            facing = GetComponent<FaceDirection>();
            jumping = GetComponent<LongJump>();
        }

        override protected void Update()
        {
           
            base.Update();
            var wallStickLeft = inputState.GetButtonValue(inputButtons[1]);
            var wallStickRight = inputState.GetButtonValue(inputButtons[2]);

            velocityX = body2d.velocity.x;

            if (!collisionState.grounded && !onWallDetected && !wallSticking)
            {
                
                body2d.sharedMaterial.friction = 0;
                capsuleCollider.enabled = false;
                capsuleCollider.enabled = true;

            }
            else
            {

                body2d.sharedMaterial.friction = 0.4f;
                capsuleCollider.enabled = false;
                capsuleCollider.enabled = true;

            }

            //set jumpForce if onWall and isGrounded
            if (onWallDetected && wallGrounded || groundWallStick && collisionState.onWallLeg && collisionState.grounded)
            {
                forceX = 10;
                forceY = 50;
            }
            if(onWallDetected && !wallGrounded)
            {
                forceX = 250;
                forceY = 250;
            }


            //wall slide
            if (onWallDetected && !collisionState.grounded)
            {

                var velY = slideVelocity;


                if (inputState.GetButtonValue(inputButtons[0]))
                {
                    velY *= slideMultiplier;

                }else if (inputState.GetButtonValue(inputButtons[4]))
                {
                    velY *= -slideMultiplier;
                }
                body2d.velocity = new Vector2(body2d.velocity.x, velY);                               
            }

            //jumping beside wall 
            if ((onWallDetected || groundWallStick) && inputState.GetButtonValue(inputButtons[3]) && inputState.GetButtonHoldTime(inputButtons[3]) < 0.1f)
            {
                //facing left
                if (!facing.isFacingRight)
                    body2d.velocity = new Vector2(forceX, forceY);
                //facing right
                else
                    body2d.velocity = new Vector2(forceX * -1f, forceY);
            }

        }

        override protected void Onstick()
        {
           
            base.Onstick();
            // body2d.velocity = Vector2.zero;
           
        }

        protected override void Offwall()
        {
            base.Offwall();
        }
    }

}
