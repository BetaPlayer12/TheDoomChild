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

        public bool upHold;
        public bool downHold;
        public bool extraJump = false;

        public bool ledgeGrabState = false;
        Animator m_anim;



        private void Start()
        {
            facing = GetComponent<FaceDirection>();
            jumping = GetComponent<LongJump>();
            m_anim = GetComponent<Animator>();

        }

        override protected void Update()
        {
           
            base.Update();
            var wallStickDown = inputState.GetButtonValue(inputButtons[0]);
            var wallStickLeft = inputState.GetButtonValue(inputButtons[1]);
            var wallStickRight = inputState.GetButtonValue(inputButtons[2]);
            var wallStickJump = inputState.GetButtonValue(inputButtons[3]);
            var wallStickJumpHold = inputState.GetButtonHoldTime(inputButtons[3]);
            var wallStickUp = inputState.GetButtonValue(inputButtons[4]);
            //
            //ledgeGrabState = collisionState.grabLedge;

            if (wallSticking)
            {
                upHold = wallStickUp;
                downHold = wallStickDown;
            }
                   
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
            if(onWallDetected && !wallGrounded && !collisionState.grounded)
            {
/*              forceX = 250;
                forceY = 250;*/


            }


            //wall slide
            if (onWallDetected && !collisionState.grounded)
            {

                if (collisionState.grabLedge && onWallDetected && !collisionState.grounded)
                {
                    slideVelocity = 0f;
                    body2d.gravityScale = 0;
                    ClimbWall();
                }
                else
                    slideVelocity = -5f;

                var velY = slideVelocity;



                if (wallStickDown)
                {
                    velY *= slideMultiplier;
                    body2d.gravityScale = 0;
                    body2d.drag = 100;

                }
                else if (wallStickUp)
                {
                    velY *= -slideMultiplier;
                    body2d.gravityScale = 0;
                    body2d.drag = 100;
                }

                if (wallStickLeft || wallStickRight)
                {
                    extraJump = true;
                }                                            
                body2d.velocity = new Vector2(body2d.velocity.x, velY);                               
            }

            //jumping beside wall 
            if ((onWallDetected || groundWallStick) && wallStickJump && wallStickJumpHold < 0.1f && !upHold)
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
        /*
            base.Onstick();
            body2d.velocity = Vector2.zero;
        */

        }

        protected override void Offwall()
        {
            base.Offwall();
        }

        protected void ClimbWall()
        {
            ledgeGrabState = true;        
        }


        private void StartClimbWall()
        {
            Debug.Log("start climb wall");
            
            ledgeGrabState = false;
        }

        private void FinishClimbWall()
        {
            Debug.Log("finish climb wall");
            //transform.position = collisionState.newPos;
            ledgeGrabState = false;
        }
    }

}
