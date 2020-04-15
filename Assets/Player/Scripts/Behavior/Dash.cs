using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Dash : PlayerBehaviour
    {
        public FaceDirection facing;
        public float dashForce = 200f;
        public float dashDelay = 0.1f;
        public float dashTime;
        public bool dashing = false;
        public bool canDash;
        public float dashCoolDown;


        protected  Vector2 dirFacing;
        protected  float lastDashTime;

        protected virtual void Start()
        {
            facing = GetComponent<FaceDirection>();
        }
        protected virtual void FixedUpdate() {
            float facingDir = facing.isFacingRight ? 1f : -1f;
            var dash = inputState.GetButtonValue(inputButtons[0]);
            var dashHold = inputState.GetButtonHoldTime(inputButtons[0]);

                if (!canDash && lastDashTime > 0.1)
                {
                   
                    lastDashTime -= Time.deltaTime;
                    OnDash(facingDir);
                }else{
                    if (dashing)
                    {
                        StartCoroutine(FinishedDashRoutine());
                    }

                }

                if (dash && dashHold< 0.1f && canDash)
                {
                    ToggleScripts(true);
                    canDash = false;
                    dashing = false;
                    lastDashTime = dashTime;
                }
        }
        //{
        //    float facingDir = facing.isFacingRight ? 1f : -1f;
        //    var dash = inputState.GetButtonValue(inputButtons[0]);
        //    var dashHold = inputState.GetButtonHoldTime(inputButtons[0]);

        //    if (!canDash && lastDashTime > 0.1)
        //    {
        //        ToggleScripts(true);
        //        lastDashTime -= Time.deltaTime;
        //        OnDash(facingDir);
        //    }
        //    else
        //    {
        //        if (!dashing)
        //        {
        //            StartCoroutine(FinishedDashRoutine());
        //        }

        //    }

        //    if (dash && dashHold < 0.1f && canDash)
        //    {
        //        canDash = false;
        //        dashing = false;
        //        lastDashTime = dashTime;
        //    }

        protected virtual void OnDash(float facingDir)
        {
            var vel = body2d.velocity;
            
            body2d.velocity = Vector2.zero;
            body2d.AddForce(new Vector2(facingDir * dashForce, vel.y), ForceMode2D.Force);
            dashing = true;
        }

        IEnumerator FinishedDashRoutine()
        {
            dashing = false;
            yield return new WaitForSeconds(dashCoolDown);
            canDash = true;
            ToggleScripts(true);
        }
    }
}