using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : PlayerBehaviour
{

    public Vector2 jumpVelocity = new Vector2(50, 200);
    public bool jumpingOffWall;
    public float resetDelay = 0.2f;
    public float jumpHeight;
    public float jumpDistance;
    public float jumpTime;
    public float holdJumpDelay = 0.15f;
    public bool hasWallJump = false;
    public float velToZero = .5f;

    private FaceDirection facingRight;
    public float timeElapsed;

    private void Start()
    {
        facingRight = GetComponent<FaceDirection>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        timeElapsed = timeElapsed % jumpTime;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        var jumping = inputState.GetButtonValue(inputButtons[0]);
        var right = inputState.GetButtonValue(inputButtons[1]);
        var left = inputState.GetButtonValue(inputButtons[2]);

        



        if (collisionState.onWall && !collisionState.grounded)
        {
            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdJump = inputState.GetButtonHoldTime(inputButtons[0]);
            if (canJump && !jumpingOffWall && holdJump < holdJumpDelay)
            {
                inputState.direction = inputState.direction == Directions.Right ? Directions.Left : Directions.Right;

                //float airControlAccelerationLimit = 0.5f;  // Higher = more responsive air control
                //float airSpeedModifier = 0.7f; // the 0.7f in your code, affects max air speed
                //float targetHorizVelocity = (float)inputState.direction * jumpVelocity.x * airSpeedModifier;  // How fast we are trying to move horizontally
                //float targetHorizChange = targetHorizVelocity  - body2d.velocity.x; // How much we want to change the horizontal velocity
                //float horizChange = Mathf.Clamp(  targetHorizChange, -airControlAccelerationLimit,  airControlAccelerationLimit); // How much we are limiting ourselves 
                //body2d.velocity = new Vector2(body2d.velocity.x + horizChange, body2d.velocity.y);

                body2d.velocity = new Vector2(jumpVelocity.x * (float)inputState.direction, jumpVelocity.y);
                //transform.position = MathParabola.Parabola()
                body2d.drag = 0.0f;
                body2d.gravityScale = 20;
                //body2d.AddForce(new Vector2(jumpVelocity.x * (float)inputState.direction, jumpVelocity.y), ForceMode2D.Impulse);
                //transform.position = MathParabola.Parabola(Vector2.zero, new Vector2(transform.position.x , transform.position.y), 0, timeElapsed/5f);
                ToggleScripts(false);
                jumpingOffWall = true;
                hasWallJump = true;
            }
        }

        //if (hasWallJump && body2d.velocity.x == 0)
        //{
        //    Debug.Log("Finish jump" + body2d.velocity);

        //    velToZero -= Time.deltaTime;
        //    if(velToZero != 0)
        //    {
        //        body2d.velocity = new Vector2(velToZero * (float)inputState.direction, - velToZero);
        //    }
        //    else
        //    {
        //        hasWallJump = false;
        //    }
            

        //}

      

        if (jumpingOffWall)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > resetDelay)
            {
                ToggleScripts(true);
                jumpingOffWall = false;
                timeElapsed = 0;
            }
        }
    }
}
