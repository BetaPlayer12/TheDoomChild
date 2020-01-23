using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Jog
{
    public float timeToDash;
    private float dashTime;
    public float dashForce;
    public bool dashing = false;

    protected override void Update()
    {

        if(dashTime <= 0)
        {
            var canDash = inputState.GetButtonValue(inputButtons[2]);
            base.Update();

            if (canDash && !dashing)
            {
                var vel = body2d.velocity;
                body2d.velocity = new Vector2(vel.x * dashForce, vel.y);
                dashing = true;
                dashTime = timeToDash;
            }
            else
            {
                dashing = false;
            }

           
        }
        else
        {
            dashTime -= Time.deltaTime;
        }

        //var canDash = inputState.GetButtonValue(inputButtons[2]);
       // base.Update();
        //if (canDash && dashTime > 0.01)
        //{
        //    var vel = body2d.velocity;
        //    body2d.velocity = new Vector2(vel.x * dashForce, vel.y);
        //    dashing = true;
        //    dashTime -= Time.deltaTime;

        //}
        //else
        //{
        //    dashing = false;
        //    dashTime = timeToDash;
        //    // StartCoroutine(FinishedDashRoutine());
        //}
       
    }

    IEnumerator FinishedDashRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("finish dashing");
        dashing = false;

        ToggleScripts(true);

    }
}