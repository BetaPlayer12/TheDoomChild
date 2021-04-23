using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallJump : PlayerBehaviour
    {
        public Vector2 jumpVelocity = new Vector2(50, 200);
        public bool jumpingOffWall;
        public float resetDelay = 0.2f;

        private float timeElapsed = 0;

        private void FixedUpdate()
        {
            var canJump = inputState.GetButtonValue(inputButtons[0]);
            var holdJump = inputState.GetButtonHoldTime(inputButtons[0]);

            if (stateManager.onWall && !stateManager.isGrounded ) {
                
                if (canJump && holdJump < 0.1f)
                {
                    Debug.Log("wall jump");
                    inputState.direction = inputState.direction == Directions.Right ? Directions.Left : Directions.Right;
                    rigidBody.velocity = new Vector2(jumpVelocity.x * (float)inputState.direction, jumpVelocity.y);
                }
            }
        }

    }
}

