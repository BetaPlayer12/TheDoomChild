using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class StepUp : PlayerBehaviour
    {
        private FaceDirection facing;
        public Vector2 ledgeBotPos;
        private Vector2 ledgePos1;
        private Vector2 ledgePos2;

        public float ledgeClimbXOffset1 = 0f;
        public float ledgeClimbYOffset1 = 0f;
        public float ledgeClimbXOffset2 = 0f;
        public float ledgeClimbYOffset2 = 0f;

        // Start is called before the first frame update
        void Start()
        {
            facing = GetComponent<FaceDirection>();
        }

        // Update is called once per frame
        void Update()
        {
            var left = inputState.GetButtonValue(inputButtons[0]);
            var right = inputState.GetButtonValue(inputButtons[1]);


            if(stateManager.onWallLeg && stateManager.isGrounded && !stateManager.onWall && !stateManager.isTouchingLedge )
            {
                ledgeBotPos = transform.position;
                if (left)
                {
                    float terminalPosX = Mathf.Floor(ledgeBotPos.x - stateManager.rightPosition.x) + ledgeClimbXOffset1;
                    float terminalPosY = Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1;
                    ledgePos2 = new Vector2(terminalPosX, terminalPosY);

                }
                else if (right)
                {
                    float terminalPosX = Mathf.Floor(ledgeBotPos.x + stateManager.rightPosition.x) - ledgeClimbXOffset1;
                    float terminalPosY = Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1;

                    ledgePos2 = new Vector2(terminalPosX, terminalPosY);
                }

                transform.position = ledgePos2;
                Debug.Log(ledgePos2);

            }
        }
    }

}

