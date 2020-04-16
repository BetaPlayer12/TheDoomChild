using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class ShadowDash : Dash
    {
        
        public bool shadowDashing = false;
        public bool shadowMode = false;

        private bool isCeilingTouch;




        override protected void FixedUpdate()
        {
            base.FixedUpdate();
           

            var dash = inputState.GetButtonValue(inputButtons[0]);
            var down = inputState.GetButtonValue(inputButtons[1]);

            float faceDir = facing.isFacingRight ? 1 : -1;
            var vel = body2d.velocity;


            if (shadowMode)
            {
               

                if (collisionState.isCeilingTouch)
                {
                    shadowMode = true;
                    shadowDashing = true;
                    body2d.AddForce(new Vector2(faceDir * dashForce, vel.y), ForceMode2D.Force);
                }
                else
                {
                    shadowMode = false;
                    shadowDashing = false;
                }

                Debug.Log(collisionState.isCeilingTouch);
            }


            if (dash && !shadowMode)
            {
                if (down && collisionState.grounded)
                {
                    shadowDashing = true;
                    shadowMode = true;
                }
            }
        }
        protected override void OnDash(float faceDir)
        {
            base.OnDash(faceDir);
        }
    }
}


