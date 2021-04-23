using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Jog : PlayerBehaviour
    {
        public float movementSpeed = 10f;
        public float speedMultiplier = 1.5f;
        public float jogTimer = 3.5f;
        public float crouchSpeedMultiplier = 0.5f;
        public bool jogging = false;

        private bool isEnabled = true;
        private CapsuleCollider2D collider2D;

        void Start()
        {
            collider2D = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void FixedUpdate()
        {
            if (isEnabled)
            {
                var right = inputState.GetButtonValue(inputButtons[0]);
                var left = inputState.GetButtonValue(inputButtons[1]);
                var down = inputState.GetButtonValue(inputButtons[2]);

                float xVelocity = movementSpeed * inputState.horizontal;

                if (stateManager.CanMove())
                {
                    if (inputState.horizontal != 0)
                    {
                        stateManager.isIdle = false;

                        jogTimer -= Time.deltaTime;

                        if (jogTimer < 0)
                        {
                            xVelocity *= speedMultiplier;
                        }

                        UpdateFacing();

                        //velX *= left ? -1 : 1;
                        jogging = true;

                        if (stateManager.isCrouching)
                        {
                            xVelocity /= crouchSpeedMultiplier;
                        }

                        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
                    }
                    else
                    {
                        if (stateManager.CheckIfIdle())
                        {
                            stateManager.isIdle = true;
                            xVelocity = 0;
                            jogging = false;
                            jogTimer = 3.5f;
                        }
                    }
                }
            }
        }

        private void UpdateFacing()
        {

        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            rigidBody.velocity = Vector2.zero;
            isEnabled = false;
        }
    }
}

