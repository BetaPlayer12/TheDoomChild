using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallGrab : PlayerBehaviour
    {

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
            spriteRenderer = GetComponent<Renderer>();
            
        }

        // Update is called once per frame
        void Update()
        {

            //if (collisionState.onWall && !collisionState.grounded && !ledgeDetected)
            //{
            //    ledgeDetected = true;
            //    ledgeBotPos = trans.position;
            //    ToggleScripts(false);
            //    CheckLedgeClimb();

            //    //call animation
            //}

            if (!collisionState.grounded && !collisionState.isTouchingLedge && collisionState.onWall && collisionState.onWallLeg && !ledgeDetected)
            {
                ToggleScripts(false);
                ledgeDetected = true;
                ledgeBotPos = transform.position;
                OnWallGrab();
            }

        }

        private void OnWallGrab()
        {
            
           if(ledgeDetected && !canLedgeGrab)
            {
                 canLedgeGrab = true;
                //if (!canLedgeGrab)
                //{

               // ledgeBotPos = trans.position;



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
                //capsuleCollider.enabled = false;

                canLedgeGrab = true;
               // spriteRenderer.enabled = false;
               




                // }

                //if (canLedgeGrab)
                //{
                //    body2d.gravityScale = 0;
                //    body2d.drag = 100;
                //    // StartCoroutine(FinishedLedgeClimbRoutine());
                //    //ToggleScripts(false);
                //}

                //FinishedLedgeClimb();
            }

        }

        IEnumerator FinishedLedgeClimbRoutine()
        {
            Debug.Log("yeild");
            yield return new WaitForSeconds(0.1f);
            // capsuleCollider.enabled = true;
            
        }
        private void FinishedLedgeClimb()
        {
            //StartCoroutine(FinishedLedgeClimbRoutine());
            ledgeDetected = false;
            canLedgeGrab = false;
            ToggleScripts(true);
        }

        private void StartLedgeClimb()
        {
            //Debug.Log("On climb");
            //  spriteRenderer.enabled = true;
            transform.position = ledgePos1;
        }
    }


}


