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

            if (!dashing)
            {
                shadowDashing = false;
                Debug.Log("Not Dashing");
            }
            else
            {
                Debug.Log("Dashing");
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
    }
}


