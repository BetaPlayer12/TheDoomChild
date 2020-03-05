using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class StepUp : PlayerBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var left = inputState.GetButtonValue(inputButtons[0]);
            var right = inputState.GetButtonValue(inputButtons[1]);


            if(collisionState.onWallLeg && collisionState.grounded && !collisionState.onWall && !collisionState.isTouchingLedge && (left || right))
            {
                Debug.Log("Step up");
            }
        }
    }

}

