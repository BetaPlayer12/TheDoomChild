using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class Idle : PlayerBehaviour
    {
       
        void Update()
        {
            if (collisionState.grounded) { }
        }
    }

}