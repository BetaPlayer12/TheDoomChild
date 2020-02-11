using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Dash : PlayerBehaviour
    {
        private FaceDirection facing;
        public float dashForce = 200f;
        public float dashDelay = 0.1f;
        public float dashTime;
        public bool dashing = false;
        

        private Vector2 dirFacing;
        private float lastDashTime;

        private void Start()
        {
            facing = GetComponent<FaceDirection>();
        }
        private void Update()
        {

         
            float facingDir = facing.isFacingRight ? 1f : -1f;
            var dash = inputState.GetButtonValue(inputButtons[0]);
            var dashHold = inputState.GetButtonHoldTime(inputButtons[0]);


            //Debug.Log(Time.deltaTime + " - " + lastDashTime);    

            //    if (dash && dashHold < 0.1f && Time.deltaTime - lastDashTime > dashDelay)
            //    {
            //        if (!collisionState.grounded)
            //        {
            //            body2d.gravityScale = 0;
            //        }

            //        OnDash(facingDir);
            //    }
            //    else
            //    {
            //        if (body2d.gravityScale <= 0)
            //        {
            //            if (body2d.drag >= 100f)
            //            {
            //                body2d.drag = 0;
            //            }
            //            body2d.gravityScale = 20f;
            //        }
            //        dashing = false;
            //    }

            if (dash && dashHold < 0.1f)
            {
                Debug.Log("dashing here");
                OnDash(facingDir);
            }
            else
            {
                if (dash && dashHold < 0.1f && Time.deltaTime - lastDashTime > dashDelay)
                {
                    if (!collisionState.grounded)
                    {
                        body2d.gravityScale = 0;
                    }

                    OnDash(facingDir);
                }
                else
                {
                    if (body2d.gravityScale <= 0)
                    {
                        if (body2d.drag >= 100f)
                        {
                            body2d.drag = 0;
                        }
                        body2d.gravityScale = 20f;
                    }
                    dashing = false;
                }
            }


            //if (dashing)
            //{
            //    ToggleScripts(false);
            //    if (dashTime > 0.01)
            //    {
            //        var vel = body2d.velocity;
            //        body2d.AddForce(new Vector2(facingDir * dashForce, vel.y), ForceMode2D.Force);
            //        dashTime -= Time.deltaTime;

            //    }
            //    else
            //    {
            //        dashing = false;
            //        dashTime = timeToDash;
            //        StartCoroutine(FinishedDashRoutine());
            //        //turn on gravity

            //    }
            //}
            //else
            //{
            //    if (canDash)
            //    {
            //        dashing = true;
            //        //turn off gravity
            //    }
            //}

        }

        private void OnDash(float facingDir)
        {
            var vel = body2d.velocity;
            lastDashTime -= Time.time;
            body2d.velocity = Vector2.zero;
            body2d.AddForce(new Vector2(facingDir * dashForce, vel.y), ForceMode2D.Force);
            dashing = true;
        }

        IEnumerator FinishedDashRoutine()
        {
            
            yield return new WaitForSeconds(dashTime);
           
            body2d.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log("finish dashing");
            ToggleScripts(true);

        }
    }
}