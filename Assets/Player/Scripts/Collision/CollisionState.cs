﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class CollisionState : MonoBehaviour
    {
        public LayerMask collisionLayer;
        public bool forceGrounded;
        public bool forceFloating;
        public bool grounded;
        public bool onWall;
        public bool isTouchingLedge;
        public float slopeAngle;
        public Vector2 bottomPosition = Vector2.zero;
        public Vector2 rightPosition = Vector2.zero;
        public Vector2 leftPosition = Vector2.zero;
        public Vector2 ledgeRightPosition = Vector2.zero;
        public Vector2 ledgeLeftPosition = Vector2.zero;
        public Vector2 slopeLeft = Vector2.zero;
        public Vector2 slopeRight = Vector2.zero;

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
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            if (forceGrounded)
            {
                grounded = true;
                forceFloating = false;
            }else if (forceFloating)
            {
                forceGrounded = false;
                grounded = false;
            }
            else
            {
                grounded = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
            }

            pos = inputState.direction == Directions.Right ? rightPosition : leftPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            onWall = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);

            pos = inputState.direction == Directions.Right ? ledgeRightPosition : ledgeLeftPosition;
            pos.x += transform.position.x;
            pos.y += transform.position.y;

            isTouchingLedge = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);

            //RaycastHit2D hit = Physics2D.Raycast(transform.position,  Vector2.left, collisionLayer);
            //Debug.DrawLine(transform.position)
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = collisionColor;

            var positions = new Vector2[] { ledgeRightPosition, ledgeLeftPosition, rightPosition, leftPosition, bottomPosition };
            var rayPositions = new Vector2[] { slopeLeft, slopeRight };



            foreach (var position in positions)
            {
                var pos = position;
                pos.x += transform.position.x;
                pos.y += transform.position.y;

                Gizmos.DrawWireSphere(pos, collisionRadius);
            }

            foreach(var rayPosition in rayPositions)
            {

            }
        }
    }

}