using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Whip : PlayerBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var canWhip = inputState.GetButtonValue(inputButtons[0]);

            if (canWhip)
            {
                Debug.Log("whip attack");
            }
        }
    }

}

