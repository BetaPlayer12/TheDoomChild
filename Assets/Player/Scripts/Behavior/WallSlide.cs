using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallSlide : WallStick
    {
        public float slideVelocity = -5f;
        public float slideMultiplier = 5f;
        override protected void Update()
        {
           
            base.Update();
            //if (!collisionState.grounded && !collisionState.onWall)
            //{
            //    body2d.sharedMaterial.friction = 0.0f;
                
            //}
            //else
            //{
            //    body2d.sharedMaterial.friction = 0.4f;
                
            //}
            //Debug.Log(body2d.sharedMaterial.friction);
            if (onWallDetected)
            {
               
                var velY = slideVelocity;
                if (inputState.GetButtonValue(inputButtons[0]))
                {
                    velY *= slideMultiplier;
                }
                body2d.velocity = new Vector2(body2d.velocity.x, velY);
            }
        }

        override protected void Onstick()
        {
           
            base.Onstick();
            body2d.velocity = Vector2.zero;
        }

        protected override void Offwall()
        {
            base.Offwall();
        }
    }

}
