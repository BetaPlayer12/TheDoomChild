using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Interactable : PlayerBehaviour
    {
        

        // Update is called once per frame
        void Update()
        {
            var up = inputState.GetButtonValue(inputButtons[0]);

            if (collisionState.isTouchingInteractable && up)
            {
                Debug.Log("Do some action starts here");
            }
        }
    }
}


