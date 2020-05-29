using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class Levitate : PlayerBehaviour
    {
        public bool levitateMode;
        private float defGravityScale;
        private bool canLevitate;
        [SerializeField]
        private float playerGravity;
        [SerializeField]
        GameObject capeObject;
        // Start is called before the first frame update
        void Start()
        {
            canLevitate = true;
            levitateMode = false;
            defGravityScale = body2d.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            var levitate = Input.GetButtonDown("Levitate");
            var holdtime = inputState.GetButtonHoldTime(inputButtons[0]);


           
            if (levitate && !levitateMode)
            {

                if (!levitateMode)
                {
                    if (!collisionState.grounded && canLevitate && !collisionState.onWall)
                    {
                        ToggleScripts(false);
                        capeObject.active = false;
                        body2d.velocity = Vector2.zero;
                        levitateMode = true;
                        canLevitate = false;
                        body2d.gravityScale = 0f;
                    }
                }
                else
                {
                    ResetGravity();
                }


            }
            else if (levitate && levitateMode)
            {
                ResetGravity();
            }



            if (collisionState.grounded)
            {
                ResetGravity();
            }

            playerGravity = body2d.gravityScale;

        }

        private void ResetGravity()
        {
            body2d.gravityScale = defGravityScale;
            levitateMode = false;
            capeObject.active = true;
            canLevitate = true;
            ToggleScripts(true);

        }


    }


}

