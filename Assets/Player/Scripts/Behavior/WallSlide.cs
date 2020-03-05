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
            

            if (onWallDetected && !collisionState.grounded)
            {
                var velY = slideVelocity;
                if (inputState.GetButtonValue(inputButtons[0]))
                {
                    velY *= slideMultiplier;
                   
                }
                body2d.velocity = new Vector2(body2d.velocity.x, velY);

                if(inputState.GetButtonValue(inputButtons[3]) && inputState.GetButtonHoldTime(inputButtons[3]) < 0.1f)
                {
                    //facing left
                    if (!facing.isFacingRight)
                        body2d.velocity = new Vector2(forceX, forceY);

                    //facing right
                    else
                        body2d.velocity = new Vector2(forceX * -1f, forceY);
                  
                }
            } else if(onWallDetected && collisionState.grounded)
            {
                Debug.Log("jump here up");
                if (inputState.GetButtonValue(inputButtons[3]) && inputState.GetButtonHoldTime(inputButtons[3]) < 0.1f)
                {
                                                           
                    Debug.Log("jump here");
                    body2d.velocity = new Vector2(body2d.velocity.x * forceX, forceY);


                }                

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
