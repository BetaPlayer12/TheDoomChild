using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class CollisionState : MonoBehaviour
    {
        public LayerMask collisionLayer;
        public bool disableGround;
        public bool forceGrounded;
        public bool grounded;
        public bool onWall;
        public bool onWallLeg;
        public bool isTouchingLedge;
        public float slopeAngle;
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

        public float collisionRadius = 10.0f;
        public Color collisionColor = Color.red;
        public float lineLength; 

        private InputState inputState;
        // Start is called before the first frame update
        void Awake()
        {
            inputState = GetComponent<InputState>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            var pos = bottomPosition;
            Vector2 lineDir;
            pos.x += transform.position.x;
            pos.y += transform.position.y;


            if (disableGround)
            {
                grounded = false;
            }else if (forceGrounded)
            {
                grounded = true;
            }else
            {
                grounded = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
               
            }

           

            pos = inputState.direction == Directions.Right ? rightPosition : leftPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            lineDir = inputState.direction == Directions.Right ? Vector2.right : Vector2.left;
            //onWall = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
            onWall = Physics2D.Raycast(pos, lineDir, lineLength, collisionLayer);

            pos = inputState.direction == Directions.Right ? ledgeRightPosition : ledgeLeftPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            lineDir = inputState.direction == Directions.Right  ? Vector2.right : Vector2.left;
            //isTouchingLedge = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
            isTouchingLedge = Physics2D.Raycast(pos, lineDir, lineLength, collisionLayer);


            pos = inputState.direction == Directions.Left ? rightLegPosition : leftLegPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;
            onWallLeg = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);

            slopeLeftHit = Physics2D.Raycast(transform.position,  Vector2.left, lineLength, collisionLayer);
            slopeRightHit = Physics2D.Raycast(transform.position, Vector2.right, lineLength, collisionLayer);
            slopeBotHit = Physics2D.Raycast(transform.position, Vector2.down, lineLength, collisionLayer);


        }

        private void OnDrawGizmos()
        {
            Gizmos.color = collisionColor;

            var positions = new Vector2[] {bottomPosition, leftLegPosition, rightLegPosition };
            var rayPositions = new Vector2[] { ledgeRightPosition, ledgeLeftPosition, rightPosition, leftPosition };




            foreach (var position in positions)
            {
                var pos = position;
                pos.x += transform.position.x;
                pos.y += transform.position.y;

                Gizmos.DrawWireSphere(pos, collisionRadius);
            }

            //foreach (var rayPosition in rayPositions)
            //{
            //    var pos = rayPosition;

            //    var lineDir = inputState.direction == Directions.Right ? Vector2.right : Vector2.left;
            //    pos.x += transform.position.x;
            //    pos.y += transform.position.y;
            //    Gizmos.DrawRay(pos, lineDir * lineLength);
            //}

            Debug.DrawRay(transform.position, Vector2.right * lineLength, Color.green);
            Debug.DrawRay(transform.position, Vector2.left * lineLength, Color.green);
        }
    }

}