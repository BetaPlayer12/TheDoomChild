using DChild.Gameplay;
using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class StateManager : MonoBehaviour
    {
        [Header("Status Flags")]
        public bool isIdle;
        public bool isAttacking;
        public bool isInCombatMode;
        public bool isJumping;
        public bool isDashing;
        public bool isCrouching;
        public bool isFlinching;

        public LayerMask collisionLayer;
        public bool disableGround;
        public bool forceGrounded;
        public bool isGrounded;
        public bool onWall;
        public bool onWallLeg;
        public bool isTouchingLedge;
        public bool isCeilingTouch;
        public float slopeAngle;
        public bool isDead;

        public Vector2 bottomPosition = Vector2.zero;
        public Vector2 rightPosition = Vector2.zero;
        public Vector2 leftPosition = Vector2.zero;
        public Vector2 ledgeRightPosition = Vector2.zero;
        public Vector2 ledgeLeftPosition = Vector2.zero;
        public Vector2 leftLegPosition = Vector2.zero;
        public Vector2 rightLegPosition = Vector2.zero;
        public Vector2 slopeLeft = Vector2.zero;
        public Vector2 slopeRight = Vector2.zero;

        public RaycastHit2D slopeLeftHit;
        public RaycastHit2D slopeRightHit;
        public RaycastHit2D slopeBotHit;
        public RaycastHit2D ledgeBotHit;

        public float collisionRadius = 10.0f;
        public Color collisionColor = Color.red;
        public float lineLength;
        public float posDir;

        //Ledge Grab
        public float yPos;
        public float xPos;
        public float xPosLeft;
        public float xPosRight;
        public bool grabLedge;
        public Vector2 newPos;

        private InputState inputState;
        private ContactFilter2D filter;
        private List<Collider2D> colliderList;
        private PlayerMovement playerMovement;
        private bool isGroundCheckerEnabled = true;
        private Character m_character;

        void Awake()
        {
            inputState = GetComponent<InputState>();
            playerMovement = GetComponent<PlayerMovement>();
            m_character = GetComponentInParent<Character>();

            filter = new ContactFilter2D();
            colliderList = new List<Collider2D>();

            filter.useTriggers = false;
            filter.useLayerMask = true;
            filter.layerMask = collisionLayer;
        }

        private void FixedUpdate()
        {
            var pos = bottomPosition;
            Vector2 lineDir;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            if (disableGround)
            {
                isGrounded = false;
            }
            else if (forceGrounded)
            {
                isGrounded = true;
            }
            else
            {
                //Check if player is grounded
                if(isGroundCheckerEnabled == true)
                {
                    int groundColliderResult = Physics2D.OverlapCircle(pos, collisionRadius, filter, colliderList);
                    isGrounded = groundColliderResult > 0 ? true : false;
                }
                else
                {
                    isGrounded = false;
                }
            }

            pos = inputState.direction == Directions.Right ? rightPosition : leftPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            lineDir = m_character.facing == HorizontalDirection.Right ? Vector2.right : Vector2.left;
            //onWall = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
            var wall = Physics2D.Raycast(pos, lineDir, lineLength, collisionLayer);

            if (wall.collider)
            {
                onWall = wall.collider.CompareTag("InvisibleWall") == false;
            }
            else
            {
                onWall = false;
            }

            Debug.DrawRay(pos, lineDir * lineLength, Color.cyan);

            pos = inputState.direction == Directions.Right ? ledgeRightPosition : ledgeLeftPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            lineDir = inputState.direction == Directions.Right ? Vector2.right : Vector2.left;
            //isTouchingLedge = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
            isTouchingLedge = Physics2D.Raycast(pos, lineDir, lineLength, collisionLayer);

            pos = inputState.direction == Directions.Left ? rightLegPosition : leftLegPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            //onWallLeg = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
            int onWallLegResult = Physics2D.OverlapCircle(pos, collisionRadius, filter, colliderList);
            onWallLeg = onWallLegResult > 0 ? true : false;

            slopeLeftHit = Physics2D.Raycast(transform.position, Vector2.left, lineLength, collisionLayer);
            slopeRightHit = Physics2D.Raycast(transform.position, Vector2.right, lineLength, collisionLayer);
            slopeBotHit = Physics2D.Raycast(transform.position, Vector2.down, lineLength, collisionLayer);
            isCeilingTouch = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 3), Vector2.up, 4, collisionLayer);

            posDir = inputState.direction == Directions.Left ? 1 : -1;
            ledgeBotHit = Physics2D.Raycast(new Vector2(transform.position.x + (1.5f * -posDir), transform.position.y), Vector2.down, lineLength, collisionLayer);

            xPos = inputState.direction == Directions.Left ? xPosLeft : xPosRight;

            var leftledgeHit = Physics2D.Raycast(new Vector2(transform.position.x + xPos * -posDir, transform.position.y + yPos), Vector2.left, 4, collisionLayer);
            //Debug.DrawRay(new Vector2(transform.position.x + xPos * -posDir, transform.position.y + yPos), lineDir * lineLength, Color.red);

            if (leftledgeHit.transform != null)
            {
                //Debug.Log(leftledgeHit.transform.tag);
                if (leftledgeHit.transform.tag == "LedgeCollider")
                {
                    grabLedge = true;
                    //GrabLedge
                    newPos = leftledgeHit.transform.gameObject.GetComponent<DebugTest>().pos;
                }
                else
                {
                    //Debug.Log("nothing detected");
                    grabLedge = false;
                }
            }
            else
            {
                //Debug.Log("nothing detected");
                grabLedge = false;
            }
        }

        public bool CheckIfIdle()
        {
            if (!isAttacking && !isJumping && !isDashing)
            {
                return true;
            }

            return false;
        }

        public bool CanMove()
        {
            if (!isAttacking && !isJumping)
            {
                return true;
            }

            return false;
        }

        public void ToggleGroundChecker(bool value)
        {
            isGroundCheckerEnabled = value;
        }

        public void DisableMovement()
        {

        }

        public void SetupFlinch()
        {
            playerMovement.EnableMovement();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = collisionColor;

            var positions = new Vector2[] { bottomPosition, leftLegPosition, rightLegPosition };
            //var rayPositions = new Vector2[] { ledgeRightPosition, ledgeLeftPosition, rightPosition, leftPosition };

            foreach (var position in positions)
            {
                var pos = position;
                pos.x += transform.position.x;
                pos.y += transform.position.y;

                Gizmos.DrawWireSphere(pos, collisionRadius);
            }
        }
    }
}