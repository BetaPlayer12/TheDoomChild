using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class Levitate : PlayerBehaviour
    {
        [SerializeField]
        private float playerGravity;
        [SerializeField]
        private float m_levitateVelocityValue = 20f;
        [SerializeField]
        GameObject capeObject;

        private float defaultGravityScale;
        private bool canLevitate;

        public bool levitateMode;

        void Start()
        {
            canLevitate = true;
            levitateMode = false;
            defaultGravityScale = rigidBody.gravityScale;
        }

        void Update()
        {
            if (inputState.levitatePressed && !levitateMode)
            {
                Debug.Log(rigidBody.velocity.y);

                if (!levitateMode)
                {
                    if (inputState.levitateHeld && (rigidBody.velocity.y >= m_levitateVelocityValue || rigidBody.velocity.y <= -m_levitateVelocityValue))
                    {
                        if (!stateManager.isGrounded && canLevitate && !stateManager.onWall)
                        {
                            ToggleScripts(false);

                            capeObject.active = false;
                            levitateMode = true;
                            canLevitate = false;
                            rigidBody.velocity = Vector2.zero;
                            rigidBody.gravityScale = 0f;
                        }
                    }
                }
                else
                {
                    ResetGravity();
                }
            }
            else if (!inputState.levitateHeld && levitateMode)
            {
                ResetGravity();
            }

            if (stateManager.isGrounded)
            {
                ResetGravity();
            }

            playerGravity = rigidBody.gravityScale;
        }

        private void ResetGravity()
        {
            //rigidBody.gravityScale = defaultGravityScale;
            levitateMode = false;
            capeObject.active = true;
            canLevitate = true;

            ToggleScripts(true);
        }
    }
}

