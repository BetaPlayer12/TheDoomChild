using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallGrab : PlayerBehaviour
    {
        [SerializeField]
        private FaceDirection facing;
        private Renderer spriteRenderer;
        

        public bool canLedgeGrab = false;
        public bool ledgeDetected;

        private bool climb = false;

        private float defaultGravityScale;
        private float defaultDrag;

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
            //spriteRenderer = GetComponent<Renderer>();
            
        }

        // Update is called once per frame
        void Update()
        {
            
            if (!stateManager.isGrounded && !stateManager.isTouchingLedge && stateManager.onWall && stateManager.onWallLeg && !ledgeDetected)
            {
                ToggleScripts(false);
                ledgeDetected = true;
                ledgeBotPos = transform.position;
            }

            //wallGrab facing right
            if (stateManager.onWall && facing.isFacingRight && !stateManager.isGrounded)
            {
                if (ledgeDetected && stateManager.onWall && (inputState.GetButtonValue(inputButtons[0]) || inputState.GetButtonValue(inputButtons[1])))
                {
                    OnWallGrab();
                }
                else if (ledgeDetected && stateManager.onWall && (inputState.GetButtonValue(inputButtons[2]) || inputState.GetButtonValue(inputButtons[3]) || inputState.GetButtonValue(inputButtons[4])))
                {
                    ToggleScripts(true);
                    ledgeDetected = false;
                }
            }
            //wallGrab facing left
            else if (stateManager.onWall && !facing.isFacingRight && !stateManager.isGrounded)
            {
                if (ledgeDetected && stateManager.onWall && (inputState.GetButtonValue(inputButtons[0]) || inputState.GetButtonValue(inputButtons[3])))
                {
                    OnWallGrab();
                }
                else if (ledgeDetected && stateManager.onWall && (inputState.GetButtonValue(inputButtons[1]) || inputState.GetButtonValue(inputButtons[2]) || inputState.GetButtonValue(inputButtons[4])))
                {
                    ToggleScripts(true);
                    ledgeDetected = false;
                }
            }




        }

        private void OnWallGrab()
        {
            
            if (ledgeDetected && !canLedgeGrab)
            {
                canLedgeGrab = true;
                ledgePos1 = transform.position;

                if (facing.isFacingRight)
                {

                    float terminalPosX = Mathf.Floor(ledgeBotPos.x + stateManager.rightPosition.x) - ledgeClimbXOffset1;
                    float terminalPosY = Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1;
                    
                    ledgePos2 = new Vector2(terminalPosX, terminalPosY);

                }
                else
                {
                    float terminalPosX = Mathf.Floor(ledgeBotPos.x - stateManager.rightPosition.x) + ledgeClimbXOffset1;
                    float terminalPosY = Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1;
                    ledgePos2 = new Vector2(terminalPosX, terminalPosY);

                }
                canLedgeGrab = true;
                

            }
        }

        IEnumerator FinishedLedgeClimbRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            FinishedLedgeClimb();

        }
        private void FinishedLedgeClimb()
        {
            ledgeDetected = false;
            canLedgeGrab = false;
            ToggleScripts(true);
            climb = false;
        }

        private void StartLedgeClimb()
        {
            transform.position = ledgePos2;
            climb = true;
        }
    }


}


