using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class LedgeGrab : PlayerBehaviour

    {
        [SerializeField]
        private Character m_character;
        private FaceDirection facing;

        public bool canLedgeGrab = false;
        public bool ledgeDetected;

        private Vector2 ledgeBotPos;
        private Vector2 ledgePos1;
        private Vector2 ledgePos2;

        public float ledgeClimbXOffset1;
        public float ledgeClimbYOffset1;
        public float ledgeClimbXOffset2;
        public float ledgeClimbYOffset2;


        // Start is called before the first frame update
        void Start()
        {
            facing = GetComponent<FaceDirection>();
        }

        // Update is called once per frame
        void Update()
        {
            if (stateManager.onWall && !stateManager.isGrounded && !ledgeDetected)
            {
                ToggleScripts(false);
                ledgeDetected = true;
                ledgeBotPos = trans.position;
                CheckLedgeClimb();

                //call animation
            }


        }

        private void CheckLedgeClimb()
        {
            if (facing.isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgeBotPos.x + stateManager.rightPosition.x) - ledgeClimbXOffset1, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgeBotPos.x + stateManager.rightPosition.x) + ledgeClimbXOffset2, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgeBotPos.x - stateManager.rightPosition.x) + ledgeClimbXOffset1, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgeBotPos.x - stateManager.rightPosition.x) - ledgeClimbXOffset2, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset2);
            }

            FinishedLedgeClimb();
        }

        private void FinishedLedgeClimb()
        {
            m_character.transform.position = ledgePos2;
            ledgeDetected = false;
            ToggleScripts(false);
        }
    }

}