using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallSlide : WallStick
    {
        [SerializeField]
        private float m_rayDistance;
        [SerializeField]
        private LayerMask m_groundLayer;
        [SerializeField]
        private float m_XWallJumpForce;
        [SerializeField]
        private float m_YWallJumpForce;
        [SerializeField]
        private float m_wallJumpInputDelay;

        private FaceDirection facing;
        private LongJump jumping;
        //private List<Collider2D> m_colliderList;

        public float slideVelocity = -5f;
        public float slideMultiplier = 5f;
        public float velocityX;
        public float forceX;
        public float forceY;
        public bool upHold;
        public bool downHold;
        public bool extraJump = false;

        private void Start()
        {
            facing = GetComponent<FaceDirection>();
            jumping = GetComponent<LongJump>();
            //m_colliderList = new List<Collider2D>();
        }

        override protected void Update()
        {
            base.Update();
            var wallStickDown = inputState.GetButtonValue(inputButtons[0]);
            var wallStickLeft = inputState.GetButtonValue(inputButtons[1]);
            var wallStickRight = inputState.GetButtonValue(inputButtons[2]);
            var wallStickJump = inputState.GetButtonValue(inputButtons[3]);
            var wallStickJumpHold = inputState.GetButtonHoldTime(inputButtons[3]);
            var wallStickUp = inputState.GetButtonValue(inputButtons[4]);

            if (wallSticking)
            {
                upHold = wallStickUp;
                downHold = wallStickDown;
            }

            velocityX = rigidBody.velocity.x;

            if (!stateManager.isGrounded && !onWallDetected && !wallSticking)
            {
                rigidBody.sharedMaterial.friction = 0;
                capsuleCollider.enabled = false;
                capsuleCollider.enabled = true;
            }
            else
            {
                rigidBody.sharedMaterial.friction = 0.4f;
                capsuleCollider.enabled = false;
                capsuleCollider.enabled = true;
            }

            //Set jumpForce if onWall and isGrounded
            if (onWallDetected && wallGrounded || groundWallStick && stateManager.onWallLeg && stateManager.isGrounded)
            {
                forceX = 10;
                forceY = 50;
            }
            if (onWallDetected && !wallGrounded && !stateManager.isGrounded)
            {
                forceX = 250;
                forceY = 250;
            }

            //Wall Slide
            if (onWallDetected && !stateManager.isGrounded)
            {
                var velY = slideVelocity;

                CheckForOneWayPlatform();

                if (wallStickDown)
                {
                    stateManager.ToggleGroundChecker(true);
                    velY *= slideMultiplier;
                    rigidBody.gravityScale = 0;
                    //rigidBody.drag = 20;
                }
                else if (wallStickUp)
                {
                    stateManager.ToggleGroundChecker(false);
                    velY *= -slideMultiplier;
                    rigidBody.gravityScale = 0;
                    //rigidBody.drag = 20;
                }
                else
                {
                    rigidBody.gravityScale = defaultGravityScale;
                    rigidBody.drag = defaultDrag;
                    stateManager.ToggleGroundChecker(true);
                }

                if (wallStickLeft || wallStickRight)
                {
                    extraJump = true;
                }

                rigidBody.velocity = new Vector2(rigidBody.velocity.x, velY);
            }
            else
            {

            }

            //Wall Jump
            if ((onWallDetected || groundWallStick) && wallStickJump && wallStickJumpHold < 0.1f && !upHold)
            {
                //jumping.jumpsRemaining += 1;
                playerMovement.DisableMovement();
                rigidBody.gravityScale = defaultGravityScale;
                rigidBody.drag = defaultDrag;
                wallSticking = false;

                if (!facing.isFacingRight) //facing left
                {
                    Debug.Log("Wall jump Right");
                    rigidBody.velocity = new Vector2(m_XWallJumpForce, m_YWallJumpForce);
                }
                else //facing right
                {
                    Debug.Log("Wall jump Left");
                    rigidBody.velocity = new Vector2(m_XWallJumpForce * -1f, m_YWallJumpForce);
                }

                StartCoroutine(WallJumpRoutine());
            }
        }

        private IEnumerator WallJumpRoutine()
        {
            yield return new WaitForSeconds(m_wallJumpInputDelay);

            playerMovement.EnableMovement();
        }

        override protected void Onstick()
        {
            /*      base.Onstick();
                    body2d.velocity = Vector2.zero;*/
        }

        protected override void Offwall()
        {
            base.Offwall();
        }

        private void CheckForOneWayPlatform()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, m_rayDistance, m_groundLayer);

            if (hit.collider)
            {
                if (hit.collider.gameObject.tag == "Droppable")
                {
                    m_colliderList.Add(hit.collider);
                    Physics2D.IgnoreCollision(capsuleCollider, hit.collider, true);
                }
            }
            else
            {
                ResetColliders();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + m_rayDistance));
        }
    }
}
