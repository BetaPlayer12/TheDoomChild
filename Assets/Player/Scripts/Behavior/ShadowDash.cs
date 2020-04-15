using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class ShadowDash : Dash
    {
        
        public bool shadowDashing = false;

    
       

        override protected void FixedUpdate()
        {
            base.FixedUpdate();
           

            var dash = inputState.GetButtonValue(inputButtons[0]);
            var down = inputState.GetButtonValue(inputButtons[1]);
            float faceDir = facing.isFacingRight ? 1 : -1;
            if (!dashing)
            {
                shadowDashing = false;
                
            }
            if (shadowDashing)
            {
                if (collisionState.isCeilingTouch)
                {
                    Debug.Log(faceDir + " " + collisionState.isCeilingTouch);
                }
                
            }


            if (dash)
            {
                if (down && collisionState.grounded)
                {
                    shadowDashing = true;
                }
                else
                {
                    shadowDashing = false;
                }
            }
        }
        protected override void OnDash(float faceDir)
        {
            base.OnDash(faceDir);
        }
    }
}


