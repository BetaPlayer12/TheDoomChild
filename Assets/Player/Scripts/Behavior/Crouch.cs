using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Crouch : PlayerBehaviour
    {

        public bool crouching;


        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void OnCrouch(bool value)
        {
            crouching = value;
            ToggleScripts(!crouching);
        }

        void Update()
        {
            var canCrouch = inputState.GetButtonValue(inputButtons[0]);
            if (canCrouch && collisionState.grounded && !crouching)
            {
                OnCrouch(true);
            }
            else if (crouching && !canCrouch)
            {
                OnCrouch(false);
            }            
        }
    }

}