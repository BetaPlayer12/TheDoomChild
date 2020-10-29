using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class ShadowSlide : Dash
    {
        public bool shadowDashing = false;
        public bool shadowMode = false;
        public float shadowlimeter = 0.3f; // sweetspot is 0.3f

        private float shadowSlideTimer;
        private bool isCeilingTouch;

        override protected void FixedUpdate()
        {
            base.FixedUpdate();

            //var dash = inputState.GetButtonValue(inputButtons[0]);
            //var down = inputState.GetButtonValue(inputButtons[1]);

            //float faceDir = facing.isFacingRight ? 1 : -1;
            //var vel = rigidBody.velocity;

            //if (shadowMode)
            //{
            //    shadowSlideTimer += Time.deltaTime;

            //    if (stateManager.isCeilingTouch)
            //    {
            //        shadowMode = true;
            //        shadowDashing = true;
            //        //rigidBody.AddForce(new Vector2(faceDir * , vel.y), ForceMode2D.Force);
            //    }
            //    else
            //    {
            //        if (shadowSlideTimer > shadowlimeter)
            //        {
            //            shadowMode = false;
            //            shadowDashing = false;
            //            shadowSlideTimer = 0f;
            //        }
            //    }
            //}

            //if (dash && !shadowMode)
            //{
            //    if (down && stateManager.isGrounded)
            //    {
            //        shadowDashing = true;
            //        shadowMode = true;
            //    }
            //}
        }

        //protected override void OnDash(float faceDir)
        //{
        //    base.OnDash(faceDir);
        //}
    }
}


