using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallGrab : PlayerBehaviour
    {
        [SerializeField]
        private Character m_character;
        private FaceDirection facing;
        private Renderer spriteRenderer;
        

        public bool canLedgeGrab = false;
        public bool ledgeDetected;

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

/*
            if (!collisionState.grounded && !collisionState.isTouchingLedge && collisionState.onWall && collisionState.onWallLeg && !ledgeDetected)
            {
                ToggleScripts(false);
                ledgeDetected = true;
                ledgeBotPos = transform.position;
               
            }

            //wallGrab facing right
            if(collisionState.onWall && facing.isFacingRight)
            {
                if(inputState.GetButtonValue[0] || inputState.GetButtonValue(inputButtons[1]))
                {
                    OnWallGrab();
                }
                else if(collisionState.onWall && (inputState.GetButtonValue[2] || inputState.GetButtonValue[3] || inputState.GetButtonValue[4]))
                {
                    ToggleScripts(true);
                }
            }
            //wallGrab facing left
            else if(collisionState.onWall && !facing.isFacingRight)
            {
                if (inputState.GetButtonValue[0] || inputState.GetButtonValue(inputButtons[3]))
                {
                    OnWallGrab();
                }
                else if (collisionState.onWall && (inputState.GetButtonValue[1] || inputState.GetButtonValue[2] || inputState.GetButtonValue[4]))
                {
                    ToggleScripts(true);
                }       
            }*/


        }

        private void OnWallGrab()
        {
            
           if(ledgeDetected && !canLedgeGrab)
            {
                 canLedgeGrab = true;

                if (facing.isFacingRight)
                {
                    ledgePos1 = new Vector2(Mathf.Floor(ledgeBotPos.x + collisionState.rightPosition.x) - ledgeClimbXOffset1, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(Mathf.Floor(ledgeBotPos.x + collisionState.rightPosition.x) + ledgeClimbXOffset2, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset2);
                    // Debug.Log("1");
                }
                else
                {
                    ledgePos1 = new Vector2(Mathf.Floor(ledgeBotPos.x - collisionState.rightPosition.x) + ledgeClimbXOffset1, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(Mathf.Floor(ledgeBotPos.x - collisionState.rightPosition.x) - ledgeClimbXOffset2, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset2);
                    //  Debug.Log("2");
                }


                canLedgeGrab = true;

            }

        }

        IEnumerator FinishedLedgeClimbRoutine()
        {
            Debug.Log("yeild");
            yield return new WaitForSeconds(0.1f);

            
        }
        private void FinishedLedgeClimb()
        {

            ledgeDetected = false;
            canLedgeGrab = false;
            ToggleScripts(true);
        }

        private void StartLedgeClimb()
        {

            transform.position = ledgePos1;
        }
    }


}


