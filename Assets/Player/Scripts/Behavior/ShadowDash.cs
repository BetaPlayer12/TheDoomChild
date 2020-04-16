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


            
            if (shadowMode)
            {
               

                if (collisionState.isCeilingTouch)
                {
                    shadowMode = true;
                    shadowDashing = true;
                }
                else
                {
                    shadowMode = false;
                    shadowDashing = false;
                }
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


