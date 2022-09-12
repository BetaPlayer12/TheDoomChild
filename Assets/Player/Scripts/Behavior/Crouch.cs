using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Crouch : PlayerBehaviour
    {
        [SerializeField]
        private GroundShaker groundsmash;
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
            if (canCrouch && stateManager.isGrounded && !crouching)
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